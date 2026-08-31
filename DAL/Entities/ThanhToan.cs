using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace DAL.Entities;

[Table("thanh_toan")]
public partial class ThanhToan
{
    [Key]
    [Column("ma_thanh_toan")]
    public int MaThanhToan { get; set; }

    [Column("ma_hoa_don")]
    public int MaHoaDon { get; set; }

    [Column("so_tien_da_tra", TypeName = "decimal(12, 2)")]
    public decimal SoTienDaTra { get; set; }

    [Column("ngay_thanh_toan")]
    public DateTime? NgayThanhToan { get; set; }

    [Column("hinh_thuc")]
    [StringLength(20)]
    public string? HinhThuc { get; set; }

    [Column("ghi_chu")]
    [StringLength(255)]
    public string? GhiChu { get; set; }

    [ForeignKey("MaHoaDon")]
    [InverseProperty("ThanhToans")]
    public virtual HoaDon MaHoaDonNavigation { get; set; } = null!;
}
