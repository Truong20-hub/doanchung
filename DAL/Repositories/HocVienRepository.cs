using DAL.Context;
using DAL.Entities;
using DAL.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace DAL.Repositories
{
    public class HocVienRepository : IHocVienRepository
    {
        private readonly AppDbContext _context;

        public HocVienRepository(AppDbContext context)
        {
            _context = context;
        }

        // =========================================================
        // CRUD CƠ BẢN
        // =========================================================

        // Lấy tất cả học viên
        public async Task<IEnumerable<HocVien>> GetAllAsync()
        {
            return await _context.HocViens
                .Include(x => x.MaNguoiDungNavigation)
                .ToListAsync();
        }

        // Lấy học viên theo ID
        public async Task<HocVien?> GetByIdAsync(int id)
        {
            return await _context.HocViens
                .Include(x => x.MaNguoiDungNavigation)
                .FirstOrDefaultAsync(x => x.MaHocVien == id);
        }

        // Kiểm tra học viên có tồn tại hay không
        public async Task<bool> ExistsByIdAsync(int id)
        {
            return await _context.HocViens
                .AnyAsync(x => x.MaHocVien == id);
        }

        // Kiểm tra mã người dùng đã được liên kết với học viên chưa
        public async Task<bool> ExistsByNguoiDungIdAsync(int maNguoiDung)
        {
            return await _context.HocViens
                .AnyAsync(x => x.MaNguoiDung == maNguoiDung);
        }

        // Thêm học viên
        public async Task<HocVien> AddAsync(HocVien hocVien)
        {
            await _context.HocViens.AddAsync(hocVien);

            return hocVien;
        }

        // Cập nhật học viên
        public async Task<HocVien?> UpdateAsync(int id, HocVien hocVien)
        {
            var existingHocVien = await _context.HocViens
                .Include(x => x.MaNguoiDungNavigation)
                .FirstOrDefaultAsync(x => x.MaHocVien == id);

            if (existingHocVien == null)
                return null;

            existingHocVien.GioiTinh = hocVien.GioiTinh;
            existingHocVien.NgaySinh = hocVien.NgaySinh;
            existingHocVien.DiaChi = hocVien.DiaChi;
            existingHocVien.TenPhuHuynh = hocVien.TenPhuHuynh;
            existingHocVien.SdtPhuHuynh = hocVien.SdtPhuHuynh;
            existingHocVien.NgayNhapHoc = hocVien.NgayNhapHoc;

            return existingHocVien;
        }

        // Xóa học viên
        public async Task<bool> DeleteAsync(int id)
        {
            var hocVien = await _context.HocViens
                .FirstOrDefaultAsync(x => x.MaHocVien == id);

            if (hocVien == null)
                return false;

            // Lấy mã người dùng trước khi xóa học viên
            int? maNguoiDung = hocVien.MaNguoiDung;

            // =====================================================
            // 1. XÓA THANH TOÁN
            // =====================================================

            var hoaDons = await _context.HoaDons
                .Where(x => x.MaHocVien == id)
                .ToListAsync();

            var maHoaDons = hoaDons
                .Select(x => x.MaHoaDon)
                .ToList();

            if (maHoaDons.Any())
            {
                var thanhToans = await _context.ThanhToans
                    .Where(x => maHoaDons.Contains(x.MaHoaDon))
                    .ToListAsync();

                _context.ThanhToans.RemoveRange(thanhToans);
            }

            // =====================================================
            // 2. XÓA HÓA ĐƠN
            // =====================================================

            _context.HoaDons.RemoveRange(hoaDons);

            // =====================================================
            // 3. XÓA ĐIỂM DANH
            // =====================================================

            var diemDanhs = await _context.DiemDanhs
                .Where(x => x.MaHocVien == id)
                .ToListAsync();

            _context.DiemDanhs.RemoveRange(diemDanhs);

            // =====================================================
            // 4. XÓA ĐIỂM THI
            // =====================================================

            var diemThis = await _context.DiemThis
                .Where(x => x.MaHocVien == id)
                .ToListAsync();

            _context.DiemThis.RemoveRange(diemThis);

            // =====================================================
            // 5. XÓA ĐĂNG KÝ HỌC
            // =====================================================

            var dangKyHocs = await _context.DangKyHocs
                .Where(x => x.MaHocVien == id)
                .ToListAsync();

            _context.DangKyHocs.RemoveRange(dangKyHocs);

            // =====================================================
            // 6. XÓA HỌC VIÊN
            // =====================================================

            _context.HocViens.Remove(hocVien);

            // =====================================================
            // 7. XÓA TÀI KHOẢN NGƯỜI DÙNG
            // =====================================================

            if (maNguoiDung.HasValue)
            {
                var nguoiDung = await _context.NguoiDungs
                    .FirstOrDefaultAsync(
                        x => x.MaNguoiDung == maNguoiDung.Value);

                if (nguoiDung != null)
                {
                    _context.NguoiDungs.Remove(nguoiDung);
                }
            }

            await _context.SaveChangesAsync();

            return true;
        }

        // =========================================================
        // TÌM KIẾM THEO TỪNG TIÊU CHÍ
        // =========================================================

        // Theo mã người dùng
        public async Task<IEnumerable<HocVien>> GetByNguoiDungIdAsync(
            int maNguoiDung)
        {
            return await _context.HocViens
                .Include(x => x.MaNguoiDungNavigation)
                .Where(x => x.MaNguoiDung == maNguoiDung)
                .ToListAsync();
        }

        // Theo tên học viên
        public async Task<IEnumerable<HocVien>> GetByNameAsync(
            string name)
        {
            if (string.IsNullOrWhiteSpace(name))
            {
                return new List<HocVien>();
            }

            name = name.Trim();

            return await _context.HocViens
                .Include(hv => hv.MaNguoiDungNavigation)
                .Where(hv =>
                    hv.MaNguoiDungNavigation != null &&
                    hv.MaNguoiDungNavigation.HoTen != null &&
                    hv.MaNguoiDungNavigation.HoTen.Contains(name))
                .ToListAsync();
        }

        // Theo tên phụ huynh
        public async Task<IEnumerable<HocVien>> GetByParentNameAsync(
            string parentName)
        {
            return await _context.HocViens
                .Include(x => x.MaNguoiDungNavigation)
                .Where(x =>
                    x.TenPhuHuynh != null &&
                    x.TenPhuHuynh.Contains(parentName))
                .ToListAsync();
        }

        // Theo địa chỉ
        public async Task<IEnumerable<HocVien>> GetByAddressAsync(
            string address)
        {
            return await _context.HocViens
                .Include(x => x.MaNguoiDungNavigation)
                .Where(x =>
                    x.DiaChi != null &&
                    x.DiaChi.Contains(address))
                .ToListAsync();
        }

        // Theo số điện thoại
        public async Task<IEnumerable<HocVien>> GetByPhoneAsync(
            string phone)
        {
            if (string.IsNullOrWhiteSpace(phone))
            {
                return new List<HocVien>();
            }

            phone = phone.Trim();

            return await _context.HocViens
                .Include(hv => hv.MaNguoiDungNavigation)
                .Where(hv =>
                    hv.MaNguoiDungNavigation != null &&
                    hv.MaNguoiDungNavigation.SoDienThoai != null &&
                    hv.MaNguoiDungNavigation.SoDienThoai.Contains(phone))
                .ToListAsync();
        }

        // Theo email
        public async Task<IEnumerable<HocVien>> GetByEmailAsync(
            string email)
        {
            return await _context.HocViens
       .Include(h => h.MaNguoiDungNavigation)
       .Where(h =>
           h.MaNguoiDungNavigation != null &&
           h.MaNguoiDungNavigation.Email != null &&
           h.MaNguoiDungNavigation.Email.Contains(email))
       .ToListAsync();
        }

        // =========================================================
        // TÌM KIẾM NHIỀU TIÊU CHÍ
        // =========================================================

        public async Task<IEnumerable<HocVien>> SearchAsync(string keyword)
        {
            return await _context.HocViens
                .Include(x => x.MaNguoiDungNavigation)
                .Where(x =>
                    (
                        x.MaNguoiDungNavigation != null
                        &&
                        (
                            (
                                x.MaNguoiDungNavigation.HoTen != null
                                &&
                                x.MaNguoiDungNavigation.HoTen.Contains(keyword)
                            )

                            ||

                            (
                                x.MaNguoiDungNavigation.Email != null
                                &&
                                x.MaNguoiDungNavigation.Email.Contains(keyword)
                            )

                            ||

                            (
                                x.MaNguoiDungNavigation.SoDienThoai != null
                                &&
                                x.MaNguoiDungNavigation.SoDienThoai.Contains(keyword)
                            )
                        )
                    )

                    ||

                    (
                        x.DiaChi != null
                        &&
                        x.DiaChi.Contains(keyword)
                    )

                    ||

                    (
                        x.TenPhuHuynh != null
                        &&
                        x.TenPhuHuynh.Contains(keyword)
                    )
                )
                .ToListAsync();
        }

        // =========================================================
        // PHÂN TRANG
        // =========================================================

        public async Task<(IEnumerable<HocVien> Data, int TotalCount)>
            GetPagedAsync(
                int pageNumber,
                int pageSize)
        {
            var query = _context.HocViens
                .Include(x => x.MaNguoiDungNavigation)
                .AsQueryable();

            var totalCount = await query.CountAsync();

            var data = await query
                .OrderBy(x => x.MaHocVien)
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            return (data, totalCount);
        }

        // =========================================================
        // TÌM KIẾM + PHÂN TRANG
        // =========================================================

        // Theo tên học viên + phân trang
        public async Task<(IEnumerable<HocVien> Data, int TotalCount)>
            GetByNamePagedAsync(
                string name,
                int pageNumber,
                int pageSize)
        {
            var query = _context.HocViens
                .Include(x => x.MaNguoiDungNavigation)
                .Where(x =>
                    x.MaNguoiDungNavigation != null &&
                    x.MaNguoiDungNavigation.HoTen != null &&
                    x.MaNguoiDungNavigation.HoTen.Contains(name))
                .AsQueryable();

            var totalCount = await query.CountAsync();

            var data = await query
                .OrderBy(x => x.MaHocVien)
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            return (data, totalCount);
        }

        // Theo tên phụ huynh + phân trang
        public async Task<(IEnumerable<HocVien> Data, int TotalCount)>
            GetByParentNamePagedAsync(
                string parentName,
                int pageNumber,
                int pageSize)
        {
            var query = _context.HocViens
                .Include(x => x.MaNguoiDungNavigation)
                .Where(x =>
                    x.TenPhuHuynh != null &&
                    x.TenPhuHuynh.Contains(parentName))
                .AsQueryable();

            var totalCount = await query.CountAsync();

            var data = await query
                .OrderBy(x => x.MaHocVien)
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            return (data, totalCount);
        }

        // Theo địa chỉ + phân trang
        public async Task<(IEnumerable<HocVien> Data, int TotalCount)>
            GetByAddressPagedAsync(
                string address,
                int pageNumber,
                int pageSize)
        {
            var query = _context.HocViens
                .Include(x => x.MaNguoiDungNavigation)
                .Where(x =>
                    x.DiaChi != null &&
                    x.DiaChi.Contains(address))
                .AsQueryable();

            var totalCount = await query.CountAsync();

            var data = await query
                .OrderBy(x => x.MaHocVien)
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            return (data, totalCount);
        }

        // Theo số điện thoại + phân trang
        public async Task<(IEnumerable<HocVien> Data, int TotalCount)>
            GetByPhonePagedAsync(
                string phone,
                int pageNumber,
                int pageSize)
        {
            var query = _context.HocViens
                .Include(x => x.MaNguoiDungNavigation)
                .Where(x =>
                    x.MaNguoiDungNavigation != null &&
                    x.MaNguoiDungNavigation.SoDienThoai != null &&
                    x.MaNguoiDungNavigation.SoDienThoai.Contains(phone))
                .AsQueryable();

            var totalCount = await query.CountAsync();

            var data = await query
                .OrderBy(x => x.MaHocVien)
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            return (data, totalCount);
        }

        // Theo email + phân trang
        public async Task<(IEnumerable<HocVien> Data, int TotalCount)>
            GetByEmailPagedAsync(
                string email,
                int pageNumber,
                int pageSize)
        {
            var query = _context.HocViens
                .Include(x => x.MaNguoiDungNavigation)
                .Where(x =>
                    x.MaNguoiDungNavigation != null &&
                    x.MaNguoiDungNavigation.Email != null &&
                    x.MaNguoiDungNavigation.Email.Contains(email))
                .AsQueryable();

            var totalCount = await query.CountAsync();

            var data = await query
                .OrderBy(x => x.MaHocVien)
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            return (data, totalCount);
        }

        // Tìm kiếm nhiều tiêu chí + phân trang
        public async Task<(IEnumerable<HocVien> Data, int TotalCount)>
            SearchPagedAsync(
                string keyword,
                int pageNumber,
                int pageSize)
        {
            var query = _context.HocViens
                .Include(x => x.MaNguoiDungNavigation)
                .Where(x =>
                    (
                        x.MaNguoiDungNavigation != null &&
                        (
                            (x.MaNguoiDungNavigation.HoTen != null && x.MaNguoiDungNavigation.HoTen.Contains(keyword)) ||
                            (x.MaNguoiDungNavigation.Email != null && x.MaNguoiDungNavigation.Email.Contains(keyword)) ||
                            (x.MaNguoiDungNavigation.SoDienThoai != null && x.MaNguoiDungNavigation.SoDienThoai.Contains(keyword))
                        )
                    )
                    ||
                    (x.DiaChi != null && x.DiaChi.Contains(keyword))
                    ||
                    (x.TenPhuHuynh != null && x.TenPhuHuynh.Contains(keyword))
                )
                .AsQueryable();

            var totalCount = await query.CountAsync();

            var data = await query
                .OrderBy(x => x.MaHocVien)
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            return (data, totalCount);
        }
        // kiểm tra học viên là khóa ngoại của bảng nào ko
        public async Task<bool> IsForeignKeyAsync(int maHocVien)
        {
            // Kiểm tra trong bảng HoaDon
            bool existsInHoaDon = await _context.HoaDons
                .AnyAsync(x => x.MaHocVien == maHocVien);

            if (existsInHoaDon)
                return true;

            // Kiểm tra trong bảng DiemDanh
            bool existsInDiemDanh = await _context.DiemDanhs
                .AnyAsync(x => x.MaHocVien == maHocVien);

            if (existsInDiemDanh)
                return true;

            // Kiểm tra trong bảng DiemThi
            bool existsInDiemThi = await _context.DiemThis
                .AnyAsync(x => x.MaHocVien == maHocVien);

            if (existsInDiemThi)
                return true;

            // Kiểm tra trong bảng DangKyHoc
            bool existsInDangKyHoc = await _context.DangKyHocs
                .AnyAsync(x => x.MaHocVien == maHocVien);

            if (existsInDangKyHoc)
                return true;

            // Nếu không tồn tại trong bất kỳ bảng nào, trả về false
            return false;
        }

        // =========================================================
        // SAVE
        // =========================================================

        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }
    }
}