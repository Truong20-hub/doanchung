using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTO.GiaoVien;

public class GiaoVienResponse
{
    // Thông tin Giáo viên
    public int MaGiaoVien { get; set; }
    public int? MaNguoiDung { get; set; }
    public string HoTen { get; set; }
    public string GioiTinh { get; set; }
    public DateOnly? NgaySinh { get; set; }
    public string SoDienThoai { get; set; }
    public string Email { get; set; }
    public string ChuyenMon { get; set; }
    public DateOnly? NgayVaoLam { get; set; }
    public decimal? LuongTheoGio { get; set; }
    public bool DangHoatDong { get; set; }

    // Thông tin Người dùng
    public string TenDangNhap { get; set; }
    public string MatKhauHash { get; set; }
    public int? MaVaiTro { get; set; }
    public string AvatarUrl { get; set; }
    public DateTime? NgayTao { get; set; }
}