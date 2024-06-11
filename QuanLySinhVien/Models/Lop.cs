using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace QuanLySinhVien.Models;

public partial class Lop : INotifyPropertyChanged
{
    public string MaLop { get; set; } = null!;

    public string? TenLop { get; set; }

    private int? siSo;
    public int? SiSo 
    {
        get => siSo;
        set
        {
            if (siSo != value)
            {
                siSo = value;
                OnPropertyChanged(nameof(SiSo));
            }
        }
    }

    public string? MaNienKhoa { get; set; }

    public string? MaKhoi { get; set; }

    public string? TenGvcn { get; set; }

    public virtual ICollection<BaoCaoHocKy> BaoCaoHocKies { get; set; } = new List<BaoCaoHocKy>();

    public virtual ICollection<BaoCaoMon> BaoCaoMons { get; set; } = new List<BaoCaoMon>();

    public virtual ICollection<Diem> Diems { get; set; } = new List<Diem>();

    public virtual ICollection<HeThongDiem> HeThongDiems { get; set; } = new List<HeThongDiem>();

    public virtual ICollection<HocSinh> HocSinhs { get; set; } = new List<HocSinh>();

    public virtual Khoi? MaKhoiNavigation { get; set; }

    public virtual NienKhoa? MaNienKhoaNavigation { get; set; }

    public virtual ICollection<PhanCongGiangDay> PhanCongGiangDays { get; set; } = new List<PhanCongGiangDay>();

    public virtual GiaoVien? TenGvcnNavigation { get; set; }

    public virtual ICollection<ThanhTich> ThanhTiches { get; set; } = new List<ThanhTich>();

    public event PropertyChangedEventHandler PropertyChanged;

    protected void OnPropertyChanged(string propertyName)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}
