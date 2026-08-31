namespace DTO.DiemDanh
{
    public class DiemDanhResponse
    {
        public int MaDiemDanh { get; set; }

        public int MaBuoi { get; set; }

        public DateOnly NgayHoc { get; set; }

        public TimeOnly? GioBatDau { get; set; }

        public TimeOnly? GioKetThuc { get; set; }

        public int MaLop { get; set; }

        public string MaLopCode { get; set; } = null!;

        public string TenLop { get; set; } = null!;

        public int MaHocVien { get; set; }

        public string TrangThai { get; set; } = null!;

        public string? GhiChu { get; set; }
    }
}