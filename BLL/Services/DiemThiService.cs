using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BLL.Interfaces;
using DAL.Entities;
using DAL.Interfaces;
using DTO.DiemThi;

namespace BLL.Services;

public class DiemThiService : IDiemThiService
{
    private readonly IDiemThiRepository _diemThiRepository;

    public DiemThiService(IDiemThiRepository diemThiRepository)
    {
        _diemThiRepository = diemThiRepository;
    }

    public async Task<IEnumerable<DiemThiResponse>> GetAllAsync()
    {
        var list = await _diemThiRepository.GetAllAsync();
        return list.Select(MapToResponse);
    }

    public async Task<DiemThiResponse?> GetByIdAsync(int maDiemThi)
    {
        var entity = await _diemThiRepository.GetByIdAsync(maDiemThi);
        return entity == null ? null : MapToResponse(entity);
    }

    public async Task<IEnumerable<DiemThiResponse>> GetByMaKyThiAsync(int maKyThi)
    {
        var list = await _diemThiRepository.GetByMaKyThiAsync(maKyThi);
        return list.Select(MapToResponse);
    }

    public async Task<IEnumerable<DiemThiResponse>> GetByMaHocVienAsync(int maHocVien)
    {
        var list = await _diemThiRepository.GetByMaHocVienAsync(maHocVien);
        return list.Select(MapToResponse);
    }

    public async Task<DiemThiResponse> CreateAsync(CreateDiemThiRequest request)
    {
        var daTonTai = await _diemThiRepository.ExistsForKyThiAndHocVienAsync(request.MaKyThi, request.MaHocVien);
        if (daTonTai)
        {
            throw new InvalidOperationException("Học viên này đã có điểm cho kỳ thi này.");
        }

        var entity = new DiemThi
        {
            MaKyThi = request.MaKyThi,
            MaHocVien = request.MaHocVien,
            DiemNghe = request.DiemNghe,
            DiemNoi = request.DiemNoi,
            DiemDoc = request.DiemDoc,
            DiemViet = request.DiemViet,
            NhanXet = request.NhanXet
        };

        entity.TongDiem = TinhTongDiem(entity.DiemNghe, entity.DiemNoi, entity.DiemDoc, entity.DiemViet);

        var created = await _diemThiRepository.AddAsync(entity);
        var result = await _diemThiRepository.GetByIdAsync(created.MaDiemThi);
        return MapToResponse(result!);
    }

    public async Task<DiemThiResponse> UpdateAsync(UpdateDiemThiRequest request)
    {
        var existing = await _diemThiRepository.GetByIdAsync(request.MaDiemThi)
            ?? throw new KeyNotFoundException($"Không tìm thấy điểm thi có mã {request.MaDiemThi}.");

        var daTonTai = await _diemThiRepository.ExistsForKyThiAndHocVienAsync(
            request.MaKyThi, request.MaHocVien, request.MaDiemThi);

        if (daTonTai)
        {
            throw new InvalidOperationException("Học viên này đã có điểm cho kỳ thi này.");
        }

        existing.MaKyThi = request.MaKyThi;
        existing.MaHocVien = request.MaHocVien;
        existing.DiemNghe = request.DiemNghe;
        existing.DiemNoi = request.DiemNoi;
        existing.DiemDoc = request.DiemDoc;
        existing.DiemViet = request.DiemViet;
        existing.NhanXet = request.NhanXet;
        existing.TongDiem = TinhTongDiem(existing.DiemNghe, existing.DiemNoi, existing.DiemDoc, existing.DiemViet);

        var updated = await _diemThiRepository.UpdateAsync(existing);
        var result = await _diemThiRepository.GetByIdAsync(updated.MaDiemThi);
        return MapToResponse(result!);
    }

    public async Task<bool> DeleteAsync(int maDiemThi)
    {
        var exists = await _diemThiRepository.ExistsAsync(maDiemThi);
        if (!exists)
        {
            throw new KeyNotFoundException($"Không tìm thấy điểm thi có mã {maDiemThi}.");
        }

        return await _diemThiRepository.DeleteAsync(maDiemThi);
    }

    // Tính điểm tổng = trung bình cộng các kỹ năng có điểm (bỏ qua kỹ năng null)
    private static decimal? TinhTongDiem(decimal? nghe, decimal? noi, decimal? doc, decimal? viet)
    {
        var diemList = new List<decimal?> { nghe, noi, doc, viet }
            .Where(d => d.HasValue)
            .Select(d => d!.Value)
            .ToList();

        if (diemList.Count == 0)
        {
            return null;
        }

        return Math.Round(diemList.Average(), 2);
    }

    private static DiemThiResponse MapToResponse(DiemThi entity)
    {
        return new DiemThiResponse
        {
            MaDiemThi = entity.MaDiemThi,
            MaKyThi = entity.MaKyThi,
            TenKyThi = entity.MaKyThiNavigation?.TenKyThi,       // đổi theo thuộc tính thật của KyThi
            MaHocVien = entity.MaHocVien,
            TenHocVien = entity.MaHocVienNavigation?.HoTen,       // đổi theo thuộc tính thật của HocVien
            DiemNghe = entity.DiemNghe,
            DiemNoi = entity.DiemNoi,
            DiemDoc = entity.DiemDoc,
            DiemViet = entity.DiemViet,
            TongDiem = entity.TongDiem,
            NhanXet = entity.NhanXet
        };
    }
}