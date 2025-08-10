using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web.Mvc;
using GymManagementSystem.Models;

namespace GymManagementSystem.Controllers
{
    [Authorize(Roles = "QuanLy")]
    public class DacQuyensController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: DacQuyens
        public async Task<ActionResult> Index(string searchString)
        {
            var dacQuyens = db.DacQuyens.AsQueryable();
            if (!string.IsNullOrWhiteSpace(searchString))
            {
                dacQuyens = dacQuyens.Where(d => d.TenDacQuyen.Contains(searchString));
            }
            ViewBag.CurrentFilter = searchString;
            return View(await dacQuyens.OrderBy(d => d.TenDacQuyen).ToListAsync());
        }

        // GET: DacQuyens/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null) return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            DacQuyen dacQuyen = await db.DacQuyens.FindAsync(id);
            if (dacQuyen == null) return HttpNotFound();

            if (Request.IsAjaxRequest())
            {
                return PartialView("Details", dacQuyen);
            }
            return View(dacQuyen);
        }

        // GET: DacQuyens/Create
        public ActionResult Create()
        {
            if (Request.IsAjaxRequest())
            {
                return PartialView("CreateOrEdit", new DacQuyen());
            }
            return View(new DacQuyen());
        }

        // POST: DacQuyens/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "Id,TenDacQuyen,MoTa,IconClass")] DacQuyen dacQuyen)
        {
            if (ModelState.IsValid)
            {
                db.DacQuyens.Add(dacQuyen);
                await db.SaveChangesAsync();

                if (Request.IsAjaxRequest())
                {
                    return Json(new { success = true });
                }
                return RedirectToAction("Index");
            }

            if (Request.IsAjaxRequest())
            {
                return PartialView("CreateOrEdit", dacQuyen);
            }
            return View(dacQuyen);
        }

        // GET: DacQuyens/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null) return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            DacQuyen dacQuyen = await db.DacQuyens.FindAsync(id);
            if (dacQuyen == null) return HttpNotFound();

            if (Request.IsAjaxRequest())
            {
                return PartialView("CreateOrEdit", dacQuyen);
            }
            return View(dacQuyen);
        }

        // POST: DacQuyens/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "Id,TenDacQuyen,MoTa,IconClass")] DacQuyen dacQuyen)
        {
            if (ModelState.IsValid)
            {
                db.Entry(dacQuyen).State = EntityState.Modified;
                await db.SaveChangesAsync();

                if (Request.IsAjaxRequest())
                {
                    return Json(new { success = true });
                }
                return RedirectToAction("Index");
            }

            if (Request.IsAjaxRequest())
            {
                return PartialView("CreateOrEdit", dacQuyen);
            }
            return View(dacQuyen);
        }

        // GET: DacQuyens/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null) return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            DacQuyen dacQuyen = await db.DacQuyens.FindAsync(id);
            if (dacQuyen == null) return HttpNotFound();

            if (Request.IsAjaxRequest())
            {
                return PartialView("Delete", dacQuyen);
            }
            return View(dacQuyen);
        }

        // POST: DacQuyens/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            DacQuyen dacQuyen = await db.DacQuyens.FindAsync(id);
            if (dacQuyen != null)
            {
                db.DacQuyens.Remove(dacQuyen);
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