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

namespace UI.Controllers
{
    public class LocalesController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        public ActionResult UserPost(int LocaleId, int? CatId, int? SubCatId)
        {
            LocalePostViewModel localePost = new LocalePostViewModel();
            if (CatId != null && SubCatId != null)
            {
                localePost.Posts = Db1.LocalePostCat(LocaleId,SubCatId);
            }
            else {
                localePost.Posts = Db1.LocalePostSubCat(LocaleId, CatId);
            }
            
            return View(localePost);
        }

        // GET: Locales
        [CustomRoleCheck(Roles = "Admin")]
        public ActionResult List(int id1, int? page)
        {
            ViewBag.areaId = id1;
            ViewBag.areaName = db.Areas.Find(id1).Name;
            return View(Db1.ListLocaleInArea(id1).ToPagedList(page ?? 1,10));
        }

        // GET: Locales/Details/5
        [CustomRoleCheck(Roles = "Admin")]
        public ActionResult Details(int id2)
        {
       
            Locale locale = db.Locales.Find(id2);
            if (locale == null)
            {
                return HttpNotFound();
            }
            return View(locale);
        }

        // GET: Locales/Create
        [CustomRoleCheck(Roles = "Admin")]
        public ActionResult Create()
        {
            return View();
        }

        // POST: Locales/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [CustomRoleCheck(Roles = "Admin")]
        public ActionResult Create([Bind(Include = "Id,Name")] Locale locale, int id1)
        {
            if (ModelState.IsValid)
            {
                locale.Area_obj = db.Areas.Find(id1);//get the area object and attach it to the new area
                locale.Viewable = true; // set local viewable to true
                db.Locales.Add(locale);
                db.SaveChanges();
                return RedirectToAction("List");
            }

            return View(Db1.ListLocaleInArea(id1));
        }

        // GET: Locales/Edit/5
        [CustomRoleCheck(Roles = "Admin")]
        public ActionResult Edit(int? id1, int id2)
        {
            /*
            if (id2 == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }*/
            Locale locale = db.Locales.Find(id2);
            if (locale == null)
            {
                return HttpNotFound();
            }
            return View(locale);
        }

        // POST: Locales/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [CustomRoleCheck(Roles = "Admin")]
        public ActionResult Edit([Bind(Include = "Id,Viewable,Name")] Locale locale)
        {
            if (ModelState.IsValid)
            {
                Db1.hidePostBasedOnLocale(locale.Id);
                db.Entry(locale).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("List");
            }
            return View(locale);
        }

        // GET: Locales/Delete/5
        [CustomRoleCheck(Roles = "Admin")]
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Locale locale = db.Locales.Find(id);
            if (locale == null)
            {
                return HttpNotFound();
            }
            return View(locale);
        }

        // POST: Locales/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [CustomRoleCheck(Roles = "Admin")]
        public ActionResult DeleteConfirmed(int id)
        {
            Locale locale = db.Locales.Find(id);
            db.Locales.Remove(locale);
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
