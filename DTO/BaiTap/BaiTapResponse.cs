namespace DTO.BaiTap
{
    public class BaiTapResponse
    {
        public int MaBaiTap { get; set; }

        public int MaLop { get; set; }

        public string? MaLopCode { get; set; }

        public string? TenLop { get; set; }

        public int? MaBuoi { get; set; }

        public string? NoiDungBuoi { get; set; }

        public int MaGiaoVien { get; set; }

        public string? HoTenGiaoVien { get; set; }

        public string TieuDe { get; set; } = null!;

        public string? MoTa { get; set; }

        public DateOnly NgayGiao { get; set; }

        public DateOnly? HanNop { get; set; }

        // Đường dẫn / URL tới tệp đính kèm
        public string? FileDinhKem { get; set; }

        // Chỉ có giá trị ở API theo học viên (/hoc-vien/{id})
        public string? TrangThaiNop { get; set; }

        public decimal? DiemSo { get; set; }

        public DateTime? NgayNop { get; set; }

        public string? NhanXet { get; set; }

        public string? GhiChuNop { get; set; }

        public List<BaiTapSubmissionFileResponse>? FilesNop { get; set; }
    }
}
