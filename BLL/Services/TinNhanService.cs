using AutoMapper;
using BLL.Interfaces;
using DAL.Entities;
using DAL.Interfaces;
using DTO.TinNhan;

namespace BLL.Services;

public class TinNhanService : ITinNhanService
{
    private readonly IMapper _mapper;
    private readonly ITinNhanRepository _repository;
    private readonly INguoiDungRepository _nguoiDungRepository;
    private readonly IHocVienRepository _hocVienRepository;
    private readonly IGiaoVienRepository _giaoVienRepository;

    public TinNhanService(
        IMapper mapper,
        ITinNhanRepository repository,
        INguoiDungRepository nguoiDungRepository,
        IHocVienRepository hocVienRepository,
        IGiaoVienRepository giaoVienRepository)
    {
        _mapper = mapper;
        _repository = repository;
        _nguoiDungRepository = nguoiDungRepository;
        _hocVienRepository = hocVienRepository;
        _giaoVienRepository = giaoVienRepository;
    }

    private TinNhanResponse MapToResponse(TinNhan entity)
    {
        return _mapper.Map<TinNhanResponse>(entity);
    }

    public async Task<IEnumerable<TinNhanResponse>> GetAllAsync()
    {
        var data = await _repository.GetAllAsync();
        return data.Select(MapToResponse);
    }

    public async Task<TinNhanResponse?> GetByIdAsync(int id)
    {
        var entity = await _repository.GetByIdAsync(id);
        return entity == null ? null : MapToResponse(entity);
    }

    public async Task<IEnumerable<TinNhanResponse>> GetByMaNguoiDungAsync(int maNguoiDung)
    {
        await EnsureUserExistsAsync(maNguoiDung);
        var data = await _repository.GetByMaNguoiDungAsync(maNguoiDung);
        return data.Select(MapToResponse);
    }

    public async Task<IEnumerable<TinNhanResponse>> GetConversationAsync(
        int maNguoiDung1,
        int maNguoiDung2,
        int maHocVien)
    {
        await EnsureUserExistsAsync(maNguoiDung1);
        await EnsureUserExistsAsync(maNguoiDung2);
        await EnsureStudentExistsAsync(maHocVien);

        var data = await _repository.GetConversationAsync(
            maNguoiDung1,
            maNguoiDung2,
            maHocVien);
        return data.Select(MapToResponse);
    }

    public async Task<IEnumerable<TinNhanResponse>> SearchAsync(string? keyword)
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

    public async Task<TinNhanResponse> CreateAsync(CreateTinNhanRequest request)
    {
        await ValidateParticipantsAsync(
            request.MaNguoiGui,
            request.MaNguoiNhan,
            request.MaHocVien);

        var entity = _mapper.Map<TinNhan>(request);
        entity.ThoiGianGui = DateTime.Now;
        entity.DaDoc = false;

        var result = await _repository.AddAsync(entity);
        var created = await _repository.GetByIdAsync(result.MaTinNhan);
        return MapToResponse(created!);
    }

    public async Task<TinNhanResponse?> UpdateAsync(int id, UpdateTinNhanRequest request)
    {
        var existing = await _repository.GetByIdAsync(id);
        if (existing == null)
            throw new KeyNotFoundException($"Tin nhắn có mã {id} không tồn tại.");

        await ValidateParticipantsAsync(
            request.MaNguoiGui,
            request.MaNguoiNhan,
            request.MaHocVien);

        _mapper.Map(request, existing);
        var updated = await _repository.UpdateAsync(existing);
        if (updated == null)
            return null;

        var result = await _repository.GetByIdAsync(updated.MaTinNhan);
        return MapToResponse(result!);
    }

    public async Task<bool> DeleteAsync(int id)
    {
        if (!await _repository.ExistsByIdAsync(id))
            throw new KeyNotFoundException($"Tin nhắn có mã {id} không tồn tại.");

        return await _repository.DeleteAsync(id);
    }

    private async Task ValidateParticipantsAsync(int maNguoiGui, int maNguoiNhan, int maHocVien)
    {
        if (maNguoiGui == maNguoiNhan)
            throw new ArgumentException("Người gửi và người nhận không được trùng nhau.");

        await EnsureUserExistsAsync(maNguoiGui);
        await EnsureUserExistsAsync(maNguoiNhan);
        await EnsureStudentExistsAsync(maHocVien);

        if (!await _giaoVienRepository.ExistsByNguoiDungIdAsync(maNguoiNhan))
        {
            throw new KeyNotFoundException(
                $"Người nhận có mã {maNguoiNhan} không phải là giáo viên.");
        }
    }

    private async Task EnsureUserExistsAsync(int maNguoiDung)
    {
        if (!await _nguoiDungRepository.ExistsByIdAsync(maNguoiDung))
        {
            throw new KeyNotFoundException(
                $"Người dùng có mã {maNguoiDung} không tồn tại.");
        }
    }

    private async Task EnsureStudentExistsAsync(int maHocVien)
    {
        if (!await _hocVienRepository.ExistsByIdAsync(maHocVien))
        {
            throw new KeyNotFoundException(
                $"Học viên có mã {maHocVien} không tồn tại.");
        }
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
