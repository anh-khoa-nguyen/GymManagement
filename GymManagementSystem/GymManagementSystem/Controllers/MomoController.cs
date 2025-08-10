// Trong file Controllers/MomoController.cs
using System;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using GymManagementSystem;
using GymManagementSystem.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;

public class MomoController : Controller
{
    private readonly MomoService _momoService = new MomoService();
    private readonly ApplicationDbContext db = new ApplicationDbContext();
    private ApplicationUserManager _userManager;
    
    #region Tạo thanh toán MoMo --- Xác nhận thanh toán --- Trả về kết quả
    // GET: /Momo/CreatePaymentRequest?hoaDonId=5
    // Action này được gọi khi người dùng nhấn vào link "Thanh toán MoMo"
    public async Task<ActionResult> CreatePaymentRequest(int hoaDonId)
    {
        var hoaDon = await db.HoaDons.FindAsync(hoaDonId);
        if (hoaDon == null || hoaDon.TrangThai == TrangThai.DaThanhToan)
        {
            TempData["ErrorMessage"] = "Hóa đơn không hợp lệ hoặc đã được thanh toán.";
            return RedirectToAction("Index", "HoaDons");
        }

        var orderInfo = $"Thanh toan hoa don #{hoaDon.Id}";
        var payUrl = await _momoService.CreatePaymentUrlAsync(hoaDon.Id, hoaDon.ThanhTien, orderInfo);

        if (!string.IsNullOrEmpty(payUrl))
        {
            return Redirect(payUrl);
        }
        else
        {
            TempData["ErrorMessage"] = "Không thể tạo yêu cầu thanh toán MoMo. Vui lòng thử lại sau.";
            return RedirectToAction("Index", "HoaDons");
        }
    }

    // MoMo gọi về để xác nhận thanh toán (IPN)
    [HttpPost]
    public async Task<ActionResult> ConfirmPayment(/* Tham số MoMo gửi về */ int resultCode, string orderId)
    {
        if (resultCode == 0)
        {
            var hoaDon = await db.HoaDons.Include(h => h.GoiTap)
                                     .FirstOrDefaultAsync(h => h.MomoOrderId == orderId);

            if (hoaDon != null && hoaDon.TrangThai == TrangThai.ChoThanhToan)
            {
                hoaDon.TrangThai = TrangThai.DaThanhToan;
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

                if (hoaDon.KhuyenMaiId.HasValue)
                {
                    var voucherDaSuDung = await db.KhuyenMaiCuaHoiViens
                        .FirstOrDefaultAsync(v => v.HoiVienId == hoivienProfile.Id &&
                                                  v.KhuyenMaiId == hoaDon.KhuyenMaiId.Value &&
                                                  v.TrangThai == TrangThaiKhuyenMaiHV.ChuaSuDung);

                    if (voucherDaSuDung != null)
                    {
                        voucherDaSuDung.TrangThai = TrangThaiKhuyenMaiHV.DaSuDung;
                    }
                }

                await db.SaveChangesAsync();
                await UpdateHoiVienRankAsync(hoaDon.HoiVienId);
            }
        }

        // Phản hồi lại MoMo
        return new HttpStatusCodeResult(204);
    }

    // Action này là trang người dùng được chuyển hướng về sau khi thanh toán - Giữ nguyên
    public ActionResult PaymentResult()
    {
        ViewBag.Message = "Xin chúc mừng, bạn đã thanh toán thành công. Vui lòng kiểm tra lại trạng thái hóa đơn và thông tin gói tập.";
        return View();
    }
    #endregion

    #region Nâng hạng Hội viên
    private async Task UpdateHoiVienRankAsync(string hoiVienId)
    {
        if (string.IsNullOrEmpty(hoiVienId)) return;

        decimal tongChiTieu = await db.HoaDons
                                      .Where(h => h.HoiVienId == hoiVienId && h.TrangThai == TrangThai.DaThanhToan)
                                      .SumAsync(h => (decimal?)h.ThanhTien) ?? 0;

        var hangMoi = await db.HangHoiViens
                              .Where(h => h.NguongChiTieu <= tongChiTieu)
                              .OrderByDescending(h => h.NguongChiTieu)
                              .FirstOrDefaultAsync();

        if (hangMoi == null) return;

        var hoivienProfile = await db.HoiViens.FirstOrDefaultAsync(h => h.ApplicationUserId == hoiVienId);
        if (hoivienProfile == null)
        {
            return;
        }

        int? oldRankId = hoivienProfile.HangHoiVienId;
        int newRankId = hangMoi.Id;

        if (oldRankId != newRankId)
        {
            hoivienProfile.HangHoiVienId = newRankId;
            await db.SaveChangesAsync();

            // GỌI SERVICE ĐỂ TẶNG ĐẶC QUYỀN
            var privilegeService = new PrivilegeService(db);
            await privilegeService.GrantPrivilegesOnRankUp(hoiVienId, newRankId);
        }
    }
    #endregion
}