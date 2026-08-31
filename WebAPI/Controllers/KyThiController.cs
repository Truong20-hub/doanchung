using BLL.Interfaces;
using DTO.KyThi;
using Microsoft.AspNetCore.Mvc;

namespace WebAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class KyThiController : ControllerBase
    {
        private readonly IKyThiService _service;

        public KyThiController(
            IKyThiService service)
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
                        "Lỗi khi lấy danh sách kỳ thi.",
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
                            $"Không tìm thấy kỳ thi có mã {id}."
                    });
                }

                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new
                {
                    message =
                        "Lỗi khi lấy kỳ thi.",
                    detail = ex.Message
                });
            }
        }

        // =====================================================
        // GET BY LOP
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
                        "Lỗi khi lấy kỳ thi theo lớp.",
                    detail = ex.Message
                });
            }
        }

        // =====================================================
        // GET BY LOAI
        // =====================================================

        [HttpGet("loai/{loaiKyThi}")]
        public async Task<IActionResult> GetByLoaiKyThi(
            string loaiKyThi)
        {
            try
            {
                return Ok(
                    await _service
                        .GetByLoaiKyThiAsync(
                            loaiKyThi));
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
                        "Lỗi khi lấy kỳ thi.",
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
                        "Lỗi khi tìm kiếm kỳ thi.",
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
                    await _service
                        .GetPagedAsync(
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
                        "Lỗi khi phân trang kỳ thi.",
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
            [FromBody] CreateKyThiRequest request)
        {
            try
            {
                var result =
                    await _service.CreateAsync(request);

                return CreatedAtAction(
                    nameof(GetById),
                    new
                    {
                        id = result.MaKyThi
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
                        "Có lỗi xảy ra khi thêm kỳ thi.",
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
            [FromBody] UpdateKyThiRequest request)
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
                            $"Không tìm thấy kỳ thi có mã {id}."
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
                        "Có lỗi xảy ra khi cập nhật kỳ thi.",
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
                        $"Đã xóa kỳ thi có mã {id} thành công."
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
                        "Có lỗi xảy ra khi xóa kỳ thi.",
                    detail = ex.Message
                });
            }
        }
    }
}