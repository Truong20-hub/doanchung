namespace DTO.BaoCao;

public class BaoCaoDiemResponse
{
    public int MaLop { get; set; }
    public string MaLopCode { get; set; } = null!;
    public string TenLop { get; set; } = null!;
    public int MaHocVien { get; set; }
    public string HoTenHocVien { get; set; } = null!;
    public int MaKyThi { get; set; }
    public string TenKyThi { get; set; } = null!;
    public DateOnly? NgayThi { get; set; }
    public decimal? DiemNghe { get; set; }
    public decimal? DiemNoi { get; set; }
    public decimal? DiemDoc { get; set; }
    public decimal? DiemViet { get; set; }
    public decimal? TongDiem { get; set; }
    public string? NhanXet { get; set; }
}
