using DTO;
using DTO.LopHoc;

namespace BLL.Interfaces
{
    public interface ILopHocService
    {
        // =====================================================
        // 1. LẤY TẤT CẢ LỚP HỌC
        // =====================================================

        Task<IEnumerable<LopHocResponse>> GetAllAsync();


        // =====================================================
        // 2. LẤY LỚP HỌC THEO ID
        // =====================================================

        Task<LopHocResponse?> GetByIdAsync(int id);


        // =====================================================
        // 3. TẠO LỚP HỌC
        // =====================================================

        Task<LopHocResponse> CreateAsync(
            CreateLopHocRequest request);


        // =====================================================
        // 4. CẬP NHẬT LỚP HỌC
        // =====================================================

        Task<LopHocResponse?> UpdateAsync(
            int id,
            UpdateLopHocRequest request);


        // =====================================================
        // 5. XÓA LỚP HỌC
        // =====================================================

        Task<bool> DeleteAsync(int id);


        // =====================================================
        // 6. TÌM THEO MÃ LỚP
        // =====================================================

        Task<IEnumerable<LopHocResponse>> GetByMaLopCodeAsync(
            string maLopCode);


        // =====================================================
        // 7. TÌM THEO MÃ LỚP + PHÂN TRANG
        // =====================================================

        Task<PagedResult<LopHocResponse>> GetPagedByMaLopCodeAsync(
            string maLopCode,
            int pageNumber,
            int pageSize);


        // =====================================================
        // 8. TÌM THEO TÊN LỚP
        // =====================================================

        Task<IEnumerable<LopHocResponse>> GetByTenLopAsync(
            string tenLop);


        // =====================================================
        // 9. TÌM THEO TÊN LỚP + PHÂN TRANG
        // =====================================================

        Task<PagedResult<LopHocResponse>> GetPagedByTenLopAsync(
            string tenLop,
            int pageNumber,
            int pageSize);


        // =====================================================
        // 10. TÌM THEO KHÓA HỌC
        // =====================================================

        Task<IEnumerable<LopHocResponse>> GetByKhoaHocIdAsync(
            int maKhoaHoc);


        // =====================================================
        // 11. TÌM THEO KHÓA HỌC + PHÂN TRANG
        // =====================================================

        Task<PagedResult<LopHocResponse>> GetPagedByKhoaHocIdAsync(
            int maKhoaHoc,
            int pageNumber,
            int pageSize);


        // =====================================================
        // 12. TÌM THEO GIÁO VIÊN
        // =====================================================

        Task<IEnumerable<LopHocResponse>> GetByGiaoVienIdAsync(
            int maGiaoVien);


        // =====================================================
        // 13. TÌM THEO GIÁO VIÊN + PHÂN TRANG
        // =====================================================

        Task<PagedResult<LopHocResponse>> GetPagedByGiaoVienIdAsync(
            int maGiaoVien,
            int pageNumber,
            int pageSize);


        // =====================================================
        // 14. TÌM THEO PHÒNG HỌC
        // =====================================================

        Task<IEnumerable<LopHocResponse>> GetByPhongHocIdAsync(
            int maPhong);


        // =====================================================
        // 15. TÌM THEO PHÒNG HỌC + PHÂN TRANG
        // =====================================================

        Task<PagedResult<LopHocResponse>> GetPagedByPhongHocIdAsync(
            int maPhong,
            int pageNumber,
            int pageSize);


        // =====================================================
        // 16. TÌM THEO TRẠNG THÁI
        // =====================================================

        Task<IEnumerable<LopHocResponse>> GetByTrangThaiAsync(
            string trangThai);


        // =====================================================
        // 17. TÌM THEO TRẠNG THÁI + PHÂN TRANG
        // =====================================================

        Task<PagedResult<LopHocResponse>> GetPagedByTrangThaiAsync(
            string trangThai,
            int pageNumber,
            int pageSize);


        // =====================================================
        // 18. TÌM KIẾM
        // Mã lớp + tên lớp
        // =====================================================

        Task<IEnumerable<LopHocResponse>> SearchAsync(
            string? maLopCode,
            string? tenLop);


        // =====================================================
        // 19. TÌM KIẾM + PHÂN TRANG
        // =====================================================

        Task<PagedResult<LopHocResponse>> SearchPagedAsync(
            string? maLopCode,
            string? tenLop,
            int pageNumber,
            int pageSize);


        // =====================================================
        // 20. LẤY TẤT CẢ + PHÂN TRANG
        // =====================================================

        Task<PagedResult<LopHocResponse>> GetPagedAsync(
            int pageNumber,
            int pageSize);
    }
}