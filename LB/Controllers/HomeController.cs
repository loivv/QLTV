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
            return View();
        }

        public ActionResult MenuAdmin()
        {
            var check = db.USER_CHECKADMIN(User.Identity.Name, "admin").FirstOrDefault();

            var isADmin = check == null ? false : true;

            return PartialView("_AdminMenu", isADmin);
        }


        public ActionResult MenuSAdmin()
        {
            var check = db.USER_CHECKADMIN(User.Identity.Name, "sadmin").FirstOrDefault();

            var isADmin = check == null ? false : true;

            return PartialView("_SAdminMenu", isADmin);
        }

        public ActionResult ShowInfoTruong()
        {
            return Content(TenTruong);
        }
    }
}