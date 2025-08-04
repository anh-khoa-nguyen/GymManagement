using GymManagementSystem.Models;
using Microsoft.AspNet.Identity;
using System;
using System.Data.Entity;
using System.Linq;
using System.Web.Mvc;

namespace GymManagementSystem.Controllers
{
    [Authorize(Roles = "HoiVien")] 
    public class HoiVienController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: HoiVien
        public ActionResult Index()
        {
            // Đây sẽ là trang dashboard chính của hội viên sau này
            return View();
        }

        // GET: HoiVien/DanhSachGoiTap
        public ActionResult DanhSachGoiTap()
        {
            var goiTaps = db.GoiTaps.ToList();
            return View(goiTaps);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ChonGoiTap(int goiTapId)
        {
            // 1. Lấy ID của người dùng đang đăng nhập
            var currentUserId = User.Identity.GetUserId();

            // 2. Tìm hồ sơ Hội viên để lấy ID của bảng HoiVien (chứ không phải ApplicationUser)
            var hoiVienProfile = db.HoiViens.FirstOrDefault(hv => hv.ApplicationUserId == currentUserId);

            // 3. Tìm thông tin gói tập để biết thời hạn (nếu có)
            // Trong tương lai, bạn có thể thêm cột ThoiHan (số ngày) vào bảng GoiTap
            var goiTap = db.GoiTaps.Find(goiTapId);

            if (hoiVienProfile != null && goiTap != null)
            {
                // 4. Tạo một bản ghi Đăng ký mới
                var dangKyMoi = new DangKyGoiTap
                {
                    HoiVienId = hoiVienProfile.Id,
                    GoiTapId = goiTapId,
                    NgayDangKy = DateTime.Now,
                    NgayHetHan = DateTime.Now.AddDays(30), // Giả sử mặc định là 30 ngày
                    TrangThai = TrangThaiDangKy.HoatDong
                };

                // 5. Thêm bản ghi mới vào CSDL
                db.DangKyGoiTaps.Add(dangKyMoi);
                db.SaveChanges();

                // 6. Gửi thông báo thành công về cho View
                TempData["SuccessMessage"] = $"Bạn đã đăng ký thành công gói '{goiTap.TenGoi}'!";
            }
            else
            {
                TempData["ErrorMessage"] = "Đã có lỗi xảy ra. Vui lòng thử lại.";
            }

            // 7. Chuyển hướng người dùng trở lại trang danh sách gói tập
            return RedirectToAction("DanhSachGoiTap");
        }

    }
}