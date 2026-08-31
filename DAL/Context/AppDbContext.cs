using System;
using System.Collections.Generic;
using DAL.Entities;
using Microsoft.EntityFrameworkCore;

namespace DAL.Context;

public partial class AppDbContext : DbContext
{
    public AppDbContext()
    {
    }

    public AppDbContext(DbContextOptions<AppDbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<BuoiHoc> BuoiHocs { get; set; }

    public virtual DbSet<DangKyHoc> DangKyHocs { get; set; }

    public virtual DbSet<DiemDanh> DiemDanhs { get; set; }

    public virtual DbSet<DiemThi> DiemThis { get; set; }

    public virtual DbSet<GiaoVien> GiaoViens { get; set; }

    public virtual DbSet<HoaDon> HoaDons { get; set; }

    public virtual DbSet<ChiTietHoaDon> ChiTietHoaDons { get; set; }

    public virtual DbSet<HocVien> HocViens { get; set; }

    public virtual DbSet<KhoaHoc> KhoaHocs { get; set; }

    public virtual DbSet<KyThi> KyThis { get; set; }

    public virtual DbSet<LichHoc> LichHocs { get; set; }

    public virtual DbSet<LopHoc> LopHocs { get; set; }

    public virtual DbSet<NguoiDung> NguoiDungs { get; set; }

    public virtual DbSet<PhongHoc> PhongHocs { get; set; }

    public virtual DbSet<ThanhToan> ThanhToans { get; set; }

    public virtual DbSet<VaiTro> VaiTros { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("Server=.\\SQLEXPRESS;Database=QuanLyLopTiengAnh;Trusted_Connection=True;TrustServerCertificate=True;");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<BuoiHoc>(entity =>
        {
            entity.HasKey(e => e.MaBuoi).HasName("PK__buoi_hoc__75622D6CB206300B");

            entity.Property(e => e.BiHuy).HasDefaultValue(false);

            entity.HasOne(d => d.MaLopNavigation).WithMany(p => p.BuoiHocs).HasConstraintName("fk_buoihoc_lop");
        });

        modelBuilder.Entity<DangKyHoc>(entity =>
        {
            entity.HasKey(e => e.MaDangKy).HasName("PK__dang_ky___BC05F6F1BF2C7608");

            entity.Property(e => e.NgayDangKy).HasDefaultValueSql("(CONVERT([date],getdate()))");
            entity.Property(e => e.TrangThai).HasDefaultValue("DangHoc");

            entity.HasOne(d => d.MaHocVienNavigation).WithMany(p => p.DangKyHocs)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_dangky_hocvien");

            entity.HasOne(d => d.MaLopNavigation).WithMany(p => p.DangKyHocs)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_dangky_lop");
        });

        modelBuilder.Entity<DiemDanh>(entity =>
        {
            entity.HasKey(e => e.MaDiemDanh).HasName("PK__diem_dan__A1C25FA55DA0CBC4");

            entity.Property(e => e.TrangThai).HasDefaultValue("CoMat");

            entity.HasOne(d => d.MaBuoiNavigation).WithMany(p => p.DiemDanhs).HasConstraintName("fk_diemdanh_buoi");

            entity.HasOne(d => d.MaHocVienNavigation).WithMany(p => p.DiemDanhs)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_diemdanh_hocvien");
        });

        modelBuilder.Entity<DiemThi>(entity =>
        {
            entity.HasKey(e => e.MaDiemThi).HasName("PK__diem_thi__9C7EB4967482BDA8");

            entity.HasOne(d => d.MaHocVienNavigation).WithMany(p => p.DiemThis)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_diemthi_hocvien");

            entity.HasOne(d => d.MaKyThiNavigation).WithMany(p => p.DiemThis).HasConstraintName("fk_diemthi_kythi");
        });

        modelBuilder.Entity<GiaoVien>(entity =>
        {
            entity.HasKey(e => e.MaGiaoVien).HasName("PK__giao_vie__35A328F5074A384B");

            entity.Property(e => e.DangHoatDong).HasDefaultValue(true);

            entity.HasOne(d => d.MaNguoiDungNavigation).WithOne(p => p.GiaoVien).HasConstraintName("fk_giaovien_nguoidung");
        });

        modelBuilder.Entity<HoaDon>(entity =>
        {
            entity.HasKey(e => e.MaHoaDon).HasName("PK__hoa_don__DBE2D9E39C3EAC87");

            entity.Property(e => e.GiamGia).HasDefaultValue(0m);
            entity.Property(e => e.NgayLap).HasDefaultValueSql("(CONVERT([date],getdate()))");
            entity.Property(e => e.ThanhTien).HasComputedColumnSql("([tong_tien]-[giam_gia])", true);
            entity.Property(e => e.TrangThai).HasDefaultValue("ChuaThanhToan");

            entity.HasOne(d => d.MaHocVienNavigation).WithMany(p => p.HoaDons)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_hoadon_hocvien");

            entity.HasOne(d => d.MaLopNavigation).WithMany(p => p.HoaDons)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_hoadon_lop");
        });

        modelBuilder.Entity<ChiTietHoaDon>(entity =>
        {
            entity.HasKey(e => e.MaChiTietHoaDon).HasName("PK__chi_tiet_hoa_don__1A2B3C4D5E6F7G8H");

            entity.ToTable("chi_tiet_hoa_don");

            entity.Property(e => e.SoLuong).IsRequired();
            entity.Property(e => e.DonGia).HasColumnType("decimal(12, 2)").IsRequired();
            entity.Property(e => e.ThanhTien).HasColumnType("decimal(12, 2)").IsRequired();
            entity.Property(e => e.GhiChu).HasMaxLength(255);

            entity.HasOne(d => d.MaHoaDonNavigation)
                .WithMany(p => p.ChiTietHoaDons)
                .HasForeignKey(d => d.MaHoaDon)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_chitiet_hoadon_hoadon");
        });

        modelBuilder.Entity<HocVien>(entity =>
        {
            entity.HasKey(e => e.MaHocVien).HasName("PK__hoc_vien__62B18A1135DC3BF0");

            entity.Property(e => e.DangHoatDong).HasDefaultValue(true);
            entity.Property(e => e.NgayNhapHoc).HasDefaultValueSql("(CONVERT([date],getdate()))");

            entity.HasOne(d => d.MaNguoiDungNavigation).WithOne(p => p.HocVien).HasConstraintName("fk_hocvien_nguoidung");
        });

        modelBuilder.Entity<KhoaHoc>(entity =>
        {
            entity.HasKey(e => e.MaKhoaHoc).HasName("PK__khoa_hoc__50BFE165A0FDED03");

            entity.Property(e => e.DangHoatDong).HasDefaultValue(true);
        });

        modelBuilder.Entity<KyThi>(entity =>
        {
            entity.HasKey(e => e.MaKyThi).HasName("PK__ky_thi__21A9582C1FC2ED98");

            entity.Property(e => e.DiemToiDa).HasDefaultValue(10m);
            entity.Property(e => e.LoaiKyThi).HasDefaultValue("QuizNgan");

            entity.HasOne(d => d.MaLopNavigation).WithMany(p => p.KyThis)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_kythi_lop");
        });

        modelBuilder.Entity<LichHoc>(entity =>
        {
            entity.HasKey(e => e.MaLich).HasName("PK__lich_hoc__C3B19B00BF8BF676");

            entity.HasOne(d => d.MaLopNavigation).WithMany(p => p.LichHocs).HasConstraintName("fk_lichhoc_lop");
        });

        modelBuilder.Entity<LopHoc>(entity =>
        {
            entity.HasKey(e => e.MaLop).HasName("PK__lop_hoc__0B8BCDEED7E565D4");

            entity.Property(e => e.SiSoToiDa).HasDefaultValue(20);
            entity.Property(e => e.TrangThai).HasDefaultValue("SapKhaiGiang");

            entity.HasOne(d => d.MaGiaoVienNavigation).WithMany(p => p.LopHocs).HasConstraintName("fk_lophoc_giaovien");

            entity.HasOne(d => d.MaKhoaHocNavigation).WithMany(p => p.LopHocs)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_lophoc_khoahoc");

            entity.HasOne(d => d.MaPhongNavigation).WithMany(p => p.LopHocs).HasConstraintName("fk_lophoc_phong");
        });

        modelBuilder.Entity<NguoiDung>(entity =>
        {
            entity.HasKey(e => e.MaNguoiDung).HasName("PK__nguoi_du__19C32CF7A58F6E30");

            entity.Property(e => e.DangHoatDong).HasDefaultValue(true);
            entity.Property(e => e.NgayTao).HasDefaultValueSql("(sysdatetime())");

            entity.HasOne(d => d.MaVaiTroNavigation).WithMany(p => p.NguoiDungs)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_nguoidung_vaitro");
        });

        modelBuilder.Entity<PhongHoc>(entity =>
        {
            entity.HasKey(e => e.MaPhong).HasName("PK__phong_ho__1BD319C928CE53E1");
        });

        modelBuilder.Entity<ThanhToan>(entity =>
        {
            entity.HasKey(e => e.MaThanhToan).HasName("PK__thanh_to__F89DBB4F09698305");

            entity.Property(e => e.HinhThuc).HasDefaultValue("TienMat");
            entity.Property(e => e.NgayThanhToan).HasDefaultValueSql("(sysdatetime())");

            entity.HasOne(d => d.MaHoaDonNavigation).WithMany(p => p.ThanhToans)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_thanhtoan_hoadon");
        });

        modelBuilder.Entity<VaiTro>(entity =>
        {
            entity.HasKey(e => e.MaVaiTro).HasName("PK__vai_tro__4AE1754D5670477B");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
