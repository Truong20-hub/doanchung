using System.ComponentModel.DataAnnotations;

namespace DTO.NguoiDung
{
    public class CreateNguoiDungRequest
    {
        [Required]
        public int MaNguoiDung { get; set; }
        [Required(ErrorMessage = "Tên đăng nhập không được để trống.")]
        [StringLength(50)]
        public string TenDangNhap { get; set; } = string.Empty;

        [Required(ErrorMessage = "Mật khẩu không được để trống.")]
        [StringLength(100, MinimumLength = 6)]
        public string MatKhau { get; set; } = string.Empty;

        [Required(ErrorMessage = "Họ tên không được để trống.")]
        [StringLength(100)]
        public string HoTen { get; set; } = string.Empty;

        [EmailAddress]
        public string? Email { get; set; }

        [StringLength(20)]
        public string? SoDienThoai { get; set; }

        [Required]
        public int MaVaiTro { get; set; }

        public string? AvatarUrl { get; set; }
    }
}