using BLL.Interfaces;
using DAL.Entities;
using DAL.Interfaces;
using DTO.ThanhToan;

namespace BLL.Services
{
    public class ThanhToanService : IThanhToanService
    {
        private readonly IThanhToanRepository _repository;
        private readonly IHoaDonRepository _hoaDonRepository;

        public ThanhToanService(
            IThanhToanRepository repository,
            IHoaDonRepository hoaDonRepository)
        {
            _repository = repository;
            _hoaDonRepository = hoaDonRepository;
        }

        // =====================================================
        // MAPPING
        // =====================================================

        private ThanhToanResponse MapToResponse(
            ThanhToan entity,
            decimal tongDaThanhToan)
        {
            var hoaDon =
                entity.MaHoaDonNavigation;

            var thanhTien =
                hoaDon?.ThanhTien
                ?? hoaDon?.TongTien
                ?? 0;

            var conLai =
                thanhTien - tongDaThanhToan;

            if (conLai < 0)
                conLai = 0;

            return new ThanhToanResponse
            {
                MaThanhToan =
                    entity.MaThanhToan,

                MaHoaDon =
                    entity.MaHoaDon,

                TongTienHoaDon =
                    hoaDon?.TongTien ?? 0,

                GiamGia =
                    hoaDon?.GiamGia ?? 0,

                ThanhTien =
                    thanhTien,

                TongDaThanhToan =
                    tongDaThanhToan,

                ConLai =
                    conLai,

                SoTienDaTra =
                    entity.SoTienDaTra,

                NgayThanhToan =
                    entity.NgayThanhToan,

                HinhThuc =
                    entity.HinhThuc,

                GhiChu =
                    entity.GhiChu
            };
        }

        // =====================================================
        // GET ALL
        // =====================================================

        public async Task<IEnumerable<ThanhToanResponse>>
            GetAllAsync()
        {
            var data =
                await _repository.GetAllAsync();

            var result =
                new List<ThanhToanResponse>();

            foreach (var item in data)
            {
                var total =
                    await _repository
                        .GetTongDaThanhToanAsync(
                            item.MaHoaDon);

                result.Add(
                    MapToResponse(
                        item,
                        total));
            }

            return result;
        }

        // =====================================================
        // GET BY ID
        // =====================================================

        public async Task<ThanhToanResponse?>
            GetByIdAsync(int id)
        {
            var entity =
                await _repository.GetByIdAsync(id);

            if (entity == null)
                return null;

            var total =
                await _repository
                    .GetTongDaThanhToanAsync(
                        entity.MaHoaDon);

            return MapToResponse(
                entity,
                total);
        }

        // =====================================================
        // GET BY HÓA ĐƠN
        // =====================================================

        public async Task<IEnumerable<ThanhToanResponse>>
            GetByMaHoaDonAsync(
                int maHoaDon)
        {
            var hoaDon =
                await _hoaDonRepository
                    .GetByIdAsync(maHoaDon);

            if (hoaDon == null)
            {
                throw new KeyNotFoundException(
                    $"Hóa đơn có mã {maHoaDon} không tồn tại.");
            }

            var data =
                await _repository
                    .GetByMaHoaDonAsync(
                        maHoaDon);

            var total =
                await _repository
                    .GetTongDaThanhToanAsync(
                        maHoaDon);

            return data.Select(
                x => MapToResponse(
                    x,
                    total));
        }

        // =====================================================
        // PAGED
        // =====================================================

        public async Task<object>
            GetPagedAsync(
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

            return await _repository
                .GetPagedAsync(
                    pageNumber,
                    pageSize);
        }

        // =====================================================
        // CREATE
        // =====================================================

        public async Task<ThanhToanResponse>
            CreateAsync(
                CreateThanhToanRequest request)
        {
            // ---------------------------------------------
            // KIỂM TRA HÓA ĐƠN
            // ---------------------------------------------

            var hoaDon =
                await _hoaDonRepository
                    .GetByIdAsync(
                        request.MaHoaDon);

            if (hoaDon == null)
            {
                throw new KeyNotFoundException(
                    $"Hóa đơn có mã {request.MaHoaDon} không tồn tại.");
            }

            // ---------------------------------------------
            // KIỂM TRA SỐ TIỀN
            // ---------------------------------------------

            if (request.SoTienDaTra <= 0)
            {
                throw new ArgumentException(
                    "Số tiền thanh toán phải lớn hơn 0.");
            }

            // ---------------------------------------------
            // TÍNH TỔNG ĐÃ THANH TOÁN
            // ---------------------------------------------

            var tongDaThanhToan =
                await _repository
                    .GetTongDaThanhToanAsync(
                        request.MaHoaDon);

            var thanhTien =
                hoaDon.ThanhTien
                ?? hoaDon.TongTien;

            var conLai =
                thanhTien - tongDaThanhToan;

            // ---------------------------------------------
            // ĐÃ THANH TOÁN HẾT
            // ---------------------------------------------

            if (conLai <= 0)
            {
                throw new InvalidOperationException(
                    "Hóa đơn này đã được thanh toán đầy đủ.");
            }

            // ---------------------------------------------
            // THANH TOÁN VƯỢT QUÁ
            // ---------------------------------------------

            if (request.SoTienDaTra > conLai)
            {
                throw new InvalidOperationException(
                    $"Số tiền thanh toán vượt quá số tiền còn lại. " +
                    $"Số tiền còn lại: {conLai:N2}.");
            }

            // ---------------------------------------------
            // TẠO THANH TOÁN
            // ---------------------------------------------

            var entity = new ThanhToan
            {
                MaHoaDon =
                    request.MaHoaDon,

                SoTienDaTra =
                    request.SoTienDaTra,

                NgayThanhToan =
                    request.NgayThanhToan
                    ?? DateTime.Now,

                HinhThuc =
                    request.HinhThuc,

                GhiChu =
                    request.GhiChu
            };

            var result =
                await _repository
                    .AddAsync(entity);

            // ---------------------------------------------
            // TỔNG SAU THANH TOÁN
            // ---------------------------------------------

            var tongSauThanhToan =
                tongDaThanhToan +
                request.SoTienDaTra;

            // ---------------------------------------------
            // CẬP NHẬT TRẠNG THÁI HÓA ĐƠN
            // ---------------------------------------------

            if (tongSauThanhToan >= thanhTien)
            {
                hoaDon.TrangThai =
                    "Đã thanh toán";
            }
            else
            {
                hoaDon.TrangThai =
                    "Thanh toán một phần";
            }

            await _hoaDonRepository
                .UpdateAsync(hoaDon);

            // ---------------------------------------------
            // TRẢ RESPONSE
            // ---------------------------------------------

            var created =
                await _repository
                    .GetByIdAsync(
                        result.MaThanhToan);

            return MapToResponse(
                created!,
                tongSauThanhToan);
        }

        // =====================================================
        // UPDATE
        // =====================================================

        public async Task<ThanhToanResponse?>
            UpdateAsync(
                int id,
                UpdateThanhToanRequest request)
        {
            // ---------------------------------------------
            // KIỂM TRA THANH TOÁN
            // ---------------------------------------------

            var existing =
                await _repository
                    .GetByIdAsync(id);

            if (existing == null)
            {
                throw new KeyNotFoundException(
                    $"Thanh toán có mã {id} không tồn tại.");
            }

            // ---------------------------------------------
            // KIỂM TRA HÓA ĐƠN
            // ---------------------------------------------

            var hoaDon =
                await _hoaDonRepository
                    .GetByIdAsync(
                        request.MaHoaDon);

            if (hoaDon == null)
            {
                throw new KeyNotFoundException(
                    $"Hóa đơn có mã {request.MaHoaDon} không tồn tại.");
            }

            // ---------------------------------------------
            // KIỂM TRA SỐ TIỀN
            // ---------------------------------------------

            if (request.SoTienDaTra <= 0)
            {
                throw new ArgumentException(
                    "Số tiền thanh toán phải lớn hơn 0.");
            }

            // ---------------------------------------------
            // TỔNG ĐÃ THANH TOÁN
            // ---------------------------------------------

            var tongDaThanhToan =
                await _repository
                    .GetTongDaThanhToanAsync(
                        request.MaHoaDon);

            // Trừ khoản thanh toán cũ
            if (existing.MaHoaDon == request.MaHoaDon)
            {
                tongDaThanhToan -=
                    existing.SoTienDaTra;
            }

            var thanhTien =
                hoaDon.ThanhTien
                ?? hoaDon.TongTien;

            var conLai =
                thanhTien - tongDaThanhToan;

            // ---------------------------------------------
            // KHÔNG ĐƯỢC VƯỢT
            // ---------------------------------------------

            if (request.SoTienDaTra > conLai)
            {
                throw new InvalidOperationException(
                    $"Số tiền thanh toán vượt quá số tiền còn lại. " +
                    $"Số tiền còn lại: {conLai:N2}.");
            }

            // ---------------------------------------------
            // UPDATE
            // ---------------------------------------------

            existing.MaHoaDon =
                request.MaHoaDon;

            existing.SoTienDaTra =
                request.SoTienDaTra;

            existing.NgayThanhToan =
                request.NgayThanhToan
                ?? existing.NgayThanhToan;

            existing.HinhThuc =
                request.HinhThuc;

            existing.GhiChu =
                request.GhiChu;

            var result =
                await _repository
                    .UpdateAsync(existing);

            if (result == null)
                return null;

            // ---------------------------------------------
            // CẬP NHẬT TRẠNG THÁI HÓA ĐƠN
            // ---------------------------------------------

            var tongMoi =
                tongDaThanhToan +
                request.SoTienDaTra;

            if (tongMoi >= thanhTien)
            {
                hoaDon.TrangThai =
                    "Đã thanh toán";
            }
            else if (tongMoi > 0)
            {
                hoaDon.TrangThai =
                    "Thanh toán một phần";
            }
            else
            {
                hoaDon.TrangThai =
                    "Chưa thanh toán";
            }

            await _hoaDonRepository
                .UpdateAsync(hoaDon);

            return MapToResponse(
                result,
                tongMoi);
        }

        // =====================================================
        // DELETE
        // =====================================================

        public async Task<bool>
            DeleteAsync(int id)
        {
            var entity =
                await _repository
                    .GetByIdAsync(id);

            if (entity == null)
            {
                throw new KeyNotFoundException(
                    $"Thanh toán có mã {id} không tồn tại.");
            }

            var maHoaDon =
                entity.MaHoaDon;

            var result =
                await _repository
                    .DeleteAsync(id);

            // ---------------------------------------------
            // CẬP NHẬT LẠI TRẠNG THÁI HÓA ĐƠN
            // ---------------------------------------------

            if (result)
            {
                var hoaDon =
                    await _hoaDonRepository
                        .GetByIdAsync(
                            maHoaDon);

                if (hoaDon != null)
                {
                    var tongDaThanhToan =
                        await _repository
                            .GetTongDaThanhToanAsync(
                                maHoaDon);

                    var thanhTien =
                        hoaDon.ThanhTien
                        ?? hoaDon.TongTien;

                    if (tongDaThanhToan <= 0)
                    {
                        hoaDon.TrangThai =
                            "Chưa thanh toán";
                    }
                    else if (tongDaThanhToan >=
                             thanhTien)
                    {
                        hoaDon.TrangThai =
                            "Đã thanh toán";
                    }
                    else
                    {
                        hoaDon.TrangThai =
                            "Thanh toán một phần";
                    }

                    await _hoaDonRepository
                        .UpdateAsync(hoaDon);
                }
            }

            return result;
        }
    }
}