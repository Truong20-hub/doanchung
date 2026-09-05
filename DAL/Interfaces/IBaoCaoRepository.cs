using DTO.BaoCao;

namespace DAL.Interfaces;

public interface IBaoCaoRepository
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
