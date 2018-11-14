using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using LB.Models;
namespace LB.Controllers.DanhMuc
{
    public class KhoAnPhamController : BaseController
    {
        // GET: KhoAnPham
        public ActionResult Show()
        {
            return View();
        }

        [HttpGet]
        public ActionResult getKhoAnPham()
        {

            var data = db.Tbl_KhoSach.Where(p => p.MaTruong == MaTruong).ToList().OrderBy(p=>p.Ma);
            ResultInfo result = new ResultWithPaging()
            {
                error = 0,
                msg = "",
                data = data
            };

            return Json(result, JsonRequestBehavior.AllowGet);
        }
        public ActionResult create(Tbl_KhoSach s)
        {

            if (String.IsNullOrEmpty(s.Ten) || String.IsNullOrEmpty(s.Ma))
                return Json(new ResultInfo() { error = 1, msg = "Missing info" }, JsonRequestBehavior.AllowGet);

            var check = db.Tbl_KhoSach.Where(p => p.MaTruong == MaTruong && p.Ma == s.Ma).FirstOrDefault();

            if (check != null)
                return Json(new ResultInfo() { error = 1, msg = "Đã tồn tại" }, JsonRequestBehavior.AllowGet);
            s.MaTruong = s.Ma.ToUpper();
            s.Ten = s.Ten.ToUpper();
            s.MaTruong = MaTruong;
            db.Tbl_KhoSach.Add(s);
            db.SaveChanges();

            return Json(new ResultInfo() { error = 0, msg = "", data = s }, JsonRequestBehavior.AllowGet);

        }

        [HttpPost]
        public ActionResult edit(Tbl_KhoSach s)
        {
            if (String.IsNullOrEmpty(s.Ten) || String.IsNullOrEmpty(s.Ma))
                return Json(new ResultInfo() { error = 1, msg = "Missing info" }, JsonRequestBehavior.AllowGet);

            var check = db.Tbl_KhoSach.Where(p => p.MaTruong == MaTruong && p.Ma == s.Ma).FirstOrDefault();

            if (check == null)
                return Json(new ResultInfo() { error = 1, msg = "Không tìm thấy thông tin" }, JsonRequestBehavior.AllowGet);

            check.Ten = s.Ten;
            check.GhiChu = s.GhiChu;

            db.Entry(check).State = System.Data.Entity.EntityState.Modified;
            db.SaveChanges();

            return Json(new ResultInfo() { error = 0, msg = "", data = check }, JsonRequestBehavior.AllowGet);

        }
        [HttpPost]
        public ActionResult delete(string  ma)
        {
            if (String.IsNullOrEmpty(ma))
                return Json(new ResultInfo() { error = 1, msg = "Missing info" }, JsonRequestBehavior.AllowGet);

            var check = db.Tbl_KhoSach.Where(p => p.MaTruong == MaTruong && p.Ma == ma).FirstOrDefault();

            if (check == null)
                return Json(new ResultInfo() { error = 1, msg = "Không tìm thấy thông tin" }, JsonRequestBehavior.AllowGet);

            db.Entry(check).State = System.Data.Entity.EntityState.Deleted;
            db.SaveChanges();

            return Json(new ResultInfo() { error = 0, msg = "", data = check }, JsonRequestBehavior.AllowGet);
        }
    }
}