using DAL.Context;
using DAL.Entities;
using DAL.Interfaces;
using Microsoft.EntityFrameworkCore;
using DAL.Model;

namespace DAL.Repositories;

public class TinNhanRepository : ITinNhanRepository
{
    private readonly AppDbContext _context;

    public TinNhanRepository(AppDbContext context)
    {
        _context = context;
    }

    private IQueryable<TinNhan> QueryWithRelations()
    {
        return _context.TinNhans
            .Include(x => x.MaNguoiGuiNavigation)
            .Include(x => x.MaNguoiNhanNavigation)
            .Include(x => x.MaHocVienNavigation);
    }

    public async Task<IEnumerable<TinNhan>> GetAllAsync()
    {
        return await QueryWithRelations()
            .AsNoTracking()
            .OrderByDescending(x => x.ThoiGianGui)
            .ToListAsync();
    }

    public async Task<TinNhan?> GetByIdAsync(int id)
    {
        return await QueryWithRelations()
            .FirstOrDefaultAsync(x => x.MaTinNhan == id);
    }

    public async Task<IEnumerable<TinNhan>> GetByMaNguoiDungAsync(int maNguoiDung)
    {
        return await QueryWithRelations()
            .Where(x => x.MaNguoiGui == maNguoiDung || x.MaNguoiNhan == maNguoiDung)
            .AsNoTracking()
            .OrderByDescending(x => x.ThoiGianGui)
            .ToListAsync();
    }

    public async Task<IEnumerable<TinNhan>> GetConversationAsync(
        int maNguoiDung1,
        int maNguoiDung2,
        int maHocVien)
    {
        return await QueryWithRelations()
            .Where(x => x.MaHocVien == maHocVien &&
                ((x.MaNguoiGui == maNguoiDung1 && x.MaNguoiNhan == maNguoiDung2) ||
                 (x.MaNguoiGui == maNguoiDung2 && x.MaNguoiNhan == maNguoiDung1)))
            .AsNoTracking()
            .OrderBy(x => x.ThoiGianGui)
            .ThenBy(x => x.MaTinNhan)
            .ToListAsync();
    }

    public async Task<IEnumerable<TinNhan>> SearchAsync(string? keyword)
    {
        var query = QueryWithRelations();

        if (!string.IsNullOrWhiteSpace(keyword))
        {
            keyword = keyword.Trim();
            query = query.Where(x =>
                x.NoiDung.Contains(keyword) ||
                x.MaNguoiGuiNavigation.HoTen.Contains(keyword) ||
                x.MaNguoiNhanNavigation.HoTen.Contains(keyword) ||
                x.MaHocVienNavigation.HoTen.Contains(keyword));
        }

        return await query
            .AsNoTracking()
            .OrderByDescending(x => x.ThoiGianGui)
            .ToListAsync();
    }

    public async Task<object> GetPagedAsync(int pageNumber, int pageSize)
    {
        var query = QueryWithRelations().AsNoTracking();
        var totalItems = await query.CountAsync();
        var data = await query
            .OrderByDescending(x => x.ThoiGianGui)
            .ThenByDescending(x => x.MaTinNhan)
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
                x.NoiDung.Contains(keyword) ||
                x.MaNguoiGuiNavigation.HoTen.Contains(keyword) ||
                x.MaNguoiNhanNavigation.HoTen.Contains(keyword) ||
                x.MaHocVienNavigation.HoTen.Contains(keyword));
        }

        var totalItems = await query.CountAsync();
        var data = await query
            .OrderByDescending(x => x.ThoiGianGui)
            .ThenByDescending(x => x.MaTinNhan)
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
        return await _context.TinNhans.AnyAsync(x => x.MaTinNhan == id);
    }

    public async Task<TinNhan> AddAsync(TinNhan tinNhan)
    {
        await _context.TinNhans.AddAsync(tinNhan);
        await _context.SaveChangesAsync();
        return tinNhan;
    }

    public async Task<TinNhan?> UpdateAsync(TinNhan tinNhan)
    {
        var existing = await _context.TinNhans
            .FirstOrDefaultAsync(x => x.MaTinNhan == tinNhan.MaTinNhan);

        if (existing == null)
            return null;

        existing.MaNguoiGui = tinNhan.MaNguoiGui;
        existing.MaNguoiNhan = tinNhan.MaNguoiNhan;
        existing.MaHocVien = tinNhan.MaHocVien;
        existing.NoiDung = tinNhan.NoiDung;
        existing.DaDoc = tinNhan.DaDoc;

        await _context.SaveChangesAsync();
        return existing;
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var entity = await _context.TinNhans
            .FirstOrDefaultAsync(x => x.MaTinNhan == id);

        if (entity == null)
            return false;

        _context.TinNhans.Remove(entity);
        await _context.SaveChangesAsync();
        return true;
    }

    public async Task<NguoiGuiSummary[]> GetDanhSachNguoiGuiChoNguoiNhanAsync(int maNguoiNhan)
    {
        var rows = await _context.TinNhans
            .AsNoTracking()
            .Where(x => x.MaNguoiNhan == maNguoiNhan)
            .GroupBy(x => new { x.MaNguoiGui, x.MaNguoiGuiNavigation.HoTen })
            .Select(g => new
            {
                g.Key.MaNguoiGui,
                g.Key.HoTen,
                Tong = g.Count(),
                ChuaDoc = g.Count(x => !x.DaDoc),
                GanNhat = g.Max(x => x.ThoiGianGui)
            })
            .ToListAsync();

        return rows
            .Select(r => new NguoiGuiSummary(r.MaNguoiGui, r.HoTen, r.Tong, r.ChuaDoc, r.GanNhat))
            .OrderByDescending(x => x.TinGanNhat)
            .ToArray();
    }


    public async Task<TinNhanItem[]> GetCuocTroChuyenAsync(int maNguoiA, int maNguoiB)
    {
        return await _context.TinNhans
            .AsNoTracking()
            .Where(x => (x.MaNguoiGui == maNguoiA && x.MaNguoiNhan == maNguoiB)
                     || (x.MaNguoiGui == maNguoiB && x.MaNguoiNhan == maNguoiA))
            .OrderBy(x => x.ThoiGianGui)
            .Select(x => new TinNhanItem(x.MaTinNhan, x.MaNguoiGui, x.MaNguoiNhan, x.NoiDung, x.ThoiGianGui))
            .ToArrayAsync();
    }
    public async Task<NguoiGuiSummary[]> GetDanhSachNguoiGuiAsync()
    {
        var rows = await _context.TinNhans
            .AsNoTracking()
            .GroupBy(x => new { x.MaNguoiGui, x.MaNguoiGuiNavigation.HoTen })
            .Select(g => new
            {
                g.Key.MaNguoiGui,
                g.Key.HoTen,
                Tong = g.Count(),
                ChuaDoc = g.Count(x => !x.DaDoc),
                GanNhat = g.Max(x => x.ThoiGianGui)
            })
            .ToListAsync();

        return rows
            .Select(r => new NguoiGuiSummary(r.MaNguoiGui, r.HoTen, r.Tong, r.ChuaDoc, r.GanNhat))
            .OrderByDescending(x => x.TinGanNhat)
            .ToArray();
    }
}
