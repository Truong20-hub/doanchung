using System.Collections.Generic;
using System.Threading.Tasks;
using DAL.Entities;

namespace DAL.Interfaces;

public interface IDiemThiRepository
{
    Task<IEnumerable<DiemThi>> GetAllAsync();
    Task<DiemThi?> GetByIdAsync(int maDiemThi);
    Task<IEnumerable<DiemThi>> GetByMaKyThiAsync(int maKyThi);
    Task<IEnumerable<DiemThi>> GetByMaHocVienAsync(int maHocVien);
    Task<DiemThi?> GetByKyThiAndHocVienAsync(int maKyThi, int maHocVien);
    Task<bool> ExistsForKyThiAndHocVienAsync(int maKyThi, int maHocVien, int? excludeMaDiemThi = null);
    Task<DiemThi> AddAsync(DiemThi diemThi);
    Task<DiemThi> UpdateAsync(DiemThi diemThi);
    Task<bool> DeleteAsync(int maDiemThi);
    Task<bool> ExistsAsync(int maDiemThi);
}