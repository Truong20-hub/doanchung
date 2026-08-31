using BLL.Interfaces;
using DAL.Entities;
using DAL.Interfaces;
using DTO.VaiTro;
namespace BLL.Services
{
    public class VaiTroService : IVaiTroService
    {
        private readonly IVaiTroRepository _vaiTroRepository;
        public VaiTroService(IVaiTroRepository vaiTroRepository)
        {
            _vaiTroRepository = vaiTroRepository;
        }
        public async Task<IEnumerable<VaiTroResponse>> GetAllVaiTroAsync()
        {
            var vaiTros = await _vaiTroRepository.GetAllVaiTroAsync();
            return vaiTros.Select(vt => new VaiTroResponse { TenVaiTro = vt.TenVaiTro, MaVaiTro = vt.MaVaiTro });
        }
        public async Task<VaiTroResponse?> GetVaiTroByIdAsync(int id)
        {
            var vaiTro = await _vaiTroRepository.GetVaiTroByIdAsync(id);
            if (vaiTro == null) return null;
            return new VaiTroResponse { TenVaiTro = vaiTro.TenVaiTro, MaVaiTro = vaiTro.MaVaiTro };
        }
        public async Task<VaiTroResponse> AddVaiTroAsync(CreateVaiTroRequest vaiTroRequest)
        {
            bool exists = await _vaiTroRepository.ExistsByNameAsync(vaiTroRequest.TenVaiTro);
            if (exists)
            {
                throw new Exception("Vai trò với tên này đã tồn tại.");
            }
            var vaiTro = new VaiTro { TenVaiTro = vaiTroRequest.TenVaiTro };
            var addedVaiTro = await _vaiTroRepository.AddVaiTroAsync(vaiTro);
            return new VaiTroResponse { TenVaiTro = addedVaiTro.TenVaiTro , MaVaiTro = addedVaiTro.MaVaiTro };
        }
        public async Task<VaiTroResponse?> UpdateVaiTroAsync(int id, UpdateVaiTroRequest vaiTroRequest)
        {
            var existingVaiTro = await _vaiTroRepository.GetVaiTroByIdAsync(id);
            if (existingVaiTro == null) return null;
            existingVaiTro.TenVaiTro = vaiTroRequest.TenVaiTro;
            var updatedVaiTro = await _vaiTroRepository.UpdateVaiTroAsync(existingVaiTro);
            return new VaiTroResponse { TenVaiTro = updatedVaiTro!.TenVaiTro, MaVaiTro = updatedVaiTro.MaVaiTro };
        }
        public async Task<bool> DeleteVaiTroAsync(int id)
        {
            return await _vaiTroRepository.DeleteVaiTroAsync(id);

        }
    }
}