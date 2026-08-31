namespace DTO.PhongHoc
{
    public class PhongHocResponse
    {
        public int MaPhong { get; set; }

        public string TenPhong { get; set; } = null!;

        public int? SucChua { get; set; }

        public string? ViTri { get; set; }
    }
}