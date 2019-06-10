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
using Microsoft.AspNet.Identity;
using PagedList;
using PagedList.Mvc;

namespace UI.Controllers
{
    public class PostsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        [CustomRoleCheck(Roles = "Admin, User")]
        public ActionResult DetermineAreaCat(string uid)
        {
            List<Category> cats = Db1.ListAllCategories();
            List<Area> areas = Db1.ListAllArea();
            CatAreaViewModel catPost = new CatAreaViewModel();
            catPost.Areas = areas;
            catPost.Categories = cats;
            return View(catPost);
            //return View(Db1.areas.FirstOrDefault());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [CustomRoleCheck(Roles = "Admin, User")]
        public ActionResult DetermineAreaCat(string Areas, string Categories, string uid)
        {
            if ((Areas.Equals("") || Areas == null) || (Categories == null || Categories.Equals("")))
            {
                ModelState.AddModelError("Categories", "Both of the Areas and Categories can't be empty!");
                List<Category> cats = Db1.ListAllCategories();
                List<Area> areas = Db1.ListAllArea();
                CatAreaViewModel catPost = new CatAreaViewModel();
                catPost.Areas = areas;
                catPost.Categories = cats;
                return View(catPost);
            }
            else {
                return RedirectToAction("Create","Posts",new {AreaId=Areas,CatId = Categories});
            }
            

        }

        // GET: Posts
        [CustomRoleCheck(Roles = "Admin")]
        public ActionResult List(string uid,int? page)
        {
            return View(Db1.findPostbyUidIncludingNonViewable(uid).ToPagedList(page ?? 1, 10));
        }

        // GET: Posts
        [CustomRoleCheck(Roles = "Admin, User")]
        public ActionResult UserPostList(int? page)
        {
            var userId = User.Identity.GetUserId();
            return View(Db1.findPostbyUid(userId).ToPagedList(page ?? 1, 10));
        }

        [CustomRoleCheck(Roles = "Admin, User")]
        public ActionResult ExpiredPosts(int? page)
        {
            var userId = User.Identity.GetUserId();
            return View(Db1.findOneExpiredPosts(userId).ToPagedList(page ?? 1, 10));
        }


        // GET: Posts/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Post post = db.Posts.Find(id);
            if (!post.Viewable||post == null)
            {
                return HttpNotFound();
            }
            return View(post);
        }

        // GET: Posts/Create
        [CustomRoleCheck(Roles = "Admin, User")]
        public ActionResult Create(string AreaId, string CatId)
        {
            if (AreaId == null || AreaId.Equals("") || CatId == null || CatId.Equals(""))
                return HttpNotFound();
            
            var SpecificCatAreaViewModel = new SpecificCatAreaViewModel();
            SpecificCatAreaViewModel.Are = Db1.FindArea(Int32.Parse(AreaId));
            SpecificCatAreaViewModel.Cat = Db1.FindCat(Int32.Parse(CatId));
            return View(SpecificCatAreaViewModel);

        }

        // POST: Posts/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [CustomRoleCheck(Roles = "Admin, User")]
        public ActionResult Create(string Body,string Title,string Locales,string Subcategories,string AreaId,string CatId)
        {

            if (Body == null || Body.Equals("") || Title == null || Title.Equals("") || Request["Are.Locales"] == null || Request["Are.Locales"].Equals("") || Request["Cat.Subcategories"] == null || Request["Cat.Subcategories"].Equals("")) {
                ModelState.AddModelError("Are", "All the above values must be filled in order to proceed.");
                var SpecificCatAreaViewModel = new SpecificCatAreaViewModel();
                SpecificCatAreaViewModel.Are = Db1.FindArea(Int32.Parse(AreaId));
                SpecificCatAreaViewModel.Cat = Db1.FindCat(Int32.Parse(CatId));
                //Db1.purge();
                return View(SpecificCatAreaViewModel);
            }

            Db1.CreatePost(Title, Body, Request["Are.Locales"], AreaId, CatId, Request["Cat.Subcategories"], User.Identity.GetUserId());
            return RedirectToAction("UserPostList");



            
        }

        // GET: Posts/Edit/5
        [CustomRoleCheck(Roles = "Admin, User")]
        public ActionResult Edit(int? id)
        {
            
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Post post = db.Posts.Find(id);
            if (post == null || !post.Viewable)
            {
                return HttpNotFound();
            }
            return View(post);
        }

        // POST: Posts/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [CustomRoleCheck(Roles = "Admin, User")]
        public ActionResult Edit([Bind(Include = "Id,Title,Body")] Post post)
        {
            
            Db1.ChangePost(post.Id, post.Title, post.Body);
            return RedirectToAction("UserPostList");
        }


        // GET: Posts/Delete/5
        [CustomRoleCheck(Roles = "Admin")]
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Post post = db.Posts.Find(id);
            if (post == null || !post.Viewable)
            {
                return HttpNotFound();
            }
            return View(post);
        }

        // POST: Posts/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [CustomRoleCheck(Roles = "Admin")]
        public ActionResult DeleteConfirmed(int id)
        {
            

            Db1.hidePost(id);
            return RedirectToAction("ListUsers","Account");
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
