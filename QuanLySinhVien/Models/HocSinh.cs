using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;

namespace QuanLySinhVien.Models;

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

    public string NgaySinhFormatted => NgaySinh?.ToString("dd/MM/yyyy", CultureInfo.InvariantCulture);
    public string GioiTinhFormatted => GioiTinh.HasValue ? (GioiTinh.Value ? "Nam" : "Nữ") : null;

    public float DiemTBHK1 => CalculateDiemTrungBinh("HK1");
    public float DiemTBHK2 => CalculateDiemTrungBinh("HK2");

    private float CalculateDiemTrungBinh(string hocKy)
    {
        var diemTBs = HeThongDiems
            .Where(h => h.MaHocKy == hocKy)
            .Select(h => h.DiemTb)
            .ToList();

        if (diemTBs.Count == 0)
        {
            return 0;
        }

        return (float)diemTBs.Average();
    }

}
