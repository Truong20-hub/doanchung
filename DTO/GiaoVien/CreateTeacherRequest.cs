using DTO.NguoiDung;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTO.GiaoVien
{
    public class CreateTeacherRequest
    {
        
            // Thông tin tài khoản
            public string TenDangNhap { get; set; } = string.Empty;
            public string MatKhau { get; set; } = string.Empty;
            public int MaVaiTro { get; set; }

            // Thông tin chung
            public string HoTen { get; set; } = string.Empty;
            public string? GioiTinh { get; set; }
            public DateOnly? NgaySinh { get; set; }
            public string? SoDienThoai { get; set; }
            public string? Email { get; set; }
            public string? AvatarUrl { get; set; }
            public bool DangHoatDong { get; set; } = true;

            // Thông tin giáo viên
            public string? ChuyenMon { get; set; }
            public DateOnly? NgayVaoLam { get; set; }
            public decimal? LuongTheoGio { get; set; }
      

    }
}
