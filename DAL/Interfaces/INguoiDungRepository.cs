using DAL.Entities;

namespace DAL.Interfaces
{
    public interface INguoiDungRepository
    {
        // lấy tất cả người dùng
        Task<IEnumerable<NguoiDung>> GetAllAsync();
        // lấy người dùng theo id
        Task<NguoiDung?> GetByIdAsync(int id);
        // lấy người dùng theo tên đăng nhập
        Task<NguoiDung?> GetByUserNameAsync(string userName);
        // lấy người dùng theo email
        Task AddAsync(NguoiDung nguoiDung);
        // cập nhật người dùng
        Task UpdateAsync(NguoiDung nguoiDung);
        // xóa người dùng
        Task DeleteAsync(int id);
        // Lưu thay đổi
        Task SaveChangesAsync();
        Task<NguoiDung?> LoginAsync(string tenDangNhap);
        // kiểm tra người dùng có tồn tại hay không
        Task<bool> ExistsByIdAsync(int maNguoiDung);
        // kiểm tra email có tồn tại hay không
        Task<bool> ExistsByEmailAsync(string email);
        // kiểm tra tên đăng nhập có tồn tại hay không
        Task<bool> ExistsByUserNameAsync(string tenDangNhap);
        // kiểm tra số điện thoại có tồn tại hay không
        Task<bool> ExistsBySoDienThoaiAsync(string soDienThoai);
        // kiểm tra email có tồn tại hay không, ngoại trừ người dùng có id = maNguoiDung
        Task<bool> ExistsByEmailAsync(string email, int maNguoiDung);
        // kiểm tra tên đăng nhập có tồn tại hay không, ngoại trừ người dùng có id = maNguoiDung
        Task<bool> ExistsByUserNameAsync(string tenDangNhap, int maNguoiDung);
        // kiểm tra số điện thoại có tồn tại hay không, ngoại trừ người dùng có id = maNguoiDung
        Task<bool> ExistsBySoDienThoaiAsync(string soDienThoai, int maNguoiDung);

    }
}