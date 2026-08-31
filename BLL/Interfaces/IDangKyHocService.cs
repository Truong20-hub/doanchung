using DTO.DangKyHoc;

namespace BLL.Interfaces
{
    public interface IDangKyHocService
    {
        // =====================================================
        // LẤY DỮ LIỆU
        // =====================================================

        // Lấy tất cả đăng ký học
        Task<IEnumerable<DangKyHocResponse>> GetAllAsync();

        // Lấy đăng ký theo ID
        Task<DangKyHocResponse?> GetByIdAsync(int id);

        // Lấy danh sách đăng ký theo học viên
        Task<IEnumerable<DangKyHocResponse>> GetByHocVienIdAsync(
            int maHocVien);

        // Lấy danh sách đăng ký theo lớp
        Task<IEnumerable<DangKyHocResponse>> GetByLopIdAsync(
            int maLop);

        // Lấy danh sách theo trạng thái
        Task<IEnumerable<DangKyHocResponse>> GetByTrangThaiAsync(
            string trangThai);

        // =====================================================
        // CREATE
        // =====================================================

        Task<DangKyHocResponse> CreateAsync(
            CreateDangKyHocRequest request);

        // =====================================================
        // UPDATE
        // =====================================================

        Task<DangKyHocResponse?> UpdateAsync(
            int id,
            UpdateDangKyHocRequest request);

        // =====================================================
        // DELETE
        // =====================================================

        Task<bool> DeleteAsync(int id);

        // =====================================================
        // SEARCH
        // =====================================================

        Task<IEnumerable<DangKyHocResponse>> SearchAsync(
            int? maHocVien,
            int? maLop,
            string? trangThai);

        // =====================================================
        // PAGING
        // =====================================================

        Task<object> GetPagedAsync(
            int pageNumber,
            int pageSize);

        // =====================================================
        // SEARCH + PAGING
        // =====================================================

        Task<object> SearchPagedAsync(
            int? maHocVien,
            int? maLop,
            string? trangThai,
            int pageNumber,
            int pageSize);

        // =====================================================
        // PAGING THEO HỌC VIÊN
        // =====================================================

        Task<object> GetPagedByHocVienIdAsync(
            int maHocVien,
            int pageNumber,
            int pageSize);

        // =====================================================
        // PAGING THEO LỚP
        // =====================================================

        Task<object> GetPagedByLopIdAsync(
            int maLop,
            int pageNumber,
            int pageSize);

        // =====================================================
        // PAGING THEO TRẠNG THÁI
        // =====================================================

        Task<object> GetPagedByTrangThaiAsync(
            string trangThai,
            int pageNumber,
            int pageSize);
    }
}