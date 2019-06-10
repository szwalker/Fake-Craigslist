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

namespace UI.Controllers
{
    public class CategoriesController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        public ActionResult UserPost(int CatId, int? LocaleId,int? AreaId,int? page)
        {
            CatPostViewModel catpost = new CatPostViewModel();
            catpost.Cat = Db1.FindCat(CatId);
            if (LocaleId != null && AreaId != null)//area and locale
            {
                catpost.Posts = Db1.CatPostWithLocale(CatId, LocaleId);
            }
            else if (AreaId != null)//only area
            {
                catpost.Posts = Db1.CatPostWithArea(CatId, AreaId);
            }
            else {//only category
                catpost.Posts = Db1.CatPost(CatId);
            }
            //why is there not a only subcat?
           
            
            return View(catpost.Posts.ToList().ToPagedList(page ?? 1,10));
        }

        // GET: Categories
        [CustomRoleCheck(Roles = "Admin")]
        public ActionResult List(int? page)
        {
            return View(Db1.ListAllCategories().ToPagedList(page ?? 1,10));
            //return View(db.Categories.ToList());
        }



        // GET: Categories/Create
        [CustomRoleCheck(Roles = "Admin")]
        public ActionResult Create()
        {
            return View();
        }

        // POST: Categories/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [CustomRoleCheck(Roles = "Admin")]
        public ActionResult Create([Bind(Include = "Name")] Category category)
        {
            category.Viewable = true;
            if (ModelState.IsValid)
            {
                db.Categories.Add(category);
                db.SaveChanges();
                return RedirectToAction("List");
            }

            return View(category);
        }

        // GET: Categories/Edit/5
        [CustomRoleCheck(Roles = "Admin")]
        public ActionResult Modify(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Category category = db.Categories.Find(id);
            if (category == null)
            {
                return HttpNotFound();
            }
            return View(category);
        }

        // POST: Categories/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [CustomRoleCheck(Roles = "Admin")]
        public ActionResult Modify([Bind(Include = "Id,Name,Viewable")] Category category)
        {
            if (ModelState.IsValid)
            {
                
                //category.Viewable = !category.Viewable;
                //if the category has been hidden
                if (!category.Viewable) {
                    Db1.HideSubcategorybasedOnCat(category.Id);
                }
                db.Entry(category).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("List", "Categories");
            }
            return RedirectToAction("List", "Categories");
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
