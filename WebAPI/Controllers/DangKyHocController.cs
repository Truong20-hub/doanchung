using BLL.Interfaces;
using DTO.DangKyHoc;
using Microsoft.AspNetCore.Mvc;

namespace WebAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class DangKyHocController : ControllerBase
    {
        private readonly IDangKyHocService _service;

        public DangKyHocController(
            IDangKyHocService service)
        {
            _service = service;
        }

        // =====================================================
        // 1. GET ALL
        // GET: api/DangKyHoc
        // =====================================================

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            try
            {
                var result =
                    await _service.GetAllAsync();

                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(
                    500,
                    new
                    {
                        message =
                            "Đã xảy ra lỗi khi lấy danh sách đăng ký học.",
                        detail = ex.Message
                    });
            }
        }

        // =====================================================
        // 2. GET BY ID
        // GET: api/DangKyHoc/1
        // =====================================================

        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetById(
            int id)
        {
            try
            {
                var result =
                    await _service.GetByIdAsync(id);

                if (result == null)
                {
                    return NotFound(new
                    {
                        message =
                            $"Đăng ký học có mã {id} không tồn tại."
                    });
                }

                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(
                    500,
                    new
                    {
                        message =
                            "Đã xảy ra lỗi khi lấy đăng ký học.",
                        detail = ex.Message
                    });
            }
        }

        // =====================================================
        // 3. GET BY HỌC VIÊN
        // GET: api/DangKyHoc/hoc-vien/1
        // =====================================================

        [HttpGet("hoc-vien/{maHocVien:int}")]
        public async Task<IActionResult> GetByHocVienId(
            int maHocVien)
        {
            try
            {
                var result =
                    await _service.GetByHocVienIdAsync(
                        maHocVien);

                return Ok(result);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new
                {
                    message = ex.Message
                });
            }
            catch (Exception ex)
            {
                return StatusCode(
                    500,
                    new
                    {
                        message =
                            "Lỗi khi lấy đăng ký của học viên.",
                        detail = ex.Message
                    });
            }
        }

        // =====================================================
        // 4. GET BY LỚP
        // GET: api/DangKyHoc/lop/1
        // =====================================================

        [HttpGet("lop/{maLop:int}")]
        public async Task<IActionResult> GetByLopId(
            int maLop)
        {
            try
            {
                var result =
                    await _service.GetByLopIdAsync(
                        maLop);

                return Ok(result);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new
                {
                    message = ex.Message
                });
            }
            catch (Exception ex)
            {
                return StatusCode(
                    500,
                    new
                    {
                        message =
                            "Lỗi khi lấy danh sách học viên của lớp.",
                        detail = ex.Message
                    });
            }
        }

        // =====================================================
        // 5. GET BY TRẠNG THÁI
        // GET: api/DangKyHoc/trang-thai?trangThai=DangHoc
        // =====================================================

        [HttpGet("trang-thai")]
        public async Task<IActionResult> GetByTrangThai(
            [FromQuery] string trangThai)
        {
            try
            {
                var result =
                    await _service.GetByTrangThaiAsync(
                        trangThai);

                return Ok(result);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new
                {
                    message = ex.Message
                });
            }
            catch (Exception ex)
            {
                return StatusCode(
                    500,
                    new
                    {
                        message =
                            "Lỗi khi lấy đăng ký theo trạng thái.",
                        detail = ex.Message
                    });
            }
        }

        // =====================================================
        // 6. CREATE
        // POST: api/DangKyHoc
        // =====================================================

        [HttpPost]
        public async Task<IActionResult> Create(
            [FromBody] CreateDangKyHocRequest request)
        {
            try
            {
                var result =
                    await _service.CreateAsync(request);

                return CreatedAtAction(
                    nameof(GetById),
                    new
                    {
                        id = result.MaDangKy
                    },
                    result);
            }
            catch (KeyNotFoundException ex)
            {
                // Học viên hoặc lớp không tồn tại
                return NotFound(new
                {
                    message = ex.Message
                });
            }
            catch (InvalidOperationException ex)
            {
                // Trùng học viên + lớp
                // hoặc vi phạm nghiệp vụ
                return Conflict(new
                {
                    message = ex.Message
                });
            }
            catch (ArgumentException ex)
            {
                // Trạng thái không hợp lệ
                return BadRequest(new
                {
                    message = ex.Message
                });
            }
            catch (Exception ex)
            {
                return StatusCode(
                    500,
                    new
                    {
                        message =
                            "Không thể tạo đăng ký học.",
                        detail = ex.Message,
                        innerException =
                            ex.InnerException?.Message
                    });
            }
        }

        // =====================================================
        // 7. UPDATE
        // PUT: api/DangKyHoc/1
        // =====================================================

        [HttpPut("{id:int}")]
        public async Task<IActionResult> Update(
            int id,
            [FromBody] UpdateDangKyHocRequest request)
        {
            try
            {
                var result =
                    await _service.UpdateAsync(
                        id,
                        request);

                if (result == null)
                {
                    return NotFound(new
                    {
                        message =
                            $"Đăng ký học có mã {id} không tồn tại."
                    });
                }

                return Ok(result);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new
                {
                    message = ex.Message
                });
            }
            catch (InvalidOperationException ex)
            {
                return Conflict(new
                {
                    message = ex.Message
                });
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new
                {
                    message = ex.Message
                });
            }
            catch (Exception ex)
            {
                return StatusCode(
                    500,
                    new
                    {
                        message =
                            "Không thể cập nhật đăng ký học.",
                        detail = ex.Message,
                        innerException =
                            ex.InnerException?.Message
                    });
            }
        }

        // =====================================================
        // 8. DELETE
        // DELETE: api/DangKyHoc/1
        // =====================================================

        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Delete(
            int id)
        {
            try
            {
                var result =
                    await _service.DeleteAsync(id);

                if (!result)
                {
                    return NotFound(new
                    {
                        message =
                            $"Đăng ký học có mã {id} không tồn tại."
                    });
                }

                return Ok(new
                {
                    message =
                        "Xóa đăng ký học thành công."
                });
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new
                {
                    message = ex.Message
                });
            }
            catch (InvalidOperationException ex)
            {
                return Conflict(new
                {
                    message = ex.Message
                });
            }
            catch (Exception ex)
            {
                return StatusCode(
                    500,
                    new
                    {
                        message =
                            "Không thể xóa đăng ký học.",
                        detail = ex.Message,
                        innerException =
                            ex.InnerException?.Message
                    });
            }
        }

        // =====================================================
        // 9. SEARCH
        // GET:
        // api/DangKyHoc/search?
        // maHocVien=1&
        // maLop=2&
        // trangThai=DangHoc
        // =====================================================

        [HttpGet("search")]
        public async Task<IActionResult> Search(
            [FromQuery] int? maHocVien,
            [FromQuery] int? maLop,
            [FromQuery] string? trangThai)
        {
            try
            {
                var result =
                    await _service.SearchAsync(
                        maHocVien,
                        maLop,
                        trangThai);

                return Ok(result);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new
                {
                    message = ex.Message
                });
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new
                {
                    message = ex.Message
                });
            }
            catch (Exception ex)
            {
                return StatusCode(
                    500,
                    new
                    {
                        message =
                            "Lỗi khi tìm kiếm đăng ký học.",
                        detail = ex.Message
                    });
            }
        }

        // =====================================================
        // 10. PAGING
        // GET:
        // api/DangKyHoc/paged?
        // pageNumber=1&
        // pageSize=10
        // =====================================================

        [HttpGet("paged")]
        public async Task<IActionResult> GetPaged(
            [FromQuery] int pageNumber = 1,
            [FromQuery] int pageSize = 10)
        {
            try
            {
                var result =
                    await _service.GetPagedAsync(
                        pageNumber,
                        pageSize);

                return Ok(result);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new
                {
                    message = ex.Message
                });
            }
            catch (Exception ex)
            {
                return StatusCode(
                    500,
                    new
                    {
                        message =
                            "Lỗi khi phân trang đăng ký học.",
                        detail = ex.Message
                    });
            }
        }

        // =====================================================
        // 11. SEARCH + PAGING
        // =====================================================

        [HttpGet("search-paged")]
        public async Task<IActionResult> SearchPaged(
            [FromQuery] int? maHocVien,
            [FromQuery] int? maLop,
            [FromQuery] string? trangThai,
            [FromQuery] int pageNumber = 1,
            [FromQuery] int pageSize = 10)
        {
            try
            {
                var result =
                    await _service.SearchPagedAsync(
                        maHocVien,
                        maLop,
                        trangThai,
                        pageNumber,
                        pageSize);

                return Ok(result);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new
                {
                    message = ex.Message
                });
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new
                {
                    message = ex.Message
                });
            }
            catch (Exception ex)
            {
                return StatusCode(
                    500,
                    new
                    {
                        message =
                            "Lỗi khi tìm kiếm và phân trang.",
                        detail = ex.Message
                    });
            }
        }

        // =====================================================
        // 12. PAGING THEO HỌC VIÊN
        // =====================================================

        [HttpGet("hoc-vien/{maHocVien:int}/paged")]
        public async Task<IActionResult> GetPagedByHocVien(
            int maHocVien,
            [FromQuery] int pageNumber = 1,
            [FromQuery] int pageSize = 10)
        {
            try
            {
                var result =
                    await _service
                        .GetPagedByHocVienIdAsync(
                            maHocVien,
                            pageNumber,
                            pageSize);

                return Ok(result);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new
                {
                    message = ex.Message
                });
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new
                {
                    message = ex.Message
                });
            }
            catch (Exception ex)
            {
                return StatusCode(
                    500,
                    new
                    {
                        message =
                            "Lỗi khi phân trang theo học viên.",
                        detail = ex.Message
                    });
            }
        }

        // =====================================================
        // 13. PAGING THEO LỚP
        // =====================================================

        [HttpGet("lop/{maLop:int}/paged")]
        public async Task<IActionResult> GetPagedByLop(
            int maLop,
            [FromQuery] int pageNumber = 1,
            [FromQuery] int pageSize = 10)
        {
            try
            {
                var result =
                    await _service
                        .GetPagedByLopIdAsync(
                            maLop,
                            pageNumber,
                            pageSize);

                return Ok(result);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new
                {
                    message = ex.Message
                });
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new
                {
                    message = ex.Message
                });
            }
            catch (Exception ex)
            {
                return StatusCode(
                    500,
                    new
                    {
                        message =
                            "Lỗi khi phân trang theo lớp.",
                        detail = ex.Message
                    });
            }
        }

        // =====================================================
        // 14. PAGING THEO TRẠNG THÁI
        // =====================================================

        [HttpGet("trang-thai/paged")]
        public async Task<IActionResult> GetPagedByTrangThai(
            [FromQuery] string trangThai,
            [FromQuery] int pageNumber = 1,
            [FromQuery] int pageSize = 10)
        {
            try
            {
                var result =
                    await _service
                        .GetPagedByTrangThaiAsync(
                            trangThai,
                            pageNumber,
                            pageSize);

                return Ok(result);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new
                {
                    message = ex.Message
                });
            }
            catch (Exception ex)
            {
                return StatusCode(
                    500,
                    new
                    {
                        message =
                            "Lỗi khi phân trang theo trạng thái.",
                        detail = ex.Message
                    });
            }
        }
    }
}