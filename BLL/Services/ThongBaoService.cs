using AutoMapper;
using BLL.Interfaces;
using DAL.Entities;
using DAL.Interfaces;
using DTO.ThongBao;

namespace BLL.Services;

public class ThongBaoService : IThongBaoService
{
    private static readonly string[] ValidTypes =
    {
        "diem_danh",
        "diem_so",
        "lich_hoc",
        "hoc_phi",
        "tin_nhan",
        "he_thong"
    };

    private readonly IMapper _mapper;
    private readonly IThongBaoRepository _repository;
    private readonly INguoiDungRepository _nguoiDungRepository;
    private readonly IHocVienRepository _hocVienRepository;
    private readonly ILopHocRepository _lopHocRepository;
    private readonly IHoaDonRepository _hoaDonRepository;

    public ThongBaoService(
        IMapper mapper,
        IThongBaoRepository repository,
        INguoiDungRepository nguoiDungRepository,
        IHocVienRepository hocVienRepository,
        ILopHocRepository lopHocRepository,
        IHoaDonRepository hoaDonRepository)
    {
        _mapper = mapper;
        _repository = repository;
        _nguoiDungRepository = nguoiDungRepository;
        _hocVienRepository = hocVienRepository;
        _lopHocRepository = lopHocRepository;
        _hoaDonRepository = hoaDonRepository;
    }

    private ThongBaoResponse MapToResponse(ThongBao entity)
    {
        return _mapper.Map<ThongBaoResponse>(entity);
    }

    public async Task<IEnumerable<ThongBaoResponse>> GetAllAsync()
    {
        var data = await _repository.GetAllAsync();
        return data.Select(MapToResponse);
    }

    public async Task<ThongBaoResponse?> GetByIdAsync(int id)
    {
        var entity = await _repository.GetByIdAsync(id);
        return entity == null ? null : MapToResponse(entity);
    }

    public async Task<IEnumerable<ThongBaoResponse>> GetByMaNguoiDungAsync(int maNguoiDung)
    {
        await EnsureUserExistsAsync(maNguoiDung);
        var data = await _repository.GetByMaNguoiDungAsync(maNguoiDung);
        return data.Select(MapToResponse);
    }

    public async Task<IEnumerable<ThongBaoResponse>> GetUnreadAsync(int maNguoiDung)
    {
        await EnsureUserExistsAsync(maNguoiDung);
        var data = await _repository.GetUnreadAsync(maNguoiDung);
        return data.Select(MapToResponse);
    }

    public async Task<IEnumerable<ThongBaoResponse>> GetByLoaiAsync(string loaiThongBao)
    {
        var normalizedType = ValidateType(loaiThongBao);
        var data = await _repository.GetByLoaiAsync(normalizedType);
        return data.Select(MapToResponse);
    }

    public async Task<IEnumerable<ThongBaoResponse>> SearchAsync(string? keyword)
    {
        var data = await _repository.SearchAsync(keyword);
        return data.Select(MapToResponse);
    }

    public async Task<object> GetPagedAsync(int pageNumber, int pageSize)
    {
        ValidatePaging(pageNumber, pageSize);
        return await _repository.GetPagedAsync(pageNumber, pageSize);
    }

    public async Task<object> SearchPagedAsync(string? keyword, int pageNumber, int pageSize)
    {
        ValidatePaging(pageNumber, pageSize);
        return await _repository.SearchPagedAsync(keyword, pageNumber, pageSize);
    }

    public async Task<ThongBaoResponse> CreateAsync(CreateThongBaoRequest request)
    {
        var normalizedType = ValidateType(request.LoaiThongBao);
        await ValidateReferencesAsync(
            request.MaNguoiDung,
            request.MaHocVien,
            request.MaLop,
            request.MaHoaDon);

        var entity = _mapper.Map<ThongBao>(request);
        entity.LoaiThongBao = normalizedType;
        entity.ThoiGianTao = DateTime.Now;
        entity.DaDoc = false;

        var result = await _repository.AddAsync(entity);
        var created = await _repository.GetByIdAsync(result.MaThongBao);
        return MapToResponse(created!);
    }

    public async Task<ThongBaoResponse?> UpdateAsync(int id, UpdateThongBaoRequest request)
    {
        var existing = await _repository.GetByIdAsync(id);
        if (existing == null)
            throw new KeyNotFoundException($"Thông báo có mã {id} không tồn tại.");

        var normalizedType = ValidateType(request.LoaiThongBao);
        await ValidateReferencesAsync(
            request.MaNguoiDung,
            request.MaHocVien,
            request.MaLop,
            request.MaHoaDon);

        _mapper.Map(request, existing);
        existing.LoaiThongBao = normalizedType;
        var updated = await _repository.UpdateAsync(existing);
        if (updated == null)
            return null;

        var result = await _repository.GetByIdAsync(updated.MaThongBao);
        return MapToResponse(result!);
    }

    public async Task<bool> MarkAsReadAsync(int id)
    {
        if (!await _repository.ExistsByIdAsync(id))
            throw new KeyNotFoundException($"Thông báo có mã {id} không tồn tại.");

        return await _repository.MarkAsReadAsync(id);
    }

    public async Task<bool> DeleteAsync(int id)
    {
        if (!await _repository.ExistsByIdAsync(id))
            throw new KeyNotFoundException($"Thông báo có mã {id} không tồn tại.");

        return await _repository.DeleteAsync(id);
    }

    private async Task ValidateReferencesAsync(
        int maNguoiDung,
        int? maHocVien,
        int? maLop,
        int? maHoaDon)
    {
        await EnsureUserExistsAsync(maNguoiDung);

        if (maHocVien.HasValue && !await _hocVienRepository.ExistsByIdAsync(maHocVien.Value))
            throw new KeyNotFoundException($"Học viên có mã {maHocVien} không tồn tại.");

        if (maLop.HasValue && !await _lopHocRepository.ExistsByIdAsync(maLop.Value))
            throw new KeyNotFoundException($"Lớp học có mã {maLop} không tồn tại.");

        if (maHoaDon.HasValue && !await _hoaDonRepository.ExistsByIdAsync(maHoaDon.Value))
            throw new KeyNotFoundException($"Hóa đơn có mã {maHoaDon} không tồn tại.");
    }

    private async Task EnsureUserExistsAsync(int maNguoiDung)
    {
        if (!await _nguoiDungRepository.ExistsByIdAsync(maNguoiDung))
            throw new KeyNotFoundException($"Người dùng có mã {maNguoiDung} không tồn tại.");
    }

    private static string ValidateType(string loaiThongBao)
    {
        if (string.IsNullOrWhiteSpace(loaiThongBao))
            throw new ArgumentException("Loại thông báo không được để trống.");

        var normalizedType = loaiThongBao.Trim().ToLowerInvariant();
        if (!ValidTypes.Contains(normalizedType, StringComparer.Ordinal))
        {
            throw new ArgumentException(
                "Loại thông báo không hợp lệ. Chỉ chấp nhận: " +
                string.Join(", ", ValidTypes) + ".");
        }

        return normalizedType;
    }

    private static void ValidatePaging(int pageNumber, int pageSize)
    {
        if (pageNumber <= 0)
            throw new ArgumentException("PageNumber phải lớn hơn 0.");

        if (pageSize <= 0)
            throw new ArgumentException("PageSize phải lớn hơn 0.");

        if (pageSize > 100)
            throw new ArgumentException("PageSize không được lớn hơn 100.");
    }
}
