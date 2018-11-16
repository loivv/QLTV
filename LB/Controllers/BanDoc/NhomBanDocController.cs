using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using LB.Models;
namespace LB.Controllers.BanDoc
{
    public class NhomBanDocController : BaseController
    {
        // GET: NhomBanDoc
        public ActionResult Show()
        {
            return View();
        }

        [HttpGet]
        public ActionResult getNhomBanDoc()
        {

            var data = db.Tbl_NhomDocGia.Where(p => p.MaTruong == MaTruong).ToList();

            ResultInfo result = new ResultWithPaging()
            {
                error = 0,
                msg = "",
                data = data
            };
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        public ActionResult create(Tbl_NhomDocGia ndg)
        {

            if (String.IsNullOrEmpty(ndg.MaNhom.ToString()) || String.IsNullOrEmpty(ndg.TenNhom))
                return Json(new ResultInfo() { error = 1, msg = "Missing info" }, JsonRequestBehavior.AllowGet);

            var check = db.Tbl_NhomDocGia.Where(p => p.MaTruong == MaTruong && p.MaNhom == ndg.MaNhom).FirstOrDefault();

            if (check != null)
                return Json(new ResultInfo() { error = 1, msg = "Đã tồn tại" }, JsonRequestBehavior.AllowGet);
            ndg.MaTruong = MaTruong;
            db.Tbl_NhomDocGia.Add(ndg);
            db.SaveChanges();

            return Json(new ResultInfo() { error = 0, msg = "", data = ndg }, JsonRequestBehavior.AllowGet);

        }

        [HttpPost]
        public ActionResult edit(Tbl_NhomDocGia ndg)
        {
            if (String.IsNullOrEmpty(ndg.MaNhom.ToString()) || String.IsNullOrEmpty(ndg.TenNhom))
                return Json(new ResultInfo() { error = 1, msg = "Missing info" }, JsonRequestBehavior.AllowGet);

            var check = db.Tbl_NhomDocGia.Where(p => p.MaTruong == MaTruong && p.MaNhom == ndg.MaNhom).FirstOrDefault();

            if (check == null)
                return Json(new ResultInfo() { error = 1, msg = "Không tìm thấy thông tin" }, JsonRequestBehavior.AllowGet);

            check.TenNhom = ndg.TenNhom;
            check.HanMuon = ndg.HanMuon;
            check.SoLuong = ndg.SoLuong;

            db.Entry(check).State = System.Data.Entity.EntityState.Modified;
            db.SaveChanges();

            return Json(new ResultInfo() { error = 0, msg = "", data = check }, JsonRequestBehavior.AllowGet);

        }
        [HttpPost]
        public ActionResult delete(int ma)
        {
            if (String.IsNullOrEmpty(ma.ToString()))
                return Json(new ResultInfo() { error = 1, msg = "Missing info" }, JsonRequestBehavior.AllowGet);

            var check = db.Tbl_NhomDocGia.Where(p => p.MaTruong == MaTruong && p.MaNhom == ma).FirstOrDefault();

            if (check == null)
                return Json(new ResultInfo() { error = 1, msg = "Không tìm thấy thông tin" }, JsonRequestBehavior.AllowGet);

            db.Entry(check).State = System.Data.Entity.EntityState.Deleted;
            db.SaveChanges();

            return Json(new ResultInfo() { error = 0, msg = "", data = check }, JsonRequestBehavior.AllowGet);
        }
    }
}