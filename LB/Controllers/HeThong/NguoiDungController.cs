using LB.Models;
using System;
using System.Linq;
using System.Web.Mvc;
namespace LB.Controllers.HeThong
{
    [Authorize(Roles = "admin")]
    public class NguoiDungController : BaseController
    {
        // GET: NguoiDung
        public ActionResult Show()
        {
            ViewBag.Groups = db.UMS_UserGroups.Where(p => p.MaTruong == MaTruong && p.IsActive == 1).Select(p => new
            {
                code = p.GroupID,
                name = p.GroupName
            }).ToList();

            return View();
        }

        [HttpGet]
        public ActionResult GetAll()
        {
            var data = db.AspNetUsers.Where(p => p.MaTruong == MaTruong).Select(p => new
            {
                UserName = p.UserName,
                Email = p.Email,
                FullName = p.HoTen,
                Group = p.UserGroup,
                IsActive = p.IsActive,
                Phone = p.PhoneNumber,
                Address = p.AddressInfo,
                UType = p.UType
            }).ToList();

            return Json(new ResultInfo()
            {
                error = 0,
                data = data
            }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult Modify(RegisterAccountModel model)
        {
            var find = db.AspNetUsers.Where(p => p.UserName == model.UserName).FirstOrDefault();

            if (find != null)
            {
                find.HoTen = model.FullName;
                find.PhoneNumber = model.Phone;
                find.AddressInfo = model.Address;
                find.Sex = model.Sex;
                find.UserGroup = model.Group;

                db.Entry(find).State = System.Data.Entity.EntityState.Modified;
                db.SaveChanges();
            }

            return Json(new ResultInfo()
            {
                error = 0
            }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult ActiveAccounts(string UserName, bool IsActive)
        {
            var find = db.AspNetUsers.Where(p => p.UserName == UserName && p.MaTruong == MaTruong).FirstOrDefault();
            if (find != null && find.UType == "USER")
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