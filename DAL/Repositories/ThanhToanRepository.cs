using DAL.Context;
using DAL.Entities;
using DAL.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace DAL.Repositories
{
    public class ThanhToanRepository : IThanhToanRepository
    {
        private readonly AppDbContext _context;

        public ThanhToanRepository(
            AppDbContext context)
        {
            _context = context;
        }

        // =====================================================
        // GET ALL
        // =====================================================

        public async Task<IEnumerable<ThanhToan>>
            GetAllAsync()
        {
            return await _context.ThanhToans
                .Include(x => x.MaHoaDonNavigation)
                .AsNoTracking()
                .ToListAsync();
        }

        // =====================================================
        // GET BY ID
        // =====================================================

        public async Task<ThanhToan?>
            GetByIdAsync(int id)
        {
            return await _context.ThanhToans
                .Include(x => x.MaHoaDonNavigation)
                .FirstOrDefaultAsync(
                    x => x.MaThanhToan == id);
        }

        // =====================================================
        // GET BY HÓA ĐƠN
        // =====================================================

        public async Task<IEnumerable<ThanhToan>>
            GetByMaHoaDonAsync(
                int maHoaDon)
        {
            return await _context.ThanhToans
                .Include(x => x.MaHoaDonNavigation)
                .Where(x =>
                    x.MaHoaDon == maHoaDon)
                .OrderBy(x => x.MaThanhToan)
                .AsNoTracking()
                .ToListAsync();
        }

        // =====================================================
        // TỔNG ĐÃ THANH TOÁN
        // =====================================================

        public async Task<decimal>
            GetTongDaThanhToanAsync(
                int maHoaDon)
        {
            return await _context.ThanhToans
                .Where(x =>
                    x.MaHoaDon == maHoaDon)
                .SumAsync(
                    x => (decimal?)x.SoTienDaTra)
                ?? 0;
        }

        // =====================================================
        // EXISTS
        // =====================================================

        public async Task<bool>
            ExistsByIdAsync(int id)
        {
            return await _context.ThanhToans
                .AnyAsync(
                    x => x.MaThanhToan == id);
        }

        // =====================================================
        // PAGED
        // =====================================================

        public async Task<object>
            GetPagedAsync(
                int pageNumber,
                int pageSize)
        {
            var query =
                _context.ThanhToans
                    .Include(x =>
                        x.MaHoaDonNavigation)
                    .AsNoTracking();

            var totalItems =
                await query.CountAsync();

            var data =
                await query
                    .OrderBy(x =>
                        x.MaThanhToan)
                    .Skip(
                        (pageNumber - 1)
                        * pageSize)
                    .Take(pageSize)
                    .ToListAsync();

            return new
            {
                PageNumber = pageNumber,

                PageSize = pageSize,

                TotalItems = totalItems,

                TotalPages =
                    (int)Math.Ceiling(
                        (double)totalItems /
                        pageSize),

                Data = data
            };
        }

        // =====================================================
        // ADD
        // =====================================================

        public async Task<ThanhToan>
            AddAsync(
                ThanhToan thanhToan)
        {
            await _context.ThanhToans
                .AddAsync(thanhToan);

            await _context.SaveChangesAsync();

            return thanhToan;
        }

        // =====================================================
        // UPDATE
        // =====================================================

        public async Task<ThanhToan?>
            UpdateAsync(
                ThanhToan thanhToan)
        {
            var existing =
                await _context.ThanhToans
                    .FirstOrDefaultAsync(
                        x => x.MaThanhToan ==
                             thanhToan.MaThanhToan);

            if (existing == null)
                return null;

            existing.MaHoaDon =
                thanhToan.MaHoaDon;

            existing.SoTienDaTra =
                thanhToan.SoTienDaTra;

            existing.NgayThanhToan =
                thanhToan.NgayThanhToan;

            existing.HinhThuc =
                thanhToan.HinhThuc;

            existing.GhiChu =
                thanhToan.GhiChu;

            await _context.SaveChangesAsync();

            return existing;
        }

        // =====================================================
        // DELETE
        // =====================================================

        public async Task<bool>
            DeleteAsync(int id)
        {
            var entity =
                await _context.ThanhToans
                    .FirstOrDefaultAsync(
                        x => x.MaThanhToan == id);

            if (entity == null)
                return false;

            _context.ThanhToans
                .Remove(entity);

            await _context.SaveChangesAsync();

            return true;
        }
    }
}