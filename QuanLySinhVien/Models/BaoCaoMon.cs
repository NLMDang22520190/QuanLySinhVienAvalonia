using System;
using System.Collections.Generic;

namespace QuanLySinhVien.Models;

public partial class BaoCaoMon
{
    public string MaBaoCaoMon { get; set; } = null!;

    public string? MaLop { get; set; }

    public string? MaMon { get; set; }

    public string? MaHocKy { get; set; }

    public string? MaNienKhoa { get; set; }
}
