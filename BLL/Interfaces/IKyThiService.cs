using DTO.KyThi;

namespace BLL.Interfaces
{
    public interface IKyThiService
    {
        Task<IEnumerable<KyThiResponse>> GetAllAsync();

        Task<KyThiResponse?> GetByIdAsync(int id);

        Task<IEnumerable<KyThiResponse>>
            GetByMaLopAsync(int maLop);

        Task<IEnumerable<KyThiResponse>>
            GetByLoaiKyThiAsync(string loaiKyThi);

        Task<IEnumerable<KyThiResponse>>
            SearchAsync(string? keyword);

        Task<object> GetPagedAsync(
            int pageNumber,
            int pageSize);

        Task<object> SearchPagedAsync(
            string? keyword,
            int pageNumber,
            int pageSize);

        Task<KyThiResponse> CreateAsync(
            CreateKyThiRequest request);

        Task<KyThiResponse?> UpdateAsync(
            int id,
            UpdateKyThiRequest request);

        Task<bool> DeleteAsync(int id);
    }
}