using DAL.Entities;

namespace DAL.Interfaces
{
    public interface IHoaDonRepository
    {
        // =====================================================
        // 1. LẤY TẤT CẢ
        // =====================================================

        Task<IEnumerable<HoaDon>> GetAllAsync();

        // =====================================================
        // 2. LẤY THEO ID
        // =====================================================

        Task<HoaDon?> GetByIdAsync(int id);

        // =====================================================
        // 3. LẤY THEO HỌC VIÊN
        // =====================================================

        Task<IEnumerable<HoaDon>> GetByMaHocVienAsync(
            int maHocVien);

        // =====================================================
        // 4. LẤY THEO LỚP
        // =====================================================

        Task<IEnumerable<HoaDon>> GetByMaLopAsync(
            int maLop);

        // =====================================================
        // 5. LẤY THEO TRẠNG THÁI
        // =====================================================

        Task<IEnumerable<HoaDon>> GetByTrangThaiAsync(
            string trangThai);

        // =====================================================
        // 6. TÌM KIẾM
        // =====================================================

        Task<IEnumerable<HoaDon>> SearchAsync(
            string? keyword);

        // =====================================================
        // 7. PHÂN TRANG
        // =====================================================

        Task<object> GetPagedAsync(
            int pageNumber,
            int pageSize);

        // =====================================================
        // 8. TÌM KIẾM + PHÂN TRANG
        // =====================================================

        Task<object> SearchPagedAsync(
            string? keyword,
            int pageNumber,
            int pageSize);

        // =====================================================
        // 9. KIỂM TRA ID
        // =====================================================

        Task<bool> ExistsByIdAsync(int id);

        // =====================================================
        // 10. KIỂM TRA THANH TOÁN
        // =====================================================

        Task<bool> HasThanhToanAsync(
            int maHoaDon);

        // =====================================================
        // 11. THÊM
        // =====================================================

        Task<HoaDon> AddAsync(
            HoaDon hoaDon);

        // =====================================================
        // 12. CẬP NHẬT
        // =====================================================

        Task<HoaDon?> UpdateAsync(
            HoaDon hoaDon);

        // =====================================================
        // 13. XÓA
        // =====================================================

        Task<bool> DeleteAsync(int id);
    }
}