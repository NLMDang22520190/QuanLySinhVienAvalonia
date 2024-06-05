using System;
using System.Collections.Generic;

namespace QuanLySinhVien.Models;

public partial class GiaoVien
{
    public string MaGiaoVien { get; set; } = null!;

    public string? TenGiaoVien { get; set; }

    public DateTime? NgaySinh { get; set; }

    public bool? GioiTinh { get; set; }

    public string? DiaChi { get; set; }

    public string? Email { get; set; }

    public byte[]? Avatar { get; set; }

    public virtual ICollection<PhanCongGiangDay> PhanCongGiangDays { get; set; } = new List<PhanCongGiangDay>();
}
