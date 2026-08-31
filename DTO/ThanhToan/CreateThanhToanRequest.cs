using System.ComponentModel.DataAnnotations;

namespace DTO.ThanhToan
{
    public class CreateThanhToanRequest
    {
        [Required(ErrorMessage = "Mã hóa đơn không được để trống")]
        public int MaHoaDon { get; set; }

        [Range(
            0.01,
            999999999999.99,
            ErrorMessage = "Số tiền đã trả phải lớn hơn 0")]
        public decimal SoTienDaTra { get; set; }

        public DateTime? NgayThanhToan { get; set; }

        [StringLength(
            20,
            ErrorMessage = "Hình thức thanh toán không được quá 20 ký tự")]
        public string? HinhThuc { get; set; }

        [StringLength(
            255,
            ErrorMessage = "Ghi chú không được quá 255 ký tự")]
        public string? GhiChu { get; set; }
    }
}