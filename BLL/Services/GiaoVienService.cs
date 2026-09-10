using BLL.Helpers;
using BLL.Interfaces;
using DAL.Entities;
using DAL.Interfaces;
using DTO.Comon;
using DTO.GiaoVien;
using DTO.NguoiDung;
using Microsoft.OpenApi.Writers;

namespace BLL.Services
{
    public class GiaoVienService : IGiaoVienService
    {
        private readonly IGiaoVienRepository _giaoVienRepository;
        private readonly INguoiDungRepository _nguoiDungRepository;

        public GiaoVienService(
            IGiaoVienRepository giaoVienRepository,
            INguoiDungRepository nguoiDungRepository)
        {
            _giaoVienRepository = giaoVienRepository;
            _nguoiDungRepository = nguoiDungRepository;
        }
        // Lấy tất cả giáo viên
        public async Task<IEnumerable<GiaoVienResponse>> GetAllAsync()
        {
            var ds = await _giaoVienRepository.GetAllAsync();

            return ds.Select(x => new GiaoVienResponse
            {
                // Thông tin Giáo viên
                MaGiaoVien = x.MaGiaoVien,
                MaNguoiDung = x.MaNguoiDung,
                HoTen = x.HoTen,
                GioiTinh = x.GioiTinh,
                NgaySinh = x.NgaySinh,
                SoDienThoai = x.SoDienThoai,
                Email = x.Email,
                ChuyenMon = x.ChuyenMon,
                NgayVaoLam = x.NgayVaoLam,
                LuongTheoGio = x.LuongTheoGio,
                DangHoatDong = x.DangHoatDong,

                // Thông tin Người dùng
                TenDangNhap = x.MaNguoiDungNavigation?.TenDangNhap,
                MatKhauHash = x.MaNguoiDungNavigation?.MatKhauHash,
                MaVaiTro = x.MaNguoiDungNavigation?.MaVaiTro,
                AvatarUrl = x.MaNguoiDungNavigation?.AvatarUrl,
                NgayTao = x.MaNguoiDungNavigation?.NgayTao
            });
        }
        // lấy giáo viên theo id
        public async Task<GiaoVienResponse?> GetByIdAsync(int id)
        {
            var x = await _giaoVienRepository.GetByIdAsync(id);

            if (x == null)
                return null;

            return new GiaoVienResponse
            {
                MaGiaoVien = x.MaGiaoVien,
                MaNguoiDung = x.MaNguoiDung,
                HoTen = x.HoTen,
                GioiTinh = x.GioiTinh,
                NgaySinh = x.NgaySinh,
                SoDienThoai = x.SoDienThoai,
                Email = x.Email,
                ChuyenMon = x.ChuyenMon,
                NgayVaoLam = x.NgayVaoLam,
                LuongTheoGio = x.LuongTheoGio,
                DangHoatDong = x.DangHoatDong,

                TenDangNhap = x.MaNguoiDungNavigation?.TenDangNhap
            };
        }
        // tạo giáo viên mới
        public async Task CreateAsync(CreateTeacherRequest requestAll)
        {
          
            // kiểm tra email có tồn tại không
            ValidationHelper.CheckEmail(requestAll.Email);
            bool emailTonTai = await _nguoiDungRepository.ExistsByEmailAsync(requestAll.Email);
            if (emailTonTai)
            {
                throw new Exception("Email đã tồn tại trong hệ thống.");
            }
            // kiểm tra số điện thoại có tồn tại không
            ValidationHelper.CheckPhone(requestAll.SoDienThoai);
            bool soDienThoaiTonTai = await _nguoiDungRepository.ExistsBySoDienThoaiAsync(requestAll.SoDienThoai);
            if(soDienThoaiTonTai)
            {
                throw new Exception("Số điện thoại đã tồn tại trong hệ thống.");
            }
            // kiểm tra tên đăng nhập có tồn tại không
            if(requestAll.TenDangNhap != null)
            {
                bool tenDangNhapTonTai = await _nguoiDungRepository.ExistsByUserNameAsync(requestAll.TenDangNhap);
                if (tenDangNhapTonTai)
                {
                    throw new Exception("Tên đăng nhập đã tồn tại trong hệ thống.");
                }
            }
            // CHECK PASSWORD
            ValidationHelper.CheckPassword(requestAll.MatKhau);
            var nguoiDung = new NguoiDung
            {
                TenDangNhap = requestAll.TenDangNhap,
                MatKhauHash = requestAll.MatKhau,
                HoTen = requestAll.HoTen,
                Email = requestAll.Email,
                SoDienThoai = requestAll.SoDienThoai,
                MaVaiTro = requestAll.MaVaiTro,
                AvatarUrl = requestAll.AvatarUrl
            };

            await _nguoiDungRepository.AddAsync(nguoiDung);
            await _nguoiDungRepository.SaveChangesAsync();

            // Lúc này EF Core đã sinh MaNguoiDung
            var giaoVien = new GiaoVien
            {
                MaNguoiDung = nguoiDung.MaNguoiDung,   // <-- Quan trọng
                HoTen = requestAll.HoTen,
                GioiTinh = requestAll.GioiTinh,
                NgaySinh = requestAll.NgaySinh,
                SoDienThoai = requestAll.SoDienThoai,
                Email = requestAll.Email,
                ChuyenMon = requestAll.ChuyenMon,
                NgayVaoLam = requestAll.NgayVaoLam,
                LuongTheoGio = requestAll.LuongTheoGio,
                DangHoatDong = requestAll.DangHoatDong
            };

            await _giaoVienRepository.AddAsync(giaoVien);
            await _giaoVienRepository.SaveChangesAsync();
        }
        // cập nhật giáo viên
        public async Task UpdateAsync(int id, UpdateTeacherRequest request)
        {
            // Lấy giáo viên
            var giaoVien = await _giaoVienRepository.GetByIdAsync(id);

            if (giaoVien == null)
                throw new Exception("Không tìm thấy giáo viên.");

            // Lấy người dùng
            var nguoiDung = await _nguoiDungRepository.GetByIdAsync(giaoVien.MaNguoiDung);

            if (nguoiDung == null)
                throw new Exception("Không tìm thấy người dùng.");

            // ==========================
            // Kiểm tra dữ liệu
            // ==========================

            ValidationHelper.CheckEmail(request.Email);

            bool emailTonTai = await _nguoiDungRepository.ExistsByEmailAsync(
                request.Email,
                nguoiDung.MaNguoiDung);

            if (emailTonTai)
                throw new Exception("Email đã tồn tại.");

            ValidationHelper.CheckPhone(request.SoDienThoai);

            bool sdtTonTai = await _nguoiDungRepository.ExistsBySoDienThoaiAsync(
                request.SoDienThoai,
                nguoiDung.MaNguoiDung);

            if (sdtTonTai)
                throw new Exception("Số điện thoại đã tồn tại.");

            bool tenDangNhapTonTai = await _nguoiDungRepository.ExistsByUserNameAsync(
                request.TenDangNhap,
                nguoiDung.MaNguoiDung);

            if (tenDangNhapTonTai)
                throw new Exception("Tên đăng nhập đã tồn tại.");

            // ==========================
            // Cập nhật Người dùng
            // ==========================

            nguoiDung.TenDangNhap = request.TenDangNhap;
            nguoiDung.HoTen = request.HoTen;
            nguoiDung.Email = request.Email;
            nguoiDung.SoDienThoai = request.SoDienThoai;
            nguoiDung.MaVaiTro = request.MaVaiTro;
            nguoiDung.AvatarUrl = request.AvatarUrl;

            if (!string.IsNullOrWhiteSpace(request.MatKhau))
            {
                ValidationHelper.CheckPassword(request.MatKhau);
                nguoiDung.MatKhauHash = request.MatKhau;
            }

            // ==========================
            // Cập nhật Giáo viên
            // ==========================

            giaoVien.HoTen = request.HoTen;
            giaoVien.GioiTinh = request.GioiTinh;
            giaoVien.NgaySinh = request.NgaySinh;
            giaoVien.SoDienThoai = request.SoDienThoai;
            giaoVien.Email = request.Email;
            giaoVien.ChuyenMon = request.ChuyenMon;
            giaoVien.NgayVaoLam = request.NgayVaoLam;
            giaoVien.LuongTheoGio = request.LuongTheoGio;
            giaoVien.DangHoatDong = request.DangHoatDong;

            // Lưu
            await _nguoiDungRepository.UpdateAsync(nguoiDung);
            await _giaoVienRepository.UpdateAsync(giaoVien);

            await _nguoiDungRepository.SaveChangesAsync();
        }
        // xóa giáo viên
        public async Task DeleteAsync(int id)
        {
            var giaoVien = await _giaoVienRepository.GetByIdAsync(id);

            if (giaoVien == null)
            {
                throw new Exception("Không tìm thấy giáo viên.");
            }

            int? maNguoiDung = giaoVien.MaNguoiDung;

            // Xóa giáo viên trước
            await _giaoVienRepository.DeleteAsync(id);
            await _giaoVienRepository.SaveChangesAsync();

            // Sau đó xóa người dùng
            if (maNguoiDung.HasValue)
            {
                await _nguoiDungRepository.DeleteAsync(maNguoiDung.Value);
                await _nguoiDungRepository.SaveChangesAsync();
            }
            else
            {
                throw new Exception("Không tìm thấy người dùng liên quan đến giáo viên này.");
            }
        }
        // lấy giáo viên theo tên [ok]
        public async Task<IEnumerable<GiaoVienResponse>> GetByNameAsync(string name)
        {
            var ds = await _giaoVienRepository.GetAllAsync();
            var result = ds.Where(x => x.HoTen != null && x.HoTen.Contains(name, StringComparison.OrdinalIgnoreCase));
            return result.Select(x => new GiaoVienResponse
            {
                MaGiaoVien = x.MaGiaoVien,
                MaNguoiDung = x.MaNguoiDung,
                HoTen = x.HoTen,
                GioiTinh = x.GioiTinh,
                NgaySinh = x.NgaySinh,
                SoDienThoai = x.SoDienThoai,
                Email = x.Email,
                ChuyenMon = x.ChuyenMon,
                NgayVaoLam = x.NgayVaoLam,
                LuongTheoGio = x.LuongTheoGio,
                DangHoatDong = x.DangHoatDong,
                TenDangNhap = x.MaNguoiDungNavigation?.TenDangNhap
            });
        }
        // lấy giáo viên theo chuyên môn [ok]
        public async Task<IEnumerable<GiaoVienResponse>> GetByChuyenMonAsync(string chuyenMon)
        {
            var ds = await _giaoVienRepository.GetAllAsync();
            var result = ds.Where(x => x.ChuyenMon != null && x.ChuyenMon.Contains(chuyenMon, StringComparison.OrdinalIgnoreCase));
            return result.Select(x => new GiaoVienResponse
            {
                MaGiaoVien = x.MaGiaoVien,
                MaNguoiDung = x.MaNguoiDung,
                HoTen = x.HoTen,
                GioiTinh = x.GioiTinh,
                NgaySinh = x.NgaySinh,
                SoDienThoai = x.SoDienThoai,
                Email = x.Email,
                ChuyenMon = x.ChuyenMon,
                NgayVaoLam = x.NgayVaoLam,
                LuongTheoGio = x.LuongTheoGio,
                DangHoatDong = x.DangHoatDong,
                TenDangNhap = x.MaNguoiDungNavigation?.TenDangNhap
            });
        }
        // tìm kiếm giáo viên theo tên, số điện thoại, email, chuyên môn [ok]
        public async Task<IEnumerable<GiaoVienResponse>> SearchTeacher(string keyword)
        {
            var ds = await _giaoVienRepository.GetAllAsync();
            var result = ds.Where(x => (x.HoTen != null && x.HoTen.Contains(keyword, StringComparison.OrdinalIgnoreCase)) ||
                                          (x.SoDienThoai != null && x.SoDienThoai.Contains(keyword, StringComparison.OrdinalIgnoreCase)) ||
                                          (x.Email != null && x.Email.Contains(keyword, StringComparison.OrdinalIgnoreCase)) ||
                                          (x.ChuyenMon != null && x.ChuyenMon.Contains(keyword, StringComparison.OrdinalIgnoreCase)));
            return result.Select(x => new GiaoVienResponse
            {
                MaGiaoVien = x.MaGiaoVien,
                MaNguoiDung = x.MaNguoiDung,
                HoTen = x.HoTen,
                GioiTinh = x.GioiTinh,
                NgaySinh = x.NgaySinh,
                SoDienThoai = x.SoDienThoai,
                Email = x.Email,
                ChuyenMon = x.ChuyenMon,
                NgayVaoLam = x.NgayVaoLam,
                LuongTheoGio = x.LuongTheoGio,
                DangHoatDong = x.DangHoatDong,
                TenDangNhap = x.MaNguoiDungNavigation?.TenDangNhap
            });
        }
        // lấy danh sách giáo viên theo phân trang
        public async Task<PagedResult<GiaoVienResponse>> GetPagedAsync( int pageNumber, int pageSize)
        {
            var result = await _giaoVienRepository.GetPagedAsync(pageNumber, pageSize);

            return new PagedResult<GiaoVienResponse>
            {
                Items = result.Items.Select(x => new GiaoVienResponse
                {
                    MaGiaoVien = x.MaGiaoVien,
                    MaNguoiDung = x.MaNguoiDung,
                    HoTen = x.HoTen,
                    GioiTinh = x.GioiTinh,
                    NgaySinh = x.NgaySinh,
                    SoDienThoai = x.SoDienThoai,
                    Email = x.Email,
                    ChuyenMon = x.ChuyenMon,
                    NgayVaoLam = x.NgayVaoLam,
                    LuongTheoGio = x.LuongTheoGio,
                    DangHoatDong = x.DangHoatDong,

                    TenDangNhap = x.MaNguoiDungNavigation?.TenDangNhap,
                    AvatarUrl = x.MaNguoiDungNavigation?.AvatarUrl,
                    MaVaiTro = x.MaNguoiDungNavigation?.MaVaiTro
                }),

                TotalItems = result.TotalItems,
                PageNumber = pageNumber,
                PageSize = pageSize
            };
        }
        // lấy danh sách giáo viên theo tên và phân trang
        public async Task<IEnumerable<GiaoVienResponse>> GetByNamePagingAsync(
    string name,
    int pageNumber,
    int pageSize)
        {
            var ds = await _giaoVienRepository.GetAllAsync();

            var result = ds
                .Where(x => x.HoTen != null &&
                            x.HoTen.Contains(name, StringComparison.OrdinalIgnoreCase))
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize);

            return result.Select(x => new GiaoVienResponse
            {
                MaGiaoVien = x.MaGiaoVien,
                MaNguoiDung = x.MaNguoiDung,
                HoTen = x.HoTen,
                GioiTinh = x.GioiTinh,
                NgaySinh = x.NgaySinh,
                SoDienThoai = x.SoDienThoai,
                Email = x.Email,
                ChuyenMon = x.ChuyenMon,
                NgayVaoLam = x.NgayVaoLam,
                LuongTheoGio = x.LuongTheoGio,
                DangHoatDong = x.DangHoatDong,
                TenDangNhap = x.MaNguoiDungNavigation?.TenDangNhap
            });
        }
        // lấy danh sách giáo viên theo chuyên môn và phân trang
        public async Task<IEnumerable<GiaoVienResponse>> GetByChuyenMonPagingAsync(
    string chuyenMon,
    int pageNumber,
    int pageSize)
        {
            var ds = await _giaoVienRepository.GetAllAsync();

            var result = ds
                .Where(x => x.ChuyenMon != null &&
                            x.ChuyenMon.Contains(chuyenMon, StringComparison.OrdinalIgnoreCase))
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize);

            return result.Select(x => new GiaoVienResponse
            {
                MaGiaoVien = x.MaGiaoVien,
                MaNguoiDung = x.MaNguoiDung,
                HoTen = x.HoTen,
                GioiTinh = x.GioiTinh,
                NgaySinh = x.NgaySinh,
                SoDienThoai = x.SoDienThoai,
                Email = x.Email,
                ChuyenMon = x.ChuyenMon,
                NgayVaoLam = x.NgayVaoLam,
                LuongTheoGio = x.LuongTheoGio,
                DangHoatDong = x.DangHoatDong,
                TenDangNhap = x.MaNguoiDungNavigation?.TenDangNhap
            });
        }
        // tìm kiếm giáo viên theo tên, số điện thoại, email, chuyên môn và phân trang
        public async Task<IEnumerable<GiaoVienResponse>> SearchTeacherPaging(
    string keyword,
    int pageNumber,
    int pageSize)
        {
            var ds = await _giaoVienRepository.GetAllAsync();

            var result = ds
                .Where(x =>
                    (x.HoTen != null && x.HoTen.Contains(keyword, StringComparison.OrdinalIgnoreCase)) ||
                    (x.SoDienThoai != null && x.SoDienThoai.Contains(keyword, StringComparison.OrdinalIgnoreCase)) ||
                    (x.Email != null && x.Email.Contains(keyword, StringComparison.OrdinalIgnoreCase)) ||
                    (x.ChuyenMon != null && x.ChuyenMon.Contains(keyword, StringComparison.OrdinalIgnoreCase)))
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize);

            return result.Select(x => new GiaoVienResponse
            {
                MaGiaoVien = x.MaGiaoVien,
                MaNguoiDung = x.MaNguoiDung,
                HoTen = x.HoTen,
                GioiTinh = x.GioiTinh,
                NgaySinh = x.NgaySinh,
                SoDienThoai = x.SoDienThoai,
                Email = x.Email,
                ChuyenMon = x.ChuyenMon,
                NgayVaoLam = x.NgayVaoLam,
                LuongTheoGio = x.LuongTheoGio,
                DangHoatDong = x.DangHoatDong,
                TenDangNhap = x.MaNguoiDungNavigation?.TenDangNhap
            });
        }

    }
}