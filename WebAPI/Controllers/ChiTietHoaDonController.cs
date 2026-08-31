using BLL.Interfaces;
using DTO.ChiTietHoaDon;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace WebAPI.Controllers
{
    /// <summary>
    /// API quản lý chi tiết hóa đơn.
    /// </summary>
    [Authorize(Roles = "Admin")]
    [ApiController]
    [Route("api/[controller]")]
    public class ChiTietHoaDonController : ControllerBase
    {
        private readonly IChiTietHoaDonService _service;

        public ChiTietHoaDonController(IChiTietHoaDonService service)
        {
            _service = service;
        }

        /// <summary>
        /// Lấy danh sách chi tiết hóa đơn theo mã hóa đơn.
        /// </summary>
        /// <param name="maHoaDon">Mã hóa đơn cần lấy chi tiết.</param>
        /// <returns>Danh sách chi tiết hóa đơn.</returns>
        [HttpGet("hoa-don/{maHoaDon:int}")]
        public async Task<IActionResult> GetByMaHoaDon(int maHoaDon)
        {
            try
            {
                var result = await _service.GetByMaHoaDonAsync(maHoaDon);
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
                return StatusCode(500, new
                {
                    message = "Lỗi khi lấy chi tiết hóa đơn theo hóa đơn.",
                    detail = ex.Message
                });
            }
        }

        /// <summary>
        /// Lấy thông tin chi tiết một dòng hóa đơn theo mã chi tiết.
        /// </summary>
        /// <param name="id">Mã chi tiết hóa đơn.</param>
        /// <returns>Thông tin chi tiết hóa đơn nếu tìm thấy.</returns>
        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetById(int id)
        {
            try
            {
                var result = await _service.GetByIdAsync(id);

                if (result == null)
                {
                    return NotFound(new
                    {
                        message = $"Không tìm thấy chi tiết hóa đơn có mã {id}."
                    });
                }

                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new
                {
                    message = "Lỗi khi lấy chi tiết hóa đơn.",
                    detail = ex.Message
                });
            }
        }

        /// <summary>
        /// Tạo mới một dòng chi tiết hóa đơn.
        /// </summary>
        /// <param name="request">Dữ liệu tạo chi tiết hóa đơn.</param>
        /// <returns>Chi tiết hóa đơn vừa tạo.</returns>
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateChiTietHoaDonRequest request)
        {
            try
            {
                var result = await _service.CreateAsync(request);
                return StatusCode(StatusCodes.Status201Created, result);
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
            catch (Exception ex)
            {
                return StatusCode(500, new
                {
                    message = "Lỗi khi tạo chi tiết hóa đơn.",
                    detail = ex.Message
                });
            }
        }

        /// <summary>
        /// Cập nhật một dòng chi tiết hóa đơn theo mã chi tiết.
        /// </summary>
        /// <param name="id">Mã chi tiết hóa đơn cần cập nhật.</param>
        /// <param name="request">Dữ liệu cập nhật chi tiết hóa đơn.</param>
        /// <returns>Chi tiết hóa đơn sau khi cập nhật.</returns>
        [HttpPut("{id:int}")]
        public async Task<IActionResult> Update(int id, [FromBody] UpdateChiTietHoaDonRequest request)
        {
            try
            {
                var result = await _service.UpdateAsync(id, request);

                if (result == null)
                {
                    return NotFound(new
                    {
                        message = $"Không tìm thấy chi tiết hóa đơn có mã {id}."
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
            catch (Exception ex)
            {
                return StatusCode(500, new
                {
                    message = "Lỗi khi cập nhật chi tiết hóa đơn.",
                    detail = ex.Message
                });
            }
        }

        /// <summary>
        /// Xóa một dòng chi tiết hóa đơn theo mã chi tiết.
        /// </summary>
        /// <param name="id">Mã chi tiết hóa đơn cần xóa.</param>
        /// <returns>Thông báo xác nhận xóa.</returns>
        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                var result = await _service.DeleteAsync(id);

                if (!result)
                {
                    return NotFound(new
                    {
                        message = $"Không tìm thấy chi tiết hóa đơn có mã {id}."
                    });
                }

                return Ok(new
                {
                    message = "Xóa chi tiết hóa đơn thành công."
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new
                {
                    message = "Lỗi khi xóa chi tiết hóa đơn.",
                    detail = ex.Message
                });
            }
        }
    }
}
