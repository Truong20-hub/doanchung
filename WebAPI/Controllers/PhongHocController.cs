using BLL.Interfaces;
using DTO.PhongHoc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class PhongHocController : ControllerBase
    {
        private readonly IPhongHocService _service;

        public PhongHocController(IPhongHocService service)
        {
            _service = service;
        }

        // =====================================================
        // 1. LẤY TẤT CẢ PHÒNG HỌC
        // GET: api/PhongHoc
        // =====================================================

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            try
            {
                var data = await _service.GetAllAsync();

                return Ok(new
                {
                    success = true,
                    message = "Lấy danh sách phòng học thành công.",
                    data = data
                });
            }
            catch (Exception ex)
            {
                return BadRequest(new
                {
                    success = false,
                    message = ex.Message
                });
            }
        }

        // =====================================================
        // 2. LẤY PHÒNG HỌC THEO ID
        // GET: api/PhongHoc/5
        // =====================================================

        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetById(int id)
        {
            try
            {
                var data = await _service.GetByIdAsync(id);

                if (data == null)
                {
                    return NotFound(new
                    {
                        success = false,
                        message = $"Không tìm thấy phòng học có mã {id}."
                    });
                }

                return Ok(new
                {
                    success = true,
                    message = "Lấy phòng học thành công.",
                    data = data
                });
            }
            catch (Exception ex)
            {
                return BadRequest(new
                {
                    success = false,
                    message = ex.Message
                });
            }
        }

        // =====================================================
        // 3. LẤY THEO TÊN
        // GET: api/PhongHoc/by-name?tenPhong=Phong%20101
        // =====================================================

        [HttpGet("by-name")]
        public async Task<IActionResult> GetByName(
            [FromQuery] string tenPhong)
        {
            try
            {
                var data = await _service.GetByNameAsync(tenPhong);

                return Ok(new
                {
                    success = true,
                    message = "Tìm phòng học theo tên thành công.",
                    data = data
                });
            }
            catch (Exception ex)
            {
                return BadRequest(new
                {
                    success = false,
                    message = ex.Message
                });
            }
        }

        // =====================================================
        // 4. LẤY THEO TÊN + PHÂN TRANG
        // GET: api/PhongHoc/by-name/paged?tenPhong=Phong&pageNumber=1&pageSize=10
        // =====================================================

        [HttpGet("by-name/paged")]
        public async Task<IActionResult> GetPagedByName(
            [FromQuery] string tenPhong,
            [FromQuery] int pageNumber = 1,
            [FromQuery] int pageSize = 10)
        {
            try
            {
                var data = await _service.GetPagedByNameAsync(
                    tenPhong,
                    pageNumber,
                    pageSize);

                return Ok(new
                {
                    success = true,
                    message = "Lấy phòng học theo tên và phân trang thành công.",
                    data = data
                });
            }
            catch (Exception ex)
            {
                return BadRequest(new
                {
                    success = false,
                    message = ex.Message
                });
            }
        }

        // =====================================================
        // 5. LẤY THEO SỨC CHỨA
        // GET: api/PhongHoc/by-capacity?sucChua=20
        // =====================================================

        [HttpGet("by-capacity")]
        public async Task<IActionResult> GetBySucChua(
            [FromQuery] int sucChua)
        {
            try
            {
                var data = await _service.GetBySucChuaAsync(
                    sucChua);

                return Ok(new
                {
                    success = true,
                    message = "Lấy phòng học theo sức chứa thành công.",
                    data = data
                });
            }
            catch (Exception ex)
            {
                return BadRequest(new
                {
                    success = false,
                    message = ex.Message
                });
            }
        }

        // =====================================================
        // 6. LẤY THEO SỨC CHỨA + PHÂN TRANG
        // GET: api/PhongHoc/by-capacity/paged?sucChua=20&pageNumber=1&pageSize=10
        // =====================================================

        [HttpGet("by-capacity/paged")]
        public async Task<IActionResult> GetPagedBySucChua(
            [FromQuery] int sucChua,
            [FromQuery] int pageNumber = 1,
            [FromQuery] int pageSize = 10)
        {
            try
            {
                var data = await _service.GetPagedBySucChuaAsync(
                    sucChua,
                    pageNumber,
                    pageSize);

                return Ok(new
                {
                    success = true,
                    message = "Lấy phòng học theo sức chứa và phân trang thành công.",
                    data = data
                });
            }
            catch (Exception ex)
            {
                return BadRequest(new
                {
                    success = false,
                    message = ex.Message
                });
            }
        }

        // =====================================================
        // 7. LẤY THEO VỊ TRÍ
        // GET: api/PhongHoc/by-location?viTri=Tang%201
        // =====================================================

        [HttpGet("by-location")]
        public async Task<IActionResult> GetByViTri(
            [FromQuery] string viTri)
        {
            try
            {
                var data = await _service.GetByViTriAsync(
                    viTri);

                return Ok(new
                {
                    success = true,
                    message = "Lấy phòng học theo vị trí thành công.",
                    data = data
                });
            }
            catch (Exception ex)
            {
                return BadRequest(new
                {
                    success = false,
                    message = ex.Message
                });
            }
        }

        // =====================================================
        // 8. LẤY THEO VỊ TRÍ + PHÂN TRANG
        // GET: api/PhongHoc/by-location/paged?viTri=Tang%201&pageNumber=1&pageSize=10
        // =====================================================

        [HttpGet("by-location/paged")]
        public async Task<IActionResult> GetPagedByViTri(
            [FromQuery] string viTri,
            [FromQuery] int pageNumber = 1,
            [FromQuery] int pageSize = 10)
        {
            try
            {
                var data = await _service.GetPagedByViTriAsync(
                    viTri,
                    pageNumber,
                    pageSize);

                return Ok(new
                {
                    success = true,
                    message = "Lấy phòng học theo vị trí và phân trang thành công.",
                    data = data
                });
            }
            catch (Exception ex)
            {
                return BadRequest(new
                {
                    success = false,
                    message = ex.Message
                });
            }
        }

        // =====================================================
        // 9. LẤY TẤT CẢ + PHÂN TRANG
        // GET: api/PhongHoc/paged?pageNumber=1&pageSize=10
        // =====================================================

        [HttpGet("paged")]
        public async Task<IActionResult> GetPaged(
            [FromQuery] int pageNumber = 1,
            [FromQuery] int pageSize = 10)
        {
            try
            {
                var data = await _service.GetPagedAsync(
                    pageNumber,
                    pageSize);

                return Ok(new
                {
                    success = true,
                    message = "Lấy danh sách phòng học phân trang thành công.",
                    data = data
                });
            }
            catch (Exception ex)
            {
                return BadRequest(new
                {
                    success = false,
                    message = ex.Message
                });
            }
        }

        // =====================================================
        // 10. TÌM KIẾM
        // GET: api/PhongHoc/search?keyword=101
        // =====================================================

        [HttpGet("search")]
        public async Task<IActionResult> Search(
            [FromQuery] string keyword)
        {
            try
            {
                var data = await _service.SearchAsync(
                    keyword);

                return Ok(new
                {
                    success = true,
                    message = "Tìm kiếm phòng học thành công.",
                    data = data
                });
            }
            catch (Exception ex)
            {
                return BadRequest(new
                {
                    success = false,
                    message = ex.Message
                });
            }
        }

        // =====================================================
        // 11. TÌM KIẾM + PHÂN TRANG
        // GET: api/PhongHoc/search/paged?keyword=101&pageNumber=1&pageSize=10
        // =====================================================

        [HttpGet("search/paged")]
        public async Task<IActionResult> SearchPaged(
            [FromQuery] string keyword,
            [FromQuery] int pageNumber = 1,
            [FromQuery] int pageSize = 10)
        {
            try
            {
                var data = await _service.SearchPagedAsync(
                    keyword,
                    pageNumber,
                    pageSize);

                return Ok(new
                {
                    success = true,
                    message = "Tìm kiếm và phân trang thành công.",
                    data = data
                });
            }
            catch (Exception ex)
            {
                return BadRequest(new
                {
                    success = false,
                    message = ex.Message
                });
            }
        }

        // =====================================================
        // 12. KIỂM TRA TÊN PHÒNG
        // GET: api/PhongHoc/check-name?tenPhong=Phong%20101
        // =====================================================

        [HttpGet("check-name")]
        public async Task<IActionResult> CheckName(
            [FromQuery] string tenPhong)
        {
            try
            {
                var exists = await _service
                    .ExistsByTenPhongAsync(tenPhong);

                return Ok(new
                {
                    success = true,
                    exists = exists,
                    message = exists
                        ? "Tên phòng đã tồn tại."
                        : "Tên phòng chưa tồn tại."
                });
            }
            catch (Exception ex)
            {
                return BadRequest(new
                {
                    success = false,
                    message = ex.Message
                });
            }
        }

        // =====================================================
        // 13. THÊM PHÒNG HỌC
        // POST: api/PhongHoc
        // =====================================================

        [HttpPost]
        public async Task<IActionResult> Create(
            [FromBody] CreatePhongHocRequest request)
        {
            try
            {
                var data = await _service.CreateAsync(request);

                return CreatedAtAction(
                    nameof(GetById),
                    new { id = data.MaPhong },
                    new
                    {
                        success = true,
                        message = "Thêm phòng học thành công.",
                        data = data
                    });
            }
            catch (Exception ex)
            {
                return BadRequest(new
                {
                    success = false,
                    message = ex.Message
                });
            }
        }

        // =====================================================
        // 14. CẬP NHẬT PHÒNG HỌC
        // PUT: api/PhongHoc/5
        // =====================================================

        [HttpPut("{id:int}")]
        public async Task<IActionResult> Update(
            int id,
            [FromBody] UpdatePhongHocRequest request)
        {
            try
            {
                var data = await _service.UpdateAsync(
                    id,
                    request);

                if (data == null)
                {
                    return NotFound(new
                    {
                        success = false,
                        message = $"Không tìm thấy phòng học có mã {id}."
                    });
                }

                return Ok(new
                {
                    success = true,
                    message = "Cập nhật phòng học thành công.",
                    data = data
                });
            }
            catch (Exception ex)
            {
                return BadRequest(new
                {
                    success = false,
                    message = ex.Message
                });
            }
        }

        // =====================================================
        // 15. XÓA PHÒNG HỌC
        // DELETE: api/PhongHoc/5
        // =====================================================

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
                        success = false,
                        message = $"Không tìm thấy phòng học có mã {id}."
                    });
                }

                return Ok(new
                {
                    success = true,
                    message = "Xóa phòng học thành công."
                });
            }
            catch (Microsoft.EntityFrameworkCore.DbUpdateException)
            {
                return Conflict(new
                {
                    success = false,
                    message = "Không thể xóa phòng học vì phòng đang được sử dụng bởi một hoặc nhiều lớp học."
                });
            }
            catch (Exception ex)
            {
                return BadRequest(new
                {
                    success = false,
                    message = ex.Message
                });
            }
        }
    }
}