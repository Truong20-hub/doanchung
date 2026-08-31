using BLL.Interfaces;
using DTO.ThanhToan;
using Microsoft.AspNetCore.Mvc;

namespace WebAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ThanhToanController : ControllerBase
    {
        private readonly IThanhToanService _service;

        public ThanhToanController(
            IThanhToanService service)
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
                        "Lỗi khi lấy danh sách thanh toán.",
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
                            $"Không tìm thấy thanh toán có mã {id}."
                    });
                }

                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new
                {
                    message =
                        "Lỗi khi lấy thanh toán.",
                    detail = ex.Message
                });
            }
        }

        // =====================================================
        // GET BY HÓA ĐƠN
        // =====================================================

        [HttpGet("hoadon/{maHoaDon:int}")]
        public async Task<IActionResult>
            GetByMaHoaDon(
                int maHoaDon)
        {
            try
            {
                return Ok(
                    await _service
                        .GetByMaHoaDonAsync(
                            maHoaDon));
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
                        "Lỗi khi lấy thanh toán theo hóa đơn.",
                    detail = ex.Message
                });
            }
        }

        // =====================================================
        // PAGED
        // =====================================================

        [HttpGet("paged")]
        public async Task<IActionResult>
            GetPaged(
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
                        "Lỗi khi phân trang thanh toán.",
                    detail = ex.Message
                });
            }
        }

        // =====================================================
        // CREATE
        // =====================================================

        [HttpPost]
        public async Task<IActionResult> Create(
            [FromBody]
            CreateThanhToanRequest request)
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
                        id = result.MaThanhToan
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
                        "Có lỗi xảy ra khi thêm thanh toán.",
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
            [FromBody]
            UpdateThanhToanRequest request)
        {
            try
            {
                var result =
                    await _service
                        .UpdateAsync(
                            id,
                            request);

                if (result == null)
                {
                    return NotFound(new
                    {
                        message =
                            $"Không tìm thấy thanh toán có mã {id}."
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
                        "Có lỗi xảy ra khi cập nhật thanh toán.",
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
                        $"Đã xóa thanh toán có mã {id} thành công."
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
                        "Có lỗi xảy ra khi xóa thanh toán.",
                    detail = ex.Message
                });
            }
        }
    }
}