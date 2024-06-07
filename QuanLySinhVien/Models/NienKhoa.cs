using System;
using System.Collections.Generic;

namespace QuanLySinhVien.Models;

public partial class NienKhoa
{
    public string MaNienKhoa { get; set; } = null!;

    public string? TenNienKhoa { get; set; }

    public virtual ICollection<BaoCaoHocKy> BaoCaoHocKies { get; set; } = new List<BaoCaoHocKy>();

    public virtual ICollection<BaoCaoMon> BaoCaoMons { get; set; } = new List<BaoCaoMon>();

    public virtual ICollection<Diem> Diems { get; set; } = new List<Diem>();

    public virtual ICollection<HeThongDiem> HeThongDiems { get; set; } = new List<HeThongDiem>();

    public virtual ICollection<Lop> Lops { get; set; } = new List<Lop>();

    public virtual ICollection<PhanCongGiangDay> PhanCongGiangDays { get; set; } = new List<PhanCongGiangDay>();

    public virtual ICollection<ThanhTich> ThanhTiches { get; set; } = new List<ThanhTich>();
}
