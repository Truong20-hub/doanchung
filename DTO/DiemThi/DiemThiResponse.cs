namespace DTO.DiemThi;

public class DiemThiResponse
{
    public int MaDiemThi { get; set; }

    public int MaKyThi { get; set; }
    public string? TenKyThi { get; set; } // đổi theo tên thuộc tính thật của KyThi

    public int MaHocVien { get; set; }
    public string? TenHocVien { get; set; } // đổi theo tên thuộc tính thật của HocVien

    public decimal? DiemNghe { get; set; }
    public decimal? DiemNoi { get; set; }
    public decimal? DiemDoc { get; set; }
    public decimal? DiemViet { get; set; }
    public decimal? TongDiem { get; set; }

    public string? NhanXet { get; set; }
}