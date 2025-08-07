using System.Web.Mvc;

namespace GymManagementSystem.Controllers
{
    [Authorize] // Chỉ nhân viên/admin đã đăng nhập mới được vào
    public class CheckinController : Controller
    {
        // GET: Checkin/Scan
        public ActionResult Scan()
        {
            return View();
        }
    }
}