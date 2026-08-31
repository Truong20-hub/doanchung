using BLL.Interfaces;
using DAL.Entities;
using DAL.Interfaces;
using DTO.HoaDon;

namespace BLL.Services
{
    public class HoaDonService : IHoaDonService
    {
        private readonly IHoaDonRepository _repository;
        private readonly IHocVienRepository _hocVienRepository;
        private readonly ILopHocRepository _lopHocRepository;

        public HoaDonService(
            IHoaDonRepository repository,
            IHocVienRepository hocVienRepository,
            ILopHocRepository lopHocRepository)
        {
            _repository = repository;
            _hocVienRepository = hocVienRepository;
            _lopHocRepository = lopHocRepository;
        }

        // =====================================================
        // MAPPING
        // =====================================================

        private HoaDonResponse MapToResponse(
            HoaDon entity)
        {
            return new HoaDonResponse
            {
                MaHoaDon = entity.MaHoaDon,

                MaHocVien = entity.MaHocVien,

                HoTenHocVien =
                    entity.MaHocVienNavigation?.HoTen,

                MaLop = entity.MaLop,

                MaLopCode =
                    entity.MaLopNavigation?.MaLopCode,

                TenLop =
                    entity.MaLopNavigation?.TenLop,

                TongTien = entity.TongTien,

                GiamGia = entity.GiamGia,

                ThanhTien = entity.ThanhTien,

                NgayLap = entity.NgayLap,

                HanThanhToan =
                    entity.HanThanhToan,

                TrangThai =
                    entity.TrangThai
            };
        }

        // =====================================================
        // VALIDATE TRẠNG THÁI
        // =====================================================

        private static void ValidateTrangThai(
            string? trangThai)
        {
            if (string.IsNullOrWhiteSpace(
                trangThai))
                return;

            var validStatuses = new[]
            {
                "Chưa thanh toán",
                "Đã thanh toán",
                "Thanh toán một phần",
                "Quá hạn",
                "Hủy"
            };

            if (!validStatuses.Contains(
                trangThai.Trim(),
                StringComparer.OrdinalIgnoreCase))
            {
                throw new ArgumentException(
                    "Trạng thái hóa đơn không hợp lệ. " +
                    "Chỉ chấp nhận: Chưa thanh toán, " +
                    "Đã thanh toán, Thanh toán một phần, " +
                    "Quá hạn, Hủy.");
            }
        }

        // =====================================================
        // VALIDATE TIỀN
        // =====================================================

        private static decimal CalculateThanhTien(
            decimal tongTien,
            decimal? giamGia)
        {
            if (tongTien < 0)
            {
                throw new ArgumentException(
                    "Tổng tiền không được âm.");
            }

            var discount =
                giamGia ?? 0;

            if (discount < 0)
            {
                throw new ArgumentException(
                    "Giảm giá không được âm.");
            }

            if (discount > tongTien)
            {
                throw new ArgumentException(
                    "Giảm giá không được lớn hơn tổng tiền.");
            }

            return tongTien - discount;
        }

        // =====================================================
        // VALIDATE NGÀY
        // =====================================================

        private static void ValidateDate(
            DateOnly? ngayLap,
            DateOnly? hanThanhToan)
        {
            if (ngayLap.HasValue &&
                hanThanhToan.HasValue &&
                hanThanhToan.Value < ngayLap.Value)
            {
                throw new ArgumentException(
                    "Hạn thanh toán không được nhỏ hơn ngày lập hóa đơn.");
            }
        }

        // =====================================================
        // GET ALL
        // =====================================================

        public async Task<IEnumerable<HoaDonResponse>>
            GetAllAsync()
        {
            var data =
                await _repository.GetAllAsync();

            return data.Select(MapToResponse);
        }

        // =====================================================
        // GET BY ID
        // =====================================================

        public async Task<HoaDonResponse?>
            GetByIdAsync(int id)
        {
            var entity =
                await _repository.GetByIdAsync(id);

            if (entity == null)
                return null;

            return MapToResponse(entity);
        }

        // =====================================================
        // GET BY HỌC VIÊN
        // =====================================================

        public async Task<IEnumerable<HoaDonResponse>>
            GetByMaHocVienAsync(
                int maHocVien)
        {
            var exists =
                await _hocVienRepository
                    .ExistsByIdAsync(maHocVien);

            if (!exists)
            {
                throw new KeyNotFoundException(
                    $"Học viên có mã {maHocVien} không tồn tại.");
            }

            var data =
                await _repository
                    .GetByMaHocVienAsync(
                        maHocVien);

            return data.Select(MapToResponse);
        }

        // =====================================================
        // GET BY LỚP
        // =====================================================

        public async Task<IEnumerable<HoaDonResponse>>
            GetByMaLopAsync(int maLop)
        {
            var exists =
                await _lopHocRepository
                    .ExistsByIdAsync(maLop);

            if (!exists)
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
        // GET BY TRẠNG THÁI
        // =====================================================

        public async Task<IEnumerable<HoaDonResponse>>
            GetByTrangThaiAsync(
                string trangThai)
        {
            ValidateTrangThai(trangThai);

            var data =
                await _repository
                    .GetByTrangThaiAsync(
                        trangThai);

            return data.Select(MapToResponse);
        }

        // =====================================================
        // SEARCH
        // =====================================================

        public async Task<IEnumerable<HoaDonResponse>>
            SearchAsync(string? keyword)
        {
            var data =
                await _repository
                    .SearchAsync(keyword);

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

        public async Task<HoaDonResponse>
            CreateAsync(
                CreateHoaDonRequest request)
        {
            // ---------------------------------------------
            // HỌC VIÊN
            // ---------------------------------------------

            var hocVienExists =
                await _hocVienRepository
                    .ExistsByIdAsync(
                        request.MaHocVien);

            if (!hocVienExists)
            {
                throw new KeyNotFoundException(
                    $"Học viên có mã {request.MaHocVien} không tồn tại.");
            }

            // ---------------------------------------------
            // LỚP
            // ---------------------------------------------

            var lopExists =
                await _lopHocRepository
                    .ExistsByIdAsync(
                        request.MaLop);

            if (!lopExists)
            {
                throw new KeyNotFoundException(
                    $"Lớp học có mã {request.MaLop} không tồn tại.");
            }

            // ---------------------------------------------
            // TIỀN
            // ---------------------------------------------

            var thanhTien =
                CalculateThanhTien(
                    request.TongTien,
                    request.GiamGia);

            // ---------------------------------------------
            // NGÀY
            // ---------------------------------------------

            ValidateDate(
                request.NgayLap,
                request.HanThanhToan);

            // ---------------------------------------------
            // TRẠNG THÁI
            // ---------------------------------------------

            ValidateTrangThai(
                request.TrangThai);

            // ---------------------------------------------
            // TẠO ENTITY
            // ---------------------------------------------

            var entity = new HoaDon
            {
                MaHocVien =
                    request.MaHocVien,

                MaLop =
                    request.MaLop,

                TongTien =
                    request.TongTien,

                GiamGia =
                    request.GiamGia ?? 0,

                ThanhTien =
                    thanhTien,

                NgayLap =
                    request.NgayLap,

                HanThanhToan =
                    request.HanThanhToan,

                TrangThai =
                    string.IsNullOrWhiteSpace(
                        request.TrangThai)
                        ? "Chưa thanh toán"
                        : request.TrangThai.Trim()
            };

            var result =
                await _repository.AddAsync(
                    entity);

            var created =
                await _repository.GetByIdAsync(
                    result.MaHoaDon);

            return MapToResponse(
                created!);
        }

        // =====================================================
        // UPDATE
        // =====================================================

        public async Task<HoaDonResponse?>
            UpdateAsync(
                int id,
                UpdateHoaDonRequest request)
        {
            var existing =
                await _repository.GetByIdAsync(id);

            if (existing == null)
            {
                throw new KeyNotFoundException(
                    $"Hóa đơn có mã {id} không tồn tại.");
            }

            // ---------------------------------------------
            // HỌC VIÊN
            // ---------------------------------------------

            var hocVienExists =
                await _hocVienRepository
                    .ExistsByIdAsync(
                        request.MaHocVien);

            if (!hocVienExists)
            {
                throw new KeyNotFoundException(
                    $"Học viên có mã {request.MaHocVien} không tồn tại.");
            }

            // ---------------------------------------------
            // LỚP
            // ---------------------------------------------

            var lopExists =
                await _lopHocRepository
                    .ExistsByIdAsync(
                        request.MaLop);

            if (!lopExists)
            {
                throw new KeyNotFoundException(
                    $"Lớp học có mã {request.MaLop} không tồn tại.");
            }

            // ---------------------------------------------
            // TIỀN
            // ---------------------------------------------

            var thanhTien =
                CalculateThanhTien(
                    request.TongTien,
                    request.GiamGia);

            // ---------------------------------------------
            // NGÀY
            // ---------------------------------------------

            ValidateDate(
                request.NgayLap,
                request.HanThanhToan);

            // ---------------------------------------------
            // TRẠNG THÁI
            // ---------------------------------------------

            ValidateTrangThai(
                request.TrangThai);

            // ---------------------------------------------
            // UPDATE
            // ---------------------------------------------

            existing.MaHocVien =
                request.MaHocVien;

            existing.MaLop =
                request.MaLop;

            existing.TongTien =
                request.TongTien;

            existing.GiamGia =
                request.GiamGia ?? 0;

            existing.ThanhTien =
                thanhTien;

            existing.NgayLap =
                request.NgayLap;

            existing.HanThanhToan =
                request.HanThanhToan;

            existing.TrangThai =
                string.IsNullOrWhiteSpace(
                    request.TrangThai)
                    ? existing.TrangThai
                    : request.TrangThai.Trim();

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

        public async Task<bool>
            DeleteAsync(int id)
        {
            var exists =
                await _repository
                    .ExistsByIdAsync(id);

            if (!exists)
            {
                throw new KeyNotFoundException(
                    $"Hóa đơn có mã {id} không tồn tại.");
            }

            // ---------------------------------------------
            // KIỂM TRA THANH TOÁN
            // ---------------------------------------------

            var hasThanhToan =
                await _repository
                    .HasThanhToanAsync(id);

            if (hasThanhToan)
            {
                throw new InvalidOperationException(
                    "Không thể xóa hóa đơn vì hóa đơn " +
                    "đã có dữ liệu thanh toán tham chiếu.");
            }

            try
            {
                return await _repository
                    .DeleteAsync(id);
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException(
                    "Không thể xóa hóa đơn.",
                    ex);
            }
        }

        // =====================================================
        // VALIDATE PAGING
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