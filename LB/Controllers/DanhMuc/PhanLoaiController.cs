using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using LB.Models;
namespace LB.Controllers.DanhMuc
{
    public class PhanLoaiController : BaseController
    {
        // GET: PhanLoai
        public ActionResult Show()
        {
            return View();
        }

        [HttpGet]
        public ActionResult getPhanLoai(int? page)
        {

            int pageSize = 50;
            int pageNumber = (page ?? 1);
            var data = db.Tbl_KyHieuPhanLoai.Where(p => p.MaTruong == MaTruong).ToList().OrderBy(p => p.Ma);

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
        public ActionResult create(Tbl_KyHieuPhanLoai khpl)
        {

            if (String.IsNullOrEmpty(khpl.Ma.ToString()) || String.IsNullOrEmpty(khpl.Ten))
                return Json(new ResultInfo() { error = 1, msg = "Missing info" }, JsonRequestBehavior.AllowGet);

            var check = db.Tbl_KyHieuPhanLoai.Where(p => p.MaTruong == MaTruong && p.Ma == khpl.Ma).FirstOrDefault();

            if (check != null)
                return Json(new ResultInfo() { error = 1, msg = "Đã tồn tại" }, JsonRequestBehavior.AllowGet);
            khpl.MaTruong = MaTruong;
            db.Tbl_KyHieuPhanLoai.Add(khpl);
            db.SaveChanges();

            return Json(new ResultInfo() { error = 0, msg = "", data = khpl }, JsonRequestBehavior.AllowGet);

        }

        [HttpPost]
        public ActionResult edit(Tbl_KyHieuPhanLoai khpl)
        {
            if (String.IsNullOrEmpty(khpl.Ma.ToString()) || String.IsNullOrEmpty(khpl.Ten))
                return Json(new ResultInfo() { error = 1, msg = "Missing info" }, JsonRequestBehavior.AllowGet);

            var check = db.Tbl_KyHieuPhanLoai.Where(p => p.MaTruong == MaTruong && p.Ma == khpl.Ma).FirstOrDefault();

            if (check == null)
                return Json(new ResultInfo() { error = 1, msg = "Không tìm thấy thông tin" }, JsonRequestBehavior.AllowGet);

            check.Ma = khpl.Ma;
            check.Ten = khpl.Ten;
            check.GhiChu = khpl.GhiChu;

            db.Entry(check).State = System.Data.Entity.EntityState.Modified;
            db.SaveChanges();

            return Json(new ResultInfo() { error = 0, msg = "", data = check }, JsonRequestBehavior.AllowGet);

        }
        [HttpPost]
        public ActionResult delete(string ma)
        {
            if (String.IsNullOrEmpty(ma.ToString()))
                return Json(new ResultInfo() { error = 1, msg = "Missing info" }, JsonRequestBehavior.AllowGet);

            var check = db.Tbl_KyHieuPhanLoai.Where(p => p.MaTruong == MaTruong && p.Ma == ma).FirstOrDefault();

            if (check == null)
                return Json(new ResultInfo() { error = 1, msg = "Không tìm thấy thông tin" }, JsonRequestBehavior.AllowGet);

            db.Entry(check).State = System.Data.Entity.EntityState.Deleted;
            db.SaveChanges();

            return Json(new ResultInfo() { error = 0, msg = "", data = check }, JsonRequestBehavior.AllowGet);
        }
    }
}