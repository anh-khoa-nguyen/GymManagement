using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Net;
using System.Web;
using System.Web.Mvc;
using GymManagementSystem.Models;

namespace GymManagementSystem.Controllers
{
    [Authorize(Roles = "QuanLy")]
    public class KhuyenMaisController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

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

            // Sửa lại để dùng ToListAsync cho nhất quán
            return View(await khuyenMais.ToListAsync());
        }

        // GET: KhuyenMais/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null) return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            KhuyenMai khuyenMai = await db.KhuyenMais.FindAsync(id);
            if (khuyenMai == null) return HttpNotFound();

            if (Request.IsAjaxRequest())
            {
                return PartialView("Details", khuyenMai);
            }

            return View(khuyenMai);
        }

        // GET: KhuyenMais/Create
        public ActionResult Create()
        {
            // Gán giá trị mặc định mới
            var model = new KhuyenMai
            {
                IsActive = true,
                SoNgayHieuLuc = 30 // Mặc định là 30 ngày
            };

            if (Request.IsAjaxRequest())
            {
                return PartialView("Create", model);
            }
            return View(model);
        }

        // POST: KhuyenMais/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        // SỬA LẠI THUỘC TÍNH BIND
        public async Task<ActionResult> Create([Bind(Include = "Id,TenKhuyenMai,MoTa,MaKhuyenMai,PhanTramGiamGia,SoTienGiamToiDa,SoNgayHieuLuc,IsActive")] KhuyenMai khuyenMai)
        {
            if (ModelState.IsValid)
            {
                db.KhuyenMais.Add(khuyenMai);
                await db.SaveChangesAsync();
                if (Request.IsAjaxRequest())
                {
                    return Json(new { success = true });
                }
                return RedirectToAction("Index");
            }

            if (Request.IsAjaxRequest())
            {
                return PartialView("_Create", khuyenMai);
            }

            return View(khuyenMai);
        }

        // GET: KhuyenMais/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null) return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            KhuyenMai khuyenMai = await db.KhuyenMais.FindAsync(id);
            if (khuyenMai == null) return HttpNotFound();

            if (Request.IsAjaxRequest())
            {
                return PartialView("Edit", khuyenMai);
            }
            return View(khuyenMai);
        }

        // POST: KhuyenMais/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        // SỬA LẠI THUỘC TÍNH BIND
        public async Task<ActionResult> Edit([Bind(Include = "Id,TenKhuyenMai,MoTa,MaKhuyenMai,PhanTramGiamGia,SoTienGiamToiDa,SoNgayHieuLuc,IsActive")] KhuyenMai khuyenMai)
        {
            if (ModelState.IsValid)
            {
                db.Entry(khuyenMai).State = EntityState.Modified;
                await db.SaveChangesAsync();
                if (Request.IsAjaxRequest())
                {
                    return Json(new { success = true });
                }
                return RedirectToAction("Index");
            }

            if (Request.IsAjaxRequest())
            {
                return PartialView("Edit", khuyenMai);
            }
            return View(khuyenMai);
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
            db.KhuyenMais.Remove(khuyenMai);
            await db.SaveChangesAsync();
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

        protected override void Dispose(bool disposing)
        {
            if (disposing) db.Dispose();
            base.Dispose(disposing);
        }
    }
}