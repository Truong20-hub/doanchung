using System.ComponentModel.DataAnnotations;

namespace DTO.HoaDon
{
    public class UpdateHoaDonRequest
    {
        [Required(ErrorMessage = "Mã học viên không được để trống")]
        public int MaHocVien { get; set; }

        [Required(ErrorMessage = "Mã lớp không được để trống")]
        public int MaLop { get; set; }

        [Range(
            0,
            999999999999.99,
            ErrorMessage = "Tổng tiền không hợp lệ")]
        public decimal TongTien { get; set; }

        [Range(
            0,
            999999999999.99,
            ErrorMessage = "Giảm giá không hợp lệ")]
        public decimal? GiamGia { get; set; }

        public DateOnly? NgayLap { get; set; }

        public DateOnly? HanThanhToan { get; set; }

        [StringLength(
            20,
            ErrorMessage = "Trạng thái không quá 20 ký tự")]
        public string? TrangThai { get; set; }
    }
}