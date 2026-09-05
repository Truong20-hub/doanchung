using DAL.Context;
using DTO.BaoCao;
using DAL.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace DAL.Repositories;

public class BaoCaoRepository : IBaoCaoRepository
{
    private readonly AppDbContext _context;

    public BaoCaoRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<BaoCaoDiemResponse>> GetBaoCaoDiemAsync(
        int? maLop,
        int? maHocVien)
    {
        var query = _context.DiemThis
            .AsNoTracking()
            .Include(x => x.MaHocVienNavigation)
            .Include(x => x.MaKyThiNavigation)
                .ThenInclude(x => x.MaLopNavigation)
            .AsQueryable();

        if (maLop.HasValue)
            query = query.Where(x => x.MaKyThiNavigation.MaLop == maLop.Value);

        if (maHocVien.HasValue)
            query = query.Where(x => x.MaHocVien == maHocVien.Value);

        return await query
            .OrderBy(x => x.MaKyThiNavigation.MaLop)
            .ThenBy(x => x.MaHocVienNavigation.HoTen)
            .ThenByDescending(x => x.MaKyThiNavigation.NgayThi)
            .Select(x => new BaoCaoDiemResponse
            {
                MaLop = x.MaKyThiNavigation.MaLop,
                MaLopCode = x.MaKyThiNavigation.MaLopNavigation.MaLopCode,
                TenLop = x.MaKyThiNavigation.MaLopNavigation.TenLop,
                MaHocVien = x.MaHocVien,
                HoTenHocVien = x.MaHocVienNavigation.HoTen,
                MaKyThi = x.MaKyThi,
                TenKyThi = x.MaKyThiNavigation.TenKyThi,
                NgayThi = x.MaKyThiNavigation.NgayThi,
                DiemNghe = x.DiemNghe,
                DiemNoi = x.DiemNoi,
                DiemDoc = x.DiemDoc,
                DiemViet = x.DiemViet,
                TongDiem = x.TongDiem,
                NhanXet = x.NhanXet
            })
            .ToListAsync();
    }

    public async Task<IEnumerable<BaoCaoChuyenCanResponse>> GetBaoCaoChuyenCanAsync(
        int? maLop)
    {
        var query = _context.DiemDanhs
            .AsNoTracking()
            .Include(x => x.MaBuoiNavigation)
                .ThenInclude(x => x.MaLopNavigation)
            .AsQueryable();

        if (maLop.HasValue)
            query = query.Where(x => x.MaBuoiNavigation.MaLop == maLop.Value);

        var records = await query
            .Select(x => new
            {
                MaLop = x.MaBuoiNavigation.MaLop,
                MaLopCode = x.MaBuoiNavigation.MaLopNavigation.MaLopCode,
                TenLop = x.MaBuoiNavigation.MaLopNavigation.TenLop,
                MaBuoi = x.MaBuoi,
                TrangThai = x.TrangThai
            })
            .ToListAsync();

        return records
            .GroupBy(x => new { x.MaLop, x.MaLopCode, x.TenLop })
            .OrderBy(x => x.Key.MaLop)
            .Select(group =>
            {
                var totalAttendance = group.Count();
                var present = group.Count(x => x.TrangThai == "CoMat");
                var late = group.Count(x => x.TrangThai != null && x.TrangThai.StartsWith("Tre"));
                var absent = group.Count(x => x.TrangThai == "Vang");
                var excusedAbsent = group.Count(x => x.TrangThai == "VangCoPhep");
                var attended = present + late;

                return new BaoCaoChuyenCanResponse
                {
                    MaLop = group.Key.MaLop,
                    MaLopCode = group.Key.MaLopCode,
                    TenLop = group.Key.TenLop,
                    TongSoBuoi = group.Select(x => x.MaBuoi).Distinct().Count(),
                    TongLuotDiemDanh = totalAttendance,
                    SoLuotCoMat = present,
                    SoLuotTre = late,
                    SoLuotVang = absent,
                    SoLuotVangCoPhep = excusedAbsent,
                    TyLeThamDu = totalAttendance == 0
                        ? 0
                        : Math.Round((decimal)attended * 100 / totalAttendance, 2)
                };
            })
            .ToList();
    }

    public async Task<IEnumerable<BaoCaoHocPhiResponse>> GetBaoCaoHocPhiAsync(
        int nam,
        int? thang)
    {
        var invoices = await _context.HoaDons
            .AsNoTracking()
            .Include(x => x.ThanhToans)
            .Where(x => x.NgayLap.HasValue &&
                        x.NgayLap.Value.Year == nam &&
                        (!thang.HasValue || x.NgayLap.Value.Month == thang.Value))
            .Select(x => new
            {
                NgayLap = x.NgayLap!.Value,
                ThanhTien = x.ThanhTien ?? x.TongTien - (x.GiamGia ?? 0),
                DaThu = x.ThanhToans.Sum(t => (decimal?)t.SoTienDaTra) ?? 0
            })
            .ToListAsync();

        return invoices
            .GroupBy(x => new { Nam = x.NgayLap.Year, Thang = x.NgayLap.Month })
            .OrderBy(x => x.Key.Nam)
            .ThenBy(x => x.Key.Thang)
            .Select(group =>
            {
                var totalDue = group.Sum(x => x.ThanhTien);
                var totalPaid = group.Sum(x => x.DaThu);
                var paidInvoices = group.Count(x => x.DaThu >= x.ThanhTien);

                return new BaoCaoHocPhiResponse
                {
                    Nam = group.Key.Nam,
                    Thang = group.Key.Thang,
                    TongSoHoaDon = group.Count(),
                    TongTienPhaiThu = totalDue,
                    TongTienDaThu = totalPaid,
                    TongTienConNo = Math.Max(0, totalDue - totalPaid),
                    SoHoaDonDaThanhToan = paidInvoices,
                    SoHoaDonChuaThanhToan = group.Count() - paidInvoices
                };
            })
            .ToList();
    }
}
