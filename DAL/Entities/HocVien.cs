using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace DAL.Entities;

[Table("hoc_vien")]
[Index("MaNguoiDung", Name = "UQ_hocvien_nguoidung", IsUnique = true)]
public partial class HocVien
{
    [Key]
    [Column("ma_hoc_vien")]
    public int MaHocVien { get; set; }

    [Column("ma_nguoi_dung")]
    public int MaNguoiDung { get; set; }

    [Column("gioi_tinh")]
    [StringLength(10)]
    public string? GioiTinh { get; set; }

    [Column("ngay_sinh")]
    public DateOnly? NgaySinh { get; set; }

    [Column("dia_chi")]
    [StringLength(255)]
    public string? DiaChi { get; set; }

    [Column("ten_phu_huynh")]
    [StringLength(100)]
    public string? TenPhuHuynh { get; set; }

    [Column("sdt_phu_huynh")]
    [StringLength(20)]
    public string? SdtPhuHuynh { get; set; }

    [Column("ngay_nhap_hoc")]
    public DateOnly? NgayNhapHoc { get; set; }

    // Helper properties mapped to NguoiDung
    [NotMapped]
    public string HoTen
    {
        get => MaNguoiDungNavigation?.HoTen ?? string.Empty;
        set
        {
            if (MaNguoiDungNavigation != null)
                MaNguoiDungNavigation.HoTen = value;
        }
    }

    [NotMapped]
    public string? Email
    {
        get => MaNguoiDungNavigation?.Email;
        set
        {
            if (MaNguoiDungNavigation != null)
                MaNguoiDungNavigation.Email = value;
        }
    }

    [NotMapped]
    public string? SoDienThoai
    {
        get => MaNguoiDungNavigation?.SoDienThoai;
        set
        {
            if (MaNguoiDungNavigation != null)
                MaNguoiDungNavigation.SoDienThoai = value;
        }
    }

    [NotMapped]
    public bool DangHoatDong
    {
        get => MaNguoiDungNavigation?.DangHoatDong ?? true;
        set
        {
            if (MaNguoiDungNavigation != null)
                MaNguoiDungNavigation.DangHoatDong = value;
        }
    }

    [InverseProperty("MaHocVienNavigation")]
    public virtual ICollection<DangKyHoc> DangKyHocs { get; set; } = new List<DangKyHoc>();

    [InverseProperty("MaHocVienNavigation")]
    public virtual ICollection<DiemDanh> DiemDanhs { get; set; } = new List<DiemDanh>();

    [InverseProperty("MaHocVienNavigation")]
    public virtual ICollection<DiemThi> DiemThis { get; set; } = new List<DiemThi>();

    [InverseProperty("MaHocVienNavigation")]
    public virtual ICollection<Diem> Diems { get; set; } = new List<Diem>();

    [InverseProperty("MaHocVienNavigation")]
    public virtual ICollection<HoaDon> HoaDons { get; set; } = new List<HoaDon>();

    [InverseProperty("MaHocVienNavigation")]
    public virtual ICollection<TinNhan> TinNhans { get; set; } = new List<TinNhan>();

    [InverseProperty("MaHocVienNavigation")]
    public virtual ICollection<ThongBao> ThongBaos { get; set; } = new List<ThongBao>();

    [ForeignKey("MaNguoiDung")]
    [InverseProperty("HocVien")]
    public virtual NguoiDung MaNguoiDungNavigation { get; set; } = null!;
}
