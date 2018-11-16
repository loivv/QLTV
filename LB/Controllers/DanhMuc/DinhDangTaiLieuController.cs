using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using LB.Models;
namespace LB.Controllers.DanhMuc
{
    public class DinhDangTaiLieuController : BaseController
    {
        // GET: DinhDangTaiLieu
        public ActionResult Show()
        {
            return View();
        }

        [HttpGet]
        public ActionResult getDinhDang(int? page)
        {

            int pageSize = 50;
            int pageNumber = (page ?? 1);
            var data = db.Tbl_DinhDangTaiLieu.Where(p => p.MaTruong == "ALL").ToList().OrderBy(p => p.Ma);

            ResultInfo result = new ResultWithPaging()
            {
                error = 0,
                msg = "",
                page = pageNumber,
                pageSize = pageSize,
                toltalSize = data.Count(),
                data = data.Skip((pageNumber - 1) * pageSize).Take(pageSize).ToList()
            };
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        public ActionResult create(Tbl_DinhDangTaiLieu dd)
        {

            if (String.IsNullOrEmpty(dd.Ma.ToString()) || String.IsNullOrEmpty(dd.Ten))
                return Json(new ResultInfo() { error = 1, msg = "Missing info" }, JsonRequestBehavior.AllowGet);

            var check = db.Tbl_DinhDangTaiLieu.Where(p => p.MaTruong == MaTruong && p.Ma == dd.Ma).FirstOrDefault();

            if (check != null)
                return Json(new ResultInfo() { error = 1, msg = "Đã tồn tại" }, JsonRequestBehavior.AllowGet);
            dd.MaTruong = "ALL";
            db.Tbl_DinhDangTaiLieu.Add(dd);
            db.SaveChanges();

            return Json(new ResultInfo() { error = 0, msg = "", data = dd }, JsonRequestBehavior.AllowGet);

        }

        [HttpPost]
        public ActionResult edit(Tbl_DinhDangTaiLieu dd)
        {
            if (String.IsNullOrEmpty(dd.Ma.ToString()) || String.IsNullOrEmpty(dd.Ten))
                return Json(new ResultInfo() { error = 1, msg = "Missing info" }, JsonRequestBehavior.AllowGet);

            var check = db.Tbl_DinhDangTaiLieu.Where(p => p.MaTruong == "ALL" && p.Ma == dd.Ma).FirstOrDefault();

            if (check == null)
                return Json(new ResultInfo() { error = 1, msg = "Không tìm thấy thông tin" }, JsonRequestBehavior.AllowGet);

            check.Ma = dd.Ma;
            check.Ten = dd.Ten;
            check.GhiChu = dd.GhiChu;

            db.Entry(check).State = System.Data.Entity.EntityState.Modified;
            db.SaveChanges();

            return Json(new ResultInfo() { error = 0, msg = "", data = check }, JsonRequestBehavior.AllowGet);

        }
        [HttpPost]
        public ActionResult delete(int ma)
        {
            if (String.IsNullOrEmpty(ma.ToString()))
                return Json(new ResultInfo() { error = 1, msg = "Missing info" }, JsonRequestBehavior.AllowGet);

            var check = db.Tbl_DinhDangTaiLieu.Where(p => p.MaTruong == "ALL" && p.Ma == ma).FirstOrDefault();

            if (check == null)
                return Json(new ResultInfo() { error = 1, msg = "Không tìm thấy thông tin" }, JsonRequestBehavior.AllowGet);

            db.Entry(check).State = System.Data.Entity.EntityState.Deleted;
            db.SaveChanges();

            return Json(new ResultInfo() { error = 0, msg = "", data = check }, JsonRequestBehavior.AllowGet);
        }
    }
}