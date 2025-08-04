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

        // GET: HoiVien/DatLich
        public ActionResult DatLich()
        {
            // Lấy danh sách PT để đưa vào dropdown
            var danhSachPT = db.HuanLuyenViens.Include(pt => pt.ApplicationUser).ToList();
            ViewBag.DanhSachPT = new SelectList(danhSachPT, "Id", "ApplicationUser.HoTen");

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult TaoLichTap(int huanLuyenVienId, string ngayDatLich, string gioBatDau, string ghiChu)
        {
            // Lấy thông tin hội viên hiện tại
            var currentUserId = User.Identity.GetUserId();
            var hoiVienProfile = db.HoiViens.FirstOrDefault(hv => hv.ApplicationUserId == currentUserId);

            if (hoiVienProfile == null)
            {
                return new HttpStatusCodeResult(System.Net.HttpStatusCode.BadRequest, "Không tìm thấy hồ sơ hội viên.");
            }

            // Xử lý chuỗi ngày và giờ để tạo đối tượng DateTime hoàn chỉnh
            DateTime ngay = DateTime.Parse(ngayDatLich);
            TimeSpan gio = TimeSpan.Parse(gioBatDau);
            DateTime thoiGianBatDau = ngay.Add(gio);

            // Giả sử mỗi buổi tập kéo dài 1 tiếng
            DateTime thoiGianKetThuc = thoiGianBatDau.AddHours(1);

            // Tạo một bản ghi Lịch Tập mới
            var lichTapMoi = new LichTap
            {
                HoiVienId = hoiVienProfile.Id,
                HuanLuyenVienId = huanLuyenVienId,
                ThoiGianBatDau = thoiGianBatDau,
                ThoiGianKetThuc = thoiGianKetThuc,
                GhiChuHoiVien = ghiChu,
                TrangThai = TrangThaiLichTap.ChoDuyet // Trạng thái ban đầu
            };

            db.LichTaps.Add(lichTapMoi);
            db.SaveChanges();

            // Có thể dùng TempData để gửi thông báo thành công
            TempData["SuccessMessage"] = "Yêu cầu đặt lịch của bạn đã được gửi đi thành công!";

            return RedirectToAction("DatLich");
        }

        // GET: HoiVien/GetLichTapCuaHoiVien
        public JsonResult GetLichTapCuaHoiVien()
        {
            var currentUserId = User.Identity.GetUserId();

            // SỬA ĐỔI LOGIC TRUY VẤN Ở ĐÂY
            // Chúng ta không cần tìm HoiVienId nữa.
            // Hãy đi trực tiếp từ bảng LichTap và lọc theo ApplicationUserId của Hội viên liên quan.
            var events = db.LichTaps
                .Where(l => l.HoiVien.ApplicationUserId == currentUserId) // <-- THAY ĐỔI QUAN TRỌNG
                .Select(l => new // Chuyển đổi thành định dạng mà FullCalendar hiểu
                {
                    id = l.Id,
                    title = l.HuanLuyenVienId != null
                            ? "Tập với PT " + l.HuanLuyenVien.ApplicationUser.HoTen
                            : "Tự tập",
                    start = l.ThoiGianBatDau, // Gửi trực tiếp đối tượng DateTime
                    end = l.ThoiGianKetThuc,   // FullCalendar v5+ có thể xử lý trực tiếp
                    color = l.TrangThai == TrangThaiLichTap.ChoDuyet ? "#f0ad4e" :
                            l.TrangThai == TrangThaiLichTap.DaDuyet ? "#5cb85c" :
                            l.TrangThai == TrangThaiLichTap.DaHuy ? "#777777" :
                            "#337ab7", // DaHoanThanh
                    extendedProps = new
                    {
                        trangThaiText = l.TrangThai.ToString(),
                        ghiChu = l.GhiChuHoiVien ?? ""
                    }
                }).ToList();

            return Json(events, JsonRequestBehavior.AllowGet);
        }

        // Hàm helper để quyết định màu sắc dựa trên trạng thái
        private string GetEventColor(TrangThaiLichTap trangThai)
        {
            switch (trangThai)
            {
                case TrangThaiLichTap.ChoDuyet:
                    return "#f0ad4e"; 
                case TrangThaiLichTap.DaDuyet:
                    return "#5cb85c"; 
                case TrangThaiLichTap.DaHuy:
                    return "#777777"; 
                case TrangThaiLichTap.DaHoanThanh:
                    return "#337ab7"; 
                default:
                    return "#777777";
            }
        }

    }
}