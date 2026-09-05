namespace DTO.TinNhan;

public class TinNhanResponse
{
    public int MaTinNhan { get; set; }
    public int MaNguoiGui { get; set; }
    public string? TenNguoiGui { get; set; }
    public int MaNguoiNhan { get; set; }
    public string? TenNguoiNhan { get; set; }
    public int MaHocVien { get; set; }
    public string? HoTenHocVien { get; set; }
    public string NoiDung { get; set; } = null!;
    public DateTime ThoiGianGui { get; set; }
    public bool DaDoc { get; set; }
}
