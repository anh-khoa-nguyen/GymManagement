using GymManagementSystem.Models;
using GymManagementSystem.Models.ViewModels;
using GymManagementSystem.Services;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace GymManagementSystem.Controllers
{
    [Authorize(Roles = "HoiVien")] 
    public class HoiVienController : Controller
    {
        private ApplicationDbContext db;
        private readonly BookingService _bookingService; 

        public HoiVienController()
        {
            this.db = new ApplicationDbContext();
            _bookingService = new BookingService(db);
        }

        // GET: HoiVien
        public ActionResult Index()
        {
            // Đây sẽ là trang dashboard chính của hội viên sau này
            return View();
        }

        //// GET: HoiVien/DanhSachGoiTap
        //public ActionResult DanhSachGoiTap()
        //{
        //    var goiTaps = db.GoiTaps.ToList();
        //    return View(goiTaps);
        //}

        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public ActionResult ChonGoiTap(int goiTapId)
        //{
        //    // 1. Lấy ID của người dùng đang đăng nhập
        //    var currentUserId = User.Identity.GetUserId();

        //    // 2. Tìm hồ sơ Hội viên để lấy ID của bảng HoiVien (chứ không phải ApplicationUser)
        //    var hoiVienProfile = db.HoiViens.FirstOrDefault(hv => hv.ApplicationUserId == currentUserId);

        //    // 3. Tìm thông tin gói tập để biết thời hạn (nếu có)
        //    // Trong tương lai, bạn có thể thêm cột ThoiHan (số ngày) vào bảng GoiTap
        //    var goiTap = db.GoiTaps.Find(goiTapId);

        //    if (hoiVienProfile != null && goiTap != null)
        //    {
        //        // 4. Tạo một bản ghi Đăng ký mới
        //        var dangKyMoi = new DangKyGoiTap
        //        {
        //            HoiVienId = hoiVienProfile.Id,
        //            GoiTapId = goiTapId,
        //            NgayDangKy = DateTime.Now,
        //            NgayHetHan = DateTime.Now.AddDays(30), // Giả sử mặc định là 30 ngày
        //            TrangThai = TrangThaiDangKy.HoatDong
        //        };

        //        // 5. Thêm bản ghi mới vào CSDL
        //        db.DangKyGoiTaps.Add(dangKyMoi);
        //        db.SaveChanges();

        //        // 6. Gửi thông báo thành công về cho View
        //        TempData["SuccessMessage"] = $"Bạn đã đăng ký thành công gói '{goiTap.TenGoi}'!";
        //    }
        //    else
        //    {
        //        TempData["ErrorMessage"] = "Đã có lỗi xảy ra. Vui lòng thử lại.";
        //    }

        //    // 7. Chuyển hướng người dùng trở lại trang danh sách gói tập
        //    return RedirectToAction("DanhSachGoiTap");
        //}

        // GET: HoiVien/DatLich
        public ActionResult DatLich()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        // SỬA LẠI TÊN CÁC THAM SỐ ĐỂ KHỚP VỚI VIEWMODEL
        public async Task<JsonResult> TaoLichTap(int? HuanLuyenVienId, string NgayDatLich, string GioBatDau, string GhiChu)
        {
            // 1. KIỂM TRA ĐẦU VÀO
            if (!HuanLuyenVienId.HasValue)
            {
                return Json(new { success = false, message = "Vui lòng chọn một Huấn luyện viên." });
            }
            // Dùng IsNullOrWhiteSpace để kiểm tra an toàn hơn
            if (string.IsNullOrWhiteSpace(NgayDatLich) || string.IsNullOrWhiteSpace(GioBatDau))
            {
                return Json(new { success = false, message = "Vui lòng chọn ngày và giờ hẹn." });
            }

            // 2. LẤY HỒ SƠ HỘI VIÊN
            var currentUserId = User.Identity.GetUserId();
            var hoiVienProfile = await this.db.HoiViens.FirstOrDefaultAsync(hv => hv.ApplicationUserId == currentUserId);

            if (hoiVienProfile == null)
            {
                return Json(new { success = false, message = "Không tìm thấy hồ sơ hội viên của bạn." });
            }

            // 3. PARSE NGÀY GIỜ
            // Dùng NgayDatLich thay vì ngayDatLich
            if (!DateTime.TryParse($"{NgayDatLich} {GioBatDau}", out DateTime thoiGianBatDau))
            {
                return Json(new { success = false, message = "Định dạng ngày hoặc giờ không hợp lệ." });
            }

            // 4. GỌI SERVICE
            var result = await _bookingService.CreateNewBookingAsync(
                hoiVienProfile.Id,
                HuanLuyenVienId.Value,
                thoiGianBatDau,
                GhiChu // Dùng GhiChu thay vì ghiChu
            );

            // 5. TRẢ VỀ KẾT QUẢ
            return Json(new { success = result.Success, message = result.Message });
        }

        // GET: HoiVien/GetLichTapCuaHoiVien
        public async Task<JsonResult> GetLichTapCuaHoiVien()
        {
            try
            {
                var currentUserId = User.Identity.GetUserId();

                // BƯỚC 1: Lấy hồ sơ hội viên trước.
                var hoiVienProfile = await db.HoiViens.FirstOrDefaultAsync(h => h.ApplicationUserId == currentUserId);

                if (hoiVienProfile == null)
                {
                    // Nếu không có hồ sơ, trả về danh sách rỗng một cách an toàn.
                    return Json(new List<object>(), JsonRequestBehavior.AllowGet);
                }

                // BƯỚC 2: Viết lại câu lệnh LINQ để chống lỗi NullReferenceException.
                var lichTapData = await db.LichTaps
                    // Chỉ lấy lịch của hội viên này
                    .Where(l => l.HoiVienId == hoiVienProfile.Id)
                    // QUAN TRỌNG: Đảm bảo rằng PT liên quan và tài khoản của PT đó phải tồn tại.
                    .Where(l => l.HuanLuyenVien != null && l.HuanLuyenVien.ApplicationUser != null)
                    .Select(l => new
                    {
                        Id = l.Id,
                        HoTenPT = l.HuanLuyenVien.ApplicationUser.HoTen,
                        ThoiGianBatDau = l.ThoiGianBatDau,
                        ThoiGianKetThuc = l.ThoiGianKetThuc,
                        TrangThai = l.TrangThai,
                        GhiChuHoiVien = l.GhiChuHoiVien,
                        DanhGiaSao = l.DanhGiaSao
                    })
                    .ToListAsync();

                // BƯỚC 3: Chuyển đổi dữ liệu sang định dạng mà FullCalendar hiểu.
                var events = lichTapData.Select(l => new
                {
                    id = l.Id,
                    title = $"{l.ThoiGianBatDau:HH:mm} - {l.HoTenPT.Split(' ').Last()}", // TIÊU ĐỀ MỚI: "14:00 - Triều"
                    start = l.ThoiGianBatDau.ToString("o"),
                    end = l.ThoiGianKetThuc.ToString("o"),
                    backgroundColor = GetEventColor(l.TrangThai),
                    borderColor = GetEventColor(l.TrangThai),
                    extendedProps = new
                    {
                        // --- THÊM CÁC THÔNG TIN CHI TIẾT VÀO ĐÂY ---
                        hoTenPTDayDu = $"PT. {l.HoTenPT}",
                        thoiGianDayDu = $"{l.ThoiGianBatDau:HH:mm} - {l.ThoiGianKetThuc:HH:mm}, {l.ThoiGianBatDau:dd/MM/yyyy}",
                        trangThaiText = l.TrangThai.ToString(),
                        ghiChu = l.GhiChuHoiVien,
                        daDanhGia = l.DanhGiaSao.HasValue
                    }
                }).ToList();

                return Json(events, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                // Ghi lại lỗi chi tiết để bạn có thể xem trong cửa sổ Output của Visual Studio
                System.Diagnostics.Debug.WriteLine("FATAL ERROR in GetLichTapCuaHoiVien: " + ex.ToString());

                // Gửi một mã lỗi 500 về cho trình duyệt
                Response.StatusCode = 500;
                return Json(new { message = "Đã xảy ra lỗi nghiêm trọng ở máy chủ khi tải dữ liệu lịch." }, JsonRequestBehavior.AllowGet);
            }
        }


        // GET: /HoiVien/DanhSachGoiTap
        // Hiển thị danh sách các gói tập cho hội viên chọn
        //public async Task<ActionResult> DanhSachGoiTap()
        //{
        //    var goiTaps = await db.GoiTaps.ToListAsync();
        //    return View(goiTaps);
        //}

        // POST: /HoiVien/DangKyGoiTap
        // Action này được gọi khi hội viên nhấn nút "Chọn Gói này"
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DangKyGoiTap(int goiTapId)
        {
            var goiTap = await db.GoiTaps.FindAsync(goiTapId);
            if (goiTap == null)
            {
                return HttpNotFound();
            }

            var userId = User.Identity.GetUserId();

            // 1. TẠO RA MỘT HÓA ĐƠN MỚI Ở TRẠNG THÁI "CHỜ THANH TOÁN"
            var hoaDon = new HoaDon
            {
                HoiVienId = userId,
                GoiTapId = goiTap.Id,
                NgayTao = DateTime.Now,
                GiaGoc = goiTap.GiaTien,
                SoTienGiam = 0, // Tạm thời chưa xử lý khuyến mãi ở bước này
                ThanhTien = goiTap.GiaTien,
                TrangThai = TrangThai.ChoThanhToan
            };

            db.HoaDons.Add(hoaDon);
            await db.SaveChangesAsync();

            // 2. CHUYỂN HƯỚNG NGƯỜI DÙNG ĐẾN TRANG XÁC NHẬN VÀ THANH TOÁN
            // Chúng ta sẽ truyền ID của hóa đơn vừa tạo đi
            return RedirectToAction("XacNhanThanhToan", new { hoaDonId = hoaDon.Id });
        }

        // GET: /HoiVien/XacNhanThanhToan?hoaDonId=5
        // Hiển thị trang xác nhận thông tin trước khi thanh toán
        public async Task<ActionResult> XacNhanThanhToan(int hoaDonId)
        {
            var hoaDon = await db.HoaDons
                         .Include(h => h.GoiTap)
                         .FirstOrDefaultAsync(h => h.Id == hoaDonId);

            if (hoaDon == null || hoaDon.HoiVienId != User.Identity.GetUserId()) return HttpNotFound();

            // Lấy hồ sơ hội viên để tìm các voucher
            var hoivienProfile = await db.HoiViens.FirstOrDefaultAsync(h => h.ApplicationUserId == hoaDon.HoiVienId);

            // Lấy danh sách các voucher CHƯA SỬ DỤNG và CÒN HẠN của hội viên
            var danhSachVoucher = await db.KhuyenMaiCuaHoiViens
                .Include(km => km.KhuyenMai)
                .Where(km => km.HoiVienId == hoivienProfile.Id &&
                             km.TrangThai == TrangThaiKhuyenMaiHV.ChuaSuDung &&
                             km.NgayHetHan >= DateTime.Today)
                .Select(km => new SelectListItem
                {
                    Value = km.Id.ToString(),
                    Text = km.KhuyenMai.TenKhuyenMai
                })
                .ToListAsync();

            danhSachVoucher.Insert(0, new SelectListItem { Value = "", Text = "-- Không áp dụng --" });

            KhuyenMai khuyenMaiDaApDung = null;
            if (hoaDon.KhuyenMaiId.HasValue)
            {
                khuyenMaiDaApDung = await db.KhuyenMais.FindAsync(hoaDon.KhuyenMaiId.Value);
            }

            var viewModel = new XacNhanThanhToanViewModel
            {
                HoaDon = hoaDon,
                DanhSachKhuyenMai = danhSachVoucher,
                KhuyenMaiDaApDung = khuyenMaiDaApDung // Gán vào ViewModel
            };

            return View(viewModel);
        }

        public async Task<ActionResult> DanhSachGoiTap()
        {
            var userIdString = User.Identity.GetUserId(); // Lấy ID dạng chuỗi

            var hoiVien = await db.HoiViens.FirstOrDefaultAsync(h => h.ApplicationUserId == userIdString);
            if (hoiVien == null)
            {
                // Xử lý trường hợp không tìm thấy hội viên tương ứng
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var hoiVienIdInt = hoiVien.Id; // Đây là ID kiểu int mà chúng ta cần

            // Lấy danh sách các gói tập đã đăng ký bằng ID kiểu int
            var goiTapDaDangKy = await db.DangKyGoiTaps
                                         .Where(d => d.HoiVienId == hoiVienIdInt) // So sánh int với int
                                         .Include(d => d.GoiTap)
                                         .OrderByDescending(d => d.NgayHetHan)
                                         .ToListAsync();


            // --- Phần còn lại của Action ---
            var daDangKyIds = goiTapDaDangKy.Select(d => d.GoiTapId).ToList();

            var goiTapChuaDangKy = await db.GoiTaps
                                           .Where(g => !daDangKyIds.Contains(g.Id))
                                           .ToListAsync();

            var viewModel = new GoiTapViewModel
            {
                GoiTapDaDangKy = goiTapDaDangKy,
                GoiTapChuaDangKy = goiTapChuaDangKy
            };

            return View(viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> ApDungKhuyenMai(XacNhanThanhToanViewModel viewModel)
        {
            var hoaDon = await db.HoaDons.FindAsync(viewModel.HoaDon.Id);
            var userId = User.Identity.GetUserId();

            if (hoaDon == null || hoaDon.HoiVienId != userId) return HttpNotFound();

            // Nếu người dùng chọn "Không áp dụng"
            if (!viewModel.KhuyenMaiCuaHoiVienId.HasValue)
            {
                // Reset lại hóa đơn về giá gốc
                hoaDon.KhuyenMaiId = null;
                hoaDon.SoTienGiam = 0;
                hoaDon.ThanhTien = hoaDon.GiaGoc;
                await db.SaveChangesAsync();
                TempData["SuccessMessage"] = "Đã gỡ bỏ khuyến mãi.";
                return RedirectToAction("XacNhanThanhToan", new { hoaDonId = hoaDon.Id });
            }

            var hoivienProfile = await db.HoiViens.FirstOrDefaultAsync(h => h.ApplicationUserId == userId);

            // Tìm chính xác voucher mà người dùng đã chọn
            var voucher = await db.KhuyenMaiCuaHoiViens
                                .Include(v => v.KhuyenMai) // Lấy cả thông tin của loại khuyến mãi
                                .FirstOrDefaultAsync(v => v.Id == viewModel.KhuyenMaiCuaHoiVienId.Value &&
                                                          v.HoiVienId == hoivienProfile.Id &&
                                                          v.TrangThai == TrangThaiKhuyenMaiHV.ChuaSuDung);

            if (voucher != null)
            {
                var khuyenMai = voucher.KhuyenMai;

                // === LOGIC TÍNH TOÁN GIẢM GIÁ MỚI ===

                // 1. Tính số tiền giảm theo phần trăm
                decimal soTienGiamTheoPhanTram = (decimal)khuyenMai.PhanTramGiamGia * hoaDon.GiaGoc / 100;

                // 2. Lấy số tiền giảm tối đa từ khuyến mãi
                decimal soTienGiamToiDa = khuyenMai.SoTienGiamToiDa;

                // 3. So sánh và chọn ra số tiền giảm cuối cùng
                // Nếu số tiền giảm tối đa > 0 và số tiền giảm theo % lớn hơn nó, thì chỉ giảm tối đa
                decimal soTienGiamCuoiCung = soTienGiamTheoPhanTram;
                if (soTienGiamToiDa > 0 && soTienGiamTheoPhanTram > soTienGiamToiDa)
                {
                    soTienGiamCuoiCung = soTienGiamToiDa;
                }

                // Cập nhật hóa đơn với số tiền giảm đã được tính toán chính xác
                hoaDon.KhuyenMaiId = khuyenMai.Id;
                hoaDon.SoTienGiam = soTienGiamCuoiCung;
                hoaDon.ThanhTien = hoaDon.GiaGoc - soTienGiamCuoiCung;
                if (hoaDon.ThanhTien < 0) hoaDon.ThanhTien = 0;

                await db.SaveChangesAsync();
                TempData["SuccessMessage"] = $"Đã áp dụng thành công khuyến mãi!";
            }
            else
            {
                TempData["ErrorMessage"] = "Khuyến mãi không hợp lệ hoặc đã được sử dụng.";
            }

            return RedirectToAction("XacNhanThanhToan", new { hoaDonId = hoaDon.Id });
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
        public async Task<PartialViewResult> GetBookingForm(string date)
        {
            try // Bọc trong khối try-catch để bắt lỗi chi tiết
            {
                // Câu lệnh LINQ an toàn hơn
                var danhSachPT = await db.HuanLuyenViens
                    .Where(pt => pt.ApplicationUser != null) // CHỈ LẤY NHỮNG PT CÓ TÀI KHOẢN TỒN TẠI
                    .Select(pt => new SelectListItem
                    {
                        Value = pt.Id.ToString(),
                        Text = pt.ApplicationUser.HoTen
                    })
                    .OrderBy(pt => pt.Text) // Sắp xếp theo tên cho thân thiện
                    .ToListAsync();

                var viewModel = new DatLichViewModel // Hoặc BookingFormViewModel
                {
                    DanhSachPT = danhSachPT,
                    NgayDatLich = DateTime.TryParse(date, out var selectedDate) ? selectedDate : DateTime.Today
                };

                return PartialView("_BookingFormPartial", viewModel);
            }
            catch (Exception ex)
            {
                // GHI LOG LỖI CHI TIẾT ĐỂ DEBUG
                // Khi bạn chạy ở chế độ Debug, lỗi sẽ hiện trong cửa sổ Output của Visual Studio
                System.Diagnostics.Debug.WriteLine("ERROR in GetBookingForm: " + ex.ToString());

                // Trả về một Partial View báo lỗi thân thiện thay vì làm sập trang
                // Bạn cần tạo một file Partial View đơn giản cho việc này
                return PartialView("_ErrorPartial", new HandleErrorInfo(ex, "HoiVien", "GetBookingForm"));
            }
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

        [HttpGet]
        public async Task<JsonResult> GetAvailableSlots(int ptId, string date)
        {
            // GHI LOG: Đây là bước quan trọng để debug. 
            // Khi bạn chạy, hãy xem cửa sổ Output của Visual Studio.
            System.Diagnostics.Debug.WriteLine($"--- API GetAvailableSlots called with ptId: {ptId}, date: {date} ---");

            try
            {
                if (!DateTime.TryParse(date, out DateTime selectedDate))
                {
                    // Trả về lỗi nếu ngày không hợp lệ, thay vì danh sách rỗng
                    return Json(new { success = false, message = "Định dạng ngày không hợp lệ." }, JsonRequestBehavior.AllowGet);
                }

                // Gọi service (đảm bảo _bookingService đã được khởi tạo trong constructor)
                var slots = await _bookingService.GetAvailableSlotsAsync(ptId, selectedDate);
                var formattedSlots = slots.Select(s => s.ToString(@"hh\:mm")).ToList();

                System.Diagnostics.Debug.WriteLine($"--- Found {formattedSlots.Count} available slots. ---");

                // Trả về dữ liệu thành công
                return Json(formattedSlots, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine("--- ERROR in GetAvailableSlots: " + ex.ToString() + " ---");
                Response.StatusCode = 500;
                return Json(new { success = false, message = "Lỗi server khi lấy dữ liệu giờ trống." }, JsonRequestBehavior.AllowGet);
            }
        }

    }
}