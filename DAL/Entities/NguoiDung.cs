using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace DAL.Entities;

[Table("nguoi_dung")]
[Index("TenDangNhap", Name = "UQ__nguoi_du__363698B33F0CD161", IsUnique = true)]
[Index("Email", Name = "UQ__nguoi_du__AB6E61643EC8AD2C", IsUnique = true)]
public partial class NguoiDung
{
    [Key]
    [Column("ma_nguoi_dung")]
    public int MaNguoiDung { get; set; }

    [Column("ten_dang_nhap")]
    [StringLength(50)]
    public string TenDangNhap { get; set; } = null!;

    [Column("mat_khau_hash")]
    [StringLength(255)]
    public string MatKhauHash { get; set; } = null!;

    [Column("ho_ten")]
    [StringLength(100)]
    public string HoTen { get; set; } = null!;

    [Column("email")]
    [StringLength(100)]
    public string? Email { get; set; }

    [Column("so_dien_thoai")]
    [StringLength(20)]
    public string? SoDienThoai { get; set; }

    [Column("ma_vai_tro")]
    public int MaVaiTro { get; set; }

    [Column("avatar_url")]
    [StringLength(500)]
    public string? AvatarUrl { get; set; }

    [Column("dang_hoat_dong")]
    public bool? DangHoatDong { get; set; }

    [Column("ngay_tao")]
    public DateTime? NgayTao { get; set; }

    [InverseProperty("MaNguoiDungNavigation")]
    public virtual GiaoVien? GiaoVien { get; set; }

    [InverseProperty("MaNguoiDungNavigation")]
    public virtual HocVien? HocVien { get; set; }

    [InverseProperty("MaNguoiGuiNavigation")]
    public virtual ICollection<TinNhan> TinNhansDaGui { get; set; } = new List<TinNhan>();

    [InverseProperty("MaNguoiNhanNavigation")]
    public virtual ICollection<TinNhan> TinNhansDaNhan { get; set; } = new List<TinNhan>();

    [ForeignKey("MaVaiTro")]
    [InverseProperty("NguoiDungs")]
    public virtual VaiTro MaVaiTroNavigation { get; set; } = null!;
}
