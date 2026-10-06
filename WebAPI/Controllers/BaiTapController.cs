using BLL.Interfaces;
using DTO.BaiTap;
using Microsoft.AspNetCore.Mvc;

namespace WebAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class BaiTapController : ControllerBase
    {
        private readonly IBaiTapService _service;

        public BaiTapController(IBaiTapService service)
        {
            _service = service;
        }

        // Trả lỗi thống nhất theo kiểu các controller khác
        private IActionResult HandleError(Exception ex, string message)
        {
            return ex switch
            {
                ArgumentException => BadRequest(new { message = ex.Message }),
                KeyNotFoundException => NotFound(new { message = ex.Message }),
                InvalidOperationException => Conflict(new { message = ex.Message }),
                _ => StatusCode(500, new { message, detail = ex.Message })
            };
        }

        // GET api/BaiTap
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            try
            {
                return Ok(await _service.GetAllAsync());
            }
            catch (Exception ex)
            {
                return HandleError(ex, "Lỗi khi lấy danh sách bài tập.");
            }
        }

        // GET api/BaiTap/5
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
                        message = $"Không tìm thấy bài tập có mã {id}."
                    });
                }

                return Ok(result);
            }
            catch (Exception ex)
            {
                return HandleError(ex, "Lỗi khi lấy bài tập.");
            }
        }

        // GET api/BaiTap/lop/1
        [HttpGet("lop/{maLop:int}")]
        public async Task<IActionResult> GetByMaLop(int maLop)
        {
            try
            {
                return Ok(await _service.GetByMaLopAsync(maLop));
            }
            catch (Exception ex)
            {
                return HandleError(ex, "Lỗi khi lấy bài tập theo lớp.");
            }
        }

        // GET api/BaiTap/giao-vien/1
        [HttpGet("giao-vien/{maGiaoVien:int}")]
        public async Task<IActionResult> GetByMaGiaoVien(int maGiaoVien)
        {
            try
            {
                return Ok(await _service.GetByMaGiaoVienAsync(maGiaoVien));
            }
            catch (Exception ex)
            {
                return HandleError(ex, "Lỗi khi lấy bài tập theo giáo viên.");
            }
        }

        // GET api/BaiTap/hoc-vien/1  (màn hình bài tập của học viên)
        [HttpGet("hoc-vien/{maHocVien:int}")]
        public async Task<IActionResult> GetByMaHocVien(int maHocVien)
        {
            try
            {
                return Ok(await _service.GetByMaHocVienAsync(maHocVien));
            }
            catch (Exception ex)
            {
                return HandleError(ex, "Lỗi khi lấy bài tập của học viên.");
            }
        }

        // GET api/BaiTap/search?keyword=...
        [HttpGet("search")]
        public async Task<IActionResult> Search([FromQuery] string? keyword)
        {
            try
            {
                return Ok(await _service.SearchAsync(keyword));
            }
            catch (Exception ex)
            {
                return HandleError(ex, "Lỗi khi tìm kiếm bài tập.");
            }
        }

        // GET api/BaiTap/paged?pageNumber=1&pageSize=10
        [HttpGet("paged")]
        public async Task<IActionResult> GetPaged(
            [FromQuery] int pageNumber = 1,
            [FromQuery] int pageSize = 10)
        {
            try
            {
                return Ok(await _service.GetPagedAsync(pageNumber, pageSize));
            }
            catch (Exception ex)
            {
                return HandleError(ex, "Lỗi khi phân trang bài tập.");
            }
        }

        // GET api/BaiTap/search-paged?keyword=...&pageNumber=1&pageSize=10
        [HttpGet("search-paged")]
        public async Task<IActionResult> SearchPaged(
            [FromQuery] string? keyword,
            [FromQuery] int pageNumber = 1,
            [FromQuery] int pageSize = 10)
        {
            try
            {
                return Ok(await _service.SearchPagedAsync(
                    keyword, pageNumber, pageSize));
            }
            catch (Exception ex)
            {
                return HandleError(ex, "Lỗi khi tìm kiếm và phân trang.");
            }
        }

        // POST api/BaiTap
        [HttpPost]
        public async Task<IActionResult> Create(
            [FromBody] CreateBaiTapRequest request)
        {
            try
            {
                var result = await _service.CreateAsync(request);

                return CreatedAtAction(
                    nameof(GetById),
                    new { id = result.MaBaiTap },
                    result);
            }
            catch (Exception ex)
            {
                return HandleError(ex, "Có lỗi xảy ra khi thêm bài tập.");
            }
        }

        // PUT api/BaiTap/5
        [HttpPut("{id:int}")]
        public async Task<IActionResult> Update(
            int id,
            [FromBody] UpdateBaiTapRequest request)
        {
            try
            {
                var result = await _service.UpdateAsync(id, request);

                if (result == null)
                {
                    return NotFound(new
                    {
                        message = $"Không tìm thấy bài tập có mã {id}."
                    });
                }

                return Ok(result);
            }
            catch (Exception ex)
            {
                return HandleError(ex, "Có lỗi xảy ra khi cập nhật bài tập.");
            }
        }

        // DELETE api/BaiTap/5
        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                await _service.DeleteAsync(id);

                return Ok(new
                {
                    message = $"Đã xóa bài tập có mã {id} thành công."
                });
            }
            catch (Exception ex)
            {
                return HandleError(ex, "Có lỗi xảy ra khi xóa bài tập.");
            }
        }
    }
}
