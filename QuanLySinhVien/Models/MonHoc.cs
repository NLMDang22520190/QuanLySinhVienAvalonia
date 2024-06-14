using System;
using System.Collections.Generic;

namespace QuanLySinhVien.Models;

public partial class MonHoc
{
    public string MaMon { get; set; } = null!;

    public string? TenMon { get; set; }


    public virtual ICollection<BaoCaoMon> BaoCaoMons { get; set; } = new List<BaoCaoMon>();

    public virtual ICollection<Diem> Diems { get; set; } = new List<Diem>();

    public virtual ICollection<HeThongDiem> HeThongDiems { get; set; } = new List<HeThongDiem>();

    public virtual ICollection<PhanCongGiangDay> PhanCongGiangDays { get; set; } = new List<PhanCongGiangDay>();
}
