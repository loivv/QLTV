using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using LB.Models;
namespace LB.Controllers.HeThong
{
    public class NgonNguController : BaseController
    {
        // GET: NgonNgu
        public ActionResult Show()
        {
            return View();
        }

        [HttpGet]
        public ActionResult getNgonNgu()
        {

            var data = db.Tbl_NgonNgu.Where(p => p.MaTruong == "ALL").ToList().OrderBy(p => p.Ma);
            ResultInfo result = new ResultWithPaging()
            {
                error = 0,
                msg = "",
                data = data
            };

            return Json(result, JsonRequestBehavior.AllowGet);
        }
        public ActionResult create(Tbl_NgonNgu nn)
        {

            if (String.IsNullOrEmpty(nn.Ten) || String.IsNullOrEmpty(nn.KyHieu))
                return Json(new ResultInfo() { error = 1, msg = "Missing info" }, JsonRequestBehavior.AllowGet);

            var check = db.Tbl_NgonNgu.Where(p => p.MaTruong == "ALL" && p.Ma == nn.Ma).FirstOrDefault();

            if (check != null)
                return Json(new ResultInfo() { error = 1, msg = "Đã tồn tại" }, JsonRequestBehavior.AllowGet);
            nn.MaTruong = "ALL";
            db.Tbl_NgonNgu.Add(nn);
            db.SaveChanges();

            return Json(new ResultInfo() { error = 0, msg = "", data = nn }, JsonRequestBehavior.AllowGet);

        }

        [HttpPost]
        public ActionResult edit(Tbl_NgonNgu nn)
        {
            if (String.IsNullOrEmpty(nn.Ten) || String.IsNullOrEmpty(nn.KyHieu))
                return Json(new ResultInfo() { error = 1, msg = "Missing info" }, JsonRequestBehavior.AllowGet);

            var check = db.Tbl_NgonNgu.Where(p => p.MaTruong == "ALL" && p.Ma == nn.Ma).FirstOrDefault();

            if (check == null)
                return Json(new ResultInfo() { error = 1, msg = "Không tìm thấy thông tin" }, JsonRequestBehavior.AllowGet);

            check.Ten = nn.Ten;
            check.GhiChu = nn.GhiChu;
            check.KyHieu = nn.KyHieu;

            db.Entry(check).State = System.Data.Entity.EntityState.Modified;
            db.SaveChanges();

            return Json(new ResultInfo() { error = 0, msg = "", data = check }, JsonRequestBehavior.AllowGet);

        }
        [HttpPost]
        public ActionResult delete(int ma)
        {
            if (String.IsNullOrEmpty(ma.ToString()))
                return Json(new ResultInfo() { error = 1, msg = "Missing info" }, JsonRequestBehavior.AllowGet);

            var check = db.Tbl_NgonNgu.Where(p => p.MaTruong == "ALL" && p.Ma == ma).FirstOrDefault();

            if (check == null)
                return Json(new ResultInfo() { error = 1, msg = "Không tìm thấy thông tin" }, JsonRequestBehavior.AllowGet);

            db.Entry(check).State = System.Data.Entity.EntityState.Deleted;
            db.SaveChanges();

            return Json(new ResultInfo() { error = 0, msg = "", data = check }, JsonRequestBehavior.AllowGet);
        }
    }
}