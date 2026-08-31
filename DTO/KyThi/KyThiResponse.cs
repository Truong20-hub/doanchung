namespace DTO.KyThi
{
    public class KyThiResponse
    {
        public int MaKyThi { get; set; }

        public int MaLop { get; set; }

        public string? MaLopCode { get; set; }

        public string? TenLop { get; set; }

        public string TenKyThi { get; set; } = null!;

        public DateOnly? NgayThi { get; set; }

        public string? LoaiKyThi { get; set; }

        public decimal? DiemToiDa { get; set; }
    }
}