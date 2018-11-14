using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using LB.Models;
namespace LB.Controllers.HeThong
{
    public class MonHocController : BaseController
    {
        // GET: MonHoc
        public ActionResult Show()
        {
            return View();
        }

        [HttpGet]
        public ActionResult getMonHoc()
        {

            var data = db.Tbl_MonHoc.Where(p => p.MaTruong == MaTruong).ToList();

            ResultInfo result = new ResultInfo()
            {
                error = 0,
                msg = "",
                data = data
            };

            return Json(result, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult create(Tbl_MonHoc mh)
        {

            if (String.IsNullOrEmpty(mh.Ten))
                return Json(new ResultInfo() { error = 1, msg = "Missing info" }, JsonRequestBehavior.AllowGet);
            var check = db.Tbl_MonHoc.Where(p => p.MaTruong == MaTruong && p.Ten == mh.Ten).FirstOrDefault();

            if (check != null)
                return Json(new ResultInfo() { error = 1, msg = "Đã tồn tại" }, JsonRequestBehavior.AllowGet);
            mh.MaTruong = MaTruong;
            db.Tbl_MonHoc.Add(mh);

            db.SaveChanges();

            return Json(new ResultInfo() { error = 0, msg = "", data = mh }, JsonRequestBehavior.AllowGet);

        }

        [HttpPost]
        public ActionResult edit(Tbl_MonHoc mh)
        {
            if (String.IsNullOrEmpty(mh.Ten))
                return Json(new ResultInfo() { error = 1, msg = "Missing info" }, JsonRequestBehavior.AllowGet);

            var check = db.Tbl_MonHoc.Where(p => p.MaTruong == MaTruong && p.Ten == mh.Ten).FirstOrDefault();

            if (check == null)
                return Json(new ResultInfo() { error = 1, msg = "Không tìm thấy thông tin" }, JsonRequestBehavior.AllowGet);

            check.Ten = mh.Ten;

            db.Entry(check).State = System.Data.Entity.EntityState.Modified;
            db.SaveChanges();

            return Json(new ResultInfo() { error = 0, msg = "", data = check }, JsonRequestBehavior.AllowGet);

        }
        [HttpPost]
        public ActionResult delete(int id)
        {
            if (String.IsNullOrEmpty(id.ToString()))
                return Json(new ResultInfo() { error = 1, msg = "Missing info" }, JsonRequestBehavior.AllowGet);

            var check = db.Tbl_MonHoc.Where(p => p.MaTruong == MaTruong && p.Ma == id).FirstOrDefault();

            if (check == null)
                return Json(new ResultInfo() { error = 1, msg = "Không tìm thấy thông tin" }, JsonRequestBehavior.AllowGet);

            db.Entry(check).State = System.Data.Entity.EntityState.Deleted;
            db.SaveChanges();

            return Json(new ResultInfo() { error = 0, msg = "", data = check }, JsonRequestBehavior.AllowGet);
        }
    }
}