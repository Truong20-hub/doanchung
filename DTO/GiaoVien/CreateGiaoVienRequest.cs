using System.ComponentModel.DataAnnotations;

namespace DTO.GiaoVien;

public class CreateGiaoVienRequest
{
    public int? MaNguoiDung { get; set; }

    [Required]
    public string HoTen { get; set; } = string.Empty;

    public string? GioiTinh { get; set; }

    public DateOnly? NgaySinh { get; set; }

    public string? SoDienThoai { get; set; }

    public string? Email { get; set; }

    public string? ChuyenMon { get; set; }

    public DateOnly? NgayVaoLam { get; set; }

    public decimal? LuongTheoGio { get; set; }

    public bool? DangHoatDong { get; set; }
}