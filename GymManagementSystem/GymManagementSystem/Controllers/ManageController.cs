using System;
using System.Data.Entity; 
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using GymManagementSystem.Models;
using GymManagementSystem.Models.ViewModels;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;
//Khoa
using QRCoder; // Và using này
using System.Collections.Generic;


namespace GymManagementSystem.Controllers
{
    [Authorize]
    public class ManageController : Controller
    {
        private ApplicationSignInManager _signInManager;
        private ApplicationUserManager _userManager;
        private ApplicationDbContext db = new ApplicationDbContext();

        public ManageController()
        {
        }

        public ManageController(ApplicationUserManager userManager, ApplicationSignInManager signInManager)
        {
            UserManager = userManager;
            SignInManager = signInManager;
        }

        public ApplicationSignInManager SignInManager
        {
            get
            {
                return _signInManager ?? HttpContext.GetOwinContext().Get<ApplicationSignInManager>();
            }
            private set 
            { 
                _signInManager = value; 
            }
        }

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

        //
        // GET: /Manage/Index
        public async Task<ActionResult> Index(ManageMessageId? message)
        {
            ViewBag.StatusMessage =
                message == ManageMessageId.ChangePasswordSuccess ? "Your password has been changed."
                : message == ManageMessageId.SetPasswordSuccess ? "Your password has been set."
                : message == ManageMessageId.SetTwoFactorSuccess ? "Your two-factor authentication provider has been set."
                : message == ManageMessageId.Error ? "An error has occurred."
                : message == ManageMessageId.AddPhoneSuccess ? "Your phone number was added."
                : message == ManageMessageId.RemovePhoneSuccess ? "Your phone number was removed."
                : "";

            // === BẮT ĐẦU LOGIC HỢP NHẤT ===
            var userId = User.Identity.GetUserId();
            var user = await UserManager.FindByIdAsync(userId);
            if (user == null)
            {
                return View("Error");
            }
            
            string maGioiThieu = null;

            if (User.IsInRole("HoiVien"))
            {
                // Giả sử bạn có một bảng HoiViens liên kết với ApplicationUser qua một khóa ngoại
                // và bạn cần lấy thông tin từ bảng đó.
                // Nếu thông tin đã có sẵn trong 'user' thì dùng user.MaGioiThieu là đủ.
                var hoivienProfile = await db.HoiViens.FirstOrDefaultAsync(h => h.ApplicationUserId == userId);
                if (hoivienProfile != null)
                {
                    maGioiThieu = hoivienProfile.MaGioiThieu;
                }
            }

            // 1. Lấy thông tin Hạng Thành Viên
            var tongChiTieu = db.HoaDons
                                .Where(h => h.HoiVienId == userId && h.TrangThai == TrangThai.DaThanhToan)
                                .Sum(h => (decimal?)h.ThanhTien) ?? 0;
            var cacHang = db.HangHoiViens.OrderBy(h => h.NguongChiTieu).ToList(); 
            var hangHienTai = cacHang.LastOrDefault(h => h.NguongChiTieu <= tongChiTieu); 
            var hangTiepTheo = cacHang.FirstOrDefault(h => h.NguongChiTieu > tongChiTieu); 

            var membershipInfo = new HangThanhVienViewModel
            {
                TongChiTieu = tongChiTieu,
                HangHienTai = hangHienTai,
                HangTiepTheo = hangTiepTheo
            };
            if (hangTiepTheo != null && hangHienTai != null)
            {
                var chiTieuDaDat = tongChiTieu - hangHienTai.NguongChiTieu;
                var chiTieuCanDat = hangTiepTheo.NguongChiTieu - hangHienTai.NguongChiTieu;
                membershipInfo.ChiTieuCanThem = hangTiepTheo.NguongChiTieu - tongChiTieu;
                membershipInfo.PhanTramHoanThanh = chiTieuCanDat > 0 ? (int)((chiTieuDaDat / chiTieuCanDat) * 100) : 100;
            }

            // 2. Lấy thông tin Lịch Sử Giới Thiệu
            var nguoiDuocGioiThieu = db.Users.Where(u => u.NguoiGioiThieuId == userId).ToList();
            var referralInfo = new LichSuGioiThieuViewModel
            {
                DanhSachNguoiDuocGioiThieu = nguoiDuocGioiThieu.Select(u => new NguoiDuocGioiThieuItem
                {
                    HoTen = u.HoTen, // Giả sử ApplicationUser có thuộc tính HoTen
                    Email = u.Email,
                    //NgayThamGia = u.NgayTao // Giả sử ApplicationUser có thuộc tính NgayTao
                }).ToList()
                // Logic tính các mốc thưởng có thể thêm vào đây nếu cần
            };

            // 3. Tạo mã QR
            string qrCodeUri = "";
            QRCodeGenerator qrGenerator = new QRCodeGenerator();
            QRCodeData qrCodeData = qrGenerator.CreateQrCode(userId, QRCodeGenerator.ECCLevel.Q);
            using (PngByteQRCode qrCode = new PngByteQRCode(qrCodeData))
            {
                byte[] qrCodeAsPng = qrCode.GetGraphic(20);
                string base64String = System.Convert.ToBase64String(qrCodeAsPng);
                qrCodeUri = "data:image/png;base64," + base64String;
            }

            //if (User.IsInRole("HoiVien"))
            //{
            //    var hoivienProfile = await db.HoiViens.FirstOrDefaultAsync(h => h.ApplicationUserId == userId);
            //    if (hoivienProfile != null)
            //    {
            //        maGioiThieu = hoivienProfile.MaGioiThieu;
            //    }
            //}

            // 4. Tạo ViewModel "mẹ" và gán dữ liệu
            var model = new UserProfileViewModel
            {
                UserName = user.HoTen, // Giả sử bạn có thuộc tính HoTen
                Email = user.Email,
                MaGioiThieu = maGioiThieu,
                QrCodeUri = qrCodeUri,
                MembershipInfo = membershipInfo,
                ReferralInfo = referralInfo
            };

            return View(model);

            //var userId = User.Identity.GetUserId();
            //string maGioiThieu = null;

            //if (User.IsInRole("HoiVien"))
            //{
            //    var hoivienProfile = await db.HoiViens.FirstOrDefaultAsync(h => h.ApplicationUserId == userId);
            //    if (hoivienProfile != null)
            //    {
            //        maGioiThieu = hoivienProfile.MaGioiThieu;
            //    }
            //}

            //var model = new IndexViewModel
            //{
            //    HasPassword = HasPassword(),
            //    PhoneNumber = await UserManager.GetPhoneNumberAsync(userId),
            //    TwoFactor = await UserManager.GetTwoFactorEnabledAsync(userId),
            //    Logins = await UserManager.GetLoginsAsync(userId),
            //    BrowserRemembered = await AuthenticationManager.TwoFactorBrowserRememberedAsync(userId),
            //    MaGioiThieu = maGioiThieu
            //};
            //return View(model);
        }

        // GET: /Manage/EditProfile
        public ActionResult Edit()
        {
            // Tạo một ViewModel riêng cho trang này nếu cần, hoặc chỉ cần trả về View
            var userId = User.Identity.GetUserId();
            var model = new IndexViewModel { HasPassword = HasPassword() }; // Tận dụng lại ViewModel cũ
            return View(model);
        }

        //
        // POST: /Manage/RemoveLogin
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> RemoveLogin(string loginProvider, string providerKey)
        {
            ManageMessageId? message;
            var result = await UserManager.RemoveLoginAsync(User.Identity.GetUserId(), new UserLoginInfo(loginProvider, providerKey));
            if (result.Succeeded)
            {
                var user = await UserManager.FindByIdAsync(User.Identity.GetUserId());
                if (user != null)
                {
                    await SignInManager.SignInAsync(user, isPersistent: false, rememberBrowser: false);
                }
                message = ManageMessageId.RemoveLoginSuccess;
            }
            else
            {
                message = ManageMessageId.Error;
            }
            return RedirectToAction("ManageLogins", new { Message = message });
        }

        //
        // GET: /Manage/AddPhoneNumber
        public ActionResult AddPhoneNumber()
        {
            return View();
        }

        //
        // POST: /Manage/AddPhoneNumber
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> AddPhoneNumber(AddPhoneNumberViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            // Generate the token and send it
            var code = await UserManager.GenerateChangePhoneNumberTokenAsync(User.Identity.GetUserId(), model.Number);
            if (UserManager.SmsService != null)
            {
                var message = new IdentityMessage
                {
                    Destination = model.Number,
                    Body = "Your security code is: " + code
                };
                await UserManager.SmsService.SendAsync(message);
            }
            return RedirectToAction("VerifyPhoneNumber", new { PhoneNumber = model.Number });
        }

        //
        // POST: /Manage/EnableTwoFactorAuthentication
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> EnableTwoFactorAuthentication()
        {
            await UserManager.SetTwoFactorEnabledAsync(User.Identity.GetUserId(), true);
            var user = await UserManager.FindByIdAsync(User.Identity.GetUserId());
            if (user != null)
            {
                await SignInManager.SignInAsync(user, isPersistent: false, rememberBrowser: false);
            }
            return RedirectToAction("Index", "Manage");
        }

        //
        // POST: /Manage/DisableTwoFactorAuthentication
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DisableTwoFactorAuthentication()
        {
            await UserManager.SetTwoFactorEnabledAsync(User.Identity.GetUserId(), false);
            var user = await UserManager.FindByIdAsync(User.Identity.GetUserId());
            if (user != null)
            {
                await SignInManager.SignInAsync(user, isPersistent: false, rememberBrowser: false);
            }
            return RedirectToAction("Index", "Manage");
        }

        //
        // GET: /Manage/VerifyPhoneNumber
        public async Task<ActionResult> VerifyPhoneNumber(string phoneNumber)
        {
            var code = await UserManager.GenerateChangePhoneNumberTokenAsync(User.Identity.GetUserId(), phoneNumber);
            // Send an SMS through the SMS provider to verify the phone number
            return phoneNumber == null ? View("Error") : View(new VerifyPhoneNumberViewModel { PhoneNumber = phoneNumber });
        }

        //
        // POST: /Manage/VerifyPhoneNumber
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> VerifyPhoneNumber(VerifyPhoneNumberViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            var result = await UserManager.ChangePhoneNumberAsync(User.Identity.GetUserId(), model.PhoneNumber, model.Code);
            if (result.Succeeded)
            {
                var user = await UserManager.FindByIdAsync(User.Identity.GetUserId());
                if (user != null)
                {
                    await SignInManager.SignInAsync(user, isPersistent: false, rememberBrowser: false);
                }
                return RedirectToAction("Index", new { Message = ManageMessageId.AddPhoneSuccess });
            }
            // If we got this far, something failed, redisplay form
            ModelState.AddModelError("", "Failed to verify phone");
            return View(model);
        }

        //
        // POST: /Manage/RemovePhoneNumber
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> RemovePhoneNumber()
        {
            var result = await UserManager.SetPhoneNumberAsync(User.Identity.GetUserId(), null);
            if (!result.Succeeded)
            {
                return RedirectToAction("Index", new { Message = ManageMessageId.Error });
            }
            var user = await UserManager.FindByIdAsync(User.Identity.GetUserId());
            if (user != null)
            {
                await SignInManager.SignInAsync(user, isPersistent: false, rememberBrowser: false);
            }
            return RedirectToAction("Index", new { Message = ManageMessageId.RemovePhoneSuccess });
        }

        //
        // GET: /Manage/ChangePassword
        public ActionResult ChangePassword()
        {
            return View();
        }

        //
        // POST: /Manage/ChangePassword
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> ChangePassword(ChangePasswordViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            var result = await UserManager.ChangePasswordAsync(User.Identity.GetUserId(), model.OldPassword, model.NewPassword);
            if (result.Succeeded)
            {
                var user = await UserManager.FindByIdAsync(User.Identity.GetUserId());
                if (user != null)
                {
                    await SignInManager.SignInAsync(user, isPersistent: false, rememberBrowser: false);
                }
                return RedirectToAction("Index", new { Message = ManageMessageId.ChangePasswordSuccess });
            }
            AddErrors(result);
            return View(model);
        }

        //
        // GET: /Manage/SetPassword
        public ActionResult SetPassword()
        {
            return View();
        }

        //
        // POST: /Manage/SetPassword
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> SetPassword(SetPasswordViewModel model)
        {
            if (ModelState.IsValid)
            {
                var result = await UserManager.AddPasswordAsync(User.Identity.GetUserId(), model.NewPassword);
                if (result.Succeeded)
                {
                    var user = await UserManager.FindByIdAsync(User.Identity.GetUserId());
                    if (user != null)
                    {
                        await SignInManager.SignInAsync(user, isPersistent: false, rememberBrowser: false);
                    }
                    return RedirectToAction("Index", new { Message = ManageMessageId.SetPasswordSuccess });
                }
                AddErrors(result);
            }

            // If we got this far, something failed, redisplay form
            return View(model);
        }

        //
        // GET: /Manage/ManageLogins
        public async Task<ActionResult> ManageLogins(ManageMessageId? message)
        {
            ViewBag.StatusMessage =
                message == ManageMessageId.RemoveLoginSuccess ? "The external login was removed."
                : message == ManageMessageId.Error ? "An error has occurred."
                : "";
            var user = await UserManager.FindByIdAsync(User.Identity.GetUserId());
            if (user == null)
            {
                return View("Error");
            }
            var userLogins = await UserManager.GetLoginsAsync(User.Identity.GetUserId());
            var otherLogins = AuthenticationManager.GetExternalAuthenticationTypes().Where(auth => userLogins.All(ul => auth.AuthenticationType != ul.LoginProvider)).ToList();
            ViewBag.ShowRemoveButton = user.PasswordHash != null || userLogins.Count > 1;
            return View(new ManageLoginsViewModel
            {
                CurrentLogins = userLogins,
                OtherLogins = otherLogins
            });
        }

        //
        // POST: /Manage/LinkLogin
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult LinkLogin(string provider)
        {
            // Request a redirect to the external login provider to link a login for the current user
            return new AccountController.ChallengeResult(provider, Url.Action("LinkLoginCallback", "Manage"), User.Identity.GetUserId());
        }

        //
        // GET: /Manage/LinkLoginCallback
        public async Task<ActionResult> LinkLoginCallback()
        {
            var loginInfo = await AuthenticationManager.GetExternalLoginInfoAsync(XsrfKey, User.Identity.GetUserId());
            if (loginInfo == null)
            {
                return RedirectToAction("ManageLogins", new { Message = ManageMessageId.Error });
            }
            var result = await UserManager.AddLoginAsync(User.Identity.GetUserId(), loginInfo.Login);
            return result.Succeeded ? RedirectToAction("ManageLogins") : RedirectToAction("ManageLogins", new { Message = ManageMessageId.Error });
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing && _userManager != null)
            {
                _userManager.Dispose();
                _userManager = null;
            }

            base.Dispose(disposing);
        }

        //Khoa

        // GET: /Manage/ShowQrCode
        public ActionResult ShowQrCode()
        {
            // 1. Lấy ID của người dùng đang đăng nhập
            string userId = User.Identity.GetUserId();

            // 2. Sử dụng thư viện QRCoder để tạo mã QR từ userId
            QRCodeGenerator qrGenerator = new QRCodeGenerator();
            QRCodeData qrCodeData = qrGenerator.CreateQrCode(userId, QRCodeGenerator.ECCLevel.Q);
            PngByteQRCode qrCode = new PngByteQRCode(qrCodeData);
            byte[] qrCodeAsPngByteArr = qrCode.GetGraphic(20);

            // 3. Chuyển byte array thành chuỗi Base64 để hiển thị trong thẻ <img>
            string qrCodeAsBase64 = "data:image/png;base64," + Convert.ToBase64String(qrCodeAsPngByteArr);

            // 4. Gửi chuỗi Base64 này tới View
            ViewBag.QrCodeUri = qrCodeAsBase64;

            return View();
        }

        public async Task<ActionResult> HangThanhVien()
        {
            // 1. Lấy thông tin người dùng đang đăng nhập
            var userId = User.Identity.GetUserId();
            var user = await UserManager.FindByIdAsync(userId);
            if (user == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            // 2. Tính tổng chi tiêu của người dùng
            decimal tongChiTieu = await db.HoaDons
                                          .Where(h => h.HoiVienId == userId && h.TrangThai == TrangThai.DaThanhToan)
                                          .SumAsync(h => (decimal?)h.ThanhTien) ?? 0;

            // 3. Lấy tất cả các hạng và sắp xếp theo ngưỡng để xử lý
            var allRanks = await db.HangHoiViens.OrderBy(h => h.NguongChiTieu).ToListAsync();

            // 4. Tìm hạng hiện tại và hạng tiếp theo
            var hangHienTai = allRanks.Where(h => h.NguongChiTieu <= tongChiTieu).LastOrDefault();
            var hangTiepTheo = allRanks.FirstOrDefault(h => h.NguongChiTieu > tongChiTieu);

            // 5. Tính toán các số liệu cho ViewModel
            decimal chiTieuCanThem = 0;
            int phanTramHoanThanh = 0;

            if (hangTiepTheo != null) // Nếu có hạng tiếp theo để phấn đấu
            {
                chiTieuCanThem = hangTiepTheo.NguongChiTieu - tongChiTieu;

                // Tính % cho thanh tiến trình
                decimal nguongTruoc = hangHienTai?.NguongChiTieu ?? 0;
                decimal khoangCachNguong = hangTiepTheo.NguongChiTieu - nguongTruoc;

                if (khoangCachNguong > 0)
                {
                    decimal phanTram = ((tongChiTieu - nguongTruoc) / khoangCachNguong) * 100;
                    phanTramHoanThanh = (int)phanTram;
                }
            }
            else // Đã đạt hạng cao nhất
            {
                phanTramHoanThanh = 100;
            }

            // 6. Tạo ViewModel và gán dữ liệu
            var viewModel = new HangThanhVienViewModel
            {
                TenHoiVien = user.HoTen,
                TongChiTieu = tongChiTieu,
                HangHienTai = hangHienTai,
                HangTiepTheo = hangTiepTheo,
                ChiTieuCanThem = chiTieuCanThem,
                PhanTramHoanThanh = phanTramHoanThanh
            };

            return View(viewModel);
        }

        // GET: /Manage/LichSuGioiThieu
        public async Task<ActionResult> LichSuGioiThieu()
        {
            var userId = User.Identity.GetUserId();

            // 1. Lấy danh sách những người đã được người dùng này giới thiệu
            var danhSachNguoiDuocGioiThieu = await UserManager.Users
                .Where(u => u.NguoiGioiThieuId == userId)
                .Select(u => new NguoiDuocGioiThieuItem
                {
                    HoTen = u.HoTen,
                    Email = u.Email,
                    // LockoutEndDateUtc có thể được dùng tạm để lưu ngày tạo, hoặc bạn cần thêm trường NgayTao vào ApplicationUser
                    NgayThamGia = u.LockoutEndDateUtc ?? System.DateTime.MinValue
                })
                .OrderByDescending(u => u.NgayThamGia)
                .ToListAsync();

            int tongSoNguoi = danhSachNguoiDuocGioiThieu.Count;

            // 2. Định nghĩa các mốc thưởng (nên được lấy từ CSDL trong thực tế)
            var cacMocThuong = new Dictionary<int, string>
            {
                { 5, "Khuyến mãi 10%" },
                { 10, "Khuyến mãi 20%" },
                { 15, "1 tháng miễn phí" }
            };

            // 3. Tìm mốc thưởng tiếp theo
            int mocThuongTiepTheo = 0;
            foreach (var moc in cacMocThuong.Keys.OrderBy(k => k))
            {
                if (tongSoNguoi < moc)
                {
                    mocThuongTiepTheo = moc;
                    break;
                }
            }

            // 4. Tạo ViewModel
            var viewModel = new LichSuGioiThieuViewModel
            {
                TongSoNguoiDaGioiThieu = tongSoNguoi,
                DanhSachNguoiDuocGioiThieu = danhSachNguoiDuocGioiThieu,
                CacMocThuong = cacMocThuong,
                MocThuongTiepTheo = mocThuongTiepTheo,
                SoNguoiCanThem = mocThuongTiepTheo > 0 ? mocThuongTiepTheo - tongSoNguoi : 0
            };

            return View(viewModel);
        }

        #region Helpers
        // Used for XSRF protection when adding external logins
        private const string XsrfKey = "XsrfId";

        private IAuthenticationManager AuthenticationManager
        {
            get
            {
                return HttpContext.GetOwinContext().Authentication;
            }
        }

        private void AddErrors(IdentityResult result)
        {
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError("", error);
            }
        }

        private bool HasPassword()
        {
            var user = UserManager.FindById(User.Identity.GetUserId());
            if (user != null)
            {
                return user.PasswordHash != null;
            }
            return false;
        }

        private bool HasPhoneNumber()
        {
            var user = UserManager.FindById(User.Identity.GetUserId());
            if (user != null)
            {
                return user.PhoneNumber != null;
            }
            return false;
        }

        public enum ManageMessageId
        {
            AddPhoneSuccess,
            ChangePasswordSuccess,
            SetTwoFactorSuccess,
            SetPasswordSuccess,
            RemoveLoginSuccess,
            RemovePhoneSuccess,
            Error
        }

#endregion
    }
}