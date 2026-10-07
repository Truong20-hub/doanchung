using DAL.Context;
using DAL.Entities;
using DAL.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace DAL.Repositories
{
    public class DiemRepository : IDiemRepository
    {
        private readonly AppDbContext _context;

        public DiemRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Diem>> GetByMaBaiTapAsync(int maBaiTap)
        {
            return await _context.Diems
                .Include(item => item.MaHocVienNavigation)
                    .ThenInclude(item => item.MaNguoiDungNavigation)
                .AsNoTracking()
                .Where(item => item.MaBaiTap == maBaiTap)
                .ToListAsync();
        }

        public Task<Diem?> GetByMaBaiTapAndHocVienAsync(
            int maBaiTap,
            int maHocVien)
        {
            return _context.Diems.FirstOrDefaultAsync(
                item => item.MaBaiTap == maBaiTap &&
                        item.MaHocVien == maHocVien);
        }

        public async Task<Diem> AddAsync(Diem diem)
        {
            await _context.Diems.AddAsync(diem);
            await _context.SaveChangesAsync();
            return diem;
        }

        public async Task<Diem> UpdateAsync(Diem diem)
        {
            _context.Diems.Update(diem);
            await _context.SaveChangesAsync();
            return diem;
        }
    }
}
