using DAL.Context;
using DAL.Entities;
using DAL.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace DAL.Repositories
{
    public class NguoiDungRepository : INguoiDungRepository
    {
        private readonly AppDbContext _context;

        public NguoiDungRepository(AppDbContext context)
        {
            _context = context;
        }
        // lấy tất cả người dùng
        public async Task<IEnumerable<NguoiDung>> GetAllAsync()
        {
            return await _context.NguoiDungs
                .Include(x => x.MaVaiTroNavigation)
                .ToListAsync();
        }
        // lấy người dùng theo id
        public async Task<NguoiDung?> GetByIdAsync(int id)
        {
            return await _context.NguoiDungs
                .Include(x => x.MaVaiTroNavigation)
                .FirstOrDefaultAsync(x => x.MaNguoiDung == id);
        }
        // lấy người dùng theo tên đăng nhập
        public async Task<NguoiDung?> GetByUserNameAsync(string userName)
        {
            return await _context.NguoiDungs
                .FirstOrDefaultAsync(x => x.TenDangNhap == userName);
        }
        // thêm người dùng
        public async Task AddAsync(NguoiDung nguoiDung)
        {
            await _context.NguoiDungs.AddAsync(nguoiDung);
        }
        // cập nhật người dùng
        public async Task UpdateAsync(NguoiDung nguoiDung)
        {
            _context.NguoiDungs.Update(nguoiDung);
            await Task.CompletedTask;
        }
        // xóa người dùng
        public async Task DeleteAsync(int id)
        {
            var nguoiDung = await GetByIdAsync(id);

            if (nguoiDung != null)
            {
                _context.NguoiDungs.Remove(nguoiDung);
            }
        }
        // Lưu thay đổi
        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }
        // đăng nhập người dùng
        public async Task<NguoiDung?> LoginAsync(string tenDangNhap)
        {
            return await _context.NguoiDungs
                .Include(x => x.MaVaiTroNavigation)
                .FirstOrDefaultAsync(x => x.TenDangNhap == tenDangNhap
                                       && x.DangHoatDong == true);
        }
        // kiểm tra người dùng có tồn tại hay không
        public async Task<bool> ExistsByIdAsync(int maNguoiDung)
        {
            return await _context.NguoiDungs
                .AnyAsync(x => x.MaNguoiDung == maNguoiDung);
        }
        // kiểm tra email có tồn tại hay không
        public async Task<bool> ExistsByEmailAsync(string email)
        {
            return await _context.NguoiDungs
                .AnyAsync(x => x.Email == email);

        }
        // kiểm tra tên đăng nhập có tồn tại hay không
        public async Task<bool> ExistsByUserNameAsync(string tenDangNhap)
        {
            return await _context.NguoiDungs
                .AnyAsync(x => x.TenDangNhap == tenDangNhap);
        }
        // kiểm tra số điện thoại có tồn tại hay không
        public async Task<bool> ExistsBySoDienThoaiAsync(string soDienThoai)
        {
            return await _context.NguoiDungs
                .AnyAsync(x => x.SoDienThoai == soDienThoai);

        }
        // kiểm tra email có tồn tại hay không, ngoại trừ người dùng có id = maNguoiDung
        public async Task<bool> ExistsByEmailAsync(string email, int maNguoiDung)
        {
            return await _context.NguoiDungs
                .AnyAsync(x => x.Email == email && x.MaNguoiDung != maNguoiDung);
        }
        // kiểm tra tên đăng nhập có tồn tại hay không, ngoại trừ người dùng có id = maNguoiDung
        public async Task<bool> ExistsByUserNameAsync(string tenDangNhap, int maNguoiDung)
        {
            return await _context.NguoiDungs
                .AnyAsync(x => x.TenDangNhap == tenDangNhap && x.MaNguoiDung != maNguoiDung);
        }
        // kiểm tra số điện thoại có tồn tại hay không, ngoại trừ người dùng có id = maNguoiDung
        public async Task<bool> ExistsBySoDienThoaiAsync(string soDienThoai, int maNguoiDung)
        {
            return await _context.NguoiDungs
                .AnyAsync(x => x.SoDienThoai == soDienThoai && x.MaNguoiDung != maNguoiDung);
        }

    }
}