namespace DTO.ThongBao;

public class ThongBaoResponse
{
    public int MaThongBao { get; set; }
    public int MaNguoiDung { get; set; }
    public string? TenNguoiDung { get; set; }
    public string LoaiThongBao { get; set; } = null!;
    public string TieuDe { get; set; } = null!;
    public string NoiDung { get; set; } = null!;
    public int? MaHocVien { get; set; }
    public string? HoTenHocVien { get; set; }
    public int? MaLop { get; set; }
    public string? TenLop { get; set; }
    public int? MaHoaDon { get; set; }
    public DateTime ThoiGianTao { get; set; }
    public bool DaDoc { get; set; }
}
