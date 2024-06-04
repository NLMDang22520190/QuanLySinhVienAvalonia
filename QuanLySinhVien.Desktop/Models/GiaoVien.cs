using System;
using System.Collections.Generic;

namespace QuanLySinhVien.Desktop.Models;

public partial class GiaoVien
{
    public string MaGiaoVien { get; set; } = null!;

    public string TenGiaoVien { get; set; } = null!;

    public DateTime? NgaySinh { get; set; }

    public bool? GioiTinh { get; set; }

    public string? DiaChi { get; set; }

    public string? Email { get; set; }

    public byte[]? Avatar { get; set; }

    public virtual ICollection<Lop> Lops { get; set; } = new List<Lop>();

    public virtual ICollection<PhanCongGiangDay> PhanCongGiangDays { get; set; } = new List<PhanCongGiangDay>();
}
