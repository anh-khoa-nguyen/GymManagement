using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;
using GymManagementSystem.Models;
using GymManagementSystem.Models.ViewModels;
using Microsoft.AspNet.Identity;

namespace GymManagementSystem.Controllers
{
    [Authorize(Roles = "HoiVien")]
    public class KeHoachController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        #region
        public async Task<ActionResult> Index()
        {
            var userId = User.Identity.GetUserId();
            var tatCaKeHoach = await db.KeHoachs.Where(k => k.IsActive).ToListAsync();

            var cacDangKy = await db.DangKyKeHoachs
                                      .Where(d => d.HoiVienId == userId)
                                      .Include(d => d.KeHoach.ChiTietKeHoachs)
                                      .ToListAsync();

            var dangKyIds = cacDangKy.ToDictionary(d => d.KeHoachId, d => d.Id);
            var tienDoKeHoach = new Dictionary<int, int>();

            foreach (var dangKy in cacDangKy)
            {
                int tongSoNgay = dangKy.KeHoach.ChiTietKeHoachs.Count;
                int phanTram = 0;
                if (tongSoNgay > 0)
                {
                    int soNgayHoanThanh = await db.TienDoBaiTaps
                                                   .CountAsync(t => t.DangKyKeHoachId == dangKy.Id);
                    phanTram = (int)(((double)soNgayHoanThanh / tongSoNgay) * 100);
                }
                tienDoKeHoach[dangKy.KeHoachId] = phanTram;
            }

            var daDangKyList = tatCaKeHoach.Where(k => dangKyIds.ContainsKey(k.Id)).ToList();
            var chuaDangKyList = tatCaKeHoach.Where(k => !dangKyIds.ContainsKey(k.Id)).ToList();

            var viewModel = new KeHoachListViewModel
            {
                DaDangKy = daDangKyList,
                ChuaDangKy = chuaDangKyList,
                DangKyIds = dangKyIds,
                TienDoKeHoach = tienDoKeHoach
            };

            return View(viewModel);
        }

        // POST: KeHoach/DangKy/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DangKy(int keHoachId)
        {
            var userId = User.Identity.GetUserId();

            bool daDangKy = await db.DangKyKeHoachs.AnyAsync(d => d.HoiVienId == userId && d.KeHoachId == keHoachId);
            if (daDangKy)
            {
                TempData["ErrorMessage"] = "Bạn đã đăng ký kế hoạch này rồi.";
                return RedirectToAction("Index");
            }

            var dangKyMoi = new DangKyKeHoach
            {
                HoiVienId = userId,
                KeHoachId = keHoachId,
                NgayBatDau = DateTime.Today,
                TrangThai = "Đang thực hiện"
            };

            db.DangKyKeHoachs.Add(dangKyMoi);
            await db.SaveChangesAsync();

            return RedirectToAction("XemKeHoach", "ThucHienKeHoach", new { dangKyKeHoachId = dangKyMoi.Id });
        }
        #endregion
    }
}