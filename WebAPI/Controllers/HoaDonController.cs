using BLL.Interfaces;
using DTO.HoaDon;
using Microsoft.AspNetCore.Mvc;

namespace WebAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class HoaDonController : ControllerBase
    {
        private readonly IHoaDonService _service;

        public HoaDonController(
            IHoaDonService service)
        {
            _service = service;
        }

        // =====================================================
        // GET ALL
        // =====================================================

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            try
            {
                return Ok(
                    await _service.GetAllAsync());
            }
            catch (Exception ex)
            {
                return StatusCode(500, new
                {
                    message =
                        "Lỗi khi lấy danh sách hóa đơn.",
                    detail = ex.Message
                });
            }
        }

        // =====================================================
        // GET BY ID
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
                            $"Không tìm thấy hóa đơn có mã {id}."
                    });
                }

                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new
                {
                    message =
                        "Lỗi khi lấy hóa đơn.",
                    detail = ex.Message
                });
            }
        }

        // =====================================================
        // GET BY HỌC VIÊN
        // =====================================================

        [HttpGet("hocvien/{maHocVien:int}")]
        public async Task<IActionResult> GetByMaHocVien(
            int maHocVien)
        {
            try
            {
                return Ok(
                    await _service
                        .GetByMaHocVienAsync(
                            maHocVien));
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
                return StatusCode(500, new
                {
                    message =
                        "Lỗi khi lấy hóa đơn theo học viên.",
                    detail = ex.Message
                });
            }
        }

        // =====================================================
        // GET BY LỚP
        // =====================================================

        [HttpGet("lop/{maLop:int}")]
        public async Task<IActionResult> GetByMaLop(
            int maLop)
        {
            try
            {
                return Ok(
                    await _service
                        .GetByMaLopAsync(maLop));
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
                return StatusCode(500, new
                {
                    message =
                        "Lỗi khi lấy hóa đơn theo lớp.",
                    detail = ex.Message
                });
            }
        }

        // =====================================================
        // GET BY TRẠNG THÁI
        // =====================================================

        [HttpGet("trang-thai/{trangThai}")]
        public async Task<IActionResult> GetByTrangThai(
            string trangThai)
        {
            try
            {
                return Ok(
                    await _service
                        .GetByTrangThaiAsync(
                            trangThai));
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
                return StatusCode(500, new
                {
                    message =
                        "Lỗi khi lấy hóa đơn.",
                    detail = ex.Message
                });
            }
        }

        // =====================================================
        // SEARCH
        // =====================================================

        [HttpGet("search")]
        public async Task<IActionResult> Search(
            [FromQuery] string? keyword)
        {
            try
            {
                return Ok(
                    await _service
                        .SearchAsync(keyword));
            }
            catch (Exception ex)
            {
                return StatusCode(500, new
                {
                    message =
                        "Lỗi khi tìm kiếm hóa đơn.",
                    detail = ex.Message
                });
            }
        }

        // =====================================================
        // PAGED
        // =====================================================

        [HttpGet("paged")]
        public async Task<IActionResult> GetPaged(
            [FromQuery] int pageNumber = 1,
            [FromQuery] int pageSize = 10)
        {
            try
            {
                return Ok(
                    await _service.GetPagedAsync(
                        pageNumber,
                        pageSize));
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
                return StatusCode(500, new
                {
                    message =
                        "Lỗi khi phân trang hóa đơn.",
                    detail = ex.Message
                });
            }
        }

        // =====================================================
        // SEARCH + PAGED
        // =====================================================

        [HttpGet("search-paged")]
        public async Task<IActionResult> SearchPaged(
            [FromQuery] string? keyword,
            [FromQuery] int pageNumber = 1,
            [FromQuery] int pageSize = 10)
        {
            try
            {
                return Ok(
                    await _service
                        .SearchPagedAsync(
                            keyword,
                            pageNumber,
                            pageSize));
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
                return StatusCode(500, new
                {
                    message =
                        "Lỗi khi tìm kiếm và phân trang.",
                    detail = ex.Message
                });
            }
        }

        // =====================================================
        // CREATE
        // =====================================================

        [HttpPost]
        public async Task<IActionResult> Create(
            [FromBody] CreateHoaDonRequest request)
        {
            try
            {
                var result =
                    await _service
                        .CreateAsync(request);

                return CreatedAtAction(
                    nameof(GetById),
                    new
                    {
                        id = result.MaHoaDon
                    },
                    result);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new
                {
                    message = ex.Message
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
                return StatusCode(500, new
                {
                    message =
                        "Có lỗi xảy ra khi thêm hóa đơn.",
                    detail = ex.Message
                });
            }
        }

        // =====================================================
        // UPDATE
        // =====================================================

        [HttpPut("{id:int}")]
        public async Task<IActionResult> Update(
            int id,
            [FromBody] UpdateHoaDonRequest request)
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
                            $"Không tìm thấy hóa đơn có mã {id}."
                    });
                }

                return Ok(result);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new
                {
                    message = ex.Message
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
                return StatusCode(500, new
                {
                    message =
                        "Có lỗi xảy ra khi cập nhật hóa đơn.",
                    detail = ex.Message
                });
            }
        }

        // =====================================================
        // DELETE
        // =====================================================

        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Delete(
            int id)
        {
            try
            {
                await _service.DeleteAsync(id);

                return Ok(new
                {
                    message =
                        $"Đã xóa hóa đơn có mã {id} thành công."
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
                return StatusCode(500, new
                {
                    message =
                        "Có lỗi xảy ra khi xóa hóa đơn.",
                    detail = ex.Message
                });
            }
        }
    }
}