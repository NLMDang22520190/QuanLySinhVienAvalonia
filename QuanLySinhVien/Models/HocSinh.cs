using System;
using System.Collections.Generic;
using System.Globalization;

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

    public string NgaySinhFormatted => NgaySinh?.ToString("dd/MM/yyyy", CultureInfo.InvariantCulture);
    public string GioiTinhFormatted => GioiTinh.HasValue ? (GioiTinh.Value ? "Nam" : "Nữ") : null;

    public virtual ICollection<ThanhTich> ThanhTiches { get; set; } = new List<ThanhTich>();
}
