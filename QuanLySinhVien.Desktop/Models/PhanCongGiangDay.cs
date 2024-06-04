using System;
using System.Collections.Generic;

namespace QuanLySinhVien.Desktop.Models;

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
}
