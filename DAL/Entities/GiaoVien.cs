using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace DAL.Entities;

[Table("giao_vien")]
[Index("MaNguoiDung", Name = "UQ_giaovien_nguoidung", IsUnique = true)]
public partial class GiaoVien
{
    [Key]
    [Column("ma_giao_vien")]
    public int MaGiaoVien { get; set; }

    [Column("ma_nguoi_dung")]
    public int MaNguoiDung { get; set; }

    [Column("gioi_tinh")]
    [StringLength(10)]
    public string? GioiTinh { get; set; }

    [Column("ngay_sinh")]
    public DateOnly? NgaySinh { get; set; }

    [Column("chuyen_mon")]
    [StringLength(100)]
    public string? ChuyenMon { get; set; }

    [Column("ngay_vao_lam")]
    public DateOnly? NgayVaoLam { get; set; }

    [Column("luong_theo_gio", TypeName = "decimal(10, 2)")]
    public decimal? LuongTheoGio { get; set; }

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

    [InverseProperty("MaGiaoVienNavigation")]
    public virtual ICollection<LopHoc> LopHocs { get; set; } = new List<LopHoc>();

    [InverseProperty("MaGiaoVienNavigation")]
    public virtual ICollection<BaiTap> BaiTaps { get; set; } = new List<BaiTap>();

    [ForeignKey("MaNguoiDung")]
    [InverseProperty("GiaoVien")]
    public virtual NguoiDung MaNguoiDungNavigation { get; set; } = null!;
}
