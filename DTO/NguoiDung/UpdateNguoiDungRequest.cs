using System.ComponentModel.DataAnnotations;

namespace DTO.NguoiDung
{
    public class UpdateNguoiDungRequest
    {
        [Required(ErrorMessage = "Tên đăng nhập không được để trống")]
        [StringLength(50, ErrorMessage = "Tên đăng nhập tối đa 50 ký tự")]
        public string TenDangNhap { get; set; } = null!;

        [Required(ErrorMessage = "Họ tên không được để trống")]
        [StringLength(100, ErrorMessage = "Họ tên tối đa 100 ký tự")]
        public string HoTen { get; set; } = null!;

        [EmailAddress(ErrorMessage = "Email không đúng định dạng")]
        [StringLength(100, ErrorMessage = "Email tối đa 100 ký tự")]
        public string? Email { get; set; }

        [StringLength(20, ErrorMessage = "Số điện thoại tối đa 20 ký tự")]
        public string? SoDienThoai { get; set; }

        [Required(ErrorMessage = "Mã vai trò không được để trống")]
        public int MaVaiTro { get; set; }

        [StringLength(500, ErrorMessage = "Avatar URL tối đa 500 ký tự")]
        public string? AvatarUrl { get; set; }

        public bool? DangHoatDong { get; set; }
    }
}