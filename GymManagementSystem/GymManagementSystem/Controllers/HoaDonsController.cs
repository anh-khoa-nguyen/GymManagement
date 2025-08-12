using System;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web.Mvc;
using GymManagementSystem.Models;
using GymManagementSystem.Models.ViewModels;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using System.Web;

namespace GymManagementSystem.Controllers
{
    [Authorize(Roles = "QuanLy")]
    public class HoaDonsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        private ApplicationUserManager _userManager;
        public ApplicationUserManager UserManager
        {
            get
            {
                return _userManager ?? HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();
            }
            private set
            {
                _userManager = value;
            }
        }

        // GET: HoaDons (Hiển thị danh sách hóa đơn đã tạo)
        public async Task<ActionResult> Index()
        {
            var hoaDons = db.HoaDons.Include(h => h.GoiTap).Include(h => h.HoiVien).Include(h => h.KhuyenMai);
            return View(await hoaDons.ToListAsync());
        }

        // GET: HoaDons/Create (Hiển thị form lập hóa đơn)
        public async Task<ActionResult> Create()
        {
            // Lấy danh sách hội viên (những người có vai trò "HoiVien")
            var danhSachHoiVien = await db.Users
                                          .Where(u => u.VaiTro == "HoiVien")
                                          .Select(u => new SelectListItem { Value = u.Id, Text = u.HoTen + " (" + u.UserName + ")" })
                                          .ToListAsync();

            // Lấy danh sách gói tập
            var danhSachGoiTap = await db.GoiTaps
                                         .Select(g => new SelectListItem { Value = g.Id.ToString(), Text = g.TenGoi })
                                         .ToListAsync();

            // === SỬA LẠI LOGIC LẤY DANH SÁCH KHUYẾN MÃI ===
            // Logic mới: Chỉ cần lấy các khuyến mãi đang hoạt động, không cần check ngày
            var danhSachKhuyenMai = await db.KhuyenMais
                                            .Where(k => k.IsActive)
                                            .Select(k => new SelectListItem { Value = k.Id.ToString(), Text = k.TenKhuyenMai })
                                            .ToListAsync();
            // Thêm một lựa chọn "Không áp dụng" vào đầu danh sách khuyến mãi
            danhSachKhuyenMai.Insert(0, new SelectListItem { Value = "", Text = "-- Không áp dụng --" });


            // Tạo ViewModel và gán các danh sách đã lấy được
            var viewModel = new TaoHoaDonViewModel
            {
                DanhSachHoiVien = danhSachHoiVien,
                DanhSachGoiTap = danhSachGoiTap,
                DanhSachKhuyenMai = danhSachKhuyenMai
            };

            if (Request.IsAjaxRequest())
            {
                return PartialView("Create", viewModel);
            }

            // Bỏ logic IsAjaxRequest không cần thiết nếu bạn không dùng partial view
            return View(viewModel);
        }

        // POST: HoaDons/Create (Xử lý khi nhấn nút tạo)
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(TaoHoaDonViewModel viewModel)
        {
            if (ModelState.IsValid)
            {
                // Lấy thông tin gói tập và khuyến mãi từ DB
                var goiTap = await db.GoiTaps.FindAsync(viewModel.GoiTapId);
                KhuyenMai khuyenMai = null;
                if (viewModel.KhuyenMaiId.HasValue)
                {
                    khuyenMai = await db.KhuyenMais.FindAsync(viewModel.KhuyenMaiId);
                }

                // Tính toán giá trị hóa đơn
                decimal giaGoc = goiTap.GiaTien;
                decimal soTienGiam = 0;
                if (khuyenMai != null)
                {
                    // === LOGIC TÍNH TOÁN GIẢM GIÁ MỚI ===
                    decimal soTienGiamTheoPhanTram = (decimal)khuyenMai.PhanTramGiamGia * giaGoc / 100;
                    decimal soTienGiamToiDa = khuyenMai.SoTienGiamToiDa; // <-- Sử dụng trường mới

                    soTienGiam = soTienGiamTheoPhanTram;
                    if (soTienGiamToiDa > 0 && soTienGiamTheoPhanTram > soTienGiamToiDa)
                    {
                        soTienGiam = soTienGiamToiDa;
                    }
                }
                decimal thanhTien = giaGoc - soTienGiam;

                // Tạo đối tượng hóa đơn mới
                var hoaDon = new HoaDon
                {
                    HoiVienId = viewModel.HoiVienId,
                    GoiTapId = viewModel.GoiTapId,
                    KhuyenMaiId = viewModel.KhuyenMaiId,
                    NgayTao = DateTime.Now,
                    GiaGoc = giaGoc,
                    SoTienGiam = soTienGiam,
                    ThanhTien = thanhTien,
                    TrangThai = TrangThai.ChoThanhToan // Mặc định
                };

                db.HoaDons.Add(hoaDon);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            // --- SỬA LẠI LOGIC LẤY DANH SÁCH KHUYẾN MÃI KHI MODELSTATE LỖI ---

            // Nếu model không hợp lệ, tải lại các danh sách và trả về view
            viewModel.DanhSachHoiVien = await db.Users
                .Where(u => u.VaiTro == "HoiVien")
                .Select(u => new SelectListItem { Value = u.Id, Text = u.HoTen + " (" + u.UserName + ")" })
                .ToListAsync();

            viewModel.DanhSachGoiTap = await db.GoiTaps
                .Select(g => new SelectListItem { Value = g.Id.ToString(), Text = g.TenGoi })
                .ToListAsync();

            // Logic mới: Chỉ cần lấy các khuyến mãi đang hoạt động, không cần check ngày
            viewModel.DanhSachKhuyenMai = await db.KhuyenMais
                .Where(k => k.IsActive)
                .Select(k => new SelectListItem { Value = k.Id.ToString(), Text = k.TenKhuyenMai })
                .ToListAsync();

            viewModel.DanhSachKhuyenMai.ToList().Insert(0, new SelectListItem { Value = "", Text = "-- Không áp dụng --" });

            return View(viewModel);
        }

        // GET: HoaDons/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            // Dùng .Include() để lấy kèm thông tin của các bảng liên quan
            // Điều này rất quan trọng để tránh lỗi khi truy cập item.HoiVien, item.GoiTap...
            HoaDon hoaDon = db.HoaDons
                             .Include(h => h.HoiVien)
                             .Include(h => h.GoiTap)
                             .Include(h => h.KhuyenMai) // Lấy cả thông tin khuyến mãi nếu có
                             .FirstOrDefault(h => h.Id == id);

            if (hoaDon == null)
            {
                return HttpNotFound();
            }

            // Logic cốt lõi: Trả về PartialView nếu là request từ JavaScript
            if (Request.IsAjaxRequest())
            {
                return PartialView("Details", hoaDon);
            }

            // Trả về View đầy đủ nếu truy cập trực tiếp
            return View(hoaDon);
        }

        // --- CÁC ACTION HỖ TRỢ AJAX ---

        [HttpGet]
        public async Task<JsonResult> GetGoiTapInfo(int id)
        {
            var goiTap = await db.GoiTaps.FindAsync(id);
            if (goiTap == null) return Json(null, JsonRequestBehavior.AllowGet);
            return Json(new { giaGoc = goiTap.GiaTien }, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public async Task<JsonResult> GetKhuyenMaiDiscount(int id, decimal giaGoc)
        {
            var khuyenMai = await db.KhuyenMais.FindAsync(id);
            if (khuyenMai == null)
            {
                // Trả về 0 nếu không tìm thấy khuyến mãi
                return Json(new { soTienGiam = 0 }, JsonRequestBehavior.AllowGet);
            }

            // === LOGIC TÍNH TOÁN GIẢM GIÁ MỚI ===

            // 1. Tính số tiền giảm theo phần trăm
            decimal soTienGiamTheoPhanTram = (decimal)khuyenMai.PhanTramGiamGia * giaGoc / 100;

            // 2. Lấy số tiền giảm tối đa từ khuyến mãi
            decimal soTienGiamToiDa = khuyenMai.SoTienGiamToiDa;

            // 3. So sánh và chọn ra số tiền giảm cuối cùng
            decimal soTienGiamCuoiCung = soTienGiamTheoPhanTram;
            if (soTienGiamToiDa > 0 && soTienGiamTheoPhanTram > soTienGiamToiDa)
            {
                soTienGiamCuoiCung = soTienGiamToiDa;
            }

            return Json(new { soTienGiam = soTienGiamCuoiCung }, JsonRequestBehavior.AllowGet);
        }

        private async Task UpdateHoiVienRankAsync(string hoiVienId)
        {
            if (string.IsNullOrEmpty(hoiVienId))
            {
                return;
            }

            // 1. Tính tổng số tiền mà hội viên đã chi tiêu (chỉ tính các hóa đơn Đã Thanh Toán)
            decimal tongChiTieu = await db.HoaDons
                                          .Where(h => h.HoiVienId == hoiVienId && h.TrangThai == TrangThai.DaThanhToan)
                                          .SumAsync(h => (decimal?)h.ThanhTien) ?? 0;

            // 2. Tìm ra hạng cao nhất mà hội viên này đạt được với tổng chi tiêu đó
            var hangMoi = await db.HangHoiViens
                                  .Where(h => h.NguongChiTieu <= tongChiTieu)
                                  .OrderByDescending(h => h.NguongChiTieu)
                                  .FirstOrDefaultAsync();

            if (hangMoi == null)
            {
                // Trường hợp không tìm thấy hạng nào (ít xảy ra nếu có hạng Đồng với ngưỡng 0)
                return;
            }

            var hoivienProfile = await db.HoiViens.FirstOrDefaultAsync(h => h.ApplicationUserId == hoiVienId);
            if (hoivienProfile == null)
            {
                return;
            }

            if (hoivienProfile.HangHoiVienId != hangMoi.Id)
            {
                // Cập nhật trực tiếp trên hồ sơ hội viên
                hoivienProfile.HangHoiVienId = hangMoi.Id;
                await db.SaveChangesAsync();
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> MarkAsPaid(int hoaDonId)
        {
            var hoaDon = await db.HoaDons.Include(h => h.GoiTap).FirstOrDefaultAsync(h => h.Id == hoaDonId);
            if (hoaDon == null) return HttpNotFound();

            hoaDon.TrangThai = TrangThai.DaThanhToan;

            // === TẠO BẢN GHI ĐĂNG KÝ GÓI TẬP (TƯƠNG TỰ MOMOCONTROLLER) ===
            var goiTap = hoaDon.GoiTap;
            var hoivienProfile = await db.HoiViens.FirstOrDefaultAsync(h => h.ApplicationUserId == hoaDon.HoiVienId);

            if (goiTap != null && hoivienProfile != null)
            {
                var dangKy = new DangKyGoiTap
                {
                    HoiVienId = hoivienProfile.Id,
                    GoiTapId = goiTap.Id,
                    NgayDangKy = DateTime.Today,
                    NgayHetHan = DateTime.Today.AddDays(goiTap.SoBuoiTapVoiPT),
                    TrangThai = TrangThaiDangKy.HoatDong,
                    SoBuoiTapVoiPT = goiTap.SoBuoiTapVoiPT, 
                    SoBuoiPTDaSuDung = 0
                };
                db.DangKyGoiTaps.Add(dangKy);
            }
            // ========================================================

            await db.SaveChangesAsync();
            // await UpdateHoiVienRankAsync(hoaDon.HoiVienId);

            TempData["SuccessMessage"] = "Đã xác nhận thanh toán và kích hoạt gói tập!";
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}