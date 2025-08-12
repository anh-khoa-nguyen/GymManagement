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

    // GET: /Momo/CreatePaymentRequest?hoaDonId=5
    // Action này được gọi khi người dùng nhấn vào link "Thanh toán MoMo"
    public async Task<ActionResult> CreatePaymentRequest(int hoaDonId)
    {
        var hoaDon = await db.HoaDons.FindAsync(hoaDonId);
        if (hoaDon == null || hoaDon.TrangThai == TrangThai.DaThanhToan)
        {
            // Nếu hóa đơn không hợp lệ, quay lại trang danh sách với thông báo lỗi
            TempData["ErrorMessage"] = "Hóa đơn không hợp lệ hoặc đã được thanh toán.";
            return RedirectToAction("Index", "HoaDons");
        }

        var orderInfo = $"Thanh toan hoa don #{hoaDon.Id}";
        var payUrl = await _momoService.CreatePaymentUrlAsync(hoaDon.Id, hoaDon.ThanhTien, orderInfo);

        if (!string.IsNullOrEmpty(payUrl))
        {
            // Nếu tạo URL thành công, chuyển hướng người dùng thẳng đến trang thanh toán của MoMo
            return Redirect(payUrl);
        }
        else
        {
            // Nếu có lỗi khi tạo URL, quay lại trang danh sách với thông báo lỗi
            TempData["ErrorMessage"] = "Không thể tạo yêu cầu thanh toán MoMo. Vui lòng thử lại sau.";
            return RedirectToAction("Index", "HoaDons");
        }
    }

    // Action này được MoMo gọi về để xác nhận thanh toán (IPN) - Giữ nguyên
    [HttpPost]
    public async Task<ActionResult> ConfirmPayment(/* Tham số MoMo gửi về */ int resultCode, string orderId)
    {
        if (resultCode == 0) // Giao dịch thành công
        {
            var hoaDon = await db.HoaDons.Include(h => h.GoiTap) // <-- Thêm .Include()
                                     .FirstOrDefaultAsync(h => h.MomoOrderId == orderId);

            // 2. Nếu tìm thấy hóa đơn và nó đang ở trạng thái chờ
            if (hoaDon != null && hoaDon.TrangThai == TrangThai.ChoThanhToan)
            {
                // 3. Cập nhật trạng thái hóa đơn
                hoaDon.TrangThai = TrangThai.DaThanhToan;
                var goiTap = hoaDon.GoiTap; // Lấy thông tin gói tập từ hóa đơn
                var hoivienProfile = await db.HoiViens.FirstOrDefaultAsync(h => h.ApplicationUserId == hoaDon.HoiVienId);
                if (goiTap != null && hoivienProfile != null)
                {
                    var dangKy = new DangKyGoiTap
                    {
                        HoiVienId = hoivienProfile.Id, // <-- DÙNG ID TỪ BẢNG HOIVIENS
                        GoiTapId = goiTap.Id,
                        // HoaDonId không có trong model của bạn, có thể bỏ qua
                        NgayDangKy = DateTime.Today,
                        NgayHetHan = DateTime.Today.AddDays(goiTap.SoBuoiTapVoiPT),
                        TrangThai = TrangThaiDangKy.HoatDong,
                        SoBuoiTapVoiPT = goiTap.SoBuoiTapVoiPT, 
                        SoBuoiPTDaSuDung = 0
                    };
                    db.DangKyGoiTaps.Add(dangKy);
                }

                if (hoaDon.KhuyenMaiId.HasValue)
                {
                    // Tìm voucher cụ thể mà hội viên đã dùng cho hóa đơn này
                    // (Dựa trên loại khuyến mãi và người sở hữu)
                    var voucherDaSuDung = await db.KhuyenMaiCuaHoiViens
                        .FirstOrDefaultAsync(v => v.HoiVienId == hoivienProfile.Id &&
                                                  v.KhuyenMaiId == hoaDon.KhuyenMaiId.Value &&
                                                  v.TrangThai == TrangThaiKhuyenMaiHV.ChuaSuDung);

                    if (voucherDaSuDung != null)
                    {
                        // Đổi trạng thái của nó thành "Đã sử dụng"
                        voucherDaSuDung.TrangThai = TrangThaiKhuyenMaiHV.DaSuDung;
                    }
                }

                await db.SaveChangesAsync();

                // 4. (Tích hợp) Gọi hàm nâng hạng cho hội viên
                await UpdateHoiVienRankAsync(hoaDon.HoiVienId);
            }
        }

        // Phản hồi cho MoMo biết đã nhận được thông tin để MoMo không gửi lại IPN nữa
        return new HttpStatusCodeResult(204);
    }

    // Action này là trang người dùng được chuyển hướng về sau khi thanh toán - Giữ nguyên
    public ActionResult PaymentResult()
    {
        ViewBag.Message = "Giao dịch của bạn đang được xử lý. Vui lòng kiểm tra lại trạng thái hóa đơn sau ít phút.";
        return View();
    }

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

        if (hoivienProfile.HangHoiVienId != hangMoi.Id)
        {
            // Cập nhật trực tiếp trên hồ sơ hội viên
            hoivienProfile.HangHoiVienId = hangMoi.Id;
            await db.SaveChangesAsync();
        }
    }
}