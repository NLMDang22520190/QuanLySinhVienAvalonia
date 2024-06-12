using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

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

    public virtual DbSet<NguoiDung> NguoiDungs { get; set; }

    public virtual DbSet<NienKhoa> NienKhoas { get; set; }

    public virtual DbSet<PhanCongGiangDay> PhanCongGiangDays { get; set; }

    public virtual DbSet<QuiDinh> QuiDinhs { get; set; }

    public virtual DbSet<ThanhTich> ThanhTiches { get; set; }

    public virtual DbSet<VStudentAchievement> VStudentAchievements { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        => optionsBuilder.UseSqlServer("Server = tcp:se104.database.windows.net, 1433; Initial Catalog = QLHS; Persist Security Info=False;User ID = CloudSAee21f269; Password=Quanlyhocsinh123@; MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout = 30;");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<BaoCaoHocKy>(entity =>
        {
            entity.HasKey(e => e.MaBaoCaoHocKy).HasName("PK__BaoCaoHo__E48B4E44AF8959D8");

            entity.ToTable("BaoCaoHocKy");

            entity.Property(e => e.MaBaoCaoHocKy).HasMaxLength(25);
            entity.Property(e => e.MaHocKy).HasMaxLength(25);
            entity.Property(e => e.MaLop).HasMaxLength(25);
            entity.Property(e => e.MaNienKhoa).HasMaxLength(25);

            entity.HasOne(d => d.MaHocKyNavigation).WithMany(p => p.BaoCaoHocKies)
                .HasForeignKey(d => d.MaHocKy)
                .HasConstraintName("FK_BCHK_MaHocKy");

            entity.HasOne(d => d.MaLopNavigation).WithMany(p => p.BaoCaoHocKies)
                .HasForeignKey(d => d.MaLop)
                .HasConstraintName("FK__BCHK_MaLop");

            entity.HasOne(d => d.MaNienKhoaNavigation).WithMany(p => p.BaoCaoHocKies)
                .HasForeignKey(d => d.MaNienKhoa)
                .HasConstraintName("FK_BCHK_MaNienKhoa");
        });

        modelBuilder.Entity<BaoCaoMon>(entity =>
        {
            entity.HasKey(e => e.MaBaoCaoMon).HasName("PK__BaoCaoMo__CB2A82D287FCBAA4");

            entity.ToTable("BaoCaoMon");

            entity.Property(e => e.MaBaoCaoMon).HasMaxLength(25);
            entity.Property(e => e.MaHocKy).HasMaxLength(25);
            entity.Property(e => e.MaLop).HasMaxLength(25);
            entity.Property(e => e.MaMon).HasMaxLength(25);
            entity.Property(e => e.MaNienKhoa).HasMaxLength(25);

            entity.HasOne(d => d.MaHocKyNavigation).WithMany(p => p.BaoCaoMons)
                .HasForeignKey(d => d.MaHocKy)
                .HasConstraintName("FK_BCMon_MaHocKy");

            entity.HasOne(d => d.MaLopNavigation).WithMany(p => p.BaoCaoMons)
                .HasForeignKey(d => d.MaLop)
                .HasConstraintName("FK_BCMon_MaLop");

            entity.HasOne(d => d.MaMonNavigation).WithMany(p => p.BaoCaoMons)
                .HasForeignKey(d => d.MaMon)
                .HasConstraintName("FK_BCMon_MaMon");

            entity.HasOne(d => d.MaNienKhoaNavigation).WithMany(p => p.BaoCaoMons)
                .HasForeignKey(d => d.MaNienKhoa)
                .HasConstraintName("FK_BCMon_MaNienKhoa");
        });

        modelBuilder.Entity<Diem>(entity =>
        {
            entity.HasKey(e => e.MaDiem).HasName("PK__Diem__3332602582122B80");

            entity.ToTable("Diem");

            entity.Property(e => e.MaDiem).HasMaxLength(25);
            entity.Property(e => e.Diem1).HasColumnName("Diem");
            entity.Property(e => e.MaHocKy).HasMaxLength(25);
            entity.Property(e => e.MaHocSinh).HasMaxLength(25);
            entity.Property(e => e.MaLoaiDiem).HasMaxLength(25);
            entity.Property(e => e.MaLop).HasMaxLength(25);
            entity.Property(e => e.MaMon).HasMaxLength(25);
            entity.Property(e => e.MaNienKhoa).HasMaxLength(25);

            entity.HasOne(d => d.MaHocKyNavigation).WithMany(p => p.Diems)
                .HasForeignKey(d => d.MaHocKy)
                .HasConstraintName("FK_HTDIEM_MaHocKy");

            entity.HasOne(d => d.MaHocSinhNavigation).WithMany(p => p.Diems)
                .HasForeignKey(d => d.MaHocSinh)
                .HasConstraintName("FK_HTDiem_MaHocSinh");

            entity.HasOne(d => d.MaLoaiDiemNavigation).WithMany(p => p.Diems)
                .HasForeignKey(d => d.MaLoaiDiem)
                .HasConstraintName("FK_HTDiem_MaLoaiDiem");

            entity.HasOne(d => d.MaLopNavigation).WithMany(p => p.Diems)
                .HasForeignKey(d => d.MaLop)
                .HasConstraintName("FK_HTDiem_MaLop");

            entity.HasOne(d => d.MaMonNavigation).WithMany(p => p.Diems)
                .HasForeignKey(d => d.MaMon)
                .HasConstraintName("FK_HTDiem_MaMon");

            entity.HasOne(d => d.MaNienKhoaNavigation).WithMany(p => p.Diems)
                .HasForeignKey(d => d.MaNienKhoa)
                .HasConstraintName("FK_HTDiem_MaNienKhoa");
        });

        modelBuilder.Entity<GiamHieu>(entity =>
        {
            entity.HasKey(e => e.MaTruong).HasName("PK__GiamHieu__5ECEF88A0EF8369F");

            entity.ToTable("GiamHieu");

            entity.Property(e => e.MaTruong).ValueGeneratedNever();
            entity.Property(e => e.TenTruong).HasMaxLength(100);
        });

        modelBuilder.Entity<GiaoVien>(entity =>
        {
            entity.HasKey(e => e.MaGiaoVien).HasName("PK__GiaoVien__8D374F5035CF4A38");

            entity.ToTable("GiaoVien");

            entity.HasIndex(e => e.TenGiaoVien, "IX_GiaoVien_TenGiaoVien").IsUnique();

            entity.Property(e => e.MaGiaoVien).HasMaxLength(25);
            entity.Property(e => e.DiaChi).HasMaxLength(200);
            entity.Property(e => e.Email).HasMaxLength(100);
            entity.Property(e => e.NgaySinh).HasColumnType("date");
            entity.Property(e => e.TenGiaoVien).HasMaxLength(100);
        });

        modelBuilder.Entity<HeThongDiem>(entity =>
        {
            entity.HasKey(e => e.Stt).HasName("PK__HeThongD__CA1EB690D49EB778");

            entity.ToTable("HeThongDiem", tb =>
                {
                    tb.HasTrigger("trg_AfterHeThongDiemChanges");
                    tb.HasTrigger("trg_UpdateXepLoai");
                });

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

            entity.HasOne(d => d.MaHocKyNavigation).WithMany(p => p.HeThongDiems)
                .HasForeignKey(d => d.MaHocKy)
                .HasConstraintName("FK_Diem_MaHocKy");

            entity.HasOne(d => d.MaHocSinhNavigation).WithMany(p => p.HeThongDiems)
                .HasForeignKey(d => d.MaHocSinh)
                .HasConstraintName("FK_Diem_MaHocSinh");

            entity.HasOne(d => d.MaLopNavigation).WithMany(p => p.HeThongDiems)
                .HasForeignKey(d => d.MaLop)
                .HasConstraintName("FK_Diem_MaLop");

            entity.HasOne(d => d.MaMonNavigation).WithMany(p => p.HeThongDiems)
                .HasForeignKey(d => d.MaMon)
                .HasConstraintName("FK_Diem_MaMon");

            entity.HasOne(d => d.MaNienKhoaNavigation).WithMany(p => p.HeThongDiems)
                .HasForeignKey(d => d.MaNienKhoa)
                .HasConstraintName("FK_Diem_MaNienKhoa");
        });

        modelBuilder.Entity<HocKy>(entity =>
        {
            entity.HasKey(e => e.MaHocKy).HasName("PK__HocKy__1EB55110DC360B6A");

            entity.ToTable("HocKy");

            entity.Property(e => e.MaHocKy).HasMaxLength(25);
            entity.Property(e => e.TenHocKy).HasMaxLength(50);
        });

        modelBuilder.Entity<HocSinh>(entity =>
        {
            entity.HasKey(e => e.MaHocSinh).HasName("PK__HocSinh__90BD01E0FD8615BB");

            entity.ToTable("HocSinh");

            entity.Property(e => e.MaHocSinh).HasMaxLength(25);
            entity.Property(e => e.DiaChi).HasMaxLength(200);
            entity.Property(e => e.Email).HasMaxLength(100);
            entity.Property(e => e.MaLop).HasMaxLength(25);
            entity.Property(e => e.NgaySinh).HasColumnType("date");
            entity.Property(e => e.TenHocSinh).HasMaxLength(100);

            entity.HasOne(d => d.MaLopNavigation).WithMany(p => p.HocSinhs)
                .HasForeignKey(d => d.MaLop)
                .HasConstraintName("FK_HS_MaLOP");
        });

        modelBuilder.Entity<Khoi>(entity =>
        {
            entity.HasKey(e => e.MaKhoi).HasName("PK__Khoi__6539040DB1147E7F");

            entity.ToTable("Khoi");

            entity.Property(e => e.MaKhoi).HasMaxLength(25);
            entity.Property(e => e.TenKhoi).HasMaxLength(50);
        });

        modelBuilder.Entity<LoaiDiem>(entity =>
        {
            entity.HasKey(e => e.MaLoaiDiem).HasName("PK__LoaiDiem__77BE9E4A6CE90FBC");

            entity.ToTable("LoaiDiem");

            entity.Property(e => e.MaLoaiDiem).HasMaxLength(25);
            entity.Property(e => e.TenLoai).HasMaxLength(20);
        });

        modelBuilder.Entity<Lop>(entity =>
        {
            entity.HasKey(e => e.MaLop).HasName("PK__Lop__3B98D2731D57A763");

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

            entity.HasOne(d => d.MaNienKhoaNavigation).WithMany(p => p.Lops)
                .HasForeignKey(d => d.MaNienKhoa)
                .HasConstraintName("FK_Lop_MaNienKhoa");

            entity.HasOne(d => d.TenGvcnNavigation).WithMany(p => p.Lops)
                .HasPrincipalKey(p => p.TenGiaoVien)
                .HasForeignKey(d => d.TenGvcn)
                .HasConstraintName("FK_Lop_TenGVCN");
        });

        modelBuilder.Entity<MonHoc>(entity =>
        {
            entity.HasKey(e => e.MaMon).HasName("PK__MonHoc__3A5B29A86FE6F41C");

            entity.ToTable("MonHoc");

            entity.Property(e => e.MaMon).HasMaxLength(25);
            entity.Property(e => e.TenMon).HasMaxLength(100);
        });

        modelBuilder.Entity<NguoiDung>(entity =>
        {
            entity.HasKey(e => e.MaNguoiDung).HasName("PK__NguoiDun__C539D762D0D270C5");

            entity.ToTable("NguoiDung");

            entity.HasIndex(e => e.TenDangNhap, "UQ__NguoiDun__55F68FC0891F4D32").IsUnique();

            entity.Property(e => e.MaNguoiDung).HasMaxLength(25);
            entity.Property(e => e.MaGiaoVien).HasMaxLength(25);
            entity.Property(e => e.MatKhau).HasMaxLength(100);
            entity.Property(e => e.TenDangNhap).HasMaxLength(50);

            entity.HasOne(d => d.MaGiaoVienNavigation).WithMany(p => p.NguoiDungs)
                .HasForeignKey(d => d.MaGiaoVien)
                .HasConstraintName("FK__NguoiDung__MaGia__503BEA1C");
        });

        modelBuilder.Entity<NienKhoa>(entity =>
        {
            entity.HasKey(e => e.MaNienKhoa).HasName("PK__NienKhoa__6F040C385F9B2F5F");

            entity.ToTable("NienKhoa");

            entity.Property(e => e.MaNienKhoa).HasMaxLength(25);
            entity.Property(e => e.TenNienKhoa).HasMaxLength(50);
        });

        modelBuilder.Entity<PhanCongGiangDay>(entity =>
        {
            entity.HasKey(e => e.MaPhanCong).HasName("PK__PhanCong__C279D9163296CD10");

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
            entity.HasKey(e => e.MaQuiDinh).HasName("PK__QuiDinh__7EDE71679CC76E96");

            entity.ToTable("QuiDinh");

            entity.Property(e => e.MaQuiDinh).HasMaxLength(25);
            entity.Property(e => e.TenQuiDinh).HasMaxLength(100);
        });

        modelBuilder.Entity<ThanhTich>(entity =>
        {
            entity.HasKey(e => e.MaThanhTich).HasName("PK__ThanhTic__C65ACB7967929AE4");

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

        modelBuilder.Entity<VStudentAchievement>(entity =>
        {
            entity
                .HasNoKey()
                .ToView("V_StudentAchievement");

            entity.Property(e => e.MaHocKy).HasMaxLength(25);
            entity.Property(e => e.MaLop).HasMaxLength(25);
            entity.Property(e => e.MaNienKhoa).HasMaxLength(25);
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
