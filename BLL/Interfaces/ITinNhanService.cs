using DTO.TinNhan;

namespace BLL.Interfaces;

public interface ITinNhanService
{
    Task<IEnumerable<TinNhanResponse>> GetAllAsync();
    Task<TinNhanResponse?> GetByIdAsync(int id);
    Task<IEnumerable<TinNhanResponse>> GetByMaNguoiDungAsync(int maNguoiDung);
    Task<IEnumerable<TinNhanResponse>> GetConversationAsync(int maNguoiDung1, int maNguoiDung2, int maHocVien);
    Task<IEnumerable<TinNhanResponse>> SearchAsync(string? keyword);
    Task<object> GetPagedAsync(int pageNumber, int pageSize);
    Task<object> SearchPagedAsync(string? keyword, int pageNumber, int pageSize);
    Task<TinNhanResponse> CreateAsync(CreateTinNhanRequest request);
    Task<TinNhanResponse?> UpdateAsync(int id, UpdateTinNhanRequest request);
    Task<bool> DeleteAsync(int id);
}
