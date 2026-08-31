using BLL.Interfaces;
using DTO.DiemDanh;
using Microsoft.AspNetCore.Mvc;

namespace WebAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class DiemDanhController : ControllerBase
    {
        private readonly IDiemDanhService _service;

        public DiemDanhController(
            IDiemDanhService service)
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
                    message =
                        "Lỗi khi lấy danh sách điểm danh.",
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
                            $"Không tìm thấy điểm danh có mã {id}."
                    });
                }

                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new
                {
                    message =
                        "Lỗi khi lấy điểm danh.",
                    detail = ex.Message
                });
            }
        }

        // =====================================================
        // GET BY BUỔI
        // =====================================================

        [HttpGet("buoi/{maBuoi:int}")]
        public async Task<IActionResult>
            GetByMaBuoi(int maBuoi)
        {
            try
            {
                var result =
                    await _service
                        .GetByMaBuoiAsync(
                            maBuoi);

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
                    message =
                        "Lỗi khi lấy điểm danh theo buổi.",
                    detail = ex.Message
                });
            }
        }

        // =====================================================
        // GET BY HỌC VIÊN
        // =====================================================

        [HttpGet("hocvien/{maHocVien:int}")]
        public async Task<IActionResult>
            GetByMaHocVien(
                int maHocVien)
        {
            try
            {
                var result =
                    await _service
                        .GetByMaHocVienAsync(
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
                return StatusCode(500, new
                {
                    message =
                        "Lỗi khi lấy điểm danh theo học viên.",
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
                var result =
                    await _service
                        .GetPagedAsync(
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
                return StatusCode(500, new
                {
                    message =
                        "Lỗi khi phân trang điểm danh.",
                    detail = ex.Message
                });
            }
        }

        // =====================================================
        // SEARCH + PAGED
        // =====================================================

        [HttpGet("search-paged")]
        public async Task<IActionResult>
            SearchPaged(
                [FromQuery] int? maBuoi,
                [FromQuery] int? maHocVien,
                [FromQuery] string? trangThai,
                [FromQuery] int pageNumber = 1,
                [FromQuery] int pageSize = 10)
        {
            try
            {
                var result =
                    await _service
                        .SearchPagedAsync(
                            maBuoi,
                            maHocVien,
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
                return StatusCode(500, new
                {
                    message =
                        "Lỗi khi tìm kiếm điểm danh.",
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
            CreateDiemDanhRequest request)
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
                        id = result.MaDiemDanh
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
                        "Có lỗi xảy ra khi thêm điểm danh.",
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
            UpdateDiemDanhRequest request)
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
                            $"Không tìm thấy điểm danh có mã {id}."
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
                        "Có lỗi xảy ra khi cập nhật điểm danh.",
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
                var result =
                    await _service
                        .DeleteAsync(id);

                if (!result)
                {
                    return NotFound(new
                    {
                        message =
                            $"Không tìm thấy điểm danh có mã {id}."
                    });
                }

                return Ok(new
                {
                    message =
                        $"Đã xóa điểm danh có mã {id} thành công."
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
                        "Có lỗi xảy ra khi xóa điểm danh.",
                    detail = ex.Message
                });
            }
        }
    }
}