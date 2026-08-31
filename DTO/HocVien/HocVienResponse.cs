using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTO.HocVien
{
 

    public class HocVienResponse
    {
        public int MaHocVien { get; set; }

        public int? MaNguoiDung { get; set; }

        public string HoTen { get; set; } = string.Empty;

        public string? GioiTinh { get; set; }

        public DateOnly? NgaySinh { get; set; }

        public string? SoDienThoai { get; set; }

        public string? Email { get; set; }

        public string? DiaChi { get; set; }

        public string? TenPhuHuynh { get; set; }

        public string? SdtPhuHuynh { get; set; }

        public DateOnly? NgayNhapHoc { get; set; }

        public bool? DangHoatDong { get; set; }

        // Thông tin tài khoản
        public string? TenDangNhap { get; set; }

        public string? AvatarUrl { get; set; }
    }
}

