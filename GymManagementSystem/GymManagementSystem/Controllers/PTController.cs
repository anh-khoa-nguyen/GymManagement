using GymManagementSystem.Models;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web.Mvc;

namespace GymManagementSystem.Controllers
{
    [Authorize(Roles = "PT")]
    public class PTController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        //=====================================================================
        // TRANG CHÍNH & API CHO LỊCH LÀM VIỆC
        //=====================================================================

        // GET: PT/LichLamViec
        public ActionResult LichLamViec()
        {
            return View();
        }

        // GET: PT/GetLichCuaPT (API Endpoint cho FullCalendar)
        public JsonResult GetLichCuaPT()
        {
            try
            {
                var currentUserId = User.Identity.GetUserId();
                var ptProfile = db.HuanLuyenViens.FirstOrDefault(pt => pt.ApplicationUserId == currentUserId);

                if (ptProfile == null)
                {
                    return Json(new List<object>(), JsonRequestBehavior.AllowGet);
                }

                var lichTapData = db.LichTaps
                    .Where(l => l.HuanLuyenVienId == ptProfile.Id)
                    .Select(l => new 
                    {
                        Id = l.Id,
                        HoTenHoiVien = l.HoiVien.ApplicationUser.HoTen,
                        ThoiGianBatDau = l.ThoiGianBatDau,
                        ThoiGianKetThuc = l.ThoiGianKetThuc,
                        TrangThai = l.TrangThai,
                        HoiVienId = l.HoiVienId,
                        GhiChuHoiVien = l.GhiChuHoiVien
                    })
                    .ToList();

                var events = lichTapData.Select(l => new
                {
                    id = l.Id,
                    title = "Tập với " + l.HoTenHoiVien,
                    start = l.ThoiGianBatDau.ToString("o"),
                    end = l.ThoiGianKetThuc.ToString("o"),
                    backgroundColor = GetEventColor(l.TrangThai),
                    borderColor = GetEventColor(l.TrangThai),
                    extendedProps = new
                    {
                        trangThaiText = l.TrangThai.ToString(),
                        hoiVienId = l.HoiVienId,
                        ghiChuHoiVien = l.GhiChuHoiVien ?? ""
                    }
                }).ToList();

                return Json(events, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.ToString());
                Response.StatusCode = 500;
                return Json(new { success = false, message = ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }

        //=====================================================================
        // CÁC ACTION XỬ LÝ LỊCH TẬP (DUYỆT, HỦY)
        //=====================================================================

        // POST: PT/DuyetLich
        [HttpPost]
        [ValidateAntiForgeryToken]
        public JsonResult DuyetLich(int lichTapId)
        {
            var lichTap = db.LichTaps.Include(l => l.HoiVien).FirstOrDefault(l => l.Id == lichTapId);
            if (lichTap != null && lichTap.TrangThai == TrangThaiLichTap.ChoDuyet)
            {
                lichTap.TrangThai = TrangThaiLichTap.DaDuyet;
                db.Entry(lichTap).State = EntityState.Modified; // Đánh dấu lịch tập đã thay đổi

                // === PHẦN SỬA ĐỔI LOGIC THÔNG BÁO ===
                var thongBao = new ThongBao
                {
                    ApplicationUserId = lichTap.HoiVien.ApplicationUserId,
                    NoiDung = $"Lịch tập lúc {lichTap.ThoiGianBatDau:HH:mm dd/MM} đã được duyệt.",
                    URL = "#", // URL tạm thời
                    NgayTao = DateTime.Now
                };
                db.ThongBaos.Add(thongBao);

                // Lưu tất cả thay đổi (cả lịch và thông báo)
                db.SaveChanges();

                // Bây giờ thông báo đã có ID, cập nhật lại URL của nó
                thongBao.URL = Url.Action("RedirectToNotificationUrl", "Home", new { notificationId = thongBao.Id });
                db.SaveChanges(); // Lưu lại lần thứ hai để cập nhật URL

                return Json(new { success = true, message = "Duyệt lịch thành công." });
            }
            return Json(new { success = false, message = "Không tìm thấy hoặc lịch đã được xử lý." });
        }

        // POST: PT/HuyLich 
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult HuyLich(int lichTapId, string lyDo)
        {
            var lichTap = db.LichTaps.Include(l => l.HoiVien).FirstOrDefault(l => l.Id == lichTapId);
            if (lichTap != null)
            {
                lichTap.TrangThai = TrangThaiLichTap.DaHuy;
                lichTap.GhiChuPT = $"Đã hủy/từ chối bởi PT. Lý do: {lyDo}";

                // === PHẦN SỬA ĐỔI LOGIC THÔNG BÁO ===
                var thongBao = new ThongBao
                {
                    ApplicationUserId = lichTap.HoiVien.ApplicationUserId,
                    NoiDung = $"Lịch tập lúc {lichTap.ThoiGianBatDau:HH:mm dd/MM} đã bị từ chối.",
                    URL = "#", // URL tạm thời
                    NgayTao = DateTime.Now
                };
                db.ThongBaos.Add(thongBao);

                // Lưu tất cả thay đổi
                db.SaveChanges();

                // Cập nhật lại URL của thông báo
                thongBao.URL = Url.Action("RedirectToNotificationUrl", "Home", new { notificationId = thongBao.Id });
                db.SaveChanges();
            }
            return RedirectToAction("LichLamViec");
        }


        //=====================================================================
        // CÁC ACTION XEM VÀ CẬP NHẬT HỒ SƠ HỘI VIÊN
        //=====================================================================

        // GET: PT/ChiTietHoiVien/5
        public ActionResult ChiTietHoiVien(int id)
        {
            var hoiVien = db.HoiViens
                            .Include("ApplicationUser") // Sử dụng chuỗi để an toàn hơn
                            .Include("ChiSoSucKhoes")
                            .FirstOrDefault(hv => hv.Id == id);

            if (hoiVien == null)
            {
                return HttpNotFound();
            }
            
            // Sắp xếp trên bộ nhớ
            if (hoiVien.ChiSoSucKhoes != null)
            {
                hoiVien.ChiSoSucKhoes = hoiVien.ChiSoSucKhoes.OrderByDescending(cs => cs.NgayCapNhat).ToList();
            }

            return View(hoiVien);
        }

        // POST: PT/CapNhatChiSo
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CapNhatChiSo(int Id, DateTime NgayCapNhat, double CanNang, double? TyLeMo)
        {
            var chiSoMoi = new ChiSoSucKhoe
            {
                HoiVienId = Id,
                NgayCapNhat = NgayCapNhat,
                CanNang = CanNang,
                TyLeMo = TyLeMo
            };

            db.ChiSoSucKhoes.Add(chiSoMoi);
            db.SaveChanges();

            return RedirectToAction("ChiTietHoiVien", new { id = Id });
        }


        //=====================================================================
        // HÀM HELPER
        //=====================================================================
        private string GetEventColor(TrangThaiLichTap trangThai)
        {
            switch (trangThai)
            {
                case TrangThaiLichTap.ChoDuyet: return "#f0ad4e";
                case TrangThaiLichTap.DaDuyet: return "#5cb85c";
                case TrangThaiLichTap.DaHuy: return "#777777";
                case TrangThaiLichTap.DaHoanThanh: return "#337ab7";
                default: return "#777777";
            }
        }
    }
}