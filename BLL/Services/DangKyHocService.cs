using BLL.Interfaces;
using DAL.Entities;
using DAL.Interfaces;
using DTO.DangKyHoc;

namespace BLL.Services
{
    public class DangKyHocService : IDangKyHocService
    {
        private readonly IDangKyHocRepository _repository;

        public DangKyHocService(
            IDangKyHocRepository repository)
        {
            _repository = repository;
        }

        // =====================================================
        // MAPPING
        // =====================================================

        private DangKyHocResponse MapToResponse(
            DangKyHoc entity)
        {
            return new DangKyHocResponse
            {
                MaDangKy = entity.MaDangKy,

                MaHocVien = entity.MaHocVien,

                TenHocVien =
                    entity.MaHocVienNavigation?.HoTen,

                MaLop = entity.MaLop,

                MaLopCode =
                    entity.MaLopNavigation?.MaLopCode,

                TenLop =
                    entity.MaLopNavigation?.TenLop,

                NgayDangKy = entity.NgayDangKy,

                TrangThai = entity.TrangThai
            };
        }

        // =====================================================
        // VALIDATE TRẠNG THÁI
        // =====================================================

        private void ValidateTrangThai(
            string? trangThai)
        {
            if (string.IsNullOrWhiteSpace(trangThai))
                return;

            var allowedStatuses = new[]
            {
                "DangHoc",
                "DaHuy",
                "HoanThanh",
                "BaoLuu"
            };

            if (!allowedStatuses.Contains(
                    trangThai,
                    StringComparer.OrdinalIgnoreCase))
            {
                throw new ArgumentException(
                    "Trạng thái không hợp lệ. " +
                    "Các trạng thái hợp lệ: " +
                    "DangHoc, DaHuy, HoanThanh, BaoLuu.");
            }
        }

        // =====================================================
        // GET ALL
        // =====================================================

        public async Task<IEnumerable<DangKyHocResponse>>
            GetAllAsync()
        {
            var data =
                await _repository.GetAllAsync();

            return data.Select(MapToResponse);
        }

        // =====================================================
        // GET BY ID
        // =====================================================

        public async Task<DangKyHocResponse?>
            GetByIdAsync(int id)
        {
            var entity =
                await _repository.GetByIdAsync(id);

            if (entity == null)
                return null;

            return MapToResponse(entity);
        }

        // =====================================================
        // GET BY HỌC VIÊN
        // =====================================================

        public async Task<IEnumerable<DangKyHocResponse>>
            GetByHocVienIdAsync(int maHocVien)
        {
            if (!await _repository.ExistsHocVienAsync(
                    maHocVien))
            {
                throw new KeyNotFoundException(
                    $"Học viên có mã {maHocVien} không tồn tại.");
            }

            var data =
                await _repository
                    .GetByHocVienIdAsync(maHocVien);

            return data.Select(MapToResponse);
        }

        // =====================================================
        // GET BY LỚP
        // =====================================================

        public async Task<IEnumerable<DangKyHocResponse>>
            GetByLopIdAsync(int maLop)
        {
            if (!await _repository.ExistsLopAsync(
                    maLop))
            {
                throw new KeyNotFoundException(
                    $"Lớp học có mã {maLop} không tồn tại.");
            }

            var data =
                await _repository
                    .GetByLopIdAsync(maLop);

            return data.Select(MapToResponse);
        }

        // =====================================================
        // GET BY TRẠNG THÁI
        // =====================================================

        public async Task<IEnumerable<DangKyHocResponse>>
            GetByTrangThaiAsync(string trangThai)
        {
            ValidateTrangThai(trangThai);

            var data =
                await _repository
                    .GetByTrangThaiAsync(trangThai);

            return data.Select(MapToResponse);
        }

        // =====================================================
        // CREATE
        // =====================================================

        public async Task<DangKyHocResponse>
            CreateAsync(
                CreateDangKyHocRequest request)
        {
            // -------------------------------------------------
            // 1. Kiểm tra học viên
            // -------------------------------------------------

            if (!await _repository.ExistsHocVienAsync(
                    request.MaHocVien))
            {
                throw new KeyNotFoundException(
                    $"Học viên có mã {request.MaHocVien} không tồn tại.");
            }

            // -------------------------------------------------
            // 2. Kiểm tra lớp
            // -------------------------------------------------

            if (!await _repository.ExistsLopAsync(
                    request.MaLop))
            {
                throw new KeyNotFoundException(
                    $"Lớp học có mã {request.MaLop} không tồn tại.");
            }

            // -------------------------------------------------
            // 3. Kiểm tra trùng học viên + lớp
            // -------------------------------------------------

            if (await _repository.ExistsHocVienLopAsync(
                    request.MaHocVien,
                    request.MaLop))
            {
                throw new InvalidOperationException(
                    "Học viên đã đăng ký lớp học này.");
            }

            // -------------------------------------------------
            // 4. Kiểm tra trạng thái
            // -------------------------------------------------

            ValidateTrangThai(request.TrangThai);

            // -------------------------------------------------
            // 5. Tạo Entity
            // -------------------------------------------------

            var entity = new DangKyHoc
            {
                MaHocVien = request.MaHocVien,

                MaLop = request.MaLop,

                NgayDangKy =
                    request.NgayDangKy
                    ?? DateOnly.FromDateTime(
                        DateTime.Now),

                TrangThai =
                    request.TrangThai ?? "DangHoc"
            };

            // -------------------------------------------------
            // 6. Lưu database
            // -------------------------------------------------

            var result =
                await _repository.AddAsync(entity);

            return MapToResponse(result);
        }

        // =====================================================
        // UPDATE
        // =====================================================

        public async Task<DangKyHocResponse?>
            UpdateAsync(
                int id,
                UpdateDangKyHocRequest request)
        {
            // -------------------------------------------------
            // 1. Kiểm tra đăng ký tồn tại
            // -------------------------------------------------

            var entity =
                await _repository.GetByIdAsync(id);

            if (entity == null)
            {
                throw new KeyNotFoundException(
                    $"Đăng ký học có mã {id} không tồn tại.");
            }

            // -------------------------------------------------
            // 2. Kiểm tra học viên
            // -------------------------------------------------

            if (!await _repository.ExistsHocVienAsync(
                    request.MaHocVien))
            {
                throw new KeyNotFoundException(
                    $"Học viên có mã {request.MaHocVien} không tồn tại.");
            }

            // -------------------------------------------------
            // 3. Kiểm tra lớp
            // -------------------------------------------------

            if (!await _repository.ExistsLopAsync(
                    request.MaLop))
            {
                throw new KeyNotFoundException(
                    $"Lớp học có mã {request.MaLop} không tồn tại.");
            }

            // -------------------------------------------------
            // 4. Kiểm tra trùng
            // -------------------------------------------------

            if (await _repository
                .ExistsHocVienLopExceptIdAsync(
                    request.MaHocVien,
                    request.MaLop,
                    id))
            {
                throw new InvalidOperationException(
                    "Học viên đã đăng ký lớp học này.");
            }

            // -------------------------------------------------
            // 5. Kiểm tra trạng thái
            // -------------------------------------------------

            ValidateTrangThai(request.TrangThai);

            // -------------------------------------------------
            // 6. Cập nhật Entity
            // -------------------------------------------------

            entity.MaHocVien =
                request.MaHocVien;

            entity.MaLop =
                request.MaLop;

            entity.NgayDangKy =
                request.NgayDangKy;

            entity.TrangThai =
                request.TrangThai;

            // -------------------------------------------------
            // 7. Update database
            // -------------------------------------------------

            var result =
                await _repository.UpdateAsync(entity);

            if (result == null)
            {
                throw new Exception(
                    "Cập nhật đăng ký học thất bại.");
            }

            return MapToResponse(result);
        }

        // =====================================================
        // DELETE
        // =====================================================

        public async Task<bool>
            DeleteAsync(int id)
        {
            // -------------------------------------------------
            // Kiểm tra tồn tại
            // -------------------------------------------------

            if (!await _repository.ExistsByIdAsync(id))
            {
                throw new KeyNotFoundException(
                    $"Đăng ký học có mã {id} không tồn tại.");
            }

            try
            {
                return await _repository.DeleteAsync(id);
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException(
                    "Không thể xóa đăng ký học vì dữ liệu " +
                    "này đang được bảng khác tham chiếu.",
                    ex);
            }
        }

        // =====================================================
        // SEARCH
        // =====================================================

        public async Task<IEnumerable<DangKyHocResponse>>
            SearchAsync(
                int? maHocVien,
                int? maLop,
                string? trangThai)
        {
            if (maHocVien.HasValue &&
                !await _repository.ExistsHocVienAsync(
                    maHocVien.Value))
            {
                throw new KeyNotFoundException(
                    $"Học viên có mã {maHocVien} không tồn tại.");
            }

            if (maLop.HasValue &&
                !await _repository.ExistsLopAsync(
                    maLop.Value))
            {
                throw new KeyNotFoundException(
                    $"Lớp học có mã {maLop} không tồn tại.");
            }

            if (!string.IsNullOrWhiteSpace(trangThai))
            {
                ValidateTrangThai(trangThai);
            }

            var data =
                await _repository.SearchAsync(
                    maHocVien,
                    maLop,
                    trangThai);

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
            ValidatePaging(
                pageNumber,
                pageSize);

            return await _repository
                .GetPagedAsync(
                    pageNumber,
                    pageSize);
        }

        // =====================================================
        // SEARCH + PAGED
        // =====================================================

        public async Task<object>
            SearchPagedAsync(
                int? maHocVien,
                int? maLop,
                string? trangThai,
                int pageNumber,
                int pageSize)
        {
            ValidatePaging(
                pageNumber,
                pageSize);

            if (maHocVien.HasValue &&
                !await _repository.ExistsHocVienAsync(
                    maHocVien.Value))
            {
                throw new KeyNotFoundException(
                    $"Học viên có mã {maHocVien} không tồn tại.");
            }

            if (maLop.HasValue &&
                !await _repository.ExistsLopAsync(
                    maLop.Value))
            {
                throw new KeyNotFoundException(
                    $"Lớp học có mã {maLop} không tồn tại.");
            }

            if (!string.IsNullOrWhiteSpace(trangThai))
            {
                ValidateTrangThai(trangThai);
            }

            return await _repository
                .SearchPagedAsync(
                    maHocVien,
                    maLop,
                    trangThai,
                    pageNumber,
                    pageSize);
        }

        // =====================================================
        // PAGED THEO HỌC VIÊN
        // =====================================================

        public async Task<object>
            GetPagedByHocVienIdAsync(
                int maHocVien,
                int pageNumber,
                int pageSize)
        {
            ValidatePaging(
                pageNumber,
                pageSize);

            if (!await _repository.ExistsHocVienAsync(
                    maHocVien))
            {
                throw new KeyNotFoundException(
                    $"Học viên có mã {maHocVien} không tồn tại.");
            }

            return await _repository
                .GetPagedByHocVienIdAsync(
                    maHocVien,
                    pageNumber,
                    pageSize);
        }

        // =====================================================
        // PAGED THEO LỚP
        // =====================================================

        public async Task<object>
            GetPagedByLopIdAsync(
                int maLop,
                int pageNumber,
                int pageSize)
        {
            ValidatePaging(
                pageNumber,
                pageSize);

            if (!await _repository.ExistsLopAsync(
                    maLop))
            {
                throw new KeyNotFoundException(
                    $"Lớp học có mã {maLop} không tồn tại.");
            }

            return await _repository
                .GetPagedByLopIdAsync(
                    maLop,
                    pageNumber,
                    pageSize);
        }

        // =====================================================
        // PAGED THEO TRẠNG THÁI
        // =====================================================

        public async Task<object>
            GetPagedByTrangThaiAsync(
                string trangThai,
                int pageNumber,
                int pageSize)
        {
            ValidatePaging(
                pageNumber,
                pageSize);

            ValidateTrangThai(trangThai);

            return await _repository
                .GetPagedByTrangThaiAsync(
                    trangThai,
                    pageNumber,
                    pageSize);
        }

        // =====================================================
        // VALIDATE PAGING
        // =====================================================

        private void ValidatePaging(
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
        }
    }
}