using System;
using System.Collections.Generic;

namespace QuanLySinhVien.Models;

public partial class Lop
{
    public string MaLop { get; set; } = null!;

    public string? TenLop { get; set; }

    public int? SiSo { get; set; }

    public string? MaNienKhoa { get; set; }

    public string? MaKhoi { get; set; }

    public string? TenGvcn { get; set; }

    public virtual Khoi? MaKhoiNavigation { get; set; }

    public virtual ICollection<PhanCongGiangDay> PhanCongGiangDays { get; set; } = new List<PhanCongGiangDay>();

    public virtual ICollection<ThanhTich> ThanhTiches { get; set; } = new List<ThanhTich>();
}
