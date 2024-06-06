﻿using System;
using System.Collections.Generic;

namespace QuanLySinhVien.Models;

public partial class HeThongDiem
{
    public int Stt { get; set; }

    public string? MaDiem { get; set; }

    public string? MaNienKhoa { get; set; }

    public string? MaHocKy { get; set; }

    public string? MaLop { get; set; }

    public string? MaMon { get; set; }

    public string? MaHocSinh { get; set; }

    public decimal? Diem15Phut { get; set; }

    public decimal? Diem1Tiet { get; set; }

    public decimal? DiemTb { get; set; }

    public bool? XepLoai { get; set; }

    public bool? TrangThai { get; set; }
}