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
            if (User.Identity.IsAuthenticated)
            {
                var currentUserId = User.Identity.GetUserId();
                var notifications = db.ThongBaos
                                      .Where(n => n.ApplicationUserId == currentUserId && !n.DaXem)
                                      .OrderByDescending(n => n.NgayTao)
                                      .ToList();
                return PartialView("_NotificationBell", notifications);
            }
            return null;
        }

        [HttpPost]
        [Authorize] // Chỉ người dùng đã đăng nhập mới được gọi
        public JsonResult MarkNotificationsAsRead()
        {
            try
            {
                var currentUserId = User.Identity.GetUserId();
                var notificationsToUpdate = db.ThongBaos
                                              .Where(n => n.ApplicationUserId == currentUserId && !n.DaXem)
                                              .ToList();

                foreach (var notification in notificationsToUpdate)
                {
                    notification.DaXem = true;
                }

                db.SaveChanges();

                return Json(new { success = true });
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.ToString());
                return Json(new { success = false });
            }
        }

        [Authorize]
        public ActionResult RedirectToNotificationUrl(int notificationId)
        {
            var currentUserId = User.Identity.GetUserId();
            var notification = db.ThongBaos.FirstOrDefault(n => n.Id == notificationId && n.ApplicationUserId == currentUserId);

            if (notification != null)
            {
                // Đánh dấu là đã xem
                notification.DaXem = true;
                db.SaveChanges();

                // Nếu URL hợp lệ, chuyển hướng đến đó
                if (Url.IsLocalUrl(notification.URL))
                {
                    return Redirect(notification.URL);
                }
            }

            // Nếu có lỗi hoặc URL không hợp lệ, chuyển về trang chủ
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