using System;
using System.Collections.Generic;
using System.Data;
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
    public class KhuyenMaisController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        #region CRUD
        // GET: KhuyenMais
        public async Task<ActionResult> Index(string searchString)
        {
            var khuyenMais = db.KhuyenMais.AsQueryable();

            if (!String.IsNullOrWhiteSpace(searchString))
            {
                khuyenMais = khuyenMais.Where(k => k.TenKhuyenMai.Contains(searchString) ||
                                                   k.MaKhuyenMai.Contains(searchString));
            }
            ViewBag.CurrentFilter = searchString;

            return View(await khuyenMais.ToListAsync());
        }

        // GET: KhuyenMais/Create
        public async Task<ActionResult> Create()
        {
            var viewModel = new KhuyenMaiViewModel();
            //viewModel.KhuyenMai.IsActive = true;
            //viewModel.KhuyenMai.SoNgayHieuLuc = 30;

            await PopulateGoiTapDropdown(viewModel);

            if (Request.IsAjaxRequest())
            {
                return PartialView("CreateOrEdit", viewModel);
            }
            return View(viewModel);
        }

        // POST: KhuyenMais/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(KhuyenMaiViewModel viewModel)
        {
            if (ModelState.IsValid)
            {
                db.KhuyenMais.Add(viewModel.KhuyenMai);
                await db.SaveChangesAsync();

                var selectedIds = viewModel.SelectedGoiTapIds ?? new List<int>();
                
                if (viewModel.ApplyType == "include")
                {
                    foreach (var goiTapId in selectedIds)
                    {
                        db.KhuyenMaiCuaGois.Add(new KhuyenMaiCuaGoi { KhuyenMaiId = viewModel.KhuyenMai.Id, GoiTapId = goiTapId });
                    }
                }
                else if (viewModel.ApplyType == "exclude")
                {
                    var allGoiTapIds = await db.GoiTaps.Select(g => g.Id).ToListAsync();
                    var idsToInclude = allGoiTapIds.Except(selectedIds);
                    foreach (var goiTapId in idsToInclude)
                    {
                        db.KhuyenMaiCuaGois.Add(new KhuyenMaiCuaGoi { KhuyenMaiId = viewModel.KhuyenMai.Id, GoiTapId = goiTapId });
                    }
                }
                await db.SaveChangesAsync();

                if (Request.IsAjaxRequest())
                {
                    return Json(new { success = true });
                }
                return RedirectToAction("Index");
            }
 
            await PopulateGoiTapDropdown(viewModel);
            if (Request.IsAjaxRequest())
            {
                return PartialView("CreateOrEdit", viewModel);
            }
            return View(viewModel);
        }

        // GET: KhuyenMais/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null) return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            var khuyenMai = await db.KhuyenMais
                .Include(k => k.ApDungChoGoiTap.Select(gt => gt.GoiTap))
                .FirstOrDefaultAsync(k => k.Id == id);
            
            if (khuyenMai == null) return HttpNotFound();

            if (Request.IsAjaxRequest())
            {
                return PartialView("Details", khuyenMai);
            }

            return View(khuyenMai);
        }

        // GET: KhuyenMais/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null) return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            var khuyenMai = await db.KhuyenMais.Include(k => k.ApDungChoGoiTap).FirstOrDefaultAsync(k => k.Id == id);
            if (khuyenMai == null) return HttpNotFound();

            var viewModel = new KhuyenMaiViewModel
            {
                KhuyenMai = khuyenMai,
                SelectedGoiTapIds = khuyenMai.ApDungChoGoiTap.Select(gt => gt.GoiTapId).ToList()
            };
            await PopulateGoiTapDropdown(viewModel);

            if (Request.IsAjaxRequest())
            {
                return PartialView("CreateOrEdit", viewModel);
            }
            return View(viewModel);
        }

        // POST: KhuyenMais/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(KhuyenMaiViewModel viewModel)
        {
            if (ModelState.IsValid)
            {
                var khuyenMaiInDb = await db.KhuyenMais
                    .Include(k => k.ApDungChoGoiTap)
                    .FirstOrDefaultAsync(k => k.Id == viewModel.KhuyenMai.Id);

                db.Entry(khuyenMaiInDb).CurrentValues.SetValues(viewModel.KhuyenMai);
                db.KhuyenMaiCuaGois.RemoveRange(khuyenMaiInDb.ApDungChoGoiTap);

                var selectedIds = viewModel.SelectedGoiTapIds ?? new List<int>();
                if (viewModel.ApplyType == "include")
                {
                    foreach (var goiTapId in selectedIds)
                    {
                        db.KhuyenMaiCuaGois.Add(new KhuyenMaiCuaGoi { KhuyenMaiId = khuyenMaiInDb.Id, GoiTapId = goiTapId });
                    }
                }
                else if (viewModel.ApplyType == "exclude")
                {
                    var allGoiTapIds = await db.GoiTaps.Select(g => g.Id).ToListAsync();
                    var idsToInclude = allGoiTapIds.Except(selectedIds);
                    foreach (var goiTapId in idsToInclude)
                    {
                        db.KhuyenMaiCuaGois.Add(new KhuyenMaiCuaGoi { KhuyenMaiId = khuyenMaiInDb.Id, GoiTapId = goiTapId });
                    }
                }
                await db.SaveChangesAsync();

                if (Request.IsAjaxRequest())
                {
                    return Json(new { success = true });
                }
                return RedirectToAction("Index");
            }

            await PopulateGoiTapDropdown(viewModel);
            if (Request.IsAjaxRequest())
            {
                return PartialView("CreateOrEdit", viewModel);
            }
            return View(viewModel);
        }

        // GET: KhuyenMais/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null) return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            KhuyenMai khuyenMai = await db.KhuyenMais.FindAsync(id);
            if (khuyenMai == null) return HttpNotFound();

            if (Request.IsAjaxRequest())
            {
                return PartialView("Delete", khuyenMai);
            }

            return View(khuyenMai);
        }

        // POST: KhuyenMais/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            KhuyenMai khuyenMai = await db.KhuyenMais.FindAsync(id);
            if (khuyenMai != null)
            {
                db.KhuyenMais.Remove(khuyenMai);
                await db.SaveChangesAsync();
            }
            if (Request.IsAjaxRequest())
            {
                return Json(new { success = true });
            }
            return RedirectToAction("Index");
        }

        // POST: KhuyenMais/DeleteMultiple
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteMultiple(int[] idsToDelete)
        {
            if (idsToDelete != null && idsToDelete.Length > 0)
            {
                var khuyenMais = await db.KhuyenMais.Where(k => idsToDelete.Contains(k.Id)).ToListAsync();
                if (khuyenMais.Any())
                {
                    db.KhuyenMais.RemoveRange(khuyenMais);
                    await db.SaveChangesAsync();
                }
            }
            return RedirectToAction("Index");
        }

        #endregion

        private async Task PopulateGoiTapDropdown(KhuyenMaiViewModel viewModel)
        {
            viewModel.DanhSachGoiTap = await db.GoiTaps
                .Select(g => new SelectListItem { Value = g.Id.ToString(), Text = g.TenGoi })
                .ToListAsync();
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing) db.Dispose();
            base.Dispose(disposing);
        }
    }
}