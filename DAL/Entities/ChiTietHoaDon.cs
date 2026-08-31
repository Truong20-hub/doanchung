using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace DAL.Entities;

[Table("chi_tiet_hoa_don")]
[Index("MaHoaDon", Name = "idx_chitiet_hoadon_hoadon")]
public partial class ChiTietHoaDon
{
    // Mã định danh duy nhất của từng dòng chi tiết hóa đơn.
    [Key]
    [Column("ma_chi_tiet_hoa_don")]
    public int MaChiTietHoaDon { get; set; }

    // Mã hóa đơn mà dòng chi tiết này thuộc về, dùng để liên kết với bảng hoa_don.
    [Column("ma_hoa_don")]
    public int MaHoaDon { get; set; }

    // Số lượng của từng mặt hàng/dịch vụ trong hóa đơn.
    [Column("so_luong")]
    public int SoLuong { get; set; }

    // Đơn giá đơn vị của từng mục trong hóa đơn.
    [Column("don_gia", TypeName = "decimal(12, 2)")]
    public decimal DonGia { get; set; }

    // Thành tiền của từng mục, thường bằng so_luong * don_gia.
    [Column("thanh_tien", TypeName = "decimal(12, 2)")]
    public decimal ThanhTien { get; set; }

    // Ghi chú bổ sung cho dòng chi tiết hóa đơn nếu cần.
    [Column("ghi_chu")]
    [StringLength(255)]
    public string? GhiChu { get; set; }

    // Navigation property cho quan hệ nhiều-1 với hóa đơn cha.
    [ForeignKey("MaHoaDon")]
    [InverseProperty("ChiTietHoaDons")]
    public virtual HoaDon MaHoaDonNavigation { get; set; } = null!;
}
