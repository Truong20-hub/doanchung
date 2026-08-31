namespace DTO.BuoiHoc
{
    public class BuoiHocResponse
    {
        public int MaBuoi { get; set; }

        public int MaLop { get; set; }

        public string? MaLopCode { get; set; }

        public string? TenLop { get; set; }

        public DateOnly NgayHoc { get; set; }

        public TimeOnly? GioBatDau { get; set; }

        public TimeOnly? GioKetThuc { get; set; }

        public string? NoiDung { get; set; }

        public bool? BiHuy { get; set; }
    }
}