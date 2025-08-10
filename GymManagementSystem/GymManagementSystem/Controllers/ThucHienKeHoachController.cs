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

    #region Xem (D) / Thực hiện kế hoạch (X)
    public async Task<ActionResult> XemKeHoach(int dangKyKeHoachId)
    {
        var userId = User.Identity.GetUserId();
        var dangKy = await db.DangKyKeHoachs
                             .Include(d => d.KeHoach)
                             .FirstOrDefaultAsync(d => d.Id == dangKyKeHoachId && d.HoiVienId == userId);

        if (dangKy == null) return HttpNotFound();

        // 1. Lấy tất cả các bài tập
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

    public async Task<ActionResult> BaiTapChiTiet(int chiTietKeHoachId, int dangKyKeHoachId)
    {
        var baiTapChiTiet = await db.ChiTietKeHoachs
                                    .Include(ct => ct.BaiTap)
                                    .FirstOrDefaultAsync(ct => ct.Id == chiTietKeHoachId);

        if (baiTapChiTiet == null) return HttpNotFound();

        ViewBag.DangKyKeHoachId = dangKyKeHoachId;
        return View(baiTapChiTiet); // Đây sẽ là View chứa camera và logic tracking
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<JsonResult> HoanThanhBaiTap(int dangKyKeHoachId, int baiTapId)
    {
        try
        {
            var userId = User.Identity.GetUserId();
            var dangKy = await db.DangKyKeHoachs
                .FirstOrDefaultAsync(d => d.Id == dangKyKeHoachId && d.HoiVienId == userId);

            if (dangKy == null)
            {
                return Json(new { success = false, message = "Không tìm thấy kế hoạch đăng ký." });
            }

            db.TienDoBaiTaps.Add(new TienDoBaiTap
            {
                DangKyKeHoachId = dangKyKeHoachId,
                BaiTapId = baiTapId,
                NgayHoanThanh = DateTime.Now
            });
            await db.SaveChangesAsync();

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

                var keHoach = await db.KeHoachs
                    .Include(k => k.KhuyenMai)
                    .FirstOrDefaultAsync(k => k.Id == dangKy.KeHoachId);

                var hoivienProfile = await db.HoiViens
                    .FirstOrDefaultAsync(h => h.ApplicationUserId == userId);

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

                return Json(new { success = true, message = "Chúc mừng bạn đã hoàn thành kế hoạch! Phần thưởng đã được trao cho bạn." });
            }

            return Json(new { success = true, message = "Hoàn thành bài tập hôm nay!" });
        }
        catch (Exception ex)
        {
            return Json(new { success = false, message = "Đã có lỗi không mong muốn xảy ra phía máy chủ. Vui lòng thử lại." });
        }
    }

    #endregion
}