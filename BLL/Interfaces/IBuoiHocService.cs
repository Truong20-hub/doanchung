using DTO.BuoiHoc;

namespace BLL.Interfaces
{
    public interface IBuoiHocService
    {
        // =========================================
        // 1. LẤY TẤT CẢ BUỔI HỌC
        // =========================================
        Task<IEnumerable<BuoiHocResponse>> GetAllAsync();

        // =========================================
        // 2. LẤY BUỔI HỌC THEO ID
        // =========================================
        Task<BuoiHocResponse?> GetByIdAsync(int id);

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
        Task<IEnumerable<BuoiHocResponse>> GetByLopIdAsync(
            int maLop);

        // =========================================
        // 6. LẤY BUỔI HỌC THEO NGÀY
        // =========================================
        Task<IEnumerable<BuoiHocResponse>> GetByDateAsync(
            DateOnly ngayHoc);

        // =========================================
        // 7. LẤY BUỔI HỌC THEO TRẠNG THÁI HỦY
        // =========================================
        Task<IEnumerable<BuoiHocResponse>> GetByBiHuyAsync(
            bool biHuy);

        // =========================================
        // 8. LẤY BUỔI HỌC THEO NỘI DUNG
        // =========================================
        Task<IEnumerable<BuoiHocResponse>> GetByNoiDungAsync(
            string noiDung);

        // =========================================
        // 9. TÌM KIẾM
        // Mã lớp + nội dung + ngày + trạng thái
        // =========================================
        Task<IEnumerable<BuoiHocResponse>> SearchAsync(
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
        // 11. THEO MÃ LỚP + PHÂN TRANG
        // =========================================
        Task<object> GetPagedByLopIdAsync(
            int maLop,
            int pageNumber,
            int pageSize);

        // =========================================
        // 12. THEO NGÀY + PHÂN TRANG
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
        // 14. TẠO BUỔI HỌC
        // =========================================
        Task<BuoiHocResponse> CreateAsync(
            CreateBuoiHocRequest request);

        // =========================================
        // 15. CẬP NHẬT BUỔI HỌC
        // =========================================
        Task<BuoiHocResponse?> UpdateAsync(
            int id,
            UpdateBuoiHocRequest request);

        // =========================================
        // 16. XÓA BUỔI HỌC
        // =========================================
        Task<bool> DeleteAsync(int id);
    }
}