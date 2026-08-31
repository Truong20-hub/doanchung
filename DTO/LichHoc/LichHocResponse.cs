namespace DTO.LichHoc
{
    public class LichHocResponse
    {
        public int MaLich { get; set; }

        public int MaLop { get; set; }

        public string? MaLopCode { get; set; }

        public string? TenLop { get; set; }

        public string ThuTrongTuan { get; set; } = null!;

        public TimeOnly GioBatDau { get; set; }

        public TimeOnly GioKetThuc { get; set; }
    }
}