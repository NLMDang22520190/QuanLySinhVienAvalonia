using System;
using System.Collections.Generic;

namespace QuanLySinhVien.Models;

public partial class BaoCaoHocKy
{
    public string MaBaoCaoHocKy { get; set; } = null!;

    public string? MaLop { get; set; }

    public string? MaHocKy { get; set; }

    public string? MaNienKhoa { get; set; }
}
