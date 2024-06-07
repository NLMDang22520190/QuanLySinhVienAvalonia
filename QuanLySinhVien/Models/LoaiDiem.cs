using System;
using System.Collections.Generic;

namespace QuanLySinhVien.Models;

public partial class LoaiDiem
{
    public string MaLoaiDiem { get; set; } = null!;

    public string? TenLoai { get; set; }

    public int? HeSo { get; set; }

    public virtual ICollection<Diem> Diems { get; set; } = new List<Diem>();
}
