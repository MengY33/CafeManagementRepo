using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CafeManagementWeb.Controllers
{
    public class AboutUsController : Controller
    {
        // GET: AboutUs
        public ActionResult VisionMission()
        {
            return View();
        }

        public ActionResult OurCompany()
        {
            return View();
        }
    }
}