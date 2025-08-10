// Trong file Controllers/HangHoiViensController.cs
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web.Mvc;
using GymManagementSystem.Models;
using GymManagementSystem.Models.ViewModels;

namespace GymManagementSystem.Controllers
{
    [Authorize(Roles = "QuanLy")]
    public class HangHoiViensController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        #region CRUD
        // GET: HangHoiViens
        public async Task<ActionResult> Index(string searchString)
        {
            var hangHoiViens = db.HangHoiViens.AsQueryable();
            if (!string.IsNullOrWhiteSpace(searchString))
            {
                hangHoiViens = hangHoiViens.Where(h => h.TenHang.Contains(searchString));
            }
            ViewBag.CurrentFilter = searchString;
            return View(await hangHoiViens.OrderBy(h => h.NguongChiTieu).ToListAsync());
        }

        // GET: HangHoiViens/Create
        public async Task<ActionResult> Create()
        {
            var viewModel = new HangHoiVienViewModel();
            await PopulateKhuyenMaiDropdown(viewModel);
            await PopulateDacQuyenDropdown(viewModel);
            if (Request.IsAjaxRequest())
            {
                return PartialView("CreateOrEdit", viewModel);
            }
            return View(viewModel);
        }

        // POST: HangHoiViens/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(HangHoiVienViewModel viewModel)
        {
            if (ModelState.IsValid)
            {
                db.HangHoiViens.Add(viewModel.HangHoiVien);
                await db.SaveChangesAsync();

                if (viewModel.SelectedKhuyenMaiIds != null)
                {
                    foreach (var kmId in viewModel.SelectedKhuyenMaiIds)
                    {
                        db.HangHoiVien_KhuyenMais.Add
                            (new HangCoKhuyenMai 
                            { HangHoiVienId = viewModel.HangHoiVien.Id, KhuyenMaiId = kmId });
                    }
                    await db.SaveChangesAsync();
                }

                if (viewModel.SelectedDacQuyenIds != null)
                {
                    foreach (var dqId in viewModel.SelectedDacQuyenIds)
                    {
                        db.HangHoiVien_DacQuyens.Add
                            (new HangCoDacQuyen 
                            { HangHoiVienId = viewModel.HangHoiVien.Id, DacQuyenId = dqId });
                    }
                    await db.SaveChangesAsync();
                }

                if (Request.IsAjaxRequest())
                {
                    return Json(new { success = true });
                }
                return RedirectToAction("Index");
            }
            await PopulateKhuyenMaiDropdown(viewModel);
            if (Request.IsAjaxRequest())
            {
                return PartialView("CreateOrEdit", viewModel);
            }
            return View(viewModel);
        }

        // GET: HangHoiViens/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null) return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            var hangHoiVien = await db.HangHoiViens
                                      .Include(h => h.KhuyenMaiDacQuyen.Select(km => km.KhuyenMai))
                                      .FirstOrDefaultAsync(h => h.Id == id);

            if (hangHoiVien == null) return HttpNotFound();

            if (Request.IsAjaxRequest())
            {
                return PartialView("Details", hangHoiVien);
            }
            return View(hangHoiVien);
        }


        // GET: HangHoiViens/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null) return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            var hangHoiVien = await db.HangHoiViens
                .Include(h => h.KhuyenMaiDacQuyen)
                .Include(h => h.HangCoDacQuyen)
                .FirstOrDefaultAsync(h => h.Id == id);
            if (hangHoiVien == null) return HttpNotFound();

            var viewModel = new HangHoiVienViewModel
            {
                HangHoiVien = hangHoiVien,
                SelectedKhuyenMaiIds = hangHoiVien.KhuyenMaiDacQuyen.Select(km => km.KhuyenMaiId).ToList(),
                SelectedDacQuyenIds = hangHoiVien.HangCoDacQuyen.Select(dq => dq.DacQuyenId).ToList()

            };
            await PopulateKhuyenMaiDropdown(viewModel);
            await PopulateDacQuyenDropdown(viewModel);

            if (Request.IsAjaxRequest())
            {
                return PartialView("CreateOrEdit", viewModel);
            }
            return View(viewModel);
        }

        // POST: HangHoiViens/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(HangHoiVienViewModel viewModel)
        {
            if (ModelState.IsValid)
            {
                var hangInDb = await db.HangHoiViens
                    .Include(h => h.KhuyenMaiDacQuyen)
                    .Include(h => h.HangCoDacQuyen)
                    .FirstOrDefaultAsync(h => h.Id == viewModel.HangHoiVien.Id);

                db.Entry(hangInDb).CurrentValues.SetValues(viewModel.HangHoiVien); //Nhanh hơn việc gán thủ công

                db.HangHoiVien_KhuyenMais.RemoveRange(hangInDb.KhuyenMaiDacQuyen);

                if (viewModel.SelectedKhuyenMaiIds != null)
                {
                    foreach (var kmId in viewModel.SelectedKhuyenMaiIds)
                    {
                        db.HangHoiVien_KhuyenMais.Add(new HangCoKhuyenMai { HangHoiVienId = hangInDb.Id, KhuyenMaiId = kmId });
                    }
                }

                db.HangHoiVien_DacQuyens.RemoveRange(hangInDb.HangCoDacQuyen);
                if (viewModel.SelectedDacQuyenIds != null)
                {
                    foreach (var dqId in viewModel.SelectedDacQuyenIds)
                    {
                        db.HangHoiVien_DacQuyens.Add(new HangCoDacQuyen { HangHoiVienId = hangInDb.Id, DacQuyenId = dqId });
                    }
                }

                await db.SaveChangesAsync();

                if (Request.IsAjaxRequest())
                {
                    return Json(new { success = true });
                }
                return RedirectToAction("Index");
            }
            await PopulateKhuyenMaiDropdown(viewModel);
            if (Request.IsAjaxRequest())
            {
                return PartialView("CreateOrEdit", viewModel);
            }
            return View(viewModel);
        }

        // GET: HangHoiViens/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null) return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            HangHoiVien hangHoiVien = await db.HangHoiViens.FindAsync(id);
            if (hangHoiVien == null) return HttpNotFound();

            if (Request.IsAjaxRequest())
            {
                return PartialView("Delete", hangHoiVien);
            }
            return View(hangHoiVien);
        }

        // POST: HangHoiViens/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            HangHoiVien hangHoiVien = await db.HangHoiViens.FindAsync(id);
            if (hangHoiVien != null)
            {
                db.HangHoiViens.Remove(hangHoiVien);
                await db.SaveChangesAsync();
            }

            if (Request.IsAjaxRequest())
            {
                return Json(new { success = true });
            }
            return RedirectToAction("Index");
        }
        #endregion

        #region Dropdowns
        private async Task PopulateKhuyenMaiDropdown(HangHoiVienViewModel viewModel)
        {
            viewModel.DanhSachKhuyenMai = await db.KhuyenMais
                .Where(k => k.IsActive)
                .Select(k => new SelectListItem { Value = k.Id.ToString(), Text = k.TenKhuyenMai })
                .ToListAsync();
        }

        private async Task PopulateDacQuyenDropdown(HangHoiVienViewModel viewModel)
        {
            viewModel.DanhSachDacQuyen = await db.DacQuyens
                .Select(d => new SelectListItem
                {
                    Value = d.Id.ToString(),
                    Text = d.TenDacQuyen
                }).ToListAsync();
        }
        #endregion

        protected override void Dispose(bool disposing)
        {
            if (disposing) db.Dispose();
            base.Dispose(disposing);
        }
    }
}