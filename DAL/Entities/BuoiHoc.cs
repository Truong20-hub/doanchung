using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace DAL.Entities;

[Table("buoi_hoc")]
[Index("MaLop", Name = "idx_buoihoc_lop")]
public partial class BuoiHoc
{
    [Key]
    [Column("ma_buoi")]
    public int MaBuoi { get; set; }

    [Column("ma_lop")]
    public int MaLop { get; set; }

    [Column("ngay_hoc")]
    public DateOnly NgayHoc { get; set; }

    [Column("gio_bat_dau")]
    public TimeOnly? GioBatDau { get; set; }

    [Column("gio_ket_thuc")]
    public TimeOnly? GioKetThuc { get; set; }

    [Column("noi_dung")]
    [StringLength(255)]
    public string? NoiDung { get; set; }

    [Column("bi_huy")]
    public bool? BiHuy { get; set; }

    [InverseProperty("MaBuoiNavigation")]
    public virtual ICollection<DiemDanh> DiemDanhs { get; set; } = new List<DiemDanh>();

    [ForeignKey("MaLop")]
    [InverseProperty("BuoiHocs")]
    public virtual LopHoc MaLopNavigation { get; set; } = null!;
}
