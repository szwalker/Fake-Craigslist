using Data.Models;
using Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;


namespace BizLogic.Logic
{
    public static class Biz1
    {

        public static Post CreatePost(string title, string body, Locale local, Area area, Category category, Subcategory subcategory, ApplicationUser owner)
        {
            Post newPost = new Post()
            {
                Title = title,
                Body = body,
                PostLocale = local,
                PostArea = area,
                PostCategory = category,
                PostSubcategory = subcategory,
                Owner = owner,
                Date = DateTime.Now,
                Expiration = DateTime.Now.AddDays(5),
                Viewable = true,
               
            };
            return newPost;  
        }
        
        /// <summary>
        /// The function examines whether a post is expired. If it's expired, the returned value would be true;
        /// </summary>
        /// <returns>True/False</returns>
        public static Boolean DetermineExpired(Post post){
            if(post.Expiration<DateTime.Now)
                return true;
            return false;
        }


        /// <summary>
        /// The function takes in a list of posts and return a list of posts in which expired post's viewable field would be set to false;
        /// </summary>
        /// <returns>List<Post> </returns>
        public static List<Post> ProcessExpiredPost(List<Post> posts){
            if(posts==null)
                return null;
            foreach (var post in posts){
                if(DetermineExpired(post)){
                    post.Viewable = false;
                }
            }

            return posts;
        }
        //

        /// <summary>
        /// change the current user level to Admin
        /// </summary>
        /// <param name="user"></param>



        public static void Modify(Post post, string title, string body, Category category, Subcategory subcategory, Locale local, Area area)
        {
            
            post.Title = title;
            post.Body = body;
            post.PostCategory = category;
            post.PostSubcategory = subcategory;
            post.PostLocale = local;
            post.PostArea = area;
        }

        public static Boolean updateSubcategories(Category cat) {
            return cat.Viewable;
        }

        public static string GenerateSlug(string phrase)
        {
            string str = phrase.RemoveAccent().ToLower();
            // invalid chars           
            str = Regex.Replace(str, @"[^a-z0-9\s-]", "");
            // convert multiple spaces into one space   
            str = Regex.Replace(str, @"\s+", " ").Trim();
            // cut and trim 
            str = str.Substring(0, str.Length <= 45 ? str.Length : 45).Trim();
            str = Regex.Replace(str, @"\s", "-"); // hyphens   
            return str;
        }

        public static string RemoveAccent(this string txt)
        {
            byte[] bytes = System.Text.Encoding.GetEncoding("Cyrillic").GetBytes(txt);
            return System.Text.Encoding.ASCII.GetString(bytes);
        }


    }


    
}
