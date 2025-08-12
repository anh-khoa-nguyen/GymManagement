using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web.Mvc;
using GymManagementSystem.Models;

namespace GymManagementSystem.Controllers
{
    [Authorize(Roles = "QuanLy")]
    public class PhongsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Phongs
        public async Task<ActionResult> Index(string searchString)
        {
            var phongs = db.Phongs.AsQueryable();
            if (!string.IsNullOrWhiteSpace(searchString))
            {
                phongs = phongs.Where(p => p.TenPhong.Contains(searchString) || p.MaPhong.Contains(searchString));
            }
            ViewBag.CurrentFilter = searchString;
            return View(await phongs.OrderBy(p => p.TenPhong).ToListAsync());
        }

        // GET: Phongs/Create
        public ActionResult Create()
        {
            if (Request.IsAjaxRequest())
            {
                return PartialView("CreateOrEdit", new Phong());
            }
            return View(new Phong());
        }

        // POST: Phongs/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "Id,TenPhong,MaPhong,ViTri")] Phong phong)
        {
            if (ModelState.IsValid)
            {
                db.Phongs.Add(phong);
                await db.SaveChangesAsync();

                if (Request.IsAjaxRequest())
                {
                    return Json(new { success = true });
                }
                return RedirectToAction("Index");
            }

            if (Request.IsAjaxRequest())
            {
                return PartialView("CreateOrEdit", phong);
            }
            return View(phong);
        }

        // GET: Phongs/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null) return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            Phong phong = await db.Phongs.FindAsync(id);
            if (phong == null) return HttpNotFound();

            if (Request.IsAjaxRequest())
            {
                return PartialView("Details", phong);
            }
            return View(phong);
        }


        // GET: Phongs/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null) return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            Phong phong = await db.Phongs.FindAsync(id);
            if (phong == null) return HttpNotFound();

            if (Request.IsAjaxRequest())
            {
                return PartialView("CreateOrEdit", phong);
            }
            return View(phong);
        }

        // POST: Phongs/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "Id,TenPhong,MaPhong,ViTri")] Phong phong)
        {
            if (ModelState.IsValid)
            {
                db.Entry(phong).State = EntityState.Modified;
                await db.SaveChangesAsync();

                if (Request.IsAjaxRequest())
                {
                    return Json(new { success = true });
                }
                return RedirectToAction("Index");
            }

            if (Request.IsAjaxRequest())
            {
                return PartialView("CreateOrEdit", phong);
            }
            return View(phong);
        }

        // GET: Phongs/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null) return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            Phong phong = await db.Phongs.FindAsync(id);
            if (phong == null) return HttpNotFound();

            if (Request.IsAjaxRequest())
            {
                return PartialView("Delete", phong);
            }
            return View(phong);
        }

        // POST: Phongs/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            Phong phong = await db.Phongs.FindAsync(id);
            if (phong != null)
            {
                db.Phongs.Remove(phong);
                await db.SaveChangesAsync();
            }

            if (Request.IsAjaxRequest())
            {
                return Json(new { success = true });
            }
            return RedirectToAction("Index");
        }
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}