using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web.Mvc;
using GymManagementSystem.Models;
using GymManagementSystem.Models.ViewModels;
using Microsoft.AspNet.Identity;

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
    }
}