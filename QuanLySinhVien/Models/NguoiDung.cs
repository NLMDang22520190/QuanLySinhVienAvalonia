using System;
using System.Collections.Generic;

namespace QuanLySinhVien.Models;

public partial class NguoiDung
{
    public string MaNguoiDung { get; set; } = null!;

    public string? MaGiaoVien { get; set; }

    public string TenDangNhap { get; set; } = null!;

    public string MatKhau { get; set; } = null!;

    public bool ChucNang { get; set; }

    public virtual GiaoVien? MaGiaoVienNavigation { get; set; }
}
