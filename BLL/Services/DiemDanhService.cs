using BLL.Interfaces;
using DAL.Entities;
using DAL.Interfaces;
using DTO.DiemDanh;

namespace BLL.Services
{
    public class DiemDanhService : IDiemDanhService
    {
        private readonly IDiemDanhRepository _repository;
        private readonly IBuoiHocRepository _buoiHocRepository;
        private readonly IHocVienRepository _hocVienRepository;
        private readonly IDangKyHocRepository _dangKyHocRepository;

        public DiemDanhService(
            IDiemDanhRepository repository,
            IBuoiHocRepository buoiHocRepository,
            IHocVienRepository hocVienRepository,
            IDangKyHocRepository dangKyHocRepository)
        {
            _repository = repository;
            _buoiHocRepository = buoiHocRepository;
            _hocVienRepository = hocVienRepository;
            _dangKyHocRepository = dangKyHocRepository;
        }

        // =====================================================
        // TRẠNG THÁI HỢP LỆ
        // =====================================================

        private static readonly string[] TrangThaiHopLe =
        {
            "Có mặt",
            "Vắng",
            "Có phép",
            "Đi muộn"
        };

        // =====================================================
        // VALIDATE TRẠNG THÁI
        // =====================================================

        private void ValidateTrangThai(
            string trangThai)
        {
            if (string.IsNullOrWhiteSpace(trangThai))
            {
                throw new ArgumentException(
                    "Trạng thái điểm danh không được để trống.");
            }

            if (!TrangThaiHopLe.Contains(
                    trangThai.Trim(),
                    StringComparer.OrdinalIgnoreCase))
            {
                throw new ArgumentException(
                    "Trạng thái không hợp lệ. " +
                    "Chỉ chấp nhận: Có mặt, Vắng, Có phép, Đi muộn.");
            }
        }

        // =====================================================
        // MAPPING
        // =====================================================

        private DiemDanhResponse
            MapToResponse(
                DiemDanh entity)
        {
            return new DiemDanhResponse
            {
                MaDiemDanh =
                    entity.MaDiemDanh,

                MaBuoi =
                    entity.MaBuoi,

                NgayHoc =
                    entity.MaBuoiNavigation.NgayHoc,

                GioBatDau =
                    entity.MaBuoiNavigation.GioBatDau,

                GioKetThuc =
                    entity.MaBuoiNavigation.GioKetThuc,

                MaLop =
                    entity.MaBuoiNavigation
                        .MaLop,

                MaLopCode =
                    entity.MaBuoiNavigation
                        .MaLopNavigation
                        .MaLopCode,

                TenLop =
                    entity.MaBuoiNavigation
                        .MaLopNavigation
                        .TenLop,

                MaHocVien =
                    entity.MaHocVien,

                TrangThai =
                    entity.TrangThai ?? "",

                GhiChu =
                    entity.GhiChu
            };
        }

        // =====================================================
        // GET ALL
        // =====================================================

        public async Task<IEnumerable<DiemDanhResponse>>
            GetAllAsync()
        {
            var data =
                await _repository.GetAllAsync();

            return data.Select(MapToResponse);
        }

        // =====================================================
        // GET BY ID
        // =====================================================

        public async Task<DiemDanhResponse?>
            GetByIdAsync(int id)
        {
            var entity =
                await _repository.GetByIdAsync(id);

            if (entity == null)
                return null;

            return MapToResponse(entity);
        }

        // =====================================================
        // GET BY BUỔI
        // =====================================================

        public async Task<IEnumerable<DiemDanhResponse>>
            GetByMaBuoiAsync(
                int maBuoi)
        {
            var buoi =
                await _buoiHocRepository
                    .GetByIdAsync(maBuoi);

            if (buoi == null)
            {
                throw new KeyNotFoundException(
                    $"Buổi học có mã {maBuoi} không tồn tại.");
            }

            var data =
                await _repository
                    .GetByMaBuoiAsync(maBuoi);

            return data.Select(MapToResponse);
        }

        // =====================================================
        // GET BY HỌC VIÊN
        // =====================================================

        public async Task<IEnumerable<DiemDanhResponse>>
            GetByMaHocVienAsync(
                int maHocVien)
        {
            var hocVien =
                await _hocVienRepository
                    .GetByIdAsync(maHocVien);

            if (hocVien == null)
            {
                throw new KeyNotFoundException(
                    $"Học viên có mã {maHocVien} không tồn tại.");
            }

            var data =
                await _repository
                    .GetByMaHocVienAsync(
                        maHocVien);

            return data.Select(MapToResponse);
        }

        // =====================================================
        // PAGED
        // =====================================================

        public async Task<object>
            GetPagedAsync(
                int pageNumber,
                int pageSize)
        {
            if (pageNumber <= 0)
            {
                throw new ArgumentException(
                    "PageNumber phải lớn hơn 0.");
            }

            if (pageSize <= 0)
            {
                throw new ArgumentException(
                    "PageSize phải lớn hơn 0.");
            }

            if (pageSize > 100)
            {
                throw new ArgumentException(
                    "PageSize không được lớn hơn 100.");
            }

            return await _repository
                .GetPagedAsync(
                    pageNumber,
                    pageSize);
        }

        // =====================================================
        // SEARCH PAGED
        // =====================================================

        public async Task<object>
            SearchPagedAsync(
                int? maBuoi,
                int? maHocVien,
                string? trangThai,
                int pageNumber,
                int pageSize)
        {
            if (pageNumber <= 0)
            {
                throw new ArgumentException(
                    "PageNumber phải lớn hơn 0.");
            }

            if (pageSize <= 0 ||
                pageSize > 100)
            {
                throw new ArgumentException(
                    "PageSize phải từ 1 đến 100.");
            }

            if (!string.IsNullOrWhiteSpace(trangThai))
            {
                ValidateTrangThai(trangThai);
            }

            return await _repository
                .SearchPagedAsync(
                    maBuoi,
                    maHocVien,
                    trangThai,
                    pageNumber,
                    pageSize);
        }

        // =====================================================
        // CREATE
        // =====================================================

        public async Task<DiemDanhResponse>
            CreateAsync(
                CreateDiemDanhRequest request)
        {
            // -------------------------------------------------
            // 1. VALIDATE TRẠNG THÁI
            // -------------------------------------------------

            ValidateTrangThai(
                request.TrangThai);

            // -------------------------------------------------
            // 2. KIỂM TRA BUỔI HỌC
            // -------------------------------------------------

            var buoi =
                await _buoiHocRepository
                    .GetByIdAsync(
                        request.MaBuoi);

            if (buoi == null)
            {
                throw new KeyNotFoundException(
                    $"Buổi học có mã {request.MaBuoi} không tồn tại.");
            }

            // -------------------------------------------------
            // 3. KIỂM TRA HỌC VIÊN
            // -------------------------------------------------

            var hocVien =
                await _hocVienRepository
                    .GetByIdAsync(
                        request.MaHocVien);

            if (hocVien == null)
            {
                throw new KeyNotFoundException(
                    $"Học viên có mã {request.MaHocVien} không tồn tại.");
            }

            // -------------------------------------------------
            // 4. KIỂM TRA ĐÃ ĐIỂM DANH CHƯA
            // -------------------------------------------------

            var exists =
                await _repository
                    .ExistsAsync(
                        request.MaBuoi,
                        request.MaHocVien);

            if (exists)
            {
                throw new InvalidOperationException(
                    "Học viên này đã được điểm danh trong buổi học.");
            }

            // -------------------------------------------------
            // 5. KIỂM TRA HỌC VIÊN CÓ ĐĂNG KÝ LỚP
            // -------------------------------------------------

            var dangKy =
                await _dangKyHocRepository
                    .GetByHocVienIdAsync(request.MaHocVien);

            if (dangKy == null)
            {
                throw new InvalidOperationException(
                    "Học viên chưa đăng ký lớp của buổi học này.");
            }

            // -------------------------------------------------
            // 6. TẠO ENTITY
            // -------------------------------------------------

            var entity = new DiemDanh
            {
                MaBuoi =
                    request.MaBuoi,

                MaHocVien =
                    request.MaHocVien,

                TrangThai =
                    request.TrangThai.Trim(),

                GhiChu =
                    request.GhiChu
            };

            // -------------------------------------------------
            // 7. SAVE
            // -------------------------------------------------

            var result =
                await _repository
                    .AddAsync(entity);

            // -------------------------------------------------
            // 8. LẤY LẠI ĐỂ MAPPING
            // -------------------------------------------------

            var created =
                await _repository
                    .GetByIdAsync(
                        result.MaDiemDanh);

            return MapToResponse(created!);
        }

        // =====================================================
        // UPDATE
        // =====================================================

        public async Task<DiemDanhResponse?>
            UpdateAsync(
                int id,
                UpdateDiemDanhRequest request)
        {
            // -------------------------------------------------
            // 1. KIỂM TRA BẢN GHI
            // -------------------------------------------------

            var existing =
                await _repository
                    .GetByIdAsync(id);

            if (existing == null)
            {
                throw new KeyNotFoundException(
                    $"Điểm danh có mã {id} không tồn tại.");
            }

            // -------------------------------------------------
            // 2. VALIDATE TRẠNG THÁI
            // -------------------------------------------------

            ValidateTrangThai(
                request.TrangThai);

            // -------------------------------------------------
            // 3. KIỂM TRA BUỔI HỌC
            // -------------------------------------------------

            var buoi =
                await _buoiHocRepository
                    .GetByIdAsync(
                        request.MaBuoi);

            if (buoi == null)
            {
                throw new KeyNotFoundException(
                    $"Buổi học có mã {request.MaBuoi} không tồn tại.");
            }

            // -------------------------------------------------
            // 4. KIỂM TRA HỌC VIÊN
            // -------------------------------------------------

            var hocVien =
                await _hocVienRepository
                    .GetByIdAsync(
                        request.MaHocVien);

            if (hocVien == null)
            {
                throw new KeyNotFoundException(
                    $"Học viên có mã {request.MaHocVien} không tồn tại.");
            }

            // -------------------------------------------------
            // 5. KIỂM TRA TRÙNG
            // -------------------------------------------------

            var duplicate =
                await _repository
                    .ExistsAsync(
                        request.MaBuoi,
                        request.MaHocVien);

            if (duplicate &&
                (existing.MaBuoi != request.MaBuoi ||
                 existing.MaHocVien != request.MaHocVien))
            {
                throw new InvalidOperationException(
                    "Học viên này đã có bản ghi điểm danh " +
                    "trong buổi học này.");
            }

            // -------------------------------------------------
            // 6. KIỂM TRA ĐĂNG KÝ LỚP
            // -------------------------------------------------

            var dangKy =
                await _dangKyHocRepository.GetByHocVienIdAndLopIdAsync(
                        request.MaHocVien,
                        buoi.MaLop);

            if (dangKy == null)
            {
                throw new InvalidOperationException(
                    "Học viên chưa đăng ký lớp của buổi học này.");
            }

            // -------------------------------------------------
            // 7. UPDATE
            // -------------------------------------------------

            existing.MaBuoi =
                request.MaBuoi;

            existing.MaHocVien =
                request.MaHocVien;

            existing.TrangThai =
                request.TrangThai.Trim();

            existing.GhiChu =
                request.GhiChu;

            var result =
                await _repository
                    .UpdateAsync(existing);

            if (result == null)
                return null;

            // -------------------------------------------------
            // 8. LẤY LẠI
            // -------------------------------------------------

            var updated =
                await _repository
                    .GetByIdAsync(id);

            return MapToResponse(updated!);
        }

        // =====================================================
        // DELETE
        // =====================================================

        public async Task<bool>
            DeleteAsync(int id)
        {
            var existing =
                await _repository
                    .GetByIdAsync(id);

            if (existing == null)
            {
                throw new KeyNotFoundException(
                    $"Điểm danh có mã {id} không tồn tại.");
            }

            return await _repository
                .DeleteAsync(id);
        }
    }
}