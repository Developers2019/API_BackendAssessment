using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace API_BackendAssessment.Controllers
{
    public class UserAccountController : Controller
    {
        // GET: UserAccount
        public ActionResult Login()
        {
            return View();
        }
        
        public ActionResult Register()
        {
            return View();
        }
    }
}