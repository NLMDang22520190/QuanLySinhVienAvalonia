﻿using System;
using System.Collections.Generic;

namespace QuanLySinhVien.Models;

public partial class QuiDinh
{
    public string MaQuiDinh { get; set; } = null!;

    public string? TenQuiDinh { get; set; }

    public int? GiaTri { get; set; }
}
