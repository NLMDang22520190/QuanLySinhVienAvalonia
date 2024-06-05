using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using System.Configuration;

namespace QuanLySinhVien.Models;

public partial class QlhsContext : DbContext
{
    public QlhsContext()
    {
    }

    public QlhsContext(DbContextOptions<QlhsContext> options)
        : base(options)
    {
    }

    public virtual DbSet<BaoCaoHocKy> BaoCaoHocKies { get; set; }

    public virtual DbSet<BaoCaoMon> BaoCaoMons { get; set; }

    public virtual DbSet<Diem> Diems { get; set; }

    public virtual DbSet<GiamHieu> GiamHieus { get; set; }

    public virtual DbSet<GiaoVien> GiaoViens { get; set; }

    public virtual DbSet<HeThongDiem> HeThongDiems { get; set; }

    public virtual DbSet<HocKy> HocKies { get; set; }

    public virtual DbSet<HocSinh> HocSinhs { get; set; }

    public virtual DbSet<Khoi> Khois { get; set; }

    public virtual DbSet<LoaiDiem> LoaiDiems { get; set; }

    public virtual DbSet<Lop> Lops { get; set; }

    public virtual DbSet<MonHoc> MonHocs { get; set; }

    public virtual DbSet<NienKhoa> NienKhoas { get; set; }

    public virtual DbSet<PhanCongGiangDay> PhanCongGiangDays { get; set; }

    public virtual DbSet<QuiDinh> QuiDinhs { get; set; }

    public virtual DbSet<ThanhTich> ThanhTiches { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        => optionsBuilder.UseSqlServer("Server = tcp:se104.database.windows.net, 1433; Initial Catalog = QLHS; Persist Security Info=False;User ID = CloudSAee21f269; Password=Quanlyhocsinh123@; MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout = 30;");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<BaoCaoHocKy>(entity =>
        {
            entity.HasKey(e => e.MaBaoCaoHocKy).HasName("PK__BaoCaoHo__E48B4E44C18F63E6");

            entity.ToTable("BaoCaoHocKy");

            entity.Property(e => e.MaBaoCaoHocKy).HasMaxLength(25);
            entity.Property(e => e.MaHocKy).HasMaxLength(25);
            entity.Property(e => e.MaLop).HasMaxLength(25);
            entity.Property(e => e.MaNienKhoa).HasMaxLength(25);
        });

        modelBuilder.Entity<BaoCaoMon>(entity =>
        {
            entity.HasKey(e => e.MaBaoCaoMon).HasName("PK__BaoCaoMo__CB2A82D2636F235B");

            entity.ToTable("BaoCaoMon");

            entity.Property(e => e.MaBaoCaoMon).HasMaxLength(25);
            entity.Property(e => e.MaHocKy).HasMaxLength(25);
            entity.Property(e => e.MaLop).HasMaxLength(25);
            entity.Property(e => e.MaMon).HasMaxLength(25);
            entity.Property(e => e.MaNienKhoa).HasMaxLength(25);
        });

        modelBuilder.Entity<Diem>(entity =>
        {
            entity.HasKey(e => e.MaDiem).HasName("PK__Diem__33326025BB7D52B2");

            entity.ToTable("Diem");

            entity.Property(e => e.MaDiem).HasMaxLength(25);
            entity.Property(e => e.Diem1).HasColumnName("Diem");
            entity.Property(e => e.MaHocKy).HasMaxLength(25);
            entity.Property(e => e.MaHocSinh).HasMaxLength(25);
            entity.Property(e => e.MaLoaiDiem).HasMaxLength(25);
            entity.Property(e => e.MaLop).HasMaxLength(25);
            entity.Property(e => e.MaMon).HasMaxLength(25);
            entity.Property(e => e.MaNienKhoa).HasMaxLength(25);
        });

        modelBuilder.Entity<GiamHieu>(entity =>
        {
            entity.HasKey(e => e.MaTruong).HasName("PK__GiamHieu__5ECEF88AB51ED435");

            entity.ToTable("GiamHieu");

            entity.Property(e => e.MaTruong).ValueGeneratedNever();
            entity.Property(e => e.TenTruong).HasMaxLength(100);
        });

        modelBuilder.Entity<GiaoVien>(entity =>
        {
            entity.HasKey(e => e.MaGiaoVien).HasName("PK__GiaoVien__8D374F50BBC1408E");

            entity.ToTable("GiaoVien");

            entity.Property(e => e.MaGiaoVien).HasMaxLength(25);
            entity.Property(e => e.DiaChi).HasMaxLength(200);
            entity.Property(e => e.Email).HasMaxLength(100);
            entity.Property(e => e.NgaySinh).HasColumnType("date");
            entity.Property(e => e.TenGiaoVien).HasMaxLength(100);
        });

        modelBuilder.Entity<HeThongDiem>(entity =>
        {
            entity.HasKey(e => e.Stt).HasName("PK__HeThongD__CA1EB6908FB042E0");

            entity.ToTable("HeThongDiem");

            entity.Property(e => e.Stt)
                .ValueGeneratedNever()
                .HasColumnName("STT");
            entity.Property(e => e.Diem15Phut).HasColumnType("decimal(5, 2)");
            entity.Property(e => e.Diem1Tiet).HasColumnType("decimal(5, 2)");
            entity.Property(e => e.DiemTb)
                .HasColumnType("decimal(5, 2)")
                .HasColumnName("DiemTB");
            entity.Property(e => e.MaDiem).HasMaxLength(25);
            entity.Property(e => e.MaHocKy).HasMaxLength(25);
            entity.Property(e => e.MaHocSinh).HasMaxLength(25);
            entity.Property(e => e.MaLop).HasMaxLength(25);
            entity.Property(e => e.MaMon).HasMaxLength(25);
            entity.Property(e => e.MaNienKhoa).HasMaxLength(25);
        });

        modelBuilder.Entity<HocKy>(entity =>
        {
            entity.HasKey(e => e.MaHocKy).HasName("PK__HocKy__1EB551104A9E0296");

            entity.ToTable("HocKy");

            entity.Property(e => e.MaHocKy).HasMaxLength(25);
            entity.Property(e => e.TenHocKy).HasMaxLength(50);
        });

        modelBuilder.Entity<HocSinh>(entity =>
        {
            entity.HasKey(e => e.MaHocSinh).HasName("PK__HocSinh__90BD01E09C95A169");

            entity.ToTable("HocSinh");

            entity.Property(e => e.MaHocSinh).HasMaxLength(25);
            entity.Property(e => e.DiaChi).HasMaxLength(200);
            entity.Property(e => e.Email).HasMaxLength(100);
            entity.Property(e => e.MaLop).HasMaxLength(25);
            entity.Property(e => e.NgaySinh).HasColumnType("date");
            entity.Property(e => e.TenHocSinh).HasMaxLength(100);
        });

        modelBuilder.Entity<Khoi>(entity =>
        {
            entity.HasKey(e => e.MaKhoi).HasName("PK__Khoi__6539040D249345F9");

            entity.ToTable("Khoi");

            entity.Property(e => e.MaKhoi).HasMaxLength(25);
            entity.Property(e => e.TenKhoi).HasMaxLength(50);
        });

        modelBuilder.Entity<LoaiDiem>(entity =>
        {
            entity.HasKey(e => e.MaLoaiDiem).HasName("PK__LoaiDiem__77BE9E4AC373141C");

            entity.ToTable("LoaiDiem");

            entity.Property(e => e.MaLoaiDiem).HasMaxLength(25);
            entity.Property(e => e.TenLoai).HasMaxLength(20);
        });

        modelBuilder.Entity<Lop>(entity =>
        {
            entity.HasKey(e => e.MaLop).HasName("PK__Lop__3B98D27300597F43");

            entity.ToTable("Lop");

            entity.Property(e => e.MaLop).HasMaxLength(25);
            entity.Property(e => e.MaKhoi).HasMaxLength(25);
            entity.Property(e => e.MaNienKhoa).HasMaxLength(25);
            entity.Property(e => e.TenGvcn)
                .HasMaxLength(100)
                .HasColumnName("TenGVCN");
            entity.Property(e => e.TenLop).HasMaxLength(50);

            entity.HasOne(d => d.MaKhoiNavigation).WithMany(p => p.Lops)
                .HasForeignKey(d => d.MaKhoi)
                .HasConstraintName("FK_Lop_MaKhoi");
        });

        modelBuilder.Entity<MonHoc>(entity =>
        {
            entity.HasKey(e => e.MaMon).HasName("PK__MonHoc__3A5B29A8B294F5A8");

            entity.ToTable("MonHoc");

            entity.Property(e => e.MaMon).HasMaxLength(25);
            entity.Property(e => e.TenMon).HasMaxLength(100);
        });

        modelBuilder.Entity<NienKhoa>(entity =>
        {
            entity.HasKey(e => e.MaNienKhoa).HasName("PK__NienKhoa__6F040C382E72FDC2");

            entity.ToTable("NienKhoa");

            entity.Property(e => e.MaNienKhoa).HasMaxLength(25);
            entity.Property(e => e.TenNienKhoa).HasMaxLength(50);
        });

        modelBuilder.Entity<PhanCongGiangDay>(entity =>
        {
            entity.HasKey(e => e.MaPhanCong).HasName("PK__PhanCong__C279D916D93CB7CB");

            entity.ToTable("PhanCongGiangDay");

            entity.Property(e => e.MaPhanCong).HasMaxLength(25);
            entity.Property(e => e.MaGiaoVienPhuTrach).HasMaxLength(25);
            entity.Property(e => e.MaHocKy).HasMaxLength(25);
            entity.Property(e => e.MaLop).HasMaxLength(25);
            entity.Property(e => e.MaMon).HasMaxLength(25);
            entity.Property(e => e.MaNienKhoa).HasMaxLength(25);
            entity.Property(e => e.TenGiaoVien).HasMaxLength(100);

            entity.HasOne(d => d.MaGiaoVienPhuTrachNavigation).WithMany(p => p.PhanCongGiangDays)
                .HasForeignKey(d => d.MaGiaoVienPhuTrach)
                .HasConstraintName("FK_MaGiaoVienPhuTrach_PC");

            entity.HasOne(d => d.MaHocKyNavigation).WithMany(p => p.PhanCongGiangDays)
                .HasForeignKey(d => d.MaHocKy)
                .HasConstraintName("FK_MaHocKy_PC");

            entity.HasOne(d => d.MaLopNavigation).WithMany(p => p.PhanCongGiangDays)
                .HasForeignKey(d => d.MaLop)
                .HasConstraintName("FK_MaLop_PC");

            entity.HasOne(d => d.MaMonNavigation).WithMany(p => p.PhanCongGiangDays)
                .HasForeignKey(d => d.MaMon)
                .HasConstraintName("FK_MaMon_PC");

            entity.HasOne(d => d.MaNienKhoaNavigation).WithMany(p => p.PhanCongGiangDays)
                .HasForeignKey(d => d.MaNienKhoa)
                .HasConstraintName("FK_MaNienKhoa_PC");
        });

        modelBuilder.Entity<QuiDinh>(entity =>
        {
            entity.HasKey(e => e.MaQuiDinh).HasName("PK__QuiDinh__7EDE7167B742382D");

            entity.ToTable("QuiDinh");

            entity.Property(e => e.MaQuiDinh).HasMaxLength(25);
            entity.Property(e => e.TenQuiDinh).HasMaxLength(100);
        });

        modelBuilder.Entity<ThanhTich>(entity =>
        {
            entity.HasKey(e => e.MaThanhTich).HasName("PK__ThanhTic__C65ACB79C14EE6C3");

            entity.ToTable("ThanhTich");

            entity.Property(e => e.MaThanhTich).HasMaxLength(25);
            entity.Property(e => e.MaHocKy).HasMaxLength(25);
            entity.Property(e => e.MaHocSinh).HasMaxLength(25);
            entity.Property(e => e.MaLop).HasMaxLength(25);
            entity.Property(e => e.MaNienKhoa).HasMaxLength(25);
            entity.Property(e => e.Tbhk).HasColumnName("TBHK");

            entity.HasOne(d => d.MaHocKyNavigation).WithMany(p => p.ThanhTiches)
                .HasForeignKey(d => d.MaHocKy)
                .HasConstraintName("fk_MaHocKy");

            entity.HasOne(d => d.MaHocSinhNavigation).WithMany(p => p.ThanhTiches)
                .HasForeignKey(d => d.MaHocSinh)
                .HasConstraintName("fk_MaHocSinh");

            entity.HasOne(d => d.MaLopNavigation).WithMany(p => p.ThanhTiches)
                .HasForeignKey(d => d.MaLop)
                .HasConstraintName("fk_MaLop");

            entity.HasOne(d => d.MaNienKhoaNavigation).WithMany(p => p.ThanhTiches)
                .HasForeignKey(d => d.MaNienKhoa)
                .HasConstraintName("fk_MaNienKhoa");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}