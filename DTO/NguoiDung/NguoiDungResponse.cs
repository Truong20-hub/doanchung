namespace DTO.NguoiDung
{
    public class NguoiDungResponse
    {
        public int MaNguoiDung { get; set; }

        public string TenDangNhap { get; set; } = string.Empty;

        public string HoTen { get; set; } = string.Empty;

        public string? Email { get; set; }

        public string? SoDienThoai { get; set; }

        public int MaVaiTro { get; set; }

        public string TenVaiTro { get; set; } = string.Empty;

        public string? AvatarUrl { get; set; }

        public bool? DangHoatDong { get; set; }

        public DateTime? NgayTao { get; set; }
    }
}