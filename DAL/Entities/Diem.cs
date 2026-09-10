using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace DAL.Entities;

[Table("diem")]
[Index("MaBaiTap", "MaHocVien", Name = "uq_baitap_hocvien", IsUnique = true)]
[Index("MaBaiTap", Name = "idx_diem_baitap")]
[Index("MaHocVien", Name = "idx_diem_hocvien")]
public partial class Diem
{
    [Key]
    [Column("ma_diem")]
    public int MaDiem { get; set; }

    [Column("ma_bai_tap")]
    public int MaBaiTap { get; set; }

    [Column("ma_hoc_vien")]
    public int MaHocVien { get; set; }

    [Column("diem_so", TypeName = "decimal(5, 2)")]
    public decimal? DiemSo { get; set; }

    [Column("trang_thai_nop")]
    [StringLength(20)]
    public string TrangThaiNop { get; set; } = null!;

    [Column("ngay_nop")]
    public DateTime? NgayNop { get; set; }

    [Column("nhan_xet")]
    [StringLength(500)]
    public string? NhanXet { get; set; }

    [ForeignKey("MaBaiTap")]
    [InverseProperty("Diems")]
    public virtual BaiTap MaBaiTapNavigation { get; set; } = null!;

    [ForeignKey("MaHocVien")]
    [InverseProperty("Diems")]
    public virtual HocVien MaHocVienNavigation { get; set; } = null!;
}
