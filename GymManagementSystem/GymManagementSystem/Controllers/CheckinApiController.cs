using System;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using GymManagementSystem.Models;
using Microsoft.AspNet.Identity.Owin;

namespace GymManagementSystem.Controllers
{
    [RoutePrefix("api/checkin")]
    public class CheckinApiController : Controller // THAY ĐỔI 2: Kế thừa từ Controller
    {
        private ApplicationDbContext _db;
        public ApplicationDbContext Db
        {
            get => _db ?? HttpContext.GetOwinContext().Get<ApplicationDbContext>();
            private set => _db = value;
        }

        [HttpGet]
        [Route("verify")]
        public JsonResult Verify(string id) // THAY ĐỔI 4: Đổi kiểu trả về thành JsonResult
        {
            if (string.IsNullOrEmpty(id))
            {
                return Json(new { success = false, message = "Mã QR không hợp lệ." }, JsonRequestBehavior.AllowGet);
            }

            var user = Db.Users.Find(id);

            if (user == null)
            {
                return Json(new { success = false, message = "Không tìm thấy khách hàng với mã này." }, JsonRequestBehavior.AllowGet);
            }

            var hoivien = Db.HoiViens.FirstOrDefault(h => h.ApplicationUserId == id);

            return Json(new
            {
                success = true,
                hoTen = user.HoTen,
                email = user.Email
            }, JsonRequestBehavior.AllowGet);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing && _db != null)
            {
                _db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}