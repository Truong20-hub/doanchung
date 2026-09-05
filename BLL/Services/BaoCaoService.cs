using BLL.Interfaces;
using DAL.Interfaces;
using DTO.BaoCao;

namespace BLL.Services;

public class BaoCaoService : IBaoCaoService
{
    private readonly IBaoCaoRepository _repository;
    private readonly ILopHocRepository _lopHocRepository;
    private readonly IHocVienRepository _hocVienRepository;

    public BaoCaoService(
        IBaoCaoRepository repository,
        ILopHocRepository lopHocRepository,
        IHocVienRepository hocVienRepository)
    {
        _repository = repository;
        _lopHocRepository = lopHocRepository;
        _hocVienRepository = hocVienRepository;
    }

    public async Task<IEnumerable<BaoCaoDiemResponse>> GetBaoCaoDiemAsync(
        int? maLop,
        int? maHocVien)
    {
        await ValidateClassAndStudentAsync(maLop, maHocVien);
        return await _repository.GetBaoCaoDiemAsync(maLop, maHocVien);
    }

    public async Task<IEnumerable<BaoCaoChuyenCanResponse>> GetBaoCaoChuyenCanAsync(
        int? maLop)
    {
        if (maLop.HasValue)
            await EnsureClassExistsAsync(maLop.Value);

        return await _repository.GetBaoCaoChuyenCanAsync(maLop);
    }

    public async Task<IEnumerable<BaoCaoHocPhiResponse>> GetBaoCaoHocPhiAsync(
        int nam,
        int? thang)
    {
        ValidatePeriod(nam, thang);
        return await _repository.GetBaoCaoHocPhiAsync(nam, thang);
    }

    private async Task ValidateClassAndStudentAsync(int? maLop, int? maHocVien)
    {
        if (maLop.HasValue)
            await EnsureClassExistsAsync(maLop.Value);

        if (maHocVien.HasValue && !await _hocVienRepository.ExistsByIdAsync(maHocVien.Value))
        {
            throw new KeyNotFoundException(
                $"Học viên có mã {maHocVien} không tồn tại.");
        }
    }

    private async Task EnsureClassExistsAsync(int maLop)
    {
        if (!await _lopHocRepository.ExistsByIdAsync(maLop))
        {
            throw new KeyNotFoundException(
                $"Lớp học có mã {maLop} không tồn tại.");
        }
    }

    private static void ValidatePeriod(int nam, int? thang)
    {
        if (nam < 1 || nam > 9999)
            throw new ArgumentException("Năm báo cáo không hợp lệ.");

        if (thang.HasValue && (thang.Value < 1 || thang.Value > 12))
            throw new ArgumentException("Tháng báo cáo phải nằm trong khoảng từ 1 đến 12.");
    }
}
