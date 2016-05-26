using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CafeManagementWeb.Controllers
{
    public class LoginController : Controller
    {
        // GET: Login
        public ActionResult UserIdentification()
        {
            return View();
        }

        public ActionResult ForgotPass()
        {
            return View();
        }
    }
}