using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace LB.Report.BangKeAnPham
{
    public class AnPhamMuonNhieuNhat
    {
        public string NhanDe { get; set; }
        public string TacGia { get; set; }
        public string NamXuatBan { get; set; }
        public string NhaXuatBan  { get; set; }
        public string SoLanMuon { get; set; }
    }

    public class AnPhamDangMuon
    {
        public string KhoSach { get; set; }
        public string NhanDe { get; set; }
        public string SoCaBiet { get; set; }
        public string TacGia { get; set; }
        public string NamXuatBan { get; set; }
        public string NhaXuatBan { get; set; }
        public string NgayMuon { get; set; }
        public string SoThe { get; set; }
        public string HoTen { get; set; }
    }


    public class AnPhamHienCo
    {
        public string KhoSach { get; set; }
        public string NhanDe { get; set; }
        public string SoCaBiet { get; set; }
        public string TacGia { get; set; }
        public string NamXuatBan { get; set; }
        public string NhaXuatBan { get; set; }

        public string GhiChu { get; set; }
    }

}