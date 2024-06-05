using System;
using System.Collections.Generic;

namespace QuanLySinhVien.Models;

public partial class HocSinh
{
    public string MaHocSinh { get; set; } = null!;

    public string? TenHocSinh { get; set; }

    public DateTime? NgaySinh { get; set; }

    public bool? GioiTinh { get; set; }

    public string? DiaChi { get; set; }

    public string? Email { get; set; }

    public byte[]? Avatar { get; set; }

    public string? MaLop { get; set; }

    public virtual ICollection<ThanhTich> ThanhTiches { get; set; } = new List<ThanhTich>();
}
