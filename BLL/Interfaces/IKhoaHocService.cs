using DTO.KhoaHoc;

namespace BLL.Interfaces
{
    public interface IKhoaHocService
    {
        // =====================================================
        // LẤY TẤT CẢ KHÓA HỌC
        // =====================================================

        Task<IEnumerable<KhoaHocResponse>> GetAllAsync();


        // =====================================================
        // LẤY KHÓA HỌC THEO ID
        // =====================================================

        Task<KhoaHocResponse?> GetByIdAsync(int id);


        // =====================================================
        // LỌC THEO TÊN KHÓA HỌC
        // =====================================================

        Task<IEnumerable<KhoaHocResponse>> GetByNameAsync(
            string tenKhoaHoc);


        // =====================================================
        // LỌC THEO TRÌNH ĐỘ
        // =====================================================

        Task<IEnumerable<KhoaHocResponse>> GetByTrinhDoAsync(
            string trinhDo);


        // =====================================================
        // LỌC THEO HỌC PHÍ
        // =====================================================

        Task<IEnumerable<KhoaHocResponse>> GetByHocPhiAsync(
            decimal hocPhi);


        // =====================================================
        // PHÂN TRANG
        // =====================================================

        Task<object> GetPagedAsync(
            int pageNumber,
            int pageSize);


        // =====================================================
        // PHÂN TRANG + TÊN
        // =====================================================

        Task<object> GetPagedByNameAsync(
            string tenKhoaHoc,
            int pageNumber,
            int pageSize);


        // =====================================================
        // PHÂN TRANG + TRÌNH ĐỘ
        // =====================================================

        Task<object> GetPagedByTrinhDoAsync(
            string trinhDo,
            int pageNumber,
            int pageSize);


        // =====================================================
        // PHÂN TRANG + HỌC PHÍ
        // =====================================================

        Task<object> GetPagedByHocPhiAsync(
            decimal hocPhi,
            int pageNumber,
            int pageSize);


        // =====================================================
        // TẠO KHÓA HỌC
        // =====================================================

        Task<KhoaHocResponse> CreateAsync(
            CreateKhoaHocRequest request);


        // =====================================================
        // CẬP NHẬT KHÓA HỌC
        // =====================================================

        Task<KhoaHocResponse?> UpdateAsync(
            int id,
            UpdateKhoaHocRequest request);


        // =====================================================
        // XÓA KHÓA HỌC
        // =====================================================

        Task<bool> DeleteAsync(int id);


        // =====================================================
        // TÌM KIẾM
        // TÊN + TRÌNH ĐỘ + HỌC PHÍ
        // =====================================================

        Task<IEnumerable<KhoaHocResponse>> SearchAsync(
            string? tenKhoaHoc,
            string? trinhDo,
            decimal? hocPhi);


        // =====================================================
        // TÌM KIẾM + PHÂN TRANG
        // =====================================================

        Task<object> SearchPagedAsync(
            string? tenKhoaHoc,
            string? trinhDo,
            decimal? hocPhi,
            int pageNumber,
            int pageSize);
    }
}