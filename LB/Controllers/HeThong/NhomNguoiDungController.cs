using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using LB.Models;
namespace LB.Controllers.HeThong
{
    public class NhomNguoiDungController : BaseController
    {
        // GET: NhomNguoiDung
        public ActionResult Show()
        {
            return View();
        }

        [HttpGet]
        public ActionResult GetNhom()
        {
            return View(db.UMS_GroupMenu.ToList());
        }
       
    }
}