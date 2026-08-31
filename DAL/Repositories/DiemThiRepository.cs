using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DAL.Context;
using DAL.Entities;
using DAL.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace DAL.Repositories;

public class DiemThiRepository : IDiemThiRepository
{
    private readonly AppDbContext _context;

    public DiemThiRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<DiemThi>> GetAllAsync()
    {
        return await _context.DiemThis
            .Include(dt => dt.MaKyThiNavigation)
            .Include(dt => dt.MaHocVienNavigation)
            .AsNoTracking()
            .ToListAsync();
    }

    public async Task<DiemThi?> GetByIdAsync(int maDiemThi)
    {
        return await _context.DiemThis
            .Include(dt => dt.MaKyThiNavigation)
            .Include(dt => dt.MaHocVienNavigation)
            .AsNoTracking()
            .FirstOrDefaultAsync(dt => dt.MaDiemThi == maDiemThi);
    }

    public async Task<IEnumerable<DiemThi>> GetByMaKyThiAsync(int maKyThi)
    {
        return await _context.DiemThis
            .Include(dt => dt.MaHocVienNavigation)
            .AsNoTracking()
            .Where(dt => dt.MaKyThi == maKyThi)
            .ToListAsync();
    }

    public async Task<IEnumerable<DiemThi>> GetByMaHocVienAsync(int maHocVien)
    {
        return await _context.DiemThis
            .Include(dt => dt.MaKyThiNavigation)
            .AsNoTracking()
            .Where(dt => dt.MaHocVien == maHocVien)
            .ToListAsync();
    }

    public async Task<DiemThi?> GetByKyThiAndHocVienAsync(int maKyThi, int maHocVien)
    {
        return await _context.DiemThis
            .Include(dt => dt.MaKyThiNavigation)
            .Include(dt => dt.MaHocVienNavigation)
            .AsNoTracking()
            .FirstOrDefaultAsync(dt => dt.MaKyThi == maKyThi && dt.MaHocVien == maHocVien);
    }

    public async Task<bool> ExistsForKyThiAndHocVienAsync(int maKyThi, int maHocVien, int? excludeMaDiemThi = null)
    {
        var query = _context.DiemThis
            .Where(dt => dt.MaKyThi == maKyThi && dt.MaHocVien == maHocVien);

        if (excludeMaDiemThi.HasValue)
        {
            query = query.Where(dt => dt.MaDiemThi != excludeMaDiemThi.Value);
        }

        return await query.AnyAsync();
    }

    public async Task<DiemThi> AddAsync(DiemThi diemThi)
    {
        _context.DiemThis.Add(diemThi);
        await _context.SaveChangesAsync();
        return diemThi;
    }

    public async Task<DiemThi> UpdateAsync(DiemThi diemThi)
    {
        _context.DiemThis.Update(diemThi);
        await _context.SaveChangesAsync();
        return diemThi;
    }

    public async Task<bool> DeleteAsync(int maDiemThi)
    {
        var entity = await _context.DiemThis.FindAsync(maDiemThi);
        if (entity == null)
        {
            return false;
        }

        _context.DiemThis.Remove(entity);
        await _context.SaveChangesAsync();
        return true;
    }

    public async Task<bool> ExistsAsync(int maDiemThi)
    {
        return await _context.DiemThis.AnyAsync(dt => dt.MaDiemThi == maDiemThi);
    }
}