using BLL.Interfaces;
using DAL.Entities;
using DAL.Interfaces;
using DTO.PhongHoc;

namespace BLL.Services
{
    public class PhongHocService : IPhongHocService
    {
        private readonly IPhongHocRepository _repository;

        public PhongHocService(IPhongHocRepository repository)
        {
            _repository = repository;
        }

        // =====================================================
        // 1. LẤY TẤT CẢ PHÒNG HỌC
        // =====================================================

        public async Task<IEnumerable<PhongHocResponse>> GetAllAsync()
        {
            var data = await _repository.GetAllAsync();

            return data.Select(MapToResponse);
        }

        // =====================================================
        // 2. LẤY TẤT CẢ + PHÂN TRANG
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
        // 3. LẤY PHÒNG HỌC THEO ID
        // =====================================================

        public async Task<PhongHocResponse?> GetByIdAsync(int id)
        {
            if (id <= 0)
            {
                throw new Exception("Mã phòng không hợp lệ.");
            }

            var phongHoc = await _repository.GetByIdAsync(id);

            if (phongHoc == null)
            {
                return null;
            }

            return MapToResponse(phongHoc);
        }

        // =====================================================
        // 4. LẤY PHÒNG HỌC THEO TÊN
        // =====================================================

        public async Task<IEnumerable<PhongHocResponse>> GetByNameAsync(
            string tenPhong)
        {
            if (string.IsNullOrWhiteSpace(tenPhong))
            {
                throw new Exception("Tên phòng không được để trống.");
            }

            var data = await _repository.GetByNameAsync(
                tenPhong.Trim());

            return data.Select(MapToResponse);
        }

        // =====================================================
        // 5. LẤY THEO TÊN + PHÂN TRANG
        // =====================================================

        public async Task<object> GetPagedByNameAsync(
            string tenPhong,
            int pageNumber,
            int pageSize)
        {
            if (string.IsNullOrWhiteSpace(tenPhong))
            {
                throw new Exception("Tên phòng không được để trống.");
            }

            ValidatePagination(pageNumber, pageSize);

            return await _repository.GetPagedByNameAsync(
                tenPhong.Trim(),
                pageNumber,
                pageSize);
        }

        // =====================================================
        // 6. LẤY PHÒNG HỌC THEO SỨC CHỨA
        // =====================================================

        public async Task<IEnumerable<PhongHocResponse>> GetBySucChuaAsync(
            int sucChua)
        {
            if (sucChua <= 0)
            {
                throw new Exception(
                    "Sức chứa phải lớn hơn 0.");
            }

            var data = await _repository.GetBySucChuaAsync(
                sucChua);

            return data.Select(MapToResponse);
        }

        // =====================================================
        // 7. LẤY THEO SỨC CHỨA + PHÂN TRANG
        // =====================================================

        public async Task<object> GetPagedBySucChuaAsync(
            int sucChua,
            int pageNumber,
            int pageSize)
        {
            if (sucChua <= 0)
            {
                throw new Exception(
                    "Sức chứa phải lớn hơn 0.");
            }

            ValidatePagination(pageNumber, pageSize);

            return await _repository.GetPagedBySucChuaAsync(
                sucChua,
                pageNumber,
                pageSize);
        }

        // =====================================================
        // 8. LẤY PHÒNG HỌC THEO VỊ TRÍ
        // =====================================================

        public async Task<IEnumerable<PhongHocResponse>> GetByViTriAsync(
            string viTri)
        {
            if (string.IsNullOrWhiteSpace(viTri))
            {
                throw new Exception(
                    "Vị trí không được để trống.");
            }

            var data = await _repository.GetByViTriAsync(
                viTri.Trim());

            return data.Select(MapToResponse);
        }

        // =====================================================
        // 9. LẤY THEO VỊ TRÍ + PHÂN TRANG
        // =====================================================

        public async Task<object> GetPagedByViTriAsync(
            string viTri,
            int pageNumber,
            int pageSize)
        {
            if (string.IsNullOrWhiteSpace(viTri))
            {
                throw new Exception(
                    "Vị trí không được để trống.");
            }

            ValidatePagination(pageNumber, pageSize);

            return await _repository.GetPagedByViTriAsync(
                viTri.Trim(),
                pageNumber,
                pageSize);
        }

        // =====================================================
        // 10. TÌM KIẾM
        // =====================================================

        public async Task<IEnumerable<PhongHocResponse>> SearchAsync(
            string keyword)
        {
            if (string.IsNullOrWhiteSpace(keyword))
            {
                throw new Exception(
                    "Từ khóa tìm kiếm không được để trống.");
            }

            var data = await _repository.SearchAsync(
                keyword.Trim());

            return data.Select(MapToResponse);
        }

        // =====================================================
        // 11. TÌM KIẾM + PHÂN TRANG
        // =====================================================

        public async Task<object> SearchPagedAsync(
            string keyword,
            int pageNumber,
            int pageSize)
        {
            if (string.IsNullOrWhiteSpace(keyword))
            {
                throw new Exception(
                    "Từ khóa tìm kiếm không được để trống.");
            }

            ValidatePagination(pageNumber, pageSize);

            return await _repository.SearchPagedAsync(
                keyword.Trim(),
                pageNumber,
                pageSize);
        }

        // =====================================================
        // 12. KIỂM TRA TÊN PHÒNG
        // =====================================================

        public async Task<bool> ExistsByTenPhongAsync(
            string tenPhong)
        {
            if (string.IsNullOrWhiteSpace(tenPhong))
            {
                throw new Exception(
                    "Tên phòng không được để trống.");
            }

            return await _repository.ExistsByTenPhongAsync(
                tenPhong.Trim());
        }

        // =====================================================
        // 13. TẠO PHÒNG HỌC
        // =====================================================

        public async Task<PhongHocResponse> CreateAsync(
            CreatePhongHocRequest request)
        {
            if (request == null)
            {
                throw new Exception(
                    "Dữ liệu phòng học không được null.");
            }

            if (string.IsNullOrWhiteSpace(request.TenPhong))
            {
                throw new Exception(
                    "Tên phòng không được để trống.");
            }

            if (request.SucChua.HasValue &&
                request.SucChua.Value <= 0)
            {
                throw new Exception(
                    "Sức chứa phải lớn hơn 0.");
            }

            // -----------------------------------------
            // Kiểm tra tên phòng đã tồn tại
            // -----------------------------------------

            var tenPhong = request.TenPhong.Trim();

            var exists = await _repository
                .ExistsByTenPhongAsync(tenPhong);

            if (exists)
            {
                throw new Exception(
                    $"Tên phòng '{tenPhong}' đã tồn tại trong hệ thống.");
            }

            // -----------------------------------------
            // Tạo entity
            // -----------------------------------------

            var phongHoc = new PhongHoc
            {
                TenPhong = tenPhong,
                SucChua = request.SucChua,
                ViTri = string.IsNullOrWhiteSpace(request.ViTri)
                    ? null
                    : request.ViTri.Trim()
            };

            var result = await _repository.AddAsync(
                phongHoc);

            return MapToResponse(result);
        }

        // =====================================================
        // 14. CẬP NHẬT PHÒNG HỌC
        // =====================================================

        public async Task<PhongHocResponse?> UpdateAsync(
            int id,
            UpdatePhongHocRequest request)
        {
            if (id <= 0)
            {
                throw new Exception(
                    "Mã phòng không hợp lệ.");
            }

            if (request == null)
            {
                throw new Exception(
                    "Dữ liệu phòng học không được null.");
            }

            if (string.IsNullOrWhiteSpace(request.TenPhong))
            {
                throw new Exception(
                    "Tên phòng không được để trống.");
            }

            if (request.SucChua.HasValue &&
                request.SucChua.Value <= 0)
            {
                throw new Exception(
                    "Sức chứa phải lớn hơn 0.");
            }

            // -----------------------------------------
            // Kiểm tra phòng có tồn tại không
            // -----------------------------------------

            var phongHoc = await _repository.GetByIdAsync(id);

            if (phongHoc == null)
            {
                throw new Exception(
                    $"Không tìm thấy phòng học có mã {id}.");
            }

            var tenPhong = request.TenPhong.Trim();

            // -----------------------------------------
            // Kiểm tra tên phòng trùng
            // -----------------------------------------

            var phongTrungTen = await _repository
                .GetByNameAsync(tenPhong);

            var trungTenPhongKhac = phongTrungTen.Any(
                x => x.MaPhong != id);

            if (trungTenPhongKhac)
            {
                throw new Exception(
                    $"Tên phòng '{tenPhong}' đã được sử dụng bởi phòng khác.");
            }

            // -----------------------------------------
            // Cập nhật dữ liệu
            // -----------------------------------------

            phongHoc.TenPhong = tenPhong;

            phongHoc.SucChua = request.SucChua;

            phongHoc.ViTri =
                string.IsNullOrWhiteSpace(request.ViTri)
                    ? null
                    : request.ViTri.Trim();

            var result = await _repository.UpdateAsync(
                phongHoc);

            if (result == null)
            {
                return null;
            }

            return MapToResponse(result);
        }

        // =====================================================
        // 15. XÓA PHÒNG HỌC
        // =====================================================

        public async Task<bool> DeleteAsync(int id)
        {
            if (id <= 0)
            {
                throw new Exception(
                    "Mã phòng không hợp lệ.");
            }

            // -----------------------------------------
            // Kiểm tra phòng có tồn tại không
            // -----------------------------------------

            var phongHoc = await _repository.GetByIdAsync(id);

            if (phongHoc == null)
            {
                throw new Exception(
                    $"Không tìm thấy phòng học có mã {id}.");
            }

            // -----------------------------------------
            // Xóa
            // -----------------------------------------

            return await _repository.DeleteAsync(id);
        }

        // =====================================================
        // HÀM KIỂM TRA PHÂN TRANG
        // =====================================================

        private static void ValidatePagination(
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
        // MAP ENTITY -> DTO
        // =====================================================

        private static PhongHocResponse MapToResponse(
            PhongHoc phongHoc)
        {
            return new PhongHocResponse
            {
                MaPhong = phongHoc.MaPhong,
                TenPhong = phongHoc.TenPhong,
                SucChua = phongHoc.SucChua,
                ViTri = phongHoc.ViTri
            };
        }
    }
}