using CrystalDecisions.CrystalReports.Engine;
using System;
using System.Collections.Generic;
using System.IO;
using System.Web.Mvc;
using System.Linq;
using System.Web;
using LB.Models;
using System.Data.SqlClient;

namespace LB.Report
{
    public class ReportUtils
    {

        ssofty_com_thuvienEntities db;

        public ReportUtils(ssofty_com_thuvienEntities db)
        {
            this.db = db;
        }


        #region report The Thu Vien
        public Stream RptTheThuVien(List<string> dsbandoc, string matruong)
        {
            var query = db.Rpt_TheThuVien(matruong).Where(p => dsbandoc.Contains(p.MaDocGia)).Select(s => new Report.BanDoc.BanDoc
            {
                HoTen = s.HoTen,
                SoThe = s.SoThe,
                Lop = s.Lop,
                NgayHetHan = s.NgayHetHan,
                NgaySinh = s.NgayHetHan,
                TenThe = "THẺ " + s.TenNhom.ToUpper(),
                TenTruong = s.TenTruong,
                TenPhong = s.TenPhong
            }).ToList();

            return GetReportStream(ReportPath.RptTheThuVien, query);
        }
        #endregion


        #region Report Bien Ban Phat The
        public Stream RptBienBanPhatThe(string maTruong, int maPhat)
        {
            var data = db.Rpt_PhatTheBanDoc(maTruong, maPhat).FirstOrDefault();
            List<Report.BanDoc.PhatThe> dsPhatThe = new List<BanDoc.PhatThe>();

            if (data != null)
            {
                Report.BanDoc.PhatThe phatThe = new Report.BanDoc.PhatThe()
                {
                    HoTen = data.HoTen,
                    GhiChu = data.GhiChu,
                    LyDo = data.LyDo,
                    MaTruong = data.MaTruong,
                    TenPhong = data.TenPhong,
                    TenTruong = data.TenTruong,
                    SoThe = data.SoThe

                };

                if (data.HinhThuc == "Giam thẻ")
                {
                    phatThe.HinhThuc = data.HinhThuc + " :  Từ ngày " + data.TuNgay + " Đến ngày " + data.DenNgay;
                }
                else if (data.HinhThuc == "Phạt tiền")
                {
                    phatThe.HinhThuc = data.HinhThuc + " :  " + data.SoTien;
                }
                else
                {
                    phatThe.HinhThuc = data.Khac;
                }

                dsPhatThe.Add(phatThe);
            }

            return GetReportStream(ReportPath.RptBienBanPhatThe, dsPhatThe);
        }
        #endregion


        #region report nhan an pham
        public Stream RpTNhanAnPham(List<string> dsMa)
        {
            var data = dsMa.Select(p => new Report.AnPham.MaAnPham()
            {
                Ma = "*" + p + "*"
            }).ToList();

            return GetReportStream(ReportPath.RptNhanAnPham, data);
        }

        #endregion

        #region Report Bien Ban Chua Kiem Ke
        public Stream RptBienBanChuaKiemKe(string maTruong)
        {
            var paramatruong = new SqlParameter("@MaTruong", maTruong);
            var result = db.Database.SqlQuery<Helper.SearchThanhLy>("BaoCaoChuaKiemKe @MaTruong", paramatruong).ToList();

            List<Report.KiemKe.KiemKe> listKiemKe = new List<KiemKe.KiemKe>();
            int stt = 1;
            foreach(var item in result)
            {
                listKiemKe.Add(new KiemKe.KiemKe()
                {
                    NamXuatBan = item.NamXuatBan,
                    NgayKiemKe = item.NgayTL.Value.ToString("dd/MM/yyyy"),
                    NhanDe = item.TenSach,
                    NhaXuatBan = item.NhaXuatBan,
                    SoCaBiet = item.SoCaBiet,
                    TacGia = item.TacGia,
                    STT = stt.ToString()
                });
                stt++;
            }

            var data = db.Tbl_ThongTin.Where(p => p.MaTruong == maTruong).FirstOrDefault();

            Dictionary<string, string> textValues = new Dictionary<string, string>();
            textValues.Add("txtTenTruong", data.TenTruong);
            textValues.Add("txtTenPhong", data.DonViCapTren);
            Dictionary<string, Dictionary<string, string>> values = new Dictionary<string, Dictionary<string, string>>();
            values.Add("Section2", textValues);

            return GetReportStream(ReportPath.RptBienBanChuaKiemKe, listKiemKe, values);
        }
        #endregion

        public Stream GetReportStream(string reportPath, Object data)
        {
            ReportDocument rptH = new ReportDocument();

            rptH.Load(HttpContext.Current.Server.MapPath(reportPath));

            if (data != null)
            {
                rptH.SetDataSource(data);
            }

            Stream stream = rptH.ExportToStream(CrystalDecisions.Shared.ExportFormatType.PortableDocFormat);

            return stream;
        }


        public Stream GetReportStream(string reportPath, Object data, Dictionary<string, Dictionary<string, string>> values)
        {
            ReportDocument rptH = new ReportDocument();

            rptH.Load(HttpContext.Current.Server.MapPath(reportPath));


            foreach (KeyValuePair<string, Dictionary<string, string>> kvp in values)
            {
                var key = kvp.Key.ToString();

                foreach (KeyValuePair<string, string> item in kvp.Value)
                {
                    TextObject _txt = (TextObject)rptH.ReportDefinition.Sections[key].ReportObjects[item.Key];
                    _txt.Text = item.Value;
                }
            }

            if (data != null)
            {
                rptH.SetDataSource(data);
            }



            Stream stream = rptH.ExportToStream(CrystalDecisions.Shared.ExportFormatType.PortableDocFormat);

            return stream;
        }
    }

    public class ReportPath
    {
        public static string RptTheThuVien = "~/Report/BanDoc/rptTheThuVien.rpt";
        public static string RptBienBanPhatThe = "~/Report/BanDoc/rptBienBanPhatThe.rpt";
        public static string RptNhanAnPham = "~/Report/AnPham/rptNhanAnPham.rpt";
        public static string RptBienBanChuaKiemKe = "~/Report/KiemKe/rptBienBanChuaKiemKe.rpt";
    }


}