using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using BizLogic.Logic;
using DB.Database;
using Models;
using Data.Models;
using System.Collections.Generic;
using Microsoft.AspNet.Identity;
using System.Linq;


namespace UnitTests
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void dbConnectionTest()
        {
            using (var db = new ApplicationDbContext())
            {
                // extract a Post object from databas and see if that works
                Post p = db.Posts.Find(1);
                Assert.IsNotNull(p);
                var user = from u in db.Users
                           where u.Email == "qq@qq.qq"
                           select u;
                ApplicationUser au = user.FirstOrDefault();
                Assert.IsNotNull(au);
            }
        }


        public Post CreateExpiredPost(Boolean Expired) {
            //create variables that can be fed into the post objects.
            Subcategory sub1 = CreateSubcategory();
            Category cat1 = CreateCategory();
            Area a1 = CreateArea();
            Locale l1 = CreateLocale();
            //craete the post object.
            Post p1 = new Post()
                {

                    Date = DateTime.Now,
                    Expiration = DateTime.Now.AddDays(5),
                    Title = "A",
                    Body = "A",
                    Viewable = true,
                    PostArea = a1,
                    PostLocale = l1,
                    PostCategory = cat1,
                    PostSubcategory = sub1
            };
            if (Expired) {
                p1.Date = DateTime.Now.AddDays(-10);
                p1.Expiration = DateTime.Now.AddDays(-5);
            }
            return p1;
        }

        
        [TestMethod]
        public void ProcessExpiredPostTest() {
            //create post objects
            Post p1 = CreateExpiredPost(false);
            Post p2 = CreateExpiredPost(true);
            //create list
            List<Post> posts = new List<Post>();
            posts.Add(p1);
            posts.Add(p2);
            //make sure the list is not null
            Assert.IsNotNull(posts);
            posts = Biz1.ProcessExpiredPost(posts);
            //make sure after the function, the list is not null
            Assert.IsNotNull(posts);
            //test result
            Assert.IsTrue(posts[0].Viewable);
            Assert.IsFalse(posts[1].Viewable);
            
            //end of creating a new post;



        }
        [TestMethod]
        public void DetermineExpiredTest()
        {
            //creating objects necessary to test Create Post Test Method
            //fields that need to be fed 
            Post nonExpiredPost = CreateExpiredPost(false);
            Post expiredPost = CreateExpiredPost(true);
            Assert.IsNotNull(Biz1.DetermineExpired(expiredPost));
            Assert.IsTrue(Biz1.DetermineExpired(expiredPost));
            Assert.IsNotNull(Biz1.DetermineExpired(nonExpiredPost));
            Assert.IsFalse(Biz1.DetermineExpired(nonExpiredPost));
        }
        [TestMethod]
        public void CreatePostTest() {
            //creating objects necessary to test Create Post Test Method
            Subcategory coffee = new Subcategory()
            {
                Name = "Starbucks",
                Viewable = true
            };

            Category drink = new Category()
            {
                Name = "Drink",
                Viewable = true,
                Subcategories = new List<Subcategory>()
            };

            coffee.Cat = drink;
            drink.Subcategories.Add(coffee);

            Area a1 = new Area() { Name = "Shenzhen", Locales = new List<Locale>() };
            Locale l1 = new Locale() { Name = "Xili", Area_obj = a1 };
            a1.Locales.Add(l1);
            Locale l2 = new Locale() { Name = "Longhua", Area_obj = a1 };
            a1.Locales.Add(l2);
            Post p1 = new Post()
            {
                Date = DateTime.Now,
                Expiration = DateTime.Now.AddDays(5),
                Title = "CY",
                Body = ":P",
                Viewable = true,
                PostArea = a1,
                PostLocale = l1,
                PostCategory = drink,
                PostSubcategory = coffee
            };
            Assert.IsNotNull(p1);
            Assert.AreEqual(p1.Title, "CY");
            Assert.AreEqual(p1.Body,":P");
            //default viewable should be true
            Assert.AreEqual(p1.Viewable, true);
            Assert.AreEqual(p1.PostArea, a1);
            Assert.AreEqual(p1.PostLocale, l1);
            Assert.AreEqual(p1.PostCategory,drink);
            Assert.AreEqual(p1.PostSubcategory, coffee);


        }




 

        public Locale CreateLocale()
        {
            return new Locale() { Name = "Longhua" };
        }

        public Area CreateArea()
        {
            Area a = new Area() { Name = "Shenzhen", Locales = new List<Locale>() };
            Locale locale = CreateLocale();
            a.Locales.Add(locale);
            locale.Area_obj = a;
            return a;
        }

        public Subcategory CreateSubcategory()
        {
            return new Subcategory() { Name = "BubbleTea", Viewable = true };
        }

        public Category CreateCategory()
        {
            Subcategory subcatagory = CreateSubcategory();
            Category ret = new Category()
            {
                Name = "Drink",
                Viewable = true,
                Subcategories = new List<Subcategory>()
            };
            ret.Subcategories.Add(subcatagory);
            subcatagory.Cat = ret;
            return ret;
        }

        public Post CreateRegularPost()
        {
            Area a = CreateArea();
            Locale lo = a.Locales.First();
            Category cat = CreateCategory();
            Subcategory subcat = cat.Subcategories.First();
            return new Post()
            {
                Date = DateTime.Now,
                Expiration = DateTime.Now.AddDays(5),
                Title = "Bubble Tea",
                Body = "Bubble Tea in longhua Jiufang",
                Viewable = true,
                PostArea = a,
                PostLocale = lo,
                PostCategory = cat,
                PostSubcategory = subcat
            };
        }

        [TestMethod]
        public void ModifyTest()
        {
            Post p = CreateRegularPost();
            Assert.IsNotNull(p);
            Assert.AreEqual(p.Title, "Bubble Tea");
            Biz1.Modify(p, "Naixue", p.Body, p.PostCategory, p.PostSubcategory, p.PostLocale, p.PostArea);
            Assert.AreEqual(p.Title, "Naixue");
            Assert.AreEqual(p.Body, "Bubble Tea in longhua Jiufang");
            Biz1.Modify(p,p.Title,"Red Mountain and Xili",p.PostCategory, p.PostSubcategory, p.PostLocale, p.PostArea);
            Assert.AreEqual(p.Body, "Red Mountain and Xili");

        }

    }
}
