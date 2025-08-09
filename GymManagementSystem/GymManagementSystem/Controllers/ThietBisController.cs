using System;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web.Mvc;
using GymManagementSystem.Models;
using Microsoft.AspNet.Identity;

namespace GymManagementSystem.Controllers
{
    [Authorize(Roles = "QuanLy")]
    public class ThietBisController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: ThietBis
        public async Task<ActionResult> Index(string searchString)
        {
            var thietBis = db.ThietBis.AsQueryable();

            if (!String.IsNullOrWhiteSpace(searchString))
            {
                thietBis = thietBis.Where(t => t.TenThietBi.Contains(searchString));
            }

            ViewBag.CurrentFilter = searchString;
            return View(await thietBis.OrderBy(t => t.TenThietBi).ToListAsync());
        }

        // GET: ThietBis/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null) return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            ThietBi thietBi = await db.ThietBis.FindAsync(id);
            if (thietBi == null) return HttpNotFound();

            if (Request.IsAjaxRequest())
            {
                return PartialView("Details", thietBi);
            }
            return View(thietBi);
        }

        // GET: ThietBis/Create
        public ActionResult Create()
        {
            var model = new ThietBi { TinhTrang = TinhTrangThietBi.HoatDongTot };
            if (Request.IsAjaxRequest())
            {
                return PartialView("CreateOrEdit", model);
            }
            return View(model);
        }

        // POST: ThietBis/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "Id,TenThietBi,MoTa,NgayMua,TinhTrang")] ThietBi thietBi)
        {
            if (ModelState.IsValid)
            {
                db.ThietBis.Add(thietBi);
                await db.SaveChangesAsync();

                GhiLogThietBi(thietBi.Id, LoaiHanhDong.TaoMoi, $"Tạo mới thiết bị '{thietBi.TenThietBi}' với tình trạng '{thietBi.TinhTrang}'.");
                await db.SaveChangesAsync();

                if (Request.IsAjaxRequest())
                {
                    return Json(new { success = true });
                }
                return RedirectToAction("Index");
            }

            if (Request.IsAjaxRequest())
            {
                return PartialView("CreateOrEdit", thietBi);
            }
            return View(thietBi);
        }

        // GET: ThietBis/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null) return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            ThietBi thietBi = await db.ThietBis.FindAsync(id);
            if (thietBi == null) return HttpNotFound();

            if (Request.IsAjaxRequest())
            {
                return PartialView("CreateOrEdit", thietBi);
            }
            return View(thietBi);
        }

        // POST: ThietBis/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "Id,TenThietBi,MoTa,NgayMua,TinhTrang")] ThietBi thietBi)
        {
            if (ModelState.IsValid)
            {
                var thietBiCu = await db.ThietBis.AsNoTracking().FirstOrDefaultAsync(t => t.Id == thietBi.Id);
                string moTaThayDoi = $"Cập nhật thiết bị '{thietBi.TenThietBi}'.";
                if (thietBiCu != null && thietBiCu.TinhTrang != thietBi.TinhTrang)
                {
                    moTaThayDoi += $" Tình trạng thay đổi từ '{thietBiCu.TinhTrang}' sang '{thietBi.TinhTrang}'.";
                }

                db.Entry(thietBi).State = EntityState.Modified;
                GhiLogThietBi(thietBi.Id, LoaiHanhDong.CapNhat, moTaThayDoi);
                await db.SaveChangesAsync();

                if (Request.IsAjaxRequest())
                {
                    return Json(new { success = true });
                }
                return RedirectToAction("Index");
            }

            if (Request.IsAjaxRequest())
            {
                return PartialView("CreateOrEdit", thietBi);
            }
            return View(thietBi);
        }

        // GET: ThietBis/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null) return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            ThietBi thietBi = await db.ThietBis.FindAsync(id);
            if (thietBi == null) return HttpNotFound();

            if (Request.IsAjaxRequest())
            {
                return PartialView("Delete", thietBi);
            }
            return View(thietBi);
        }

        // POST: ThietBis/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            ThietBi thietBi = await db.ThietBis.FindAsync(id);
            if (thietBi != null)
            {
                GhiLogThietBi(thietBi.Id, LoaiHanhDong.Xoa, $"Xóa thiết bị '{thietBi.TenThietBi}'.");
                db.ThietBis.Remove(thietBi);
                await db.SaveChangesAsync();
            }

            if (Request.IsAjaxRequest())
            {
                return Json(new { success = true });
            }
            return RedirectToAction("Index");
        }

        // GET: ThietBis/LichSu
        public async Task<ActionResult> History(DateTime? ngayLoc)
        {
            var lichSuQuery = db.LichSuThietBis
                                 .Include(l => l.NguoiThucHien)
                                 .OrderByDescending(l => l.NgayThucHien);

            if (ngayLoc.HasValue)
            {
                DateTime batDau = ngayLoc.Value.Date;
                DateTime ketThuc = batDau.AddDays(1);
                lichSuQuery = (IOrderedQueryable<LichSuThietBi>)lichSuQuery.Where(l => l.NgayThucHien >= batDau && l.NgayThucHien < ketThuc);
            }

            ViewBag.NgayLoc = ngayLoc?.ToString("yyyy-MM-dd");
            return View(await lichSuQuery.ToListAsync());
        }

        private void GhiLogThietBi(int thietBiId, LoaiHanhDong hanhDong, string moTa)
        {
            var log = new LichSuThietBi
            {
                ThietBiId = thietBiId,
                NguoiThucHienId = User.Identity.GetUserId(),
                HanhDong = hanhDong,
                NgayThucHien = DateTime.Now,
                MoTaThayDoi = moTa
            };
            db.LichSuThietBis.Add(log);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing) db.Dispose();
            base.Dispose(disposing);
        }
    }
}