using System;
using System.Collections.Generic;

namespace QuanLySinhVien.Models;

public partial class BaoCaoHocKy
{
    public string MaBaoCaoHocKy { get; set; } = null!;

    public string? MaLop { get; set; }

    public string? MaHocKy { get; set; }

    public string? MaNienKhoa { get; set; }

    public int? SoLuongDat { get; set; }

    public double? TiLe { get; set; }

    public virtual HocKy? MaHocKyNavigation { get; set; }

    public virtual Lop? MaLopNavigation { get; set; }

    public virtual NienKhoa? MaNienKhoaNavigation { get; set; }
}
