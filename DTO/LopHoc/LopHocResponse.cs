namespace DTO.LopHoc
{
    public class LopHocResponse
    {
        public int MaLop { get; set; }

        public string MaLopCode { get; set; } = null!;

        public string TenLop { get; set; } = null!;


        // ================================
        // KHÓA HỌC
        // ================================

        public int MaKhoaHoc { get; set; }

        public string? TenKhoaHoc { get; set; }


        // ================================
        // GIÁO VIÊN
        // ================================

        public int? MaGiaoVien { get; set; }

        public string? TenGiaoVien { get; set; }


        // ================================
        // PHÒNG HỌC
        // ================================

        public int? MaPhong { get; set; }

        public string? TenPhong { get; set; }


        // ================================
        // THÔNG TIN KHÁC
        // ================================

        public string? AvatarUrl { get; set; }

        public DateOnly? NgayBatDau { get; set; }

        public DateOnly? NgayKetThuc { get; set; }

        public int? SiSoToiDa { get; set; }

        public string? TrangThai { get; set; }
    }
}