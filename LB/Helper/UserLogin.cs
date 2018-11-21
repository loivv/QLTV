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

    public class MuonSachTheoMaBanDoc
    {
        public string MaMuon { get; set; }
        public DateTime? NgayMuon { get; set; }
        public string SoCaBiet { get; set; }
        public string TrangThai { get; set; }
        public DateTime? NgayTra { get; set; }
        public string NguoiNhap { get; set; }
        public string TenSach { get; set; }
        public string MaSach { get; set; }
        public string MaHS { get; set; }
        public string TenHS { get; set; }
        public bool? DaTra { get; set; }
        public string GhiChu { get; set; }
    }


    public class ThongKeAnPhamMuonNhieu
    {
        public string NhanDe { get; set; }
        public string TacGia { get; set; }
        public string NamXuatBan { get; set; }
        public string NhaXuatBan { get; set; }
        public int? SoLanMuon { get; set; }
    }

    public class MuonSach
    {
        public string MaXuat { get; set; }
        public string MaDocGia { get; set; }
        public string HoTen { get; set; }
        public int? MaNhom { get; set; }
        public string TenNhom { get; set; }
        public int? SoLuong { get; set; }
        public DateTime? NgayMuon { get; set; }
        public DateTime? NgayTra { get; set; }
        public string GhiChu { get; set; }
        public string DaTra { get; set; }
        public string TrangThai { get; set; }
        public string NguoiNhap { get; set; }
        public string Lop { get; set; }
        public string TenSach { get; set; }
        public string SoCaBiet { get; set; }
        public string MaSach { get; set; }
        public string HinhThuc { get; set; }
        public string SoThe { get; set; }
        public string TacGia { get; set; }
        public string NamXuatBan { get; set; }
        public string NhaXuatBan { get; set; }
        public string TenKho { get; set; }
    }
    public class BaoCaoCaBiet
    {
        public DateTime? NgayVaoSo { get; set; }
        public string TTSach { get; set; }
        public string STT { get; set; }
        public string TGTS { get; set; }
        public string NXB { get; set; }
        public string NoiXuatBan { get; set; }
        public string NamXuatBan { get; set; }
        public double? DonGia { get; set; }
        public int? Nguon { get; set; }
        public string MonLoai { get; set; }
        public string SoVaoSoTQ { get; set; }
        public string SoBienXB { get; set; }
        public string NamKK { get; set; }


    }
}