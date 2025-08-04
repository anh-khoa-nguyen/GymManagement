using System;
using System.Data.Entity;
using System.Linq;
using System.Web.Mvc;
using GymManagementSystem.Models;
using Microsoft.AspNet.Identity;

namespace GymManagementSystem.Controllers
{
    [Authorize(Roles = "PT")]
    public class PTController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: PT/Dashboard
        public ActionResult Dashboard()
        {
            var currentUserId = User.Identity.GetUserId();
            var ptProfile = db.HuanLuyenViens.FirstOrDefault(pt => pt.ApplicationUserId == currentUserId);

            if (ptProfile == null)
            {
                return HttpNotFound();
            }

            // Lấy danh sách LỊCH CHỜ DUYỆT được gửi đến PT này
            var lichChoDuyet = db.LichTaps
                                .Include(l => l.HoiVien.ApplicationUser) // Lấy thông tin Hội viên
                                .Where(l => l.HuanLuyenVienId == ptProfile.Id && l.TrangThai == TrangThaiLichTap.ChoDuyet)
                                .OrderBy(l => l.ThoiGianBatDau)
                                .ToList();

            // Lấy danh sách LỊCH ĐÃ DUYỆT (sắp tới) của PT này
            var lichSapToi = db.LichTaps
                                .Include(l => l.HoiVien.ApplicationUser)
                                .Where(l => l.HuanLuyenVienId == ptProfile.Id
                                             && l.TrangThai == TrangThaiLichTap.DaDuyet
                                             && l.ThoiGianBatDau >= DateTime.Now)
                                .OrderBy(l => l.ThoiGianBatDau)
                                .ToList();

            var viewModel = new PTDashboardViewModel
            {
                LichChoDuyet = lichChoDuyet,
                LichSapToi = lichSapToi
            };

            return View(viewModel);
        }

        // POST: PT/DuyetLich
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult DuyetLich(int lichTapId)
        {
            var lichTap = db.LichTaps.Find(lichTapId);
            if (lichTap != null)
            {
                lichTap.TrangThai = TrangThaiLichTap.DaDuyet;
                db.Entry(lichTap).State = EntityState.Modified;
                db.SaveChanges();
            }
            return RedirectToAction("Dashboard");
        }

        // POST: PT/HuyLich
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult HuyLich(int lichTapId, string lyDo)
        {
            var lichTap = db.LichTaps.Find(lichTapId);
            if (lichTap != null)
            {
                lichTap.TrangThai = TrangThaiLichTap.DaHuy;
                lichTap.GhiChuPT = $"Đã hủy bởi PT. Lý do: {lyDo}"; // Thêm lý do hủy
                db.Entry(lichTap).State = EntityState.Modified;
                db.SaveChanges();
            }
            return RedirectToAction("Dashboard");
        }


    }
}