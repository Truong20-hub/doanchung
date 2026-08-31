using DTO.LichHoc;

namespace BLL.Interfaces
{
    public interface ILichHocService
    {
        Task<IEnumerable<LichHocResponse>> GetAllAsync();

        Task<LichHocResponse?> GetByIdAsync(int id);

        Task<IEnumerable<LichHocResponse>> GetByMaLopAsync(
            int maLop);

        Task<IEnumerable<LichHocResponse>> GetByThuAsync(
            string thuTrongTuan);

        Task<IEnumerable<LichHocResponse>>
            GetByMaLopAndThuAsync(
                int maLop,
                string thuTrongTuan);

        Task<LichHocResponse> CreateAsync(
            CreateLichHocRequest request);

        Task<LichHocResponse?> UpdateAsync(
            int id,
            UpdateLichHocRequest request);

        Task<bool> DeleteAsync(int id);

        Task<object> GetPagedAsync(
            int pageNumber,
            int pageSize);

        Task<IEnumerable<LichHocResponse>> SearchAsync(
            string? keyword);

        Task<object> SearchPagedAsync(
            string? keyword,
            int pageNumber,
            int pageSize);
    }
}