using DAL.Entities;

namespace DAL.Interfaces
{
    public interface ILichHocRepository
    {
        // =====================================================
        // LẤY TẤT CẢ
        // =====================================================

        Task<IEnumerable<LichHoc>> GetAllAsync();

        // =====================================================
        // LẤY THEO ID
        // =====================================================

        Task<LichHoc?> GetByIdAsync(int id);

        // =====================================================
        // LẤY THEO MÃ LỚP
        // =====================================================

        Task<IEnumerable<LichHoc>> GetByMaLopAsync(int maLop);

        // =====================================================
        // LẤY THEO THỨ
        // =====================================================

        Task<IEnumerable<LichHoc>> GetByThuAsync(
            string thuTrongTuan);

        // =====================================================
        // LẤY THEO LỚP + THỨ
        // =====================================================

        Task<IEnumerable<LichHoc>> GetByMaLopAndThuAsync(
            int maLop,
            string thuTrongTuan);

        // =====================================================
        // KIỂM TRA TỒN TẠI
        // =====================================================

        Task<bool> ExistsByIdAsync(int id);

        // =====================================================
        // KIỂM TRA TRÙNG LỊCH
        // =====================================================

        Task<bool> ExistsScheduleAsync(
            int maLop,
            string thuTrongTuan,
            TimeOnly gioBatDau,
            TimeOnly gioKetThuc,
            int? excludeId = null);

        // =====================================================
        // THÊM
        // =====================================================

        Task<LichHoc> AddAsync(LichHoc lichHoc);

        // =====================================================
        // CẬP NHẬT
        // =====================================================

        Task<LichHoc?> UpdateAsync(LichHoc lichHoc);

        // =====================================================
        // XÓA
        // =====================================================

        Task<bool> DeleteAsync(int id);

        // =====================================================
        // PHÂN TRANG
        // =====================================================

        Task<object> GetPagedAsync(
            int pageNumber,
            int pageSize);

        // =====================================================
        // TÌM KIẾM
        // =====================================================

        Task<IEnumerable<LichHoc>> SearchAsync(
            string? keyword);

        // =====================================================
        // TÌM KIẾM + PHÂN TRANG
        // =====================================================

        Task<object> SearchPagedAsync(
            string? keyword,
            int pageNumber,
            int pageSize);
    }
}