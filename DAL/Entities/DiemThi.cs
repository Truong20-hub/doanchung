using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace DAL.Entities;

[Table("diem_thi")]
[Index("MaKyThi", "MaHocVien", Name = "uq_kythi_hocvien", IsUnique = true)]
public partial class DiemThi
{
    [Key]
    [Column("ma_diem_thi")]
    public int MaDiemThi { get; set; }

    [Column("ma_ky_thi")]
    public int MaKyThi { get; set; }

    [Column("ma_hoc_vien")]
    public int MaHocVien { get; set; }

    [Column("diem_nghe", TypeName = "decimal(5, 2)")]
    public decimal? DiemNghe { get; set; }

    [Column("diem_noi", TypeName = "decimal(5, 2)")]
    public decimal? DiemNoi { get; set; }

    [Column("diem_doc", TypeName = "decimal(5, 2)")]
    public decimal? DiemDoc { get; set; }

    [Column("diem_viet", TypeName = "decimal(5, 2)")]
    public decimal? DiemViet { get; set; }

    [Column("tong_diem", TypeName = "decimal(5, 2)")]
    public decimal? TongDiem { get; set; }

    [Column("nhan_xet")]
    public string? NhanXet { get; set; }

    [ForeignKey("MaHocVien")]
    [InverseProperty("DiemThis")]
    public virtual HocVien MaHocVienNavigation { get; set; } = null!;

    [ForeignKey("MaKyThi")]
    [InverseProperty("DiemThis")]
    public virtual KyThi MaKyThiNavigation { get; set; } = null!;
}
