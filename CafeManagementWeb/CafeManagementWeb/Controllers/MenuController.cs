using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CafeManagementWeb.Controllers
{
    public class MenuController : Controller
    {
        // GET: Drinks
        public ActionResult Drinks()
        {
            return View();
        }

        public ActionResult Food()
        {
            return View();
        }
    }
}