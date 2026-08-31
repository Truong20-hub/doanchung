using DAL.Entities;

namespace DAL.Interfaces
{
    public interface IDangKyHocRepository
    {
        // =====================================================
        // LẤY DỮ LIỆU
        // =====================================================

        // Lấy tất cả đăng ký học
        Task<IEnumerable<DangKyHoc>> GetAllAsync();

        // Lấy đăng ký theo ID
        Task<DangKyHoc?> GetByIdAsync(int id);

        // Lấy theo mã học viên
        Task<IEnumerable<DangKyHoc>> GetByHocVienIdAsync(
            int maHocVien);

        // Lấy theo mã lớp
        Task<IEnumerable<DangKyHoc>> GetByLopIdAsync(
            int maLop);

        // Lấy theo trạng thái
        Task<IEnumerable<DangKyHoc>> GetByTrangThaiAsync(
            string trangThai);

        // =====================================================
        // KIỂM TRA
        // =====================================================

        // Kiểm tra đăng ký tồn tại
        Task<bool> ExistsByIdAsync(int id);

        // Kiểm tra học viên đã đăng ký lớp chưa
        Task<bool> ExistsHocVienLopAsync(
            int maHocVien,
            int maLop);

        // Kiểm tra khi UPDATE
        // loại trừ chính bản ghi đang sửa
        Task<bool> ExistsHocVienLopExceptIdAsync(
            int maHocVien,
            int maLop,
            int maDangKy);

        // Kiểm tra học viên có tồn tại
        Task<bool> ExistsHocVienAsync(
            int maHocVien);

        // Kiểm tra lớp có tồn tại
        Task<bool> ExistsLopAsync(
            int maLop);

        // =====================================================
        // TẠO
        // =====================================================

        Task<DangKyHoc> AddAsync(
            DangKyHoc dangKyHoc);

        // =====================================================
        // CẬP NHẬT
        // =====================================================

        Task<DangKyHoc?> UpdateAsync(
            DangKyHoc dangKyHoc);

        // =====================================================
        // XÓA
        // =====================================================

        Task<bool> DeleteAsync(int id);

        // =====================================================
        // TÌM KIẾM
        // =====================================================

        Task<IEnumerable<DangKyHoc>> SearchAsync(
            int? maHocVien,
            int? maLop,
            string? trangThai);

        // =====================================================
        // PHÂN TRANG
        // =====================================================

        Task<object> GetPagedAsync(
            int pageNumber,
            int pageSize);

        // =====================================================
        // TÌM KIẾM + PHÂN TRANG
        // =====================================================

        Task<object> SearchPagedAsync(
            int? maHocVien,
            int? maLop,
            string? trangThai,
            int pageNumber,
            int pageSize);

        // =====================================================
        // PHÂN TRANG THEO HỌC VIÊN
        // =====================================================

        Task<object> GetPagedByHocVienIdAsync(
            int maHocVien,
            int pageNumber,
            int pageSize);

        // =====================================================
        // PHÂN TRANG THEO LỚP
        // =====================================================

        Task<object> GetPagedByLopIdAsync(
            int maLop,
            int pageNumber,
            int pageSize);

        // =====================================================
        // PHÂN TRANG THEO TRẠNG THÁI
        // =====================================================

        Task<object> GetPagedByTrangThaiAsync(
            string trangThai,
            int pageNumber,
            int pageSize);
        // =====================================================
        // lấy theo học viên và lớp
        // =====================================================
        Task<DangKyHoc?> GetByHocVienIdAndLopIdAsync(
            int maHocVien,
            int maLop);
    }
}