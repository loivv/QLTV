using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using LB.Models;
using System.Globalization;
namespace LB.Controllers.HeThong
{
    public class NamHocController : BaseController
    {
        // GET: NamHoc
        public ActionResult Show()
        {
            return View();
        }

        [HttpGet]
        public ActionResult getNamHoc()
        {
            
            var data = db.Tbl_NamHoc.Where(p=>p.MaTruong == MaTruong).ToList();

            ResultInfo result = new ResultInfo()
            {
                error = 0,
                msg = "",              
                data = data
            };

            return Json(result, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult create(Tbl_NamHoc nm, string tuNgay, string denNgay)
        {

            if (String.IsNullOrEmpty(nm.Ten) || String.IsNullOrEmpty(tuNgay) || String.IsNullOrEmpty(denNgay))
                return Json(new ResultInfo() { error = 1, msg = "Missing info" }, JsonRequestBehavior.AllowGet);
            string tngay = tuNgay.Substring(0, 2);
            string tthang = tuNgay.Substring(3, 2);
            string tnam = tuNgay.Substring(6, 4);

            string dngay = denNgay.Substring(0, 2);
            string dthang = denNgay.Substring(3, 2);
            string dnam = denNgay.Substring(6, 4);

            string sysFormat = CultureInfo.CurrentCulture.DateTimeFormat.ShortDatePattern;

            //format sever M/d/yyyy
            DateTime dateFrom;
            DateTime dateTo;
            if (sysFormat == "M/d/yyyy")
            {
                dateFrom = DateTime.Parse(tthang + '/' + tngay + '/' + tnam);
                dateTo = DateTime.Parse(dthang + '/' + dngay + '/' + dnam);
            }
            else
            {
                dateFrom = DateTime.Parse(tuNgay);
                dateTo = DateTime.Parse(denNgay);
            }
            var check = db.Tbl_NamHoc.Where(p => p.MaTruong == MaTruong && p.Ten == nm.Ten).FirstOrDefault();

            if (check != null)
                return Json(new ResultInfo() { error = 1, msg = "Đã tồn tại" }, JsonRequestBehavior.AllowGet);
            nm.MaTruong = MaTruong;
            db.Tbl_NamHoc.Add(nm);

            db.SaveChanges();

            return Json(new ResultInfo() { error = 0, msg = "", data = nm }, JsonRequestBehavior.AllowGet);

        }

        [HttpPost]
        public ActionResult edit(Tbl_NamHoc nm, string tuNgay, string denNgay)
        {
            if (String.IsNullOrEmpty(nm.Ten) || String.IsNullOrEmpty(tuNgay) || String.IsNullOrEmpty(denNgay))
                return Json(new ResultInfo() { error = 1, msg = "Missing info" }, JsonRequestBehavior.AllowGet);
            DateTime dateFrom = DateTime.Parse(tuNgay);
            DateTime dateTo = DateTime.Parse(denNgay);
            var check = db.Tbl_NamHoc.Where(p => p.MaTruong == MaTruong && p.Ma ==  nm.Ma).FirstOrDefault();

            if (check == null)
                return Json(new ResultInfo() { error = 1, msg = "Không tìm thấy thông tin" }, JsonRequestBehavior.AllowGet);

            check.MaTruong = MaTruong;
            check.Ten = nm.Ten;
            check.TuNgay = dateFrom;
            check.DenNgay = dateTo;
            check.GhiChu = nm.GhiChu;

            db.Entry(check).State = System.Data.Entity.EntityState.Modified;
            db.SaveChanges();

            return Json(new ResultInfo() { error = 0, msg = "", data = check }, JsonRequestBehavior.AllowGet);

        }
        [HttpPost]
        public ActionResult delete(int id)
        {
            if (String.IsNullOrEmpty(id.ToString()))
                return Json(new ResultInfo() { error = 1, msg = "Missing info" }, JsonRequestBehavior.AllowGet);

            var check = db.Tbl_NamHoc.Where(p => p.MaTruong == MaTruong && p.Ma == id).FirstOrDefault();

            if (check == null)
                return Json(new ResultInfo() { error = 1, msg = "Không tìm thấy thông tin" }, JsonRequestBehavior.AllowGet);

            db.Entry(check).State = System.Data.Entity.EntityState.Deleted;
            db.SaveChanges();

            return Json(new ResultInfo() { error = 0, msg = "", data = check }, JsonRequestBehavior.AllowGet);
        }
    }
}