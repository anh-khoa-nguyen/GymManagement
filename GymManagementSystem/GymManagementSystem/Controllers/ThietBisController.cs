using System;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web.Mvc;
using GymManagementSystem.Models;
using Microsoft.AspNet.Identity;
using Newtonsoft.Json;

namespace GymManagementSystem.Controllers
{
    [Authorize(Roles = "QuanLy")]
    public class ThietBisController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        #region CRUD
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

        // GET: ThietBis/Create
        public async Task<ActionResult> Create()
        {
            await PopulatePhongDropdown();
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
        public async Task<ActionResult> Create([Bind(Include = "Id,TenThietBi,MoTa,NgayMua,TinhTrang, PhongId")] ThietBi thietBi)
        {
            if (ModelState.IsValid)
            {
                db.ThietBis.Add(thietBi);
                await db.SaveChangesAsync();

                GhiLogThietBi(thietBi.Id, LoaiHanhDong.TaoMoi, null, thietBi); // Trạng thái trước là null
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

        // GET: ThietBis/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null) return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            ThietBi thietBi = await db.ThietBis.FindAsync(id);
            await PopulatePhongDropdown();
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
        public async Task<ActionResult> Edit([Bind(Include = "Id,TenThietBi,MoTa,NgayMua,TinhTrang, PhongId")] ThietBi thietBi)
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
                GhiLogThietBi(thietBi.Id, LoaiHanhDong.CapNhat, thietBiCu, thietBi);
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
                GhiLogThietBi(thietBi.Id, LoaiHanhDong.Xoa, thietBi, null);
                db.ThietBis.Remove(thietBi);
                await db.SaveChangesAsync();
            }

            if (Request.IsAjaxRequest())
            {
                return Json(new { success = true });
            }
            return RedirectToAction("Index");
        }

        #endregion

        #region Lịch sử, Log
        // GET: ThietBis/LichSu
        public async Task<ActionResult> History(DateTime? ngayLoc)
        {
            DateTime targetDate = ngayLoc ?? DateTime.Now;

            var historyService = new HistoryReconstructionService(db);
            ViewBag.EquipmentStates = await historyService.GetEquipmentStateAtAsync(targetDate);

            var logQuery = db.LichSuThietBis.Include(l => l.NguoiThucHien);
            DateTime batDau = targetDate.Date;
            DateTime ketThuc = batDau.AddDays(1);
            var logsForDate = await logQuery
                .Where(l => l.NgayThucHien >= batDau && l.NgayThucHien < ketThuc)
                .OrderByDescending(l => l.NgayThucHien)
                .ToListAsync();

            ViewBag.NgayLoc = targetDate;
            return View(logsForDate); // Truyền danh sách log vào Model

        }

        private void GhiLogThietBi(int thietBiId, LoaiHanhDong hanhDong, ThietBi trangThaiTruoc, ThietBi trangThaiSau)
        {
            db.Configuration.ProxyCreationEnabled = false;

            var settings = new JsonSerializerSettings
            {
                // Bỏ qua các vòng lặp tham chiếu thay vì ném ra lỗi
                ReferenceLoopHandling = ReferenceLoopHandling.Ignore,
            };

            var log = new LichSuThietBi
            {
                ThietBiId = thietBiId,
                NguoiThucHienId = User.Identity.GetUserId(),
                HanhDong = hanhDong,
                NgayThucHien = DateTime.Now,
                // Sử dụng 'settings' khi serialize
                TrangThaiTruoc = trangThaiTruoc == null ? null : JsonConvert.SerializeObject(trangThaiTruoc, settings),
                TrangThaiSau = trangThaiSau == null ? null : JsonConvert.SerializeObject(trangThaiSau, settings)
            };
            db.LichSuThietBis.Add(log);
            db.Configuration.ProxyCreationEnabled = true;
        }

        #endregion

        private async Task PopulatePhongDropdown()
        {
            ViewBag.PhongId = new SelectList
                (await db.Phongs.OrderBy(p => p.TenPhong)
                .ToListAsync(), "Id", "TenPhong");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing) db.Dispose();
            base.Dispose(disposing);
        }
    }
}