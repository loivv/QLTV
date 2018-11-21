using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace LB.Report
{
    public class ReportCommonData
    {
    }

    public class DMSach
    {
        public string MaSach { get; set; }

        public string SoCaBiet { get; set; }

        public int SoLuong { get; set; }

        public string NhanDe { get; set; }
    }

    public class ThanhLy
    {
        public string NgayTL { get; set; }
        public string MaSach { get; set; }
        public string TenSach { get; set; }
        public string SoCaBiet { get; set; }

        public double GiaTien { get; set; }
    }

    public class BCTonKho
    {
        public string SoCaBiet { get; set; }

        public string MaSach { get; set; }

        public string Loai { get; set; }

        public string TenSach { get; set; }
        
        public string TrangThai { get; set; }
    }
}