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
    
    public class SubcategoriesController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();


        public ActionResult UserPost(int CatId, int SubCatId, int? LocaleId, int? AreaId)
        {
            SubcatPostViewModel subcatpost = new SubcatPostViewModel();
            subcatpost.Sub = Db1.FindSubcat(SubCatId);
            if (LocaleId != null && AreaId != null)
            {
                subcatpost.Posts = Db1.SubcatPostWithLocale(SubCatId, LocaleId);

            }
            else if (AreaId != null)
            {
                subcatpost.Posts = Db1.SubcatPostWithArea(SubCatId,AreaId);
            }
            else {
                subcatpost.Posts = Db1.SubcatPost(SubCatId);
            }
            return View(subcatpost);
        }

        // GET: Subcategories
        [CustomRoleCheck(Roles = "Admin")]
        public ActionResult List(int id1, int? page)
        {           
            return View(Db1.ListAllSubCategories(id1).ToPagedList(page ?? 1,10));
        }

        // GET: Subcategories/Details/5
        [CustomRoleCheck(Roles = "Admin")]
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Subcategory subcategory = db.Subcategories.Find(id);
            if (subcategory == null)
            {
                return HttpNotFound();
            }
            return View(subcategory);
        }
        [CustomRoleCheck(Roles = "Admin")]
        // GET: Subcategories/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Subcategories/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [CustomRoleCheck(Roles = "Admin")]
        public ActionResult Create([Bind(Include = "Id,Name,Viewable")] Subcategory subcategory,int id1)
        {
            if (ModelState.IsValid)
            {
                subcategory.Viewable = true;
                subcategory = Db1.AddSubToCat(subcategory,id1);
                db.Subcategories.Add(subcategory);
                return RedirectToAction("List", "Subcategories");
            }

            return RedirectToAction("List", "Subcategories");
        }

        // GET: Subcategories/Edit/5
        [CustomRoleCheck(Roles = "Admin")]
        public ActionResult Modify(int? id1, int? id2)
        {
            if (id2 == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Subcategory subcategory = db.Subcategories.Find(id2);
            if (subcategory == null)
            {
                return HttpNotFound();
            }
            ViewBag.catId = id1;
            return View(subcategory);
        }

        // POST: Subcategories/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [CustomRoleCheck(Roles = "Admin")]
        public ActionResult Modify([Bind(Include = "Id,Name,Viewable")] Subcategory subcategory,int id1, int id2)
        {
            if (ModelState.IsValid)
            {
                //subcategory.Viewable = !subcategory.Viewable;
                Db1.hidePostBasedOnSubCat(id2);
                db.Entry(subcategory).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("List"); 
            }
            ViewBag.catId = id1;
            return View(subcategory);

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
