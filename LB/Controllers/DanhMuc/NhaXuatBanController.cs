﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using LB.Models;
namespace LB.Controllers.DanhMuc
{
    public class NhaXuatBanController : BaseController
    {
        // GET: NhaXuatBan
        public ActionResult Show()
        {
            return View();
        }

        [HttpGet]
        public ActionResult getNhaXuatBan()
        {

            var data = db.Tbl_NhaXuatBan.Where(p => p.MaTruong == MaTruong).ToList().OrderBy(p => p.Ma);
            ResultInfo result = new ResultWithPaging()
            {
                error = 0,
                msg = "",
                data = data
            };

            return Json(result, JsonRequestBehavior.AllowGet);
        }
        public ActionResult create(Tbl_NhaXuatBan nxb)
        {

            if (String.IsNullOrEmpty(nxb.Ma.ToString()) || String.IsNullOrEmpty(nxb.Ten))
                return Json(new ResultInfo() { error = 1, msg = "Missing info" }, JsonRequestBehavior.AllowGet);

            var check = db.Tbl_NhaXuatBan.Where(p => p.MaTruong == MaTruong && p.Ma == nxb.Ma).FirstOrDefault();

            if (check != null)
                return Json(new ResultInfo() { error = 1, msg = "Đã tồn tại" }, JsonRequestBehavior.AllowGet);
            nxb.MaTruong = MaTruong;
            db.Tbl_NhaXuatBan.Add(nxb);
            db.SaveChanges();

            return Json(new ResultInfo() { error = 0, msg = "", data = nxb }, JsonRequestBehavior.AllowGet);

        }

        [HttpPost]
        public ActionResult edit(Tbl_NhaXuatBan nxb)
        {
            if (String.IsNullOrEmpty(nxb.Ma.ToString()) || String.IsNullOrEmpty(nxb.Ten))
                return Json(new ResultInfo() { error = 1, msg = "Missing info" }, JsonRequestBehavior.AllowGet);

            var check = db.Tbl_NhaXuatBan.Where(p => p.MaTruong == MaTruong && p.Ma == nxb.Ma).FirstOrDefault();

            if (check == null)
                return Json(new ResultInfo() { error = 1, msg = "Không tìm thấy thông tin" }, JsonRequestBehavior.AllowGet);

            check.Ten = nxb.Ten;
            check.GhiChu = nxb.GhiChu;

            db.Entry(check).State = System.Data.Entity.EntityState.Modified;
            db.SaveChanges();

            return Json(new ResultInfo() { error = 0, msg = "", data = check }, JsonRequestBehavior.AllowGet);

        }
        [HttpPost]
        public ActionResult delete(int ma)
        {
            if (String.IsNullOrEmpty(ma.ToString()))
                return Json(new ResultInfo() { error = 1, msg = "Missing info" }, JsonRequestBehavior.AllowGet);

            var check = db.Tbl_NhaXuatBan.Where(p => p.MaTruong == MaTruong && p.Ma == ma).FirstOrDefault();

            if (check == null)
                return Json(new ResultInfo() { error = 1, msg = "Không tìm thấy thông tin" }, JsonRequestBehavior.AllowGet);

            db.Entry(check).State = System.Data.Entity.EntityState.Deleted;
            db.SaveChanges();

            return Json(new ResultInfo() { error = 0, msg = "", data = check }, JsonRequestBehavior.AllowGet);
        }
    }
}