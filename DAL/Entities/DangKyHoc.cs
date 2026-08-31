using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace DAL.Entities;

[Table("dang_ky_hoc")]
[Index("MaHocVien", Name = "idx_dangky_hocvien")]
[Index("MaLop", Name = "idx_dangky_lop")]
[Index("MaHocVien", "MaLop", Name = "uq_hocvien_lop", IsUnique = true)]
public partial class DangKyHoc
{
    [Key]
    [Column("ma_dang_ky")]
    public int MaDangKy { get; set; }

    [Column("ma_hoc_vien")]
    public int MaHocVien { get; set; }

    [Column("ma_lop")]
    public int MaLop { get; set; }

    [Column("ngay_dang_ky")]
    public DateOnly? NgayDangKy { get; set; }

    [Column("trang_thai")]
    [StringLength(20)]
    public string? TrangThai { get; set; }

    [ForeignKey("MaHocVien")]
    [InverseProperty("DangKyHocs")]
    public virtual HocVien MaHocVienNavigation { get; set; } = null!;

    [ForeignKey("MaLop")]
    [InverseProperty("DangKyHocs")]
    public virtual LopHoc MaLopNavigation { get; set; } = null!;
}
