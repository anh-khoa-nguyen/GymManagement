using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using GymManagementSystem.Models;
using GymManagementSystem.Models.ViewModels;

namespace GymManagementSystem.Controllers
{
    [Authorize(Roles = "QuanLy")]
    public class BaiTapsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: BaiTaps
        public async Task<ActionResult> Index(string searchString)
        {
            var baiTapsQuery = db.BaiTaps.AsQueryable();
            if (!string.IsNullOrWhiteSpace(searchString))
            {
                baiTapsQuery = baiTapsQuery.Where(b => b.TenBaiTap.Contains(searchString) || b.NhomCoChinh.Contains(searchString));
            }
            ViewBag.CurrentFilter = searchString;
            return View(await baiTapsQuery.OrderBy(b => b.TenBaiTap).ToListAsync());
        }

        // GET: BaiTaps/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null) return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            var baiTap = await db.BaiTaps.Include(b => b.CacBuocThucHien).FirstOrDefaultAsync(b => b.Id == id);
            if (baiTap == null) return HttpNotFound();

            if (Request.IsAjaxRequest())
            {
                return PartialView("Details", baiTap);
            }
            return View(baiTap);
        }

        // GET: BaiTaps/Create
        public async Task<ActionResult> Create()
        {
            var viewModel = new BaiTapViewModel();
            ViewBag.AvailableDungCu = await GetAvailableThietBiList();

            if (Request.IsAjaxRequest())
            {
                return PartialView("CreateOrEdit", viewModel);
            }
            return View(viewModel);
        }

        // POST: BaiTaps/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(BaiTapViewModel viewModel, HttpPostedFileBase imageFile)
        {
            if (ModelState.IsValid)
            {
                if (imageFile != null && imageFile.ContentLength > 0)
                {
                    var cloudinaryService = new CloudinaryService();
                    var imageUrl = await cloudinaryService.UploadImageAsync(imageFile);
                    if (!string.IsNullOrEmpty(imageUrl))
                    {
                        viewModel.BaiTap.ImageUrl = imageUrl;
                    }
                }

                db.BaiTaps.Add(viewModel.BaiTap);
                await db.SaveChangesAsync();

                if (Request.IsAjaxRequest())
                {
                    return Json(new { success = true });
                }
                return RedirectToAction("Index");
            }

            ViewBag.AvailableDungCu = await GetAvailableThietBiList();
            if (Request.IsAjaxRequest())
            {
                return PartialView("CreateOrEdit", viewModel);
            }
            return View(viewModel);
        }

        // GET: BaiTaps/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null) return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            var baiTap = await db.BaiTaps.Include(b => b.CacBuocThucHien).FirstOrDefaultAsync(b => b.Id == id);
            if (baiTap == null) return HttpNotFound();

            var viewModel = new BaiTapViewModel { BaiTap = baiTap };
            ViewBag.AvailableDungCu = await GetAvailableThietBiList();

            if (Request.IsAjaxRequest())
            {
                return PartialView("CreateOrEdit", viewModel);
            }
            return View(viewModel);
        }

        // POST: BaiTaps/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(BaiTapViewModel viewModel, HttpPostedFileBase imageFile)
        {
            if (ModelState.IsValid)
            {
                if (imageFile != null && imageFile.ContentLength > 0)
                {
                    var cloudinaryService = new CloudinaryService();
                    var imageUrl = await cloudinaryService.UploadImageAsync(imageFile);
                    if (!string.IsNullOrEmpty(imageUrl))
                    {
                        viewModel.BaiTap.ImageUrl = imageUrl;
                    }
                }

                var baiTapInDb = await db.BaiTaps.Include(b => b.CacBuocThucHien).FirstOrDefaultAsync(b => b.Id == viewModel.BaiTap.Id);
                if (baiTapInDb == null) return HttpNotFound();

                db.Entry(baiTapInDb).CurrentValues.SetValues(viewModel.BaiTap);
                db.BuocThucHiens.RemoveRange(baiTapInDb.CacBuocThucHien);

                if (viewModel.BaiTap.CacBuocThucHien != null)
                {
                    foreach (var buoc in viewModel.BaiTap.CacBuocThucHien)
                    {
                        baiTapInDb.CacBuocThucHien.Add(buoc);
                    }
                }

                await db.SaveChangesAsync();

                if (Request.IsAjaxRequest())
                {
                    return Json(new { success = true });
                }
                return RedirectToAction("Index");
            }

            ViewBag.AvailableDungCu = await GetAvailableThietBiList();
            if (Request.IsAjaxRequest())
            {
                return PartialView("CreateOrEdit", viewModel);
            }
            return View(viewModel);
        }

        // GET: BaiTaps/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null) return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            var baiTap = await db.BaiTaps.FindAsync(id);
            if (baiTap == null) return HttpNotFound();

            if (Request.IsAjaxRequest())
            {
                return PartialView("Delete", baiTap);
            }
            return View(baiTap);
        }

        // POST: BaiTaps/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            var baiTap = await db.BaiTaps.FindAsync(id);
            if (baiTap != null)
            {
                db.BaiTaps.Remove(baiTap);
                await db.SaveChangesAsync();
            }

            if (Request.IsAjaxRequest())
            {
                return Json(new { success = true });
            }
            return RedirectToAction("Index");
        }

        private async Task<List<string>> GetAvailableThietBiList()
        {
            var tenThietBiList = await db.ThietBis
                                         .Where(t => t.TinhTrang == TinhTrangThietBi.HoatDongTot)
                                         .Select(t => t.TenThietBi)
                                         .ToListAsync();
            var distinctTenThietBi = tenThietBiList
                                        .Select(name => name.Trim())
                                        .Distinct(System.StringComparer.CurrentCultureIgnoreCase)
                                        .OrderBy(name => name);
            return distinctTenThietBi.ToList();
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