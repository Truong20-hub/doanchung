using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace DAL.Entities;

[Table("bai_tap")]
[Index("MaLop", Name = "idx_baitap_lop")]
[Index("MaBuoi", Name = "idx_baitap_buoi")]
public partial class BaiTap
{
    [Key]
    [Column("ma_bai_tap")]
    public int MaBaiTap { get; set; }

    [Column("ma_lop")]
    public int MaLop { get; set; }

    [Column("ma_buoi")]
    public int? MaBuoi { get; set; }

    [Column("ma_giao_vien")]
    public int MaGiaoVien { get; set; }

    [Column("tieu_de")]
    [StringLength(200)]
    public string TieuDe { get; set; } = null!;

    [Column("mo_ta")]
    public string? MoTa { get; set; }

    [Column("ngay_giao")]
    public DateOnly NgayGiao { get; set; }

    [Column("han_nop")]
    public DateOnly? HanNop { get; set; }

    [Column("file_dinh_kem")]
    [StringLength(500)]
    public string? FileDinhKem { get; set; }

    [InverseProperty("MaBaiTapNavigation")]
    public virtual ICollection<Diem> Diems { get; set; } = new List<Diem>();

    [ForeignKey("MaBuoi")]
    [InverseProperty("BaiTaps")]
    public virtual BuoiHoc? MaBuoiNavigation { get; set; }

    [ForeignKey("MaGiaoVien")]
    [InverseProperty("BaiTaps")]
    public virtual GiaoVien MaGiaoVienNavigation { get; set; } = null!;

    [ForeignKey("MaLop")]
    [InverseProperty("BaiTaps")]
    public virtual LopHoc MaLopNavigation { get; set; } = null!;
}
