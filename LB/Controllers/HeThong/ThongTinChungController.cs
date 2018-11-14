using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using LB.Models;
namespace LB.Controllers.HeThong
{
    public class ThongTinChungController : BaseController
    {
        // GET: ThongTinChung
        public ActionResult Show()
        {
            ViewBag.NamHoc = db.Tbl_NamHoc.Where(p => p.MaTruong == MaTruong).OrderBy(p => p.Ten).ToList();
            var schoolInfo = db.Tbl_ThongTin.Where(p => p.MaTruong == MaTruong).FirstOrDefault();
            ViewBag.Info = schoolInfo;
            ViewBag.MaTruong = MaTruong;
            return View();
        }
        [HttpPost]
        public ActionResult edit(Tbl_ThongTin tt)
        {
            if (String.IsNullOrEmpty(tt.TenTruong))
                return Json(new ResultInfo() { error = 1, msg = "Missing info" }, JsonRequestBehavior.AllowGet);
       
            var check = db.Tbl_ThongTin.Where(p => p.MaTruong == MaTruong ).FirstOrDefault();

            if (check == null)
            {
                tt.MaTruong = MaTruong;
                db.Tbl_ThongTin.Add(tt);
                db.SaveChanges();
            }
            else
            {
                check.DonViCapTren = tt.DonViCapTren;
                check.TenTruong = tt.TenTruong;
                check.DiaChi = tt.DiaChi;
                check.SoDienThoai = tt.SoDienThoai;
                check.Fax = tt.Fax;
                check.Email = tt.Email;
                check.Website = tt.Website;
                check.NamHoc = tt.NamHoc;
                check.HieuTruong = tt.HieuTruong;
                check.KeToan = tt.KeToan;
                check.ThuThu = tt.ThuThu;

                db.Entry(check).State = System.Data.Entity.EntityState.Modified;
                db.SaveChanges();
            }
            return Json(new ResultInfo() { error = 0, msg = "", data = check }, JsonRequestBehavior.AllowGet);

        }
    }
}