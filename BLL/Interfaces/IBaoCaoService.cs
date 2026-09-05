using DTO.BaoCao;

namespace BLL.Interfaces;

public interface IBaoCaoService
{
    Task<IEnumerable<BaoCaoDiemResponse>> GetBaoCaoDiemAsync(
        int? maLop,
        int? maHocVien);

    Task<IEnumerable<BaoCaoChuyenCanResponse>> GetBaoCaoChuyenCanAsync(
        int? maLop);

    Task<IEnumerable<BaoCaoHocPhiResponse>> GetBaoCaoHocPhiAsync(
        int nam,
        int? thang);
}
