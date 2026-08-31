using DAL.Context;
using DAL.Entities;
using DAL.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace DAL.Repositories
{
    public class BuoiHocRepository : IBuoiHocRepository
    {
        private readonly AppDbContext _context;

        public BuoiHocRepository(AppDbContext context)
        {
            _context = context;
        }

        // =========================================
        // 1. LẤY TẤT CẢ
        // =========================================
        public async Task<IEnumerable<BuoiHoc>> GetAllAsync()
        {
            return await _context.BuoiHocs
                .Include(x => x.MaLopNavigation)
                .AsNoTracking()
                .OrderBy(x => x.NgayHoc)
                .ThenBy(x => x.GioBatDau)
                .ToListAsync();
        }

        // =========================================
        // 2. LẤY THEO ID
        // =========================================
        public async Task<BuoiHoc?> GetByIdAsync(int id)
        {
            return await _context.BuoiHocs
                .Include(x => x.MaLopNavigation)
                .FirstOrDefaultAsync(x => x.MaBuoi == id);
        }

        // =========================================
        // 3. KIỂM TRA ID
        // =========================================
        public async Task<bool> ExistsByIdAsync(int id)
        {
            return await _context.BuoiHocs
                .AnyAsync(x => x.MaBuoi == id);
        }

        // =========================================
        // 4. KIỂM TRA LỚP
        // =========================================
        public async Task<bool> ExistsByLopIdAsync(int maLop)
        {
            return await _context.LopHocs
                .AnyAsync(x => x.MaLop == maLop);
        }

        // =========================================
        // 5. THEO LỚP
        // =========================================
        // =========================================
        // 5. THEO LỚP
        // =========================================
        public async Task<IEnumerable<BuoiHoc>> GetByLopIdAsync(int maLop)
        {
            return await _context.BuoiHocs
                .Include(x => x.MaLopNavigation) // Thêm dòng này để nạp dữ liệu Lớp học
                .AsNoTracking()
                .Where(x => x.MaLop == maLop)
                .OrderBy(x => x.NgayHoc)
                .ThenBy(x => x.GioBatDau)
                .ToListAsync();
        }

        // =========================================
        // 6. THEO NGÀY
        // =========================================
        public async Task<IEnumerable<BuoiHoc>> GetByDateAsync(
            DateOnly ngayHoc)
        {
            return await _context.BuoiHocs
                .Include(x => x.MaLopNavigation)
                .AsNoTracking()
                .Where(x => x.NgayHoc == ngayHoc)
                .OrderBy(x => x.GioBatDau)
                .ToListAsync();
        }

        // =========================================
        // 7. THEO TRẠNG THÁI HỦY
        // =========================================
        public async Task<IEnumerable<BuoiHoc>> GetByBiHuyAsync(
            bool biHuy)
        {
            return await _context.BuoiHocs
                .Include(x => x.MaLopNavigation)
                .AsNoTracking()
                .Where(x => x.BiHuy == biHuy)
                .OrderBy(x => x.NgayHoc)
                .ToListAsync();
        }

        // =========================================
        // 8. THEO NỘI DUNG
        // =========================================
        public async Task<IEnumerable<BuoiHoc>> GetByNoiDungAsync(
            string noiDung)
        {
            return await _context.BuoiHocs
                .Include(x => x.MaLopNavigation)
                .AsNoTracking()
                .Where(x => x.NoiDung != null &&
                            x.NoiDung.Contains(noiDung))
                .OrderBy(x => x.NgayHoc)
                .ToListAsync();
        }

        // =========================================
        // 9. TÌM KIẾM
        // =========================================
        public async Task<IEnumerable<BuoiHoc>> SearchAsync(
            int? maLop,
            string? noiDung,
            DateOnly? ngayHoc,
            bool? biHuy)
        {
            IQueryable<BuoiHoc> query =
                _context.BuoiHocs.AsNoTracking();

            if (maLop.HasValue)
            {
                query = query.Where(x =>
                    x.MaLop == maLop.Value);
            }

            if (!string.IsNullOrWhiteSpace(noiDung))
            {
                query = query.Where(x =>
                    x.NoiDung != null &&
                    x.NoiDung.Contains(noiDung));
            }

            if (ngayHoc.HasValue)
            {
                query = query.Where(x =>
                    x.NgayHoc == ngayHoc.Value);
            }

            if (biHuy.HasValue)
            {
                query = query.Where(x =>
                    x.BiHuy == biHuy.Value);
            }

            return await query
                .OrderBy(x => x.NgayHoc)
                .ThenBy(x => x.GioBatDau)
                .ToListAsync();
        }

        // =========================================
        // 10. PHÂN TRANG
        // =========================================
        public async Task<object> GetPagedAsync(
            int pageNumber,
            int pageSize)
        {
            var query = _context.BuoiHocs
                .AsNoTracking()
                .OrderBy(x => x.NgayHoc)
                .ThenBy(x => x.GioBatDau);

            var totalItems = await query.CountAsync();

            var data = await query
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            return new
            {
                pageNumber,
                pageSize,
                totalItems,
                totalPages =
                    (int)Math.Ceiling(
                        totalItems / (double)pageSize),
                data
            };
        }

        // =========================================
        // 11. THEO LỚP + PHÂN TRANG
        // =========================================
        public async Task<object> GetPagedByLopIdAsync(
            int maLop,
            int pageNumber,
            int pageSize)
        {
            var query = _context.BuoiHocs
                .AsNoTracking()
                .Where(x => x.MaLop == maLop)
                .OrderBy(x => x.NgayHoc)
                .ThenBy(x => x.GioBatDau);

            var totalItems = await query.CountAsync();

            var data = await query
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            return new
            {
                maLop,
                pageNumber,
                pageSize,
                totalItems,
                totalPages =
                    (int)Math.Ceiling(
                        totalItems / (double)pageSize),
                data
            };
        }

        // =========================================
        // 12. THEO NGÀY + PHÂN TRANG
        // =========================================
        public async Task<object> GetPagedByDateAsync(
            DateOnly ngayHoc,
            int pageNumber,
            int pageSize)
        {
            var query = _context.BuoiHocs
                .AsNoTracking()
                .Where(x => x.NgayHoc == ngayHoc)
                .OrderBy(x => x.GioBatDau);

            var totalItems = await query.CountAsync();

            var data = await query
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            return new
            {
                ngayHoc,
                pageNumber,
                pageSize,
                totalItems,
                totalPages =
                    (int)Math.Ceiling(
                        totalItems / (double)pageSize),
                data
            };
        }

        // =========================================
        // 13. SEARCH + PHÂN TRANG
        // =========================================
        public async Task<object> SearchPagedAsync(
            int? maLop,
            string? noiDung,
            DateOnly? ngayHoc,
            bool? biHuy,
            int pageNumber,
            int pageSize)
        {
            IQueryable<BuoiHoc> query =
                _context.BuoiHocs.AsNoTracking();

            if (maLop.HasValue)
            {
                query = query.Where(x =>
                    x.MaLop == maLop.Value);
            }

            if (!string.IsNullOrWhiteSpace(noiDung))
            {
                query = query.Where(x =>
                    x.NoiDung != null &&
                    x.NoiDung.Contains(noiDung));
            }

            if (ngayHoc.HasValue)
            {
                query = query.Where(x =>
                    x.NgayHoc == ngayHoc.Value);
            }

            if (biHuy.HasValue)
            {
                query = query.Where(x =>
                    x.BiHuy == biHuy.Value);
            }

            query = query
                .OrderBy(x => x.NgayHoc)
                .ThenBy(x => x.GioBatDau);

            var totalItems = await query.CountAsync();

            var data = await query
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            return new
            {
                maLop,
                noiDung,
                ngayHoc,
                biHuy,
                pageNumber,
                pageSize,
                totalItems,
                totalPages =
                    (int)Math.Ceiling(
                        totalItems / (double)pageSize),
                data
            };
        }

        // =========================================
        // 14. THÊM
        // =========================================
        public async Task<BuoiHoc> AddAsync(
            BuoiHoc buoiHoc)
        {
            await _context.BuoiHocs.AddAsync(buoiHoc);
            await _context.SaveChangesAsync();

            return buoiHoc;
        }

        // =========================================
        // 15. CẬP NHẬT
        // =========================================
        public async Task<BuoiHoc?> UpdateAsync(
            BuoiHoc buoiHoc)
        {
            var existing = await _context.BuoiHocs
                .FirstOrDefaultAsync(
                    x => x.MaBuoi == buoiHoc.MaBuoi);

            if (existing == null)
                return null;

            existing.MaLop = buoiHoc.MaLop;
            existing.NgayHoc = buoiHoc.NgayHoc;
            existing.GioBatDau = buoiHoc.GioBatDau;
            existing.GioKetThuc = buoiHoc.GioKetThuc;
            existing.NoiDung = buoiHoc.NoiDung;
            existing.BiHuy = buoiHoc.BiHuy;

            await _context.SaveChangesAsync();

            return existing;
        }

        // =========================================
        // 16. XÓA
        // =========================================
        public async Task<bool> DeleteAsync(int id)
        {
            var buoiHoc = await _context.BuoiHocs
                .FirstOrDefaultAsync(x => x.MaBuoi == id);

            if (buoiHoc == null)
                return false;

            _context.BuoiHocs.Remove(buoiHoc);

            await _context.SaveChangesAsync();

            return true;
        }
    }
}