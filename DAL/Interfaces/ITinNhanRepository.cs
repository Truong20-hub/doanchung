using DAL.Entities;
using DAL.Model;

namespace DAL.Interfaces;

public interface ITinNhanRepository
{
    Task<IEnumerable<TinNhan>> GetAllAsync();
    Task<TinNhan?> GetByIdAsync(int id);
    Task<IEnumerable<TinNhan>> GetByMaNguoiDungAsync(int maNguoiDung);
    Task<IEnumerable<TinNhan>> GetConversationAsync(int maNguoiDung1, int maNguoiDung2, int maHocVien);
    Task<IEnumerable<TinNhan>> SearchAsync(string? keyword);
    Task<object> GetPagedAsync(int pageNumber, int pageSize);
    Task<object> SearchPagedAsync(string? keyword, int pageNumber, int pageSize);
    Task<bool> ExistsByIdAsync(int id);
    Task<TinNhan> AddAsync(TinNhan tinNhan);
    Task<TinNhan?> UpdateAsync(TinNhan tinNhan);
    Task<NguoiGuiSummary[]> GetDanhSachNguoiGuiAsync();
    Task<NguoiGuiSummary[]> GetDanhSachNguoiGuiChoNguoiNhanAsync(int maNguoiNhan);
    Task<TinNhanItem[]> GetCuocTroChuyenAsync(int maNguoiA, int maNguoiB);
    Task<bool> DeleteAsync(int id);
}
