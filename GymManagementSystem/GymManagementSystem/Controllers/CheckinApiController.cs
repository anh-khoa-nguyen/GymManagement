using System;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using GymManagementSystem.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;

namespace GymManagementSystem.Controllers
{
    [RoutePrefix("api/checkin")]
    [Authorize(Roles = "QuanLy,PT")] // Đảm bảo chỉ nhân viên mới có thể gọi API này
    public class CheckinApiController : Controller
    {
        // Sử dụng thuộc tính get-only để lấy instance một cách an toàn
        private ApplicationDbContext Db => HttpContext.GetOwinContext().Get<ApplicationDbContext>();
        private ApplicationUserManager UserManager => HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();

        [HttpGet]
        [Route("verify")]
        public async Task<JsonResult> Verify(string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                return Json(new { success = false, message = "Mã QR không hợp lệ." }, JsonRequestBehavior.AllowGet);
            }

            var user = await UserManager.FindByIdAsync(id);
            if (user == null)
            {
                return Json(new { success = false, message = "Không tìm thấy người dùng với mã này." }, JsonRequestBehavior.AllowGet);
            }

            // Khởi tạo các biến kết quả
            bool isAccessGranted = false;
            string logMessage = "";
            string tenGoiTap = null;
            DateTime? ngayHetHan = null;

            // Xử lý logic dựa trên vai trò của người được quét mã
            switch (user.VaiTro)
            {
                case "HoiVien":
                    var hoivienProfile = await Db.HoiViens.FirstOrDefaultAsync(h => h.ApplicationUserId == id);
                    if (hoivienProfile == null)
                    {
                        return Json(new { success = false, message = "Hội viên này chưa có hồ sơ chi tiết." }, JsonRequestBehavior.AllowGet);
                    }

                    var goiTapHienTai = await Db.DangKyGoiTaps
                        .Include(d => d.GoiTap)
                        .Where(d => d.HoiVienId == hoivienProfile.Id &&
                                    d.TrangThai == TrangThaiDangKy.HoatDong &&
                                    d.NgayHetHan >= DateTime.Today)
                        .OrderByDescending(d => d.NgayHetHan)
                        .FirstOrDefaultAsync();

                    isAccessGranted = goiTapHienTai != null;
                    logMessage = isAccessGranted ? "Check-in thành công." : "Thất bại: Gói tập không hợp lệ hoặc đã hết hạn.";
                    tenGoiTap = goiTapHienTai?.GoiTap.TenGoi;
                    ngayHetHan = goiTapHienTai?.NgayHetHan;
                    break;

                case "PT":
                case "QuanLy":
                    isAccessGranted = true;
                    logMessage = $"Nhân viên '{user.VaiTro}' chấm công thành công.";
                    break;

                default:
                    isAccessGranted = false;
                    logMessage = $"Thất bại: Vai trò '{user.VaiTro}' không được phép check-in.";
                    break;
            }

            // Tạo và lưu bản ghi log check-in
            var log = new LichSuCheckin
            {
                ApplicationUserId = user.Id,
                VaiTro = user.VaiTro, // Lưu lại vai trò tại thời điểm check-in
                ThoiGianCheckin = DateTime.Now,
                ThanhCong = isAccessGranted,
                GhiChu = logMessage
            };
            Db.LichSuCheckins.Add(log);
            await Db.SaveChangesAsync();

            // Trả về kết quả dưới dạng JSON cho frontend
            return Json(new
            {
                success = true, // Luôn là true vì đã tìm thấy người dùng
                accessGranted = isAccessGranted,
                hoTen = user.HoTen,
                email = user.Email,
                avatarUrl = user.AvatarUrl,
                vaiTro = user.VaiTro,
                tenGoiTap = tenGoiTap,
                ngayHetHan = ngayHetHan?.ToString("dd/MM/yyyy"),
                message = logMessage
            }, JsonRequestBehavior.AllowGet);
        }

        // Dispose DbContext và UserManager khi Controller bị hủy
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (UserManager != null)
                {
                    UserManager.Dispose();
                }
                // DbContext được quản lý bởi OwinContext nên không cần dispose thủ công ở đây
            }
            base.Dispose(disposing);
        }
    }
}