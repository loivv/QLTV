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
    // [Authorize]
    public class BaseController : Controller
    {
        // GET: Base
        protected ssofty_com_thuvienEntities db = new ssofty_com_thuvienEntities();

        protected RoleManager<IdentityRole> RoleManager { get; private set; }

        protected UserManager<ApplicationUser> UserManager { get; private set; }
        private ApplicationDbContext sdb = new ApplicationDbContext();

        public string MaTruong;
        public bool? PhanLoai; //phong hay truong hay so
        public string TenTruong;
        public List<string> roles;
        public int? NamHoc;

        protected override void Initialize(System.Web.Routing.RequestContext requestContext)
        {
            MaTruong = "LQDON";
            NamHoc = 1; //xóa xóa xóa
            base.Initialize(requestContext);

            if (requestContext.HttpContext.User.Identity.IsAuthenticated)
            {
                var checkUser = db.AspNetUsers.Where(p => p.UserName == requestContext.HttpContext.User.Identity.Name).FirstOrDefault();
                MaTruong = "LQDON";
                if (checkUser != null)
                {
                    MaTruong = checkUser.MaTruong;
                    MaTruong = "LQDON";
                    var checkloai = db.Tbl_ThongTin.Where(x => x.MaTruong == MaTruong).FirstOrDefault();
                    PhanLoai = checkloai.Phong; // phòng = true , truong = false
                    TenTruong = checkloai.TenTruong;
                    NamHoc = checkloai.NamHoc;

                }
            }
        }
        public BaseController()
        {
            RoleManager = new RoleManager<IdentityRole>(new RoleStore<IdentityRole>(sdb));
            UserManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(sdb));

        }
        //[HttpGet]
        public ActionResult Menus()
        {
            var user = User.Identity.Name;

            var us = new SqlParameter("@user", user);
            var getMenu = db.Database.SqlQuery<USER_GETMENU>("USER_GETMENU @user", us).ToList();
            //var getMenu = db.USER_GETMENU(user).ToList();

            List<GroupMenuInfo> groupMenus = new List<GroupMenuInfo>();

            var listGroup = db.UMS_GroupMenu.OrderBy(p => p.Position).ToList();

            foreach (var item in listGroup)
            {
                GroupMenuInfo groupInfo = new GroupMenuInfo()
                {
                    name = item.Name,
                    icon = item.Icon,
                    menus = new List<MenuInfo>()
                };

                var listMenu = getMenu.Where(p => p.GroupMenuId == item.Id).OrderBy(p => p.Position).ToList();

                if (listMenu.Count() > 0)
                {
                    foreach (var menuItem in listMenu)
                    {
                        groupInfo.menus.Add(new MenuInfo()
                        {
                            name = menuItem.Name,
                            link = menuItem.Link
                        });
                    }
                    groupMenus.Add(groupInfo);
                }
            }

            return PartialView("_MenuUser", groupMenus);
        }
    }
}