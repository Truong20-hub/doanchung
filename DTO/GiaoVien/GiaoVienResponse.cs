using System;

namespace DTO.GiaoVien;

public class GiaoVienResponse
{
    // Thông tin giáo viên
    public int MaGiaoVien { get; set; }
    public int MaNguoiDung { get; set; }
    public string? GioiTinh { get; set; }
    public DateOnly? NgaySinh { get; set; }
    public string? ChuyenMon { get; set; }
    public DateOnly? NgayVaoLam { get; set; }
    public decimal? LuongTheoGio { get; set; }

    // Thông tin tham chiếu từ bảng người dùng
    public string HoTen { get; set; } = string.Empty;
    public string? Email { get; set; }
    public string? SoDienThoai { get; set; }
    public bool DangHoatDong { get; set; }

    // Thông tin tài khoản người dùng
    public string TenDangNhap { get; set; } = string.Empty;
    public string? MatKhauHash { get; set; }
    public int? MaVaiTro { get; set; }
    public string? AvatarUrl { get; set; }
    public DateTime? NgayTao { get; set; }
}