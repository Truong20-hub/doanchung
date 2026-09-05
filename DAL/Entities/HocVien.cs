using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace DAL.Entities;

[Table("hoc_vien")]
[Index("MaNguoiDung", Name = "UQ__hoc_vien__19C32CF6D07AA977", IsUnique = true)]
public partial class HocVien
{
    [Key]
    [Column("ma_hoc_vien")]
    public int MaHocVien { get; set; }

    [Column("ma_nguoi_dung")]
    public int? MaNguoiDung { get; set; }

    [Column("ho_ten")]
    [StringLength(100)]
    public string HoTen { get; set; } = null!;

    [Column("gioi_tinh")]
    [StringLength(10)]
    public string? GioiTinh { get; set; }

    [Column("ngay_sinh")]
    public DateOnly? NgaySinh { get; set; }

    [Column("so_dien_thoai")]
    [StringLength(20)]
    public string? SoDienThoai { get; set; }

    [Column("email")]
    [StringLength(100)]
    public string? Email { get; set; }

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

    [Column("dang_hoat_dong")]
    public bool? DangHoatDong { get; set; }

    [InverseProperty("MaHocVienNavigation")]
    public virtual ICollection<DangKyHoc> DangKyHocs { get; set; } = new List<DangKyHoc>();

    [InverseProperty("MaHocVienNavigation")]
    public virtual ICollection<DiemDanh> DiemDanhs { get; set; } = new List<DiemDanh>();

    [InverseProperty("MaHocVienNavigation")]
    public virtual ICollection<DiemThi> DiemThis { get; set; } = new List<DiemThi>();

    [InverseProperty("MaHocVienNavigation")]
    public virtual ICollection<HoaDon> HoaDons { get; set; } = new List<HoaDon>();

    [InverseProperty("MaHocVienNavigation")]
    public virtual ICollection<TinNhan> TinNhans { get; set; } = new List<TinNhan>();

    [ForeignKey("MaNguoiDung")]
    [InverseProperty("HocVien")]
    public virtual NguoiDung? MaNguoiDungNavigation { get; set; }
}
