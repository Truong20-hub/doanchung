using BLL.Interfaces;
using DAL.Entities;
using DAL.Interfaces;
using DTO.LichHoc;

namespace BLL.Services
{
    public class LichHocService : ILichHocService
    {
        private readonly ILichHocRepository _repository;
        private readonly ILopHocRepository _lopHocRepository;

        public LichHocService(
            ILichHocRepository repository,
            ILopHocRepository lopHocRepository)
        {
            _repository = repository;
            _lopHocRepository = lopHocRepository;
        }

        // =====================================================
        // MAPPING
        // =====================================================

        private LichHocResponse MapToResponse(
            LichHoc entity)
        {
            return new LichHocResponse
            {
                MaLich = entity.MaLich,

                MaLop = entity.MaLop,

                MaLopCode =
                    entity.MaLopNavigation?.MaLopCode,

                TenLop =
                    entity.MaLopNavigation?.TenLop,

                ThuTrongTuan =
                    entity.ThuTrongTuan,

                GioBatDau =
                    entity.GioBatDau,

                GioKetThuc =
                    entity.GioKetThuc
            };
        }

        // =====================================================
        // VALIDATE
        // =====================================================

        private static void ValidateTime(
            TimeOnly gioBatDau,
            TimeOnly gioKetThuc)
        {
            if (gioBatDau >= gioKetThuc)
            {
                throw new ArgumentException(
                    "Giờ bắt đầu phải nhỏ hơn giờ kết thúc.");
            }
        }

        private static void ValidateThu(
            string thu)
        {
            var validThu = new[]
            {
                "T2",
                "T3",
                "T4",
                "T5",
                "T6",
                "T7",
                "CN"
            };

            if (!validThu.Contains(
                thu.Trim().ToUpper()))
            {
                throw new ArgumentException(
                    "Thứ trong tuần không hợp lệ. " +
                    "Chỉ chấp nhận T2, T3, T4, T5, T6, T7, CN.");
            }
        }

        // =====================================================
        // GET ALL
        // =====================================================

        public async Task<IEnumerable<LichHocResponse>>
            GetAllAsync()
        {
            var data =
                await _repository.GetAllAsync();

            return data.Select(MapToResponse);
        }

        // =====================================================
        // GET BY ID
        // =====================================================

        public async Task<LichHocResponse?> GetByIdAsync(
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

        public async Task<IEnumerable<LichHocResponse>>
            GetByMaLopAsync(int maLop)
        {
            var data =
                await _repository.GetByMaLopAsync(maLop);

            return data.Select(MapToResponse);
        }

        // =====================================================
        // GET BY THU
        // =====================================================

        public async Task<IEnumerable<LichHocResponse>>
            GetByThuAsync(string thuTrongTuan)
        {
            ValidateThu(thuTrongTuan);

            var data =
                await _repository.GetByThuAsync(
                    thuTrongTuan);

            return data.Select(MapToResponse);
        }

        // =====================================================
        // GET BY LOP + THU
        // =====================================================

        public async Task<IEnumerable<LichHocResponse>>
            GetByMaLopAndThuAsync(
                int maLop,
                string thuTrongTuan)
        {
            ValidateThu(thuTrongTuan);

            var data =
                await _repository
                    .GetByMaLopAndThuAsync(
                        maLop,
                        thuTrongTuan);

            return data.Select(MapToResponse);
        }

        // =====================================================
        // CREATE
        // =====================================================

        public async Task<LichHocResponse> CreateAsync(
            CreateLichHocRequest request)
        {
            ValidateThu(request.ThuTrongTuan);

            ValidateTime(
                request.GioBatDau,
                request.GioKetThuc);

            var lopExists =
                await _lopHocRepository
                    .ExistsByIdAsync(request.MaLop);

            if (!lopExists)
            {
                throw new KeyNotFoundException(
                    $"Lớp học có mã {request.MaLop} không tồn tại.");
            }

            var duplicate =
                await _repository.ExistsScheduleAsync(
                    request.MaLop,
                    request.ThuTrongTuan,
                    request.GioBatDau,
                    request.GioKetThuc);

            if (duplicate)
            {
                throw new InvalidOperationException(
                    "Lớp học đã có lịch học bị trùng thời gian.");
            }

            var entity = new LichHoc
            {
                MaLop = request.MaLop,
                ThuTrongTuan =
                    request.ThuTrongTuan.Trim().ToUpper(),
                GioBatDau = request.GioBatDau,
                GioKetThuc = request.GioKetThuc
            };

            var result =
                await _repository.AddAsync(entity);

            var created =
                await _repository.GetByIdAsync(
                    result.MaLich);

            return MapToResponse(created!);
        }

        // =====================================================
        // UPDATE
        // =====================================================

        public async Task<LichHocResponse?> UpdateAsync(
            int id,
            UpdateLichHocRequest request)
        {
            var existing =
                await _repository.GetByIdAsync(id);

            if (existing == null)
            {
                throw new KeyNotFoundException(
                    $"Lịch học có mã {id} không tồn tại.");
            }

            ValidateThu(request.ThuTrongTuan);

            ValidateTime(
                request.GioBatDau,
                request.GioKetThuc);

            var lopExists =
                await _lopHocRepository
                    .ExistsByIdAsync(request.MaLop);

            if (!lopExists)
            {
                throw new KeyNotFoundException(
                    $"Lớp học có mã {request.MaLop} không tồn tại.");
            }

            var duplicate =
                await _repository.ExistsScheduleAsync(
                    request.MaLop,
                    request.ThuTrongTuan,
                    request.GioBatDau,
                    request.GioKetThuc,
                    id);

            if (duplicate)
            {
                throw new InvalidOperationException(
                    "Lịch học mới bị trùng với lịch học khác.");
            }

            existing.MaLop = request.MaLop;

            existing.ThuTrongTuan =
                request.ThuTrongTuan.Trim().ToUpper();

            existing.GioBatDau =
                request.GioBatDau;

            existing.GioKetThuc =
                request.GioKetThuc;

            var result =
                await _repository.UpdateAsync(existing);

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
                await _repository.ExistsByIdAsync(id);

            if (!exists)
            {
                throw new KeyNotFoundException(
                    $"Lịch học có mã {id} không tồn tại.");
            }

            try
            {
                return await _repository.DeleteAsync(id);
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException(
                    "Không thể xóa lịch học vì dữ liệu " +
                    "đang được bảng khác tham chiếu.",
                    ex);
            }
        }

        // =====================================================
        // PAGED
        // =====================================================

        public async Task<object> GetPagedAsync(
            int pageNumber,
            int pageSize)
        {
            if (pageNumber <= 0)
                throw new ArgumentException(
                    "PageNumber phải lớn hơn 0.");

            if (pageSize <= 0)
                throw new ArgumentException(
                    "PageSize phải lớn hơn 0.");

            return await _repository.GetPagedAsync(
                pageNumber,
                pageSize);
        }

        // =====================================================
        // SEARCH
        // =====================================================

        public async Task<IEnumerable<LichHocResponse>>
            SearchAsync(string? keyword)
        {
            var data =
                await _repository.SearchAsync(keyword);

            return data.Select(MapToResponse);
        }

        // =====================================================
        // SEARCH + PAGED
        // =====================================================

        public async Task<object> SearchPagedAsync(
            string? keyword,
            int pageNumber,
            int pageSize)
        {
            if (pageNumber <= 0)
                throw new ArgumentException(
                    "PageNumber phải lớn hơn 0.");

            if (pageSize <= 0)
                throw new ArgumentException(
                    "PageSize phải lớn hơn 0.");

            return await _repository.SearchPagedAsync(
                keyword,
                pageNumber,
                pageSize);
        }
    }
}