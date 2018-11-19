using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace LB.Helper
{
    public class UserLogin
    {

    }

    public class SearchThanhLy
    {
        public string MaSach { get; set; }
        public string TenSach { get; set; }
        public string SoCaBiet { get; set; }
        public DateTime? NgayTL { get; set; }
        public int? TrangThai { get; set; }
        public double? GiaTien { get; set; }
        public string TenKho { get; set; }
        public string TacGia { get; set; }
        public string NamXuatBan { get; set; }
        public string NhaXuatBan { get; set; }
    }
}