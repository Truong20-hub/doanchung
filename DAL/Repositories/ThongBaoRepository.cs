using DAL.Context;
using DAL.Entities;
using DAL.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace DAL.Repositories;

public class ThongBaoRepository : IThongBaoRepository
{
    private readonly AppDbContext _context;

    public ThongBaoRepository(AppDbContext context)
    {
        _context = context;
    }

    private IQueryable<ThongBao> QueryWithRelations()
    {
        return _context.ThongBaos
            .Include(x => x.MaNguoiDungNavigation)
            .Include(x => x.MaHocVienNavigation)
            .Include(x => x.MaLopNavigation)
            .Include(x => x.MaHoaDonNavigation);
    }

    public async Task<IEnumerable<ThongBao>> GetAllAsync()
    {
        return await QueryWithRelations()
            .AsNoTracking()
            .OrderByDescending(x => x.ThoiGianTao)
            .ThenByDescending(x => x.MaThongBao)
            .ToListAsync();
    }

    public async Task<ThongBao?> GetByIdAsync(int id)
    {
        return await QueryWithRelations()
            .FirstOrDefaultAsync(x => x.MaThongBao == id);
    }

    public async Task<IEnumerable<ThongBao>> GetByMaNguoiDungAsync(int maNguoiDung)
    {
        return await QueryWithRelations()
            .Where(x => x.MaNguoiDung == maNguoiDung)
            .AsNoTracking()
            .OrderByDescending(x => x.ThoiGianTao)
            .ThenByDescending(x => x.MaThongBao)
            .ToListAsync();
    }

    public async Task<IEnumerable<ThongBao>> GetUnreadAsync(int maNguoiDung)
    {
        return await QueryWithRelations()
            .Where(x => x.MaNguoiDung == maNguoiDung && !x.DaDoc)
            .AsNoTracking()
            .OrderByDescending(x => x.ThoiGianTao)
            .ThenByDescending(x => x.MaThongBao)
            .ToListAsync();
    }

    public async Task<IEnumerable<ThongBao>> GetByLoaiAsync(string loaiThongBao)
    {
        return await QueryWithRelations()
            .Where(x => x.LoaiThongBao == loaiThongBao)
            .AsNoTracking()
            .OrderByDescending(x => x.ThoiGianTao)
            .ThenByDescending(x => x.MaThongBao)
            .ToListAsync();
    }

    public async Task<IEnumerable<ThongBao>> SearchAsync(string? keyword)
    {
        var query = QueryWithRelations();

        if (!string.IsNullOrWhiteSpace(keyword))
        {
            keyword = keyword.Trim();
            query = query.Where(x =>
                x.LoaiThongBao.Contains(keyword) ||
                x.TieuDe.Contains(keyword) ||
                x.NoiDung.Contains(keyword) ||
                x.MaNguoiDungNavigation.HoTen.Contains(keyword) ||
                (x.MaHocVienNavigation != null && x.MaHocVienNavigation.HoTen.Contains(keyword)) ||
                (x.MaLopNavigation != null && x.MaLopNavigation.TenLop.Contains(keyword)));
        }

        return await query
            .AsNoTracking()
            .OrderByDescending(x => x.ThoiGianTao)
            .ThenByDescending(x => x.MaThongBao)
            .ToListAsync();
    }

    public async Task<object> GetPagedAsync(int pageNumber, int pageSize)
    {
        var query = QueryWithRelations().AsNoTracking();
        var totalItems = await query.CountAsync();
        var data = await query
            .OrderByDescending(x => x.ThoiGianTao)
            .ThenByDescending(x => x.MaThongBao)
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();

        return new
        {
            PageNumber = pageNumber,
            PageSize = pageSize,
            TotalItems = totalItems,
            TotalPages = (int)Math.Ceiling((double)totalItems / pageSize),
            Data = data
        };
    }

    public async Task<object> SearchPagedAsync(string? keyword, int pageNumber, int pageSize)
    {
        var query = QueryWithRelations();

        if (!string.IsNullOrWhiteSpace(keyword))
        {
            keyword = keyword.Trim();
            query = query.Where(x =>
                x.LoaiThongBao.Contains(keyword) ||
                x.TieuDe.Contains(keyword) ||
                x.NoiDung.Contains(keyword) ||
                x.MaNguoiDungNavigation.HoTen.Contains(keyword) ||
                (x.MaHocVienNavigation != null && x.MaHocVienNavigation.HoTen.Contains(keyword)) ||
                (x.MaLopNavigation != null && x.MaLopNavigation.TenLop.Contains(keyword)));
        }

        var totalItems = await query.CountAsync();
        var data = await query
            .OrderByDescending(x => x.ThoiGianTao)
            .ThenByDescending(x => x.MaThongBao)
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .AsNoTracking()
            .ToListAsync();

        return new
        {
            PageNumber = pageNumber,
            PageSize = pageSize,
            TotalItems = totalItems,
            TotalPages = (int)Math.Ceiling((double)totalItems / pageSize),
            Data = data
        };
    }

    public async Task<bool> ExistsByIdAsync(int id)
    {
        return await _context.ThongBaos.AnyAsync(x => x.MaThongBao == id);
    }

    public async Task<ThongBao> AddAsync(ThongBao thongBao)
    {
        await _context.ThongBaos.AddAsync(thongBao);
        await _context.SaveChangesAsync();
        return thongBao;
    }

    public async Task<ThongBao?> UpdateAsync(ThongBao thongBao)
    {
        var existing = await _context.ThongBaos
            .FirstOrDefaultAsync(x => x.MaThongBao == thongBao.MaThongBao);

        if (existing == null)
            return null;

        existing.MaNguoiDung = thongBao.MaNguoiDung;
        existing.LoaiThongBao = thongBao.LoaiThongBao;
        existing.TieuDe = thongBao.TieuDe;
        existing.NoiDung = thongBao.NoiDung;
        existing.MaHocVien = thongBao.MaHocVien;
        existing.MaLop = thongBao.MaLop;
        existing.MaHoaDon = thongBao.MaHoaDon;
        existing.DaDoc = thongBao.DaDoc;

        await _context.SaveChangesAsync();
        return existing;
    }

    public async Task<bool> MarkAsReadAsync(int id)
    {
        var entity = await _context.ThongBaos
            .FirstOrDefaultAsync(x => x.MaThongBao == id);

        if (entity == null)
            return false;

        entity.DaDoc = true;
        await _context.SaveChangesAsync();
        return true;
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var entity = await _context.ThongBaos
            .FirstOrDefaultAsync(x => x.MaThongBao == id);

        if (entity == null)
            return false;

        _context.ThongBaos.Remove(entity);
        await _context.SaveChangesAsync();
        return true;
    }
}
