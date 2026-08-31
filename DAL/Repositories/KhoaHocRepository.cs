using DAL.Context;
using DAL.Entities;
using DAL.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace DAL.Repositories
{
    public class KhoaHocRepository : IKhoaHocRepository
    {
        private readonly AppDbContext _context;

        public KhoaHocRepository(AppDbContext context)
        {
            _context = context;
        }

        // =====================================================
        // 1. LẤY DANH SÁCH TẤT CẢ KHÓA HỌC
        // =====================================================

        public async Task<IEnumerable<KhoaHoc>> GetAllAsync()
        {
            return await _context.KhoaHocs
                .OrderBy(x => x.MaKhoaHoc)
                .ToListAsync();
        }


        // =====================================================
        // 2. LẤY KHÓA HỌC THEO TÊN
        // =====================================================

        public async Task<IEnumerable<KhoaHoc>> GetByNameAsync(
            string tenKhoaHoc)
        {
            return await _context.KhoaHocs
                .Where(x =>
                    x.TenKhoaHoc != null &&
                    x.TenKhoaHoc.Contains(tenKhoaHoc))
                .OrderBy(x => x.MaKhoaHoc)
                .ToListAsync();
        }


        // =====================================================
        // 3. LẤY KHÓA HỌC THEO TRÌNH ĐỘ
        // =====================================================

        public async Task<IEnumerable<KhoaHoc>> GetByTrinhDoAsync(
            string trinhDo)
        {
            return await _context.KhoaHocs
                .Where(x =>
                    x.TrinhDo != null &&
                    x.TrinhDo.Contains(trinhDo))
                .OrderBy(x => x.MaKhoaHoc)
                .ToListAsync();
        }


        // =====================================================
        // 4. LẤY KHÓA HỌC THEO HỌC PHÍ
        // =====================================================

        public async Task<IEnumerable<KhoaHoc>> GetByHocPhiAsync(
            decimal hocPhi)
        {
            return await _context.KhoaHocs
                .Where(x =>
                    x.HocPhiChuan.HasValue &&
                    x.HocPhiChuan.Value == hocPhi)
                .OrderBy(x => x.MaKhoaHoc)
                .ToListAsync();
        }


        // =====================================================
        // 5. LẤY KHÓA HỌC + PHÂN TRANG
        // =====================================================

        public async Task<object> GetPagedAsync(
            int pageNumber,
            int pageSize)
        {
            // Kiểm tra dữ liệu phân trang
            if (pageNumber < 1)
                pageNumber = 1;

            if (pageSize < 1)
                pageSize = 10;

            // Tổng số khóa học
            var totalCount = await _context.KhoaHocs
                .CountAsync();

            // Tổng số trang
            var totalPages = (int)Math.Ceiling(
                (double)totalCount / pageSize);

            // Lấy dữ liệu
            var data = await _context.KhoaHocs
                .OrderBy(x => x.MaKhoaHoc)
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            return new
            {
                PageNumber = pageNumber,
                PageSize = pageSize,
                TotalCount = totalCount,
                TotalPages = totalPages,
                Data = data
            };
        }


        // =====================================================
        // 6. PHÂN TRANG + LỌC THEO TÊN
        // =====================================================

        public async Task<object> GetPagedByNameAsync(
            string tenKhoaHoc,
            int pageNumber,
            int pageSize)
        {
            if (pageNumber < 1)
                pageNumber = 1;

            if (pageSize < 1)
                pageSize = 10;

            var query = _context.KhoaHocs
                .Where(x =>
                    x.TenKhoaHoc != null &&
                    x.TenKhoaHoc.Contains(tenKhoaHoc));

            var totalCount = await query.CountAsync();

            var totalPages = (int)Math.Ceiling(
                (double)totalCount / pageSize);

            var data = await query
                .OrderBy(x => x.MaKhoaHoc)
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            return new
            {
                PageNumber = pageNumber,
                PageSize = pageSize,
                TotalCount = totalCount,
                TotalPages = totalPages,
                Data = data
            };
        }


        // =====================================================
        // 7. PHÂN TRANG + LỌC THEO TRÌNH ĐỘ
        // =====================================================

        public async Task<object> GetPagedByTrinhDoAsync(
            string trinhDo,
            int pageNumber,
            int pageSize)
        {
            if (pageNumber < 1)
                pageNumber = 1;

            if (pageSize < 1)
                pageSize = 10;

            var query = _context.KhoaHocs
                .Where(x =>
                    x.TrinhDo != null &&
                    x.TrinhDo.Contains(trinhDo));

            var totalCount = await query.CountAsync();

            var totalPages = (int)Math.Ceiling(
                (double)totalCount / pageSize);

            var data = await query
                .OrderBy(x => x.MaKhoaHoc)
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            return new
            {
                PageNumber = pageNumber,
                PageSize = pageSize,
                TotalCount = totalCount,
                TotalPages = totalPages,
                Data = data
            };
        }


        // =====================================================
        // 8. PHÂN TRANG + LỌC THEO HỌC PHÍ
        // =====================================================

        public async Task<object> GetPagedByHocPhiAsync(
            decimal hocPhi,
            int pageNumber,
            int pageSize)
        {
            if (pageNumber < 1)
                pageNumber = 1;

            if (pageSize < 1)
                pageSize = 10;

            var query = _context.KhoaHocs
                .Where(x =>
                    x.HocPhiChuan.HasValue &&
                    x.HocPhiChuan.Value == hocPhi);

            var totalCount = await query.CountAsync();

            var totalPages = (int)Math.Ceiling(
                (double)totalCount / pageSize);

            var data = await query
                .OrderBy(x => x.MaKhoaHoc)
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            return new
            {
                PageNumber = pageNumber,
                PageSize = pageSize,
                TotalCount = totalCount,
                TotalPages = totalPages,
                Data = data
            };
        }


        // =====================================================
        // 9. TẠO KHÓA HỌC
        // =====================================================

        public async Task<KhoaHoc> AddAsync(KhoaHoc khoaHoc)
        {
            try
            {
                await _context.KhoaHocs.AddAsync(khoaHoc);
                await _context.SaveChangesAsync();

                return khoaHoc;
            }
            catch (DbUpdateException ex)
            {
                throw new Exception(
                    ex.InnerException?.Message ?? ex.Message,
                    ex);
            }
        }


        // =====================================================
        // 10. CẬP NHẬT KHÓA HỌC
        // =====================================================

        public async Task<KhoaHoc?> UpdateAsync(
            KhoaHoc khoaHoc)
        {
            var existing = await _context.KhoaHocs
                .FirstOrDefaultAsync(
                    x => x.MaKhoaHoc == khoaHoc.MaKhoaHoc);

            if (existing == null)
                return null;

            existing.TenKhoaHoc = khoaHoc.TenKhoaHoc;
            existing.MaCode = khoaHoc.MaCode;
            existing.TrinhDo = khoaHoc.TrinhDo;
            existing.MoTa = khoaHoc.MoTa;
            existing.TongSoBuoi = khoaHoc.TongSoBuoi;
            existing.SoTuanHoc = khoaHoc.SoTuanHoc;
            existing.HocPhiChuan = khoaHoc.HocPhiChuan;
            existing.DangHoatDong = khoaHoc.DangHoatDong;

            await _context.SaveChangesAsync();

            return existing;
        }


        // =====================================================
        // 11. XÓA KHÓA HỌC
        // =====================================================

        public async Task<bool> DeleteAsync(int id)
        {
            var khoaHoc = await _context.KhoaHocs
                .FirstOrDefaultAsync(
                    x => x.MaKhoaHoc == id);

            if (khoaHoc == null)
                return false;

            // Kiểm tra khóa học đã được sử dụng
            // trong lớp học hay chưa
            var hasLopHoc = await _context.LopHocs
                .AnyAsync(x => x.MaKhoaHoc == id);

            if (hasLopHoc)
            {
                throw new InvalidOperationException(
                    "Không thể xóa khóa học vì khóa học đang được sử dụng trong lớp học.");
            }

            _context.KhoaHocs.Remove(khoaHoc);

            await _context.SaveChangesAsync();

            return true;
        }


        // =====================================================
        // 12. TÌM KIẾM
        //     TÊN + TRÌNH ĐỘ + HỌC PHÍ
        // =====================================================

        public async Task<IEnumerable<KhoaHoc>> SearchAsync(
            string? tenKhoaHoc,
            string? trinhDo,
            decimal? hocPhi)
        {
            var query = _context.KhoaHocs
                .AsQueryable();

            // Lọc theo tên
            if (!string.IsNullOrWhiteSpace(tenKhoaHoc))
            {
                query = query.Where(x =>
                    x.TenKhoaHoc.Contains(tenKhoaHoc));
            }

            // Lọc theo trình độ
            if (!string.IsNullOrWhiteSpace(trinhDo))
            {
                query = query.Where(x =>
                    x.TrinhDo != null &&
                    x.TrinhDo.Contains(trinhDo));
            }

            // Lọc theo học phí
            if (hocPhi.HasValue)
            {
                query = query.Where(x =>
                    x.HocPhiChuan.HasValue &&
                    x.HocPhiChuan.Value == hocPhi.Value);
            }

            return await query
                .OrderBy(x => x.MaKhoaHoc)
                .ToListAsync();
        }


        // =====================================================
        // 13. TÌM KIẾM + PHÂN TRANG
        //     TÊN + TRÌNH ĐỘ + HỌC PHÍ
        // =====================================================

        public async Task<object> SearchPagedAsync(
            string? tenKhoaHoc,
            string? trinhDo,
            decimal? hocPhi,
            int pageNumber,
            int pageSize)
        {
            if (pageNumber < 1)
                pageNumber = 1;

            if (pageSize < 1)
                pageSize = 10;

            var query = _context.KhoaHocs
                .AsQueryable();

            // =================================================
            // LỌC THEO TÊN
            // =================================================

            if (!string.IsNullOrWhiteSpace(tenKhoaHoc))
            {
                query = query.Where(x =>
                    x.TenKhoaHoc.Contains(tenKhoaHoc));
            }

            // =================================================
            // LỌC THEO TRÌNH ĐỘ
            // =================================================

            if (!string.IsNullOrWhiteSpace(trinhDo))
            {
                query = query.Where(x =>
                    x.TrinhDo != null &&
                    x.TrinhDo.Contains(trinhDo));
            }

            // =================================================
            // LỌC THEO HỌC PHÍ
            // =================================================

            if (hocPhi.HasValue)
            {
                query = query.Where(x =>
                    x.HocPhiChuan.HasValue &&
                    x.HocPhiChuan.Value == hocPhi.Value);
            }

            // =================================================
            // TỔNG SỐ BẢN GHI
            // =================================================

            var totalCount = await query.CountAsync();

            // =================================================
            // TỔNG SỐ TRANG
            // =================================================

            var totalPages = (int)Math.Ceiling(
                (double)totalCount / pageSize);

            // =================================================
            // PHÂN TRANG
            // =================================================

            var data = await query
                .OrderBy(x => x.MaKhoaHoc)
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            return new
            {
                PageNumber = pageNumber,
                PageSize = pageSize,
                TotalCount = totalCount,
                TotalPages = totalPages,
                Data = data
            };
        }
        // =====================================================
        // 14. LẤY DANH SÁCH HỌC PHÍ CHUẨN
        // =====================================================
        public async Task<IEnumerable<decimal>> GetAllHocPhiChuanAsync()
        {
            return await _context.KhoaHocs
                .Where(x => x.HocPhiChuan.HasValue)
                .Select(x => x.HocPhiChuan.Value)
                .Distinct()
                .OrderBy(x => x)
                .ToListAsync();
        }
        // =====================================================
        // 15. LẤY DANH SÁCh TRÌNH ĐỘ
        // =====================================================
        public async Task<IEnumerable<string>> GetAllTrinhDoAsync()
        {
            return await _context.KhoaHocs
                .Where(x => !string.IsNullOrEmpty(x.TrinhDo))
                .Select(x => x.TrinhDo!)
                .Distinct()
                .OrderBy(x => x)
                .ToListAsync();
        }
        // =====================================================
        // 16. LẤY DANH SÁCH ten khóa học
        // =====================================================
        public async Task<IEnumerable<string>> GetAllTenKhoaHocAsync()
        {
            return await _context.KhoaHocs
                .Where(x => !string.IsNullOrEmpty(x.TenKhoaHoc))
                .Select(x => x.TenKhoaHoc!)
                .Distinct()
                .OrderBy(x => x)
                .ToListAsync();
        }
        // =====================================================
        // 17. kiểm tra mã của khóa học có tồn tại ko
        // =====================================================
        public async Task<bool> ExistsByMakhoahocAsync(int makhoahoc)
        {
            return await _context.KhoaHocs
                .FirstOrDefaultAsync(x => x.MaKhoaHoc == makhoahoc) != null;
        }
        // =====================================================
        // 18. LẤY KHÓA HỌC THEO ID
        // =====================================================
        public async Task<KhoaHoc?> GetByIdAsync(int id)
        {
            return await _context.KhoaHocs
                .FirstOrDefaultAsync(x => x.MaKhoaHoc == id);
        }
        // =====================================================
        // 19. kiểm tra tên của khóa học có tồn tại ko
        // =====================================================
        public async Task<bool> ExistsByTenKhoaHocAsync(string tenKhoaHoc)
        {
            return await _context.KhoaHocs
                .FirstOrDefaultAsync(x => x.TenKhoaHoc == tenKhoaHoc) != null;
        }
        // =====================================================
        // 20. kiểm tra mã code của khóa học có tồn tại ko
        // =====================================================
        public async Task<bool> ExistsByMaCodeAsync(string maCode)
        {
            return await _context.KhoaHocs
                .FirstOrDefaultAsync(x => x.MaCode == maCode) != null;
        }

    }
}