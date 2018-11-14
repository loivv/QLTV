using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using LB.Models;
namespace LB.Controllers.HeThong
{
    public class NguoiDungController : BaseController
    {
        // GET: NguoiDung
        public ActionResult Show()
        {
            ViewBag.AllNhom = db.Tbl_NhomNguoiDung.Where(p => p.MaTruong == MaTruong).ToList();
            return View();
        }
        [HttpGet]
        public ActionResult getNhanVien()
        {

            var query = from t in db.Tbl_NhanVien
                        orderby t.HoTen
                        where t.MaTruong == MaTruong
                        select t;

            ResultInfo result = new ResultInfo()
            {
                error = 0,
                msg = "",
                data = query
            };


            return Json(result, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public ActionResult create(Tbl_NhanVien nv)
        {
            // if (String.IsNullOrEmpty(ph.MaLoai))
            //     return Json(new ResultInfo() { error = 1, msg = "Missing info" }, JsonRequestBehavior.AllowGet);

            var check = db.Tbl_NhanVien.Where(p => p.MaTruong == MaTruong && p.MaNV == nv.MaNV).FirstOrDefault();

            if (check != null)
                return Json(new ResultInfo() { error = 1, msg = "Đã tồn tại" }, JsonRequestBehavior.AllowGet);

            nv.MaTruong = MaTruong;
            db.Tbl_NhanVien.Add(nv);

            db.SaveChanges();
            return Json(new ResultInfo() { error = 0, msg = "", data = nv }, JsonRequestBehavior.AllowGet);

        }
        [HttpPost]
        public ActionResult edit(Tbl_NhanVien nv)
        {
            // if (String.IsNullOrEmpty(ph.MaLoai))
            //     return Json(new ResultInfo() { error = 1, msg = "Missing info" }, JsonRequestBehavior.AllowGet);

            var check = db.Tbl_NhanVien.Where(p => p.MaTruong == MaTruong && p.MaNV == nv.MaNV).FirstOrDefault();

            if (check == null)
                return Json(new ResultInfo() { error = 1, msg = "Không tìm thấy thông tin" }, JsonRequestBehavior.AllowGet);

            check.GioiTinh = nv.GioiTinh;
            check.HoTen = nv.HoTen;
            check.KichHoat = nv.KichHoat;
            check.MatKhau = System.Web.Security.FormsAuthentication.HashPasswordForStoringInConfigFile(nv.MatKhau, "SHA1");
            check.NgaySinh = nv.NgaySinh;
            check.Quyen = nv.Quyen;
            check.SDT = nv.SDT;
            db.Entry(check).State = System.Data.Entity.EntityState.Modified;

            db.SaveChanges();
            return Json(new ResultInfo() { error = 0, msg = "", data = check }, JsonRequestBehavior.AllowGet);

        }
        [HttpPost]
        public ActionResult delete(string manv)
        {
            if (String.IsNullOrEmpty(manv))
                return Json(new ResultInfo() { error = 1, msg = "Missing info" }, JsonRequestBehavior.AllowGet);

            var check = db.Tbl_NhanVien.Where(p => p.MaTruong == MaTruong && p.MaNV == manv).FirstOrDefault();

            if (check == null)
                return Json(new ResultInfo() { error = 1, msg = "Không tìm thấy thông tin" }, JsonRequestBehavior.AllowGet);

            db.Entry(check).State = System.Data.Entity.EntityState.Deleted;
            db.SaveChanges();


            return Json(new ResultInfo() { error = 0, msg = "", data = check }, JsonRequestBehavior.AllowGet);
        }
    }
}