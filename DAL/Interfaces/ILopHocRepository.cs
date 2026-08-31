using DAL.Entities;
using DTO; // Giả định class PagedResult<T> nằm ở DTO (hoặc namespace phù hợp)

namespace DAL.Interfaces
{
    public interface ILopHocRepository
    {
        // =====================================================
        // 1. LẤY TẤT CẢ
        // =====================================================

        Task<IEnumerable<LopHoc>> GetAllAsync();


        // =====================================================
        // 2. LẤY THEO ID
        // =====================================================

        Task<LopHoc?> GetByIdAsync(int id);


        // =====================================================
        // 3. KIỂM TRA ID
        // =====================================================

        Task<bool> ExistsByIdAsync(int id);


        // =====================================================
        // 4. KIỂM TRA MÃ LỚP CODE
        // =====================================================

        Task<bool> ExistsByMaLopCodeAsync(string maLopCode);


        // =====================================================
        // 5. KIỂM TRA TÊN LỚP
        // =====================================================

        Task<bool> ExistsByTenLopAsync(string tenLop);


        // =====================================================
        // 6. KIỂM TRA KHÓA HỌC
        // =====================================================

        Task<bool> ExistsByKhoaHocIdAsync(int maKhoaHoc);


        // =====================================================
        // 7. KIỂM TRA GIÁO VIÊN
        // =====================================================

        Task<bool> ExistsByGiaoVienIdAsync(int maGiaoVien);


        // =====================================================
        // 8. KIỂM TRA PHÒNG HỌC
        // =====================================================

        Task<bool> ExistsByPhongHocIdAsync(int maPhong);


        // =====================================================
        // 9. THÊM
        // =====================================================

        Task<LopHoc> AddAsync(LopHoc lopHoc);


        // =====================================================
        // 10. CẬP NHẬT
        // =====================================================

        Task<LopHoc?> UpdateAsync(LopHoc lopHoc);


        // =====================================================
        // 11. XÓA
        // =====================================================

        Task<bool> DeleteAsync(int id);


        // =====================================================
        // 12. TÌM THEO MÃ LỚP
        // =====================================================

        Task<IEnumerable<LopHoc>> GetByMaLopCodeAsync(
            string maLopCode);


        // =====================================================
        // 13. TÌM THEO MÃ LỚP + PHÂN TRANG
        // =====================================================

        Task<PagedResult<LopHoc>> GetPagedByMaLopCodeAsync(
            string maLopCode,
            int pageNumber,
            int pageSize);


        // =====================================================
        // 14. TÌM THEO TÊN LỚP
        // =====================================================

        Task<IEnumerable<LopHoc>> GetByTenLopAsync(
            string tenLop);


        // =====================================================
        // 15. TÌM THEO TÊN LỚP + PHÂN TRANG
        // =====================================================

        Task<PagedResult<LopHoc>> GetPagedByTenLopAsync(
            string tenLop,
            int pageNumber,
            int pageSize);


        // =====================================================
        // 16. TÌM THEO KHÓA HỌC
        // =====================================================

        Task<IEnumerable<LopHoc>> GetByKhoaHocIdAsync(
            int maKhoaHoc);


        // =====================================================
        // 17. TÌM THEO KHÓA HỌC + PHÂN TRANG
        // =====================================================

        Task<PagedResult<LopHoc>> GetPagedByKhoaHocIdAsync(
            int maKhoaHoc,
            int pageNumber,
            int pageSize);


        // =====================================================
        // 18. TÌM THEO GIÁO VIÊN
        // =====================================================

        Task<IEnumerable<LopHoc>> GetByGiaoVienIdAsync(
            int maGiaoVien);


        // =====================================================
        // 19. TÌM THEO GIÁO VIÊN + PHÂN TRANG
        // =====================================================

        Task<PagedResult<LopHoc>> GetPagedByGiaoVienIdAsync(
            int maGiaoVien,
            int pageNumber,
            int pageSize);


        // =====================================================
        // 20. TÌM THEO PHÒNG HỌC
        // =====================================================

        Task<IEnumerable<LopHoc>> GetByPhongHocIdAsync(
            int maPhong);


        // =====================================================
        // 21. TÌM THEO PHÒNG HỌC + PHÂN TRANG
        // =====================================================

        Task<PagedResult<LopHoc>> GetPagedByPhongHocIdAsync(
            int maPhong,
            int pageNumber,
            int pageSize);


        // =====================================================
        // 22. TÌM THEO TRẠNG THÁI
        // =====================================================

        Task<IEnumerable<LopHoc>> GetByTrangThaiAsync(
            string trangThai);


        // =====================================================
        // 23. TÌM THEO TRẠNG THÁI + PHÂN TRANG
        // =====================================================

        Task<PagedResult<LopHoc>> GetPagedByTrangThaiAsync(
            string trangThai,
            int pageNumber,
            int pageSize);


        // =====================================================
        // 24. TÌM KIẾM
        // Mã lớp + tên lớp
        // =====================================================

        Task<IEnumerable<LopHoc>> SearchAsync(
            string? maLopCode,
            string? tenLop);


        // =====================================================
        // 25. TÌM KIẾM + PHÂN TRANG
        // =====================================================

        Task<PagedResult<LopHoc>> SearchPagedAsync(
            string? maLopCode,
            string? tenLop,
            int pageNumber,
            int pageSize);


        // =====================================================
        // 26. LẤY TẤT CẢ + PHÂN TRANG
        // =====================================================

        Task<PagedResult<LopHoc>> GetPagedAsync(
            int pageNumber,
            int pageSize);
    }
}