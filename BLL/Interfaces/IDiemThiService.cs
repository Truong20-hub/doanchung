using System.Collections.Generic;
using System.Threading.Tasks;
using DTO.DiemThi;

namespace BLL.Interfaces;

public interface IDiemThiService
{
    Task<IEnumerable<DiemThiResponse>> GetAllAsync();
    Task<DiemThiResponse?> GetByIdAsync(int maDiemThi);
    Task<IEnumerable<DiemThiResponse>> GetByMaKyThiAsync(int maKyThi);
    Task<IEnumerable<DiemThiResponse>> GetByMaHocVienAsync(int maHocVien);
    Task<DiemThiResponse> CreateAsync(CreateDiemThiRequest request);
    Task<DiemThiResponse> UpdateAsync(UpdateDiemThiRequest request);
    Task<bool> DeleteAsync(int maDiemThi);
}