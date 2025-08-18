using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;
using GymManagementSystem.Models;
using GymManagementSystem.Models.ViewModels;
using GymManagementSystem.Services;
using Microsoft.AspNet.Identity;

namespace GymManagementSystem.Controllers
{
    [Authorize(Roles = "HoiVien")]
    public class ChatbotController : Controller
    {
        private readonly GoogleAiService _aiService = new GoogleAiService();
        private readonly ApplicationDbContext db = new ApplicationDbContext();

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<JsonResult> Ask(string query)
        {
            if (string.IsNullOrWhiteSpace(query))
            {
                return Json(new { answer = "Vui lòng nhập câu hỏi của bạn." });
            }

            var userId = User.Identity.GetUserId();
            var hoiVien = await db.HoiViens
                                  .Include(h => h.ChiSoSucKhoes)
                                  .FirstOrDefaultAsync(h => h.ApplicationUserId == userId);

            if (hoiVien == null || !hoiVien.ChiSoSucKhoes.Any())
            {
                return Json(new { answer = "Tôi chưa có dữ liệu sức khỏe của bạn để đưa ra lời khuyên." });
            }

            var latestHealthIndex = hoiVien.ChiSoSucKhoes.OrderByDescending(cs => cs.NgayCapNhat).First();

            var conversationHistory = Session["ChatHistory"] as List<GeminiContent> ?? new List<GeminiContent>();

            string botAnswer = await _aiService.ConverseAsync(query, conversationHistory, latestHealthIndex, hoiVien);

            Session["ChatHistory"] = conversationHistory;

            return Json(new { answer = botAnswer });
        }
    }
}