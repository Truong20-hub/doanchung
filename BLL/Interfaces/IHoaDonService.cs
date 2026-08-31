using DTO.HoaDon;

namespace BLL.Interfaces
{
    public interface IHoaDonService
    {
        Task<IEnumerable<HoaDonResponse>>
            GetAllAsync();

        Task<HoaDonResponse?>
            GetByIdAsync(int id);

        Task<IEnumerable<HoaDonResponse>>
            GetByMaHocVienAsync(int maHocVien);

        Task<IEnumerable<HoaDonResponse>>
            GetByMaLopAsync(int maLop);

        Task<IEnumerable<HoaDonResponse>>
            GetByTrangThaiAsync(string trangThai);

        Task<IEnumerable<HoaDonResponse>>
            SearchAsync(string? keyword);

        Task<object> GetPagedAsync(
            int pageNumber,
            int pageSize);

        Task<object> SearchPagedAsync(
            string? keyword,
            int pageNumber,
            int pageSize);

        Task<HoaDonResponse> CreateAsync(
            CreateHoaDonRequest request);

        Task<HoaDonResponse?> UpdateAsync(
            int id,
            UpdateHoaDonRequest request);

        Task<bool> DeleteAsync(int id);
    }
}