using System;
using System.Collections.Generic;

namespace QuanLySinhVien.Models;

public partial class Khoi
{
    public string MaKhoi { get; set; } = null!;

    public string? TenKhoi { get; set; }

    public virtual ICollection<Lop> Lops { get; set; } = new List<Lop>();
}
