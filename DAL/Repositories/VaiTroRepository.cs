using DAL.Context;
using DAL.Entities;
using DAL.Interfaces;
using Microsoft.EntityFrameworkCore;
namespace DAL.Repositories
{
    public class VaiTroRepository : IVaiTroRepository
    {
        private readonly AppDbContext _context;
        public VaiTroRepository(AppDbContext context)
        {
            _context = context;
        }
        // lấy tất cả vai trò 
        public async Task<IEnumerable<VaiTro>> GetAllVaiTroAsync()
        {
            return await _context.VaiTros.ToListAsync();
        }
        // lấy vai trò theo id
        public async Task<VaiTro?> GetVaiTroByIdAsync(int id)
        {
            return await _context.VaiTros.FindAsync(id);
        }
        // thêm vai trò mới
        public async Task<VaiTro> AddVaiTroAsync(VaiTro vaiTro)
        {
            _context.VaiTros.Add(vaiTro);
            await _context.SaveChangesAsync();
            return vaiTro;
        }
        // cập nhật vai trò
        public async Task<VaiTro?> UpdateVaiTroAsync(VaiTro vaiTro)
        {
            var existingVaiTro = await _context.VaiTros.FindAsync(vaiTro.MaVaiTro);
            if (existingVaiTro == null)
            {
                return null;
            }
            existingVaiTro.TenVaiTro = vaiTro.TenVaiTro;
            await _context.SaveChangesAsync();
            return existingVaiTro;
        }
        // xóa vai trò
        public async Task<bool> DeleteVaiTroAsync(int id)
        {
            var vaiTro = await _context.VaiTros.FindAsync(id);
            if (vaiTro == null)
            {
                return false;
            }
            _context.VaiTros.Remove(vaiTro);
            await _context.SaveChangesAsync();
            return true;
        }
        // lưu thay đổi
        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }
        // kiểm tra vai trò đã tồn tại chưa
        public async Task<bool> ExistsByNameAsync(string tenVaiTro)
        {
            return await _context.VaiTros
                .AnyAsync(x => x.TenVaiTro == tenVaiTro);
        }
    } 
}