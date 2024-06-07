using System;
using System.Collections.Generic;

namespace QuanLySinhVien.Models;

public partial class Diem
{
    public string MaDiem { get; set; } = null!;

    public string? MaNienKhoa { get; set; }

    public string? MaHocKy { get; set; }

    public string? MaLop { get; set; }

    public string? MaMon { get; set; }

    public string? MaHocSinh { get; set; }

    public string? MaLoaiDiem { get; set; }

    public double? Diem1 { get; set; }

    public virtual HocKy? MaHocKyNavigation { get; set; }

    public virtual HocSinh? MaHocSinhNavigation { get; set; }

    public virtual LoaiDiem? MaLoaiDiemNavigation { get; set; }

    public virtual Lop? MaLopNavigation { get; set; }

    public virtual MonHoc? MaMonNavigation { get; set; }

    public virtual NienKhoa? MaNienKhoaNavigation { get; set; }
}
