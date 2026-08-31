using DAL.Context;
using DAL.Entities;
using DAL.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace DAL.Repositories
{
    public class HoaDonRepository : IHoaDonRepository
    {
        private readonly AppDbContext _context;

        public HoaDonRepository(
            AppDbContext context)
        {
            _context = context;
        }

        // =====================================================
        // GET ALL
        // =====================================================

        public async Task<IEnumerable<HoaDon>>
            GetAllAsync()
        {
            return await _context.HoaDons
                .Include(x => x.MaHocVienNavigation)
                .Include(x => x.MaLopNavigation)
                .AsNoTracking()
                .ToListAsync();
        }

        // =====================================================
        // GET BY ID
        // =====================================================

        public async Task<HoaDon?> GetByIdAsync(
            int id)
        {
            return await _context.HoaDons
                .Include(x => x.MaHocVienNavigation)
                .Include(x => x.MaLopNavigation)
                .FirstOrDefaultAsync(
                    x => x.MaHoaDon == id);
        }

        // =====================================================
        // GET BY HỌC VIÊN
        // =====================================================

        public async Task<IEnumerable<HoaDon>>
            GetByMaHocVienAsync(
                int maHocVien)
        {
            return await _context.HoaDons
                .Include(x => x.MaHocVienNavigation)
                .Include(x => x.MaLopNavigation)
                .Where(x =>
                    x.MaHocVien == maHocVien)
                .AsNoTracking()
                .ToListAsync();
        }

        // =====================================================
        // GET BY LỚP
        // =====================================================

        public async Task<IEnumerable<HoaDon>>
            GetByMaLopAsync(
                int maLop)
        {
            return await _context.HoaDons
                .Include(x => x.MaHocVienNavigation)
                .Include(x => x.MaLopNavigation)
                .Where(x =>
                    x.MaLop == maLop)
                .AsNoTracking()
                .ToListAsync();
        }

        // =====================================================
        // GET BY TRẠNG THÁI
        // =====================================================

        public async Task<IEnumerable<HoaDon>>
            GetByTrangThaiAsync(
                string trangThai)
        {
            return await _context.HoaDons
                .Include(x => x.MaHocVienNavigation)
                .Include(x => x.MaLopNavigation)
                .Where(x =>
                    x.TrangThai == trangThai)
                .AsNoTracking()
                .ToListAsync();
        }

        // =====================================================
        // SEARCH
        // =====================================================

        public async Task<IEnumerable<HoaDon>>
            SearchAsync(string? keyword)
        {
            var query = _context.HoaDons
                .Include(x => x.MaHocVienNavigation)
                .Include(x => x.MaLopNavigation)
                .AsQueryable();

            if (!string.IsNullOrWhiteSpace(keyword))
            {
                keyword = keyword.Trim();

                query = query.Where(x =>
                    x.MaHocVienNavigation.HoTen
                        .Contains(keyword)

                    || x.MaLopNavigation.TenLop
                        .Contains(keyword)

                    || x.MaLopNavigation.MaLopCode
                        .Contains(keyword)

                    || (x.TrangThai != null &&
                        x.TrangThai.Contains(keyword)));
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
            var query = _context.HoaDons
                .Include(x => x.MaHocVienNavigation)
                .Include(x => x.MaLopNavigation)
                .AsNoTracking();

            var totalItems =
                await query.CountAsync();

            var data = await query
                .OrderBy(x => x.MaHoaDon)
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
                        (double)totalItems /
                        pageSize),

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
            var query = _context.HoaDons
                .Include(x => x.MaHocVienNavigation)
                .Include(x => x.MaLopNavigation)
                .AsQueryable();

            if (!string.IsNullOrWhiteSpace(keyword))
            {
                keyword = keyword.Trim();

                query = query.Where(x =>
                    x.MaHocVienNavigation.HoTen
                        .Contains(keyword)

                    || x.MaLopNavigation.TenLop
                        .Contains(keyword)

                    || x.MaLopNavigation.MaLopCode
                        .Contains(keyword)

                    || (x.TrangThai != null &&
                        x.TrangThai.Contains(keyword)));
            }

            var totalItems =
                await query.CountAsync();

            var data = await query
                .OrderBy(x => x.MaHoaDon)
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
                        (double)totalItems /
                        pageSize),

                Data = data
            };
        }

        // =====================================================
        // EXISTS
        // =====================================================

        public async Task<bool> ExistsByIdAsync(
            int id)
        {
            return await _context.HoaDons
                .AnyAsync(
                    x => x.MaHoaDon == id);
        }

        // =====================================================
        // KIỂM TRA CÓ THANH TOÁN
        // =====================================================

        public async Task<bool> HasThanhToanAsync(
            int maHoaDon)
        {
            return await _context.ThanhToans
                .AnyAsync(
                    x => x.MaHoaDon == maHoaDon);
        }

        // =====================================================
        // ADD
        // =====================================================

        public async Task<HoaDon> AddAsync(
            HoaDon hoaDon)
        {
            await _context.HoaDons
                .AddAsync(hoaDon);

            await _context.SaveChangesAsync();

            return hoaDon;
        }

        // =====================================================
        // UPDATE
        // =====================================================

        public async Task<HoaDon?> UpdateAsync(
            HoaDon hoaDon)
        {
            var existing =
                await _context.HoaDons
                    .FirstOrDefaultAsync(
                        x => x.MaHoaDon ==
                             hoaDon.MaHoaDon);

            if (existing == null)
                return null;

            existing.MaHocVien =
                hoaDon.MaHocVien;

            existing.MaLop =
                hoaDon.MaLop;

            existing.TongTien =
                hoaDon.TongTien;

            existing.GiamGia =
                hoaDon.GiamGia;

            existing.ThanhTien =
                hoaDon.ThanhTien;

            existing.NgayLap =
                hoaDon.NgayLap;

            existing.HanThanhToan =
                hoaDon.HanThanhToan;

            existing.TrangThai =
                hoaDon.TrangThai;

            await _context.SaveChangesAsync();

            return existing;
        }

        // =====================================================
        // DELETE
        // =====================================================

        public async Task<bool> DeleteAsync(
            int id)
        {
            var entity =
                await _context.HoaDons
                    .FirstOrDefaultAsync(
                        x => x.MaHoaDon == id);

            if (entity == null)
                return false;

            _context.HoaDons.Remove(entity);

            await _context.SaveChangesAsync();

            return true;
        }
    }
}