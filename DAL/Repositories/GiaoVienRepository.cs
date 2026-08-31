using DAL.Context;
using DAL.Entities;
using DAL.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace DAL.Repositories
{
    public class GiaoVienRepository : IGiaoVienRepository
    {
        private readonly AppDbContext _context;

        public GiaoVienRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<GiaoVien>> GetAllAsync()
        {
            return await _context.GiaoViens
                .Include(x => x.MaNguoiDungNavigation)
                .ToListAsync();
        }

        public async Task<GiaoVien?> GetByIdAsync(int id)
        {
            return await _context.GiaoViens
                .Include(x => x.MaNguoiDungNavigation)
                .FirstOrDefaultAsync(x => x.MaGiaoVien == id);
        }

        public async Task AddAsync(GiaoVien giaoVien)
        {
            await _context.GiaoViens.AddAsync(giaoVien);
        }

        public Task UpdateAsync(GiaoVien giaoVien)
        {
            _context.GiaoViens.Update(giaoVien);
            return Task.CompletedTask;
        }

        public async Task DeleteAsync(int id)
        {
            var giaoVien = await _context.GiaoViens.FindAsync(id);

            if (giaoVien != null)
            {
                _context.GiaoViens.Remove(giaoVien);
            }
        }

        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }
        // kiểm tra giáo viên có tồn tại hay không theo id người dùng
        public async Task<bool> ExistsByNguoiDungIdAsync(int maNguoiDung)
        {
            return await _context.GiaoViens
                .AnyAsync(x => x.MaNguoiDung == maNguoiDung);
        }
        public async Task<(IEnumerable<GiaoVien> Items, int TotalItems)> GetPagedAsync(int pageNumber, int pageSize)
        {
            var query = _context.GiaoViens
                .Include(x => x.MaNguoiDungNavigation)
                .AsQueryable();

            int totalItems = await query.CountAsync();

            var items = await query
                .OrderBy(x => x.MaGiaoVien)
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            return (items, totalItems);
        }
        // Lấy giáo viên theo tên
        public async Task<IEnumerable<GiaoVien>> GetByNameAsync(string name)
        {
            return await _context.GiaoViens
                .Include(x => x.MaNguoiDungNavigation)
                .Where(x => x.HoTen != null &&
                            x.HoTen.Contains(name, StringComparison.OrdinalIgnoreCase))
                .ToListAsync();
        }
        // Lấy giáo viên theo chuyên môn
        public async Task<IEnumerable<GiaoVien>> GetByChuyenMonAsync(string chuyenMon)
        {
            return await _context.GiaoViens
                .Include(x => x.MaNguoiDungNavigation)
                .Where(x => x.ChuyenMon != null &&
                            x.ChuyenMon.Contains(chuyenMon, StringComparison.OrdinalIgnoreCase))
                .ToListAsync();
        }
        // Tìm kiếm giáo viên theo tên, số điện thoại, email, chuyên môn
        public async Task<IEnumerable<GiaoVien>> SearchTeacherAsync(string keyword)
        {
            return await _context.GiaoViens
                .Include(x => x.MaNguoiDungNavigation)
                .Where(x => (x.HoTen != null && x.HoTen.Contains(keyword, StringComparison.OrdinalIgnoreCase)) ||
                            (x.SoDienThoai != null && x.SoDienThoai.Contains(keyword, StringComparison.OrdinalIgnoreCase)) ||
                            (x.Email != null && x.Email.Contains(keyword, StringComparison.OrdinalIgnoreCase)) ||
                            (x.ChuyenMon != null && x.ChuyenMon.Contains(keyword, StringComparison.OrdinalIgnoreCase)))
                .ToListAsync();
        }
        // Lấy giáo viên theo tên phân trang
        public async Task<IEnumerable<GiaoVien>> GetByNamePagingAsync(string name, int pageNumber, int pageSize)
        {
            return await _context.GiaoViens
                .Include(x => x.MaNguoiDungNavigation)
                .Where(x => x.HoTen != null &&
                            x.HoTen.Contains(name, StringComparison.OrdinalIgnoreCase))
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();
        }
        // Lấy giáo viên theo chuyên môn phân trang
        public async Task<IEnumerable<GiaoVien>> GetByChuyenMonPagingAsync(string chuyenMon, int pageNumber, int pageSize)
        {
            return await _context.GiaoViens
                .Include(x => x.MaNguoiDungNavigation)
                .Where(x => x.ChuyenMon != null &&
                            x.ChuyenMon.Contains(chuyenMon, StringComparison.OrdinalIgnoreCase))
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();
        }
        // Tìm kiếm giáo viên theo tên, số điện thoại, email, chuyên môn phân trang
        public async Task<IEnumerable<GiaoVien>> SearchTeacherPagingAsync(string keyword, int pageNumber, int pageSize)
        {
            return await _context.GiaoViens
                .Include(x => x.MaNguoiDungNavigation)
                .Where(x => (x.HoTen != null && x.HoTen.Contains(keyword, StringComparison.OrdinalIgnoreCase)) ||
                            (x.SoDienThoai != null && x.SoDienThoai.Contains(keyword, StringComparison.OrdinalIgnoreCase)) ||
                            (x.Email != null && x.Email.Contains(keyword, StringComparison.OrdinalIgnoreCase)) ||
                            (x.ChuyenMon != null && x.ChuyenMon.Contains(keyword, StringComparison.OrdinalIgnoreCase)))
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();
        }
    }
}