namespace DTO.ThanhToan
{
    public class ThanhToanResponse
    {
        public int MaThanhToan { get; set; }

        public int MaHoaDon { get; set; }

        public decimal TongTienHoaDon { get; set; }

        public decimal GiamGia { get; set; }

        public decimal ThanhTien { get; set; }

        public decimal TongDaThanhToan { get; set; }

        public decimal ConLai { get; set; }

        public decimal SoTienDaTra { get; set; }

        public DateTime? NgayThanhToan { get; set; }

        public string? HinhThuc { get; set; }

        public string? GhiChu { get; set; }
    }
}