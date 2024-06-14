using System;
using System.Collections.Generic;

namespace QuanLySinhVien.Models;

public partial class BaoCaoMon
{

    public string TenLop => MaLopNavigation?.TenLop ?? string.Empty;

    public string SiSo => MaLopNavigation?.SiSo.ToString() ?? string.Empty;

    public string MaBaoCaoMon { get; set; } = null!;

    public string? MaLop { get; set; }

    public string? MaMon { get; set; }

    public string? MaHocKy { get; set; }

    public string? MaNienKhoa { get; set; }

    public int? SoLuongDat { get; set; }

    public double? TiLe { get; set; }

    public virtual HocKy? MaHocKyNavigation { get; set; }

    public virtual Lop? MaLopNavigation { get; set; }

    public virtual MonHoc? MaMonNavigation { get; set; }

    public virtual NienKhoa? MaNienKhoaNavigation { get; set; }

    public float TiLeFormatted => TiLe.HasValue ? (float)(TiLe.Value * 100) : 0;

    public string TiLeDisplay => TiLe.HasValue ? (TiLe.Value * 100).ToString("0.##") + "%" : "0%";
}
