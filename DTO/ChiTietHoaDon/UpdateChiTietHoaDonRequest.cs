using System.ComponentModel.DataAnnotations;

namespace DTO.ChiTietHoaDon
{
    public class UpdateChiTietHoaDonRequest
    {
        // Mã hóa đơn mà dòng chi tiết thuộc về.
        [Required(ErrorMessage = "Mã hóa đơn không được để trống")]
        public int MaHoaDon { get; set; }

        [Required(ErrorMessage = "Loại khoản không được để trống")]
        [RegularExpression("^(HocPhi|GiaoTrinh|LePhiThi|Khac)$", ErrorMessage = "Loại khoản không hợp lệ")]
        public string LoaiKhoan { get; set; } = "HocPhi";

        [Required(ErrorMessage = "Mô tả không được để trống")]
        [StringLength(255, ErrorMessage = "Mô tả không được vượt quá 255 ký tự")]
        public string MoTa { get; set; } = null!;

        // Số lượng mới của mặt hàng/dịch vụ trong hóa đơn.
        [Range(1, int.MaxValue, ErrorMessage = "Số lượng phải lớn hơn 0")]
        public int SoLuong { get; set; }

        // Đơn giá mới của từng mục trong hóa đơn.
        [Range(typeof(decimal), "0", "9999999999.99", ErrorMessage = "Đơn giá không hợp lệ")]
        public decimal DonGia { get; set; }

        // Ghi chú mới cho chi tiết hóa đơn.
        [StringLength(255, ErrorMessage = "Ghi chú không được vượt quá 255 ký tự")]
        public string? GhiChu { get; set; }
    }
}
