using DAL.Entities;

namespace DAL.Interfaces;

public interface IThongBaoRepository
{
    Task<IEnumerable<ThongBao>> GetAllAsync();
    Task<ThongBao?> GetByIdAsync(int id);
    Task<IEnumerable<ThongBao>> GetByMaNguoiDungAsync(int maNguoiDung);
    Task<IEnumerable<ThongBao>> GetUnreadAsync(int maNguoiDung);
    Task<IEnumerable<ThongBao>> GetByLoaiAsync(string loaiThongBao);
    Task<IEnumerable<ThongBao>> SearchAsync(string? keyword);
    Task<object> GetPagedAsync(int pageNumber, int pageSize);
    Task<object> SearchPagedAsync(string? keyword, int pageNumber, int pageSize);
    Task<bool> ExistsByIdAsync(int id);
    Task<ThongBao> AddAsync(ThongBao thongBao);
    Task<ThongBao?> UpdateAsync(ThongBao thongBao);
    Task<bool> MarkAsReadAsync(int id);
    Task<bool> DeleteAsync(int id);
}
