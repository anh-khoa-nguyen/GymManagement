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
        public ActionResult Create()
        {
            var viewModel = new TaoHoaDonViewModel
            {
                // Lấy danh sách hội viên (những người có vai trò "HoiVien")
                DanhSachHoiVien = db.Users
                                    .Where(u => u.VaiTro == "HoiVien")
                                    .Select(u => new SelectListItem { Value = u.Id, Text = u.HoTen + " (" + u.UserName + ")" })
                                    .ToList(),
                // Lấy danh sách gói tập
                DanhSachGoiTap = db.GoiTaps
                                   .Select(g => new SelectListItem { Value = g.Id.ToString(), Text = g.TenGoi })
                                   .ToList(),
                // Lấy danh sách khuyến mãi còn hoạt động
                DanhSachKhuyenMai = db.KhuyenMais
                                      .Where(k => k.IsActive && k.NgayKetThuc >= DateTime.Now)
                                      .Select(k => new SelectListItem { Value = k.Id.ToString(), Text = k.TenKhuyenMai })
                                      .ToList()
            };
            // Thêm một lựa chọn "Không áp dụng" vào đầu danh sách khuyến mãi
            viewModel.DanhSachKhuyenMai.ToList().Insert(0, new SelectListItem { Value = "", Text = "-- Không áp dụng --" });

            if (Request.IsAjaxRequest())
            {
                return PartialView("Create", viewModel);
            }


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
                    soTienGiam = (decimal)khuyenMai.PhanTramGiamGia * giaGoc / 100;
                    soTienGiam += khuyenMai.SoTienGiamGia;
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

            // Nếu model không hợp lệ, tải lại các danh sách và trả về view
            viewModel.DanhSachHoiVien = db.Users.Where(u => u.VaiTro == "HoiVien").Select(u => new SelectListItem { Value = u.Id, Text = u.HoTen + " (" + u.UserName + ")" }).ToList();
            viewModel.DanhSachGoiTap = db.GoiTaps.Select(g => new SelectListItem { Value = g.Id.ToString(), Text = g.TenGoi }).ToList();
            viewModel.DanhSachKhuyenMai = db.KhuyenMais.Where(k => k.IsActive && k.NgayKetThuc >= DateTime.Now).Select(k => new SelectListItem { Value = k.Id.ToString(), Text = k.TenKhuyenMai }).ToList();
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
            if (khuyenMai == null) return Json(null, JsonRequestBehavior.AllowGet);

            decimal soTienGiam = (decimal)khuyenMai.PhanTramGiamGia * giaGoc / 100;
            soTienGiam += khuyenMai.SoTienGiamGia;

            return Json(new { soTienGiam = soTienGiam }, JsonRequestBehavior.AllowGet);
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

            // 3. Lấy thông tin người dùng hiện tại để kiểm tra và cập nhật
            var user = await UserManager.FindByIdAsync(hoiVienId);

            if (user == null)
            {
                return;
            }

            // 4. Chỉ cập nhật nếu hạng mới cao hơn hạng hiện tại
            // (Tránh trường hợp bị hạ hạng hoặc cập nhật không cần thiết)
            var hangHienTai = await db.HangHoiViens.FindAsync(user.HangHoiVienId);

            if (hangHienTai == null || hangMoi.NguongChiTieu > hangHienTai.NguongChiTieu)
            {
                // 1. Cập nhật thuộc tính của đối tượng user
                user.HangHoiVienId = hangMoi.Id;

                // 2. Dùng UserManager để cập nhật người dùng vào CSDL
                var result = await UserManager.UpdateAsync(user);

                // (Tùy chọn) Kiểm tra kết quả và xử lý lỗi nếu cần
                if (result.Succeeded)
                {
                    // Gửi thông báo cho người dùng về việc được nâng hạng...
                }
                else
                {
                    // Ghi log lỗi hoặc xử lý vấn đề không cập nhật được
                }
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
                    TrangThai = TrangThaiDangKy.HoatDong
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