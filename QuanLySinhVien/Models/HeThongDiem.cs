using System;
using System.Collections.Generic;

namespace QuanLySinhVien.Models;

public partial class HeThongDiem
{
    public int Stt { get; set; }

    public string? MaDiem { get; set; }

    public string? MaNienKhoa { get; set; }

    public string? MaHocKy { get; set; }

    public string? MaLop { get; set; }

    public string? MaMon { get; set; }

    public string? MaHocSinh { get; set; }

    public decimal? Diem15Phut { get; set; }

    public decimal? Diem1Tiet { get; set; }

    public decimal? DiemTb { get; set; }

    public bool? XepLoai { get; set; }

    public bool? TrangThai { get; set; }

    public virtual HocKy? MaHocKyNavigation { get; set; }

    public virtual HocSinh? MaHocSinhNavigation { get; set; }

    public virtual Lop? MaLopNavigation { get; set; }

    public virtual MonHoc? MaMonNavigation { get; set; }

    public virtual NienKhoa? MaNienKhoaNavigation { get; set; }

    public string HoTen => MaHocSinhNavigation?.TenHocSinh ?? string.Empty;

    public string TrangThaiFormatted => TrangThai.HasValue ? (TrangThai.Value ? "Đã chốt" : "Chưa chốt") : string.Empty;

    // Thuộc tính tính toán để hiển thị xếp loại dưới dạng chuỗi
    public string XepLoaiFormatted => XepLoai.HasValue ? (XepLoai.Value ? "Đạt" : "Chưa đạt") : string.Empty;
}

