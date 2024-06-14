using System;
using System.Collections.Generic;

namespace QuanLySinhVien.Models;

public partial class PhanCongGiangDay
{
    public string MaPhanCong { get; set; } = null!;

    public string? MaNienKhoa { get; set; }

    public string? MaHocKy { get; set; }

    public string? MaLop { get; set; }

    public string? MaMon { get; set; }

    public string? MaGiaoVienPhuTrach { get; set; }

    public string? TenGiaoVien { get; set; }

    public virtual GiaoVien? MaGiaoVienPhuTrachNavigation { get; set; }

    public virtual HocKy? MaHocKyNavigation { get; set; }

    public virtual Lop? MaLopNavigation { get; set; }

    public virtual MonHoc? MaMonNavigation { get; set; }

    public virtual NienKhoa? MaNienKhoaNavigation { get; set; }

    public string TenMon => MaMonNavigation?.TenMon ?? string.Empty;

    public string TenLop => MaLopNavigation?.TenLop ?? string.Empty;

    public string TenNienKhoa => MaNienKhoaNavigation?.TenNienKhoa ?? string.Empty;
    public string TenHocKy => MaHocKyNavigation?.TenHocKy ?? string.Empty;
}
