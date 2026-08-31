using DTO.VaiTro;
namespace BLL.Interfaces
{
    public interface IVaiTroService
    {
        Task<IEnumerable<VaiTroResponse>> GetAllVaiTroAsync();
        Task<VaiTroResponse?> GetVaiTroByIdAsync(int id);
        Task<VaiTroResponse> AddVaiTroAsync(CreateVaiTroRequest vaiTroRequest);
        Task<VaiTroResponse?> UpdateVaiTroAsync(int id, UpdateVaiTroRequest vaiTroRequest);
        Task<bool> DeleteVaiTroAsync(int id);
    }
}