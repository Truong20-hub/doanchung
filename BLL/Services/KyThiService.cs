using BLL.Interfaces;
using DAL.Entities;
using DAL.Interfaces;
using DTO.KyThi;

namespace BLL.Services
{
    public class KyThiService : IKyThiService
    {
        private readonly IKyThiRepository _repository;
        private readonly ILopHocRepository _lopHocRepository;

        public KyThiService(
            IKyThiRepository repository,
            ILopHocRepository lopHocRepository)
        {
            _repository = repository;
            _lopHocRepository = lopHocRepository;
        }

        // =====================================================
        // MAPPING
        // =====================================================

        private KyThiResponse MapToResponse(
            KyThi entity)
        {
            return new KyThiResponse
            {
                MaKyThi = entity.MaKyThi,

                MaLop = entity.MaLop,

                MaLopCode =
                    entity.MaLopNavigation?.MaLopCode,

                TenLop =
                    entity.MaLopNavigation?.TenLop,

                TenKyThi =
                    entity.TenKyThi,

                NgayThi =
                    entity.NgayThi,

                LoaiKyThi =
                    entity.LoaiKyThi,

                DiemToiDa =
                    entity.DiemToiDa
            };
        }

        // =====================================================
        // VALIDATE LOẠI KỲ THI
        // =====================================================

        private static void ValidateLoaiKyThi(
            string? loaiKyThi)
        {
            if (string.IsNullOrWhiteSpace(loaiKyThi))
                return;

            var validTypes = new[]
            {
                "Giữa kỳ",
                "Cuối kỳ",
                "Kiểm tra",
                "Thi thử"
            };

            if (!validTypes.Contains(
                loaiKyThi.Trim(),
                StringComparer.OrdinalIgnoreCase))
            {
                throw new ArgumentException(
                    "Loại kỳ thi không hợp lệ. " +
                    "Chỉ chấp nhận: Giữa kỳ, Cuối kỳ, " +
                    "Kiểm tra, Thi thử.");
            }
        }

        // =====================================================
        // VALIDATE ĐIỂM
        // =====================================================

        private static void ValidateDiemToiDa(
            decimal? diemToiDa)
        {
            if (diemToiDa.HasValue &&
                diemToiDa.Value <= 0)
            {
                throw new ArgumentException(
                    "Điểm tối đa phải lớn hơn 0.");
            }
        }

        // =====================================================
        // GET ALL
        // =====================================================

        public async Task<IEnumerable<KyThiResponse>>
            GetAllAsync()
        {
            var data =
                await _repository.GetAllAsync();

            return data.Select(MapToResponse);
        }

        // =====================================================
        // GET BY ID
        // =====================================================

        public async Task<KyThiResponse?> GetByIdAsync(
            int id)
        {
            var entity =
                await _repository.GetByIdAsync(id);

            if (entity == null)
                return null;

            return MapToResponse(entity);
        }

        // =====================================================
        // GET BY MA LOP
        // =====================================================

        public async Task<IEnumerable<KyThiResponse>>
            GetByMaLopAsync(int maLop)
        {
            var lopExists =
                await _lopHocRepository
                    .ExistsByIdAsync(maLop);

            if (!lopExists)
            {
                throw new KeyNotFoundException(
                    $"Lớp học có mã {maLop} không tồn tại.");
            }

            var data =
                await _repository
                    .GetByMaLopAsync(maLop);

            return data.Select(MapToResponse);
        }

        // =====================================================
        // GET BY LOAI KY THI
        // =====================================================

        public async Task<IEnumerable<KyThiResponse>>
            GetByLoaiKyThiAsync(
                string loaiKyThi)
        {
            ValidateLoaiKyThi(loaiKyThi);

            var data =
                await _repository
                    .GetByLoaiKyThiAsync(loaiKyThi);

            return data.Select(MapToResponse);
        }

        // =====================================================
        // SEARCH
        // =====================================================

        public async Task<IEnumerable<KyThiResponse>>
            SearchAsync(string? keyword)
        {
            var data =
                await _repository.SearchAsync(keyword);

            return data.Select(MapToResponse);
        }

        // =====================================================
        // PAGED
        // =====================================================

        public async Task<object> GetPagedAsync(
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

        public async Task<object> SearchPagedAsync(
            string? keyword,
            int pageNumber,
            int pageSize)
        {
            ValidatePaging(
                pageNumber,
                pageSize);

            return await _repository
                .SearchPagedAsync(
                    keyword,
                    pageNumber,
                    pageSize);
        }

        // =====================================================
        // CREATE
        // =====================================================

        public async Task<KyThiResponse> CreateAsync(
            CreateKyThiRequest request)
        {
            // Kiểm tra lớp
            var lopExists =
                await _lopHocRepository
                    .ExistsByIdAsync(request.MaLop);

            if (!lopExists)
            {
                throw new KeyNotFoundException(
                    $"Lớp học có mã {request.MaLop} không tồn tại.");
            }

            // Kiểm tra tên
            if (string.IsNullOrWhiteSpace(
                request.TenKyThi))
            {
                throw new ArgumentException(
                    "Tên kỳ thi không được để trống.");
            }

            // Kiểm tra loại
            ValidateLoaiKyThi(
                request.LoaiKyThi);

            // Kiểm tra điểm
            ValidateDiemToiDa(
                request.DiemToiDa);

            // Kiểm tra trùng tên
            var duplicate =
                await _repository
                    .ExistsByTenKyThiAsync(
                        request.MaLop,
                        request.TenKyThi.Trim());

            if (duplicate)
            {
                throw new InvalidOperationException(
                    $"Lớp {request.MaLop} đã tồn tại " +
                    $"kỳ thi có tên '{request.TenKyThi}'.");
            }

            var entity = new KyThi
            {
                MaLop = request.MaLop,

                TenKyThi =
                    request.TenKyThi.Trim(),

                NgayThi =
                    request.NgayThi,

                LoaiKyThi =
                    string.IsNullOrWhiteSpace(
                        request.LoaiKyThi)
                        ? null
                        : request.LoaiKyThi.Trim(),

                DiemToiDa =
                    request.DiemToiDa
            };

            var result =
                await _repository.AddAsync(entity);

            var created =
                await _repository.GetByIdAsync(
                    result.MaKyThi);

            return MapToResponse(created!);
        }

        // =====================================================
        // UPDATE
        // =====================================================

        public async Task<KyThiResponse?> UpdateAsync(
            int id,
            UpdateKyThiRequest request)
        {
            var existing =
                await _repository.GetByIdAsync(id);

            if (existing == null)
            {
                throw new KeyNotFoundException(
                    $"Kỳ thi có mã {id} không tồn tại.");
            }

            // Kiểm tra lớp
            var lopExists =
                await _lopHocRepository
                    .ExistsByIdAsync(request.MaLop);

            if (!lopExists)
            {
                throw new KeyNotFoundException(
                    $"Lớp học có mã {request.MaLop} không tồn tại.");
            }

            // Kiểm tra tên
            if (string.IsNullOrWhiteSpace(
                request.TenKyThi))
            {
                throw new ArgumentException(
                    "Tên kỳ thi không được để trống.");
            }

            // Kiểm tra loại
            ValidateLoaiKyThi(
                request.LoaiKyThi);

            // Kiểm tra điểm
            ValidateDiemToiDa(
                request.DiemToiDa);

            // Kiểm tra trùng
            var duplicate =
                await _repository
                    .ExistsByTenKyThiAsync(
                        request.MaLop,
                        request.TenKyThi.Trim(),
                        id);

            if (duplicate)
            {
                throw new InvalidOperationException(
                    $"Lớp {request.MaLop} đã có " +
                    $"kỳ thi tên '{request.TenKyThi}'.");
            }

            existing.MaLop =
                request.MaLop;

            existing.TenKyThi =
                request.TenKyThi.Trim();

            existing.NgayThi =
                request.NgayThi;

            existing.LoaiKyThi =
                string.IsNullOrWhiteSpace(
                    request.LoaiKyThi)
                    ? null
                    : request.LoaiKyThi.Trim();

            existing.DiemToiDa =
                request.DiemToiDa;

            var result =
                await _repository
                    .UpdateAsync(existing);

            if (result == null)
                return null;

            return MapToResponse(result);
        }

        // =====================================================
        // DELETE
        // =====================================================

        public async Task<bool> DeleteAsync(int id)
        {
            var exists =
                await _repository
                    .ExistsByIdAsync(id);

            if (!exists)
            {
                throw new KeyNotFoundException(
                    $"Kỳ thi có mã {id} không tồn tại.");
            }

            try
            {
                return await _repository
                    .DeleteAsync(id);
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException(
                    "Không thể xóa kỳ thi vì kỳ thi " +
                    "đang được bảng ĐiểmThi tham chiếu. " +
                    "Hãy xóa hoặc xử lý điểm thi trước.",
                    ex);
            }
        }

        // =====================================================
        // PAGING VALIDATE
        // =====================================================

        private static void ValidatePaging(
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