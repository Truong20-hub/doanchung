using DAL.Entities;

namespace DAL.Interfaces
{
    public interface IKyThiRepository
    {
        // =====================================================
        // 1. LẤY TẤT CẢ
        // =====================================================

        Task<IEnumerable<KyThi>> GetAllAsync();

        // =====================================================
        // 2. LẤY THEO ID
        // =====================================================

        Task<KyThi?> GetByIdAsync(int id);

        // =====================================================
        // 3. LẤY THEO MÃ LỚP
        // =====================================================

        Task<IEnumerable<KyThi>> GetByMaLopAsync(
            int maLop);

        // =====================================================
        // 4. LẤY THEO LOẠI KỲ THI
        // =====================================================

        Task<IEnumerable<KyThi>> GetByLoaiKyThiAsync(
            string loaiKyThi);

        // =====================================================
        // 5. TÌM KIẾM
        // =====================================================

        Task<IEnumerable<KyThi>> SearchAsync(
            string? keyword);

        // =====================================================
        // 6. PHÂN TRANG
        // =====================================================

        Task<object> GetPagedAsync(
            int pageNumber,
            int pageSize);

        // =====================================================
        // 7. TÌM KIẾM + PHÂN TRANG
        // =====================================================

        Task<object> SearchPagedAsync(
            string? keyword,
            int pageNumber,
            int pageSize);

        // =====================================================
        // 8. KIỂM TRA ID
        // =====================================================

        Task<bool> ExistsByIdAsync(int id);

        // =====================================================
        // 9. KIỂM TRA TRÙNG TÊN KỲ THI TRONG LỚP
        // =====================================================

        Task<bool> ExistsByTenKyThiAsync(
            int maLop,
            string tenKyThi,
            int? excludeId = null);

        // =====================================================
        // 10. THÊM
        // =====================================================

        Task<KyThi> AddAsync(KyThi kyThi);

        // =====================================================
        // 11. CẬP NHẬT
        // =====================================================

        Task<KyThi?> UpdateAsync(KyThi kyThi);

        // =====================================================
        // 12. XÓA
        // =====================================================

        Task<bool> DeleteAsync(int id);
    }
}