using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace DAL.Entities;

[Table("khoa_hoc")]
[Index("MaCode", Name = "UQ__khoa_hoc__7749BF8F6C31E880", IsUnique = true)]
public partial class KhoaHoc
{
    [Key]
    [Column("ma_khoa_hoc")]
    public int MaKhoaHoc { get; set; }

    [Column("ten_khoa_hoc")]
    [StringLength(150)]
    public string TenKhoaHoc { get; set; } = null!;

    [Column("ma_code")]
    [StringLength(20)]
    public string? MaCode { get; set; }

    [Column("trinh_do")]
    [StringLength(50)]
    public string? TrinhDo { get; set; }

    [Column("mo_ta")]
    public string? MoTa { get; set; }

    [Column("tong_so_buoi")]
    public int? TongSoBuoi { get; set; }

    [Column("so_tuan_hoc")]
    public int? SoTuanHoc { get; set; }

    [Column("hoc_phi_chuan", TypeName = "decimal(12, 2)")]
    public decimal? HocPhiChuan { get; set; }

    [Column("dang_hoat_dong")]
    public bool? DangHoatDong { get; set; }

    [InverseProperty("MaKhoaHocNavigation")]
    public virtual ICollection<LopHoc> LopHocs { get; set; } = new List<LopHoc>();
}
