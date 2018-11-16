using LB.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace LB.Controllers
{

    public class MenuController : BaseController
    {
        [HttpGet]
        public ActionResult AdminMenu()
        {
            if (UType == "SADMIN")
            {
                return PartialView("_SAdminMenu");
            }
            else if (UType == "ADMIN")
            {
                return PartialView("_AdminMenu");
            } else
            {
                return PartialView("_NoneView");
            }
        }


        [HttpGet]
        public ActionResult Menus()
        {
            var user = User.Identity.Name;

            var getMenu = db.USER_GETMENU(user).ToList();

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