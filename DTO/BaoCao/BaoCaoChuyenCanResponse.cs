namespace DTO.BaoCao;

public class BaoCaoChuyenCanResponse
{
    public int MaLop { get; set; }
    public string MaLopCode { get; set; } = null!;
    public string TenLop { get; set; } = null!;
    public int TongSoBuoi { get; set; }
    public int TongLuotDiemDanh { get; set; }
    public int SoLuotCoMat { get; set; }
    public int SoLuotTre { get; set; }
    public int SoLuotVang { get; set; }
    public int SoLuotVangCoPhep { get; set; }
    public decimal TyLeThamDu { get; set; }
}
