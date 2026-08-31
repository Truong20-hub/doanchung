using BLL.Interfaces;
using DTO.LichHoc;
using Microsoft.AspNetCore.Mvc;

namespace WebAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class LichHocController : ControllerBase
    {
        private readonly ILichHocService _service;

        public LichHocController(
            ILichHocService service)
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
                var result =
                    await _service.GetAllAsync();

                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new
                {
                    message = "Lỗi khi lấy danh sách lịch học.",
                    detail = ex.Message
                });
            }
        }

        // =====================================================
        // GET BY ID
        // =====================================================

        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetById(int id)
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
                            $"Không tìm thấy lịch học có mã {id}."
                    });
                }

                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new
                {
                    message = "Lỗi khi lấy lịch học.",
                    detail = ex.Message
                });
            }
        }

        // =====================================================
        // GET BY MA LOP
        // =====================================================

        [HttpGet("lop/{maLop:int}")]
        public async Task<IActionResult> GetByMaLop(
            int maLop)
        {
            try
            {
                return Ok(
                    await _service.GetByMaLopAsync(maLop));
            }
            catch (Exception ex)
            {
                return StatusCode(500, new
                {
                    message = "Lỗi khi lấy lịch học theo lớp.",
                    detail = ex.Message
                });
            }
        }

        // =====================================================
        // GET BY THU
        // =====================================================

        [HttpGet("thu/{thu}")]
        public async Task<IActionResult> GetByThu(
            string thu)
        {
            try
            {
                return Ok(
                    await _service.GetByThuAsync(thu));
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
                    message = "Lỗi khi lấy lịch học.",
                    detail = ex.Message
                });
            }
        }

        // =====================================================
        // GET BY LOP + THU
        // =====================================================

        [HttpGet("lop/{maLop:int}/thu/{thu}")]
        public async Task<IActionResult> GetByMaLopAndThu(
            int maLop,
            string thu)
        {
            try
            {
                return Ok(
                    await _service
                        .GetByMaLopAndThuAsync(
                            maLop,
                            thu));
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
                    message = "Lỗi khi tìm lịch học.",
                    detail = ex.Message
                });
            }
        }

        // =====================================================
        // CREATE
        // =====================================================

        [HttpPost]
        public async Task<IActionResult> Create(
            [FromBody] CreateLichHocRequest request)
        {
            try
            {
                var result =
                    await _service.CreateAsync(request);

                return CreatedAtAction(
                    nameof(GetById),
                    new { id = result.MaLich },
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
                        "Có lỗi xảy ra khi thêm lịch học.",
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
            [FromBody] UpdateLichHocRequest request)
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
                            $"Không tìm thấy lịch học có mã {id}."
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
                        "Có lỗi xảy ra khi cập nhật lịch học.",
                    detail = ex.Message
                });
            }
        }

        // =====================================================
        // DELETE
        // =====================================================

        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                await _service.DeleteAsync(id);

                return Ok(new
                {
                    message =
                        $"Đã xóa lịch học có mã {id} thành công."
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
                        "Có lỗi xảy ra khi xóa lịch học.",
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
                    message = "Lỗi khi phân trang.",
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
                    await _service.SearchAsync(keyword));
            }
            catch (Exception ex)
            {
                return StatusCode(500, new
                {
                    message = "Lỗi khi tìm kiếm.",
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
                    await _service.SearchPagedAsync(
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
    }
}