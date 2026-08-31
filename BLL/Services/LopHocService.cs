using BLL.Interfaces;
using DAL.Entities;
using DAL.Interfaces;
using DTO;
using DTO.LopHoc;
using Microsoft.EntityFrameworkCore;

namespace BLL.Services
{
    public class LopHocService : ILopHocService
    {
        private readonly ILopHocRepository _repository;

        public LopHocService(ILopHocRepository repository)
        {
            _repository = repository;
        }


        // =====================================================
        // MAPPING
        // LopHoc -> LopHocResponse
        // =====================================================

        private LopHocResponse MapToResponse(LopHoc lopHoc)
        {
            return new LopHocResponse
            {
                MaLop = lopHoc.MaLop,

                MaLopCode = lopHoc.MaLopCode,

                TenLop = lopHoc.TenLop,

                // Khóa học
                MaKhoaHoc = lopHoc.MaKhoaHoc,

                TenKhoaHoc =
                    lopHoc.MaKhoaHocNavigation?.TenKhoaHoc,

                // Giáo viên
                MaGiaoVien = lopHoc.MaGiaoVien,

                TenGiaoVien =
                    lopHoc.MaGiaoVienNavigation?.HoTen,

                // Phòng học
                MaPhong = lopHoc.MaPhong,

                TenPhong =
                    lopHoc.MaPhongNavigation?.TenPhong,

                // Thông tin khác
                AvatarUrl = lopHoc.AvatarUrl,

                NgayBatDau = lopHoc.NgayBatDau,

                NgayKetThuc = lopHoc.NgayKetThuc,

                SiSoToiDa = lopHoc.SiSoToiDa,

                TrangThai = lopHoc.TrangThai
            };
        }


        // =====================================================
        // MAPPING DANH SÁCH
        // =====================================================

        private IEnumerable<LopHocResponse> MapToResponse(
            IEnumerable<LopHoc> lopHocs)
        {
            return lopHocs.Select(MapToResponse);
        }


        // =====================================================
        // VALIDATE PAGE
        // =====================================================

        private void ValidatePagination(
            int pageNumber,
            int pageSize)
        {
            if (pageNumber <= 0)
            {
                throw new Exception(
                    "Số trang phải lớn hơn 0.");
            }

            if (pageSize <= 0)
            {
                throw new Exception(
                    "Kích thước trang phải lớn hơn 0.");
            }

            if (pageSize > 100)
            {
                throw new Exception(
                    "Kích thước trang không được lớn hơn 100.");
            }
        }


        // =====================================================
        // 1. LẤY TẤT CẢ
        // =====================================================

        public async Task<IEnumerable<LopHocResponse>> GetAllAsync()
        {
            var data = await _repository.GetAllAsync();

            return MapToResponse(data);
        }


        // =====================================================
        // 2. LẤY THEO ID
        // =====================================================

        public async Task<LopHocResponse?> GetByIdAsync(int id)
        {
            var lopHoc = await _repository.GetByIdAsync(id);

            if (lopHoc == null)
            {
                return null;
            }

            return MapToResponse(lopHoc);
        }


        // =====================================================
        // 3. CREATE
        // =====================================================

        public async Task<LopHocResponse> CreateAsync(
            CreateLopHocRequest request)
        {
            if (request == null)
            {
                throw new Exception(
                    "Dữ liệu lớp học không được để trống.");
            }


            // -------------------------------------------------
            // Kiểm tra mã lớp
            // -------------------------------------------------

            if (string.IsNullOrWhiteSpace(request.MaLopCode))
            {
                throw new Exception(
                    "Mã lớp không được để trống.");
            }

            var maLopCode = request.MaLopCode.Trim();

            var maLopExists =
                await _repository.ExistsByMaLopCodeAsync(
                    maLopCode);

            if (maLopExists)
            {
                throw new Exception(
                    $"Mã lớp '{maLopCode}' đã tồn tại trong hệ thống.");
            }


            // -------------------------------------------------
            // Kiểm tra tên lớp
            // -------------------------------------------------

            if (string.IsNullOrWhiteSpace(request.TenLop))
            {
                throw new Exception(
                    "Tên lớp không được để trống.");
            }

            var tenLop = request.TenLop.Trim();

            var tenLopExists =
                await _repository.ExistsByTenLopAsync(
                    tenLop);

            if (tenLopExists)
            {
                throw new Exception(
                    $"Tên lớp '{tenLop}' đã tồn tại trong hệ thống.");
            }


            // -------------------------------------------------
            // Kiểm tra khóa học
            // -------------------------------------------------

            var khoaHocExists =
                await _repository.ExistsByKhoaHocIdAsync(
                    request.MaKhoaHoc);

            if (!khoaHocExists)
            {
                throw new Exception(
                    $"Khóa học có mã {request.MaKhoaHoc} không tồn tại.");
            }


            // -------------------------------------------------
            // Kiểm tra giáo viên nếu có
            // -------------------------------------------------

            if (request.MaGiaoVien.HasValue)
            {
                var giaoVienExists =
                    await _repository.ExistsByGiaoVienIdAsync(
                        request.MaGiaoVien.Value);

                if (!giaoVienExists)
                {
                    throw new Exception(
                        $"Giáo viên có mã {request.MaGiaoVien.Value} không tồn tại.");
                }
            }


            // -------------------------------------------------
            // Kiểm tra phòng học nếu có
            // -------------------------------------------------

            if (request.MaPhong.HasValue)
            {
                var phongExists =
                    await _repository.ExistsByPhongHocIdAsync(
                        request.MaPhong.Value);

                if (!phongExists)
                {
                    throw new Exception(
                        $"Phòng học có mã {request.MaPhong.Value} không tồn tại.");
                }
            }


            // -------------------------------------------------
            // Kiểm tra ngày
            // -------------------------------------------------

            if (request.NgayBatDau.HasValue &&
                request.NgayKetThuc.HasValue)
            {
                if (request.NgayKetThuc.Value <
                    request.NgayBatDau.Value)
                {
                    throw new Exception(
                        "Ngày kết thúc không được nhỏ hơn ngày bắt đầu.");
                }
            }


            // -------------------------------------------------
            // Tạo entity
            // -------------------------------------------------

            var lopHoc = new LopHoc
            {
                MaLopCode = maLopCode,

                TenLop = tenLop,

                MaKhoaHoc = request.MaKhoaHoc,

                MaGiaoVien = request.MaGiaoVien,

                MaPhong = request.MaPhong,

                AvatarUrl = request.AvatarUrl,

                NgayBatDau = request.NgayBatDau,

                NgayKetThuc = request.NgayKetThuc,

                SiSoToiDa = request.SiSoToiDa,

                TrangThai = request.TrangThai
            };


            // -------------------------------------------------
            // Thêm
            // -------------------------------------------------

            var result =
                await _repository.AddAsync(lopHoc);


            // Lấy lại entity có Include
            var created =
                await _repository.GetByIdAsync(
                    result.MaLop);

            if (created == null)
            {
                throw new Exception(
                    "Không thể lấy dữ liệu lớp học vừa tạo.");
            }

            return MapToResponse(created);
        }


        // =====================================================
        // 4. UPDATE
        // =====================================================

        public async Task<LopHocResponse?> UpdateAsync(
            int id,
            UpdateLopHocRequest request)
        {
            if (request == null)
            {
                throw new Exception(
                    "Dữ liệu cập nhật không được để trống.");
            }


            // -------------------------------------------------
            // Kiểm tra lớp học
            // -------------------------------------------------

            var existing =
                await _repository.GetByIdAsync(id);

            if (existing == null)
            {
                throw new Exception(
                    $"Lớp học có mã {id} không tồn tại.");
            }


            // -------------------------------------------------
            // Kiểm tra mã lớp
            // -------------------------------------------------

            if (string.IsNullOrWhiteSpace(request.MaLopCode))
            {
                throw new Exception(
                    "Mã lớp không được để trống.");
            }

            var maLopCode =
                request.MaLopCode.Trim();


            // Chỉ kiểm tra trùng nếu mã mới khác mã cũ
            if (!string.Equals(
                existing.MaLopCode,
                maLopCode,
                StringComparison.OrdinalIgnoreCase))
            {
                var maLopExists =
                    await _repository.ExistsByMaLopCodeAsync(
                        maLopCode);

                if (maLopExists)
                {
                    throw new Exception(
                        $"Mã lớp '{maLopCode}' đã tồn tại trong hệ thống.");
                }
            }


            // -------------------------------------------------
            // Kiểm tra tên lớp
            // -------------------------------------------------

            if (string.IsNullOrWhiteSpace(request.TenLop))
            {
                throw new Exception(
                    "Tên lớp không được để trống.");
            }

            var tenLop = request.TenLop.Trim();


            // Chỉ kiểm tra nếu tên thay đổi
            if (!string.Equals(
                existing.TenLop,
                tenLop,
                StringComparison.OrdinalIgnoreCase))
            {
                var tenLopExists =
                    await _repository.ExistsByTenLopAsync(
                        tenLop);

                if (tenLopExists)
                {
                    throw new Exception(
                        $"Tên lớp '{tenLop}' đã tồn tại trong hệ thống.");
                }
            }


            // -------------------------------------------------
            // Kiểm tra khóa học
            // -------------------------------------------------

            var khoaHocExists =
                await _repository.ExistsByKhoaHocIdAsync(
                    request.MaKhoaHoc);

            if (!khoaHocExists)
            {
                throw new Exception(
                    $"Khóa học có mã {request.MaKhoaHoc} không tồn tại.");
            }


            // -------------------------------------------------
            // Kiểm tra giáo viên
            // -------------------------------------------------

            if (request.MaGiaoVien.HasValue)
            {
                var giaoVienExists =
                    await _repository.ExistsByGiaoVienIdAsync(
                        request.MaGiaoVien.Value);

                if (!giaoVienExists)
                {
                    throw new Exception(
                        $"Giáo viên có mã {request.MaGiaoVien.Value} không tồn tại.");
                }
            }


            // -------------------------------------------------
            // Kiểm tra phòng
            // -------------------------------------------------

            if (request.MaPhong.HasValue)
            {
                var phongExists =
                    await _repository.ExistsByPhongHocIdAsync(
                        request.MaPhong.Value);

                if (!phongExists)
                {
                    throw new Exception(
                        $"Phòng học có mã {request.MaPhong.Value} không tồn tại.");
                }
            }


            // -------------------------------------------------
            // Kiểm tra ngày
            // -------------------------------------------------

            if (request.NgayBatDau.HasValue &&
                request.NgayKetThuc.HasValue)
            {
                if (request.NgayKetThuc.Value <
                    request.NgayBatDau.Value)
                {
                    throw new Exception(
                        "Ngày kết thúc không được nhỏ hơn ngày bắt đầu.");
                }
            }


            // -------------------------------------------------
            // Cập nhật entity
            // -------------------------------------------------

            existing.MaLopCode = maLopCode;

            existing.TenLop = tenLop;

            existing.MaKhoaHoc = request.MaKhoaHoc;

            existing.MaGiaoVien = request.MaGiaoVien;

            existing.MaPhong = request.MaPhong;

            existing.AvatarUrl = request.AvatarUrl;

            existing.NgayBatDau = request.NgayBatDau;

            existing.NgayKetThuc = request.NgayKetThuc;

            existing.SiSoToiDa = request.SiSoToiDa;

            existing.TrangThai = request.TrangThai;


            await _repository.UpdateAsync(existing);


            // Lấy lại dữ liệu có Include
            var updated =
                await _repository.GetByIdAsync(id);

            if (updated == null)
            {
                return null;
            }

            return MapToResponse(updated);
        }


        // =====================================================
        // 5. DELETE
        // =====================================================

        public async Task<bool> DeleteAsync(int id)
        {
            var existing =
                await _repository.GetByIdAsync(id);

            if (existing == null)
            {
                throw new Exception(
                    $"Lớp học có mã {id} không tồn tại.");
            }

            try
            {
                return await _repository.DeleteAsync(id);
            }
            catch (DbUpdateException)
            {
                throw new Exception(
                    "Không thể xóa lớp học vì lớp học này đang được "
                    + "tham chiếu bởi dữ liệu ở bảng khác. "
                    + "Vui lòng kiểm tra BuoiHoc, DangKyHoc, HoaDon, "
                    + "KyThi hoặc LichHoc trước khi xóa.");
            }
            catch (Exception)
            {
                throw new Exception(
                    "Đã xảy ra lỗi khi xóa lớp học.");
            }
        }


        // =====================================================
        // 6. TÌM THEO MÃ LỚP
        // =====================================================

        public async Task<IEnumerable<LopHocResponse>>
            GetByMaLopCodeAsync(string maLopCode)
        {
            if (string.IsNullOrWhiteSpace(maLopCode))
            {
                throw new Exception(
                    "Mã lớp không được để trống.");
            }

            var data =
                await _repository.GetByMaLopCodeAsync(
                    maLopCode.Trim());

            return MapToResponse(data);
        }


        // =====================================================
        // 7. TÌM THEO MÃ LỚP + PHÂN TRANG
        // =====================================================

        public async Task<PagedResult<LopHocResponse>> GetPagedByMaLopCodeAsync(
            string maLopCode,
            int pageNumber,
            int pageSize)
        {
            ValidatePagination(pageNumber, pageSize);

            if (string.IsNullOrWhiteSpace(maLopCode))
            {
                throw new Exception(
                    "Mã lớp không được để trống.");
            }

            var result =
                await _repository.GetPagedByMaLopCodeAsync(
                    maLopCode.Trim(),
                    pageNumber,
                    pageSize);

            return MapPagedResult(result);
        }


        // =====================================================
        // 8. TÌM THEO TÊN LỚP
        // =====================================================

        public async Task<IEnumerable<LopHocResponse>>
            GetByTenLopAsync(string tenLop)
        {
            if (string.IsNullOrWhiteSpace(tenLop))
            {
                throw new Exception(
                    "Tên lớp không được để trống.");
            }

            var data =
                await _repository.GetByTenLopAsync(
                    tenLop.Trim());

            return MapToResponse(data);
        }


        // =====================================================
        // 9. TÌM THEO TÊN LỚP + PHÂN TRANG
        // =====================================================

        public async Task<PagedResult<LopHocResponse>> GetPagedByTenLopAsync(
            string tenLop,
            int pageNumber,
            int pageSize)
        {
            ValidatePagination(pageNumber, pageSize);

            if (string.IsNullOrWhiteSpace(tenLop))
            {
                throw new Exception(
                    "Tên lớp không được để trống.");
            }

            var result =
                await _repository.GetPagedByTenLopAsync(
                    tenLop.Trim(),
                    pageNumber,
                    pageSize);

            return MapPagedResult(result);
        }


        // =====================================================
        // 10. TÌM THEO KHÓA HỌC
        // =====================================================

        public async Task<IEnumerable<LopHocResponse>>
            GetByKhoaHocIdAsync(int maKhoaHoc)
        {
            var exists =
                await _repository.ExistsByKhoaHocIdAsync(
                    maKhoaHoc);

            if (!exists)
            {
                throw new Exception(
                    $"Khóa học có mã {maKhoaHoc} không tồn tại.");
            }

            var data =
                await _repository.GetByKhoaHocIdAsync(
                    maKhoaHoc);

            return MapToResponse(data);
        }


        // =====================================================
        // 11. KHÓA HỌC + PHÂN TRANG
        // =====================================================

        public async Task<PagedResult<LopHocResponse>> GetPagedByKhoaHocIdAsync(
            int maKhoaHoc,
            int pageNumber,
            int pageSize)
        {
            ValidatePagination(pageNumber, pageSize);

            var exists =
                await _repository.ExistsByKhoaHocIdAsync(
                    maKhoaHoc);

            if (!exists)
            {
                throw new Exception(
                    $"Khóa học có mã {maKhoaHoc} không tồn tại.");
            }

            var result =
                await _repository.GetPagedByKhoaHocIdAsync(
                    maKhoaHoc,
                    pageNumber,
                    pageSize);

            return MapPagedResult(result);
        }


        // =====================================================
        // 12. TÌM THEO GIÁO VIÊN
        // =====================================================

        public async Task<IEnumerable<LopHocResponse>>
            GetByGiaoVienIdAsync(int maGiaoVien)
        {
            var exists =
                await _repository.ExistsByGiaoVienIdAsync(
                    maGiaoVien);

            if (!exists)
            {
                throw new Exception(
                    $"Giáo viên có mã {maGiaoVien} không tồn tại.");
            }

            var data =
                await _repository.GetByGiaoVienIdAsync(
                    maGiaoVien);

            return MapToResponse(data);
        }


        // =====================================================
        // 13. GIÁO VIÊN + PHÂN TRANG
        // =====================================================

        public async Task<PagedResult<LopHocResponse>> GetPagedByGiaoVienIdAsync(
            int maGiaoVien,
            int pageNumber,
            int pageSize)
        {
            ValidatePagination(pageNumber, pageSize);

            var exists =
                await _repository.ExistsByGiaoVienIdAsync(
                    maGiaoVien);

            if (!exists)
            {
                throw new Exception(
                    $"Giáo viên có mã {maGiaoVien} không tồn tại.");
            }

            var result =
                await _repository.GetPagedByGiaoVienIdAsync(
                    maGiaoVien,
                    pageNumber,
                    pageSize);

            return MapPagedResult(result);
        }


        // =====================================================
        // 14. TÌM THEO PHÒNG
        // =====================================================

        public async Task<IEnumerable<LopHocResponse>>
            GetByPhongHocIdAsync(int maPhong)
        {
            var exists =
                await _repository.ExistsByPhongHocIdAsync(
                    maPhong);

            if (!exists)
            {
                throw new Exception(
                    $"Phòng học có mã {maPhong} không tồn tại.");
            }

            var data =
                await _repository.GetByPhongHocIdAsync(
                    maPhong);

            return MapToResponse(data);
        }


        // =====================================================
        // 15. PHÒNG + PHÂN TRANG
        // =====================================================

        public async Task<PagedResult<LopHocResponse>> GetPagedByPhongHocIdAsync(
            int maPhong,
            int pageNumber,
            int pageSize)
        {
            ValidatePagination(pageNumber, pageSize);

            var exists =
                await _repository.ExistsByPhongHocIdAsync(
                    maPhong);

            if (!exists)
            {
                throw new Exception(
                    $"Phòng học có mã {maPhong} không tồn tại.");
            }

            var result =
                await _repository.GetPagedByPhongHocIdAsync(
                    maPhong,
                    pageNumber,
                    pageSize);

            return MapPagedResult(result);
        }


        // =====================================================
        // 16. TÌM THEO TRẠNG THÁI
        // =====================================================

        public async Task<IEnumerable<LopHocResponse>>
            GetByTrangThaiAsync(string trangThai)
        {
            if (string.IsNullOrWhiteSpace(trangThai))
            {
                throw new Exception(
                    "Trạng thái không được để trống.");
            }

            var data =
                await _repository.GetByTrangThaiAsync(
                    trangThai.Trim());

            return MapToResponse(data);
        }


        // =====================================================
        // 17. TRẠNG THÁI + PHÂN TRANG
        // =====================================================

        public async Task<PagedResult<LopHocResponse>> GetPagedByTrangThaiAsync(
            string trangThai,
            int pageNumber,
            int pageSize)
        {
            ValidatePagination(pageNumber, pageSize);

            if (string.IsNullOrWhiteSpace(trangThai))
            {
                throw new Exception(
                    "Trạng thái không được để trống.");
            }

            var result =
                await _repository.GetPagedByTrangThaiAsync(
                    trangThai.Trim(),
                    pageNumber,
                    pageSize);

            return MapPagedResult(result);
        }


        // =====================================================
        // 18. TÌM KIẾM
        // =====================================================

        public async Task<IEnumerable<LopHocResponse>>
            SearchAsync(
                string? maLopCode,
                string? tenLop)
        {
            if (string.IsNullOrWhiteSpace(maLopCode) &&
                string.IsNullOrWhiteSpace(tenLop))
            {
                throw new Exception(
                    "Vui lòng nhập mã lớp hoặc tên lớp để tìm kiếm.");
            }

            var data =
                await _repository.SearchAsync(
                    maLopCode?.Trim(),
                    tenLop?.Trim());

            return MapToResponse(data);
        }


        // =====================================================
        // 19. TÌM KIẾM + PHÂN TRANG
        // =====================================================

        public async Task<PagedResult<LopHocResponse>> SearchPagedAsync(
            string? maLopCode,
            string? tenLop,
            int pageNumber,
            int pageSize)
        {
            ValidatePagination(pageNumber, pageSize);

            if (string.IsNullOrWhiteSpace(maLopCode) &&
                string.IsNullOrWhiteSpace(tenLop))
            {
                throw new Exception(
                    "Vui lòng nhập mã lớp hoặc tên lớp để tìm kiếm.");
            }

            var result =
                await _repository.SearchPagedAsync(
                    maLopCode?.Trim(),
                    tenLop?.Trim(),
                    pageNumber,
                    pageSize);

            return MapPagedResult(result);
        }


        // =====================================================
        // 20. LẤY TẤT CẢ + PHÂN TRANG
        // =====================================================

        public async Task<PagedResult<LopHocResponse>> GetPagedAsync(
            int pageNumber,
            int pageSize)
        {
            ValidatePagination(pageNumber, pageSize);

            var result =
                await _repository.GetPagedAsync(
                    pageNumber,
                    pageSize);

            return MapPagedResult(result);
        }


        // =====================================================
        // MAPPING KẾT QUẢ PHÂN TRANG
        // =====================================================

        private PagedResult<LopHocResponse> MapPagedResult(
            PagedResult<LopHoc> result)
        {
            return new PagedResult<LopHocResponse>
            {
                PageNumber = result.PageNumber,

                PageSize = result.PageSize,

                TotalItems = result.TotalItems,

                TotalPages = result.TotalPages,

                Data = MapToResponse(result.Data)
            };
        }
    }
}