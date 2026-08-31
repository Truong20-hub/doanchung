using DTO.Auth;
using DTO.NguoiDung;

namespace BLL.Interfaces
{
    public interface INguoiDungService
    {
        // Lấy danh sách người dùng
        Task<IEnumerable<NguoiDungResponse>> GetAllAsync();

        // Lấy người dùng theo ID
        Task<NguoiDungResponse?> GetByIdAsync(int id);

        // Thêm người dùng
        Task CreateAsync(CreateNguoiDungRequest request);

        // Cập nhật người dùng
        Task UpdateAsync(int id, UpdateNguoiDungRequest request);

        // Xóa người dùng
        Task DeleteAsync(int id);
        Task<LoginResponse> LoginAsync(LoginResquest request);
    }
}