using BLL.Interfaces;
using DAL.Entities;
using DAL.Interfaces;
using DTO.Auth;
using DTO.NguoiDung;

namespace BLL.Services
{
    public class NguoiDungService : INguoiDungService
    {
        private readonly INguoiDungRepository _nguoiDungRepository;

        public NguoiDungService(INguoiDungRepository nguoiDungRepository)
        {
            _nguoiDungRepository = nguoiDungRepository;
        }

        // Lấy danh sách
        public async Task<IEnumerable<NguoiDungResponse>> GetAllAsync()
        {
            var dsNguoiDung = await _nguoiDungRepository.GetAllAsync();

            return dsNguoiDung.Select(nd => new NguoiDungResponse
            {
                MaNguoiDung = nd.MaNguoiDung,
                TenDangNhap = nd.TenDangNhap,
                HoTen = nd.HoTen,
                Email = nd.Email,
                SoDienThoai = nd.SoDienThoai,
                AvatarUrl = nd.AvatarUrl,
                DangHoatDong = nd.DangHoatDong,
                MaVaiTro = nd.MaVaiTro,
                TenVaiTro = nd.MaVaiTroNavigation.TenVaiTro
            });
        }

        // Lấy theo ID
        public async Task<NguoiDungResponse?> GetByIdAsync(int id)
        {
            var nd = await _nguoiDungRepository.GetByIdAsync(id);

            if (nd == null)
                return null;

            return new NguoiDungResponse
            {
                MaNguoiDung = nd.MaNguoiDung,
                TenDangNhap = nd.TenDangNhap,
                HoTen = nd.HoTen,
                Email = nd.Email,
                SoDienThoai = nd.SoDienThoai,
                AvatarUrl = nd.AvatarUrl,
                DangHoatDong = nd.DangHoatDong,
                MaVaiTro = nd.MaVaiTro,
                TenVaiTro = nd.MaVaiTroNavigation.TenVaiTro
            };
        }

        // Thêm
        public async Task CreateAsync(CreateNguoiDungRequest request)
        {
            var checkUser = await _nguoiDungRepository
                .GetByUserNameAsync(request.TenDangNhap);

            if (checkUser != null)
                throw new Exception("Tên đăng nhập đã tồn tại.");

            var nguoiDung = new NguoiDung
            {
                TenDangNhap = request.TenDangNhap,
                MatKhauHash = request.MatKhau, // Sau này sẽ hash BCrypt
                HoTen = request.HoTen,
                Email = request.Email,
                SoDienThoai = request.SoDienThoai,
                MaVaiTro = request.MaVaiTro,
                AvatarUrl = request.AvatarUrl,
                DangHoatDong = true,
                NgayTao = DateTime.Now
            };

            await _nguoiDungRepository.AddAsync(nguoiDung);
            await _nguoiDungRepository.SaveChangesAsync();
        }

        // Cập nhật
        public async Task UpdateAsync(int id, UpdateNguoiDungRequest request)
        {
            var nguoiDung = await _nguoiDungRepository.GetByIdAsync(id);

            if (nguoiDung == null)
                throw new Exception("Không tìm thấy người dùng.");

            nguoiDung.HoTen = request.HoTen;
            nguoiDung.Email = request.Email;
            nguoiDung.SoDienThoai = request.SoDienThoai;
            nguoiDung.MaVaiTro = request.MaVaiTro;
            nguoiDung.AvatarUrl = request.AvatarUrl;
            nguoiDung.DangHoatDong = request.DangHoatDong;

            await _nguoiDungRepository.UpdateAsync(nguoiDung);
            await _nguoiDungRepository.SaveChangesAsync();
        }

        // Xóa
        public async Task DeleteAsync(int id)
        {
            await _nguoiDungRepository.DeleteAsync(id);
            await _nguoiDungRepository.SaveChangesAsync();
        }
        private readonly IJwtService _jwtService;
        public NguoiDungService(
    INguoiDungRepository nguoiDungRepository,
    IJwtService jwtService)
        {
            _nguoiDungRepository = nguoiDungRepository;
            _jwtService = jwtService;
        }
        public async Task<LoginResponse> LoginAsync(LoginResquest request)
        {
            var user = await _nguoiDungRepository.LoginAsync(request.TenDangNhap);

            if (user == null)
                throw new Exception("Sai tài khoản hoặc mật khẩu.");

            // Nếu đã hash BCrypt thì thay bằng BCrypt.Verify(...)
            if (user.MatKhauHash != request.MatKhau)
                throw new Exception("Sai tài khoản hoặc mật khẩu.");

            var token = _jwtService.GenerateToken(user);

            return new LoginResponse
            {
                MaNguoiDung = user.MaNguoiDung,
                HoTen = user.HoTen,
                TenDangNhap = user.TenDangNhap,
                VaiTro = user.MaVaiTroNavigation.TenVaiTro,
                Token = token
            };
        }
    }
}