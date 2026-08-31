using DTO.HocVien;

namespace BLL.Interfaces
{
    public interface IHocVienService
    {
        // =========================================================
        // CRUD
        // =========================================================

        // Lấy tất cả học viên
        Task<IEnumerable<HocVienResponse>> GetAllAsync();

        // Lấy học viên theo mã học viên
        Task<HocVienResponse?> GetByIdAsync(int id);

        // Tạo học viên + tài khoản người dùng
        Task<HocVienResponse> CreateAsync(
            CreateHocVienRequest request);

        // Cập nhật học viên
        Task<HocVienResponse?> UpdateAsync(
            int id,
            UpdateHocVienRequest request);

        // Xóa học viên
        Task<bool> DeleteAsync(int id);


        // =========================================================
        // LẤY THEO TỪNG TIÊU CHÍ
        // =========================================================

        // Lấy học viên theo mã người dùng
        Task<IEnumerable<HocVienResponse>>
            GetByNguoiDungIdAsync(int maNguoiDung);

        // Lấy học viên theo tên học viên
        Task<IEnumerable<HocVienResponse>>
            GetByNameAsync(string name);

        // Lấy học viên theo tên phụ huynh
        Task<IEnumerable<HocVienResponse>>
            GetByParentNameAsync(string parentName);

        // Lấy học viên theo địa chỉ
        Task<IEnumerable<HocVienResponse>>
            GetByAddressAsync(string address);

        // Lấy học viên theo số điện thoại
        Task<IEnumerable<HocVienResponse>>
            GetByPhoneAsync(string phone);

        // Lấy học viên theo email
        Task<IEnumerable<HocVienResponse>>
            GetByEmailAsync(string email);


        // =========================================================
        // TÌM KIẾM
        // =========================================================

        // Tìm kiếm tất cả tiêu chí:
        // Họ tên
        // Email
        // Số điện thoại
        // Địa chỉ
        // Tên phụ huynh
        Task<IEnumerable<HocVienResponse>>
            SearchAsync(string keyword);


        // =========================================================
        // PHÂN TRANG TẤT CẢ
        // =========================================================

        // Lấy tất cả học viên + phân trang
        Task<object> GetPagedAsync(
            int pageNumber,
            int pageSize);


        // =========================================================
        // TỪNG TIÊU CHÍ + PHÂN TRANG
        // =========================================================

        // Mã người dùng + phân trang
        Task<object> GetByNguoiDungIdPagedAsync(
            int maNguoiDung,
            int pageNumber,
            int pageSize);

        // Tên học viên + phân trang
        Task<object> GetByNamePagedAsync(
            string name,
            int pageNumber,
            int pageSize);

        // Tên phụ huynh + phân trang
        Task<object> GetByParentNamePagedAsync(
            string parentName,
            int pageNumber,
            int pageSize);

        // Địa chỉ + phân trang
        Task<object> GetByAddressPagedAsync(
            string address,
            int pageNumber,
            int pageSize);

        // Số điện thoại + phân trang
        Task<object> GetByPhonePagedAsync(
            string phone,
            int pageNumber,
            int pageSize);

        // Email + phân trang
        Task<object> GetByEmailPagedAsync(
            string email,
            int pageNumber,
            int pageSize);


        // =========================================================
        // TÌM KIẾM + PHÂN TRANG
        // =========================================================

        Task<object> SearchPagedAsync(
            string keyword,
            int pageNumber,
            int pageSize);
    }
}