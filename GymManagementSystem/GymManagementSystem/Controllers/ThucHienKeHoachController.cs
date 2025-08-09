using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;
using GymManagementSystem.Models;
using GymManagementSystem.Models.ViewModels;
using Microsoft.AspNet.Identity;
using GymManagementSystem.Attributes;

[Authorize(Roles = "HoiVien")]
public class ThucHienKeHoachController : Controller
{
    private ApplicationDbContext db = new ApplicationDbContext();


    public async Task<ActionResult> XemKeHoach(int dangKyKeHoachId)
    {
        var userId = User.Identity.GetUserId();
        var dangKy = await db.DangKyKeHoachs
                             .Include(d => d.KeHoach)
                             .FirstOrDefaultAsync(d => d.Id == dangKyKeHoachId && d.HoiVienId == userId);

        if (dangKy == null) return HttpNotFound();

        // 1. Lấy tất cả các bài tập của kế hoạch này
        var tatCaBaiTap = await db.ChiTietKeHoachs
                                   .Include(ct => ct.BaiTap)
                                   .Where(ct => ct.KeHoachId == dangKy.KeHoachId)
                                   .OrderBy(ct => ct.NgayTrongKeHoach)
                                   .ToListAsync();

        // 2. Lấy danh sách các bài tập mà người dùng đã hoàn thành
        var baiTapDaHoanThanhIds = await db.TienDoBaiTaps
                                           .Where(td => td.DangKyKeHoachId == dangKyKeHoachId)
                                           .Select(td => td.BaiTapId)
                                           .Distinct()
                                           .ToListAsync();

        // 3. Xây dựng danh sách ngày tập luyện cho ViewModel
        var danhSachNgayTap = new List<NgayTapLuyenItem>();
        int ngayHienTaiCuaKeHoach = (DateTime.Today - dangKy.NgayBatDau.Date).Days + 1;

        foreach (var baiTap in tatCaBaiTap)
        {
            bool daHoanThanh = baiTapDaHoanThanhIds.Contains(baiTap.BaiTapId);

            // Logic cho phép tập: Người dùng có thể tập bài của hôm nay hoặc các ngày trước đó chưa hoàn thành
            bool coTheTap = !daHoanThanh && (baiTap.NgayTrongKeHoach <= ngayHienTaiCuaKeHoach);

            danhSachNgayTap.Add(new NgayTapLuyenItem
            {
                NgayTrongKeHoach = baiTap.NgayTrongKeHoach,
                ChiTietBaiTap = baiTap,
                DaHoanThanh = daHoanThanh,
                CoTheTap = coTheTap
            });
        }

        // 4. Tính toán % hoàn thành
        int phanTramHoanThanh = 0;
        if (tatCaBaiTap.Any())
        {
            phanTramHoanThanh = (baiTapDaHoanThanhIds.Count * 100) / tatCaBaiTap.Count;
        }

        // 5. Tạo ViewModel chính
        var viewModel = new ChiTietKeHoachViewModel
        {
            DangKyKeHoach = dangKy,
            DanhSachNgayTap = danhSachNgayTap,
            PhanTramHoanThanh = phanTramHoanThanh
        };

        return View(viewModel);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<JsonResult> HoanThanhBaiTap(int dangKyKeHoachId, int baiTapId)
    {
        try
        {
            // --- TOÀN BỘ LOGIC CŨ CỦA BẠN NẰM TRONG KHỐI TRY ---
            var userId = User.Identity.GetUserId();
            var dangKy = await db.DangKyKeHoachs.FirstOrDefaultAsync(d => d.Id == dangKyKeHoachId && d.HoiVienId == userId);

            if (dangKy == null)
            {
                return Json(new { success = false, message = "Không tìm thấy kế hoạch đăng ký." });
            }

            // 1. Ghi nhận tiến độ vào CSDL
            db.TienDoBaiTaps.Add(new TienDoBaiTap
            {
                DangKyKeHoachId = dangKyKeHoachId,
                BaiTapId = baiTapId,
                NgayHoanThanh = DateTime.Now
            });
            await db.SaveChangesAsync();

            // 2. Kiểm tra xem đã hoàn thành toàn bộ kế hoạch chưa
            int soBaiTapDaHoanThanh = await db.TienDoBaiTaps
                                              .Where(td => td.DangKyKeHoachId == dangKyKeHoachId)
                                              .Select(td => td.BaiTapId)
                                              .Distinct()
                                              .CountAsync();

            int tongSoBaiTapCuaKeHoach = await db.ChiTietKeHoachs
                                                 .CountAsync(ct => ct.KeHoachId == dangKy.KeHoachId);

            if (soBaiTapDaHoanThanh >= tongSoBaiTapCuaKeHoach)
            {
                // HOÀN THÀNH KẾ HOẠCH!
                dangKy.TrangThai = "Đã hoàn thành";
                dangKy.NgayHoanThanh = DateTime.Now;
                db.Entry(dangKy).State = EntityState.Modified;

                var keHoach = await db.KeHoachs.Include(k => k.KhuyenMai).FirstOrDefaultAsync(k => k.Id == dangKy.KeHoachId);
                var hoivienProfile = await db.HoiViens.FirstOrDefaultAsync(h => h.ApplicationUserId == userId);

                if (keHoach != null && keHoach.KhuyenMaiId.HasValue && hoivienProfile != null)
                {
                    var voucher = new KhuyenMaiCuaHoiVien
                    {
                        HoiVienId = hoivienProfile.Id,
                        KhuyenMaiId = keHoach.KhuyenMaiId.Value,
                        NgayNhan = DateTime.Now,
                        NgayHetHan = DateTime.Now.AddDays(keHoach.KhuyenMai.SoNgayHieuLuc),
                        TrangThai = TrangThaiKhuyenMaiHV.ChuaSuDung
                    };
                    db.KhuyenMaiCuaHoiViens.Add(voucher);
                }
                await db.SaveChangesAsync();

                return Json(new { success = true, message = "Chúc mừng bạn đã hoàn thành kế hoạch! Một phần thưởng đang chờ bạn." });
            }

            return Json(new { success = true, message = "Hoàn thành bài tập hôm nay!" });
        }
        catch (Exception ex)
        {
            // --- KHỐI CATCH SẼ BẮT LẠI MỌI LỖI XẢY RA ---

            // Ghi log lỗi ra hệ thống (quan trọng cho việc debug sau này)
            // Khi debug, lỗi sẽ hiện trong cửa sổ Output của Visual Studio
            System.Diagnostics.Debug.WriteLine("LỖI TRONG HoanThanhBaiTap: " + ex.ToString());

            // Set mã lỗi HTTP 500 (Internal Server Error) để JavaScript có thể nhận biết
            Response.StatusCode = 500;

            // Trả về một thông báo lỗi dạng JSON thân thiện với người dùng
            return Json(new { success = false, message = "Đã có lỗi không mong muốn xảy ra phía máy chủ. Vui lòng thử lại." });
        }
    }

    public async Task<ActionResult> BaiTapChiTiet(int chiTietKeHoachId, int dangKyKeHoachId)
    {
        var baiTapChiTiet = await db.ChiTietKeHoachs
                                    .Include(ct => ct.BaiTap)
                                    .FirstOrDefaultAsync(ct => ct.Id == chiTietKeHoachId);

        if (baiTapChiTiet == null) return HttpNotFound();

        ViewBag.DangKyKeHoachId = dangKyKeHoachId;
        return View(baiTapChiTiet); // Đây sẽ là View chứa camera và logic tracking
    }
}