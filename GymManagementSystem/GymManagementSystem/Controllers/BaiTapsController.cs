using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using GymManagementSystem.Models;

namespace GymManagementSystem.Controllers
{
    public class BaiTapsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: BaiTaps
        public ActionResult Index()
        {
            return View(db.BaiTaps.ToList());
        }

        // GET: BaiTaps/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            BaiTap baiTap = db.BaiTaps.Find(id);
            if (baiTap == null)
            {
                return HttpNotFound();
            }
            return View(baiTap);
        }

        // GET: BaiTaps/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: BaiTaps/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,TenBaiTap,MoTa,NhomCoChinh,NhomCoPhu,DungCu,MucDo,ImageUrl,VideoUrl")] BaiTap baiTap)
        {
            if (ModelState.IsValid)
            {
                db.BaiTaps.Add(baiTap);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(baiTap);
        }

        // GET: BaiTaps/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            BaiTap baiTap = db.BaiTaps.Find(id);
            if (baiTap == null)
            {
                return HttpNotFound();
            }
            return View(baiTap);
        }

        // POST: BaiTaps/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,TenBaiTap,MoTa,NhomCoChinh,NhomCoPhu,DungCu,MucDo,ImageUrl,VideoUrl")] BaiTap baiTap)
        {
            if (ModelState.IsValid)
            {
                db.Entry(baiTap).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(baiTap);
        }

        // GET: BaiTaps/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            BaiTap baiTap = db.BaiTaps.Find(id);
            if (baiTap == null)
            {
                return HttpNotFound();
            }
            return View(baiTap);
        }

        // POST: BaiTaps/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            BaiTap baiTap = db.BaiTaps.Find(id);
            db.BaiTaps.Remove(baiTap);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        public ActionResult Practice(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            BaiTap baiTap = db.BaiTaps.Find(id);
            if (baiTap == null)
            {
                return HttpNotFound();
            }
            ViewBag.RepGoal = 10;

            // TRUYỀN TÊN LOGIC ĐẾM SANG VIEW
            ViewBag.RepCountingLogicName = baiTap.RepCountingLogic;

            return View(baiTap);
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
