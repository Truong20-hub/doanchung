using DAL.Context;
using DAL.Entities;
using DAL.Interfaces;
using DTO;
using Microsoft.EntityFrameworkCore;

namespace DAL.Repositories
{
    public class LopHocRepository : ILopHocRepository
    {
        private readonly AppDbContext _context;

        public LopHocRepository(AppDbContext context)
        {
            _context = context;
        }


        // =====================================================
        // QUERY CƠ SỞ
        // Dùng lại cho các phương thức cần thông tin liên quan
        // =====================================================

        private IQueryable<LopHoc> GetBaseQuery()
        {
            return _context.LopHocs
                .Include(x => x.MaKhoaHocNavigation)
                .Include(x => x.MaGiaoVienNavigation)
                .Include(x => x.MaPhongNavigation)
                .AsQueryable();
        }


        // =====================================================
        // 1. LẤY TẤT CẢ LỚP HỌC
        // =====================================================

        public async Task<IEnumerable<LopHoc>> GetAllAsync()
        {
            return await GetBaseQuery()
                .OrderBy(x => x.MaLop)
                .ToListAsync();
        }


        // =====================================================
        // 2. LẤY LỚP HỌC THEO ID
        // =====================================================

        public async Task<LopHoc?> GetByIdAsync(int id)
        {
            return await GetBaseQuery()
                .FirstOrDefaultAsync(x => x.MaLop == id);
        }


        // =====================================================
        // 3. KIỂM TRA ID
        // =====================================================

        public async Task<bool> ExistsByIdAsync(int id)
        {
            return await _context.LopHocs
                .AnyAsync(x => x.MaLop == id);
        }


        // =====================================================
        // 4. KIỂM TRA MÃ LỚP CODE
        // =====================================================

        public async Task<bool> ExistsByMaLopCodeAsync(string maLopCode)
        {
            return await _context.LopHocs
                .AnyAsync(x => x.MaLopCode == maLopCode);
        }


        // =====================================================
        // 5. KIỂM TRA TÊN LỚP
        // =====================================================

        public async Task<bool> ExistsByTenLopAsync(string tenLop)
        {
            return await _context.LopHocs
                .AnyAsync(x => x.TenLop == tenLop);
        }


        // =====================================================
        // 6. KIỂM TRA KHÓA HỌC
        // =====================================================

        public async Task<bool> ExistsByKhoaHocIdAsync(int maKhoaHoc)
        {
            return await _context.KhoaHocs
                .AnyAsync(x => x.MaKhoaHoc == maKhoaHoc);
        }


        // =====================================================
        // 7. KIỂM TRA GIÁO VIÊN
        // =====================================================

        public async Task<bool> ExistsByGiaoVienIdAsync(int maGiaoVien)
        {
            return await _context.GiaoViens
                .AnyAsync(x => x.MaGiaoVien == maGiaoVien);
        }


        // =====================================================
        // 8. KIỂM TRA PHÒNG HỌC
        // =====================================================

        public async Task<bool> ExistsByPhongHocIdAsync(int maPhong)
        {
            return await _context.PhongHocs
                .AnyAsync(x => x.MaPhong == maPhong);
        }


        // =====================================================
        // 9. THÊM LỚP HỌC
        // =====================================================

        public async Task<LopHoc> AddAsync(LopHoc lopHoc)
        {
            await _context.LopHocs.AddAsync(lopHoc);

            await _context.SaveChangesAsync();

            return lopHoc;
        }


        // =====================================================
        // 10. CẬP NHẬT LỚP HỌC
        // =====================================================

        public async Task<LopHoc?> UpdateAsync(LopHoc lopHoc)
        {
            var existing = await _context.LopHocs
                .FirstOrDefaultAsync(x => x.MaLop == lopHoc.MaLop);

            if (existing == null)
            {
                return null;
            }

            existing.MaLopCode = lopHoc.MaLopCode;
            existing.TenLop = lopHoc.TenLop;
            existing.MaKhoaHoc = lopHoc.MaKhoaHoc;
            existing.MaGiaoVien = lopHoc.MaGiaoVien;
            existing.MaPhong = lopHoc.MaPhong;
            existing.AvatarUrl = lopHoc.AvatarUrl;
            existing.NgayBatDau = lopHoc.NgayBatDau;
            existing.NgayKetThuc = lopHoc.NgayKetThuc;
            existing.SiSoToiDa = lopHoc.SiSoToiDa;
            existing.TrangThai = lopHoc.TrangThai;

            await _context.SaveChangesAsync();

            return existing;
        }


        // =====================================================
        // 11. XÓA LỚP HỌC
        // =====================================================

        public async Task<bool> DeleteAsync(int id)
        {
            var lopHoc = await _context.LopHocs
                .FirstOrDefaultAsync(x => x.MaLop == id);

            if (lopHoc == null)
            {
                return false;
            }

            _context.LopHocs.Remove(lopHoc);

            await _context.SaveChangesAsync();

            return true;
        }


        // =====================================================
        // 12. TÌM THEO MÃ LỚP
        // =====================================================

        public async Task<IEnumerable<LopHoc>> GetByMaLopCodeAsync(
            string maLopCode)
        {
            return await GetBaseQuery()
                .Where(x => x.MaLopCode.Contains(maLopCode))
                .OrderBy(x => x.MaLop)
                .ToListAsync();
        }


        // =====================================================
        // 13. TÌM THEO MÃ LỚP + PHÂN TRANG
        // =====================================================

        public async Task<PagedResult<LopHoc>> GetPagedByMaLopCodeAsync(
            string maLopCode,
            int pageNumber,
            int pageSize)
        {
            var query = _context.LopHocs
                .Where(x => x.MaLopCode.Contains(maLopCode));

            var totalItems = await query.CountAsync();

            var data = await query
                .Include(x => x.MaKhoaHocNavigation)
                .Include(x => x.MaGiaoVienNavigation)
                .Include(x => x.MaPhongNavigation)
                .OrderBy(x => x.MaLop)
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            return new PagedResult<LopHoc>
            {
                PageNumber = pageNumber,
                PageSize = pageSize,
                TotalItems = totalItems,
                TotalPages = (int)Math.Ceiling(
                    totalItems / (double)pageSize),
                Data = data
            };
        }


        // =====================================================
        // 14. TÌM THEO TÊN LỚP
        // =====================================================

        public async Task<IEnumerable<LopHoc>> GetByTenLopAsync(
            string tenLop)
        {
            return await GetBaseQuery()
                .Where(x => x.TenLop.Contains(tenLop))
                .OrderBy(x => x.MaLop)
                .ToListAsync();
        }


        // =====================================================
        // 15. TÌM THEO TÊN LỚP + PHÂN TRANG
        // =====================================================

        public async Task<PagedResult<LopHoc>> GetPagedByTenLopAsync(
            string tenLop,
            int pageNumber,
            int pageSize)
        {
            var query = _context.LopHocs
                .Where(x => x.TenLop.Contains(tenLop));

            var totalItems = await query.CountAsync();

            var data = await query
                .Include(x => x.MaKhoaHocNavigation)
                .Include(x => x.MaGiaoVienNavigation)
                .Include(x => x.MaPhongNavigation)
                .OrderBy(x => x.MaLop)
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            return new PagedResult<LopHoc>
            {
                PageNumber = pageNumber,
                PageSize = pageSize,
                TotalItems = totalItems,
                TotalPages = (int)Math.Ceiling(
                    totalItems / (double)pageSize),
                Data = data
            };
        }


        // =====================================================
        // 16. TÌM THEO KHÓA HỌC
        // =====================================================

        public async Task<IEnumerable<LopHoc>> GetByKhoaHocIdAsync(
            int maKhoaHoc)
        {
            return await GetBaseQuery()
                .Where(x => x.MaKhoaHoc == maKhoaHoc)
                .OrderBy(x => x.MaLop)
                .ToListAsync();
        }


        // =====================================================
        // 17. TÌM THEO KHÓA HỌC + PHÂN TRANG
        // =====================================================

        public async Task<PagedResult<LopHoc>> GetPagedByKhoaHocIdAsync(
            int maKhoaHoc,
            int pageNumber,
            int pageSize)
        {
            var query = _context.LopHocs
                .Where(x => x.MaKhoaHoc == maKhoaHoc);

            var totalItems = await query.CountAsync();

            var data = await query
                .Include(x => x.MaKhoaHocNavigation)
                .Include(x => x.MaGiaoVienNavigation)
                .Include(x => x.MaPhongNavigation)
                .OrderBy(x => x.MaLop)
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            return new PagedResult<LopHoc>
            {
                PageNumber = pageNumber,
                PageSize = pageSize,
                TotalItems = totalItems,
                TotalPages = (int)Math.Ceiling(
                    totalItems / (double)pageSize),
                Data = data
            };
        }


        // =====================================================
        // 18. TÌM THEO GIÁO VIÊN
        // =====================================================

        public async Task<IEnumerable<LopHoc>> GetByGiaoVienIdAsync(
            int maGiaoVien)
        {
            return await GetBaseQuery()
                .Where(x => x.MaGiaoVien == maGiaoVien)
                .OrderBy(x => x.MaLop)
                .ToListAsync();
        }


        // =====================================================
        // 19. TÌM THEO GIÁO VIÊN + PHÂN TRANG
        // =====================================================

        public async Task<PagedResult<LopHoc>> GetPagedByGiaoVienIdAsync(
            int maGiaoVien,
            int pageNumber,
            int pageSize)
        {
            var query = _context.LopHocs
                .Where(x => x.MaGiaoVien == maGiaoVien);

            var totalItems = await query.CountAsync();

            var data = await query
                .Include(x => x.MaKhoaHocNavigation)
                .Include(x => x.MaGiaoVienNavigation)
                .Include(x => x.MaPhongNavigation)
                .OrderBy(x => x.MaLop)
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            return new PagedResult<LopHoc>
            {
                PageNumber = pageNumber,
                PageSize = pageSize,
                TotalItems = totalItems,
                TotalPages = (int)Math.Ceiling(
                    totalItems / (double)pageSize),
                Data = data
            };
        }


        // =====================================================
        // 20. TÌM THEO PHÒNG HỌC
        // =====================================================

        public async Task<IEnumerable<LopHoc>> GetByPhongHocIdAsync(
            int maPhong)
        {
            return await GetBaseQuery()
                .Where(x => x.MaPhong == maPhong)
                .OrderBy(x => x.MaLop)
                .ToListAsync();
        }


        // =====================================================
        // 21. TÌM THEO PHÒNG HỌC + PHÂN TRANG
        // =====================================================

        public async Task<PagedResult<LopHoc>> GetPagedByPhongHocIdAsync(
            int maPhong,
            int pageNumber,
            int pageSize)
        {
            var query = _context.LopHocs
                .Where(x => x.MaPhong == maPhong);

            var totalItems = await query.CountAsync();

            var data = await query
                .Include(x => x.MaKhoaHocNavigation)
                .Include(x => x.MaGiaoVienNavigation)
                .Include(x => x.MaPhongNavigation)
                .OrderBy(x => x.MaLop)
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            return new PagedResult<LopHoc>
            {
                PageNumber = pageNumber,
                PageSize = pageSize,
                TotalItems = totalItems,
                TotalPages = (int)Math.Ceiling(
                    totalItems / (double)pageSize),
                Data = data
            };
        }


        // =====================================================
        // 22. TÌM THEO TRẠNG THÁI
        // =====================================================

        public async Task<IEnumerable<LopHoc>> GetByTrangThaiAsync(
            string trangThai)
        {
            return await GetBaseQuery()
                .Where(x => x.TrangThai == trangThai)
                .OrderBy(x => x.MaLop)
                .ToListAsync();
        }


        // =====================================================
        // 23. TÌM THEO TRẠNG THÁI + PHÂN TRANG
        // =====================================================

        public async Task<PagedResult<LopHoc>> GetPagedByTrangThaiAsync(
            string trangThai,
            int pageNumber,
            int pageSize)
        {
            var query = _context.LopHocs
                .Where(x => x.TrangThai == trangThai);

            var totalItems = await query.CountAsync();

            var data = await query
                .Include(x => x.MaKhoaHocNavigation)
                .Include(x => x.MaGiaoVienNavigation)
                .Include(x => x.MaPhongNavigation)
                .OrderBy(x => x.MaLop)
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            return new PagedResult<LopHoc>
            {
                PageNumber = pageNumber,
                PageSize = pageSize,
                TotalItems = totalItems,
                TotalPages = (int)Math.Ceiling(
                    totalItems / (double)pageSize),
                Data = data
            };
        }


        // =====================================================
        // 24. TÌM KIẾM
        // Mã lớp + tên lớp
        // =====================================================

        public async Task<IEnumerable<LopHoc>> SearchAsync(
            string? maLopCode,
            string? tenLop)
        {
            var query = GetBaseQuery();

            if (!string.IsNullOrWhiteSpace(maLopCode))
            {
                query = query.Where(x =>
                    x.MaLopCode.Contains(maLopCode));
            }

            if (!string.IsNullOrWhiteSpace(tenLop))
            {
                query = query.Where(x =>
                    x.TenLop.Contains(tenLop));
            }

            return await query
                .OrderBy(x => x.MaLop)
                .ToListAsync();
        }


        // =====================================================
        // 25. TÌM KIẾM + PHÂN TRANG
        // =====================================================

        public async Task<PagedResult<LopHoc>> SearchPagedAsync(
            string? maLopCode,
            string? tenLop,
            int pageNumber,
            int pageSize)
        {
            var query = _context.LopHocs.AsQueryable();

            if (!string.IsNullOrWhiteSpace(maLopCode))
            {
                query = query.Where(x =>
                    x.MaLopCode.Contains(maLopCode));
            }

            if (!string.IsNullOrWhiteSpace(tenLop))
            {
                query = query.Where(x =>
                    x.TenLop.Contains(tenLop));
            }

            var totalItems = await query.CountAsync();

            var data = await query
                .Include(x => x.MaKhoaHocNavigation)
                .Include(x => x.MaGiaoVienNavigation)
                .Include(x => x.MaPhongNavigation)
                .OrderBy(x => x.MaLop)
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            return new PagedResult<LopHoc>
            {
                PageNumber = pageNumber,
                PageSize = pageSize,
                TotalItems = totalItems,
                TotalPages = (int)Math.Ceiling(
                    totalItems / (double)pageSize),
                Data = data
            };
        }


        // =====================================================
        // 26. LẤY TẤT CẢ + PHÂN TRANG
        // =====================================================

        public async Task<PagedResult<LopHoc>> GetPagedAsync(
            int pageNumber,
            int pageSize)
        {
            var query = _context.LopHocs.AsQueryable();

            var totalItems = await query.CountAsync();

            var data = await query
                .Include(x => x.MaKhoaHocNavigation)
                .Include(x => x.MaGiaoVienNavigation)
                .Include(x => x.MaPhongNavigation)
                .OrderBy(x => x.MaLop)
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            return new PagedResult<LopHoc>
            {
                PageNumber = pageNumber,
                PageSize = pageSize,
                TotalItems = totalItems,
                TotalPages = (int)Math.Ceiling(
                    totalItems / (double)pageSize),
                Data = data
            };
        }
    }
}