using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace DAL.Entities;

[Table("hoa_don")]
[Index("MaHocVien", Name = "idx_hoadon_hocvien")]
public partial class HoaDon
{
    [Key]
    [Column("ma_hoa_don")]
    public int MaHoaDon { get; set; }

    [Column("ma_hoc_vien")]
    public int MaHocVien { get; set; }

    [Column("ma_lop")]
    public int MaLop { get; set; }

    [Column("tong_tien", TypeName = "decimal(12, 2)")]
    public decimal TongTien { get; set; }

    [Column("giam_gia", TypeName = "decimal(12, 2)")]
    public decimal? GiamGia { get; set; }

    [Column("thanh_tien", TypeName = "decimal(13, 2)")]
    public decimal? ThanhTien { get; set; }

    [Column("ngay_lap")]
    public DateOnly? NgayLap { get; set; }

    [Column("han_thanh_toan")]
    public DateOnly? HanThanhToan { get; set; }

    [Column("trang_thai")]
    [StringLength(20)]
    public string? TrangThai { get; set; }

    [ForeignKey("MaHocVien")]
    [InverseProperty("HoaDons")]
    public virtual HocVien MaHocVienNavigation { get; set; } = null!;

    [ForeignKey("MaLop")]
    [InverseProperty("HoaDons")]
    public virtual LopHoc MaLopNavigation { get; set; } = null!;

    [InverseProperty("MaHoaDonNavigation")]
    public virtual ICollection<ChiTietHoaDon> ChiTietHoaDons { get; set; } = new List<ChiTietHoaDon>();

    [InverseProperty("MaHoaDonNavigation")]
    public virtual ICollection<ThanhToan> ThanhToans { get; set; } = new List<ThanhToan>();
}
