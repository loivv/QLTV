using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using LB.Models;
namespace LB.Controllers.DanhMuc
{
    public class TacGiaController : BaseController
    {
        // GET: TacGia
        public ActionResult Show()
        {
            return View();
        }

        [HttpGet]
        public ActionResult getTacGia()
        {

            var data = db.Tbl_TacGia.Where(p => p.MaTruong == MaTruong).ToList().OrderBy(p => p.Ma);
            ResultInfo result = new ResultWithPaging()
            {
                error = 0,
                msg = "",
                data = data
            };

            return Json(result, JsonRequestBehavior.AllowGet);
        }
        public ActionResult create(Tbl_TacGia tg)
        {

            if (String.IsNullOrEmpty(tg.Ma.ToString()) || String.IsNullOrEmpty(tg.Ten))
                return Json(new ResultInfo() { error = 1, msg = "Missing info" }, JsonRequestBehavior.AllowGet);

            var check = db.Tbl_TacGia.Where(p => p.MaTruong == MaTruong && p.Ma == tg.Ma).FirstOrDefault();

            if (check != null)
                return Json(new ResultInfo() { error = 1, msg = "Đã tồn tại" }, JsonRequestBehavior.AllowGet);
            tg.MaTruong = MaTruong;
            db.Tbl_TacGia.Add(tg);
            db.SaveChanges();

            return Json(new ResultInfo() { error = 0, msg = "", data = tg }, JsonRequestBehavior.AllowGet);

        }

        [HttpPost]
        public ActionResult edit(Tbl_TacGia tg)
        {
            if (String.IsNullOrEmpty(tg.Ma.ToString()) || String.IsNullOrEmpty(tg.Ten))
                return Json(new ResultInfo() { error = 1, msg = "Missing info" }, JsonRequestBehavior.AllowGet);

            var check = db.Tbl_TacGia.Where(p => p.MaTruong == MaTruong && p.Ma == tg.Ma).FirstOrDefault();

            if (check == null)
                return Json(new ResultInfo() { error = 1, msg = "Không tìm thấy thông tin" }, JsonRequestBehavior.AllowGet);

            check.Ten = tg.Ten;
            check.GhiChu = tg.GhiChu;           

            db.Entry(check).State = System.Data.Entity.EntityState.Modified;
            db.SaveChanges();

            return Json(new ResultInfo() { error = 0, msg = "", data = check }, JsonRequestBehavior.AllowGet);

        }
        [HttpPost]
        public ActionResult delete(int ma)
        {
            if (String.IsNullOrEmpty(ma.ToString()))
                return Json(new ResultInfo() { error = 1, msg = "Missing info" }, JsonRequestBehavior.AllowGet);

            var check = db.Tbl_TacGia.Where(p => p.MaTruong == MaTruong && p.Ma == ma).FirstOrDefault();

            if (check == null)
                return Json(new ResultInfo() { error = 1, msg = "Không tìm thấy thông tin" }, JsonRequestBehavior.AllowGet);

            db.Entry(check).State = System.Data.Entity.EntityState.Deleted;
            db.SaveChanges();

            return Json(new ResultInfo() { error = 0, msg = "", data = check }, JsonRequestBehavior.AllowGet);
        }
    }
}