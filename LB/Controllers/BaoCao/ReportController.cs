using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using LB.Models;
using LB.Report;

namespace LB.Controllers.BaoCao
{
    public class ReportController : Controller
    {
        ssofty_com_thuvienEntities db = new ssofty_com_thuvienEntities();

        // GET: Report
        public ActionResult Show()
        {

            return View();

          
        }

        public ActionResult GetReport()
        {
            ReportUtils utils = new ReportUtils(db);
           
            List<string> dsDocGia = new List<string>();
            dsDocGia.Add("loivv");

            return File(utils.RptTheThuVien(dsDocGia, "LQDON"), "application/pdf");
           
        }
    }
}