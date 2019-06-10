
﻿using DB.Database;
﻿using Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace UI.Controllers
{
    public class HomeController : Controller
    {
        [AllowAnonymous]
        public ActionResult Index(string AreaName, string LocaleName)
        {
            if (LocaleName != null)
            {
                ViewBag.Header = LocaleName;
            }
            else if(AreaName != null){
                ViewBag.Header = AreaName;
            }
            else
            {
                ViewBag.Header = "Default";
            }
            var model = new HomePageViewModel();
            model.Area = Db1.ListAreas();
            model.Cat = Db1.ListAllCategories();
            return View(model);
        }


        [CustomRoleCheck(Roles = "Admin")]
        public ActionResult Admin()
        {

            return View();
        }

    }
}