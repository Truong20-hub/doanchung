using DAL.Context;
using DAL.Entities;
using DAL.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace DAL.Repositories
{
    public class BaiTapRepository : IBaiTapRepository
    {
        private readonly AppDbContext _context;

        public BaiTapRepository(AppDbContext context)
        {
            _context = context;
        }

        // Query gốc: include lớp, buổi, giáo viên (+ người dùng để lấy họ tên)
        private IQueryable<BaiTap> BaseQuery()
        {
            return _context.BaiTaps
                .Include(x => x.MaLopNavigation)
                .Include(x => x.MaBuoiNavigation)
                .Include(x => x.MaGiaoVienNavigation)
                    .ThenInclude(g => g.MaNguoiDungNavigation);
        }

        private static IQueryable<BaiTap> ApplySearch(
            IQueryable<BaiTap> query,
            string? keyword)
        {
            if (string.IsNullOrWhiteSpace(keyword))
                return query;

            keyword = keyword.Trim();

            return query.Where(x =>
                x.TieuDe.Contains(keyword) ||
                (x.MoTa != null && x.MoTa.Contains(keyword)) ||
                x.MaLopNavigation.TenLop.Contains(keyword) ||
                x.MaLopNavigation.MaLopCode.Contains(keyword));
        }

        // =====================================================
        // GET ALL
        // =====================================================

        public async Task<IEnumerable<BaiTap>> GetAllAsync()
        {
            return await BaseQuery()
                .AsNoTracking()
                .OrderByDescending(x => x.NgayGiao)
                .ThenByDescending(x => x.MaBaiTap)
                .ToListAsync();
        }

        // =====================================================
        // GET BY ID
        // =====================================================

        public async Task<BaiTap?> GetByIdAsync(int id)
        {
            return await BaseQuery()
                .FirstOrDefaultAsync(x => x.MaBaiTap == id);
        }

        // =====================================================
        // GET BY LỚP
        // =====================================================

        public async Task<IEnumerable<BaiTap>> GetByMaLopAsync(int maLop)
        {
            return await BaseQuery()
                .Where(x => x.MaLop == maLop)
                .AsNoTracking()
                .OrderByDescending(x => x.NgayGiao)
                .ThenByDescending(x => x.MaBaiTap)
                .ToListAsync();
        }

        // =====================================================
        // GET BY GIÁO VIÊN
        // =====================================================

        public async Task<IEnumerable<BaiTap>> GetByMaGiaoVienAsync(
            int maGiaoVien)
        {
            return await BaseQuery()
                .Where(x => x.MaGiaoVien == maGiaoVien)
                .AsNoTracking()
                .OrderByDescending(x => x.NgayGiao)
                .ThenByDescending(x => x.MaBaiTap)
                .ToListAsync();
        }

        // =====================================================
        // GET BY HỌC VIÊN
        // =====================================================

        public async Task<IEnumerable<BaiTap>> GetByMaHocVienAsync(
            int maHocVien)
        {
            var maLops = _context.DangKyHocs
                .Where(d => d.MaHocVien == maHocVien)
                .Select(d => d.MaLop);

            return await BaseQuery()
                // Chỉ nạp bài nộp của đúng học viên này
                .Include(x => x.Diems.Where(d => d.MaHocVien == maHocVien))
                .Where(x => maLops.Contains(x.MaLop))
                .AsNoTracking()
                .OrderBy(x => x.HanNop == null)
                .ThenBy(x => x.HanNop)
                .ThenByDescending(x => x.MaBaiTap)
                .ToListAsync();
        }

        // =====================================================
        // SEARCH
        // =====================================================

        public async Task<IEnumerable<BaiTap>> SearchAsync(string? keyword)
        {
            return await ApplySearch(BaseQuery(), keyword)
                .AsNoTracking()
                .OrderByDescending(x => x.NgayGiao)
                .ThenByDescending(x => x.MaBaiTap)
                .ToListAsync();
        }

        // =====================================================
        // SEARCH + PAGED
        // =====================================================

        public async Task<(IEnumerable<BaiTap> Data, int TotalItems)>
            SearchPagedAsync(
                string? keyword,
                int pageNumber,
                int pageSize)
        {
            var query = ApplySearch(BaseQuery(), keyword);

            var totalItems = await query.CountAsync();

            var data = await query
                .OrderByDescending(x => x.NgayGiao)
                .ThenByDescending(x => x.MaBaiTap)
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .AsNoTracking()
                .ToListAsync();

            return (data, totalItems);
        }

        // =====================================================
        // EXISTS
        // =====================================================

        public async Task<bool> ExistsByIdAsync(int id)
        {
            return await _context.BaiTaps
                .AnyAsync(x => x.MaBaiTap == id);
        }

        public async Task<bool> ExistsByTieuDeAsync(
            int maLop,
            string tieuDe,
            int? excludeId = null)
        {
            return await _context.BaiTaps
                .AnyAsync(x =>
                    x.MaLop == maLop &&
                    x.TieuDe == tieuDe &&
                    (!excludeId.HasValue ||
                     x.MaBaiTap != excludeId.Value));
        }

        // =====================================================
        // ADD
        // =====================================================

        public async Task<BaiTap> AddAsync(BaiTap baiTap)
        {
            await _context.BaiTaps.AddAsync(baiTap);
            await _context.SaveChangesAsync();
            return baiTap;
        }

        // =====================================================
        // UPDATE
        // =====================================================

        public async Task<BaiTap?> UpdateAsync(BaiTap baiTap)
        {
            var existing = await _context.BaiTaps
                .FirstOrDefaultAsync(x => x.MaBaiTap == baiTap.MaBaiTap);

            if (existing == null)
                return null;

            existing.MaLop = baiTap.MaLop;
            existing.MaBuoi = baiTap.MaBuoi;
            existing.MaGiaoVien = baiTap.MaGiaoVien;
            existing.TieuDe = baiTap.TieuDe;
            existing.MoTa = baiTap.MoTa;
            existing.NgayGiao = baiTap.NgayGiao;
            existing.HanNop = baiTap.HanNop;
            existing.FileDinhKem = baiTap.FileDinhKem;

            await _context.SaveChangesAsync();

            return existing;
        }

        // =====================================================
        // DELETE
        // =====================================================

        public async Task<bool> DeleteAsync(int id)
        {
            var entity = await _context.BaiTaps
                .FirstOrDefaultAsync(x => x.MaBaiTap == id);

            if (entity == null)
                return false;

            _context.BaiTaps.Remove(entity);
            await _context.SaveChangesAsync();

            return true;
        }
    }
}
