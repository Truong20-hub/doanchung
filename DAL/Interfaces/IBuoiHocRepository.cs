using DAL.Entities;

namespace DAL.Interfaces
{
    public interface IBuoiHocRepository
    {
        // =========================================
        // 1. LẤY TẤT CẢ BUỔI HỌC
        // =========================================
        Task<IEnumerable<BuoiHoc>> GetAllAsync();

        // =========================================
        // 2. LẤY BUỔI HỌC THEO ID
        // =========================================
        Task<BuoiHoc?> GetByIdAsync(int id);

        // =========================================
        // 3. KIỂM TRA BUỔI HỌC CÓ TỒN TẠI
        // =========================================
        Task<bool> ExistsByIdAsync(int id);

        // =========================================
        // 4. KIỂM TRA LỚP HỌC CÓ TỒN TẠI
        // =========================================
        Task<bool> ExistsByLopIdAsync(int maLop);

        // =========================================
        // 5. LẤY BUỔI HỌC THEO MÃ LỚP
        // =========================================
        Task<IEnumerable<BuoiHoc>> GetByLopIdAsync(
            int maLop);

        // =========================================
        // 6. LẤY BUỔI HỌC THEO NGÀY
        // =========================================
        Task<IEnumerable<BuoiHoc>> GetByDateAsync(
            DateOnly ngayHoc);

        // =========================================
        // 7. LẤY BUỔI HỌC THEO TRẠNG THÁI HỦY
        // =========================================
        Task<IEnumerable<BuoiHoc>> GetByBiHuyAsync(
            bool biHuy);

        // =========================================
        // 8. TÌM KIẾM THEO NỘI DUNG
        // =========================================
        Task<IEnumerable<BuoiHoc>> GetByNoiDungAsync(
            string noiDung);

        // =========================================
        // 9. TÌM KIẾM
        // mã lớp + nội dung
        // =========================================
        Task<IEnumerable<BuoiHoc>> SearchAsync(
            int? maLop,
            string? noiDung,
            DateOnly? ngayHoc,
            bool? biHuy);

        // =========================================
        // 10. PHÂN TRANG
        // =========================================
        Task<object> GetPagedAsync(
            int pageNumber,
            int pageSize);

        // =========================================
        // 11. MÃ LỚP + PHÂN TRANG
        // =========================================
        Task<object> GetPagedByLopIdAsync(
            int maLop,
            int pageNumber,
            int pageSize);

        // =========================================
        // 12. NGÀY HỌC + PHÂN TRANG
        // =========================================
        Task<object> GetPagedByDateAsync(
            DateOnly ngayHoc,
            int pageNumber,
            int pageSize);

        // =========================================
        // 13. TÌM KIẾM + PHÂN TRANG
        // =========================================
        Task<object> SearchPagedAsync(
            int? maLop,
            string? noiDung,
            DateOnly? ngayHoc,
            bool? biHuy,
            int pageNumber,
            int pageSize);

        // =========================================
        // 14. THÊM BUỔI HỌC
        // =========================================
        Task<BuoiHoc> AddAsync(
            BuoiHoc buoiHoc);

        // =========================================
        // 15. CẬP NHẬT
        // =========================================
        Task<BuoiHoc?> UpdateAsync(
            BuoiHoc buoiHoc);

        // =========================================
        // 16. XÓA
        // =========================================
        Task<bool> DeleteAsync(int id);
    }
}