using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace DAL.Entities;

[Table("lop_hoc")]
[Index("MaLopCode", Name = "UQ__lop_hoc__434E0C8111D894BA", IsUnique = true)]
[Index("MaGiaoVien", Name = "idx_lophoc_giaovien")]
[Index("MaKhoaHoc", Name = "idx_lophoc_khoahoc")]
public partial class LopHoc
{
    [Key]
    [Column("ma_lop")]
    public int MaLop { get; set; }

    [Column("ma_lop_code")]
    [StringLength(30)]
    public string MaLopCode { get; set; } = null!;

    [Column("ten_lop")]
    [StringLength(150)]
    public string TenLop { get; set; } = null!;

    [Column("ma_khoa_hoc")]
    public int MaKhoaHoc { get; set; }

    [Column("ma_giao_vien")]
    public int? MaGiaoVien { get; set; }

    [Column("ma_phong")]
    public int? MaPhong { get; set; }

    [Column("avatar_url")]
    [StringLength(500)]
    public string? AvatarUrl { get; set; }

    [Column("ngay_bat_dau")]
    public DateOnly? NgayBatDau { get; set; }

    [Column("ngay_ket_thuc")]
    public DateOnly? NgayKetThuc { get; set; }

    [Column("si_so_toi_da")]
    public int? SiSoToiDa { get; set; }

    [Column("trang_thai")]
    [StringLength(20)]
    public string? TrangThai { get; set; }

    [InverseProperty("MaLopNavigation")]
    public virtual ICollection<BuoiHoc> BuoiHocs { get; set; } = new List<BuoiHoc>();

    [InverseProperty("MaLopNavigation")]
    public virtual ICollection<DangKyHoc> DangKyHocs { get; set; } = new List<DangKyHoc>();

    [InverseProperty("MaLopNavigation")]
    public virtual ICollection<HoaDon> HoaDons { get; set; } = new List<HoaDon>();

    [InverseProperty("MaLopNavigation")]
    public virtual ICollection<KyThi> KyThis { get; set; } = new List<KyThi>();

    [InverseProperty("MaLopNavigation")]
    public virtual ICollection<LichHoc> LichHocs { get; set; } = new List<LichHoc>();

    [InverseProperty("MaLopNavigation")]
    public virtual ICollection<ThongBao> ThongBaos { get; set; } = new List<ThongBao>();

    [InverseProperty("MaLopNavigation")]
    public virtual ICollection<BaiTap> BaiTaps { get; set; } = new List<BaiTap>();

    [ForeignKey("MaGiaoVien")]
    [InverseProperty("LopHocs")]
    public virtual GiaoVien? MaGiaoVienNavigation { get; set; }

    [ForeignKey("MaKhoaHoc")]
    [InverseProperty("LopHocs")]
    public virtual KhoaHoc MaKhoaHocNavigation { get; set; } = null!;

    [ForeignKey("MaPhong")]
    [InverseProperty("LopHocs")]
    public virtual PhongHoc? MaPhongNavigation { get; set; }
}
