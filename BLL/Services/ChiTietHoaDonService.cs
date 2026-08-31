using BLL.Interfaces;
using DAL.Entities;
using DAL.Interfaces;
using DTO.ChiTietHoaDon;

namespace BLL.Services
{
    public class ChiTietHoaDonService : IChiTietHoaDonService
    {
        private readonly IChiTietHoaDonRepository _repository;
        private readonly IHoaDonRepository _hoaDonRepository;

        public ChiTietHoaDonService(
            IChiTietHoaDonRepository repository,
            IHoaDonRepository hoaDonRepository)
        {
            _repository = repository;
            _hoaDonRepository = hoaDonRepository;
        }

        private static ChiTietHoaDonResponse MapToResponse(ChiTietHoaDon entity)
        {
            return new ChiTietHoaDonResponse
            {
                MaChiTietHoaDon = entity.MaChiTietHoaDon,
                MaHoaDon = entity.MaHoaDon,
                SoLuong = entity.SoLuong,
                DonGia = entity.DonGia,
                ThanhTien = entity.ThanhTien,
                GhiChu = entity.GhiChu,
                MaHocVien = entity.MaHoaDonNavigation?.MaHocVien,
                HoTenHocVien = entity.MaHoaDonNavigation?.MaHocVienNavigation?.HoTen,
                MaLop = entity.MaHoaDonNavigation?.MaLop,
                TenLop = entity.MaHoaDonNavigation?.MaLopNavigation?.TenLop,
                TrangThaiHoaDon = entity.MaHoaDonNavigation?.TrangThai
            };
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

            var entity = new ChiTietHoaDon
            {
                MaHoaDon = request.MaHoaDon,
                SoLuong = request.SoLuong,
                DonGia = request.DonGia,
                ThanhTien = request.SoLuong * request.DonGia,
                GhiChu = request.GhiChu,
                MaHoaDonNavigation = hoaDon
            };

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

            existing.MaHoaDon = request.MaHoaDon;
            existing.SoLuong = request.SoLuong;
            existing.DonGia = request.DonGia;
            existing.ThanhTien = request.SoLuong * request.DonGia;
            existing.GhiChu = request.GhiChu;
            existing.MaHoaDonNavigation = hoaDon;

            var updated = await _repository.UpdateAsync(existing);
            return updated == null ? null : MapToResponse(updated);
        }

        public async Task<bool> DeleteAsync(int id)
        {
            return await _repository.DeleteAsync(id);
        }
    }
}
