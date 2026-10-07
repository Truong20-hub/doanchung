using DAL.Entities;

namespace DAL.Interfaces
{
    public interface IDiemRepository
    {
        Task<IEnumerable<Diem>> GetByMaBaiTapAsync(int maBaiTap);
        Task<Diem?> GetByMaBaiTapAndHocVienAsync(
            int maBaiTap,
            int maHocVien);
        Task<Diem> AddAsync(Diem diem);
        Task<Diem> UpdateAsync(Diem diem);
    }
}
