using Data.Models;
using Microsoft.AspNet.Identity.EntityFramework;
using Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BizLogic.Logic;
using Microsoft.AspNet.Identity;
using System.Web.Security;

namespace DB.Database
{

    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext()
            : base("DefaultConnection", throwIfV1Schema: false)
        {
        }

        public static ApplicationDbContext Create()
        {
            return new ApplicationDbContext();
        }
        public System.Data.Entity.DbSet<Area> Areas { get; set; }
        public System.Data.Entity.DbSet<Category> Categories { get; set; }
        public System.Data.Entity.DbSet<Subcategory> Subcategories { get; set; }
        public System.Data.Entity.DbSet<Post> Posts { get; set; }
        public System.Data.Entity.DbSet<Message> Messages { get; set; }
        public System.Data.Entity.DbSet<Locale> Locales { get; set; }

    }


    public class Db1
    {

        public static string GetUserRoleById(string uid)
        {
            using (var db = new ApplicationDbContext())
            {
                var UserManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(db));
                return UserManager.GetRoles(uid).FirstOrDefault();
            }

        }

        public static void hideLocaleBasedOnArea(int areaId) {
            HideAreaPost(areaId);
            using (var db = new ApplicationDbContext())
            {
                var SubAreaList = from loc in db.Locales
                                     where loc.Area_obj.Id == areaId
                                     select loc;
                List<Locale> locales = SubAreaList.ToList();
                foreach (var sub in locales)
                {
                    sub.Viewable = false;
                }
                db.SaveChanges();
                return;
            }
        }
        public static void HideAreaPost(int catId) {
            using (var db = new ApplicationDbContext()) {
                var allPosts = from p in db.Posts
                               where p.PostCategory.Id == catId
                               select p;
                var postLists = allPosts.ToList();
                foreach (var pp in postLists) {
                    pp.Viewable = false;
                }
                db.SaveChanges();
                return;
            }

        }



        public static string GetPostStatus(int PostId)
        {
            using (var db = new ApplicationDbContext())
            {
                var postQ = from p in db.Posts
                            where p.Id == PostId
                            select p;
                Post po = postQ.FirstOrDefault();
                // if the post is null, not viewable(hidden), or expired
                if (po == null || po.Viewable == false || po.Expiration <= DateTime.Now)
                {
                    return "Invalid (expired or deleted)";
                }
                else
                {
                    return "Active";
                }
            }
        }

        /// <summary>
        /// Get all inbox message for a user with a given user id
        /// </summary>
        /// <param name="uid"></param>
        /// <returns></returns>
        public static List<Message> GetInBoxMessage(string uid)
        {
            using (var db = new ApplicationDbContext())
            {
                var messageQ = from m in db.Messages
                               where m.ReceiverId == uid
                               orderby m.Id descending
                               select m;
                return messageQ.ToList();
            }
        }

        public static void hidePostBasedOnLocale(int LocaleId)
        {
            using (var db = new ApplicationDbContext())
            {
                var postList = from post in db.Posts
                               where post.PostLocale.Id == LocaleId
                               select post;
                var p = postList.ToList();
                foreach (var i in p)
                {
                    i.Viewable = false;
                }
                db.SaveChanges();
                return;
            }
        }

        public static string GetPostTitle(int PostId)
        {
            using (var db = new ApplicationDbContext())
            {
                var postQ = from p in db.Posts
                            where p.Id == PostId
                            select p;
                return postQ.FirstOrDefault().Title;

            }
        }

        public static List<Post> findPostbyUidIncludingNonViewable(string uid)
        {
            using (var db = new ApplicationDbContext())
            {
                var p = from post in db.Posts
                        where post.Owner.Id.Equals(uid)
                        orderby post.Title
                        select post;
                return p.ToList();
            }


        }


        //find all the posts related to a specific subcategories
        public static List<Post> SubcatPost(int subcatid)
        {
            using (var db = new ApplicationDbContext())
            {
                var p = from post in db.Posts
                        where post.PostSubcategory.Id == subcatid && post.Expiration >= DateTime.Now && post.Viewable == true
                        orderby post.Title
                        select post;
                return p.ToList();
            }
        }



        /// <summary>
        /// get a specific applicationuser with a given user id
        /// </summary>
        /// <param name="uid"></param>
        /// <returns></returns>
        public static ApplicationUser GetApplicationUser(string uid)
        {
            using (var db = new ApplicationDbContext())
            {
                var userQ = from u in db.Users
                            where u.Id.Equals(uid)
                            select u;
                return userQ.FirstOrDefault();
            }
        }

        public static List<ApplicationUser> AllUserList()
        {
            using (var db = new ApplicationDbContext())
            {
                return db.Users.ToList();
            }
        }




        public static List<Post> findPostbyUid(string uid)
        {
            using (var db = new ApplicationDbContext())
            {
                var p = from post in db.Posts
                        where post.Owner.Id.Equals(uid) && post.Expiration > DateTime.Now && post.Viewable==true
                        orderby post.Title
                        select post;
                return p.ToList();


            }
        }

        public static void hidePost(int id) {
            using (var db = new ApplicationDbContext()) {
                var p = from post in db.Posts
                        where post.Id == id
                        select post;

                var targetPost = p.FirstOrDefault();
                targetPost.Viewable = false;
                db.SaveChanges();
            }
        }


        /// <summary>
        /// returns true if the post is not expired and is viewable, false otherwise
        /// </summary>
        /// <param name="p"></param>
        /// <returns></returns>
        public static bool DetermineExpiredOrNotViewable(Post p)
        {
            return p.Viewable && p.Expiration >= DateTime.Now;
        }

        public static List<Post> findOneExpiredPosts(string userId) {
            using (var db = new ApplicationDbContext())
            {
                var p = from post in db.Posts
                        where post.Owner.Id.Equals(userId) && post.Expiration <= DateTime.Now && post.Viewable==true
                        orderby post.Title
                        select post;
                return p.ToList();
            }

        }



        //find all the posts related to a specific subcategories
        public static List<Post> SubcatPostWithLocale (int subcatid,int? localeid)
        {
            using (var db = new ApplicationDbContext())
            {
                var p = from post in db.Posts
                        where post.PostSubcategory.Id == subcatid && post.Expiration >= DateTime.Now && post.PostLocale.Id== localeid && post.Viewable==true
                        orderby post.Title
                        select post;
                return p.ToList();
            }
        }

        //find all the posts related to a specific subcategories
        public static List<Post> SubcatPostWithArea(int subcatid, int? AreaId)
        {
            using (var db = new ApplicationDbContext())
            {
                var p = from post in db.Posts
                        where post.PostSubcategory.Id == subcatid && post.Expiration >= DateTime.Now && post.PostArea.Id== AreaId && post.Viewable == true
                        orderby post.Title
                        select post;
                return p.ToList();
            }
        }

        public static Subcategory FindSubcat(int subcatid)
        {
            using (var db = new ApplicationDbContext())
            {
                var p = from sub in db.Subcategories
                        where sub.Id == subcatid
                        orderby sub.Name
                        select sub;
                return p.FirstOrDefault();
            }
        }

        //find all the posts related to a specific categories
        public static List<Post> CatPost(int Catid)
        {
            using (var db = new ApplicationDbContext())
            {
                var p = from post in db.Posts
                        where post.PostCategory.Id == Catid && post.Expiration >= DateTime.Now && post.Viewable == true
                        orderby post.Title
                        select post;
                return p.ToList();
            }
        }

        //find all the posts related to a specific categories
        public static List<Post> CatPostWithArea(int Catid,int? AreaId)
        {
            using (var db = new ApplicationDbContext())
            {
                var p = from post in db.Posts
                        where post.PostCategory.Id == Catid && post.Expiration >= DateTime.Now && post.PostArea.Id==AreaId && post.Viewable == true
                        orderby post.Title
                        select post;
                return p.ToList();
            }
        }

        //find all the posts related to a specific categories
        public static List<Post> CatPostWithLocale(int Catid,int? LocaleId)
        {
            using (var db = new ApplicationDbContext())
            {
                var p = from post in db.Posts
                        where post.PostCategory.Id == Catid && post.Expiration >= DateTime.Now && LocaleId==post.PostLocale.Id && post.Viewable == true
                        orderby post.Title
                        select post;
                return p.ToList();
            }
        }

        public static Category FindCat(int Catid)
        {
            using (var db = new ApplicationDbContext())
            {
                var p = from cat in db.Categories.Include("Subcategories")
                        where cat.Id == Catid
                        orderby cat.Name
                        select cat;
                return p.FirstOrDefault();
            }
        }

        public static List<Post> LocalePostCat(int localeid, int? catid) {
            using (var db = new ApplicationDbContext()) {
                var p = from post in db.Posts
                        where post.PostLocale.Id == localeid && post.Expiration >= DateTime.Now && post.PostCategory.Id==catid && post.Viewable == true
                        orderby post.Title
                        select post;
                return p.ToList();
            }
        }

        public static List<Post> LocalePostSubCat(int localeid, int? subcatid)
        {
            using (var db = new ApplicationDbContext())
            {
                var p = from post in db.Posts
                        where post.PostLocale.Id == localeid && post.Expiration >= DateTime.Now && post.PostSubcategory.Id==subcatid && post.Viewable == true
                        orderby post.Title
                        select post;
                return p.ToList();
            }
        }

        public static Locale FindLocale(int localeid)
        {
            using (var db = new ApplicationDbContext())
            {
                var p = from loc in db.Locales
                        where loc.Id == localeid
                        orderby loc.Name
                        select loc;
                return p.FirstOrDefault();
            }
        }


        public static List<Post> AreaPostCat(int areaid,int? CatId)
        {
            using (var db = new ApplicationDbContext())
            {
                var p = from post in db.Posts
                        where post.PostArea.Id == areaid && post.Expiration >= DateTime.Now && post.PostCategory.Id==CatId && post.Viewable == true
                        orderby post.Title
                        select post;
                return p.ToList();
            }
        }

        public static List<Post> AreaPostSubCat(int areaid,int? SubCatId)
        {
            using (var db = new ApplicationDbContext())
            {
                var p = from post in db.Posts
                        where post.PostArea.Id == areaid && post.Expiration >= DateTime.Now && post.PostSubcategory.Id == SubCatId && post.Viewable == true
                        orderby post.Title
                        select post;
                return p.ToList();
            }
        }


        //find area based on area id
        public static Area FindArea(int areaid)
        {
            using (var db = new ApplicationDbContext())
            {
                var p = from loc in db.Areas.Include("Locales")
                        where loc.Id == areaid
                        orderby loc.Name
                        select loc;
                return p.FirstOrDefault();
            }
        }



        public static void DoDatabaseOperation()
        {
            // You can place your models into ApplicationDbContext
            // or create your own context
            using (var db = new ApplicationDbContext())
            {
                // Create and manipulate your model

                // Call into biz logic

                // Do something useful here

                db.SaveChanges();
            }
        }
        public static void CreatePost(string Title, string Body, string LocaleId, string AreaId, string CatId, string SubCatId, string ownerId)
        {
            using (var db = new ApplicationDbContext())
            {
                Post post = new Post();
                post.Title = Title;
                post.Body = Body;
                //[Bind(Include = "Id,Date,Expiration,Title,Body,Viewable")] Post post
                post.Viewable = true;
                post.Date = DateTime.Now;
                post.Expiration = DateTime.Now.AddDays(5);
                post.PostCategory = db.Categories.Find(Int32.Parse(CatId));
                post.PostArea = db.Areas.Find(Int32.Parse(AreaId));
                post.PostLocale = db.Locales.Find(Int32.Parse(LocaleId));
                post.PostSubcategory = db.Subcategories.Find(Int32.Parse(SubCatId));
                post.Owner = db.Users.Find(ownerId);
                db.Posts.Add(post);
                db.SaveChanges();
            }

        }

        public static List<Category> ListAllTheCategories()
        {
            using (var db = new ApplicationDbContext())
            {
                var p = from cat in db.Categories
                        where cat.Viewable == true
                        orderby cat.Name
                        select cat;
                return p.ToList();
            }
        }

        



        /// <summary>
        /// The function queries all the areas from database, exclude the given area, and sort 
        /// the rest in alphabetical order.
        /// </summary>
        /// <returns></returns>
        public static List<Area> ListAreas()
        {
            using (var db = new ApplicationDbContext())
            {
                var AreaQuery = from a in db.Areas.Include("Locales")
                                where a.Viewable == true
                                orderby a.Name
                                select a;
                return AreaQuery.ToList();
            }
        }

        /// <summary>
        /// The function queries all the locale from database and sort 
        /// them in alphabetical order.
        /// </summary>
        /// <returns></returns>
        public static List<Locale> ListLocales()
        {
            using (var db = new ApplicationDbContext())
            {
                var LocaleQuery = from locale in db.Locales
                                  orderby locale.Name ascending
                                  select locale;
                return LocaleQuery.ToList();
            }
        }

        
        public static List<Locale> ListLocaleInArea(int AreaId)
        {
            using(var db = new ApplicationDbContext())
            {
                var LocaleQuery = from Locale in db.Locales
                                  where Locale.Area_obj.Id == AreaId && Locale.Viewable == true
                                  orderby Locale.Name ascending
                                  select Locale;
                return LocaleQuery.ToList();
            }
        }
        public static List<Area> ListViewableAreas()
        {
            using (var db = new ApplicationDbContext())
            {
                var AreaQuery = from area in db.Areas
                                where area.Viewable == true
                                orderby area.Name ascending
                                select area;
                return AreaQuery.ToList();
            }
        }

        /// <summary>
        /// gets a post object by post id
        /// </summary>
        /// <returns></returns>
        public static Post GetPostById(int PostId) 
        {
            using (var db = new ApplicationDbContext())
            {
                var p = from post in db.Posts.Include("Owner")
                        where post.Id == PostId
                        select post;
                return p.FirstOrDefault();
            }
        }

        /// <summary>
        /// List<Post> of expired and unexpired posts created by the User
        ///Description: Lists all posts of a given user, regardless whether the posts are expired or not
        /// </summary>
        /// <returns>List<Post> post</returns>
        public static List<Post> ListPosts (ApplicationUser user) {
            
            List<Post> posts = new List<Post>();
            using (var db = new ApplicationDbContext())
            {
                var postsObj = from post in db.Posts
                             where (user.Id == post.Owner.Id)
                             select post;
                posts = postsObj.ToList();
                posts = Biz1.ProcessExpiredPost(posts);
                db.SaveChanges();
            }

            return posts;

        }





        /// <summary>
        /// hide the category by set the to false and all its subcategories' viewable to false
        /// </summary>
        /// <returns>List<Post> post</returns>
        public static void HideCategory(string CategoryName) {
            
            using (var db = new ApplicationDbContext())
            {

                var catInDbList = from cat in db.Categories
                               where cat.Name.Equals(CategoryName)
                               select cat;
                var catInDb = catInDbList.FirstOrDefault();
                catInDb.Viewable = false;
                //hide all the subcategories in the category
                foreach (var subcat in catInDb.Subcategories) {
                    subcat.Viewable = false;
                }
                db.SaveChanges();
            }
        }


        /// <summary>
        /// unhide the category by set the viewbale to true and the subcategories to false
        /// </summary>
        /// <returns>List<Post> post</returns>
        public static void ShowCategory(string CategoryName)
        {
            using (var db = new ApplicationDbContext())
            {

                var catInDbList = from cat in db.Categories
                                  where cat.Name.Equals(CategoryName)
                                  select cat;
                var catInDb = catInDbList.FirstOrDefault();
                catInDb.Viewable = true;
                foreach (var subcat in catInDb.Subcategories)
                {
                    subcat.Viewable = false;
                }
                db.SaveChanges();
            }
        }

        ///<summary>
        ///hide all the subcategory belongs to a category
        ///</summary>
        ///return nothing
        public static void HideSubcategorybasedOnCat(int catID)
        {
            HideSubCatPost(catID);
            using (var db = new ApplicationDbContext())
            {
                var extractCatList = from cat in db.Categories
                                     where cat.Id == catID
                                     select cat;
                Category targetCat = extractCatList.FirstOrDefault();
                foreach (var sub in targetCat.Subcategories) {
                    HideSubcategory(sub);
                }
                db.SaveChanges();
                return;
            }
        }

        public static void hidePostBasedOnSubCat(int SubId) {
            using (var db = new ApplicationDbContext())
            {
                var postList = from post in db.Posts
                               where post.PostSubcategory.Id == SubId
                               select post;
                var p = postList.ToList();
                foreach (var i in p)
                {
                    i.Viewable = false;
                }
                db.SaveChanges();
                return;
            }
        }

        public static void HideSubCatPost(int catID)
        {

            using (var db = new ApplicationDbContext())
            {
                var postList = from post in db.Posts
                               where post.PostCategory.Id == catID
                               select post;
                var p = postList.ToList();
                foreach (var i in p) {
                    i.Viewable = false;
                }
                db.SaveChanges();
                return;
            }
        }


        /// <summary>
        /// hide the subcategory by set the viewbale to false
        /// </summary>
        /// <returns>List<Post> post</returns>
        public static void HideSubcategory(Subcategory subcategory)
        {
            
            using (var db = new ApplicationDbContext())
            {
                var subList = from subCat in db.Subcategories
                              where subCat.Id == subcategory.Id
                              select subCat;
                var sub = subList.FirstOrDefault();
                sub.Viewable = false;
                subcategory.Viewable = false;
                db.SaveChanges();
            }
        }





        /// <summary>
        /// unhide the subcategory by set the viewbale to true
        /// </summary>
        /// <returns>List<Post> post</returns>
        public static void ShowSubcategory(Subcategory subcategory)
        {

            using (var db = new ApplicationDbContext())
            {
                subcategory.Viewable = true;
                db.SaveChanges();
            }
        }



        /// <summary>
        /// Filter All Posts according to area, locale, category and subcategory, it is a db qury function 
        /// that returns a list of Posts 
        /// </summary>
        /// <param name="area"></param>
        /// <param name="locale"></param>
        /// <param name="category"></param>
        /// <param name="subcategory"></param>
        /// <returns></returns>
        public static List<Post> FilterPost(Area area, Locale locale, Category category, Subcategory subcategory)
        {
            using (var db = new ApplicationDbContext())
            {
                //first the whole area and category
                Area tempArea;
                Category tempCat;

                if (locale == null) tempArea = area;
                else tempArea = locale.Area_obj;

                if (subcategory == null) tempCat = category;
                else tempCat = subcategory.Cat;
                                             
                var widePost = from p in db.Posts
                               where p.PostArea.Equals(tempArea) && p.PostCategory.Equals(tempCat)
                               select p;

                if(locale !=null)//if locale is specified
                {
                    var narrowPost = from p in widePost
                                     where p.PostLocale.Equals(locale)
                                     select p;
                    if (subcategory != null)//if locale and subcategory are specified
                    {
                        var narrowerPost = from p in narrowPost
                                           where p.PostSubcategory.Equals(subcategory)
                                           select p;
                        return narrowerPost.ToList();//*Filter: locale & subcategory

                    }
                    return narrowPost.ToList(); //*Filter: locale
                }
                else if(subcategory != null)//if locale is NOT specified by subcategory is specified
                {
                    var narrowPost = from p in widePost
                                     where p.PostSubcategory.Equals(subcategory)
                                     select p;
                    return narrowPost.ToList(); //*Filter: subcategory
                }
                //if locale and subcategory are null, return the widest filter
                return widePost.ToList(); //*Filter: Area & Category
            }


        }

        /// <summary>
        /// List all categories that are not hiddden.
        /// </summary>
        /// <returns>List<Category> </returns>
        public static List<Category> ListAllCategories()
        {
            List<Category> allCatList = null;
            
            using (var db = new ApplicationDbContext())
            {
                var allCats  = from p in db.Categories.Include("Subcategories")
                               where p.Viewable == true
                               orderby p.Name
                               select p;
                allCatList = allCats.ToList();
            }
            return allCatList;
        }

        public static List<Area> ListAllArea()
        {

            using (var db = new ApplicationDbContext())
            {

                var areas = from area in db.Areas
                              where (area.Viewable == true)
                              orderby area.Name
                              select area;
                
                return areas.ToList();
            }

        }


        public static List<Subcategory> ListAllSubCategories(int catId)
        {

            using (var db = new ApplicationDbContext())
            {

                var subcats = from p in db.Subcategories
                              where (p.Viewable == true && p.Cat.Id == catId)
                              orderby p.Name
                              select p;
                List<Subcategory> allSubList = subcats.ToList();
                return allSubList;
            }

        }

        /// <summary>
        /// add subcategory to the category
        /// </summary>
        /// <returns>List<Category> </returns>
        public static Subcategory AddSubToCat(Subcategory sub,int id1)
        {

            using (var db = new ApplicationDbContext())
            {

                var catList = from p in db.Categories
                          where (p.Viewable == true && p.Id == id1)
                          select p;
                var cat= catList.FirstOrDefault();
                sub.Cat = cat;
                
                cat.Subcategories.Add(sub);
                db.Subcategories.Add(sub);
                db.SaveChanges();
                return sub;
            }

        }

        /// <summary>
        /// Assuem only the admin user have the authorization to access this function 
        /// </summary>
        /// <returns>List<Category> </returns>
        public static Category AddCategory(string categoryName)
        {
            Category category = new Category()
            {
                Name = categoryName,
                Subcategories = new List<Subcategory>(),
                Viewable = true
            };
            return category;
        }





        /// <summary>
        /// modify the title and body of the post 
        /// </summary>
        /// <param name="owner"></param>
        /// <param name="post"></param>
        /// <param name="title"></param>
        /// <param name="body"></param>
        public static void ModiftyPost( Post post, string title, string body, Category category, Subcategory subcategory, Locale local, Area area)
        {   
            using (var db = new ApplicationDbContext())
            {
                Biz1.Modify(post, title, body, category, subcategory, local, area);
                db.SaveChanges();
            }
        }

        public static void ChangePost(int PostId, string Title, string Body) 
        {
            using (var db = new ApplicationDbContext())
            {
                var postList = from p in db.Posts
                               where p.Id == PostId
                               select p;
                var truePost = postList.FirstOrDefault();
                truePost.Title = Title;
                truePost.Body = Body;
                truePost.Date = DateTime.Now;
                truePost.Expiration = DateTime.Now.AddDays(5);
                db.SaveChanges();
            }
        }



        /// <summary>
        /// change the current user level to Admin
        /// </summary>
        /// <param name="user"></param>
        public static void ChangeToAdmin(ApplicationUser user)
        {

            using (var db = new ApplicationDbContext())
            {

                var roleManager = new RoleManager<IdentityRole>(new RoleStore<IdentityRole>(db));
                var UserManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(db));

                var oldUser = UserManager.FindById(user.Id);
                var oldRoleId = oldUser.Roles.SingleOrDefault().RoleId;
                var oldRoleName = db.Roles.SingleOrDefault(r => r.Id == oldRoleId).Name;

                if (oldRoleName != "Admin")
                {
                    UserManager.RemoveFromRole(user.Id, oldRoleName);
                    UserManager.AddToRole(user.Id, "Admin");
                }

                db.SaveChanges();
               

            }


        }







        /// <summary>
        /// upgrade the user rolw to admin from given user Id
        /// </summary>
        /// <param name="Id"></param>
        public static void UpgradeToAdmin(String UserId)
        {
            using (var db = new ApplicationDbContext())
            {
                UserManager<ApplicationUser> UserManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(db));
                ApplicationUser user = UserManager.FindById(UserId); // UserId is from User.Identity.GetUserId();
                ChangeToAdmin(user); // change to admin level
                db.SaveChanges();
            }
        }


        /// <summary>
        /// delete a post from databse 
        /// </summary>
        /// <param name="post"></param>
        public static void DeletePost(Post post)
        {
            using (var db = new ApplicationDbContext())
            {
                db.Posts.Remove(post);
                db.SaveChanges();
            }
        }


    }
}

