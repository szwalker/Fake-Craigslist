using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace UI
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            // 1
            routes.MapRoute(
            name: "Given a selected locale and subcategory",
            url: "Area/{AreaName}/{AreaId}/Locale/{LocaleName}/{LocaleId}/Categories/{CatName}/{CatId}/Subcategories/{SubCatName}/{SubCatId}",
            defaults: new { controller = "Subcategories", action = "UserPost" }
            );

            // 2
            routes.MapRoute(
            name: "Given a selected locale and category",
            url: "Area/{AreaName}/{AreaId}/Locale/{LocaleName}/{LocaleId}/Categories/{CatName}/{CatId}",
            defaults: new { controller = "Categories", action = "UserPost" }
            );

            // 3
            routes.MapRoute(
            name: "Given a selected locale",
            url: "Area/{AreaName}/{AreaId}/Locale/{LocaleName}/{LocaleId}",
            defaults: new { controller = "Home", action = "Index" }
            );

            // 4
            routes.MapRoute(
            name: "Given a selected area and subcategory",
            url: "Area/{AreaName}/{AreaId}/Categories/{CatName}/{CatId}/Subcategories/{SubCatName}/{SubCatId}",
            defaults: new { controller = "Subcategories", action = "UserPost" }
            );

            // 5
            routes.MapRoute(
            name: "Given a selected area and category",
            url: "Area/{AreaName}/{AreaId}/Categories/{CatName}/{CatId}",
            defaults: new { controller = "Categories", action = "UserPost" }
            );
            
            // 6
            routes.MapRoute(
            name: "Given a subcategory",
            url: "Categories/{CatName}/{CatId}/Subcategories/{SubCatName}/{SubCatId}",
            defaults: new { controller = "Subcategories", action = "UserPost" }
            );

            routes.MapRoute(
             name: "Category Edit",
             url: "Categories/Modify/{id}",
             defaults: new { controller = "Categories", action = "Modify" }
         );

            routes.MapRoute(
            name: "listCat",
            url: "Categories/List/{id}",
            defaults: new { controller = "Categories", action = "List", id = UrlParameter.Optional }
        );
            // 7
            routes.MapRoute(
            name: "Given a selected category",
            url: "Categories/{CatName}/{CatId}",
            defaults: new { controller = "Categories", action = "UserPost" }
            );

            // 8
            routes.MapRoute(
                name: "Given a selected area",
                url: "Area/{AreaName}/{AreaId}",
                defaults: new { controller = "Home", action = "Index" }
            );
            // buggy here
            routes.MapRoute(
                name: "Message Details",
                url: "Message/Details/{MessageId}",
                defaults: new { controller = "Message", action = "Details" }
            );

            routes.MapRoute(
                name: "Reply Post",
                url: "Message/Create/{PostId}",
                defaults: new { controller = "Message", action = "Create" }
            );

            routes.MapRoute(
                name: "Inbox Screen",
                url: "Message/Inbox",
                defaults: new { controller = "Message", action = "Inbox" }
            );





            routes.MapRoute(
                name: "List Posts of a user",
                url: "Account/{uid}/Posts/List",
                defaults: new { controller = "Posts", action = "List" }
            );

            routes.MapRoute(
                name: "List All Users",
                url: "Account/ListUsers",
                defaults: new { controller = "Account", action = "ListUsers" }
            );

            routes.MapRoute(
                name: "Edit User",
                url: "Account/Edit/{uid}",
                defaults: new { controller = "Account", action = "Edit" }
            );

            routes.MapRoute(
                name: "createSubCat",
                url: "Categories/{id1}/Subcategories/Create",
                defaults: new { controller = "Subcategories", action = "Create" }
            );


            routes.MapRoute(
                name: "modifySubCat",
                url: "Categories/{id1}/Subcategories/Modify/{id2}",
                defaults: new { controller = "Subcategories", action = "Modify" }
            );

            routes.MapRoute(
              name: "list all categories",
              url: "Categories/UserPost/{catid}",
              defaults: new { controller = "Categories", action = "UserPost" }
          );






            routes.MapRoute(
                name: "list all SubCat",
                url: "Categories/{id1}/Subcategories/List",
                defaults: new { controller = "Subcategories", action = "List" }
            );

            routes.MapRoute(
                name: "List all post for a subcategory",
                url: "Categories/{catid}/Subcategories/UserPost/{subcatid}",
                defaults: new { controller = "Subcategories", action = "UserPost" }
            );

            routes.MapRoute(
             name: "List all post for an areas",
             url: "Areas/UserPost/{areaid}",
             defaults: new { controller = "Areas", action = "UserPost" }
         );

            routes.MapRoute(
               name: "List all post for a locales",
               url: "Areas/{areaid}/Locales/UserPost/{localeid}",
               defaults: new { controller = "Locales", action = "UserPost" }
           );


            routes.MapRoute(
                name: "Create Locales for a certain Area",
                url: "Areas/{id1}/Locales/Create",
                defaults: new { controller = "Locales", action = "Create" }
            );

            routes.MapRoute(
                name: "List Locales for a certain Area",
                url: "Areas/{id1}/Locales/List",
                defaults: new { controller = "Locales", action = "List" }
            );

            routes.MapRoute(
               name: "Edit Locales for a certain Area",
               url: "Areas/{id1}/Locales/{id2}/Edit",
               defaults: new { controller = "Locales", action = "Edit" }
           );

            routes.MapRoute(
             name: "Detail Locales for a certain Area",
             url: "Areas/{id1}/Locales/{id2}/Details",
             defaults: new { controller = "Locales", action = "Details" }
         );

            routes.MapRoute(
             name: "Posts Details",
             url: "Posts/Details/{id}",
             defaults: new { controller = "Posts", action = "Details" }
         );

            routes.MapRoute(
             name: "Area Details",
             url: "Areas/Details/{id}",
             defaults: new { controller = "Areas", action = "Details" }
         );
            routes.MapRoute(
             name: "Area Edit",
             url: "Areas/Edit/{id}",
             defaults: new { controller = "Areas", action = "Edit" }
         );

            routes.MapRoute(
             name: "List Posts",
             url: "Posts/List",
             defaults: new { controller = "Posts", action = "List" }
         );




            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}",
                defaults: new { controller = "Home", action = "Index"}
            );

            


        }
    }
}
