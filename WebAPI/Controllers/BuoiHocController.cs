using BLL.Interfaces;
using DTO.BuoiHoc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace WebAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class BuoiHocController : ControllerBase
    {
        private readonly IBuoiHocService _service;

        public BuoiHocController(IBuoiHocService service)
        {
            _service = service;
        }

        // =====================================================
        // 1. LẤY TẤT CẢ
        // GET: api/BuoiHoc
        // =====================================================

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            try
            {
                var result = await _service.GetAllAsync();

                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(new
                {
                    message = ex.Message
                });
            }
        }

        // =====================================================
        // 2. LẤY THEO ID
        // GET: api/BuoiHoc/1
        // =====================================================

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            try
            {
                var result = await _service.GetByIdAsync(id);

                if (result == null)
                {
                    return NotFound(new
                    {
                        message = $"Buổi học có mã {id} không tồn tại."
                    });
                }

                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(new
                {
                    message = ex.Message
                });
            }
        }

        // =====================================================
        // 3. THEO MÃ LỚP
        // GET: api/BuoiHoc/lop/1
        // =====================================================

        [HttpGet("lop/{maLop}")]
        public async Task<IActionResult> GetByLopId(int maLop)
        {
            try
            {
                var result =
                    await _service.GetByLopIdAsync(maLop);

                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(new
                {
                    message = ex.Message
                });
            }
        }

        // =====================================================
        // 4. THEO NGÀY
        // GET: api/BuoiHoc/ngay/2026-09-01
        // =====================================================

        [HttpGet("ngay/{ngayHoc}")]
        public async Task<IActionResult> GetByDate(
            DateOnly ngayHoc)
        {
            try
            {
                var result =
                    await _service.GetByDateAsync(ngayHoc);

                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(new
                {
                    message = ex.Message
                });
            }
        }

        // =====================================================
        // 5. THEO TRẠNG THÁI HỦY
        // GET: api/BuoiHoc/bi-huy/false
        // =====================================================

        [HttpGet("bi-huy/{biHuy}")]
        public async Task<IActionResult> GetByBiHuy(
            bool biHuy)
        {
            try
            {
                var result =
                    await _service.GetByBiHuyAsync(biHuy);

                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(new
                {
                    message = ex.Message
                });
            }
        }

        // =====================================================
        // 6. THEO NỘI DUNG
        // GET: api/BuoiHoc/noi-dung?noiDung=Unit
        // =====================================================

        [HttpGet("noi-dung")]
        public async Task<IActionResult> GetByNoiDung(
            [FromQuery] string noiDung)
        {
            try
            {
                var result =
                    await _service.GetByNoiDungAsync(noiDung);

                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(new
                {
                    message = ex.Message
                });
            }
        }

        // =====================================================
        // 7. TÌM KIẾM
        // GET: api/BuoiHoc/search
        // =====================================================

        [HttpGet("search")]
        public async Task<IActionResult> Search(
            [FromQuery] int? maLop,
            [FromQuery] string? noiDung,
            [FromQuery] DateOnly? ngayHoc,
            [FromQuery] bool? biHuy)
        {
            try
            {
                var result =
                    await _service.SearchAsync(
                        maLop,
                        noiDung,
                        ngayHoc,
                        biHuy);

                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(new
                {
                    message = ex.Message
                });
            }
        }

        // =====================================================
        // 8. PHÂN TRANG
        // GET: api/BuoiHoc/paged?pageNumber=1&pageSize=10
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
            catch (Exception ex)
            {
                return BadRequest(new
                {
                    message = ex.Message
                });
            }
        }

        // =====================================================
        // 9. THEO LỚP + PHÂN TRANG
        // =====================================================

        [HttpGet("lop/{maLop}/paged")]
        public async Task<IActionResult> GetPagedByLopId(
            int maLop,
            [FromQuery] int pageNumber = 1,
            [FromQuery] int pageSize = 10)
        {
            try
            {
                var result =
                    await _service.GetPagedByLopIdAsync(
                        maLop,
                        pageNumber,
                        pageSize);

                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(new
                {
                    message = ex.Message
                });
            }
        }

        // =====================================================
        // 10. THEO NGÀY + PHÂN TRANG
        // =====================================================

        [HttpGet("ngay/{ngayHoc}/paged")]
        public async Task<IActionResult> GetPagedByDate(
            DateOnly ngayHoc,
            [FromQuery] int pageNumber = 1,
            [FromQuery] int pageSize = 10)
        {
            try
            {
                var result =
                    await _service.GetPagedByDateAsync(
                        ngayHoc,
                        pageNumber,
                        pageSize);

                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(new
                {
                    message = ex.Message
                });
            }
        }

        // =====================================================
        // 11. TÌM KIẾM + PHÂN TRANG
        // =====================================================

        [HttpGet("search/paged")]
        public async Task<IActionResult> SearchPaged(
            [FromQuery] int? maLop,
            [FromQuery] string? noiDung,
            [FromQuery] DateOnly? ngayHoc,
            [FromQuery] bool? biHuy,
            [FromQuery] int pageNumber = 1,
            [FromQuery] int pageSize = 10)
        {
            try
            {
                var result =
                    await _service.SearchPagedAsync(
                        maLop,
                        noiDung,
                        ngayHoc,
                        biHuy,
                        pageNumber,
                        pageSize);

                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(new
                {
                    message = ex.Message
                });
            }
        }

        // =====================================================
        // 12. TẠO
        // POST: api/BuoiHoc
        // =====================================================

        [HttpPost]
        public async Task<IActionResult> Create(
            [FromBody] CreateBuoiHocRequest request)
        {
            try
            {
                var result =
                    await _service.CreateAsync(request);

                return CreatedAtAction(
                    nameof(GetById),
                    new { id = result.MaBuoi },
                    result);
            }
            catch (Exception ex)
            {
                return BadRequest(new
                {
                    message = ex.Message
                });
            }
        }

        // =====================================================
        // 13. CẬP NHẬT
        // PUT: api/BuoiHoc/1
        // =====================================================

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(
            int id,
            [FromBody] UpdateBuoiHocRequest request)
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
                            $"Buổi học có mã {id} không tồn tại."
                    });
                }

                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(new
                {
                    message = ex.Message
                });
            }
        }

        // =====================================================
        // 14. XÓA
        // DELETE: api/BuoiHoc/1
        // =====================================================

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
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
                            $"Buổi học có mã {id} không tồn tại."
                    });
                }

                return Ok(new
                {
                    message = "Xóa buổi học thành công."
                });
            }
            catch (Exception ex)
            {
                return BadRequest(new
                {
                    message = ex.Message
                });
            }
        }
    }
}