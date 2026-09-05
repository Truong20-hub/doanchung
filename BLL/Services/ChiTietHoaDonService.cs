using BLL.Interfaces;
using DAL.Entities;
using DAL.Interfaces;
using DTO.ChiTietHoaDon;

namespace BLL.Services
{
    public class ChiTietHoaDonService : IChiTietHoaDonService
    {
        private readonly AutoMapper.IMapper _mapper;
        private readonly IChiTietHoaDonRepository _repository;
        private readonly IHoaDonRepository _hoaDonRepository;

        public ChiTietHoaDonService(
            AutoMapper.IMapper mapper,
            IChiTietHoaDonRepository repository,
            IHoaDonRepository hoaDonRepository)
        {
            _mapper = mapper;
            _repository = repository;
            _hoaDonRepository = hoaDonRepository;
        }

        private ChiTietHoaDonResponse MapToResponse(ChiTietHoaDon entity)
        {
            return _mapper.Map<ChiTietHoaDonResponse>(entity);
        }

        public async Task<IEnumerable<ChiTietHoaDonResponse>> GetAllAsync()
        {
            var data = await _repository.GetAllAsync();
            return data.Select(MapToResponse);
        }

        public async Task<IEnumerable<ChiTietHoaDonResponse>> GetByMaHoaDonAsync(int maHoaDon)
        {
            var hoaDon = await _hoaDonRepository.GetByIdAsync(maHoaDon);
            if (hoaDon == null)
            {
                throw new KeyNotFoundException($"Hóa đơn có mã {maHoaDon} không tồn tại.");
            }

            var data = await _repository.GetByMaHoaDonAsync(maHoaDon);
            return data.Select(MapToResponse);
        }

        public async Task<ChiTietHoaDonResponse?> GetByIdAsync(int id)
        {
            var entity = await _repository.GetByIdAsync(id);
            if (entity == null)
                return null;

            return MapToResponse(entity);
        }

        public async Task<IEnumerable<ChiTietHoaDonResponse>> SearchAsync(string? keyword)
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

        public async Task<ChiTietHoaDonResponse> CreateAsync(CreateChiTietHoaDonRequest request)
        {
            var hoaDon = await _hoaDonRepository.GetByIdAsync(request.MaHoaDon);
            if (hoaDon == null)
            {
                throw new KeyNotFoundException($"Hóa đơn có mã {request.MaHoaDon} không tồn tại.");
            }

            if (request.SoLuong <= 0)
                throw new ArgumentException("Số lượng phải lớn hơn 0.");

            if (request.DonGia < 0)
                throw new ArgumentException("Đơn giá không được âm.");

            var entity = _mapper.Map<ChiTietHoaDon>(request);
            entity.MaHoaDon = request.MaHoaDon;
            entity.ThanhTien = request.SoLuong * request.DonGia;
            entity.MaHoaDonNavigation = hoaDon;

            var result = await _repository.AddAsync(entity);
            var created = await _repository.GetByIdAsync(result.MaChiTietHoaDon);
            return MapToResponse(created!);
        }

        public async Task<ChiTietHoaDonResponse?> UpdateAsync(int id, UpdateChiTietHoaDonRequest request)
        {
            var existing = await _repository.GetByIdAsync(id);
            if (existing == null)
                return null;

            var hoaDon = await _hoaDonRepository.GetByIdAsync(request.MaHoaDon);
            if (hoaDon == null)
            {
                throw new KeyNotFoundException($"Hóa đơn có mã {request.MaHoaDon} không tồn tại.");
            }

            if (request.SoLuong <= 0)
                throw new ArgumentException("Số lượng phải lớn hơn 0.");

            if (request.DonGia < 0)
                throw new ArgumentException("Đơn giá không được âm.");

            _mapper.Map(request, existing);
            existing.MaHoaDon = request.MaHoaDon;
            existing.ThanhTien = request.SoLuong * request.DonGia;
            existing.MaHoaDonNavigation = hoaDon;

            var updated = await _repository.UpdateAsync(existing);
            return updated == null ? null : MapToResponse(updated);
        }

        public async Task<bool> DeleteAsync(int id)
        {
            if (!await _repository.ExistsByIdAsync(id))
            {
                throw new KeyNotFoundException(
                    $"Chi tiết hóa đơn có mã {id} không tồn tại.");
            }

            return await _repository.DeleteAsync(id);
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
}
