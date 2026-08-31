using DAL.Entities;

namespace DAL.Interfaces
{
    public interface IChiTietHoaDonRepository
    {
        // =====================================================
        // 1. LẤY TẤT CẢ
        // =====================================================

        Task<IEnumerable<ChiTietHoaDon>> GetAllAsync();

        // =====================================================
        // 2. LẤY THEO ID
        // =====================================================

        Task<ChiTietHoaDon?> GetByIdAsync(int id);

        // =====================================================
        // 3. LẤY THEO MÃ HÓA ĐƠN
        // =====================================================

        Task<IEnumerable<ChiTietHoaDon>> GetByMaHoaDonAsync(int maHoaDon);

        // =====================================================
        // 4. TÌM KIẾM
        // =====================================================

        Task<IEnumerable<ChiTietHoaDon>> SearchAsync(string? keyword);

        // =====================================================
        // 5. KIỂM TRA ID
        // =====================================================

        Task<bool> ExistsByIdAsync(int id);

        // =====================================================
        // 6. THÊM
        // =====================================================

        Task<ChiTietHoaDon> AddAsync(ChiTietHoaDon chiTietHoaDon);

        // =====================================================
        // 7. CẬP NHẬT
        // =====================================================

        Task<ChiTietHoaDon?> UpdateAsync(ChiTietHoaDon chiTietHoaDon);

        // =====================================================
        // 8. XÓA
        // =====================================================

        Task<bool> DeleteAsync(int id);
    }
}
