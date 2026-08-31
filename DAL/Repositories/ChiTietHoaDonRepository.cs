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
                .AsNoTracking()
                .ToListAsync();
        }

        // =====================================================
        // GET BY ID
        // =====================================================

        public async Task<ChiTietHoaDon?> GetByIdAsync(int id)
        {
            return await _context.ChiTietHoaDons
                .Include(x => x.MaHoaDonNavigation)
                .FirstOrDefaultAsync(x => x.MaChiTietHoaDon == id);
        }

        // =====================================================
        // GET BY MÃ HÓA ĐƠN
        // =====================================================

        public async Task<IEnumerable<ChiTietHoaDon>> GetByMaHoaDonAsync(int maHoaDon)
        {
            return await _context.ChiTietHoaDons
                .Include(x => x.MaHoaDonNavigation)
                .Where(x => x.MaHoaDon == maHoaDon)
                .AsNoTracking()
                .ToListAsync();
        }

        // =====================================================
        // SEARCH
        // =====================================================

        public async Task<IEnumerable<ChiTietHoaDon>> SearchAsync(string? keyword)
        {
            var query = _context.ChiTietHoaDons
                .Include(x => x.MaHoaDonNavigation)
                .AsQueryable();

            if (!string.IsNullOrWhiteSpace(keyword))
            {
                keyword = keyword.Trim();

                query = query.Where(x =>
                    x.GhiChu != null && x.GhiChu.Contains(keyword)
                    || x.MaHoaDonNavigation.TrangThai != null && x.MaHoaDonNavigation.TrangThai.Contains(keyword));
            }

            return await query
                .AsNoTracking()
                .ToListAsync();
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
