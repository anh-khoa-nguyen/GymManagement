using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;
using GymManagementSystem.Models;
using GymManagementSystem.Models.ViewModels;
using Microsoft.AspNet.Identity;

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

            // 3. Tự động tặng khuyến mãi
            var keHoach = await db.KeHoachs.FindAsync(dangKy.KeHoachId);
            if (keHoach.KhuyenMaiId.HasValue)
            {
                // TODO: Tạo một bản ghi trong bảng nối giữa Hội viên và Khuyến mãi
                // để "gán" khuyến mãi này cho người dùng.
                // Ví dụ: db.HoiVien_KhuyenMais.Add(new HoiVien_KhuyenMai { HoiVienId = userId, KhuyenMaiId = keHoach.KhuyenMaiId.Value });
            }
            await db.SaveChangesAsync();

            return Json(new { success = true, message = "Chúc mừng bạn đã hoàn thành kế hoạch! Một phần thưởng đang chờ bạn." });
        }

        return Json(new { success = true, message = "Hoàn thành bài tập hôm nay!" });
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