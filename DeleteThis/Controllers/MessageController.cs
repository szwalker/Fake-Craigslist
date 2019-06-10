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
using Models;
using Microsoft.AspNet.Identity;
using PagedList;
using PagedList.Mvc;

namespace UI.Controllers
{
    [CustomRoleCheck(Roles = "Admin, User")]
    public class MessageController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Message
        public ActionResult Inbox(int? page)
        {
            List<Message> res = Db1.GetInBoxMessage(User.Identity.GetUserId());
            return View(res.ToPagedList(page?? 1,10));
        }

        // GET: Message/Details/5
        public ActionResult Details(int? MessageId)
        {
            if (MessageId == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Message message = db.Messages.Find(MessageId);
            if (message == null)
            {
                return HttpNotFound();
            }
            ViewBag.PostTitle = Db1.GetPostTitle(message.PostId);
            ViewBag.PostId = message.PostId;
            ViewBag.PostStatus = Db1.GetPostStatus(message.PostId);
            ViewBag.SenderEmail = db.Users.Find(message.SenderId).UserName;
            return View(message);
        }

        // GET: Message/Create
        /// <summary>
        /// extract post id from url and render compose message screen
        /// </summary>
        /// <param name="PostId"></param>
        /// <returns></returns>
        [Authorize]
        public ActionResult Create(int PostId)
        {
            Post p = Db1.GetPostById(PostId);
            // post do not exist
            if (p is null)
            {
                return new HttpNotFoundResult("Cannot locate the post.");
            }
            // the post is valid
            if (Db1.DetermineExpiredOrNotViewable(p))
            {
                ViewBag.PostTitle = p.Title;
                return View();
            }
            else
            {
                return new HttpNotFoundResult("The post was deleted or expired");
            }
        }

        // POST: Message/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public ActionResult Create([Bind(Include = "Subject,Content")] Message message, int PostId)
        {
            if (ModelState.IsValid)
            {
                Post p = Db1.GetPostById(PostId);
                message.ReceiverId = p.Owner.Id;
                message.SenderId = db.Users.Find(User.Identity.GetUserId()).Id;
                message.PostId = p.Id;
                db.Messages.Add(message);
                db.SaveChanges();
                return RedirectToAction("Inbox");
            }

            return View(message);
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
