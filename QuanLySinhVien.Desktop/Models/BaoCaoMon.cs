using System;
using System.Collections.Generic;

namespace QuanLySinhVien.Desktop.Models;

public partial class BaoCaoMon
{
    public string MaBaoCaoMon { get; set; } = null!;

    public string? MaLop { get; set; }

    public string? MaMon { get; set; }

    public string? MaHocKy { get; set; }

    public string? MaNienKhoa { get; set; }

    public virtual HocKy? MaHocKyNavigation { get; set; }

    public virtual Lop? MaLopNavigation { get; set; }

    public virtual MonHoc? MaMonNavigation { get; set; }

    public virtual NienKhoa? MaNienKhoaNavigation { get; set; }
}
