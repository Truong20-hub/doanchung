using BLL.Interfaces;
using DAL.Entities;
using DAL.Interfaces;
using DAL.Repositories;
using DTO.BuoiHoc;
using Microsoft.EntityFrameworkCore;

namespace BLL.Services
{
    public class BuoiHocService : IBuoiHocService
    {
        private readonly IBuoiHocRepository _repository;
        private readonly ILopHocRepository lopHocRepository;

        public BuoiHocService(
            IBuoiHocRepository repository)
        {
            _repository = repository;
        }

        // =====================================================
        // MAPPING
        // =====================================================

        private BuoiHocResponse MapToResponse(BuoiHoc entity)
        {
            return new BuoiHocResponse
            {
                MaBuoi = entity.MaBuoi,
                MaLop = entity.MaLop,

                MaLopCode = entity.MaLopNavigation?.MaLopCode,

                TenLop = entity.MaLopNavigation?.TenLop,

                NgayHoc = entity.NgayHoc,
                GioBatDau = entity.GioBatDau,
                GioKetThuc = entity.GioKetThuc,
                NoiDung = entity.NoiDung,
                BiHuy = entity.BiHuy
            };
        }

        // =====================================================
        // 1. LẤY TẤT CẢ
        // =====================================================

        public async Task<IEnumerable<BuoiHocResponse>> GetAllAsync()
        {
            var data = await _repository.GetAllAsync();

            return data.Select(MapToResponse);
        }

        // =====================================================
        // 2. LẤY THEO ID
        // =====================================================

        public async Task<BuoiHocResponse?> GetByIdAsync(int id)
        {
            var entity = await _repository.GetByIdAsync(id);

            if (entity == null)
                return null;

            return MapToResponse(entity);
        }

        // =====================================================
        // 3. KIỂM TRA BUỔI HỌC
        // =====================================================

        public async Task<bool> ExistsByIdAsync(int id)
        {
            return await _repository.ExistsByIdAsync(id);
        }

        // =====================================================
        // 4. KIỂM TRA LỚP
        // =====================================================

        public async Task<bool> ExistsByLopIdAsync(int maLop)
        {
            return await _repository.ExistsByLopIdAsync(maLop);
        }

        // =====================================================
        // 5. THEO MÃ LỚP
        // =====================================================

        public async Task<IEnumerable<BuoiHocResponse>>
            GetByLopIdAsync(int maLop)
        {
            if (!await _repository.ExistsByLopIdAsync(maLop))
            {
                throw new Exception(
                    $"Lớp học có mã {maLop} không tồn tại.");
            }

            var data =
                await _repository.GetByLopIdAsync(maLop);

            return data.Select(MapToResponse);
        }

        // =====================================================
        // 6. THEO NGÀY
        // =====================================================

        public async Task<IEnumerable<BuoiHocResponse>>
            GetByDateAsync(DateOnly ngayHoc)
        {
            var data =
                await _repository.GetByDateAsync(ngayHoc);

            return data.Select(MapToResponse);
        }

        // =====================================================
        // 7. THEO TRẠNG THÁI HỦY
        // =====================================================

        public async Task<IEnumerable<BuoiHocResponse>>
            GetByBiHuyAsync(bool biHuy)
        {
            var data =
                await _repository.GetByBiHuyAsync(biHuy);

            return data.Select(MapToResponse);
        }

        // =====================================================
        // 8. THEO NỘI DUNG
        // =====================================================

        public async Task<IEnumerable<BuoiHocResponse>>
            GetByNoiDungAsync(string noiDung)
        {
            if (string.IsNullOrWhiteSpace(noiDung))
            {
                throw new Exception(
                    "Nội dung tìm kiếm không được để trống.");
            }

            var data =
                await _repository.GetByNoiDungAsync(noiDung);

            return data.Select(MapToResponse);
        }

        // =====================================================
        // 9. TÌM KIẾM
        // =====================================================

        public async Task<IEnumerable<BuoiHocResponse>>
            SearchAsync(
                int? maLop,
                string? noiDung,
                DateOnly? ngayHoc,
                bool? biHuy)
        {
            if (maLop.HasValue)
            {
                if (!await _repository
                    .ExistsByLopIdAsync(maLop.Value))
                {
                    throw new Exception(
                        $"Lớp học có mã {maLop} không tồn tại.");
                }
            }

            var data =
                await _repository.SearchAsync(
                    maLop,
                    noiDung,
                    ngayHoc,
                    biHuy);

            return data.Select(MapToResponse);
        }

        // =====================================================
        // 10. PHÂN TRANG
        // =====================================================

        public async Task<object> GetPagedAsync(
            int pageNumber,
            int pageSize)
        {
            ValidatePagination(pageNumber, pageSize);

            return await _repository.GetPagedAsync(
                pageNumber,
                pageSize);
        }

        // =====================================================
        // 11. THEO LỚP + PHÂN TRANG
        // =====================================================

        public async Task<object> GetPagedByLopIdAsync(
            int maLop,
            int pageNumber,
            int pageSize)
        {
            ValidatePagination(pageNumber, pageSize);

            if (!await _repository.ExistsByLopIdAsync(maLop))
            {
                throw new Exception(
                    $"Lớp học có mã {maLop} không tồn tại.");
            }

            return await _repository.GetPagedByLopIdAsync(
                maLop,
                pageNumber,
                pageSize);
        }

        // =====================================================
        // 12. THEO NGÀY + PHÂN TRANG
        // =====================================================

        public async Task<object> GetPagedByDateAsync(
            DateOnly ngayHoc,
            int pageNumber,
            int pageSize)
        {
            ValidatePagination(pageNumber, pageSize);

            return await _repository.GetPagedByDateAsync(
                ngayHoc,
                pageNumber,
                pageSize);
        }

        // =====================================================
        // 13. TÌM KIẾM + PHÂN TRANG
        // =====================================================

        public async Task<object> SearchPagedAsync(
            int? maLop,
            string? noiDung,
            DateOnly? ngayHoc,
            bool? biHuy,
            int pageNumber,
            int pageSize)
        {
            ValidatePagination(pageNumber, pageSize);

            if (maLop.HasValue)
            {
                if (!await _repository
                    .ExistsByLopIdAsync(maLop.Value))
                {
                    throw new Exception(
                        $"Lớp học có mã {maLop} không tồn tại.");
                }
            }

            return await _repository.SearchPagedAsync(
                maLop,
                noiDung,
                ngayHoc,
                biHuy,
                pageNumber,
                pageSize);
        }

        // =====================================================
        // 14. TẠO BUỔI HỌC
        // =====================================================

        public async Task<BuoiHocResponse> CreateAsync(
            CreateBuoiHocRequest request)
        {
            // ---------------------------------------------
            // Kiểm tra lớp tồn tại
            // ---------------------------------------------

            if (!await _repository
                .ExistsByLopIdAsync(request.MaLop))
            {
                throw new Exception(
                    $"Lớp học có mã {request.MaLop} không tồn tại.");
            }

            // ---------------------------------------------
            // Kiểm tra giờ học
            // ---------------------------------------------

            ValidateTime(
                request.GioBatDau,
                request.GioKetThuc);

            // ---------------------------------------------
            // Kiểm tra trùng buổi học
            // ---------------------------------------------

            var existing =
                await _repository.SearchAsync(
                    request.MaLop,
                    null,
                    request.NgayHoc,
                    false);

            if (request.GioBatDau.HasValue)
            {
                var isDuplicate = existing.Any(x =>
                    x.GioBatDau == request.GioBatDau);

                if (isDuplicate)
                {
                    throw new Exception(
                        "Buổi học này đã tồn tại trong lớp.");
                }
            }

            // ---------------------------------------------
            // Mapping DTO → Entity
            // ---------------------------------------------

            var entity = new BuoiHoc
            {
                MaLop = request.MaLop,
                NgayHoc = request.NgayHoc,
                GioBatDau = request.GioBatDau,
                GioKetThuc = request.GioKetThuc,
                NoiDung = request.NoiDung,
                BiHuy = request.BiHuy ?? false
            };

            var result =
                await _repository.AddAsync(entity);

            // Lấy lại entity có navigation LopHoc
            var created =
                await _repository.GetByIdAsync(
                    result.MaBuoi);

            if (created == null)
            {
                throw new Exception(
                    "Không thể lấy dữ liệu buổi học vừa tạo.");
            }

            return MapToResponse(created);
        }

        // =====================================================
        // 15. CẬP NHẬT
        // =====================================================

        // =====================================================
        // 15. CẬP NHẬT
        // =====================================================
        public async Task<BuoiHocResponse?> UpdateAsync(int id, UpdateBuoiHocRequest request)
        {
            // 1. Kiểm tra buổi học
            var existing = await _repository.GetByIdAsync(id);
            if (existing == null)
            {
                throw new Exception($"Buổi học có mã {id} không tồn tại.");
            }

            // 2. Kiểm tra lớp
            if (!await _repository.ExistsByLopIdAsync(request.MaLop))
            {
                throw new Exception($"Lớp học có mã {request.MaLop} không tồn tại.");
            }

            // 3. Validate giờ
            ValidateTime(request.GioBatDau, request.GioKetThuc);

            // 4. Kiểm tra trùng lịch (truyền null vào biHuy để tìm cả các buổi đã hủy)
            var sameDate = await _repository.SearchAsync(
                request.MaLop,
                null,
                request.NgayHoc,
                null);

            if (request.GioBatDau.HasValue)
            {
                var isDuplicate = sameDate.Any(x =>
                    x.MaBuoi != id &&
                    x.GioBatDau.HasValue &&
                    x.GioBatDau.Value.Hour == request.GioBatDau.Value.Hour &&
                    x.GioBatDau.Value.Minute == request.GioBatDau.Value.Minute);

                if (isDuplicate)
                {
                    throw new Exception("Buổi học này đã tồn tại trong lớp vào cùng khung giờ.");
                }
            }

            // 5. Tạo một đối tượng MỚI để truyền sang Repository, tránh lỗi Tracking của EF Core
            var entityToUpdate = new BuoiHoc
            {
                MaBuoi = id,
                MaLop = request.MaLop,
                NgayHoc = request.NgayHoc,
                GioBatDau = request.GioBatDau,
                GioKetThuc = request.GioKetThuc,
                NoiDung = request.NoiDung,
                BiHuy = request.BiHuy ?? false
            };

            var result = await _repository.UpdateAsync(entityToUpdate);

            if (result == null) return null;

            // 6. Lấy lại để hiển thị đầy đủ Navigation (Tên Lớp, Mã Lớp Code)
            var updated = await _repository.GetByIdAsync(id);

            return MapToResponse(updated!);
        }

        // =====================================================
        // 16. XÓA
        // =====================================================

        public async Task<bool> DeleteAsync(int id)
        {
            var entity =
                await _repository.GetByIdAsync(id);

            if (entity == null)
            {
                throw new Exception(
                    $"Buổi học có mã {id} không tồn tại.");
            }

            // ---------------------------------------------
            // KIỂM TRA ĐIỂM DANH
            // ---------------------------------------------

            if (entity.DiemDanhs != null &&
                entity.DiemDanhs.Any())
            {
                throw new Exception(
                    "Không thể xóa buổi học vì buổi học " +
                    "đã có dữ liệu điểm danh.");
            }

            try
            {
                return await _repository.DeleteAsync(id);
            }
            catch (DbUpdateException)
            {
                throw new Exception(
                    "Không thể xóa buổi học vì dữ liệu " +
                    "đang được bảng khác tham chiếu.");
            }
        }

        // =====================================================
        // VALIDATE PHÂN TRANG
        // =====================================================

        private void ValidatePagination(
            int pageNumber,
            int pageSize)
        {
            if (pageNumber <= 0)
            {
                throw new Exception(
                    "PageNumber phải lớn hơn 0.");
            }

            if (pageSize <= 0)
            {
                throw new Exception(
                    "PageSize phải lớn hơn 0.");
            }

            if (pageSize > 100)
            {
                throw new Exception(
                    "PageSize không được lớn hơn 100.");
            }
        }

        // =====================================================
        // VALIDATE GIỜ
        // =====================================================

        private void ValidateTime(
            TimeOnly? gioBatDau,
            TimeOnly? gioKetThuc)
        {
            // Nếu không nhập cả hai thì cho phép
            if (!gioBatDau.HasValue &&
                !gioKetThuc.HasValue)
            {
                return;
            }

            // Một cái có, một cái không
            if (!gioBatDau.HasValue ||
                !gioKetThuc.HasValue)
            {
                throw new Exception(
                    "Phải nhập cả giờ bắt đầu và giờ kết thúc.");
            }

            // Giờ bắt đầu >= giờ kết thúc
            if (gioBatDau.Value >= gioKetThuc.Value)
            {
                throw new Exception(
                    "Giờ bắt đầu phải nhỏ hơn giờ kết thúc.");
            }
        }
    }
}