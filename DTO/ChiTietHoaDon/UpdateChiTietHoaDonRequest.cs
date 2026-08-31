using System.ComponentModel.DataAnnotations;

namespace DTO.ChiTietHoaDon
{
    public class UpdateChiTietHoaDonRequest
    {
        // Mã hóa đơn mà dòng chi tiết thuộc về.
        [Required(ErrorMessage = "Mã hóa đơn không được để trống")]
        public int MaHoaDon { get; set; }

        // Số lượng mới của mặt hàng/dịch vụ trong hóa đơn.
        [Range(1, int.MaxValue, ErrorMessage = "Số lượng phải lớn hơn 0")]
        public int SoLuong { get; set; }

        // Đơn giá mới của từng mục trong hóa đơn.
        [Range(0, 999999999999.99, ErrorMessage = "Đơn giá không hợp lệ")]
        public decimal DonGia { get; set; }

        // Ghi chú mới cho chi tiết hóa đơn.
        [StringLength(255, ErrorMessage = "Ghi chú không được vượt quá 255 ký tự")]
        public string? GhiChu { get; set; }
    }
}
