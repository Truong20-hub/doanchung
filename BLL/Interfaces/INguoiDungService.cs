using DAL.Entities;
using DTO.Auth;
using DTO.NguoiDung;
using DTO.result;

namespace BLL.Interfaces
{
    public interface INguoiDungService
    {
        // Lấy danh sách người dùng
        Task<IEnumerable<NguoiDungResponse>> GetAllAsync();

        // Lấy người dùng theo ID
        Task<NguoiDungResponse?> GetByIdAsync(int id);
        // lấy người dùng theo tên đăng nhập
        Task<NguoiDungResponse?> GetByNamelogin(string NameLogin); 

        // Thêm người dùng
        Task CreateAsync(CreateNguoiDungRequest request);

        // Cập nhật người dùng
        Task UpdateAsync(int id, UpdateNguoiDungRequest request);

        // Xóa người dùng
        Task<result> DeleteAsync(int id);
        Task<LoginResponse> LoginAsync(LoginResquest request);
    }
}