using LB.Models;
using System;
using System.Linq;
using System.Web.Mvc;
namespace LB.Controllers.HeThong
{
    [Authorize(Roles = "admin")]
    public class NhomNguoiDungController : BaseController
    {
        // GET: NhomNguoiDung
        public ActionResult Show()
        {
            ViewBag.GroupMenu = db.UMS_GroupMenu.Where(p => p.IsActive == 1).Select(p => new
            {
                code = p.Id,
                name = p.Name

            }).ToList();
            return View();
        }

        [HttpGet]
        public ActionResult GetNhom()
        {
            var data = db.UMS_UserGroups.Where(p => p.MaTruong == MaTruong).ToList();

            return Json(data, JsonRequestBehavior.AllowGet);
        }


        [HttpPost]
        public ActionResult AddGroup(string name)
        {
            if (!String.IsNullOrEmpty(name))
            {
                var data = new UMS_UserGroups()
                {
                    MaTruong = MaTruong,
                    GroupID = Guid.NewGuid().ToString(),
                    GroupName = name,
                    IsActive = 1
                };

                db.UMS_UserGroups.Add(data);
                db.SaveChanges();
            }

            return Json(new ResultInfo()
            {
                error = 0
            }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult EditGroup(string id, string name)
        {
            var find = db.UMS_UserGroups.Where(p => p.MaTruong == MaTruong && p.GroupID == id).FirstOrDefault();

            if (find != null)
            {
                find.GroupName = name;

                db.Entry(find).State = System.Data.Entity.EntityState.Modified;
                db.SaveChanges();
            }

            return Json(new ResultInfo()
            {
                error = 0
            }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult ActiveGroup(string id, bool isActive)
        {
            var find = db.UMS_UserGroups.Where(p => p.MaTruong == MaTruong && p.GroupID == id).FirstOrDefault();

            if (find != null)
            {
                find.IsActive = isActive ? 1 : 0;

                db.Entry(find).State = System.Data.Entity.EntityState.Modified;
                db.SaveChanges();
            }

            return Json(new ResultInfo()
            {
                error = 0
            }, JsonRequestBehavior.AllowGet);
        }


        [HttpGet]
        public ActionResult GrouUserMenu(string id = "")
        {
            var data = db.GROUPUSER_GETLISTMENU(id).ToList();

            return Json(data, JsonRequestBehavior.AllowGet);
        }

        ///
        [HttpPost]
        public ActionResult AddRole(string menuId, string groupId, bool role)
        {
            var check = db.UMS_MenuGroupUser.Where(p => p.MenuId == menuId && p.GroupUserId == groupId).FirstOrDefault();

            var checkMenu = db.UMS_Menu.Find(menuId);

            if (checkMenu == null)
                return Json(new ResultInfo()
                {
                    error = 1,
                    msg = "Lỗi khi cấp quyền"
                }, JsonRequestBehavior.AllowGet);

            var checkGroup = db.UMS_UserGroups.Where(p => p.GroupID == groupId);

            if (checkGroup == null)
                return Json(new ResultInfo()
                {
                    error = 1,
                    msg = "Lỗi khi cấp quyền"
                }, JsonRequestBehavior.AllowGet);


            if (role)
            {

                if (check == null)
                {
                    var data = new UMS_MenuGroupUser()
                    {
                        Id = Guid.NewGuid().ToString(),
                        GroupUserId = groupId,
                        CanEdit = 0,
                        MenuId = menuId
                    };

                    db.UMS_MenuGroupUser.Add(data);
                    db.SaveChanges();

                    return Json(new ResultInfo()
                    {
                        error = 0,
                        msg = "Đã cấp quyền"
                    }, JsonRequestBehavior.AllowGet);
                }
            }
            else
            {
                if (check != null)
                {
                    db.UMS_MenuGroupUser.Remove(check);
                    db.SaveChanges();
                }

                return Json(new ResultInfo()
                {
                    error = 0,
                    msg = "Đã bỏ quyền"

                }, JsonRequestBehavior.AllowGet);
            }

            return Json(new ResultInfo()
            {
                error = 1,
                msg = "Lỗi khi cấp quyền"
            }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult AddEdit(string menuId, string groupId, bool role)
        {
            var check = db.UMS_MenuGroupUser.Where(p => p.MenuId == menuId && p.GroupUserId == groupId).FirstOrDefault();

            if (check != null)
            {
                check.CanEdit = role ? 1 : 0;
                db.Entry(check).State = System.Data.Entity.EntityState.Modified;
                db.SaveChanges();
                return Json(new ResultInfo()
                {
                    error = 0,
                    msg = "Đã thực hiện"
                }, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json(new ResultInfo()
                {
                    error = 1,
                    msg = "Lỗi"
                }, JsonRequestBehavior.AllowGet);
            }
        }
    }
}