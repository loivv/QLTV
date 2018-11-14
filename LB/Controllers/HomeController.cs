using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace LB.Controllers
{
    public class HomeController : BaseController
    {
        public ActionResult Index()
        {
            ViewBag.MaTruong = MaTruong;
            ViewBag.TenTruong = TenTruong;
            return View();
        }

    }
}