using DAL.Context;
using DAL.Entities;
using DAL.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace DAL.Repositories
{
    public class KyThiRepository : IKyThiRepository
    {
        private readonly AppDbContext _context;

        public KyThiRepository(
            AppDbContext context)
        {
            _context = context;
        }

        // =====================================================
        // GET ALL
        // =====================================================

        public async Task<IEnumerable<KyThi>> GetAllAsync()
        {
            return await _context.KyThis
                .Include(x => x.MaLopNavigation)
                .AsNoTracking()
                .ToListAsync();
        }

        // =====================================================
        // GET BY ID
        // =====================================================

        public async Task<KyThi?> GetByIdAsync(int id)
        {
            return await _context.KyThis
                .Include(x => x.MaLopNavigation)
                .FirstOrDefaultAsync(
                    x => x.MaKyThi == id);
        }

        // =====================================================
        // GET BY MA LOP
        // =====================================================

        public async Task<IEnumerable<KyThi>>
            GetByMaLopAsync(int maLop)
        {
            return await _context.KyThis
                .Include(x => x.MaLopNavigation)
                .Where(x => x.MaLop == maLop)
                .AsNoTracking()
                .ToListAsync();
        }

        // =====================================================
        // GET BY LOAI KY THI
        // =====================================================

        public async Task<IEnumerable<KyThi>>
            GetByLoaiKyThiAsync(string loaiKyThi)
        {
            return await _context.KyThis
                .Include(x => x.MaLopNavigation)
                .Where(x =>
                    x.LoaiKyThi == loaiKyThi)
                .AsNoTracking()
                .ToListAsync();
        }

        // =====================================================
        // SEARCH
        // =====================================================

        public async Task<IEnumerable<KyThi>>
            SearchAsync(string? keyword)
        {
            var query = _context.KyThis
                .Include(x => x.MaLopNavigation)
                .AsQueryable();

            if (!string.IsNullOrWhiteSpace(keyword))
            {
                keyword = keyword.Trim();

                query = query.Where(x =>
                    x.TenKyThi.Contains(keyword) ||
                    (x.LoaiKyThi != null &&
                     x.LoaiKyThi.Contains(keyword)) ||
                    x.MaLopNavigation.TenLop.Contains(keyword) ||
                    x.MaLopNavigation.MaLopCode.Contains(keyword));
            }

            return await query
                .AsNoTracking()
                .ToListAsync();
        }

        // =====================================================
        // PAGED
        // =====================================================

        public async Task<object> GetPagedAsync(
            int pageNumber,
            int pageSize)
        {
            var query = _context.KyThis
                .Include(x => x.MaLopNavigation)
                .AsNoTracking();

            var totalItems =
                await query.CountAsync();

            var data = await query
                .OrderBy(x => x.MaKyThi)
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            return new
            {
                PageNumber = pageNumber,
                PageSize = pageSize,
                TotalItems = totalItems,

                TotalPages =
                    (int)Math.Ceiling(
                        (double)totalItems / pageSize),

                Data = data
            };
        }

        // =====================================================
        // SEARCH + PAGED
        // =====================================================

        public async Task<object> SearchPagedAsync(
            string? keyword,
            int pageNumber,
            int pageSize)
        {
            var query = _context.KyThis
                .Include(x => x.MaLopNavigation)
                .AsQueryable();

            if (!string.IsNullOrWhiteSpace(keyword))
            {
                keyword = keyword.Trim();

                query = query.Where(x =>
                    x.TenKyThi.Contains(keyword) ||
                    (x.LoaiKyThi != null &&
                     x.LoaiKyThi.Contains(keyword)) ||
                    x.MaLopNavigation.TenLop.Contains(keyword) ||
                    x.MaLopNavigation.MaLopCode.Contains(keyword));
            }

            var totalItems =
                await query.CountAsync();

            var data = await query
                .OrderBy(x => x.MaKyThi)
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .AsNoTracking()
                .ToListAsync();

            return new
            {
                PageNumber = pageNumber,
                PageSize = pageSize,
                TotalItems = totalItems,

                TotalPages =
                    (int)Math.Ceiling(
                        (double)totalItems / pageSize),

                Data = data
            };
        }

        // =====================================================
        // EXISTS ID
        // =====================================================

        public async Task<bool> ExistsByIdAsync(int id)
        {
            return await _context.KyThis
                .AnyAsync(x => x.MaKyThi == id);
        }

        // =====================================================
        // EXISTS TÊN
        // =====================================================

        public async Task<bool> ExistsByTenKyThiAsync(
            int maLop,
            string tenKyThi,
            int? excludeId = null)
        {
            return await _context.KyThis
                .AnyAsync(x =>
                    x.MaLop == maLop &&
                    x.TenKyThi == tenKyThi &&
                    (!excludeId.HasValue ||
                     x.MaKyThi != excludeId.Value));
        }

        // =====================================================
        // ADD
        // =====================================================

        public async Task<KyThi> AddAsync(
            KyThi kyThi)
        {
            await _context.KyThis.AddAsync(kyThi);

            await _context.SaveChangesAsync();

            return kyThi;
        }

        // =====================================================
        // UPDATE
        // =====================================================

        public async Task<KyThi?> UpdateAsync(
            KyThi kyThi)
        {
            var existing =
                await _context.KyThis
                    .FirstOrDefaultAsync(
                        x => x.MaKyThi == kyThi.MaKyThi);

            if (existing == null)
                return null;

            existing.MaLop = kyThi.MaLop;
            existing.TenKyThi = kyThi.TenKyThi;
            existing.NgayThi = kyThi.NgayThi;
            existing.LoaiKyThi = kyThi.LoaiKyThi;
            existing.DiemToiDa = kyThi.DiemToiDa;

            await _context.SaveChangesAsync();

            return existing;
        }

        // =====================================================
        // DELETE
        // =====================================================

        public async Task<bool> DeleteAsync(int id)
        {
            var entity =
                await _context.KyThis
                    .FirstOrDefaultAsync(
                        x => x.MaKyThi == id);

            if (entity == null)
                return false;

            _context.KyThis.Remove(entity);

            await _context.SaveChangesAsync();

            return true;
        }
    }
}