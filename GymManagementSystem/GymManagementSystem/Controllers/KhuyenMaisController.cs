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

         //GET: KhuyenMais
        public async Task<ActionResult> Index(string searchString)
        {
            //return View(await db.KhuyenMais.ToListAsync());
            var khuyenMais = db.KhuyenMais.AsQueryable();

            // Nếu có chuỗi tìm kiếm, lọc kết quả
            if (!String.IsNullOrWhiteSpace(searchString))
            {
                khuyenMais = khuyenMais.Where(k => k.TenKhuyenMai.Contains(searchString) ||
                                                   k.MaKhuyenMai.Contains(searchString));
            }

            // Truyền lại chuỗi tìm kiếm về View để hiển thị trong ô input
            ViewBag.CurrentFilter = searchString;

            return View(khuyenMais.ToList());
        }

        //public ActionResult Index(string searchString)
        //{
            
        //}

        // GET: KhuyenMais/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            KhuyenMai khuyenMai = await db.KhuyenMais.FindAsync(id);
            if (khuyenMai == null)
            {
                return HttpNotFound();
            }

            if (Request.IsAjaxRequest())
            {
                return PartialView("Details", khuyenMai);
            }

            return View(khuyenMai);
        }

        // GET: KhuyenMais/Create
        public ActionResult Create()
        {
            var model = new KhuyenMai
            {
                NgayBatDau = DateTime.Now,
                NgayKetThuc = DateTime.Now.AddMonths(1),
                IsActive = true
            };

            if (Request.IsAjaxRequest())
            {
                return PartialView("Create");
            }
            return View(model);
        }

        // POST: KhuyenMais/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "Id,TenKhuyenMai,MoTa,MaKhuyenMai,PhanTramGiamGia,SoTienGiamGia,NgayBatDau,NgayKetThuc,IsActive")] KhuyenMai khuyenMai)
        {
            if (ModelState.IsValid)
            {
                db.KhuyenMais.Add(khuyenMai);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            return View(khuyenMai);
        }

        // GET: KhuyenMais/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            KhuyenMai khuyenMai = await db.KhuyenMais.FindAsync(id);
            if (khuyenMai == null)
            {
                return HttpNotFound();
            }
            if (Request.IsAjaxRequest())
            {
                return PartialView("Edit", khuyenMai);
            }
            return View(khuyenMai);
        }

        // POST: KhuyenMais/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "Id,TenKhuyenMai,MoTa,MaKhuyenMai,PhanTramGiamGia,SoTienGiamGia,NgayBatDau,NgayKetThuc,IsActive")] KhuyenMai khuyenMai)
        {
            if (ModelState.IsValid)
            {
                db.Entry(khuyenMai).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return View(khuyenMai);
        }

        // GET: KhuyenMais/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            KhuyenMai khuyenMai = await db.KhuyenMais.FindAsync(id);
            if (khuyenMai == null)
            {
                return HttpNotFound();
            }
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
            return RedirectToAction("Index");
        }


        // POST: KhuyenMais/DeleteMultiple
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteMultiple(int[] idsToDelete)
        {
            if (idsToDelete != null && idsToDelete.Length > 0)
            {
                // Tìm tất cả các khuyến mãi có ID trong danh sách
                var khuyenMais = db.KhuyenMais.Where(k => idsToDelete.Contains(k.Id)).ToList();

                if (khuyenMais.Any())
                {
                    db.KhuyenMais.RemoveRange(khuyenMais);
                    db.SaveChanges();
                }
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
