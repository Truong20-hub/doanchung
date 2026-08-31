using BLL.Interfaces;
using DAL.Entities;
using DAL.Interfaces;
using DTO.KhoaHoc;

namespace BLL.Services
{
    public class KhoaHocService : IKhoaHocService
    {
        private readonly IKhoaHocRepository _khoaHocRepository;

        public KhoaHocService(
            IKhoaHocRepository khoaHocRepository)
        {
            _khoaHocRepository = khoaHocRepository;
        }


        // =====================================================
        // 1. LẤY TẤT CẢ
        // =====================================================

        public async Task<IEnumerable<KhoaHocResponse>> GetAllAsync()
        {
            var data = await _khoaHocRepository.GetAllAsync();

            return data.Select(MapToResponse);
        }


        // =====================================================
        // 2. LẤY THEO ID
        // =====================================================

        public async Task<KhoaHocResponse?> GetByIdAsync(int id)
        {
            var khoaHocl =
                await _khoaHocRepository.GetByIdAsync(id);

            if (khoaHocl == null)
                return null;

            return MapToResponse(khoaHocl);
        }


        // =====================================================
        // 3. TÌM THEO TÊN
        // =====================================================

        public async Task<IEnumerable<KhoaHocResponse>> GetByNameAsync(
            string tenKhoaHoc)
        {
            var data =
                await _khoaHocRepository.GetByNameAsync(tenKhoaHoc);

            return data.Select(MapToResponse);
        }


        // =====================================================
        // 4. TÌM THEO TRÌNH ĐỘ
        // =====================================================

        public async Task<IEnumerable<KhoaHocResponse>> GetByTrinhDoAsync(
            string trinhDo)
        {
            var data =
                await _khoaHocRepository.GetByTrinhDoAsync(trinhDo);

            return data.Select(MapToResponse);
        }


        // =====================================================
        // 5. TÌM THEO HỌC PHÍ
        // =====================================================

        public async Task<IEnumerable<KhoaHocResponse>> GetByHocPhiAsync(
            decimal hocPhi)
        {
            var data =
                await _khoaHocRepository.GetByHocPhiAsync(hocPhi);

            return data.Select(MapToResponse);
        }


        // =====================================================
        // 6. PHÂN TRANG
        // =====================================================

        public async Task<object> GetPagedAsync(
            int pageNumber,
            int pageSize)
        {
            return await _khoaHocRepository.GetPagedAsync(
                pageNumber,
                pageSize);
        }


        // =====================================================
        // 7. PHÂN TRANG + TÊN
        // =====================================================

        public async Task<object> GetPagedByNameAsync(
            string tenKhoaHoc,
            int pageNumber,
            int pageSize)
        {
            return await _khoaHocRepository.GetPagedByNameAsync(
                tenKhoaHoc,
                pageNumber,
                pageSize);
        }


        // =====================================================
        // 8. PHÂN TRANG + TRÌNH ĐỘ
        // =====================================================

        public async Task<object> GetPagedByTrinhDoAsync(
            string trinhDo,
            int pageNumber,
            int pageSize)
        {
            return await _khoaHocRepository.GetPagedByTrinhDoAsync(
                trinhDo,
                pageNumber,
                pageSize);
        }


        // =====================================================
        // 9. PHÂN TRANG + HỌC PHÍ
        // =====================================================

        public async Task<object> GetPagedByHocPhiAsync(
            decimal hocPhi,
            int pageNumber,
            int pageSize)
        {
            return await _khoaHocRepository.GetPagedByHocPhiAsync(
                hocPhi,
                pageNumber,
                pageSize);
        }


        // =====================================================
        // 10. TẠO KHÓA HỌC
        // =====================================================

        public async Task<KhoaHocResponse> CreateAsync(
            CreateKhoaHocRequest request)
        {
            // 1. Kiểm tra tên khóa học
            bool exists = await _khoaHocRepository
                .ExistsByTenKhoaHocAsync(request.TenKhoaHoc);
            // 2. Nếu đã tồn tại
            if (exists)
            {
                throw new Exception(
                    "Tên khóa học đã tồn tại trong hệ thống.");
            }
            // 3. Kiểm tra mã code
            bool codeExists = await _khoaHocRepository
                .ExistsByMaCodeAsync(request.MaCode);
            if (codeExists) { 
                throw new Exception("Mã code đã tồn tại trong hệ thống.");
            } 

            var khoaHoc = new KhoaHoc
            {
                TenKhoaHoc = request.TenKhoaHoc,
                MaCode = request.MaCode,
                TrinhDo = request.TrinhDo,
                MoTa = request.MoTa,
                TongSoBuoi = request.TongSoBuoi,
                SoTuanHoc = request.SoTuanHoc,
                HocPhiChuan = request.HocPhiChuan,
                DangHoatDong = request.DangHoatDong ?? true
            };

            var result =
                await _khoaHocRepository.AddAsync(khoaHoc);

            return MapToResponse(result);
        }


        // =====================================================
        // 11. CẬP NHẬT
        // =====================================================

        public async Task<KhoaHocResponse?> UpdateAsync(
            int id,
            UpdateKhoaHocRequest request)
        {
            var existing =
                await _khoaHocRepository.GetByIdAsync(id);

            if (existing == null)
                return null;

            // Kiểm tra mã code có bị trùng
            if (!string.IsNullOrWhiteSpace(request.MaCode))
            {
                var all =
                    await _khoaHocRepository.GetAllAsync();

                var duplicate = all.Any(x =>
                    x.MaCode == request.MaCode &&
                    x.MaKhoaHoc != id);

                if (duplicate)
                {
                    throw new Exception(
                        "Mã khóa học đã tồn tại.");
                }
            }

            var khoaHoc = new KhoaHoc
            {
                MaKhoaHoc = id,
                TenKhoaHoc = request.TenKhoaHoc,
                MaCode = request.MaCode,
                TrinhDo = request.TrinhDo,
                MoTa = request.MoTa,
                TongSoBuoi = request.TongSoBuoi,
                SoTuanHoc = request.SoTuanHoc,
                HocPhiChuan = request.HocPhiChuan,
                DangHoatDong = request.DangHoatDong
            };

            var result =
                await _khoaHocRepository.UpdateAsync(khoaHoc);

            if (result == null)
                return null;

            return MapToResponse(result);
        }


        // =====================================================
        // 12. XÓA
        // =====================================================

        public async Task<bool> DeleteAsync(int id)
        {
            return await _khoaHocRepository.DeleteAsync(id);
        }


        // =====================================================
        // 13. TÌM KIẾM
        // =====================================================

        public async Task<IEnumerable<KhoaHocResponse>> SearchAsync(
            string? tenKhoaHoc,
            string? trinhDo,
            decimal? hocPhi)
        {
            var data =
                await _khoaHocRepository.SearchAsync(
                    tenKhoaHoc,
                    trinhDo,
                    hocPhi);

            return data.Select(MapToResponse);
        }


        // =====================================================
        // 14. TÌM KIẾM + PHÂN TRANG
        // =====================================================

        public async Task<object> SearchPagedAsync(
            string? tenKhoaHoc,
            string? trinhDo,
            decimal? hocPhi,
            int pageNumber,
            int pageSize)
        {
            return await _khoaHocRepository.SearchPagedAsync(
                tenKhoaHoc,
                trinhDo,
                hocPhi,
                pageNumber,
                pageSize);
        }


        // =====================================================
        // MAPPING ENTITY → RESPONSE
        // =====================================================

        private static KhoaHocResponse MapToResponse(
            KhoaHoc khoaHoc)
        {
            return new KhoaHocResponse
            {
                MaKhoaHoc = khoaHoc.MaKhoaHoc,
                TenKhoaHoc = khoaHoc.TenKhoaHoc,
                MaCode = khoaHoc.MaCode,
                TrinhDo = khoaHoc.TrinhDo,
                MoTa = khoaHoc.MoTa,
                TongSoBuoi = khoaHoc.TongSoBuoi,
                SoTuanHoc = khoaHoc.SoTuanHoc,
                HocPhiChuan = khoaHoc.HocPhiChuan,
                DangHoatDong = khoaHoc.DangHoatDong
            };
        }
    }
}