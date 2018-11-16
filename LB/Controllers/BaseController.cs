using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using LB.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System.Data.SqlClient;
namespace LB.Controllers
{
    [Authorize]
    public class BaseController : Controller
    {
        // GET: Base
        protected ssofty_com_thuvienEntities db = new ssofty_com_thuvienEntities();

        protected RoleManager<IdentityRole> RoleManager { get; private set; }

        protected UserManager<ApplicationUser> UserManager { get; private set; }

        private ApplicationDbContext sdb = new ApplicationDbContext();

        public string MaTruong = "SAdmin";
        public string TenTruong = "SUP Admin";
        public string UType;

        protected override void Initialize(System.Web.Routing.RequestContext requestContext)
        {
            MaTruong = "LQDON";
            base.Initialize(requestContext);

            if (requestContext.HttpContext.User.Identity.IsAuthenticated)
            {
                var checkUser = db.AspNetUsers.Where(p => p.UserName == requestContext.HttpContext.User.Identity.Name).FirstOrDefault();
                UType = checkUser.UType;
                if (checkUser != null && UType != "SADMIN")
                {
                    MaTruong = checkUser.MaTruong;
                    var checkloai = db.Tbl_ThongTin.Where(x => x.MaTruong == MaTruong).FirstOrDefault();
                    TenTruong = checkloai.TenTruong;
                }
            }
        }
        public BaseController()
        {
            RoleManager = new RoleManager<IdentityRole>(new RoleStore<IdentityRole>(sdb));
            UserManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(sdb));
        }

    }
}