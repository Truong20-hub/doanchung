using DAL.Entities;

namespace DAL.Interfaces
{
    public interface IThanhToanRepository
    {
        // =====================================================
        // 1. LẤY TẤT CẢ
        // =====================================================

        Task<IEnumerable<ThanhToan>> GetAllAsync();

        // =====================================================
        // 2. LẤY THEO ID
        // =====================================================

        Task<ThanhToan?> GetByIdAsync(int id);

        // =====================================================
        // 3. LẤY THEO HÓA ĐƠN
        // =====================================================

        Task<IEnumerable<ThanhToan>> GetByMaHoaDonAsync(
            int maHoaDon);

        // =====================================================
        // 4. TỔNG TIỀN ĐÃ THANH TOÁN
        // =====================================================

        Task<decimal> GetTongDaThanhToanAsync(
            int maHoaDon);

        // =====================================================
        // 5. KIỂM TRA THANH TOÁN
        // =====================================================

        Task<bool> ExistsByIdAsync(int id);

        // =====================================================
        // 6. PHÂN TRANG
        // =====================================================

        Task<object> GetPagedAsync(
            int pageNumber,
            int pageSize);

        // =====================================================
        // 7. THÊM
        // =====================================================

        Task<ThanhToan> AddAsync(
            ThanhToan thanhToan);

        // =====================================================
        // 8. CẬP NHẬT
        // =====================================================

        Task<ThanhToan?> UpdateAsync(
            ThanhToan thanhToan);

        // =====================================================
        // 9. XÓA
        // =====================================================

        Task<bool> DeleteAsync(int id);
    }
}