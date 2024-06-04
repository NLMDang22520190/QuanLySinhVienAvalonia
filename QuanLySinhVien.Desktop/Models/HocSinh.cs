using System;
using System.Collections.Generic;

namespace QuanLySinhVien.Desktop.Models;

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
}
