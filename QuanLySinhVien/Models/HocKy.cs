using System;
using System.Collections.Generic;

namespace QuanLySinhVien.Models;

public partial class HocKy
{
    public string MaHocKy { get; set; } = null!;

    public string? TenHocKy { get; set; }

    public virtual ICollection<PhanCongGiangDay> PhanCongGiangDays { get; set; } = new List<PhanCongGiangDay>();

    public virtual ICollection<ThanhTich> ThanhTiches { get; set; } = new List<ThanhTich>();
}
