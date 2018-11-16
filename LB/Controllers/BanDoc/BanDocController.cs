using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using LB.Models;
using System.Globalization;
namespace LB.Controllers.BanDoc
{
    public class BanDocController : BaseController
    {
        // GET: BanDoc
        public ActionResult Show()
        {
            ViewBag.AllNhom = db.Tbl_NhomDocGia.Where(p => p.MaTruong == MaTruong).ToList();
            return View();
        }

        [HttpGet]
        public ActionResult getBanDoc(int? page)
        {
            int pageSize = 50;
            int pageNumber = (page ?? 1);
            var data = (from s in db.Tbl_DocGia
                         join l in db.Tbl_NhomDocGia
                         on s.MaNhom equals l.MaNhom
                         where s.MaTruong == MaTruong
                         select new 
                         {
                             MaDocGia = s.MaDocGia,
                             HoTen = s.HoTen,
                             Lop = s.Lop,
                             NgayDangKy = s.NgayDangKy,
                             MaNhom =s.MaNhom,
                             TenNhom = l.TenNhom,
                             NgayHetHan = s.NgayHetHan,
                             SoThe = s.SoThe,
                             TrangThai = s.TrangThai,
                             DiaChi = s.DiaChi,
                             NgaySinh = s.NgaySinh,
                             In = false
                         }).ToList();

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
        [HttpPost]
        public ActionResult create(Tbl_DocGia dg, string tuNgay,string denNgay)
        {

            if (String.IsNullOrEmpty(dg.MaDocGia.ToString()) || String.IsNullOrEmpty(dg.HoTen))
                return Json(new ResultInfo() { error = 1, msg = "Missing info" }, JsonRequestBehavior.AllowGet);

            string tngay = tuNgay.Substring(0, 2);
            string tthang = tuNgay.Substring(3, 2);
            string tnam = tuNgay.Substring(6, 4);

            string dngay = denNgay.Substring(0, 2);
            string dthang = denNgay.Substring(3, 2);
            string dnam = denNgay.Substring(6, 4);

            string sysFormat = CultureInfo.CurrentCulture.DateTimeFormat.ShortDatePattern;
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

            var check = db.Tbl_DocGia.Where(p => p.MaTruong == MaTruong && p.MaDocGia == dg.MaDocGia).FirstOrDefault();

            if (check != null)
                return Json(new ResultInfo() { error = 1, msg = "Đã tồn tại" }, JsonRequestBehavior.AllowGet);

            var ngayhh = db.Tbl_NhomDocGia.Where(p => p.MaTruong == MaTruong && p.MaNhom == dg.MaNhom).FirstOrDefault();
            DateTime dateHan = dateFrom.AddDays(double.Parse(ngayhh.HanMuon.ToString()));
            dg.MaTruong = MaTruong;
            dg.NgayDangKy = dateFrom;
            dg.NgayHetHan = dateHan;
            dg.NgaySinh = dateTo;
            //maxid
            string smaxid;
            try
            {
                smaxid = db.Tbl_DocGia.Where(t => t.MaTruong == MaTruong).Max(u => u.SoThe);
                smaxid = (Convert.ToUInt32(smaxid) + 1).ToString("D5");
            }
            catch
            {
                smaxid = "00001";
            }

            dg.SoThe = smaxid;
            db.Tbl_DocGia.Add(dg);
            db.SaveChanges();

            return Json(new ResultInfo() { error = 0, msg = "", data = dg }, JsonRequestBehavior.AllowGet);

        }

        [HttpPost]
        public ActionResult edit(Tbl_DocGia dg,string tuNgay,string denNgay)
        {
            if (String.IsNullOrEmpty(dg.MaDocGia.ToString()) || String.IsNullOrEmpty(dg.HoTen))
                return Json(new ResultInfo() { error = 1, msg = "Missing info" }, JsonRequestBehavior.AllowGet);

            var check = db.Tbl_DocGia.Where(p => p.MaTruong == MaTruong && p.MaDocGia == dg.MaDocGia).FirstOrDefault();

            string tngay = tuNgay.Substring(0, 2);
            string tthang = tuNgay.Substring(3, 2);
            string tnam = tuNgay.Substring(6, 4);

            string dngay = denNgay.Substring(0, 2);
            string dthang = denNgay.Substring(3, 2);
            string dnam = denNgay.Substring(6, 4);

            string sysFormat = CultureInfo.CurrentCulture.DateTimeFormat.ShortDatePattern;
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

            if (check == null)
                return Json(new ResultInfo() { error = 1, msg = "Không tìm thấy thông tin" }, JsonRequestBehavior.AllowGet);

            var ngayhh = db.Tbl_NhomDocGia.Where(p => p.MaTruong == MaTruong && p.MaNhom == dg.MaNhom).FirstOrDefault();
            DateTime dateHan = dateFrom.AddDays(double.Parse(ngayhh.HanMuon.ToString()));
            check.MaTruong = MaTruong;
            check.NgayDangKy = dateFrom;
            check.NgayHetHan = dateHan;
            check.MaNhom = dg.MaNhom;
            check.HoTen = dg.HoTen;
            check.NgaySinh = dateTo;
            check.DiaChi = dg.DiaChi;
            check.MaNhom = dg.MaNhom;
            check.Lop = dg.Lop;
           
            check.TrangThai = dg.TrangThai;
            db.Entry(check).State = System.Data.Entity.EntityState.Modified;
            db.SaveChanges();

            return Json(new ResultInfo() { error = 0, msg = "", data = check }, JsonRequestBehavior.AllowGet);

        }
        [HttpPost]
        public ActionResult delete(string ma)
        {
            if (String.IsNullOrEmpty(ma))
                return Json(new ResultInfo() { error = 1, msg = "Missing info" }, JsonRequestBehavior.AllowGet);

            var check = db.Tbl_DocGia.Where(p => p.MaTruong == MaTruong && p.MaDocGia == ma).FirstOrDefault();

            if (check == null)
                return Json(new ResultInfo() { error = 1, msg = "Không tìm thấy thông tin" }, JsonRequestBehavior.AllowGet);

            db.Entry(check).State = System.Data.Entity.EntityState.Deleted;
            db.SaveChanges();

            return Json(new ResultInfo() { error = 0, msg = "", data = check }, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public ActionResult giahan(int ngay)
        {

            var data = db.Tbl_DocGia.Where(p => p.MaTruong == MaTruong && p.NgayHetHan < DateTime.Now).ToList();
            foreach(var item in data)
            {
                var check = db.Tbl_DocGia.Where(p => p.MaDocGia == item.MaDocGia && p.MaTruong == MaTruong).FirstOrDefault();
                if(check != null)
                {
                    check.NgayHetHan = DateTime.Now.AddDays(double.Parse(ngay.ToString()));
                    db.Entry(check).State = System.Data.Entity.EntityState.Modified;
                }
            }
            db.SaveChanges();
            ResultInfo result = new ResultInfo()
            {
                error = 0,
                msg = "",
                data = ngay
            };
            return Json(result, JsonRequestBehavior.AllowGet);
        }
    }
}