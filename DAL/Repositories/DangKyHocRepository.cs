using DAL.Context;
using DAL.Entities;
using DAL.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace DAL.Repositories
{
    public class DangKyHocRepository : IDangKyHocRepository
    {
        private readonly AppDbContext _context;

        public DangKyHocRepository(AppDbContext context)
        {
            _context = context;
        }

        // =====================================================
        // LẤY TẤT CẢ
        // =====================================================

        public async Task<IEnumerable<DangKyHoc>> GetAllAsync()
        {
            return await _context.DangKyHocs
                .Include(x => x.MaHocVienNavigation)
                .Include(x => x.MaLopNavigation)
                .AsNoTracking()
                .ToListAsync();
        }

        // =====================================================
        // LẤY THEO ID
        // =====================================================

        public async Task<DangKyHoc?> GetByIdAsync(int id)
        {
            return await _context.DangKyHocs
                .Include(x => x.MaHocVienNavigation)
                .Include(x => x.MaLopNavigation)
                .FirstOrDefaultAsync(x => x.MaDangKy == id);
        }

        // =====================================================
        // LẤY THEO HỌC VIÊN
        // =====================================================

        public async Task<IEnumerable<DangKyHoc>> GetByHocVienIdAsync(
            int maHocVien)
        {
            return await _context.DangKyHocs
                .Include(x => x.MaHocVienNavigation)
                .Include(x => x.MaLopNavigation)
                .Where(x => x.MaHocVien == maHocVien)
                .AsNoTracking()
                .ToListAsync();
        }

        // =====================================================
        // LẤY THEO LỚP
        // =====================================================

        public async Task<IEnumerable<DangKyHoc>> GetByLopIdAsync(
            int maLop)
        {
            return await _context.DangKyHocs
                .Include(x => x.MaHocVienNavigation)
                .Include(x => x.MaLopNavigation)
                .Where(x => x.MaLop == maLop)
                .AsNoTracking()
                .ToListAsync();
        }

        // =====================================================
        // LẤY THEO TRẠNG THÁI
        // =====================================================

        public async Task<IEnumerable<DangKyHoc>> GetByTrangThaiAsync(
            string trangThai)
        {
            return await _context.DangKyHocs
                .Include(x => x.MaHocVienNavigation)
                .Include(x => x.MaLopNavigation)
                .Where(x => x.TrangThai == trangThai)
                .AsNoTracking()
                .ToListAsync();
        }

        // =====================================================
        // KIỂM TRA ID
        // =====================================================

        public async Task<bool> ExistsByIdAsync(int id)
        {
            return await _context.DangKyHocs
                .AnyAsync(x => x.MaDangKy == id);
        }

        // =====================================================
        // KIỂM TRA HỌC VIÊN + LỚP
        // =====================================================

        public async Task<bool> ExistsHocVienLopAsync(
            int maHocVien,
            int maLop)
        {
            return await _context.DangKyHocs
                .AnyAsync(x =>
                    x.MaHocVien == maHocVien &&
                    x.MaLop == maLop);
        }

        // =====================================================
        // KIỂM TRA HỌC VIÊN + LỚP KHI UPDATE
        // =====================================================

        public async Task<bool> ExistsHocVienLopExceptIdAsync(
            int maHocVien,
            int maLop,
            int maDangKy)
        {
            return await _context.DangKyHocs
                .AnyAsync(x =>
                    x.MaHocVien == maHocVien &&
                    x.MaLop == maLop &&
                    x.MaDangKy != maDangKy);
        }

        // =====================================================
        // KIỂM TRA HỌC VIÊN
        // =====================================================

        public async Task<bool> ExistsHocVienAsync(
            int maHocVien)
        {
            return await _context.HocViens
                .AnyAsync(x => x.MaHocVien == maHocVien);
        }

        // =====================================================
        // KIỂM TRA LỚP
        // =====================================================

        public async Task<bool> ExistsLopAsync(
            int maLop)
        {
            return await _context.LopHocs
                .AnyAsync(x => x.MaLop == maLop);
        }

        // =====================================================
        // THÊM
        // =====================================================

        public async Task<DangKyHoc> AddAsync(
            DangKyHoc dangKyHoc)
        {
            await _context.DangKyHocs.AddAsync(dangKyHoc);

            await _context.SaveChangesAsync();

            return await GetByIdAsync(
                dangKyHoc.MaDangKy) ?? dangKyHoc;
        }

        // =====================================================
        // UPDATE
        // =====================================================

        public async Task<DangKyHoc?> UpdateAsync(
            DangKyHoc dangKyHoc)
        {
            var existing = await _context.DangKyHocs
                .FirstOrDefaultAsync(
                    x => x.MaDangKy == dangKyHoc.MaDangKy);

            if (existing == null)
                return null;

            existing.MaHocVien = dangKyHoc.MaHocVien;
            existing.MaLop = dangKyHoc.MaLop;
            existing.NgayDangKy = dangKyHoc.NgayDangKy;
            existing.TrangThai = dangKyHoc.TrangThai;

            await _context.SaveChangesAsync();

            return await GetByIdAsync(
                existing.MaDangKy);
        }

        // =====================================================
        // DELETE
        // =====================================================

        public async Task<bool> DeleteAsync(int id)
        {
            var entity = await _context.DangKyHocs
                .FirstOrDefaultAsync(
                    x => x.MaDangKy == id);

            if (entity == null)
                return false;

            _context.DangKyHocs.Remove(entity);

            await _context.SaveChangesAsync();

            return true;
        }

        // =====================================================
        // SEARCH
        // =====================================================

        public async Task<IEnumerable<DangKyHoc>> SearchAsync(
            int? maHocVien,
            int? maLop,
            string? trangThai)
        {
            var query = _context.DangKyHocs
                .Include(x => x.MaHocVienNavigation)
                .Include(x => x.MaLopNavigation)
                .AsQueryable();

            if (maHocVien.HasValue)
            {
                query = query.Where(
                    x => x.MaHocVien == maHocVien.Value);
            }

            if (maLop.HasValue)
            {
                query = query.Where(
                    x => x.MaLop == maLop.Value);
            }

            if (!string.IsNullOrWhiteSpace(trangThai))
            {
                query = query.Where(
                    x => x.TrangThai!
                        .Contains(trangThai));
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
            var query = _context.DangKyHocs
                .Include(x => x.MaHocVienNavigation)
                .Include(x => x.MaLopNavigation)
                .AsNoTracking();

            var totalRecords = await query.CountAsync();

            var data = await query
                .OrderBy(x => x.MaDangKy)
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            return new
            {
                data,
                pageNumber,
                pageSize,
                totalRecords,
                totalPages =
                    (int)Math.Ceiling(
                        totalRecords /
                        (double)pageSize)
            };
        }

        // =====================================================
        // SEARCH + PAGED
        // =====================================================

        public async Task<object> SearchPagedAsync(
            int? maHocVien,
            int? maLop,
            string? trangThai,
            int pageNumber,
            int pageSize)
        {
            var query = _context.DangKyHocs
                .Include(x => x.MaHocVienNavigation)
                .Include(x => x.MaLopNavigation)
                .AsQueryable();

            if (maHocVien.HasValue)
            {
                query = query.Where(
                    x => x.MaHocVien == maHocVien.Value);
            }

            if (maLop.HasValue)
            {
                query = query.Where(
                    x => x.MaLop == maLop.Value);
            }

            if (!string.IsNullOrWhiteSpace(trangThai))
            {
                query = query.Where(
                    x => x.TrangThai!
                        .Contains(trangThai));
            }

            var totalRecords =
                await query.CountAsync();

            var data = await query
                .OrderBy(x => x.MaDangKy)
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .AsNoTracking()
                .ToListAsync();

            return new
            {
                data,
                pageNumber,
                pageSize,
                totalRecords,
                totalPages =
                    (int)Math.Ceiling(
                        totalRecords /
                        (double)pageSize)
            };
        }

        // =====================================================
        // PAGED THEO HỌC VIÊN
        // =====================================================

        public async Task<object> GetPagedByHocVienIdAsync(
            int maHocVien,
            int pageNumber,
            int pageSize)
        {
            var query = _context.DangKyHocs
                .Include(x => x.MaHocVienNavigation)
                .Include(x => x.MaLopNavigation)
                .Where(x => x.MaHocVien == maHocVien)
                .AsNoTracking();

            var totalRecords =
                await query.CountAsync();

            var data = await query
                .OrderBy(x => x.MaDangKy)
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            return new
            {
                data,
                pageNumber,
                pageSize,
                totalRecords,
                totalPages =
                    (int)Math.Ceiling(
                        totalRecords /
                        (double)pageSize)
            };
        }

        // =====================================================
        // PAGED THEO LỚP
        // =====================================================

        public async Task<object> GetPagedByLopIdAsync(
            int maLop,
            int pageNumber,
            int pageSize)
        {
            var query = _context.DangKyHocs
                .Include(x => x.MaHocVienNavigation)
                .Include(x => x.MaLopNavigation)
                .Where(x => x.MaLop == maLop)
                .AsNoTracking();

            var totalRecords =
                await query.CountAsync();

            var data = await query
                .OrderBy(x => x.MaDangKy)
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            return new
            {
                data,
                pageNumber,
                pageSize,
                totalRecords,
                totalPages =
                    (int)Math.Ceiling(
                        totalRecords /
                        (double)pageSize)
            };
        }

        // =====================================================
        // PAGED THEO TRẠNG THÁI
        // =====================================================

        public async Task<object> GetPagedByTrangThaiAsync(
            string trangThai,
            int pageNumber,
            int pageSize)
        {
            var query = _context.DangKyHocs
                .Include(x => x.MaHocVienNavigation)
                .Include(x => x.MaLopNavigation)
                .Where(x => x.TrangThai == trangThai)
                .AsNoTracking();

            var totalRecords =
                await query.CountAsync();

            var data = await query
                .OrderBy(x => x.MaDangKy)
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            return new
            {
                data,
                pageNumber,
                pageSize,
                totalRecords,
                totalPages =
                    (int)Math.Ceiling(
                        totalRecords /
                        (double)pageSize)
            };
        }
        // =====================================================
        // lấy theo học viên và lớp
        // =====================================================
        public async Task<DangKyHoc?> GetByHocVienIdAndLopIdAsync(
            int maHocVien,
            int maLop)
        {
            return await _context.DangKyHocs
                .Include(x => x.MaHocVienNavigation)
                .Include(x => x.MaLopNavigation)
                .FirstOrDefaultAsync(x =>
                    x.MaHocVien == maHocVien &&
                    x.MaLop == maLop);
        }
    }
}