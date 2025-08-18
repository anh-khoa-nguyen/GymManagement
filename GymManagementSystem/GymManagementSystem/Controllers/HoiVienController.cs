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
using System.Globalization; 

namespace GymManagementSystem.Controllers
{
    [Authorize(Roles = "HoiVien")] 
    public class HoiVienController : Controller
    {
        private readonly ApplicationDbContext db;
        private readonly BookingService _bookingService; 

        public HoiVienController()
        {
            db = new ApplicationDbContext(); 
            _bookingService = new BookingService(db);
        }

        // GET: HoiVien
        public ActionResult Index()
        {
            return View();
        }

        //// GET: HoiVien/DanhSachGoiTap
        //public ActionResult DanhSachGoiTap()
        //{
        //    var goiTaps = db.GoiTaps.ToList();
        //    return View(goiTaps);
        //}

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> ChonGoiTap(int goiTapId)
        {
            var goiTap = await db.GoiTaps.FindAsync(goiTapId);
            if (goiTap == null) return HttpNotFound();

            var currentUserId = User.Identity.GetUserId();
            var hoiVienProfile = await db.HoiViens.FirstOrDefaultAsync(hv => hv.ApplicationUserId == currentUserId);
            if (hoiVienProfile == null) return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            // 1. TẠO BẢN GHI ĐĂNG KÝ MỚI
            var dangKyMoi = new DangKyGoiTap
            {
                HoiVienId = hoiVienProfile.Id,
                GoiTapId = goiTapId,
                NgayDangKy = DateTime.Now,
                TrangThai = TrangThaiDangKy.ChoThanhToan, 
                SoBuoiTapVoiPT = goiTap.SoBuoiTapVoiPT, 
                SoBuoiPTDaSuDung = 0
            };
            db.DangKyGoiTaps.Add(dangKyMoi);
            await db.SaveChangesAsync();

            // 2. TẠO HÓA ĐƠN TƯƠNG ỨNG
            var hoaDon = new HoaDon
            {
                // Sửa: HoaDon cần ApplicationUserId (string), không phải HoiVienId (int)
                HoiVienId = currentUserId,
                GoiTapId = goiTap.Id,
                NgayTao = DateTime.Now,
                GiaGoc = goiTap.GiaTien,
                SoTienGiam = 0,
                ThanhTien = goiTap.GiaTien,
                TrangThai = TrangThai.ChoThanhToan
            };
            db.HoaDons.Add(hoaDon);
            await db.SaveChangesAsync();

            // 3. CHUYỂN HƯỚNG ĐẾN TRANG THANH TOÁN
            return RedirectToAction("XacNhanThanhToan", new { hoaDonId = hoaDon.Id });
        }
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

        #region Thanh toán hóa đơn
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

        private async Task<List<SelectListItem>> VouchersChoGoiTap(int goiTapId)
        {
            var userId = User.Identity.GetUserId();
            var hoivienProfile = await db.HoiViens.FirstOrDefaultAsync(h => h.ApplicationUserId == userId);
            if (hoivienProfile == null)
            {
                return new List<SelectListItem>();
            }

            var userVouchers = await db.KhuyenMaiCuaHoiViens
                .Include(v => v.KhuyenMai.ApDungChoGoiTap)
                .Where(v => v.HoiVienId == hoivienProfile.Id &&
                            v.TrangThai == TrangThaiKhuyenMaiHV.ChuaSuDung &&
                            v.NgayHetHan >= DateTime.Today)
                .ToListAsync();

            var validVouchers = userVouchers.Where(v =>
            {
                var linkedGoiTaps = v.KhuyenMai.ApDungChoGoiTap;
                if (!linkedGoiTaps.Any()) return true;
                return linkedGoiTaps.Any(gt => gt.GoiTapId == goiTapId);
            })
            .Select(v => new SelectListItem
            {
                Value = v.Id.ToString(),
                Text = v.KhuyenMai.TenKhuyenMai
            }).ToList();

            return validVouchers;
        }

        // GET: /HoiVien/XacNhanThanhToan?hoaDonId=5
        // Hiển thị trang xác nhận thông tin trước khi thanh toán
        public async Task<ActionResult> XacNhanThanhToan(int hoaDonId)
        {
            if (hoaDonId == null)
            {
                return new HttpStatusCodeResult(System.Net.HttpStatusCode.BadRequest);
            }

            var hoaDon = await db.HoaDons
                         .Include(h => h.GoiTap)
                         .FirstOrDefaultAsync(h => h.Id == hoaDonId);

            if (hoaDon == null || hoaDon.HoiVienId != User.Identity.GetUserId()) 
                return HttpNotFound();

            var danhSachVoucher = await VouchersChoGoiTap(hoaDon.GoiTapId);


            var viewModel = new XacNhanThanhToanViewModel
            {
                HoaDon = hoaDon,
                DanhSachKhuyenMai = danhSachVoucher
            };

            if (hoaDon.KhuyenMaiId.HasValue)
            {
                viewModel.KhuyenMaiDaApDung = await db.KhuyenMais.FindAsync(hoaDon.KhuyenMaiId.Value);
            }

            return View(viewModel);
        }
        #endregion


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

        // Action này trả về Partial View chứa thông tin chi tiết của một lịch hẹn
        public ActionResult GetBookingDetails(int lichTapId)
        {
            var currentUserId = User.Identity.GetUserId();
    
            // Tách câu lệnh Include để làm cho nó an toàn hơn
            var lichTap = db.LichTaps
                            .Include(l => l.HoiVien.ApplicationUser) // Luôn cần thông tin hội viên
                            .Include(l => l.HuanLuyenVien.ApplicationUser) // Vẫn include nhưng sẽ xử lý null ở View
                            .FirstOrDefault(l => l.Id == lichTapId && l.HoiVien.ApplicationUserId == currentUserId);

            if (lichTap == null)
            {
                return HttpNotFound();
            }

            return PartialView("_BookingDetailsPartial", lichTap);
        }

        // GET: HoiVien/GetFeedbackForm?lichTapId=5
        public ActionResult GetFeedbackForm(int lichTapId)
        {
            var currentUserId = User.Identity.GetUserId();
            // Tìm lịch tập và kiểm tra quyền sở hữu
            var lichTap = db.LichTaps.FirstOrDefault(l => l.Id == lichTapId && l.HoiVien.ApplicationUserId == currentUserId);

            if (lichTap == null)
            {
                // Trả về lỗi nếu không tìm thấy hoặc không có quyền
                return new HttpStatusCodeResult(System.Net.HttpStatusCode.NotFound, "Không tìm thấy buổi tập.");
            }

            // Trả về PartialView, truyền model LichTap vào
            return PartialView("_FeedbackFormPartial", lichTap);
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
            System.Diagnostics.Debug.WriteLine($"--- API GetAvailableSlots called with ptId: {ptId}, date: {date} ---");

            try
            {
                if (!DateTime.TryParse(date, out DateTime selectedDate))
                {
                    return Json(new { success = false, message = "Định dạng ngày không hợp lệ." }, JsonRequestBehavior.AllowGet);
                }

                var slots = await _bookingService.GetAvailableSlotsAsync(ptId, selectedDate);


                System.Diagnostics.Debug.WriteLine($"--- Found {slots.Count} available slots. ---");

                return Json(slots, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine("--- ERROR in GetAvailableSlots: " + ex.ToString() + " ---");
                Response.StatusCode = 500;
                return Json(new { success = false, message = "Lỗi server khi lấy dữ liệu giờ trống." }, JsonRequestBehavior.AllowGet);
            }
        }


        // GET: HoiVien/KhuyenMai
        public async Task<ActionResult> KhuyenMai()
        {
            var userId = User.Identity.GetUserId();
            var hoivienProfile = await db.HoiViens.FirstOrDefaultAsync(h => h.ApplicationUserId == userId);

            if (hoivienProfile == null)
            {
                // Nếu không có hồ sơ hội viên, trả về một danh sách rỗng
                return View(new List<KhuyenMaiCuaHoiVien>());
            }

            // Lấy danh sách các voucher CHƯA SỬ DỤNG và CÒN HẠN của hội viên
            var vouchers = await db.KhuyenMaiCuaHoiViens
                .Include(v => v.KhuyenMai) // Nạp sẵn thông tin của KhuyenMai để hiển thị tên
                .Where(v => v.HoiVienId == hoivienProfile.Id &&
                            v.TrangThai == TrangThaiKhuyenMaiHV.ChuaSuDung &&
                            v.NgayHetHan >= System.DateTime.Today)
                .OrderBy(v => v.NgayHetHan) // Sắp xếp theo ngày hết hạn gần nhất
                .ToListAsync();

            return View(vouchers);
        }

        [HttpGet] // Chỉ cho phép yêu cầu GET
        public JsonResult GetPTListForChat()
        {
            var pts = db.HuanLuyenViens
                        .Select(pt => new {
                            Id = pt.ApplicationUser.Id, // Lấy ApplicationUserId để chat
                            Name = pt.ApplicationUser.HoTen,
                            Avatar = pt.ApplicationUser.AvatarUrl
                        }).ToList();
            return Json(pts, JsonRequestBehavior.AllowGet);
        }

        // POST: HoiVien/HuyLich
        [HttpPost]
        [ValidateAntiForgeryToken]
        public JsonResult HuyLich(int lichTapId)
        {
            var currentUserId = User.Identity.GetUserId();
            // Tìm lịch hẹn và đảm bảo nó thuộc về người dùng đang đăng nhập
            var lichTap = db.LichTaps.FirstOrDefault(l => l.Id == lichTapId && l.HoiVien.ApplicationUserId == currentUserId);

            if (lichTap == null)
            {
                return Json(new { success = false, message = "Không tìm thấy lịch hẹn." });
            }

            // Chỉ cho phép hủy lịch chưa diễn ra và chưa bị hủy
            if (lichTap.ThoiGianBatDau <= DateTime.Now)
            {
                return Json(new { success = false, message = "Không thể hủy lịch hẹn đã hoặc đang diễn ra." });
            }
            if (lichTap.TrangThai == TrangThaiLichTap.DaHuy)
            {
                return Json(new { success = false, message = "Lịch hẹn này đã được hủy trước đó." });
            }

            lichTap.TrangThai = TrangThaiLichTap.DaHuy;

            // Tùy chọn: Tạo thông báo cho PT biết rằng hội viên đã hủy lịch
            if (lichTap.HuanLuyenVienId.HasValue)
            {
                var pt = db.HuanLuyenViens.Find(lichTap.HuanLuyenVienId.Value);
                if (pt != null)
                {
                    var thongBaoChoPT = new ThongBao
                    {
                        ApplicationUserId = pt.ApplicationUserId,
                        NoiDung = $"Hội viên {lichTap.HoiVien.ApplicationUser.HoTen} đã hủy lịch hẹn lúc {lichTap.ThoiGianBatDau:HH:mm dd/MM}.",
                        URL = "#", // URL tạm thời
                        NgayTao = DateTime.Now
                    };
                    db.ThongBaos.Add(thongBaoChoPT);
                }
            }

            db.SaveChanges();

            return Json(new { success = true });
        }

        
    }
}