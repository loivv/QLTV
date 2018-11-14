using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using LB.Models;
namespace LB.Controllers.HeThong
{
    public class NhomNguoiDungController : BaseController
    {
        // GET: NhomNguoiDung
        public ActionResult Show()
        {
            return View();
        }

        [HttpGet]
        public ActionResult getNhomNguoiDung()
        {

            var data = db.Tbl_NhomNguoiDung.Where(p => p.MaTruong == MaTruong).ToList();
            ResultInfo result = new ResultWithPaging()
            {
                error = 0,
                msg = "",
                data = data
            };

            return Json(result, JsonRequestBehavior.AllowGet);
        }
        public ActionResult create(Tbl_NhomNguoiDung group)
        {

            if (String.IsNullOrEmpty(group.MaNhom))
                return Json(new ResultInfo() { error = 1, msg = "Missing info" }, JsonRequestBehavior.AllowGet);

            var check = db.Tbl_NhomNguoiDung.Where(p => p.MaTruong == MaTruong && p.MaNhom == group.MaNhom).FirstOrDefault();

            if (check != null)
                return Json(new ResultInfo() { error = 1, msg = "Đã tồn tại" }, JsonRequestBehavior.AllowGet);
            group.MaTruong = MaTruong;
            db.Tbl_NhomNguoiDung.Add(group);
            db.SaveChanges();

            return Json(new ResultInfo() { error = 0, msg = "", data = group }, JsonRequestBehavior.AllowGet);

        }

        [HttpPost]
        public ActionResult edit(Tbl_NhomNguoiDung group)
        {
            if (String.IsNullOrEmpty(group.MaNhom))
                return Json(new ResultInfo() { error = 1, msg = "Missing info" }, JsonRequestBehavior.AllowGet);

            var check = db.Tbl_NhomNguoiDung.Where(p => p.MaTruong == MaTruong && p.MaNhom == group.MaNhom).FirstOrDefault();

            if (check == null)
                return Json(new ResultInfo() { error = 1, msg = "Không tìm thấy thông tin" }, JsonRequestBehavior.AllowGet);

            check.TenNhom = group.TenNhom;

            db.Entry(check).State = System.Data.Entity.EntityState.Modified;
            db.SaveChanges();

            return Json(new ResultInfo() { error = 0, msg = "", data = check }, JsonRequestBehavior.AllowGet);

        }
        [HttpPost]
        public ActionResult delete(Tbl_NhomNguoiDung group)
        {
            if (String.IsNullOrEmpty(group.MaNhom))
                return Json(new ResultInfo() { error = 1, msg = "Missing info" }, JsonRequestBehavior.AllowGet);

            var check = db.Tbl_NhomNguoiDung.Where(p => p.MaTruong == MaTruong && p.MaNhom == group.MaNhom).FirstOrDefault();

            if (check == null)
                return Json(new ResultInfo() { error = 1, msg = "Không tìm thấy thông tin" }, JsonRequestBehavior.AllowGet);

            db.Entry(check).State = System.Data.Entity.EntityState.Deleted;
            db.SaveChanges();

            return Json(new ResultInfo() { error = 0, msg = "", data = check }, JsonRequestBehavior.AllowGet);
        }
    }
}