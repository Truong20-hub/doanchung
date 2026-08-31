using DAL.Entities;

namespace DAL.Interfaces
{
    public interface IDiemDanhRepository
    {
        // =====================================================
        // 1. LẤY TẤT CẢ
        // =====================================================

        Task<IEnumerable<DiemDanh>> GetAllAsync();

        // =====================================================
        // 2. LẤY THEO MÃ
        // =====================================================

        Task<DiemDanh?> GetByIdAsync(int id);

        // =====================================================
        // 3. LẤY THEO BUỔI HỌC
        // =====================================================

        Task<IEnumerable<DiemDanh>> GetByMaBuoiAsync(
            int maBuoi);

        // =====================================================
        // 4. LẤY THEO HỌC VIÊN
        // =====================================================

        Task<IEnumerable<DiemDanh>> GetByMaHocVienAsync(
            int maHocVien);

        // =====================================================
        // 5. KIỂM TRA ĐÃ ĐIỂM DANH CHƯA
        // =====================================================

        Task<bool> ExistsAsync(
            int maBuoi,
            int maHocVien);

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
            int? maBuoi,
            int? maHocVien,
            string? trangThai,
            int pageNumber,
            int pageSize);

        // =====================================================
        // 8. THÊM
        // =====================================================

        Task<DiemDanh> AddAsync(
            DiemDanh diemDanh);

        // =====================================================
        // 9. CẬP NHẬT
        // =====================================================

        Task<DiemDanh?> UpdateAsync(
            DiemDanh diemDanh);

        // =====================================================
        // 10. XÓA
        // =====================================================

        Task<bool> DeleteAsync(int id);
    }
}