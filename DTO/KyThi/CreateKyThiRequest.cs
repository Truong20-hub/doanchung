using System.ComponentModel.DataAnnotations;

namespace DTO.KyThi
{
    public class CreateKyThiRequest
    {
        [Required(ErrorMessage = "Mã lớp không được để trống")]
        public int MaLop { get; set; }

        [Required(ErrorMessage = "Tên kỳ thi không được để trống")]
        [StringLength(150, ErrorMessage = "Tên kỳ thi không quá 150 ký tự")]
        public string TenKyThi { get; set; } = null!;

        public DateOnly? NgayThi { get; set; }

        [StringLength(20, ErrorMessage = "Loại kỳ thi không quá 20 ký tự")]
        public string? LoaiKyThi { get; set; }

        [Range(
            0.01,
            999.99,
            ErrorMessage = "Điểm tối đa phải lớn hơn 0")]
        public decimal? DiemToiDa { get; set; }
    }
}