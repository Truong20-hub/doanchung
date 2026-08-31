using BLL.Interfaces;
using DTO.KhoaHoc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class KhoaHocController : ControllerBase
    {
        private readonly IKhoaHocService _khoaHocService;

        public KhoaHocController(
            IKhoaHocService khoaHocService)
        {
            _khoaHocService = khoaHocService;
        }


        // =====================================================
        // GET: api/KhoaHoc
        // =====================================================

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var result =
                await _khoaHocService.GetAllAsync();

            return Ok(result);
        }


        // =====================================================
        // GET: api/KhoaHoc/5
        // =====================================================

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var result =
                await _khoaHocService.GetByIdAsync(id);

            if (result == null)
            {
                return NotFound(
                    $"Không tìm thấy khóa học có mã {id}.");
            }

            return Ok(result);
        }


        // =====================================================
        // GET: api/KhoaHoc/ten?tenKhoaHoc=IELTS
        // =====================================================

        [HttpGet("ten")]
        public async Task<IActionResult> GetByName(
            [FromQuery] string tenKhoaHoc)
        {
            var result =
                await _khoaHocService.GetByNameAsync(
                    tenKhoaHoc);

            return Ok(result);
        }


        // =====================================================
        // GET: api/KhoaHoc/trinh-do?trinhDo=A1
        // =====================================================

        [HttpGet("trinh-do")]
        public async Task<IActionResult> GetByTrinhDo(
            [FromQuery] string trinhDo)
        {
            var result =
                await _khoaHocService.GetByTrinhDoAsync(
                    trinhDo);

            return Ok(result);
        }


        // =====================================================
        // GET: api/KhoaHoc/hoc-phi?hocPhi=5000000
        // =====================================================

        [HttpGet("hoc-phi")]
        public async Task<IActionResult> GetByHocPhi(
            [FromQuery] decimal hocPhi)
        {
            var result =
                await _khoaHocService.GetByHocPhiAsync(
                    hocPhi);

            return Ok(result);
        }


        // =====================================================
        // GET: api/KhoaHoc/paging
        // =====================================================

        [HttpGet("paging")]
        public async Task<IActionResult> GetPaged(
            [FromQuery] int pageNumber = 1,
            [FromQuery] int pageSize = 10)
        {
            var result =
                await _khoaHocService.GetPagedAsync(
                    pageNumber,
                    pageSize);

            return Ok(result);
        }


        // =====================================================
        // GET: api/KhoaHoc/paging/ten
        // =====================================================

        [HttpGet("paging/ten")]
        public async Task<IActionResult> GetPagedByName(
            [FromQuery] string tenKhoaHoc,
            [FromQuery] int pageNumber = 1,
            [FromQuery] int pageSize = 10)
        {
            var result =
                await _khoaHocService.GetPagedByNameAsync(
                    tenKhoaHoc,
                    pageNumber,
                    pageSize);

            return Ok(result);
        }


        // =====================================================
        // GET: api/KhoaHoc/paging/trinh-do
        // =====================================================

        [HttpGet("paging/trinh-do")]
        public async Task<IActionResult> GetPagedByTrinhDo(
            [FromQuery] string trinhDo,
            [FromQuery] int pageNumber = 1,
            [FromQuery] int pageSize = 10)
        {
            var result =
                await _khoaHocService.GetPagedByTrinhDoAsync(
                    trinhDo,
                    pageNumber,
                    pageSize);

            return Ok(result);
        }


        // =====================================================
        // GET: api/KhoaHoc/paging/hoc-phi
        // =====================================================

        [HttpGet("paging/hoc-phi")]
        public async Task<IActionResult> GetPagedByHocPhi(
            [FromQuery] decimal hocPhi,
            [FromQuery] int pageNumber = 1,
            [FromQuery] int pageSize = 10)
        {
            var result =
                await _khoaHocService.GetPagedByHocPhiAsync(
                    hocPhi,
                    pageNumber,
                    pageSize);

            return Ok(result);
        }


        // =====================================================
        // GET: api/KhoaHoc/search
        // =====================================================

        [HttpGet("search")]
        public async Task<IActionResult> Search(
            [FromQuery] string? tenKhoaHoc,
            [FromQuery] string? trinhDo,
            [FromQuery] decimal? hocPhi)
        {
            var result =
                await _khoaHocService.SearchAsync(
                    tenKhoaHoc,
                    trinhDo,
                    hocPhi);

            return Ok(result);
        }


        // =====================================================
        // GET: api/KhoaHoc/search-paging
        // =====================================================

        [HttpGet("search-paging")]
        public async Task<IActionResult> SearchPaged(
            [FromQuery] string? tenKhoaHoc,
            [FromQuery] string? trinhDo,
            [FromQuery] decimal? hocPhi,
            [FromQuery] int pageNumber = 1,
            [FromQuery] int pageSize = 10)
        {
            var result =
                await _khoaHocService.SearchPagedAsync(
                    tenKhoaHoc,
                    trinhDo,
                    hocPhi,
                    pageNumber,
                    pageSize);

            return Ok(result);
        }


        // =====================================================
        // POST: api/KhoaHoc
        // =====================================================

        [HttpPost]
        public async Task<IActionResult> Create(
            [FromBody] CreateKhoaHocRequest request)
        {
            try
            {
                var result =
                    await _khoaHocService.CreateAsync(request);

                return CreatedAtAction(
                    nameof(GetById),
                    new { id = result.MaKhoaHoc },
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
        // PUT: api/KhoaHoc/5
        // =====================================================

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(
            int id,
            [FromBody] UpdateKhoaHocRequest request)
        {
            try
            {
                var result =
                    await _khoaHocService.UpdateAsync(
                        id,
                        request);

                if (result == null)
                {
                    return NotFound(
                        $"Không tìm thấy khóa học có mã {id}.");
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
        // DELETE: api/KhoaHoc/5
        // =====================================================

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                var result =
                    await _khoaHocService.DeleteAsync(id);

                if (!result)
                {
                    return NotFound(
                        $"Không tìm thấy khóa học có mã {id}.");
                }

                return Ok(new
                {
                    message = "Xóa khóa học thành công."
                });
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new
                {
                    message = ex.Message
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