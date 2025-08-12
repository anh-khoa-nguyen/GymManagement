// Trong file Controllers/GoiTapsController.cs
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using GymManagementSystem.Models;

namespace GymManagementSystem.Controllers
{
    [Authorize(Roles = "QuanLy")]
    public class GoiTapsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        #region CRUD
        // GET: GoiTaps
        public async Task<ActionResult> Index(string searchString)
        {
            var goiTaps = db.GoiTaps.AsQueryable();
            if (!string.IsNullOrWhiteSpace(searchString))
            {
                goiTaps = goiTaps.Where(g => g.TenGoi.Contains(searchString));
            }
            ViewBag.CurrentFilter = searchString;
            return View(await goiTaps.OrderBy(g => g.GiaTien).ToListAsync());
        }


        // GET: GoiTaps/Create
        public ActionResult Create()
        {
            var model = new GoiTap();
            if (Request.IsAjaxRequest())
            {
                return PartialView("CreateOrEdit", model);
            }
            return View(model);
        }

        // POST: GoiTaps/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "Id,TenGoi,GiaTien,MoTaQuyenLoi,SoBuoiTapVoiPT,SoThang")] GoiTap goiTap, HttpPostedFileBase imageFile)
        {
            if (ModelState.IsValid)
            {
                if (imageFile != null && imageFile.ContentLength > 0)
                {
                    var cloudinaryService = new CloudinaryService();
                    goiTap.ImageUrl = await cloudinaryService.UploadImageAsync(imageFile);
                }
                db.GoiTaps.Add(goiTap);
                await db.SaveChangesAsync();

                if (Request.IsAjaxRequest())
                {
                    return Json(new { success = true });
                }
                return RedirectToAction("Index");
            }

            if (Request.IsAjaxRequest())
            {
                return PartialView("CreateOrEdit", goiTap);
            }
            return View(goiTap);
        }

        // GET: GoiTaps/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null) return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            GoiTap goiTap = await db.GoiTaps.FindAsync(id);
            if (goiTap == null) return HttpNotFound();

            if (Request.IsAjaxRequest())
            {
                return PartialView("Details", goiTap);
            }
            return View(goiTap);
        }

        // GET: GoiTaps/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null) return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            GoiTap goiTap = await db.GoiTaps.FindAsync(id);
            if (goiTap == null) return HttpNotFound();

            if (Request.IsAjaxRequest())
            {
                return PartialView("CreateOrEdit", goiTap);
            }
            return View(goiTap);
        }

        // POST: GoiTaps/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "Id,TenGoi,GiaTien,MoTaQuyenLoi,SoBuoiTapVoiPT,SoThang")] GoiTap goiTap, HttpPostedFileBase imageFile)
        {
            if (ModelState.IsValid)
            {
                if (imageFile != null && imageFile.ContentLength > 0)
                {
                    var cloudinaryService = new CloudinaryService();
                    goiTap.ImageUrl = await cloudinaryService.UploadImageAsync(imageFile);
                }
                db.Entry(goiTap).State = EntityState.Modified;
                await db.SaveChangesAsync();

                if (Request.IsAjaxRequest())
                {
                    return Json(new { success = true });
                }
                return RedirectToAction("Index");
            }

            if (Request.IsAjaxRequest())
            {
                return PartialView("CreateOrEdit", goiTap);
            }
            return View(goiTap);
        }

        // GET: GoiTaps/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null) return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            GoiTap goiTap = await db.GoiTaps.FindAsync(id);
            if (goiTap == null) return HttpNotFound();

            if (Request.IsAjaxRequest())
            {
                return PartialView("Delete", goiTap);
            }
            return View(goiTap);
        }

        // POST: GoiTaps/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            GoiTap goiTap = await db.GoiTaps.FindAsync(id);
            if (goiTap != null)
            {
                db.GoiTaps.Remove(goiTap);
                await db.SaveChangesAsync();
            }

            if (Request.IsAjaxRequest())
            {
                return Json(new { success = true });
            }
            return RedirectToAction("Index");
        }
        #endregion

        protected override void Dispose(bool disposing)
        {
            if (disposing) db.Dispose();
            base.Dispose(disposing);
        }
    }
}