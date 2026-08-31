using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace DAL.Entities;

[Table("diem_danh")]
[Index("MaHocVien", Name = "idx_diemdanh_hocvien")]
[Index("MaBuoi", "MaHocVien", Name = "uq_buoi_hocvien", IsUnique = true)]
public partial class DiemDanh
{
    [Key]
    [Column("ma_diem_danh")]
    public int MaDiemDanh { get; set; }

    [Column("ma_buoi")]
    public int MaBuoi { get; set; }

    [Column("ma_hoc_vien")]
    public int MaHocVien { get; set; }

    [Column("trang_thai")]
    [StringLength(20)]
    public string? TrangThai { get; set; }

    [Column("ghi_chu")]
    [StringLength(255)]
    public string? GhiChu { get; set; }

    [ForeignKey("MaBuoi")]
    [InverseProperty("DiemDanhs")]
    public virtual BuoiHoc MaBuoiNavigation { get; set; } = null!;

    [ForeignKey("MaHocVien")]
    [InverseProperty("DiemDanhs")]
    public virtual HocVien MaHocVienNavigation { get; set; } = null!;
}
