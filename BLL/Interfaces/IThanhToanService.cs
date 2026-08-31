using DTO.ThanhToan;

namespace BLL.Interfaces
{
    public interface IThanhToanService
    {
        // GET
        Task<IEnumerable<ThanhToanResponse>>
            GetAllAsync();

        Task<ThanhToanResponse?>
            GetByIdAsync(int id);

        Task<IEnumerable<ThanhToanResponse>>
            GetByMaHoaDonAsync(int maHoaDon);

        // PAGING
        Task<object> GetPagedAsync(
            int pageNumber,
            int pageSize);

        // CREATE
        Task<ThanhToanResponse>
            CreateAsync(
                CreateThanhToanRequest request);

        // UPDATE
        Task<ThanhToanResponse?>
            UpdateAsync(
                int id,
                UpdateThanhToanRequest request);

        // DELETE
        Task<bool> DeleteAsync(int id);
    }
}