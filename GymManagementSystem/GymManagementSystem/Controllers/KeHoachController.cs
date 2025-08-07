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

        // GET: KeHoach (Hiển thị danh sách các kế hoạch có thể đăng ký)
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

            // PHÂN LOẠI KẾ HOẠCH
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

        // POST: KeHoach/DangKy/5 (Xử lý khi người dùng nhấn nút đăng ký)
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DangKy(int keHoachId)
        {
            var userId = User.Identity.GetUserId();

            // Kiểm tra xem người dùng đã đăng ký kế hoạch này chưa để tránh trùng lặp
            bool daDangKy = await db.DangKyKeHoachs.AnyAsync(d => d.HoiVienId == userId && d.KeHoachId == keHoachId);
            if (daDangKy)
            {
                // Có thể thêm TempData để báo lỗi cho người dùng
                TempData["ErrorMessage"] = "Bạn đã đăng ký kế hoạch này rồi.";
                return RedirectToAction("Index");
            }

            // Tạo bản ghi đăng ký mới
            var dangKyMoi = new DangKyKeHoach
            {
                HoiVienId = userId,
                KeHoachId = keHoachId,
                NgayBatDau = DateTime.Today, // Bắt đầu từ hôm nay
                TrangThai = "Đang thực hiện"
            };

            db.DangKyKeHoachs.Add(dangKyMoi);
            await db.SaveChangesAsync();

            // Sau khi đăng ký thành công, chuyển thẳng đến trang xem chi tiết kế hoạch
            return RedirectToAction("XemKeHoach", "ThucHienKeHoach", new { dangKyKeHoachId = dangKyMoi.Id });
        }
    }
}