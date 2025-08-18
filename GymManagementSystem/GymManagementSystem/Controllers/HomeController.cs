using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using GymManagementSystem.Models; 


namespace GymManagementSystem.Controllers
{
    public class HomeController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }

        [ChildActionOnly]
        public PartialViewResult NotificationBell()
        {
            var notifications = new List<ThongBao>();
            if (User.Identity.IsAuthenticated)
            {
                var currentUserId = User.Identity.GetUserId();
                notifications = db.ThongBaos
                                  .Where(n => n.ApplicationUserId == currentUserId && !n.DaXem)
                                  .OrderByDescending(n => n.NgayTao)
                                  .ToList();
            }
            return PartialView("_NotificationBell", notifications);
        }

        [HttpPost] // Chỉ chấp nhận yêu cầu POST
        [Authorize] // Yêu cầu người dùng phải đăng nhập để thực hiện hành động này
        [ValidateAntiForgeryToken] // Thêm bước bảo mật để chống tấn công CSRF
        public JsonResult MarkNotificationsAsRead()
        {
            try
            {
                // 1. Lấy ID của người dùng đang đăng nhập
                var currentUserId = User.Identity.GetUserId();

                // 2. Tìm tất cả các thông báo CHƯA XEM của người dùng này
                var notificationsToUpdate = db.ThongBaos
                                              .Where(n => n.ApplicationUserId == currentUserId && !n.DaXem)
                                              .ToList();

                // 3. Nếu có thông báo mới, duyệt qua và cập nhật trạng thái
                if (notificationsToUpdate.Any())
                {
                    foreach (var notification in notificationsToUpdate)
                    {
                        notification.DaXem = true;
                    }

                    // 4. Lưu tất cả các thay đổi vào cơ sở dữ liệu
                    db.SaveChanges();
                }

                // 5. Trả về kết quả thành công dưới dạng JSON
                return Json(new { success = true });
            }
            catch (Exception ex)
            {
                // Ghi lại lỗi để bạn có thể debug nếu có vấn đề
                System.Diagnostics.Debug.WriteLine(ex.ToString());

                // Trả về lỗi nếu có sự cố xảy ra
                Response.StatusCode = 500; // Báo lỗi Internal Server Error
                return Json(new { success = false, message = "Đã có lỗi xảy ra." });
            }
        }

        [Authorize]
        [Authorize]
        public ActionResult RedirectToNotificationUrl(int notificationId)
        {
            var currentUserId = User.Identity.GetUserId();
            var notification = db.ThongBaos.FirstOrDefault(n => n.Id == notificationId && n.ApplicationUserId == currentUserId);

            if (notification != null)
            {
                notification.DaXem = true;
                db.SaveChanges();
                if (!string.IsNullOrEmpty(notification.URL) && Url.IsLocalUrl(notification.URL))
                {
                    return Redirect(notification.URL);
                }
            }
            return RedirectToAction("Index", "Home");
        }


        [Authorize] // Quan trọng: Chỉ người đã đăng nhập mới gọi được
        [HttpGet]
        public JsonResult CheckAuthentication()
        {
            // Nếu người dùng có thể gọi được đến đây, nghĩa là họ vẫn đang đăng nhập
            return Json(new { isAuthenticated = true }, JsonRequestBehavior.AllowGet);
        }

    }
}