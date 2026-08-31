using DAL.Context;
using DAL.Entities;
using DAL.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace DAL.Repositories
{
    public class DiemDanhRepository : IDiemDanhRepository
    {
        private readonly AppDbContext _context;

        public DiemDanhRepository(
            AppDbContext context)
        {
            _context = context;
        }

        // =====================================================
        // GET ALL
        // =====================================================

        public async Task<IEnumerable<DiemDanh>>
            GetAllAsync()
        {
            return await _context.DiemDanhs
                .Include(x => x.MaBuoiNavigation)
                    .ThenInclude(x => x.MaLopNavigation)
                .Include(x => x.MaHocVienNavigation)
                .AsNoTracking()
                .OrderBy(x => x.MaDiemDanh)
                .ToListAsync();
        }

        // =====================================================
        // GET BY ID
        // =====================================================

        public async Task<DiemDanh?>
            GetByIdAsync(int id)
        {
            return await _context.DiemDanhs
                .Include(x => x.MaBuoiNavigation)
                    .ThenInclude(x => x.MaLopNavigation)
                .Include(x => x.MaHocVienNavigation)
                .FirstOrDefaultAsync(
                    x => x.MaDiemDanh == id);
        }

        // =====================================================
        // GET BY BUỔI
        // =====================================================

        public async Task<IEnumerable<DiemDanh>>
            GetByMaBuoiAsync(int maBuoi)
        {
            return await _context.DiemDanhs
                .Include(x => x.MaBuoiNavigation)
                    .ThenInclude(x => x.MaLopNavigation)
                .Include(x => x.MaHocVienNavigation)
                .Where(x => x.MaBuoi == maBuoi)
                .OrderBy(x => x.MaHocVien)
                .AsNoTracking()
                .ToListAsync();
        }

        // =====================================================
        // GET BY HỌC VIÊN
        // =====================================================

        public async Task<IEnumerable<DiemDanh>>
            GetByMaHocVienAsync(int maHocVien)
        {
            return await _context.DiemDanhs
                .Include(x => x.MaBuoiNavigation)
                    .ThenInclude(x => x.MaLopNavigation)
                .Include(x => x.MaHocVienNavigation)
                .Where(x => x.MaHocVien == maHocVien)
                .OrderByDescending(x => x.MaBuoiNavigation.NgayHoc)
                .AsNoTracking()
                .ToListAsync();
        }

        // =====================================================
        // EXISTS
        // =====================================================

        public async Task<bool>
            ExistsAsync(
                int maBuoi,
                int maHocVien)
        {
            return await _context.DiemDanhs
                .AnyAsync(x =>
                    x.MaBuoi == maBuoi &&
                    x.MaHocVien == maHocVien);
        }

        // =====================================================
        // PAGED
        // =====================================================

        public async Task<object>
            GetPagedAsync(
                int pageNumber,
                int pageSize)
        {
            var query = _context.DiemDanhs
                .Include(x => x.MaBuoiNavigation)
                    .ThenInclude(x => x.MaLopNavigation)
                .Include(x => x.MaHocVienNavigation)
                .AsNoTracking();

            var totalItems =
                await query.CountAsync();

            var data =
                await query
                    .OrderBy(x => x.MaDiemDanh)
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
        // SEARCH PAGED
        // =====================================================

        public async Task<object>
            SearchPagedAsync(
                int? maBuoi,
                int? maHocVien,
                string? trangThai,
                int pageNumber,
                int pageSize)
        {
            var query = _context.DiemDanhs
                .Include(x => x.MaBuoiNavigation)
                    .ThenInclude(x => x.MaLopNavigation)
                .Include(x => x.MaHocVienNavigation)
                .AsNoTracking()
                .AsQueryable();

            if (maBuoi.HasValue)
            {
                query = query.Where(
                    x => x.MaBuoi ==
                         maBuoi.Value);
            }

            if (maHocVien.HasValue)
            {
                query = query.Where(
                    x => x.MaHocVien ==
                         maHocVien.Value);
            }

            if (!string.IsNullOrWhiteSpace(trangThai))
            {
                query = query.Where(
                    x => x.TrangThai ==
                         trangThai);
            }

            var totalItems =
                await query.CountAsync();

            var data =
                await query
                    .OrderBy(x => x.MaDiemDanh)
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

        public async Task<DiemDanh>
            AddAsync(DiemDanh diemDanh)
        {
            await _context.DiemDanhs
                .AddAsync(diemDanh);

            await _context.SaveChangesAsync();

            return diemDanh;
        }

        // =====================================================
        // UPDATE
        // =====================================================

        public async Task<DiemDanh?>
            UpdateAsync(
                DiemDanh diemDanh)
        {
            var existing =
                await _context.DiemDanhs
                    .FirstOrDefaultAsync(
                        x => x.MaDiemDanh ==
                             diemDanh.MaDiemDanh);

            if (existing == null)
                return null;

            existing.MaBuoi =
                diemDanh.MaBuoi;

            existing.MaHocVien =
                diemDanh.MaHocVien;

            existing.TrangThai =
                diemDanh.TrangThai;

            existing.GhiChu =
                diemDanh.GhiChu;

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
                await _context.DiemDanhs
                    .FirstOrDefaultAsync(
                        x => x.MaDiemDanh == id);

            if (entity == null)
                return false;

            _context.DiemDanhs
                .Remove(entity);

            await _context.SaveChangesAsync();

            return true;
        }
    }
}