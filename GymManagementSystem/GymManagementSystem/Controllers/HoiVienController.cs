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
        public JsonResult TaoLichTap(int huanLuyenVienId, string ngayDatLich, string gioBatDau, string ghiChu)
        {
            try
            {
                var currentUserId = User.Identity.GetUserId();
                var hoiVienProfile = db.HoiViens.FirstOrDefault(hv => hv.ApplicationUserId == currentUserId);

                if (hoiVienProfile == null)
                {
                    return Json(new { success = false, message = "Không tìm thấy hồ sơ hội viên." });
                }

                // Kiểm tra đầu vào
                if (string.IsNullOrEmpty(ngayDatLich) || string.IsNullOrEmpty(gioBatDau))
                {
                    return Json(new { success = false, message = "Vui lòng chọn ngày và giờ." });
                }

                DateTime ngay = DateTime.Parse(ngayDatLich);
                TimeSpan gio = TimeSpan.Parse(gioBatDau);
                DateTime thoiGianBatDau = ngay.Add(gio);
                DateTime thoiGianKetThuc = thoiGianBatDau.AddHours(1);

                var lichTapMoi = new LichTap
                {
                    HoiVienId = hoiVienProfile.Id,
                    HuanLuyenVienId = huanLuyenVienId,
                    ThoiGianBatDau = thoiGianBatDau,
                    ThoiGianKetThuc = thoiGianKetThuc,
                    GhiChuHoiVien = ghiChu,
                    TrangThai = TrangThaiLichTap.ChoDuyet
                };

                db.LichTaps.Add(lichTapMoi);
                db.SaveChanges();

                // Trả về kết quả thành công dưới dạng JSON
                return Json(new { success = true, message = "Yêu cầu đặt lịch đã được gửi đi!" });
            }
            catch (Exception ex)
            {
                // Ghi lại lỗi và trả về thông báo lỗi
                System.Diagnostics.Debug.WriteLine(ex.ToString());
                return Json(new { success = false, message = "Đã có lỗi hệ thống xảy ra. Vui lòng thử lại." });
            }
        }

        // GET: HoiVien/GetLichTapCuaHoiVien
        public JsonResult GetLichTapCuaHoiVien()
        {
            try
            {
                var currentUserId = User.Identity.GetUserId();

                // Lấy dữ liệu thô từ CSDL
                var lichTapData = db.LichTaps
                    .Where(l => l.HoiVien.ApplicationUserId == currentUserId)
                    .Select(l => new
                    {
                        Id = l.Id,
                        HoTenPT = l.HuanLuyenVien.ApplicationUser.HoTen,
                        ThoiGianBatDau = l.ThoiGianBatDau,
                        ThoiGianKetThuc = l.ThoiGianKetThuc,
                        TrangThai = l.TrangThai,
                        GhiChuHoiVien = l.GhiChuHoiVien
                    })
                    .ToList();

                // Xử lý dữ liệu trên bộ nhớ
                var events = lichTapData.Select(l => new
                {
                    id = l.Id,
                    title = "Tập với PT " + (l.HoTenPT ?? "Chưa xác định"),
                    start = l.ThoiGianBatDau.ToString("o"),
                    end = l.ThoiGianKetThuc.ToString("o"),
                    backgroundColor = GetEventColor(l.TrangThai),
                    borderColor = GetEventColor(l.TrangThai),
                    extendedProps = new
                    {
                        trangThaiText = l.TrangThai.ToString(),
                        ghiChu = l.GhiChuHoiVien ?? ""
                    }
                }).ToList();

                return Json(events, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.ToString());
                Response.StatusCode = 500;
                return Json(new { message = ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }

        // Hàm helper để quyết định màu sắc dựa trên trạng thái
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

        // GET: HoiVien/XemTienDo
        public ActionResult XemTienDo()
        {
            var currentUserId = User.Identity.GetUserId();
            var hoiVien = db.HoiViens
                            .Include(hv => hv.ApplicationUser)
                            .Include(hv => hv.ChiSoSucKhoes)
                            .FirstOrDefault(hv => hv.ApplicationUserId == currentUserId);

            if (hoiVien == null)
            {
                return HttpNotFound("Không tìm thấy hồ sơ hội viên của bạn.");
            }

            // Sắp xếp lại danh sách chỉ số theo ngày tháng
            if (hoiVien.ChiSoSucKhoes != null)
            {
                hoiVien.ChiSoSucKhoes = hoiVien.ChiSoSucKhoes.OrderBy(cs => cs.NgayCapNhat).ToList();
            }

            // --- BẮT ĐẦU TÍNH TOÁN CÁC CHỈ SỐ THỐNG KÊ ---
            if (hoiVien.ChiSoSucKhoes.Any())
            {
                var chiSoDauTien = hoiVien.ChiSoSucKhoes.First();
                var chiSoMoiNhat = hoiVien.ChiSoSucKhoes.Last();

                ViewBag.CanNangBatDau = chiSoDauTien.CanNang;
                ViewBag.CanNangHienTai = chiSoMoiNhat.CanNang;
                ViewBag.ThayDoiCanNang = Math.Round(chiSoMoiNhat.CanNang - chiSoDauTien.CanNang, 1);

                ViewBag.SoNgayTheoDoi = (chiSoMoiNhat.NgayCapNhat - chiSoDauTien.NgayCapNhat).Days;
            }
            else
            {
                ViewBag.CanNangBatDau = 0;
                ViewBag.CanNangHienTai = 0;
                ViewBag.ThayDoiCanNang = 0;
                ViewBag.SoNgayTheoDoi = 0;
            }
            // --- KẾT THÚC TÍNH TOÁN ---

            return View(hoiVien);
        }

        // GET: HoiVien/GetBookingForm
        public PartialViewResult GetBookingForm()
        {
            var danhSachPT = db.HuanLuyenViens.Include(pt => pt.ApplicationUser).ToList();
            ViewBag.DanhSachPT = new SelectList(danhSachPT, "Id", "ApplicationUser.HoTen");
            return PartialView("_BookingFormPartial");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public JsonResult GuiDanhGia(int lichTapId, int soSao, string phanHoi)
        {
            try
            {
                var currentUserId = User.Identity.GetUserId();
                // Tìm lịch tập, đồng thời kiểm tra xem nó có đúng là của hội viên này không để bảo mật
                var lichTap = db.LichTaps.FirstOrDefault(l => l.Id == lichTapId && l.HoiVien.ApplicationUserId == currentUserId);

                if (lichTap == null)
                {
                    return Json(new { success = false, message = "Không tìm thấy buổi tập hợp lệ." });
                }

                // Cập nhật thông tin đánh giá
                lichTap.DanhGiaSao = soSao;
                lichTap.PhanHoi = phanHoi;

                db.Entry(lichTap).State = EntityState.Modified;
                db.SaveChanges();

                return Json(new { success = true, message = "Cảm ơn bạn đã gửi đánh giá!" });
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.ToString());
                return Json(new { success = false, message = "Đã có lỗi hệ thống xảy ra." });
            }
        }

    }
}