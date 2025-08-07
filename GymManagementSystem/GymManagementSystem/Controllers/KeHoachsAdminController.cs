// Trong file Controllers/KeHoachsAdminController.cs
using System;
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
    public class KeHoachsAdminController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: KeHoachsAdmin (Danh sách kế hoạch)
        public async Task<ActionResult> Index(string searchString)
        {
            var keHoachs = db.KeHoachs.AsQueryable();

            // Lọc kết quả nếu có chuỗi tìm kiếm
            if (!String.IsNullOrWhiteSpace(searchString))
            {
                keHoachs = keHoachs.Where(k => k.TenKeHoach.Contains(searchString));
            }

            ViewBag.CurrentFilter = searchString;
            return View(keHoachs.ToList());
        }

        // --- THÊM ACTION DETAILS ---
        // GET: KeHoachsAdmin/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null) return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            var keHoach = await db.KeHoachs
                                  .Include(k => k.ChiTietKeHoachs.Select(ct => ct.BaiTap)) // Lấy chi tiết và tên bài tập
                                  .FirstOrDefaultAsync(k => k.Id == id);

            if (keHoach == null) return HttpNotFound();

            if (Request.IsAjaxRequest())
            {
                return PartialView("Details", keHoach);
            }

            return View(keHoach);
        }

        // Hàm private để chuẩn bị dữ liệu cho ViewModel
        private async Task PopulateViewModelDropdowns(KeHoachViewModel viewModel)
        {
            viewModel.DanhSachBaiTap = await db.BaiTaps
                .Select(b => new SelectListItem { Value = b.Id.ToString(), Text = b.TenBaiTap })
                .ToListAsync();

            viewModel.DanhSachKhuyenMai = await db.KhuyenMais
                .Where(k => k.IsActive)
                .Select(k => new SelectListItem { Value = k.Id.ToString(), Text = k.TenKhuyenMai })
                .ToListAsync();
            viewModel.DanhSachKhuyenMai.ToList().Insert(0, new SelectListItem { Value = "", Text = "-- Không có khuyến mãi thưởng --" });
        }

        // GET: KeHoachsAdmin/Create
        public async Task<ActionResult> Create()
        {
            var viewModel = new KeHoachViewModel();
            await PopulateViewModelDropdowns(viewModel);
            if (Request.IsAjaxRequest())
            {
                return PartialView("Create", viewModel);
            }
            return View(viewModel);
        }

        // POST: KeHoachsAdmin/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(KeHoachViewModel viewModel)
        {
            if (ModelState.IsValid)
            {
                // Gán lại số ngày cho đúng
                if (viewModel.KeHoach.ChiTietKeHoachs != null)
                {
                    for (int i = 0; i < viewModel.KeHoach.ChiTietKeHoachs.Count; i++)
                    {
                        viewModel.KeHoach.ChiTietKeHoachs.ElementAt(i).NgayTrongKeHoach = i + 1;
                    }
                }

                db.KeHoachs.Add(viewModel.KeHoach);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            await PopulateViewModelDropdowns(viewModel);
            return View(viewModel);
        }

        // GET: KeHoachsAdmin/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null) return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            var keHoach = await db.KeHoachs
                                  .Include(k => k.ChiTietKeHoachs)
                                  .FirstOrDefaultAsync(k => k.Id == id);

            if (keHoach == null) return HttpNotFound();

            var viewModel = new KeHoachViewModel { KeHoach = keHoach };
            await PopulateViewModelDropdowns(viewModel);
            if (Request.IsAjaxRequest())
            {
                return PartialView("Edit", viewModel);
            }
            return View(viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(KeHoachViewModel viewModel)
        {
            if (ModelState.IsValid)
            {
                var keHoachInDb = await db.KeHoachs
                                          .Include(k => k.ChiTietKeHoachs)
                                          .FirstOrDefaultAsync(k => k.Id == viewModel.KeHoach.Id);

                db.ChiTietKeHoachs.RemoveRange(keHoachInDb.ChiTietKeHoachs);

                keHoachInDb.TenKeHoach = viewModel.KeHoach.TenKeHoach;
                keHoachInDb.MoTa = viewModel.KeHoach.MoTa;
                keHoachInDb.ThoiGianThucHien = viewModel.KeHoach.ThoiGianThucHien;
                keHoachInDb.KhuyenMaiId = viewModel.KeHoach.KhuyenMaiId;
                keHoachInDb.IsActive = viewModel.KeHoach.IsActive;

                if (viewModel.KeHoach.ChiTietKeHoachs != null)
                {
                    for (int i = 0; i < viewModel.KeHoach.ChiTietKeHoachs.Count; i++)
                    {
                        var detail = viewModel.KeHoach.ChiTietKeHoachs.ElementAt(i);
                        detail.NgayTrongKeHoach = i + 1;
                        keHoachInDb.ChiTietKeHoachs.Add(detail);
                    }
                }

                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            await PopulateViewModelDropdowns(viewModel);
            return View(viewModel);
        }

        // GET: KeHoachsAdmin/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null) return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            var keHoach = await db.KeHoachs.FindAsync(id);
            if (keHoach == null) return HttpNotFound();

            if (Request.IsAjaxRequest())
            {
                return PartialView("Delete", keHoach);
            }

            return View(keHoach);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteMultiple(int[] idsToDelete)
        {
            if (idsToDelete != null && idsToDelete.Length > 0)
            {
                var keHoachsToDelete = db.KeHoachs.Where(k => idsToDelete.Contains(k.Id)).ToList();
                if (keHoachsToDelete.Any())
                {
                    db.KeHoachs.RemoveRange(keHoachsToDelete);
                    db.SaveChanges();
                }
            }
            return RedirectToAction("Index");
        }

        // POST: KeHoachsAdmin/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            var keHoach = await db.KeHoachs.FindAsync(id);
            db.KeHoachs.Remove(keHoach);
            await db.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing) db.Dispose();
            base.Dispose(disposing);
        }
    }
}   