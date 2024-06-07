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

    public virtual ICollection<Diem> Diems { get; set; } = new List<Diem>();

    public virtual ICollection<HeThongDiem> HeThongDiems { get; set; } = new List<HeThongDiem>();

    public virtual Lop? MaLopNavigation { get; set; }

    public virtual ICollection<ThanhTich> ThanhTiches { get; set; } = new List<ThanhTich>();

    public string NgaySinhFormatted => NgaySinh?.ToString("dd/MM/yyyy", CultureInfo.InvariantCulture);
    public string GioiTinhFormatted => GioiTinh.HasValue ? (GioiTinh.Value ? "Nam" : "Nữ") : null;

}
