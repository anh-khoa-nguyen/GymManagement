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
using GymManagementSystem.Services;

namespace GymManagementSystem.Controllers
{
    public class ChiSoSucKhoesController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: ChiSoSucKhoes
        public ActionResult Index()
        {
            var chiSoSucKhoes = db.ChiSoSucKhoes.Include(c => c.HoiVien);
            return View(chiSoSucKhoes.ToList());
        }

        // GET: ChiSoSucKhoes/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ChiSoSucKhoe chiSoSucKhoe = db.ChiSoSucKhoes.Find(id);
            if (chiSoSucKhoe == null)
            {
                return HttpNotFound();
            }
            return View(chiSoSucKhoe);
        }

        // GET: ChiSoSucKhoes/Create
        public ActionResult Create()
        {
            ViewBag.HoiVienId = new SelectList(db.HoiViens, "Id", "MucTieuTapLuyen");
            return View();
        }

        [HttpPost] 
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(ChiSoSucKhoe chiSoSucKhoe)
        {
            if (ModelState.IsValid)
            {
                var hoiVien = await db.HoiViens.FindAsync(chiSoSucKhoe.HoiVienId);
                if (hoiVien != null && hoiVien.ChieuCao > 0)
                {
                    var aiService = new GoogleAiService();

                    // Truyền thẳng đối tượng chiSoSucKhoe và hoiVien vào service
                    string loiKhuyen = await aiService.GenerateHealthAdviceAsync(chiSoSucKhoe, hoiVien);

                    // 4. Gán lời khuyên vào đối tượng trước khi lưu
                    chiSoSucKhoe.LoiKhuyenAI = loiKhuyen;
                }

                db.ChiSoSucKhoes.Add(chiSoSucKhoe);
                await db.SaveChangesAsync();

                // Chuyển hướng đến trang tiến độ của hội viên đó
                return RedirectToAction("XemTienDo", "HoiVien", new { hoiVienId = chiSoSucKhoe.HoiVienId });
            }

            ViewBag.HoiVienId = new SelectList(db.HoiViens, "Id", "ApplicationUser.HoTen", chiSoSucKhoe.HoiVienId);

            return View(chiSoSucKhoe);
        }

        private string GetPhanLoaiBMI(double bmi)
        {
            if (bmi < 18.5) return "Gầy";
            if (bmi < 25) return "Bình thường";
            if (bmi < 30) return "Thừa cân";
            return "Béo phì";
        }

        // GET: ChiSoSucKhoes/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ChiSoSucKhoe chiSoSucKhoe = db.ChiSoSucKhoes.Find(id);
            if (chiSoSucKhoe == null)
            {
                return HttpNotFound();
            }
            ViewBag.HoiVienId = new SelectList(db.HoiViens, "Id", "MucTieuTapLuyen", chiSoSucKhoe.HoiVienId);
            return View(chiSoSucKhoe);
        }

        // POST: ChiSoSucKhoes/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,HoiVienId,NgayCapNhat,CanNang,TyLeMo,ChieuCao,LoiKhuyenAI")] ChiSoSucKhoe chiSoSucKhoe)
        {
            if (ModelState.IsValid)
            {
                db.Entry(chiSoSucKhoe).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.HoiVienId = new SelectList(db.HoiViens, "Id", "MucTieuTapLuyen", chiSoSucKhoe.HoiVienId);
            return View(chiSoSucKhoe);
        }

        // GET: ChiSoSucKhoes/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ChiSoSucKhoe chiSoSucKhoe = db.ChiSoSucKhoes.Find(id);
            if (chiSoSucKhoe == null)
            {
                return HttpNotFound();
            }
            return View(chiSoSucKhoe);
        }

        // POST: ChiSoSucKhoes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            ChiSoSucKhoe chiSoSucKhoe = db.ChiSoSucKhoes.Find(id);
            db.ChiSoSucKhoes.Remove(chiSoSucKhoe);
            db.SaveChanges();
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