using LB.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace LB.Controllers.QuanTri
{

    [Authorize(Roles = "sadmin")]
    public class QuanTriController : Controller
    {
        ssofty_com_thuvienEntities db = new ssofty_com_thuvienEntities();

        // GET: QuanTri
        public ActionResult Show()
        {
         
            return View();
        }


        [HttpGet]
        public ActionResult GetFrist()
        {
            var data = db.Tbl_ThongTin.Select(p => new IdentityCommon()
            {
                code = p.MaTruong,
                name = p.TenTruong
            }).ToList();


            return Json(new { schools = data}, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult GetAccounts(string MaTruong)
        {
            var data = db.AspNetUsers.Where(p => p.MaTruong == MaTruong && p.UType == "ADMIN").Select(p=> new
            {
                UserName = p.UserName,
                IsActive = p.IsActive,
                FullName = p.HoTen

            }).ToList();

            return Json(new ResultInfo()
            {
                error = 0,
                data = data
            }, JsonRequestBehavior.AllowGet);
        }


        [HttpPost]
        public ActionResult ActiveAccounts (string UserName, bool IsActive)
        {
            var find = db.AspNetUsers.Where(p => p.UserName == UserName).FirstOrDefault();
            if(find != null)
            {
                find.IsActive = IsActive;
                db.Entry(find).State = System.Data.Entity.EntityState.Modified;
                db.SaveChanges();
            }

            return Json(new ResultInfo()
            {
                error = 0
            }, JsonRequestBehavior.AllowGet);
        }

        // add menu
        //
        // GET: /Menu/
        public ActionResult ShowMenu()
        {

            ViewBag.GroupMenu = db.UMS_GroupMenu.Where(p => p.IsActive == 1).Select(p => new { Id = p.Id, Name = p.Name, Position = p.Position, Icon = p.Icon }).OrderBy(p => p.Position).ToList();

            return View();
        }


        [HttpGet]
        public ActionResult GetMenus(string groupId)
        {

            var result = db.UMS_Menu.Where(p => p.GroupMenuId == groupId).OrderBy(p => p.Position).Select(p => new { Id = p.Id, Name = p.Name, Position = p.Position, Code = p.Code, Link = p.Link, GroupMenuId = p.GroupMenuId }).ToList();

            return Json(new { data = result }, JsonRequestBehavior.AllowGet);

        }


        [HttpPost]
        public ActionResult AddMenu(UMS_Menu menu)
        {
            menu.Id = Guid.NewGuid().ToString();

            var checkGroup = db.UMS_GroupMenu.Find(menu.GroupMenuId);

            if (checkGroup == null)
                return Json(new { error = 1, msg = "Sai thông tin" }, JsonRequestBehavior.AllowGet);

            db.UMS_Menu.Add(menu);
            db.SaveChanges();

            return Json(new { error = 0, data = new { Id = menu.Id, Name = menu.Name, Position = menu.Position, Code = menu.Code, Link = menu.Link, GroupMenuId = menu.GroupMenuId } }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult EditMenu(UMS_Menu menu)
        {
            var check = db.UMS_Menu.Find(menu.Id);

            if (check == null)
                return Json(new { error = 1, msg = "Sai thông tin" }, JsonRequestBehavior.AllowGet);

            var checkGroup = db.UMS_GroupMenu.Find(menu.GroupMenuId);

            if (checkGroup == null)
                return Json(new { error = 1, msg = "Sai thông tin" }, JsonRequestBehavior.AllowGet);

            check.Code = menu.Code;
            check.Name = menu.Name;
            check.Link = menu.Link;
            check.Position = menu.Position;
            check.GroupMenuId = menu.GroupMenuId;

            db.Entry(check).State = System.Data.Entity.EntityState.Modified;
            db.SaveChanges();

            return Json(new { error = 0 }, JsonRequestBehavior.AllowGet);

        }


    }
}