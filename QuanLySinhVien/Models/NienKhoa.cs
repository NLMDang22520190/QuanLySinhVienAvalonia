using System;
using System.Collections.Generic;

namespace QuanLySinhVien.Models;

public partial class NienKhoa
{
    public string MaNienKhoa { get; set; } = null!;

    public string? TenNienKhoa { get; set; }

    public virtual ICollection<PhanCongGiangDay> PhanCongGiangDays { get; set; } = new List<PhanCongGiangDay>();

    public virtual ICollection<ThanhTich> ThanhTiches { get; set; } = new List<ThanhTich>();
}
