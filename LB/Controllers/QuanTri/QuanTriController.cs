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
    }
}