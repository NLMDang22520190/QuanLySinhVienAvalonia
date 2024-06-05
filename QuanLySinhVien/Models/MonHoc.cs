using System;
using System.Collections.Generic;

namespace QuanLySinhVien.Models;

public partial class MonHoc
{
    public string MaMon { get; set; } = null!;

    public string? TenMon { get; set; }

    public virtual ICollection<PhanCongGiangDay> PhanCongGiangDays { get; set; } = new List<PhanCongGiangDay>();
}
