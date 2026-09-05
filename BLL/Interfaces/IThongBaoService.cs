using DTO.ThongBao;

namespace BLL.Interfaces;

public interface IThongBaoService
{
    Task<IEnumerable<ThongBaoResponse>> GetAllAsync();
    Task<ThongBaoResponse?> GetByIdAsync(int id);
    Task<IEnumerable<ThongBaoResponse>> GetByMaNguoiDungAsync(int maNguoiDung);
    Task<IEnumerable<ThongBaoResponse>> GetUnreadAsync(int maNguoiDung);
    Task<IEnumerable<ThongBaoResponse>> GetByLoaiAsync(string loaiThongBao);
    Task<IEnumerable<ThongBaoResponse>> SearchAsync(string? keyword);
    Task<object> GetPagedAsync(int pageNumber, int pageSize);
    Task<object> SearchPagedAsync(string? keyword, int pageNumber, int pageSize);
    Task<ThongBaoResponse> CreateAsync(CreateThongBaoRequest request);
    Task<ThongBaoResponse?> UpdateAsync(int id, UpdateThongBaoRequest request);
    Task<bool> MarkAsReadAsync(int id);
    Task<bool> DeleteAsync(int id);
}
