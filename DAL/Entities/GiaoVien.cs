using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace DAL.Entities;

[Table("giao_vien")]
[Index("MaNguoiDung", Name = "UQ__giao_vie__19C32CF68D4DEA94", IsUnique = true)]
public partial class GiaoVien
{
    [Key]
    [Column("ma_giao_vien")]
    public int MaGiaoVien { get; set; }

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

    [Column("chuyen_mon")]
    [StringLength(100)]
    public string? ChuyenMon { get; set; }

    [Column("ngay_vao_lam")]
    public DateOnly? NgayVaoLam { get; set; }

    [Column("luong_theo_gio", TypeName = "decimal(10, 2)")]
    public decimal? LuongTheoGio { get; set; }

    [Column("dang_hoat_dong")]
    public bool DangHoatDong { get; set; }

    [InverseProperty("MaGiaoVienNavigation")]
    public virtual ICollection<LopHoc> LopHocs { get; set; } = new List<LopHoc>();

    [ForeignKey("MaNguoiDung")]
    [InverseProperty("GiaoVien")]
    public virtual NguoiDung? MaNguoiDungNavigation { get; set; }
}
