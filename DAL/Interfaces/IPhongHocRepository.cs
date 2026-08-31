using DAL.Entities;

namespace DAL.Interfaces
{
    public interface IPhongHocRepository
    {
        // ==========================================
        // 1. Lấy tất cả phòng học
        // ==========================================
        Task<IEnumerable<PhongHoc>> GetAllAsync();

        // ==========================================
        // 2. Lấy phòng học theo ID
        // ==========================================
        Task<PhongHoc?> GetByIdAsync(int id);

        // ==========================================
        // 3. Kiểm tra phòng học tồn tại theo ID
        // ==========================================
        Task<bool> ExistsByIdAsync(int id);

        // ==========================================
        // 4. Kiểm tra tên phòng đã tồn tại chưa
        // ==========================================
        Task<bool> ExistsByTenPhongAsync(string tenPhong);

        // ==========================================
        // 5. Lấy phòng học theo tên
        // ==========================================
        Task<IEnumerable<PhongHoc>> GetByNameAsync(
            string tenPhong);

        // ==========================================
        // 6. Lấy phòng học theo tên + phân trang
        // ==========================================
        Task<object> GetPagedByNameAsync(
            string tenPhong,
            int pageNumber,
            int pageSize);

        // ==========================================
        // 7. Lấy phòng học theo sức chứa
        // ==========================================
        Task<IEnumerable<PhongHoc>> GetBySucChuaAsync(
            int sucChua);

        // ==========================================
        // 8. Lấy phòng học theo sức chứa + phân trang
        // ==========================================
        Task<object> GetPagedBySucChuaAsync(
            int sucChua,
            int pageNumber,
            int pageSize);

        // ==========================================
        // 9. Lấy phòng học theo vị trí
        // ==========================================
        Task<IEnumerable<PhongHoc>> GetByViTriAsync(
            string viTri);

        // ==========================================
        // 10. Lấy phòng học theo vị trí + phân trang
        // ==========================================
        Task<object> GetPagedByViTriAsync(
            string viTri,
            int pageNumber,
            int pageSize);

        // ==========================================
        // 11. Tìm kiếm
        // ==========================================
        Task<IEnumerable<PhongHoc>> SearchAsync(
            string keyword);

        // ==========================================
        // 12. Tìm kiếm + phân trang
        // ==========================================
        Task<object> SearchPagedAsync(
            string keyword,
            int pageNumber,
            int pageSize);

        // ==========================================
        // 13. Lấy tất cả + phân trang
        // ==========================================
        Task<object> GetPagedAsync(
            int pageNumber,
            int pageSize);

        // ==========================================
        // 14. Thêm phòng học
        // ==========================================
        Task<PhongHoc> AddAsync(
            PhongHoc phongHoc);

        // ==========================================
        // 15. Cập nhật phòng học
        // ==========================================
        Task<PhongHoc?> UpdateAsync(
            PhongHoc phongHoc);

        // ==========================================
        // 16. Xóa phòng học
        // ==========================================
        Task<bool> DeleteAsync(
            int id);
    }
}