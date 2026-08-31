using DAL.Entities;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DAL.Interfaces
{
    public interface IHocVienRepository
    {
        // =========================================================
        // CRUD CƠ BẢN
        // =========================================================

        // Lấy tất cả danh sách học viên
        Task<IEnumerable<HocVien>> GetAllAsync();

        // Lấy học viên theo ID
        Task<HocVien?> GetByIdAsync(int id);

        // Kiểm tra học viên có tồn tại hay không bằng ID
        Task<bool> ExistsByIdAsync(int id);

        // Kiểm tra tài khoản người dùng đã được liên kết với học viên chưa
        Task<bool> ExistsByNguoiDungIdAsync(int maNguoiDung);

        // Thêm học viên
        Task<HocVien> AddAsync(HocVien hocVien);

        // Cập nhật học viên
        Task<HocVien?> UpdateAsync(int id, HocVien hocVien);

        // Xóa học viên
        Task<bool> DeleteAsync(int id);


        // =========================================================
        // TÌM KIẾM THEO TỪNG TIÊU CHÍ
        // =========================================================

        // Lấy danh sách học viên theo mã người dùng
        Task<IEnumerable<HocVien>> GetByNguoiDungIdAsync(int maNguoiDung);

        // Lấy danh sách học viên theo tên học viên
        Task<IEnumerable<HocVien>> GetByNameAsync(string name);

        // Lấy danh sách học viên theo tên phụ huynh
        Task<IEnumerable<HocVien>> GetByParentNameAsync(string parentName);

        // Lấy danh sách học viên theo địa chỉ
        Task<IEnumerable<HocVien>> GetByAddressAsync(string address);

        // Lấy danh sách học viên theo số điện thoại
        Task<IEnumerable<HocVien>> GetByPhoneAsync(string phone);

        // Lấy danh sách học viên theo email
        Task<IEnumerable<HocVien>> GetByEmailAsync(string email);


        // =========================================================
        // TÌM KIẾM NHIỀU TIÊU CHÍ
        // =========================================================

        // Tìm kiếm học viên
        // Có thể tìm theo:
        // - Họ tên
        // - Email
        // - Số điện thoại
        // - Địa chỉ
        // - Tên phụ huynh
        Task<IEnumerable<HocVien>> SearchAsync(string keyword);


        // =========================================================
        // PHÂN TRANG
        // =========================================================

        // Lấy danh sách học viên và phân trang
        Task<(IEnumerable<HocVien> Data, int TotalCount)>
            GetPagedAsync(int pageNumber, int pageSize);


        // =========================================================
        // TÌM KIẾM + PHÂN TRANG
        // =========================================================

        // Theo mã người dùng + phân trang
        Task<(IEnumerable<HocVien> Data, int TotalCount)>
            GetByNguoiDungIdPagedAsync(
                int maNguoiDung,
                int pageNumber,
                int pageSize);

        // Theo tên học viên + phân trang
        Task<(IEnumerable<HocVien> Data, int TotalCount)>
            GetByNamePagedAsync(
                string name,
                int pageNumber,
                int pageSize);

        // Theo tên phụ huynh + phân trang
        Task<(IEnumerable<HocVien> Data, int TotalCount)>
            GetByParentNamePagedAsync(
                string parentName,
                int pageNumber,
                int pageSize);

        // Theo địa chỉ + phân trang
        Task<(IEnumerable<HocVien> Data, int TotalCount)>
            GetByAddressPagedAsync(
                string address,
                int pageNumber,
                int pageSize);

        // Theo số điện thoại + phân trang
        Task<(IEnumerable<HocVien> Data, int TotalCount)>
            GetByPhonePagedAsync(
                string phone,
                int pageNumber,
                int pageSize);

        // Theo email + phân trang
        Task<(IEnumerable<HocVien> Data, int TotalCount)>
            GetByEmailPagedAsync(
                string email,
                int pageNumber,
                int pageSize);

        // Tìm kiếm nhiều tiêu chí + phân trang
        Task<(IEnumerable<HocVien> Data, int TotalCount)>
            SearchPagedAsync(
                string keyword,
                int pageNumber,
                int pageSize);


        // =========================================================
        // LƯU DATABASE
        // =========================================================

        Task SaveChangesAsync();
    }
}