using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTO.HocVien
{
    public class CreateHocVienRequest
    {
        // =========================
        // THÔNG TIN TÀI KHOẢN
        // =========================

        [Required]
        public string TenDangNhap { get; set; } = string.Empty;

        [Required]
        public string MatKhau { get; set; } = string.Empty;

        public int MaVaiTro { get; set; }

        public string? AvatarUrl { get; set; }


        // =========================
        // THÔNG TIN HỌC VIÊN
        // =========================

        [Required]
        public string HoTen { get; set; } = string.Empty;

        public string? GioiTinh { get; set; }

        public DateOnly? NgaySinh { get; set; }

        public string? SoDienThoai { get; set; }

        public string? Email { get; set; }

        public string? DiaChi { get; set; }

        public string? TenPhuHuynh { get; set; }

        public string? SdtPhuHuynh { get; set; }

        public DateOnly? NgayNhapHoc { get; set; }

        public bool DangHoatDong { get; set; } = true;

    }
}
