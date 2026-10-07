namespace DTO.BaiTap
{
    public class BaiTapSubmissionResponse
    {
        public int MaHocVien { get; set; }
        public string HoTenHocVien { get; set; } = null!;
        public string TrangThaiNop { get; set; } = "ChuaNop";
        public DateTime? NgayNop { get; set; }
        public string? GhiChuNop { get; set; }
        public List<BaiTapSubmissionFileResponse>? FilesNop { get; set; }
        public decimal? DiemSo { get; set; }
        public string? NhanXet { get; set; }
    }
}
