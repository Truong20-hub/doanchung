using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace DAL.Entities;

[Table("ky_thi")]
public partial class KyThi
{
    [Key]
    [Column("ma_ky_thi")]
    public int MaKyThi { get; set; }

    [Column("ma_lop")]
    public int MaLop { get; set; }

    [Column("ten_ky_thi")]
    [StringLength(150)]
    public string TenKyThi { get; set; } = null!;

    [Column("ngay_thi")]
    public DateOnly? NgayThi { get; set; }

    [Column("loai_ky_thi")]
    [StringLength(20)]
    public string? LoaiKyThi { get; set; }

    [Column("diem_toi_da", TypeName = "decimal(5, 2)")]
    public decimal? DiemToiDa { get; set; }

    [InverseProperty("MaKyThiNavigation")]
    public virtual ICollection<DiemThi> DiemThis { get; set; } = new List<DiemThi>();

    [ForeignKey("MaLop")]
    [InverseProperty("KyThis")]
    public virtual LopHoc MaLopNavigation { get; set; } = null!;
}
