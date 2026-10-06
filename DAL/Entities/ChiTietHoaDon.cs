using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace DAL.Entities;

[Table("chi_tiet_hoa_don")]
[Index("MaHoaDon", Name = "idx_cthd_hoadon")]
public partial class ChiTietHoaDon
{
    [Key]
    [Column("ma_chi_tiet")]
    public int MaChiTietHoaDon { get; set; }

    [Column("ma_hoa_don")]
    public int MaHoaDon { get; set; }

    [Column("loai_khoan")]
    [StringLength(20)]
    public string LoaiKhoan { get; set; } = "HocPhi";

    [Column("mo_ta")]
    [StringLength(255)]
    public string MoTa { get; set; } = null!;

    [Column("so_luong")]
    public int SoLuong { get; set; }

    [Column("don_gia", TypeName = "decimal(12, 2)")]
    public decimal DonGia { get; set; }

    [Column("thanh_tien", TypeName = "decimal(23, 2)")]
    public decimal ThanhTien { get; set; }

    [Column("ghi_chu")]
    [StringLength(255)]
    public string? GhiChu { get; set; }

    [ForeignKey("MaHoaDon")]
    [InverseProperty("ChiTietHoaDons")]
    public virtual HoaDon MaHoaDonNavigation { get; set; } = null!;
}
