using DAL.Entities;
namespace DAL.Interfaces
{
    public interface IVaiTroRepository
    {
        Task<IEnumerable<VaiTro>> GetAllVaiTroAsync();
        Task<VaiTro?> GetVaiTroByIdAsync(int id);
        Task<VaiTro> AddVaiTroAsync(VaiTro vaiTro);
        Task<VaiTro?> UpdateVaiTroAsync(VaiTro vaiTro);
        Task<bool> DeleteVaiTroAsync(int id);
        Task<bool> ExistsByNameAsync(string tenVaiTro);
    }
}