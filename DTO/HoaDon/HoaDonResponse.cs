namespace DTO.HoaDon
{
    public class HoaDonResponse
    {
        public int MaHoaDon { get; set; }

        public int MaHocVien { get; set; }

        public string? HoTenHocVien { get; set; }

        public int MaLop { get; set; }

        public string? MaLopCode { get; set; }

        public string? TenLop { get; set; }

        public decimal TongTien { get; set; }

        public decimal? GiamGia { get; set; }

        public decimal? ThanhTien { get; set; }

        public DateOnly? NgayLap { get; set; }

        public DateOnly? HanThanhToan { get; set; }

        public string? TrangThai { get; set; }
    }
}