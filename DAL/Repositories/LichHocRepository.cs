using DAL.Context;
using DAL.Entities;
using DAL.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace DAL.Repositories
{
    public class LichHocRepository : ILichHocRepository
    {
        private readonly AppDbContext _context;

        public LichHocRepository(
            AppDbContext context)
        {
            _context = context;
        }

        // =====================================================
        // LẤY TẤT CẢ
        // =====================================================

        public async Task<IEnumerable<LichHoc>> GetAllAsync()
        {
            return await _context.LichHocs
                .Include(x => x.MaLopNavigation)
                .AsNoTracking()
                .ToListAsync();
        }

        // =====================================================
        // LẤY THEO ID
        // =====================================================

        public async Task<LichHoc?> GetByIdAsync(int id)
        {
            return await _context.LichHocs
                .Include(x => x.MaLopNavigation)
                .FirstOrDefaultAsync(x => x.MaLich == id);
        }

        // =====================================================
        // LẤY THEO MÃ LỚP
        // =====================================================

        public async Task<IEnumerable<LichHoc>> GetByMaLopAsync(
            int maLop)
        {
            return await _context.LichHocs
                .Include(x => x.MaLopNavigation)
                .Where(x => x.MaLop == maLop)
                .AsNoTracking()
                .ToListAsync();
        }

        // =====================================================
        // LẤY THEO THỨ
        // =====================================================

        public async Task<IEnumerable<LichHoc>> GetByThuAsync(
            string thuTrongTuan)
        {
            return await _context.LichHocs
                .Include(x => x.MaLopNavigation)
                .Where(x =>
                    x.ThuTrongTuan.Contains(thuTrongTuan))
                .AsNoTracking()
                .ToListAsync();
        }

        // =====================================================
        // LẤY THEO LỚP + THỨ
        // =====================================================

        public async Task<IEnumerable<LichHoc>>
            GetByMaLopAndThuAsync(
                int maLop,
                string thuTrongTuan)
        {
            return await _context.LichHocs
                .Include(x => x.MaLopNavigation)
                .Where(x =>
                    x.MaLop == maLop &&
                    x.ThuTrongTuan == thuTrongTuan)
                .AsNoTracking()
                .ToListAsync();
        }

        // =====================================================
        // KIỂM TRA TỒN TẠI
        // =====================================================

        public async Task<bool> ExistsByIdAsync(int id)
        {
            return await _context.LichHocs
                .AnyAsync(x => x.MaLich == id);
        }

        // =====================================================
        // KIỂM TRA TRÙNG LỊCH
        // =====================================================

        public async Task<bool> ExistsScheduleAsync(
            int maLop,
            string thuTrongTuan,
            TimeOnly gioBatDau,
            TimeOnly gioKetThuc,
            int? excludeId = null)
        {
            return await _context.LichHocs
                .AnyAsync(x =>
                    x.MaLop == maLop &&
                    x.ThuTrongTuan == thuTrongTuan &&

                    (
                        gioBatDau < x.GioKetThuc &&
                        gioKetThuc > x.GioBatDau
                    ) &&

                    (!excludeId.HasValue ||
                     x.MaLich != excludeId.Value));
        }

        // =====================================================
        // THÊM
        // =====================================================

        public async Task<LichHoc> AddAsync(
            LichHoc lichHoc)
        {
            await _context.LichHocs.AddAsync(lichHoc);
            await _context.SaveChangesAsync();

            return lichHoc;
        }

        // =====================================================
        // CẬP NHẬT
        // =====================================================

        public async Task<LichHoc?> UpdateAsync(
            LichHoc lichHoc)
        {
            var existing = await _context.LichHocs
                .FirstOrDefaultAsync(
                    x => x.MaLich == lichHoc.MaLich);

            if (existing == null)
                return null;

            existing.MaLop = lichHoc.MaLop;
            existing.ThuTrongTuan = lichHoc.ThuTrongTuan;
            existing.GioBatDau = lichHoc.GioBatDau;
            existing.GioKetThuc = lichHoc.GioKetThuc;

            await _context.SaveChangesAsync();

            return existing;
        }

        // =====================================================
        // XÓA
        // =====================================================

        public async Task<bool> DeleteAsync(int id)
        {
            var entity = await _context.LichHocs
                .FirstOrDefaultAsync(x => x.MaLich == id);

            if (entity == null)
                return false;

            _context.LichHocs.Remove(entity);

            await _context.SaveChangesAsync();

            return true;
        }

        // =====================================================
        // PHÂN TRANG
        // =====================================================

        public async Task<object> GetPagedAsync(
            int pageNumber,
            int pageSize)
        {
            var query = _context.LichHocs
                .Include(x => x.MaLopNavigation)
                .AsNoTracking();

            var totalItems = await query.CountAsync();

            var data = await query
                .OrderBy(x => x.MaLich)
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
        // TÌM KIẾM
        // =====================================================

        public async Task<IEnumerable<LichHoc>> SearchAsync(
            string? keyword)
        {
            var query = _context.LichHocs
                .Include(x => x.MaLopNavigation)
                .AsQueryable();

            if (!string.IsNullOrWhiteSpace(keyword))
            {
                query = query.Where(x =>
                    x.ThuTrongTuan.Contains(keyword) ||
                    x.MaLopNavigation.TenLop.Contains(keyword) ||
                    x.MaLopNavigation.MaLopCode.Contains(keyword));
            }

            return await query
                .AsNoTracking()
                .ToListAsync();
        }

        // =====================================================
        // TÌM KIẾM + PHÂN TRANG
        // =====================================================

        public async Task<object> SearchPagedAsync(
            string? keyword,
            int pageNumber,
            int pageSize)
        {
            var query = _context.LichHocs
                .Include(x => x.MaLopNavigation)
                .AsQueryable();

            if (!string.IsNullOrWhiteSpace(keyword))
            {
                query = query.Where(x =>
                    x.ThuTrongTuan.Contains(keyword) ||
                    x.MaLopNavigation.TenLop.Contains(keyword) ||
                    x.MaLopNavigation.MaLopCode.Contains(keyword));
            }

            var totalItems = await query.CountAsync();

            var data = await query
                .OrderBy(x => x.MaLich)
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
    }
}