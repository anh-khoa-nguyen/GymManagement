using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using GymManagementSystem.Models;
using GymManagementSystem.Models.ViewModels;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;

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

        #region CR
        // GET: HoaDons
        public async Task<ActionResult> Index(string searchString)
        {
            var hoaDons = db.HoaDons
                .Include(h => h.GoiTap)
                .Include(h => h.HoiVien);
            if (!string.IsNullOrWhiteSpace(searchString))
            {
                hoaDons = hoaDons.Where(
                    h => h.HoiVien.HoTen.Contains(searchString) || h.HoiVien.Email.Contains(searchString));
            }
            ViewBag.CurrentFilter = searchString;
            return View(await hoaDons.OrderByDescending(h => h.NgayTao).ToListAsync());
        }

        // GET: HoaDons/Create
        public async Task<ActionResult> Create()
        {
            var viewModel = new TaoHoaDonViewModel
            {
                DanhSachHoiVien = await db.Users
                    .Where(u => u.VaiTro == "HoiVien")
                    .Select(u => new SelectListItem { Value = u.Id, Text = u.HoTen + " (" + u.Email + ")" })
                    .ToListAsync(),

                DanhSachGoiTap = await db.GoiTaps
                    .Select(g => new SelectListItem { Value = g.Id.ToString(), Text = g.TenGoi })
                    .ToListAsync(),

                DanhSachKhuyenMai = new List<SelectListItem> { new SelectListItem { Value = "", Text = "-- Chọn Hội viên và Gói tập --" } }
            };

            if (Request.IsAjaxRequest())
            {
                return PartialView("Create", viewModel);
            }
            return View(viewModel);
        }

        // POST: HoaDons/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(TaoHoaDonViewModel viewModel)
        {
            if (ModelState.IsValid)
            {
                var goiTap = await db.GoiTaps.FindAsync(viewModel.GoiTapId);
                KhuyenMai khuyenMai = null;
                if (viewModel.KhuyenMaiId.HasValue)
                {
                    khuyenMai = await db.KhuyenMais.FindAsync(viewModel.KhuyenMaiId);
                }

                decimal giaGoc = goiTap.GiaTien;
                decimal soTienGiam = 0;
                if (khuyenMai != null)
                {
                    decimal soTienGiamTheoPhanTram = (decimal)khuyenMai.PhanTramGiamGia * giaGoc / 100;
                    decimal soTienGiamToiDa = khuyenMai.SoTienGiamToiDa; // <-- Sử dụng trường mới

                    soTienGiam = soTienGiamTheoPhanTram;
                    if (soTienGiamToiDa > 0 && soTienGiamTheoPhanTram > soTienGiamToiDa)
                    {
                        soTienGiam = soTienGiamToiDa;
                    }
                }
                decimal thanhTien = giaGoc - soTienGiam;

                var hoaDon = new HoaDon
                {
                    HoiVienId = viewModel.HoiVienId,
                    GoiTapId = viewModel.GoiTapId,
                    KhuyenMaiId = viewModel.KhuyenMaiId,
                    NgayTao = DateTime.Now,
                    GiaGoc = giaGoc,
                    SoTienGiam = soTienGiam,
                    ThanhTien = thanhTien,
                    TrangThai = TrangThai.ChoThanhToan
                };

                db.HoaDons.Add(hoaDon);
                await db.SaveChangesAsync();
                if (Request.IsAjaxRequest())
                {
                    return Json(new { success = true });
                }
                return RedirectToAction("Index");
            }
            return View(viewModel);
        }

        // GET: HoaDons/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            HoaDon hoaDon = db.HoaDons
                             .Include(h => h.HoiVien)
                             .Include(h => h.GoiTap)
                             .Include(h => h.KhuyenMai)
                             .FirstOrDefault(h => h.Id == id);

            if (hoaDon == null) return HttpNotFound();

            if (Request.IsAjaxRequest())
            {
                return PartialView("Details", hoaDon);
            }

            return View(hoaDon);
        }

        #endregion

        private async Task PopulateTaoHoaDonViewModel(TaoHoaDonViewModel viewModel)
        {
            viewModel.DanhSachHoiVien = await db.Users
                .Where(u => u.VaiTro == "HoiVien")
                .Select(u => new SelectListItem { Value = u.Id, Text = u.HoTen + " (" + u.Email + ")" })
                .ToListAsync();

            viewModel.DanhSachGoiTap = await db.GoiTaps
                .Select(g => new SelectListItem { Value = g.Id.ToString(), Text = g.TenGoi })
                .ToListAsync();

            var danhSachKhuyenMai = await db.KhuyenMais
                .Where(k => k.IsActive)
                .Select(k => new SelectListItem { Value = k.Id.ToString(), Text = k.TenKhuyenMai })
                .ToListAsync();
            danhSachKhuyenMai.Insert(0, new SelectListItem { Value = "", Text = "-- Không áp dụng --" });
            viewModel.DanhSachKhuyenMai = danhSachKhuyenMai;
        }

        #region Support AJAX
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
                return Json(new { soTienGiam = 0 }, JsonRequestBehavior.AllowGet);
            }

            decimal soTienGiamTheoPhanTram = (decimal)khuyenMai.PhanTramGiamGia * giaGoc / 100;
            decimal soTienGiamToiDa = khuyenMai.SoTienGiamToiDa;
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
                var dangKyHienTai = await db.DangKyGoiTaps
                    .FirstOrDefaultAsync(d => d.HoiVienId == hoivienProfile.Id &&
                                              d.GoiTapId == goiTap.Id &&
                                              d.TrangThai == TrangThaiDangKy.HoatDong);
                if (dangKyHienTai != null)
                {
                    DateTime ngayBatDauCongDon = dangKyHienTai.NgayHetHan > DateTime.Today
                                                ? dangKyHienTai.NgayHetHan
                                                : DateTime.Today;
                    dangKyHienTai.NgayHetHan = ngayBatDauCongDon.AddDays(goiTap.SoBuoiTapVoiPT);
                }
                else
                {
                    var dangKyMoi = new DangKyGoiTap
                    {
                        HoiVienId = hoivienProfile.Id,
                        GoiTapId = goiTap.Id,
                        NgayDangKy = DateTime.Today,
                        NgayHetHan = DateTime.Today.AddDays(goiTap.SoBuoiTapVoiPT),
                        TrangThai = TrangThaiDangKy.HoatDong
                    };
                    db.DangKyGoiTaps.Add(dangKyMoi);
                }
            }

            await db.SaveChangesAsync();
            // await UpdateHoiVienRankAsync(hoaDon.HoiVienId);

            TempData["SuccessMessage"] = "Đã xác nhận thanh toán và kích hoạt gói tập!";
            Debug.WriteLine("Hello");
            return RedirectToAction("Index");
        }

        [HttpGet]
        public async Task<JsonResult> GetAvailableVouchers(string hoiVienId, int goiTapId)
        {
            if (string.IsNullOrEmpty(hoiVienId) || goiTapId == 0)
            {
                return Json(new List<SelectListItem>(), JsonRequestBehavior.AllowGet);
            }

            var hoivienProfile = await db.HoiViens.FirstOrDefaultAsync(h => h.ApplicationUserId == hoiVienId);
            if (hoivienProfile == null)
            {
                return Json(new List<SelectListItem>(), JsonRequestBehavior.AllowGet);
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
                Value = v.KhuyenMaiId.ToString(),
                Text = v.KhuyenMai.TenKhuyenMai
            }).ToList();

            return Json(validVouchers, JsonRequestBehavior.AllowGet);
        }
        #endregion

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