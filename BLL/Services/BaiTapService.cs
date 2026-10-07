using BLL.Interfaces;
using DAL.Entities;
using DAL.Interfaces;
using DTO.BaiTap;
using System.Text.Json;

namespace BLL.Services
{
    public class BaiTapService : IBaiTapService
    {
        private readonly IBaiTapRepository _repository;
        private readonly ILopHocRepository _lopHocRepository;
        private readonly IBuoiHocRepository _buoiHocRepository;
        private readonly IGiaoVienRepository _giaoVienRepository;
        private readonly IHocVienRepository _hocVienRepository;
        private readonly IDangKyHocRepository _dangKyHocRepository;
        private readonly IDiemRepository _diemRepository;

        public BaiTapService(
            IBaiTapRepository repository,
            ILopHocRepository lopHocRepository,
            IBuoiHocRepository buoiHocRepository,
            IGiaoVienRepository giaoVienRepository,
            IHocVienRepository hocVienRepository,
            IDangKyHocRepository dangKyHocRepository,
            IDiemRepository diemRepository)
        {
            _repository = repository;
            _lopHocRepository = lopHocRepository;
            _buoiHocRepository = buoiHocRepository;
            _giaoVienRepository = giaoVienRepository;
            _hocVienRepository = hocVienRepository;
            _dangKyHocRepository = dangKyHocRepository;
            _diemRepository = diemRepository;
        }

        // =====================================================
        // MAPPING
        // =====================================================

        private static BaiTapResponse MapToResponse(BaiTap entity)
        {
            // Bài nộp của học viên (chỉ có khi nạp từ GetByMaHocVienAsync)
            var diem = entity.Diems?.FirstOrDefault();

            return new BaiTapResponse
            {
                MaBaiTap = entity.MaBaiTap,
                MaLop = entity.MaLop,
                MaLopCode = entity.MaLopNavigation?.MaLopCode,
                TenLop = entity.MaLopNavigation?.TenLop,
                MaBuoi = entity.MaBuoi,
                NoiDungBuoi = entity.MaBuoiNavigation?.NoiDung,
                MaGiaoVien = entity.MaGiaoVien,
                HoTenGiaoVien =
                    entity.MaGiaoVienNavigation?.MaNguoiDungNavigation?.HoTen,
                TieuDe = entity.TieuDe,
                MoTa = entity.MoTa,
                NgayGiao = entity.NgayGiao,
                HanNop = entity.HanNop,
                FileDinhKem = entity.FileDinhKem,

                TrangThaiNop = diem?.TrangThaiNop,
                DiemSo = diem?.DiemSo,
                NgayNop = diem?.NgayNop,
                NhanXet = diem?.NhanXet,
                GhiChuNop = diem?.GhiChuNop,
                FilesNop = string.IsNullOrWhiteSpace(diem?.TepNopJson)
                    ? null
                    : JsonSerializer.Deserialize<
                        List<BaiTapSubmissionFileResponse>>(
                            diem.TepNopJson)
            };
        }

        // =====================================================
        // VALIDATE
        // =====================================================

        // Chuẩn hóa đường dẫn tệp: chỉ lưu chuỗi đường dẫn / URL
        private static string? NormalizeFilePath(string? path)
        {
            if (string.IsNullOrWhiteSpace(path))
                return null;

            path = path.Trim();

            if (path.Length > 500)
            {
                throw new ArgumentException(
                    "Đường dẫn tệp không quá 500 ký tự.");
            }

            if (path.Any(char.IsControl))
            {
                throw new ArgumentException(
                    "Đường dẫn tệp chứa ký tự không hợp lệ.");
            }

            // Nếu là URL tuyệt đối thì chỉ chấp nhận http/https
            if (path.Contains("://"))
            {
                if (!Uri.TryCreate(path, UriKind.Absolute, out var uri) ||
                    (uri.Scheme != Uri.UriSchemeHttp &&
                     uri.Scheme != Uri.UriSchemeHttps))
                {
                    throw new ArgumentException(
                        "Đường dẫn tệp phải là URL http/https " +
                        "hoặc đường dẫn tương đối.");
                }
            }

            return path;
        }

        private async Task ValidateRequestAsync(
            int maLop,
            int? maBuoi,
            int maGiaoVien,
            string tieuDe,
            DateOnly ngayGiao,
            DateOnly? hanNop)
        {
            if (string.IsNullOrWhiteSpace(tieuDe))
            {
                throw new ArgumentException(
                    "Tiêu đề không được để trống.");
            }

            if (!await _lopHocRepository.ExistsByIdAsync(maLop))
            {
                throw new KeyNotFoundException(
                    $"Lớp học có mã {maLop} không tồn tại.");
            }

            var giaoVien =
                await _giaoVienRepository.GetByIdAsync(maGiaoVien);

            if (giaoVien == null)
            {
                throw new KeyNotFoundException(
                    $"Giáo viên có mã {maGiaoVien} không tồn tại.");
            }

            var assignedClasses =
                await _lopHocRepository.GetByGiaoVienIdAsync(maGiaoVien);
            if (!assignedClasses.Any(item => item.MaLop == maLop))
            {
                throw new ArgumentException(
                    "Giáo viên không được phân công lớp đã chọn.");
            }

            if (maBuoi.HasValue)
            {
                var buoi =
                    await _buoiHocRepository.GetByIdAsync(maBuoi.Value);

                if (buoi == null)
                {
                    throw new KeyNotFoundException(
                        $"Buổi học có mã {maBuoi.Value} không tồn tại.");
                }

                if (buoi.MaLop != maLop)
                {
                    throw new ArgumentException(
                        "Buổi học không thuộc lớp đã chọn.");
                }
            }

            if (hanNop.HasValue && hanNop.Value < ngayGiao)
            {
                throw new ArgumentException(
                    "Hạn nộp không được trước ngày giao.");
            }
        }

        private static void ValidatePaging(int pageNumber, int pageSize)
        {
            if (pageNumber <= 0)
                throw new ArgumentException("PageNumber phải lớn hơn 0.");

            if (pageSize <= 0)
                throw new ArgumentException("PageSize phải lớn hơn 0.");

            if (pageSize > 100)
                throw new ArgumentException(
                    "PageSize không được lớn hơn 100.");
        }

        // =====================================================
        // GET
        // =====================================================

        public async Task<IEnumerable<BaiTapResponse>> GetAllAsync()
        {
            var data = await _repository.GetAllAsync();
            return data.Select(MapToResponse);
        }

        public async Task<BaiTapResponse?> GetByIdAsync(int id)
        {
            var entity = await _repository.GetByIdAsync(id);
            return entity == null ? null : MapToResponse(entity);
        }

        public async Task<IEnumerable<BaiTapResponse>> GetByMaLopAsync(
            int maLop)
        {
            if (!await _lopHocRepository.ExistsByIdAsync(maLop))
            {
                throw new KeyNotFoundException(
                    $"Lớp học có mã {maLop} không tồn tại.");
            }

            var data = await _repository.GetByMaLopAsync(maLop);
            return data.Select(MapToResponse);
        }

        public async Task<IEnumerable<BaiTapResponse>> GetByMaGiaoVienAsync(
            int maGiaoVien)
        {
            if (await _giaoVienRepository.GetByIdAsync(maGiaoVien) == null)
            {
                throw new KeyNotFoundException(
                    $"Giáo viên có mã {maGiaoVien} không tồn tại.");
            }

            var data = await _repository.GetByMaGiaoVienAsync(maGiaoVien);
            return data.Select(MapToResponse);
        }

        public async Task<IEnumerable<BaiTapResponse>> GetByMaHocVienAsync(
            int maHocVien)
        {
            if (!await _hocVienRepository.ExistsByIdAsync(maHocVien))
            {
                throw new KeyNotFoundException(
                    $"Học viên có mã {maHocVien} không tồn tại.");
            }

            var data = await _repository.GetByMaHocVienAsync(maHocVien);
            return data.Select(MapToResponse);
        }

        public async Task<IEnumerable<BaiTapSubmissionResponse>>
            GetSubmissionsAsync(int maBaiTap, int maGiaoVien)
        {
            var assignment = await _repository.GetByIdAsync(maBaiTap);
            if (assignment == null)
            {
                throw new KeyNotFoundException(
                    $"Không tìm thấy bài tập có mã {maBaiTap}.");
            }

            if (assignment.MaGiaoVien != maGiaoVien)
            {
                throw new ArgumentException(
                    "Bài tập không thuộc giáo viên đang đăng nhập.");
            }

            var registrations = await _dangKyHocRepository
                .GetByLopIdAsync(assignment.MaLop);
            var grades = await _diemRepository
                .GetByMaBaiTapAsync(maBaiTap);

            var gradeByStudent = grades.ToDictionary(item => item.MaHocVien);
            return registrations
                .Where(item => item.TrangThai == "DangHoc")
                .Select(item =>
                {
                    gradeByStudent.TryGetValue(
                        item.MaHocVien, out var grade);
                    return new BaiTapSubmissionResponse
                    {
                        MaHocVien = item.MaHocVien,
                        HoTenHocVien =
                            item.MaHocVienNavigation.HoTen,
                        TrangThaiNop = grade?.TrangThaiNop ?? "ChuaNop",
                        NgayNop = grade?.NgayNop,
                        GhiChuNop = grade?.GhiChuNop,
                        FilesNop = string.IsNullOrWhiteSpace(grade?.TepNopJson)
                            ? null
                            : JsonSerializer.Deserialize<
                                List<BaiTapSubmissionFileResponse>>(
                                grade.TepNopJson),
                        DiemSo = grade?.DiemSo,
                        NhanXet = grade?.NhanXet
                    };
                })
                .OrderBy(item => item.HoTenHocVien)
                .ToList();
        }

        public async Task<BaiTapResponse> SubmitAsync(
            int maBaiTap,
            int maHocVien,
            string? ghiChu,
            string? tepNopJson)
        {
            var assignment = await _repository.GetByIdAsync(maBaiTap);
            if (assignment == null)
            {
                throw new KeyNotFoundException(
                    $"Không tìm thấy bài tập có mã {maBaiTap}.");
            }

            if (!await _hocVienRepository.ExistsByIdAsync(maHocVien))
            {
                throw new KeyNotFoundException(
                    $"Học viên có mã {maHocVien} không tồn tại.");
            }

            var enrollment = await _dangKyHocRepository
                .GetByHocVienIdAndLopIdAsync(maHocVien, assignment.MaLop);
            if (enrollment?.TrangThai != "DangHoc")
            {
                throw new ArgumentException(
                    "Học viên không đang theo học lớp của bài tập này.");
            }

            var status = assignment.HanNop.HasValue &&
                         DateOnly.FromDateTime(DateTime.Now) >
                         assignment.HanNop.Value
                ? "NopTre"
                : "DaNop";
            var diem = await _diemRepository
                .GetByMaBaiTapAndHocVienAsync(maBaiTap, maHocVien);

            if (diem == null)
            {
                diem = new Diem
                {
                    MaBaiTap = maBaiTap,
                    MaHocVien = maHocVien,
                    TrangThaiNop = status
                };
            }
            else
            {
                diem.TrangThaiNop = status;
            }

            diem.NgayNop = DateTime.Now;
            diem.GhiChuNop = string.IsNullOrWhiteSpace(ghiChu)
                ? null
                : ghiChu.Trim();
            if (!string.IsNullOrWhiteSpace(tepNopJson))
            {
                diem.TepNopJson = tepNopJson;
            }

            if (diem.MaDiem == 0)
            {
                await _diemRepository.AddAsync(diem);
            }
            else
            {
                await _diemRepository.UpdateAsync(diem);
            }

            var result = MapToResponse(assignment);
            result.TrangThaiNop = diem.TrangThaiNop;
            result.DiemSo = diem.DiemSo;
            result.NgayNop = diem.NgayNop;
            result.NhanXet = diem.NhanXet;
            result.GhiChuNop = diem.GhiChuNop;
            result.FilesNop = string.IsNullOrWhiteSpace(diem.TepNopJson)
                ? null
                : JsonSerializer.Deserialize<
                    List<BaiTapSubmissionFileResponse>>(diem.TepNopJson);
            return result;
        }

        public async Task GradeAsync(
            int maBaiTap,
            int maHocVien,
            int maGiaoVien,
            GradeBaiTapRequest request)
        {
            var assignment = await _repository.GetByIdAsync(maBaiTap);
            if (assignment == null)
            {
                throw new KeyNotFoundException(
                    $"Không tìm thấy bài tập có mã {maBaiTap}.");
            }

            if (assignment.MaGiaoVien != maGiaoVien)
            {
                throw new ArgumentException(
                    "Bài tập không thuộc giáo viên đang đăng nhập.");
            }

            var diem = await _diemRepository
                .GetByMaBaiTapAndHocVienAsync(maBaiTap, maHocVien);
            if (diem == null ||
                diem.TrangThaiNop == "ChuaNop" ||
                !diem.NgayNop.HasValue)
            {
                throw new InvalidOperationException(
                    "Chỉ có thể chấm bài sau khi học viên đã nộp.");
            }

            diem.DiemSo = request.DiemSo;
            diem.NhanXet = string.IsNullOrWhiteSpace(request.NhanXet)
                ? null
                : request.NhanXet.Trim();
            await _diemRepository.UpdateAsync(diem);
        }

        public async Task<IEnumerable<BaiTapResponse>> SearchAsync(
            string? keyword)
        {
            var data = await _repository.SearchAsync(keyword);
            return data.Select(MapToResponse);
        }

        // =====================================================
        // PAGED
        // =====================================================

        public Task<object> GetPagedAsync(int pageNumber, int pageSize)
        {
            return SearchPagedAsync(null, pageNumber, pageSize);
        }

        public async Task<object> SearchPagedAsync(
            string? keyword,
            int pageNumber,
            int pageSize)
        {
            ValidatePaging(pageNumber, pageSize);

            var (data, totalItems) = await _repository.SearchPagedAsync(
                keyword,
                pageNumber,
                pageSize);

            return new
            {
                PageNumber = pageNumber,
                PageSize = pageSize,
                TotalItems = totalItems,
                TotalPages =
                    (int)Math.Ceiling((double)totalItems / pageSize),
                Data = data.Select(MapToResponse).ToList()
            };
        }

        // =====================================================
        // CREATE
        // =====================================================

        public async Task<BaiTapResponse> CreateAsync(
            CreateBaiTapRequest request)
        {
            var ngayGiao =
                request.NgayGiao ??
                DateOnly.FromDateTime(DateTime.Today);

            await ValidateRequestAsync(
                request.MaLop,
                request.MaBuoi,
                request.MaGiaoVien,
                request.TieuDe,
                ngayGiao,
                request.HanNop);

            var tieuDe = request.TieuDe.Trim();

            if (await _repository.ExistsByTieuDeAsync(request.MaLop, tieuDe))
            {
                throw new InvalidOperationException(
                    $"Lớp {request.MaLop} đã có bài tập " +
                    $"tên '{tieuDe}'.");
            }

            var entity = new BaiTap
            {
                MaLop = request.MaLop,
                MaBuoi = request.MaBuoi,
                MaGiaoVien = request.MaGiaoVien,
                TieuDe = tieuDe,
                MoTa = string.IsNullOrWhiteSpace(request.MoTa)
                    ? null
                    : request.MoTa.Trim(),
                NgayGiao = ngayGiao,
                HanNop = request.HanNop,
                // Chỉ lưu đường dẫn, không lưu nội dung tệp
                FileDinhKem = NormalizeFilePath(request.FileDinhKem)
            };

            var result = await _repository.AddAsync(entity);
            var created = await _repository.GetByIdAsync(result.MaBaiTap);

            return MapToResponse(created!);
        }

        // =====================================================
        // UPDATE
        // =====================================================

        public async Task<BaiTapResponse?> UpdateAsync(
            int id,
            UpdateBaiTapRequest request)
        {
            var existing = await _repository.GetByIdAsync(id);

            if (existing == null)
            {
                throw new KeyNotFoundException(
                    $"Bài tập có mã {id} không tồn tại.");
            }

            var ngayGiao = request.NgayGiao ?? existing.NgayGiao;

            await ValidateRequestAsync(
                request.MaLop,
                request.MaBuoi,
                request.MaGiaoVien,
                request.TieuDe,
                ngayGiao,
                request.HanNop);

            var tieuDe = request.TieuDe.Trim();

            if (await _repository.ExistsByTieuDeAsync(
                    request.MaLop, tieuDe, id))
            {
                throw new InvalidOperationException(
                    $"Lớp {request.MaLop} đã có bài tập " +
                    $"tên '{tieuDe}'.");
            }

            var updated = new BaiTap
            {
                MaBaiTap = id,
                MaLop = request.MaLop,
                MaBuoi = request.MaBuoi,
                MaGiaoVien = request.MaGiaoVien,
                TieuDe = tieuDe,
                MoTa = string.IsNullOrWhiteSpace(request.MoTa)
                    ? null
                    : request.MoTa.Trim(),
                NgayGiao = ngayGiao,
                HanNop = request.HanNop,
                FileDinhKem = NormalizeFilePath(request.FileDinhKem)
            };

            var result = await _repository.UpdateAsync(updated);

            if (result == null)
                return null;

            var reloaded = await _repository.GetByIdAsync(id);
            return MapToResponse(reloaded!);
        }

        // =====================================================
        // DELETE
        // =====================================================

        public async Task<bool> DeleteAsync(int id)
        {
            if (!await _repository.ExistsByIdAsync(id))
            {
                throw new KeyNotFoundException(
                    $"Bài tập có mã {id} không tồn tại.");
            }

            // Bảng diem có ON DELETE CASCADE nên điểm/bài nộp
            // của bài tập này cũng bị xóa theo
            return await _repository.DeleteAsync(id);
        }
    }
}
