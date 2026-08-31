using DTO.PhongHoc;

namespace BLL.Interfaces
{
    public interface IPhongHocService
    {
        // ==========================================
        // 1. Lấy tất cả phòng học
        // ==========================================
        Task<IEnumerable<PhongHocResponse>> GetAllAsync();

        // ==========================================
        // 2. Lấy tất cả + phân trang
        // ==========================================
        Task<object> GetPagedAsync(
            int pageNumber,
            int pageSize);

        // ==========================================
        // 3. Lấy phòng học theo ID
        // ==========================================
        Task<PhongHocResponse?> GetByIdAsync(
            int id);

        // ==========================================
        // 4. Lấy phòng học theo tên
        // ==========================================
        Task<IEnumerable<PhongHocResponse>> GetByNameAsync(
            string tenPhong);

        // ==========================================
        // 5. Lấy theo tên + phân trang
        // ==========================================
        Task<object> GetPagedByNameAsync(
            string tenPhong,
            int pageNumber,
            int pageSize);

        // ==========================================
        // 6. Lấy phòng học theo sức chứa
        // ==========================================
        Task<IEnumerable<PhongHocResponse>> GetBySucChuaAsync(
            int sucChua);

        // ==========================================
        // 7. Lấy theo sức chứa + phân trang
        // ==========================================
        Task<object> GetPagedBySucChuaAsync(
            int sucChua,
            int pageNumber,
            int pageSize);

        // ==========================================
        // 8. Lấy phòng học theo vị trí
        // ==========================================
        Task<IEnumerable<PhongHocResponse>> GetByViTriAsync(
            string viTri);

        // ==========================================
        // 9. Lấy theo vị trí + phân trang
        // ==========================================
        Task<object> GetPagedByViTriAsync(
            string viTri,
            int pageNumber,
            int pageSize);

        // ==========================================
        // 10. Tìm kiếm
        // ==========================================
        Task<IEnumerable<PhongHocResponse>> SearchAsync(
            string keyword);

        // ==========================================
        // 11. Tìm kiếm + phân trang
        // ==========================================
        Task<object> SearchPagedAsync(
            string keyword,
            int pageNumber,
            int pageSize);

        // ==========================================
        // 12. Kiểm tra tên phòng
        // ==========================================
        Task<bool> ExistsByTenPhongAsync(
            string tenPhong);

        // ==========================================
        // 13. Tạo phòng học
        // ==========================================
        Task<PhongHocResponse> CreateAsync(
            CreatePhongHocRequest request);

        // ==========================================
        // 14. Cập nhật phòng học
        // ==========================================
        Task<PhongHocResponse?> UpdateAsync(
            int id,
            UpdatePhongHocRequest request);

        // ==========================================
        // 15. Xóa phòng học
        // ==========================================
        Task<bool> DeleteAsync(
            int id);
    }
}