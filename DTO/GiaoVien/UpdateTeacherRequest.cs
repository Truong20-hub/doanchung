using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTO.GiaoVien
{
    public class UpdateTeacherRequest
    {
        public string? TenDangNhap { get; set; }
        public string? MatKhau { get; set; }
        public int MaVaiTro { get; set; }

        public string? HoTen { get; set; }
        public string? GioiTinh { get; set; }
        public DateOnly NgaySinh { get; set; }
        public string? SoDienThoai { get; set; }
        public string? Email { get; set; }
        public string? AvatarUrl { get; set; }

        public bool DangHoatDong { get; set; }
        public string? ChuyenMon { get; set; }
        public DateOnly NgayVaoLam { get; set; }
        public decimal LuongTheoGio { get; set; }
    }
}
