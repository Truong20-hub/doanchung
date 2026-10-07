using BLL.Interfaces;
using BLL.Helpers;
using DAL.Entities;
using DAL.Interfaces;
using DTO.HocVien;

namespace BLL.Services
{
    public class HocVienService : IHocVienService
    {
        private readonly IHocVienRepository _hocVienRepository;
        private readonly INguoiDungRepository _nguoiDungRepository;
        private readonly IVaiTroRepository _vaiTroRepository;

        public HocVienService(
            IHocVienRepository hocVienRepository,
            INguoiDungRepository nguoiDungRepository,
            IVaiTroRepository vaiTroRepository)
        {
            _hocVienRepository = hocVienRepository;
            _nguoiDungRepository = nguoiDungRepository;
            _vaiTroRepository = vaiTroRepository;
        }

        // =========================================================
        // CRUD
        // =========================================================

        // Lấy tất cả học viên
        public async Task<IEnumerable<HocVienResponse>> GetAllAsync()
        {
            var hocViens = await _hocVienRepository.GetAllAsync();

            return hocViens.Select(MapToResponse);
        }

        // Lấy học viên theo ID
        public async Task<HocVienResponse?> GetByIdAsync(int id)
        {
            var hocVien = await _hocVienRepository.GetByIdAsync(id);

            if (hocVien == null)
                return null;

            return MapToResponse(hocVien);
        }

        public async Task<IEnumerable<PhuHuynhLienHeResponse>>
            GetParentsForStudentAsync(int maHocVien)
        {
            if (!await _hocVienRepository.ExistsByIdAsync(maHocVien))
            {
                throw new KeyNotFoundException(
                    $"Không tìm thấy học viên có mã {maHocVien}.");
            }

            var parents = await _hocVienRepository
                .GetParentsByHocVienIdAsync(maHocVien);
            return parents.Select(parent => new PhuHuynhLienHeResponse
            {
                MaNguoiDung = parent.MaNguoiDung,
                HoTen = parent.MaNguoiDungNavigation.HoTen
            });
        }

        // =========================================================
        // CREATE
        // Tạo NguoiDung + HocVien
        // =========================================================

        public async Task<HocVienResponse> CreateAsync(
            CreateHocVienRequest request)
        {
            if (request == null)
                throw new ArgumentNullException(nameof(request));

            var nguoiDung =
                await _nguoiDungRepository.GetByIdAsync(request.MaNguoiDung);

            if (nguoiDung == null)
                throw new ArgumentException("Mã người dùng không tồn tại.");

            if (!string.IsNullOrWhiteSpace(nguoiDung.Email))
                ValidationHelper.CheckEmail(nguoiDung.Email);

            if (!string.IsNullOrWhiteSpace(request.SdtPhuHuynh))
                ValidationHelper.CheckPhone(request.SdtPhuHuynh);

            ValidationHelper.CheckBirthDate(request.NgaySinh);

            // -----------------------------------------------------
            // 5. Kiểm tra người dùng đã là học viên chưa
            // -----------------------------------------------------

            var daLaHocVien =
                await _hocVienRepository
                    .ExistsByNguoiDungIdAsync(
                        request.MaNguoiDung);

            if (daLaHocVien)
            {
                throw new Exception(
                    "Tài khoản này đã được liên kết với học viên.");
            }

            // -----------------------------------------------------
            // 6. Tạo học viên
            // -----------------------------------------------------

            var hocVien = new HocVien
            {
                MaNguoiDung = request.MaNguoiDung,

                GioiTinh = request.GioiTinh,

                NgaySinh = request.NgaySinh,

                DiaChi = request.DiaChi,

                TenPhuHuynh = request.TenPhuHuynh,

                SdtPhuHuynh = request.SdtPhuHuynh,

                NgayNhapHoc = request.NgayNhapHoc,
            };

            var hocVienMoi =
                await _hocVienRepository.AddAsync(hocVien);

            // Lưu học viên
            await _hocVienRepository.SaveChangesAsync();

            var hocVienDaTao =
                await _hocVienRepository.GetByIdAsync(
                    hocVienMoi.MaHocVien);

            return MapToResponse(hocVienDaTao!);
        }

        // =========================================================
        // UPDATE
        // =========================================================

        public async Task<HocVienResponse?> UpdateAsync(
            int id,
            UpdateHocVienRequest request)
        {
            var hocVien =
                await _hocVienRepository.GetByIdAsync(id);

            if (hocVien == null)
                throw new ArgumentException("Học viên không tồn tại.");

            if (!string.IsNullOrWhiteSpace(request.SdtPhuHuynh))
                ValidationHelper.CheckPhone(request.SdtPhuHuynh);

            ValidationHelper.CheckBirthDate(request.NgaySinh);


            // -----------------------------------------------------
            // Tạo entity cập nhật
            // -----------------------------------------------------

            var entity = new HocVien
            {
                GioiTinh = request.GioiTinh,

                NgaySinh = request.NgaySinh,

                DiaChi = request.DiaChi,

                TenPhuHuynh = request.TenPhuHuynh,

                SdtPhuHuynh = request.SdtPhuHuynh,

                NgayNhapHoc = request.NgayNhapHoc,
            };

            var updated =
                await _hocVienRepository.UpdateAsync(
                    id,
                    entity);

            if (updated == null)
                return null;

            await _hocVienRepository.SaveChangesAsync();

            return MapToResponse(updated);
        }

        // =========================================================
        // DELETE
        // =========================================================

        public async Task<bool> DeleteAsync(int id)
        {
            // kiểm tra học viên có là khóa ngoại của bảng nào ko
            var isForeignKey =
                await _hocVienRepository.IsForeignKeyAsync(id);
            if (isForeignKey)
                throw new Exception(
                    "Không thể xóa học viên này vì nó đang được tham chiếu bởi các bản ghi khác.");
            var hocVien =
                await _hocVienRepository.GetByIdAsync(id);

            if (hocVien == null)
                return false;

            var result =
                await _hocVienRepository.DeleteAsync(id);

            if (!result)
                return false;
            try
            {
                await _hocVienRepository.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                throw new Exception(
                    "Lỗi khi lưu thay đổi vào cơ sở dữ liệu: " + ex.Message);
            }


            return true;
        }

        // =========================================================
        // LẤY THEO MÃ NGƯỜI DÙNG
        // =========================================================

        public async Task<IEnumerable<HocVienResponse>>
            GetByNguoiDungIdAsync(int maNguoiDung)
        {
            // Kiểm tra người dùng có tồn tại hay không
            bool nguoiDungExists =
                await _nguoiDungRepository
                    .ExistsByIdAsync(maNguoiDung);
            if (!nguoiDungExists)
                throw new Exception("Người dùng không tồn tại.");
            var hocViens =
                await _hocVienRepository
                    .GetByNguoiDungIdAsync(maNguoiDung);

            return hocViens.Select(MapToResponse);
        }

        // =========================================================
        // LẤY THEO TÊN
        // =========================================================

        public async Task<IEnumerable<HocVienResponse>>
            GetByNameAsync(string name)
        {
            var hocViens =
                await _hocVienRepository
                    .GetByNameAsync(name);

            return hocViens.Select(MapToResponse);
        }

        // =========================================================
        // LẤY THEO TÊN PHỤ HUYNH
        // =========================================================

        public async Task<IEnumerable<HocVienResponse>>
            GetByParentNameAsync(string parentName)
        {
            var hocViens =
                await _hocVienRepository
                    .GetByParentNameAsync(parentName);

            return hocViens.Select(MapToResponse);
        }

        // =========================================================
        // LẤY THEO ĐỊA CHỈ
        // =========================================================

        public async Task<IEnumerable<HocVienResponse>>
            GetByAddressAsync(string address)
        {
            var hocViens =
                await _hocVienRepository
                    .GetByAddressAsync(address);

            return hocViens.Select(MapToResponse);
        }

        // =========================================================
        // LẤY THEO SỐ ĐIỆN THOẠI
        // =========================================================

        public async Task<IEnumerable<HocVienResponse>>
            GetByPhoneAsync(string phone)
        {
            var hocViens =
                await _hocVienRepository
                    .GetByPhoneAsync(phone);

            return hocViens.Select(MapToResponse);
        }

        // =========================================================
        // LẤY THEO EMAIL
        // =========================================================

        public async Task<IEnumerable<HocVienResponse>>
            GetByEmailAsync(string email)
        {
            var hocViens =
                await _hocVienRepository
                    .GetByEmailAsync(email);

            return hocViens.Select(MapToResponse);
        }

        // =========================================================
        // TÌM KIẾM
        // =========================================================

        public async Task<IEnumerable<HocVienResponse>>
            SearchAsync(string keyword)
        {
            var hocViens =
                await _hocVienRepository
                    .SearchAsync(keyword);

            return hocViens.Select(MapToResponse);
        }

        // =========================================================
        // PHÂN TRANG
        // =========================================================

        public async Task<object> GetPagedAsync(
            int pageNumber,
            int pageSize)
        {
            // Giá trị mặc định
            if (pageNumber < 1)
                pageNumber = 1;

            if (pageSize < 1)
                pageSize = 10;

            // Giới hạn pageSize
            if (pageSize > 100)
                pageSize = 100;

            var result =
                await _hocVienRepository
                    .GetPagedAsync(
                        pageNumber,
                        pageSize);

            var totalPages =
                (int)Math.Ceiling(
                    result.TotalCount /
                    (double)pageSize);

            return new
            {
                PageNumber = pageNumber,

                PageSize = pageSize,

                TotalCount = result.TotalCount,

                TotalPages = totalPages,

                Data = result.Data
                    .Select(MapToResponse)
                    .ToList()
            };
        }

        // =========================================================
        // TÊN + PHÂN TRANG
        // =========================================================

        public async Task<object>
            GetByNamePagedAsync(
                string name,
                int pageNumber,
                int pageSize)
        {
            if (pageNumber < 1)
                pageNumber = 1;

            if (pageSize < 1)
                pageSize = 10;

            if (pageSize > 100)
                pageSize = 100;

            var result =
                await _hocVienRepository
                    .GetByNamePagedAsync(
                        name,
                        pageNumber,
                        pageSize);

            return CreatePagedResult(
                result.Data,
                result.TotalCount,
                pageNumber,
                pageSize);
        }

        // =========================================================
        // TÊN PHỤ HUYNH + PHÂN TRANG
        // =========================================================

        public async Task<object>
            GetByParentNamePagedAsync(
                string parentName,
                int pageNumber,
                int pageSize)
        {
            if (pageNumber < 1)
                pageNumber = 1;

            if (pageSize < 1)
                pageSize = 10;

            if (pageSize > 100)
                pageSize = 100;

            var result =
                await _hocVienRepository
                    .GetByParentNamePagedAsync(
                        parentName,
                        pageNumber,
                        pageSize);

            return CreatePagedResult(
                result.Data,
                result.TotalCount,
                pageNumber,
                pageSize);
        }

        // =========================================================
        // ĐỊA CHỈ + PHÂN TRANG
        // =========================================================

        public async Task<object>
            GetByAddressPagedAsync(
                string address,
                int pageNumber,
                int pageSize)
        {
            if (pageNumber < 1)
                pageNumber = 1;

            if (pageSize < 1)
                pageSize = 10;

            if (pageSize > 100)
                pageSize = 100;

            var result =
                await _hocVienRepository
                    .GetByAddressPagedAsync(
                        address,
                        pageNumber,
                        pageSize);

            return CreatePagedResult(
                result.Data,
                result.TotalCount,
                pageNumber,
                pageSize);
        }

        // =========================================================
        // SỐ ĐIỆN THOẠI + PHÂN TRANG
        // =========================================================

        public async Task<object>
            GetByPhonePagedAsync(
                string phone,
                int pageNumber,
                int pageSize)
        {
            if (pageNumber < 1)
                pageNumber = 1;

            if (pageSize < 1)
                pageSize = 10;

            if (pageSize > 100)
                pageSize = 100;

            var result =
                await _hocVienRepository
                    .GetByPhonePagedAsync(
                        phone,
                        pageNumber,
                        pageSize);

            return CreatePagedResult(
                result.Data,
                result.TotalCount,
                pageNumber,
                pageSize);
        }

        // =========================================================
        // EMAIL + PHÂN TRANG
        // =========================================================

        public async Task<object>
            GetByEmailPagedAsync(
                string email,
                int pageNumber,
                int pageSize)
        {
            if (pageNumber < 1)
                pageNumber = 1;

            if (pageSize < 1)
                pageSize = 10;

            if (pageSize > 100)
                pageSize = 100;

            var result =
                await _hocVienRepository
                    .GetByEmailPagedAsync(
                        email,
                        pageNumber,
                        pageSize);

            return CreatePagedResult(
                result.Data,
                result.TotalCount,
                pageNumber,
                pageSize);
        }

        // =========================================================
        // SEARCH + PHÂN TRANG
        // =========================================================

        public async Task<object> SearchPagedAsync(
            string keyword,
            int pageNumber,
            int pageSize)
        {
            if (pageNumber < 1)
                pageNumber = 1;

            if (pageSize < 1)
                pageSize = 10;

            if (pageSize > 100)
                pageSize = 100;

            var result =
                await _hocVienRepository
                    .SearchPagedAsync(
                        keyword,
                        pageNumber,
                        pageSize);

            return CreatePagedResult(
                result.Data,
                result.TotalCount,
                pageNumber,
                pageSize);
        }

        // =========================================================
        // HÀM TẠO KẾT QUẢ PHÂN TRANG
        // =========================================================

        private static object CreatePagedResult(
            IEnumerable<HocVien> data,
            int totalCount,
            int pageNumber,
            int pageSize)
        {
            var totalPages =
                (int)Math.Ceiling(
                    totalCount /
                    (double)pageSize);

            return new
            {
                PageNumber = pageNumber,

                PageSize = pageSize,

                TotalCount = totalCount,

                TotalPages = totalPages,

                Data = data
                    .Select(MapToResponse)
                    .ToList()
            };
        }

        // =========================================================
        // MAPPING
        // =========================================================

        private static HocVienResponse MapToResponse(HocVien hocVien)
        {
            return new HocVienResponse
            {
                MaHocVien = hocVien.MaHocVien,

                MaNguoiDung = hocVien.MaNguoiDung,

                // Thông tin người dùng
                HoTen = hocVien.MaNguoiDungNavigation?.HoTen ?? string.Empty,

                Email = hocVien.MaNguoiDungNavigation?.Email,

                SoDienThoai = hocVien.MaNguoiDungNavigation?.SoDienThoai,

                DangHoatDong = hocVien.MaNguoiDungNavigation?.DangHoatDong ?? true,

                TenDangNhap = hocVien.MaNguoiDungNavigation?.TenDangNhap ?? string.Empty,

                AvatarUrl = hocVien.MaNguoiDungNavigation?.AvatarUrl,

                // Thông tin học viên
                GioiTinh = hocVien.GioiTinh,

                NgaySinh = hocVien.NgaySinh,

                DiaChi = hocVien.DiaChi,

                TenPhuHuynh = hocVien.TenPhuHuynh,

                SdtPhuHuynh = hocVien.SdtPhuHuynh,

                NgayNhapHoc = hocVien.NgayNhapHoc
            };
        }
    }
}