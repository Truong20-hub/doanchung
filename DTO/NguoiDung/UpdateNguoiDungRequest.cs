using System.ComponentModel.DataAnnotations;

namespace DTO.NguoiDung
{
    public class UpdateNguoiDungRequest
    {
        [Required]
        [StringLength(100)]
        public string HoTen { get; set; } = string.Empty;

        [EmailAddress]
        public string? Email { get; set; }

        [StringLength(20)]
        public string? SoDienThoai { get; set; }

        [Required]
        public int MaVaiTro { get; set; }

        public string? AvatarUrl { get; set; }

        public bool DangHoatDong { get; set; }
    }
}