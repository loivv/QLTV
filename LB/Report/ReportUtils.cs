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
            foreach (var item in result)
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


        #region Report Bien Ban Da Kiem Ke
        public Stream RptBienBanKiemKe(string maTruong)
        {
            var paramatruong = new SqlParameter("@MaTruong", maTruong);
            var query = from tl in db.Tbl_KiemKe
                        join tlct in db.Tbl_KiemKeChiTiet on tl.Ma equals tlct.Ma
                        join t in db.Tbl_SoCaBiet on tlct.SoCaBiet equals t.STT
                        join s in db.Tbl_Sach on t.MaSach equals s.MaSach
                        join k in db.Tbl_KhoSach on s.MaLoai equals k.Ma
                        join tg in db.Tbl_TacGia on s.TacGia equals tg.Ma
                        join nxb in db.Tbl_NhaXuatBan on s.NhaXuatBan equals nxb.Ma
                        where t.MaTruong == maTruong
                        && tl.MaTruong == maTruong
                        && tlct.MaTruong == maTruong
                        && t.MaTruong == maTruong
                        && k.MaTruong == maTruong
                        && tg.MaTruong == maTruong
                        && nxb.MaTruong == maTruong
                        && s.MaTruong == maTruong
                        select new Helper.SearchThanhLy
                        {
                            MaSach = t.MaSach,
                            TenSach = s.NhanDe,
                            SoCaBiet = tlct.SoCaBiet,
                            TacGia = tg.Ten,
                            TenKho = k.Ten,
                            NamXuatBan = s.NamXuatBan,
                            NhaXuatBan = nxb.Ten,
                            NgayTL = tl.Ngay
                        };

            var result = query.ToList();

            List<Report.KiemKe.KiemKe> listKiemKe = new List<KiemKe.KiemKe>();
            int stt = 1;
            foreach (var item in result)
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

            return GetReportStream(ReportPath.RptBienBanKiemKe, listKiemKe, values);
        }
        #endregion


        #region report phieu nhap kho
        public Stream RptPhieuNhapKho(string maTruong)
        {
            var data = db.Tbl_BienMuc.Where(p => p.MaTruong == maTruong).Select(p => new LuuThong.BCNhapKho()
            {
                Gia = (double)p.Gia,
                NhanDe = p.NhanDe,
                SoLuong = p.SoLuong.ToString(),
                SoPhieu = p.SoPhieu,
                ThanhTien = (double)p.ThanhTien,
                STT = "",
                NgayNhap = ""

            }).ToList();

            var ttTruong = db.Tbl_ThongTin.Where(p => p.MaTruong == maTruong).FirstOrDefault();

            Dictionary<string, string> textValues = new Dictionary<string, string>();
            textValues.Add("txtTenTruong", ttTruong.TenTruong);
            textValues.Add("txtTenPhong", ttTruong.DonViCapTren);
            Dictionary<string, Dictionary<string, string>> values = new Dictionary<string, Dictionary<string, string>>();
            values.Add("Section2", textValues);

            return GetReportStream(ReportPath.RptPhieuNhapKho, data, values);
        }
        #endregion


        #region report BB Muon Sah
        public Stream RptBBMuonSach(string smatruong, string smabandoc, string smamuon,
            string snhanvien, string shoten, DateTime dngaymuon, DateTime dngaytra)
        {
            var query = from t in db.Tbl_XuatSachChiTiet
                        join s in db.Tbl_Sach on t.MaSach equals s.MaSach
                        where t.MaTruong == smatruong && s.MaTruong == smatruong
                        && t.MaXuat == smamuon
                        select new Report.DMSach
                        {
                            MaSach = s.MaSach,
                            NhanDe = s.NhanDe,
                            SoLuong = (int)t.SoLuong,
                            SoCaBiet = t.SoCaBiet
                        };

            var data = query.ToList();
            var ttTruong = db.Tbl_ThongTin.Where(p => p.MaTruong == smatruong).FirstOrDefault();

            Dictionary<string, string> textValues = new Dictionary<string, string>();
            textValues.Add("txtTenTruong", ttTruong.TenTruong);
            textValues.Add("txtTenPhong", ttTruong.DonViCapTren);
            textValues.Add("txtMaBanDoc", smabandoc);
            textValues.Add("txtTenBanDoc", shoten);
            textValues.Add("txtNgayMuon", dngaymuon.ToString("dd/MM/yyyy"));
            textValues.Add("txtNgayTra", dngaytra.ToString("dd/MM/yyyy"));
            Dictionary<string, Dictionary<string, string>> values = new Dictionary<string, Dictionary<string, string>>();
            values.Add("Section2", textValues);

            return GetReportStream(ReportPath.RptBBMuonSach, data, values);
        }

        #endregion
        #region Report BB Thanh Ly
        public Stream RptBBThanhLy(string smatruong, string smathanhly, string sngaythanhly)
        {
            var query = from t in db.Tbl_Sach
                        join d in db.Tbl_ThanhLyChiTiet on t.MaSach equals d.MaSach
                        join b in db.Tbl_BienMuc on t.MaSach equals b.MaSach
                        where d.MaTruong == smatruong && d.MaTL == smathanhly
                        select new Report.ThanhLy
                        {
                            MaSach = t.MaSach,
                            TenSach = t.NhanDe,
                            SoCaBiet = d.SoCaBiet,
                            GiaTien = (double)b.Gia
                        };

            var data = query.ToList();

            var ttTruong = db.Tbl_ThongTin.Where(p => p.MaTruong == smatruong).FirstOrDefault();


            Dictionary<string, string> textValues = new Dictionary<string, string>();

            textValues.Add("txtMaThanhLy", smathanhly);
            textValues.Add("txtNgayThanhLy", sngaythanhly);
            textValues.Add("txtTenTruong", ttTruong.TenTruong);
            textValues.Add("txtTenPhong", ttTruong.DonViCapTren);
            textValues.Add("txtHieuTruong", ttTruong.HieuTruong.ToUpper());
            textValues.Add("txtKeToan", ttTruong.KeToan.ToUpper());
            textValues.Add("txtThuThu", ttTruong.ThuThu.ToUpper());
            Dictionary<string, Dictionary<string, string>> values = new Dictionary<string, Dictionary<string, string>>();
            values.Add("Section2", textValues);

            return GetReportStream(ReportPath.RptBBThanhLy, data, values);
        }



        #endregion


        #region report RptBaoCaoTon
        public Stream RptBaoCaoTon()
        {
            return GetReportStream(ReportPath.RptBaoCaoTon, null);
        }
        #endregion


        #region report BiaSo

        public Stream RptBiaSo(string maTruong, string tenSo)
        {
            var ttTruong = db.Tbl_ThongTin.Where(p => p.MaTruong == maTruong).FirstOrDefault();
            Dictionary<string, string> textValues = new Dictionary<string, string>();

            textValues.Add("txtTenSo", "MƯỢN SÁCH CỦA " + tenSo.ToUpper());
            textValues.Add("txtTenTruong", ttTruong.TenTruong);
            textValues.Add("txtNamHoc", ttTruong.NamHoc.ToString());

            Dictionary<string, Dictionary<string, string>> values = new Dictionary<string, Dictionary<string, string>>();
            values.Add("Section3", textValues);

            return GetReportStream(ReportPath.RptBiaSo, null, values);
        }

        #endregion

        #region report RptSoMuonSach
        public Stream RptSoMuonSach(string smatruong, string shoten, string slop, string sdiachi, string sothe, DateTime tungay, DateTime denngay, string snhom)
        {
            var query = (from t in db.Tbl_XuatSachTong
                         join c in db.Tbl_XuatSachChiTiet on t.MaXuat equals c.MaXuat
                         join s in db.Tbl_Sach on c.MaSach equals s.MaSach
                         where t.MaTruong == smatruong
                         && c.MaTruong == smatruong
                         && s.MaTruong == smatruong
                         && t.MaHS == sothe
                         && (t.NgayMuon >= tungay.Date && t.NgayMuon <= denngay.Date)
                         orderby c.MaXuat
                         select new Helper.MuonSachTheoMaBanDoc
                         {
                             MaMuon = t.MaXuat,
                             NgayMuon = t.NgayMuon,
                             SoCaBiet = c.SoCaBiet,
                             TenSach = s.NhanDe,
                             TrangThai = c.TrangThai,
                             NgayTra = c.NgayTra,
                             NguoiNhap = t.NguoiNhap,
                             GhiChu = t.HinhThuc,
                             DaTra = c.DaTra,
                             MaHS = t.MaHS,
                             MaSach = s.MaSach
                         }).ToList();

            string sngaytra = string.Empty;
            var data = new List<SoGiaoVien.SoMuonSach>();
            foreach (var item in query)
            {
                try
                {
                    sngaytra = (DateTime.Parse(item.NgayTra.ToString())).ToString("dd/MM/yyyy");
                }
                catch
                {
                    sngaytra = "";
                }
                data.Add(new SoGiaoVien.SoMuonSach()
                {
                    NgayMuon = (DateTime.Parse(item.NgayMuon.ToString())).ToString("dd/MM/yyyy"),
                    NgayTra = sngaytra,
                    GhiChu = item.GhiChu,
                    SoCaBiet = item.SoCaBiet,
                    TenSach = item.TenSach
                });
            }
            Dictionary<string, string> textValues = new Dictionary<string, string>();

            textValues.Add("txtHoTen", snhom.ToUpper());
            textValues.Add("txtLop", slop);
            textValues.Add("txtDiaChi", sdiachi);

            Dictionary<string, Dictionary<string, string>> values = new Dictionary<string, Dictionary<string, string>>();
            values.Add("Section2", textValues);


            return GetReportStream(ReportPath.RptSoMuonSach, data, values);

        }

        #endregion


        #region RptAnPhamMuonNhieuNhat
        public Stream RptAnPhamMuonNhieuNhat(string smatruong, DateTime stungay, DateTime sdenngay)
        {
            var paramatruong = new SqlParameter("@MaTruong", smatruong);
            var paratungay = new SqlParameter("@TuNgay", stungay.Date.ToString("yyyy-MM-dd"));
            var paradenngay = new SqlParameter("@DenNgay", sdenngay.Date.ToString("yyyy-MM-dd"));
            var result = db.Database.SqlQuery<Helper.ThongKeAnPhamMuonNhieu>("ThongKe_AnPham_MuonNhieuNhat @MaTruong, @TuNgay, @DenNgay", paramatruong, paratungay, paradenngay).ToList();

            var data = new List<BangKeAnPham.AnPhamMuonNhieuNhat>();
            foreach (var item in result)
            {
                data.Add(new BangKeAnPham.AnPhamMuonNhieuNhat()
                {
                    NamXuatBan = item.NamXuatBan,
                    NhanDe = item.NhanDe,
                    NhaXuatBan = item.NhaXuatBan,
                    SoLanMuon = item.SoLanMuon.ToString(),
                    TacGia = item.TacGia
                });
            }

            var ttTruong = db.Tbl_ThongTin.Where(p => p.MaTruong == smatruong).FirstOrDefault();
            Dictionary<string, string> textValues = new Dictionary<string, string>();

            textValues.Add("txtNgay", "Từ ngày " + stungay.Date.ToString("dd/MM/yyyy") + " đến ngày " + sdenngay.Date.ToString("dd/MM/yyyy"));
            textValues.Add("txtTenTruong", ttTruong.TenTruong);
            textValues.Add("txtTenPhong", ttTruong.DonViCapTren);
            textValues.Add("txtThuThu", ttTruong.ThuThu);
            Dictionary<string, Dictionary<string, string>> values = new Dictionary<string, Dictionary<string, string>>();
            values.Add("Section2", textValues);

            return GetReportStream(ReportPath.RptAnPhamMuonNhieuNhat, data, values);
        }
        #endregion


        #region RptBangKeAnPhamDangMuon
        public Stream RptBangKeAnPhamDangMuon(string smatruong)
        {
            var query = from t in db.Tbl_XuatSachTong
                        join ct in db.Tbl_XuatSachChiTiet on t.MaXuat equals ct.MaXuat
                        join d in db.Tbl_DocGia on t.MaHS equals d.SoThe
                        join n in db.Tbl_NhomDocGia on d.MaNhom equals n.MaNhom
                        join s in db.Tbl_Sach on ct.MaSach equals s.MaSach
                        join k in db.Tbl_KhoSach on s.MaLoai equals k.Ma
                        join tg in db.Tbl_TacGia on s.TacGia equals tg.Ma
                        join nxb in db.Tbl_NhaXuatBan on s.NhaXuatBan equals nxb.Ma
                        where t.MaTruong == smatruong
                        && ct.MaTruong == smatruong
                        && d.MaTruong == smatruong
                        && n.MaTruong == smatruong
                        && s.MaTruong == smatruong
                        && tg.MaTruong == smatruong
                        && nxb.MaTruong == smatruong
                        && k.MaTruong == smatruong
                        && ct.DaTra == false
                        orderby t.MaXuat
                        select new Helper.MuonSach
                        {
                            SoCaBiet = ct.SoCaBiet,
                            TenSach = s.NhanDe,
                            TacGia = tg.Ten,
                            NamXuatBan = s.NamXuatBan,
                            NhaXuatBan = nxb.Ten,
                            NgayMuon = t.NgayMuon,
                            SoThe = d.SoThe,
                            HoTen = d.HoTen,
                            TenKho = k.Ten
                        };


            var result = query.ToList();

            var data = new List<BangKeAnPham.AnPhamDangMuon>();

            foreach (var item in result)
            {
                data.Add(new BangKeAnPham.AnPhamDangMuon()
                {
                    HoTen = item.HoTen,
                    KhoSach = item.TenKho,
                    NamXuatBan = item.NamXuatBan,
                    NgayMuon = DateTime.Parse(item.NgayMuon.ToString()).ToString("dd/MM/yyyy"),
                    NhanDe = item.TenSach,
                    NhaXuatBan = item.NhaXuatBan,
                    SoCaBiet = item.SoCaBiet,
                    SoThe = item.SoThe,
                    TacGia = item.TacGia
                });
            }

            var ttTruong = db.Tbl_ThongTin.Where(p => p.MaTruong == smatruong).FirstOrDefault();
            Dictionary<string, string> textValues = new Dictionary<string, string>();

            textValues.Add("txtNgay", "Tính đến ngày " + DateTime.Now.ToString("dd/MM/yyyy"));
            textValues.Add("txtTenTruong", ttTruong.TenTruong);
            textValues.Add("txtTenPhong", ttTruong.DonViCapTren);
            textValues.Add("txtThuThu", ttTruong.ThuThu);
            Dictionary<string, Dictionary<string, string>> values = new Dictionary<string, Dictionary<string, string>>();
            values.Add("Section2", textValues);
            return GetReportStream(ReportPath.RptBangKeAnPhamDangMuon, data, values);
        }
        #endregion


        #region RptBangKeAnPhamHienCo
        public Stream RptBangKeAnPhamHienCo(string smatruong)
        {
            var query = from t in db.Tbl_Sach
                        join d in db.Tbl_SoCaBiet on t.MaSach equals d.MaSach
                        join b in db.Tbl_BienMuc on t.MaSach equals b.MaSach
                        join k in db.Tbl_KhoSach on t.MaLoai equals k.Ma
                        join tg in db.Tbl_TacGia on t.TacGia equals tg.Ma
                        join nxb in db.Tbl_NhaXuatBan on t.NhaXuatBan equals nxb.Ma
                        where t.MaTruong == smatruong
                        && d.MaTruong == smatruong
                        && b.MaTruong == smatruong
                        && k.MaTruong == smatruong
                        && tg.MaTruong == smatruong
                        && tg.MaTruong == smatruong
                        select new Helper.SearchThanhLy
                        {
                            MaSach = t.MaSach,
                            TenSach = t.NhanDe,
                            SoCaBiet = d.STT,
                            TrangThai = d.TrangThai,
                            GiaTien = b.Gia,
                            TacGia = tg.Ten,
                            TenKho = k.Ten,
                            NamXuatBan = t.NamXuatBan,
                            NhaXuatBan = nxb.Ten
                        };
            var result = query.ToList();

            var data = new List<BangKeAnPham.AnPhamHienCo>();

            foreach (var item in result)
            {
                data.Add(new BangKeAnPham.AnPhamHienCo()
                {
                    KhoSach = item.TenKho,
                    NamXuatBan = item.NamXuatBan,
                    NhanDe = item.TenSach,
                    NhaXuatBan = item.NhaXuatBan,
                    SoCaBiet = item.SoCaBiet,
                    TacGia = item.TacGia

                });
            }
            var ttTruong = db.Tbl_ThongTin.Where(p => p.MaTruong == smatruong).FirstOrDefault();
            Dictionary<string, string> textValues = new Dictionary<string, string>();

            textValues.Add("txtNgay", "Tính đến ngày " + DateTime.Now.ToString("dd/MM/yyyy"));
            textValues.Add("txtTenTruong", ttTruong.TenTruong);
            textValues.Add("txtTenPhong", ttTruong.DonViCapTren);
            textValues.Add("txtThuThu", ttTruong.ThuThu);
            Dictionary<string, Dictionary<string, string>> values = new Dictionary<string, Dictionary<string, string>>();
            values.Add("Section2", textValues);
            return GetReportStream(ReportPath.RptBangKeAnPhamHienCo, data, values);
        }
        #endregion


        #region SoDangKyCaBiet Phan1
        public Stream RptSoDangKyCaBietPhan1(string matruong)
        {
            var paramatruong = new SqlParameter("@MaTruong", matruong);
            var result = db.Database.SqlQuery<Helper.BaoCaoCaBiet>("SoCaBiet_P1_01 @MaTruong", paramatruong).ToList();

            string tgts = "";
            int i = 1;
            var data = new List<SoDangKyCaBiet.SoCaBiet>();
            foreach (var item in result)
            {
                SoDangKyCaBiet.SoCaBiet soCaBiet = new SoDangKyCaBiet.SoCaBiet();
                if (tgts != item.TGTS)
                {
                    soCaBiet.NgayVaoSo = DateTime.Parse(item.NgayVaoSo.ToString()).ToString("dd/MM");
                    soCaBiet.TTTS = i.ToString();
                    soCaBiet.TGTS = item.TGTS;
                    soCaBiet.NXB = item.NXB;
                    soCaBiet.NoiXuatBan = item.NoiXuatBan;
                    soCaBiet.NamXuatBan = item.NamXuatBan;
                    soCaBiet.MonLoai = item.MonLoai;
                    i += 1;
                    tgts = item.TGTS;
                }
                else
                {
                    soCaBiet.NgayVaoSo = "";
                    soCaBiet.TTTS = "";
                    soCaBiet.TGTS = "";
                    soCaBiet.NXB = "--";
                    soCaBiet.NoiXuatBan = "--";
                    soCaBiet.NamXuatBan = "--";
                    soCaBiet.MonLoai = "--";
                }

                //phat ko hoac mua
                soCaBiet.Nguon = (int)item.Nguon;
                if ((item.Nguon.ToString() == "3") && (item.Nguon.ToString() == "5") && (item.Nguon.ToString() == "6"))
                {
                    soCaBiet.PhatKhong = (double)item.DonGia;
                }
                else
                {
                    soCaBiet.Mua = (double)item.DonGia;
                }

                soCaBiet.SoVaoSoTQ = item.SoVaoSoTQ;
                soCaBiet.SoBienXB = item.SoBienXB;

                string nam1 = (DateTime.Now.Year - 4).ToString();
                string nam2 = (DateTime.Now.Year - 3).ToString();
                string nam3 = (DateTime.Now.Year - 2).ToString();
                string nam4 = (DateTime.Now.Year - 1).ToString();
                string nam5 = (DateTime.Now.Year).ToString();

                soCaBiet.Nam1 = (DateTime.Now.Year - 4).ToString();
                soCaBiet.Nam2 = (DateTime.Now.Year - 3).ToString();
                soCaBiet.Nam3 = (DateTime.Now.Year - 2).ToString();
                soCaBiet.Nam4 = (DateTime.Now.Year - 1).ToString();
                soCaBiet.Nam5 = (DateTime.Now.Year).ToString();

                soCaBiet.NamKK = item.NamKK;
                if (item.NamKK == nam1)
                {
                    soCaBiet.Nam1VL = "X";
                }
                else if (item.NamKK == nam2)
                {
                    soCaBiet.Nam2VL = "X";
                }
                else if (item.NamKK == nam3)
                {
                    soCaBiet.Nam3VL = "X";
                }
                else if (item.NamKK == nam4)
                {
                    soCaBiet.Nam4VL = "X";
                }
                else if (item.NamKK == nam5)
                {
                    soCaBiet.Nam5VL = "X";
                }

                data.Add(soCaBiet);

            }


            return GetReportStream(ReportPath.RptSoDangKyCaBietPhan1, data);
        }
        #endregion

        #region RptSoDangKyCaBietToXacNhan
        public Stream RptSoDangKyCaBietToXacNhan(string matruong)
        {
            var paramatruong = new SqlParameter("@MaTruong", matruong);
            var result = db.Database.SqlQuery<Helper.BaoCaoCaBiet>("SoCaBiet_P1_01 @MaTruong", paramatruong).ToList();


            var ttTruong = db.Tbl_ThongTin.Where(p => p.MaTruong == matruong).FirstOrDefault();
            Dictionary<string, string> textValues = new Dictionary<string, string>();
            
            if(result.Count() > 0)
            {
                textValues.Add("txtbatdau", result[0].STT);
                textValues.Add("txtkethuc", result[result.Count() - 1].STT);
            }
           
            textValues.Add("txttentruong", ttTruong.TenTruong);
            Dictionary<string, Dictionary<string, string>> values = new Dictionary<string, Dictionary<string, string>>();
            values.Add("Section2", textValues);


            return GetReportStream(ReportPath.RptSoDangKyCaBietToXacNhan, null, values);
        }
        #endregion

        #region RptSoDangKyCaBietBiaSo
        public Stream RptSoDangKyCaBietBiaSo(string matruong)
        {
            var ttTruong = db.Tbl_ThongTin.Where(p => p.MaTruong == matruong).FirstOrDefault();
            Dictionary<string, string> textValues = new Dictionary<string, string>();

            textValues.Add("txttentruong", ttTruong.TenTruong);
            Dictionary<string, Dictionary<string, string>> values = new Dictionary<string, Dictionary<string, string>>();
            values.Add("Section2", textValues);


            return GetReportStream(ReportPath.RptSoDangKyCaBietBiaSo, null, values);
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
        public static string RptBienBanKiemKe = "~/Report/KiemKe/rptBienBanKiemKe.rpt";

        public static string RptPhieuNhapKho = "~/Report/LuuThong/rptPhieuNhapKho.rpt";

        public static string RptBBMuonSach = "~/Report/rptBBMuonSach.rpt";
        public static string RptBBThanhLy = "~/Report/rptBBThanhLy.rpt";

        public static string RptBaoCaoTon = "~/Report/rptBaoCaoTon.rpt";
        public static string RptBiaSo = "~/Report/SoGiaoVien/rptBiaSo.rpt";
        public static string RptSoMuonSach = "~/Report/SoGiaoVien/rptSoMuonSach.rpt";


        //BangKeAnPham
        public static string RptAnPhamMuonNhieuNhat = "~/Report/BangKeAnPham/rptAnPhamMuonNhieuNhat.rpt";
        public static string RptBangKeAnPhamDangMuon = "~/Report/BangKeAnPham/rptBangKeAnPhamDangMuon.rpt";
        public static string RptBangKeAnPhamHienCo = "~/Report/BangKeAnPham/rptBangKeAnPhamHienCo.rpt";

        //SoDangKyCaBiet
        public static string RptSoDangKyCaBietPhan1 = "~/Report/SoDangKyCaBiet/rptPhan1.rpt";
        public static string RptSoDangKyCaBietToXacNhan = "~/Report/SoDangKyCaBiet/rptToXacNhan.rpt";
        public static string RptSoDangKyCaBietBiaSo = "~/Report/SoDangKyCaBiet/rptBiaSo.rpt";

    }


}