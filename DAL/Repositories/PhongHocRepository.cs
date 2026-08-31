using DAL.Context;
using DAL.Entities;
using DAL.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace DAL.Repositories
{
    public class PhongHocRepository : IPhongHocRepository
    {
        private readonly AppDbContext _context;

        public PhongHocRepository(AppDbContext context)
        {
            _context = context;
        }

        // =====================================================
        // 1. LẤY TẤT CẢ PHÒNG HỌC
        // =====================================================

        public async Task<IEnumerable<PhongHoc>> GetAllAsync()
        {
            return await _context.PhongHocs
                .OrderBy(x => x.MaPhong)
                .ToListAsync();
        }

        // =====================================================
        // 2. LẤY TẤT CẢ + PHÂN TRANG
        // =====================================================

        public async Task<object> GetPagedAsync(
            int pageNumber,
            int pageSize)
        {
            var query = _context.PhongHocs
                .OrderBy(x => x.MaPhong)
                .AsQueryable();

            var totalRecords = await query.CountAsync();

            var data = await query
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            return new
            {
                PageNumber = pageNumber,
                PageSize = pageSize,
                TotalRecords = totalRecords,
                TotalPages = (int)Math.Ceiling(
                    (double)totalRecords / pageSize),
                Data = data
            };
        }

        // =====================================================
        // 3. LẤY PHÒNG HỌC THEO ID
        // =====================================================

        public async Task<PhongHoc?> GetByIdAsync(int id)
        {
            return await _context.PhongHocs
                .FirstOrDefaultAsync(x => x.MaPhong == id);
        }

        // =====================================================
        // 4. KIỂM TRA PHÒNG HỌC CÓ TỒN TẠI THEO ID
        // =====================================================

        public async Task<bool> ExistsByIdAsync(int id)
        {
            return await _context.PhongHocs
                .AnyAsync(x => x.MaPhong == id);
        }

        // =====================================================
        // 5. KIỂM TRA TÊN PHÒNG ĐÃ TỒN TẠI CHƯA
        // =====================================================

        public async Task<bool> ExistsByTenPhongAsync(
            string tenPhong)
        {
            return await _context.PhongHocs
                .AnyAsync(x =>
                    x.TenPhong.ToLower() == tenPhong.ToLower());
        }

        // =====================================================
        // 6. LẤY PHÒNG HỌC THEO TÊN
        // =====================================================

        public async Task<IEnumerable<PhongHoc>> GetByNameAsync(
            string tenPhong)
        {
            return await _context.PhongHocs
                .Where(x =>
                    x.TenPhong.Contains(tenPhong))
                .OrderBy(x => x.MaPhong)
                .ToListAsync();
        }

        // =====================================================
        // 7. LẤY THEO TÊN + PHÂN TRANG
        // =====================================================

        public async Task<object> GetPagedByNameAsync(
            string tenPhong,
            int pageNumber,
            int pageSize)
        {
            var query = _context.PhongHocs
                .Where(x =>
                    x.TenPhong.Contains(tenPhong))
                .OrderBy(x => x.MaPhong)
                .AsQueryable();

            var totalRecords = await query.CountAsync();

            var data = await query
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            return new
            {
                PageNumber = pageNumber,
                PageSize = pageSize,
                TotalRecords = totalRecords,
                TotalPages = (int)Math.Ceiling(
                    (double)totalRecords / pageSize),
                Data = data
            };
        }

        // =====================================================
        // 8. LẤY THEO SỨC CHỨA
        // =====================================================

        public async Task<IEnumerable<PhongHoc>> GetBySucChuaAsync(
            int sucChua)
        {
            return await _context.PhongHocs
                .Where(x => x.SucChua == sucChua)
                .OrderBy(x => x.MaPhong)
                .ToListAsync();
        }

        // =====================================================
        // 9. LẤY THEO SỨC CHỨA + PHÂN TRANG
        // =====================================================

        public async Task<object> GetPagedBySucChuaAsync(
            int sucChua,
            int pageNumber,
            int pageSize)
        {
            var query = _context.PhongHocs
                .Where(x => x.SucChua == sucChua)
                .OrderBy(x => x.MaPhong)
                .AsQueryable();

            var totalRecords = await query.CountAsync();

            var data = await query
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            return new
            {
                PageNumber = pageNumber,
                PageSize = pageSize,
                TotalRecords = totalRecords,
                TotalPages = (int)Math.Ceiling(
                    (double)totalRecords / pageSize),
                Data = data
            };
        }

        // =====================================================
        // 10. LẤY THEO VỊ TRÍ
        // =====================================================

        public async Task<IEnumerable<PhongHoc>> GetByViTriAsync(
            string viTri)
        {
            return await _context.PhongHocs
                .Where(x =>
                    x.ViTri != null &&
                    x.ViTri.Contains(viTri))
                .OrderBy(x => x.MaPhong)
                .ToListAsync();
        }

        // =====================================================
        // 11. LẤY THEO VỊ TRÍ + PHÂN TRANG
        // =====================================================

        public async Task<object> GetPagedByViTriAsync(
            string viTri,
            int pageNumber,
            int pageSize)
        {
            var query = _context.PhongHocs
                .Where(x =>
                    x.ViTri != null &&
                    x.ViTri.Contains(viTri))
                .OrderBy(x => x.MaPhong)
                .AsQueryable();

            var totalRecords = await query.CountAsync();

            var data = await query
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            return new
            {
                PageNumber = pageNumber,
                PageSize = pageSize,
                TotalRecords = totalRecords,
                TotalPages = (int)Math.Ceiling(
                    (double)totalRecords / pageSize),
                Data = data
            };
        }

        // =====================================================
        // 12. TÌM KIẾM
        // =====================================================

        public async Task<IEnumerable<PhongHoc>> SearchAsync(
            string keyword)
        {
            return await _context.PhongHocs
                .Where(x =>
                    x.TenPhong.Contains(keyword) ||
                    (x.ViTri != null &&
                     x.ViTri.Contains(keyword)) ||
                    (x.SucChua.HasValue &&
                     x.SucChua.Value.ToString().Contains(keyword)))
                .OrderBy(x => x.MaPhong)
                .ToListAsync();
        }

        // =====================================================
        // 13. TÌM KIẾM + PHÂN TRANG
        // =====================================================

        public async Task<object> SearchPagedAsync(
            string keyword,
            int pageNumber,
            int pageSize)
        {
            var query = _context.PhongHocs
                .Where(x =>
                    x.TenPhong.Contains(keyword) ||
                    (x.ViTri != null &&
                     x.ViTri.Contains(keyword)) ||
                    (x.SucChua.HasValue &&
                     x.SucChua.Value.ToString().Contains(keyword)))
                .OrderBy(x => x.MaPhong)
                .AsQueryable();

            var totalRecords = await query.CountAsync();

            var data = await query
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            return new
            {
                PageNumber = pageNumber,
                PageSize = pageSize,
                TotalRecords = totalRecords,
                TotalPages = (int)Math.Ceiling(
                    (double)totalRecords / pageSize),
                Data = data
            };
        }

        // =====================================================
        // 14. THÊM PHÒNG HỌC
        // =====================================================

        public async Task<PhongHoc> AddAsync(
            PhongHoc phongHoc)
        {
            await _context.PhongHocs.AddAsync(phongHoc);

            await _context.SaveChangesAsync();

            return phongHoc;
        }

        // =====================================================
        // 15. CẬP NHẬT PHÒNG HỌC
        // =====================================================

        public async Task<PhongHoc?> UpdateAsync(
            PhongHoc phongHoc)
        {
            var existing = await _context.PhongHocs
                .FirstOrDefaultAsync(x =>
                    x.MaPhong == phongHoc.MaPhong);

            if (existing == null)
            {
                return null;
            }

            existing.TenPhong = phongHoc.TenPhong;
            existing.SucChua = phongHoc.SucChua;
            existing.ViTri = phongHoc.ViTri;

            await _context.SaveChangesAsync();

            return existing;
        }

        // =====================================================
        // 16. XÓA PHÒNG HỌC
        // =====================================================

        public async Task<bool> DeleteAsync(int id)
        {
            var phongHoc = await _context.PhongHocs
                .FirstOrDefaultAsync(x =>
                    x.MaPhong == id);

            if (phongHoc == null)
            {
                return false;
            }

            _context.PhongHocs.Remove(phongHoc);

            await _context.SaveChangesAsync();

            return true;
        }
    }
}