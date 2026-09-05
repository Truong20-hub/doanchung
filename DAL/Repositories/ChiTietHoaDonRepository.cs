using DAL.Context;
using DAL.Entities;
using DAL.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace DAL.Repositories
{
    public class ChiTietHoaDonRepository : IChiTietHoaDonRepository
    {
        private readonly AppDbContext _context;

        public ChiTietHoaDonRepository(AppDbContext context)
        {
            _context = context;
        }

        // =====================================================
        // GET ALL
        // =====================================================

        public async Task<IEnumerable<ChiTietHoaDon>> GetAllAsync()
        {
            return await _context.ChiTietHoaDons
                .Include(x => x.MaHoaDonNavigation)
                .ThenInclude(x => x.MaHocVienNavigation)
                .Include(x => x.MaHoaDonNavigation)
                .ThenInclude(x => x.MaLopNavigation)
                .AsNoTracking()
                .OrderBy(x => x.MaChiTietHoaDon)
                .ToListAsync();
        }

        // =====================================================
        // GET BY ID
        // =====================================================

        public async Task<ChiTietHoaDon?> GetByIdAsync(int id)
        {
            return await _context.ChiTietHoaDons
                .Include(x => x.MaHoaDonNavigation)
                .ThenInclude(x => x.MaHocVienNavigation)
                .Include(x => x.MaHoaDonNavigation)
                .ThenInclude(x => x.MaLopNavigation)
                .FirstOrDefaultAsync(x => x.MaChiTietHoaDon == id);
        }

        // =====================================================
        // GET BY MÃ HÓA ĐƠN
        // =====================================================

        public async Task<IEnumerable<ChiTietHoaDon>> GetByMaHoaDonAsync(int maHoaDon)
        {
            return await _context.ChiTietHoaDons
                .Include(x => x.MaHoaDonNavigation)
                .ThenInclude(x => x.MaHocVienNavigation)
                .Include(x => x.MaHoaDonNavigation)
                .ThenInclude(x => x.MaLopNavigation)
                .Where(x => x.MaHoaDon == maHoaDon)
                .AsNoTracking()
                .OrderBy(x => x.MaChiTietHoaDon)
                .ToListAsync();
        }

        // =====================================================
        // SEARCH
        // =====================================================

        public async Task<IEnumerable<ChiTietHoaDon>> SearchAsync(string? keyword)
        {
            var query = _context.ChiTietHoaDons
                .Include(x => x.MaHoaDonNavigation)
                .ThenInclude(x => x.MaHocVienNavigation)
                .Include(x => x.MaHoaDonNavigation)
                .ThenInclude(x => x.MaLopNavigation)
                .AsQueryable();

            if (!string.IsNullOrWhiteSpace(keyword))
            {
                keyword = keyword.Trim();

                query = query.Where(x =>
                    (x.GhiChu != null && x.GhiChu.Contains(keyword))
                    || x.MaHoaDonNavigation.TrangThai != null &&
                       x.MaHoaDonNavigation.TrangThai.Contains(keyword)
                    || x.MaHoaDonNavigation.MaHocVienNavigation.HoTen.Contains(keyword)
                    || x.MaHoaDonNavigation.MaLopNavigation.TenLop.Contains(keyword)
                    || x.MaHoaDonNavigation.MaLopNavigation.MaLopCode.Contains(keyword));
            }

            return await query
                .AsNoTracking()
                .ToListAsync();
        }

        public async Task<object> GetPagedAsync(int pageNumber, int pageSize)
        {
            var query = _context.ChiTietHoaDons
                .Include(x => x.MaHoaDonNavigation)
                .ThenInclude(x => x.MaHocVienNavigation)
                .Include(x => x.MaHoaDonNavigation)
                .ThenInclude(x => x.MaLopNavigation)
                .AsNoTracking();

            var totalItems = await query.CountAsync();
            var data = await query
                .OrderBy(x => x.MaChiTietHoaDon)
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            return new
            {
                PageNumber = pageNumber,
                PageSize = pageSize,
                TotalItems = totalItems,
                TotalPages = (int)Math.Ceiling((double)totalItems / pageSize),
                Data = data
            };
        }

        public async Task<object> SearchPagedAsync(string? keyword, int pageNumber, int pageSize)
        {
            var query = _context.ChiTietHoaDons
                .Include(x => x.MaHoaDonNavigation)
                .ThenInclude(x => x.MaHocVienNavigation)
                .Include(x => x.MaHoaDonNavigation)
                .ThenInclude(x => x.MaLopNavigation)
                .AsQueryable();

            if (!string.IsNullOrWhiteSpace(keyword))
            {
                keyword = keyword.Trim();
                query = query.Where(x =>
                    (x.GhiChu != null && x.GhiChu.Contains(keyword))
                    || (x.MaHoaDonNavigation.TrangThai != null &&
                        x.MaHoaDonNavigation.TrangThai.Contains(keyword))
                    || x.MaHoaDonNavigation.MaHocVienNavigation.HoTen.Contains(keyword)
                    || x.MaHoaDonNavigation.MaLopNavigation.TenLop.Contains(keyword)
                    || x.MaHoaDonNavigation.MaLopNavigation.MaLopCode.Contains(keyword));
            }

            var totalItems = await query.CountAsync();
            var data = await query
                .OrderBy(x => x.MaChiTietHoaDon)
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .AsNoTracking()
                .ToListAsync();

            return new
            {
                PageNumber = pageNumber,
                PageSize = pageSize,
                TotalItems = totalItems,
                TotalPages = (int)Math.Ceiling((double)totalItems / pageSize),
                Data = data
            };
        }

        // =====================================================
        // EXISTS
        // =====================================================

        public async Task<bool> ExistsByIdAsync(int id)
        {
            return await _context.ChiTietHoaDons
                .AnyAsync(x => x.MaChiTietHoaDon == id);
        }

        // =====================================================
        // ADD
        // =====================================================

        public async Task<ChiTietHoaDon> AddAsync(ChiTietHoaDon chiTietHoaDon)
        {
            await _context.ChiTietHoaDons.AddAsync(chiTietHoaDon);
            await _context.SaveChangesAsync();
            return chiTietHoaDon;
        }

        // =====================================================
        // UPDATE
        // =====================================================

        public async Task<ChiTietHoaDon?> UpdateAsync(ChiTietHoaDon chiTietHoaDon)
        {
            var existing = await _context.ChiTietHoaDons
                .FirstOrDefaultAsync(x => x.MaChiTietHoaDon == chiTietHoaDon.MaChiTietHoaDon);

            if (existing == null)
                return null;

            existing.MaHoaDon = chiTietHoaDon.MaHoaDon;
            existing.SoLuong = chiTietHoaDon.SoLuong;
            existing.DonGia = chiTietHoaDon.DonGia;
            existing.ThanhTien = chiTietHoaDon.ThanhTien;
            existing.GhiChu = chiTietHoaDon.GhiChu;

            await _context.SaveChangesAsync();

            return existing;
        }

        // =====================================================
        // DELETE
        // =====================================================

        public async Task<bool> DeleteAsync(int id)
        {
            var entity = await _context.ChiTietHoaDons
                .FirstOrDefaultAsync(x => x.MaChiTietHoaDon == id);

            if (entity == null)
                return false;

            _context.ChiTietHoaDons.Remove(entity);
            await _context.SaveChangesAsync();

            return true;
        }
    }
}
