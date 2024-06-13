using System;
using System.Collections.Generic;

namespace QuanLySinhVien.Models;

public partial class ThanhTich
{
    public string MaThanhTich { get; set; } = null!;

    public string? MaNienKhoa { get; set; }

    public string? MaHocKy { get; set; }

    public string? MaLop { get; set; }

    public string? MaHocSinh { get; set; }

    public bool? XepLoai { get; set; }

    public string? NhanXet { get; set; }

    public double? Tbhk { get; set; }

    public virtual HocKy? MaHocKyNavigation { get; set; }

    public virtual HocSinh? MaHocSinhNavigation { get; set; }

    public virtual Lop? MaLopNavigation { get; set; }

    public virtual NienKhoa? MaNienKhoaNavigation { get; set; }
    public string HoTen => MaHocSinhNavigation?.TenHocSinh ?? string.Empty;

    public string TenLop => MaLopNavigation?.TenLop ?? string.Empty;



}
