namespace DTO.DangKyHoc
{
    public class DangKyHocResponse
    {
        public int MaDangKy { get; set; }

        public int MaHocVien { get; set; }

        public string? TenHocVien { get; set; }

        public int MaLop { get; set; }

        public string? MaLopCode { get; set; }

        public string? TenLop { get; set; }

        public DateOnly? NgayDangKy { get; set; }

        public string? TrangThai { get; set; }
    }
}