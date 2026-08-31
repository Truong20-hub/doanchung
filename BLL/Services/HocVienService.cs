using BLL.Interfaces;
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

        // =========================================================
        // CREATE
        // Tạo NguoiDung + HocVien
        // =========================================================

        public async Task<HocVienResponse> CreateAsync(
            CreateHocVienRequest request)
        {
            // -----------------------------------------------------
            // 1. Kiểm tra tên đăng nhập
            // -----------------------------------------------------

            var nguoiDungs =
                await _nguoiDungRepository.GetAllAsync();

            var usernameExists = nguoiDungs.Any(x =>
                x.TenDangNhap.Equals(
                    request.TenDangNhap,
                    StringComparison.OrdinalIgnoreCase));

            if (usernameExists)
            {
                throw new Exception(
                    "Tên đăng nhập đã tồn tại trong hệ thống.");
            }

            // -----------------------------------------------------
            // 2. Kiểm tra email
            // -----------------------------------------------------

            if (!string.IsNullOrWhiteSpace(request.Email))
            {
                var emailExists = nguoiDungs.Any(x =>
                    x.Email != null &&
                    x.Email.Equals(
                        request.Email,
                        StringComparison.OrdinalIgnoreCase));

                if (emailExists)
                {
                    throw new Exception(
                        "Email đã tồn tại trong hệ thống.");
                }
            }

            // -----------------------------------------------------
            // 3. Kiểm tra vai trò
            // -----------------------------------------------------

            var vaiTro =
                await _vaiTroRepository.GetVaiTroByIdAsync(
                    request.MaVaiTro);

            if (vaiTro == null)
            {
                throw new Exception(
                    "Vai trò không tồn tại.");
            }

            // -----------------------------------------------------
            // 4. Tạo tài khoản người dùng
            // -----------------------------------------------------

            NguoiDung nguoiDung = new NguoiDung
            {
                TenDangNhap = request.TenDangNhap,

                MatKhauHash = request.MatKhau,

                HoTen = request.HoTen,

                Email = request.Email,

                SoDienThoai = request.SoDienThoai,

                MaVaiTro = request.MaVaiTro,

                AvatarUrl = request.AvatarUrl,

                DangHoatDong = request.DangHoatDong,

                NgayTao = DateTime.Now
            };
            await _nguoiDungRepository.AddAsync(nguoiDung);

            // Lưu để lấy MaNguoiDung
            await _nguoiDungRepository.SaveChangesAsync();

            // -----------------------------------------------------
            // 5. Kiểm tra người dùng đã là học viên chưa
            // -----------------------------------------------------

            var daLaHocVien =
                await _hocVienRepository
                    .ExistsByNguoiDungIdAsync(
                        nguoiDung.MaNguoiDung);

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
                MaNguoiDung = nguoiDung.MaNguoiDung,

                HoTen = request.HoTen,

                GioiTinh = request.GioiTinh,

                NgaySinh = request.NgaySinh,

                SoDienThoai = request.SoDienThoai,

                Email = request.Email,

                DiaChi = request.DiaChi,

                TenPhuHuynh = request.TenPhuHuynh,

                SdtPhuHuynh = request.SdtPhuHuynh,

                NgayNhapHoc = request.NgayNhapHoc,

                DangHoatDong = request.DangHoatDong
            };

            var hocVienMoi =
                await _hocVienRepository.AddAsync(hocVien);

            // Lưu học viên
            await _hocVienRepository.SaveChangesAsync();

            // Gắn navigation để trả về thông tin tài khoản
            hocVienMoi.MaNguoiDungNavigation = nguoiDung;

            return MapToResponse(hocVienMoi);
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
                return null;

            // -----------------------------------------------------
            // Kiểm tra email
            // -----------------------------------------------------

            if (!string.IsNullOrWhiteSpace(request.Email))
            {
                var nguoiDungs =
                    await _nguoiDungRepository.GetAllAsync();

                var emailExists = nguoiDungs.Any(x =>
                    x.Email != null &&
                    x.Email.Equals(
                        request.Email,
                        StringComparison.OrdinalIgnoreCase) &&
                    x.MaNguoiDung != hocVien.MaNguoiDung);

                if (emailExists)
                {
                    throw new Exception(
                        "Email đã tồn tại trong hệ thống.");
                }
            }

            // -----------------------------------------------------
            // Tạo entity cập nhật
            // -----------------------------------------------------

            var entity = new HocVien
            {
                HoTen = request.HoTen,

                GioiTinh = request.GioiTinh,

                NgaySinh = request.NgaySinh,

                SoDienThoai = request.SoDienThoai,

                Email = request.Email,

                DiaChi = request.DiaChi,

                TenPhuHuynh = request.TenPhuHuynh,

                SdtPhuHuynh = request.SdtPhuHuynh,

                NgayNhapHoc = request.NgayNhapHoc,

                DangHoatDong = request.DangHoatDong
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
            var hocVien =
                await _hocVienRepository.GetByIdAsync(id);

            if (hocVien == null)
                return false;

            var result =
                await _hocVienRepository.DeleteAsync(id);

            if (!result)
                return false;

            await _hocVienRepository.SaveChangesAsync();

            return true;
        }

        // =========================================================
        // LẤY THEO MÃ NGƯỜI DÙNG
        // =========================================================

        public async Task<IEnumerable<HocVienResponse>>
            GetByNguoiDungIdAsync(int maNguoiDung)
        {
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
        // MÃ NGƯỜI DÙNG + PHÂN TRANG
        // =========================================================

        public async Task<object>
            GetByNguoiDungIdPagedAsync(
                int maNguoiDung,
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
                    .GetByNguoiDungIdPagedAsync(
                        maNguoiDung,
                        pageNumber,
                        pageSize);

            return CreatePagedResult(
                result.Data,
                result.TotalCount,
                pageNumber,
                pageSize);
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

        private static HocVienResponse MapToResponse(
            HocVien hocVien)
        {
            return new HocVienResponse
            {
                MaHocVien = hocVien.MaHocVien,

                MaNguoiDung = hocVien.MaNguoiDung,

                HoTen = hocVien.HoTen,

                GioiTinh = hocVien.GioiTinh,

                NgaySinh = hocVien.NgaySinh,

                SoDienThoai = hocVien.SoDienThoai,

                Email = hocVien.Email,

                DiaChi = hocVien.DiaChi,

                TenPhuHuynh = hocVien.TenPhuHuynh,

                SdtPhuHuynh = hocVien.SdtPhuHuynh,

                NgayNhapHoc = hocVien.NgayNhapHoc,

                DangHoatDong = hocVien.DangHoatDong,

                TenDangNhap =
                    hocVien.MaNguoiDungNavigation
                        ?.TenDangNhap,

                AvatarUrl =
                    hocVien.MaNguoiDungNavigation
                        ?.AvatarUrl
            };
        }
    }
}