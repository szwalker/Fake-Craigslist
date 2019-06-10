using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using DB.Database;
using Data.Models;
using PagedList;
using PagedList.Mvc;
using System.Web.UI;

namespace UI.Controllers
{
    public class AreasController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        public ActionResult UserPost(int AreaId,int? CatId, int? SubCatId)
        {
            
            AreaViewModel areaPost = new AreaViewModel();
            if (CatId != null && SubCatId != null)
            {
                areaPost.Posts = Db1.AreaPostSubCat(AreaId, SubCatId);
            }
            else {
                areaPost.Posts = Db1.AreaPostCat(AreaId,CatId);
            }
            return View(areaPost);
        }

        // GET: Areas
        [CustomRoleCheck(Roles = "Admin")]
        public ActionResult List(int ? page)
        {
            return View(Db1.ListViewableAreas().ToList().ToPagedList(page ?? 1, 10));
        }

        // GET: Areas/Details/5
        [CustomRoleCheck(Roles = "Admin")]
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Area area = db.Areas.Find(id);
            if (area == null)
            {
                return HttpNotFound();
            }
            return View(area);
        }

        // GET: Areas/Create
        [CustomRoleCheck(Roles = "Admin")]
        public ActionResult Create()
        {
            return View();
        }

        // POST: Areas/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [CustomRoleCheck(Roles = "Admin")]
        public ActionResult Create([Bind(Include = "Id,Name")] Area area)
        {
            if (ModelState.IsValid)
            {
                area.Viewable = true;
                db.Areas.Add(area);
                db.SaveChanges();
                return RedirectToAction("List");
            }

            return View(Db1.ListViewableAreas());
        }

        // GET: Areas/Edit/5
        [CustomRoleCheck(Roles = "Admin")]
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Area area = db.Areas.Find(id);
            if (area == null)
            {
                return HttpNotFound();
            }
            return View(area);
        }

        // POST: Areas/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [CustomRoleCheck(Roles = "Admin")]
        public ActionResult Edit([Bind(Include = "Id,Viewable,Name")] Area area)
        {
            if (ModelState.IsValid)
            {
                Db1.hideLocaleBasedOnArea(area.Id);
                db.Entry(area).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("List");
            }
            return View(Db1.ListViewableAreas());
        }

        // GET: Areas/Delete/5
        [CustomRoleCheck(Roles = "Admin")]
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Area area = db.Areas.Find(id);
            if (area == null)
            {
                return HttpNotFound();
            }
            return View(area);
        }

        // POST: Areas/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [CustomRoleCheck(Roles = "Admin")]
        public ActionResult DeleteConfirmed(int id)
        {
            Area area = db.Areas.Find(id);
            db.Areas.Remove(area);
            db.SaveChanges();
            return RedirectToAction("List");
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
