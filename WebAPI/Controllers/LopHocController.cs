using BLL.Interfaces;
using DTO.LopHoc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class LopHocController : ControllerBase
    {
        private readonly ILopHocService _service;

        public LopHocController(ILopHocService service)
        {
            _service = service;
        }


        // =====================================================
        // 1. LẤY TẤT CẢ LỚP HỌC
        // GET: api/LopHoc
        // =====================================================

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            try
            {
                var result = await _service.GetAllAsync();

                return Ok(new
                {
                    message = "Lấy danh sách lớp học thành công.",
                    data = result
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


        // =====================================================
        // 2. LẤY LỚP HỌC THEO ID
        // GET: api/LopHoc/1
        // =====================================================

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
                        message =
                            $"Không tìm thấy lớp học có mã {id}."
                    });
                }

                return Ok(new
                {
                    message = "Lấy thông tin lớp học thành công.",
                    data = result
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


        // =====================================================
        // 3. TẠO LỚP HỌC
        // POST: api/LopHoc
        // =====================================================

        [HttpPost]
        public async Task<IActionResult> Create(
            [FromBody] CreateLopHocRequest request)
        {
            try
            {
                var result =
                    await _service.CreateAsync(request);

                return Ok(new
                {
                    message = "Tạo lớp học thành công.",
                    data = result
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


        // =====================================================
        // 4. CẬP NHẬT LỚP HỌC
        // PUT: api/LopHoc/1
        // =====================================================

        [HttpPut("{id:int}")]
        public async Task<IActionResult> Update(
            int id,
            [FromBody] UpdateLopHocRequest request)
        {
            try
            {
                var result =
                    await _service.UpdateAsync(id, request);

                if (result == null)
                {
                    return NotFound(new
                    {
                        message =
                            $"Không tìm thấy lớp học có mã {id}."
                    });
                }

                return Ok(new
                {
                    message = "Cập nhật lớp học thành công.",
                    data = result
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


        // =====================================================
        // 5. XÓA LỚP HỌC
        // DELETE: api/LopHoc/1
        // =====================================================

        [HttpDelete("{id:int}")]
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
                            $"Không tìm thấy lớp học có mã {id}."
                    });
                }

                return Ok(new
                {
                    message = "Xóa lớp học thành công."
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


        // =====================================================
        // 6. TÌM THEO MÃ LỚP
        // GET: api/LopHoc/by-code?maLopCode=IELTS01
        // =====================================================

        [HttpGet("by-code")]
        public async Task<IActionResult> GetByMaLopCode(
            [FromQuery] string maLopCode)
        {
            try
            {
                var result =
                    await _service.GetByMaLopCodeAsync(
                        maLopCode);

                return Ok(new
                {
                    message = "Tìm lớp học thành công.",
                    data = result
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


        // =====================================================
        // 7. MÃ LỚP + PHÂN TRANG
        // GET:
        // api/LopHoc/by-code/paged
        // =====================================================

        [HttpGet("by-code/paged")]
        public async Task<IActionResult> GetPagedByMaLopCode(
            [FromQuery] string maLopCode,
            [FromQuery] int pageNumber = 1,
            [FromQuery] int pageSize = 10)
        {
            try
            {
                var result =
                    await _service.GetPagedByMaLopCodeAsync(
                        maLopCode,
                        pageNumber,
                        pageSize);

                return Ok(new
                {
                    message = "Lấy danh sách phân trang thành công.",
                    data = result
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


        // =====================================================
        // 8. TÌM THEO TÊN LỚP
        // GET: api/LopHoc/by-name?tenLop=IELTS
        // =====================================================

        [HttpGet("by-name")]
        public async Task<IActionResult> GetByTenLop(
            [FromQuery] string tenLop)
        {
            try
            {
                var result =
                    await _service.GetByTenLopAsync(
                        tenLop);

                return Ok(new
                {
                    message = "Tìm lớp học thành công.",
                    data = result
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


        // =====================================================
        // 9. TÊN LỚP + PHÂN TRANG
        // GET: api/LopHoc/by-name/paged
        // =====================================================

        [HttpGet("by-name/paged")]
        public async Task<IActionResult> GetPagedByTenLop(
            [FromQuery] string tenLop,
            [FromQuery] int pageNumber = 1,
            [FromQuery] int pageSize = 10)
        {
            try
            {
                var result =
                    await _service.GetPagedByTenLopAsync(
                        tenLop,
                        pageNumber,
                        pageSize);

                return Ok(new
                {
                    message = "Lấy danh sách phân trang thành công.",
                    data = result
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


        // =====================================================
        // 10. THEO KHÓA HỌC
        // GET: api/LopHoc/by-course/1
        // =====================================================

        [HttpGet("by-course/{maKhoaHoc:int}")]
        public async Task<IActionResult> GetByKhoaHoc(
            int maKhoaHoc)
        {
            try
            {
                var result =
                    await _service.GetByKhoaHocIdAsync(
                        maKhoaHoc);

                return Ok(new
                {
                    message = "Lấy danh sách lớp theo khóa học thành công.",
                    data = result
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


        // =====================================================
        // 11. KHÓA HỌC + PHÂN TRANG
        // GET: api/LopHoc/by-course/1/paged
        // =====================================================

        [HttpGet("by-course/{maKhoaHoc:int}/paged")]
        public async Task<IActionResult> GetPagedByKhoaHoc(
            int maKhoaHoc,
            [FromQuery] int pageNumber = 1,
            [FromQuery] int pageSize = 10)
        {
            try
            {
                var result =
                    await _service.GetPagedByKhoaHocIdAsync(
                        maKhoaHoc,
                        pageNumber,
                        pageSize);

                return Ok(new
                {
                    message = "Lấy danh sách lớp phân trang thành công.",
                    data = result
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


        // =====================================================
        // 12. THEO GIÁO VIÊN
        // GET: api/LopHoc/by-teacher/1
        // =====================================================

        [HttpGet("by-teacher/{maGiaoVien:int}")]
        public async Task<IActionResult> GetByGiaoVien(
            int maGiaoVien)
        {
            try
            {
                var result =
                    await _service.GetByGiaoVienIdAsync(
                        maGiaoVien);

                return Ok(new
                {
                    message = "Lấy danh sách lớp theo giáo viên thành công.",
                    data = result
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


        // =====================================================
        // 13. GIÁO VIÊN + PHÂN TRANG
        // GET: api/LopHoc/by-teacher/1/paged
        // =====================================================

        [HttpGet("by-teacher/{maGiaoVien:int}/paged")]
        public async Task<IActionResult> GetPagedByGiaoVien(
            int maGiaoVien,
            [FromQuery] int pageNumber = 1,
            [FromQuery] int pageSize = 10)
        {
            try
            {
                var result =
                    await _service.GetPagedByGiaoVienIdAsync(
                        maGiaoVien,
                        pageNumber,
                        pageSize);

                return Ok(new
                {
                    message = "Lấy danh sách lớp phân trang thành công.",
                    data = result
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


        // =====================================================
        // 14. THEO PHÒNG HỌC
        // GET: api/LopHoc/by-room/1
        // =====================================================

        [HttpGet("by-room/{maPhong:int}")]
        public async Task<IActionResult> GetByPhongHoc(
            int maPhong)
        {
            try
            {
                var result =
                    await _service.GetByPhongHocIdAsync(
                        maPhong);

                return Ok(new
                {
                    message = "Lấy danh sách lớp theo phòng học thành công.",
                    data = result
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


        // =====================================================
        // 15. PHÒNG HỌC + PHÂN TRANG
        // GET: api/LopHoc/by-room/1/paged
        // =====================================================

        [HttpGet("by-room/{maPhong:int}/paged")]
        public async Task<IActionResult> GetPagedByPhongHoc(
            int maPhong,
            [FromQuery] int pageNumber = 1,
            [FromQuery] int pageSize = 10)
        {
            try
            {
                var result =
                    await _service.GetPagedByPhongHocIdAsync(
                        maPhong,
                        pageNumber,
                        pageSize);

                return Ok(new
                {
                    message = "Lấy danh sách lớp phân trang thành công.",
                    data = result
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


        // =====================================================
        // 16. THEO TRẠNG THÁI
        // GET: api/LopHoc/by-status?trangThai=DangHoc
        // =====================================================

        [HttpGet("by-status")]
        public async Task<IActionResult> GetByTrangThai(
            [FromQuery] string trangThai)
        {
            try
            {
                var result =
                    await _service.GetByTrangThaiAsync(
                        trangThai);

                return Ok(new
                {
                    message = "Lấy danh sách lớp theo trạng thái thành công.",
                    data = result
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


        // =====================================================
        // 17. TRẠNG THÁI + PHÂN TRANG
        // GET: api/LopHoc/by-status/paged
        // =====================================================

        [HttpGet("by-status/paged")]
        public async Task<IActionResult> GetPagedByTrangThai(
            [FromQuery] string trangThai,
            [FromQuery] int pageNumber = 1,
            [FromQuery] int pageSize = 10)
        {
            try
            {
                var result =
                    await _service.GetPagedByTrangThaiAsync(
                        trangThai,
                        pageNumber,
                        pageSize);

                return Ok(new
                {
                    message = "Lấy danh sách lớp phân trang thành công.",
                    data = result
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


        // =====================================================
        // 18. TÌM KIẾM
        // GET:
        // api/LopHoc/search?maLopCode=...&tenLop=...
        // =====================================================

        [HttpGet("search")]
        public async Task<IActionResult> Search(
            [FromQuery] string? maLopCode,
            [FromQuery] string? tenLop)
        {
            try
            {
                var result =
                    await _service.SearchAsync(
                        maLopCode,
                        tenLop);

                return Ok(new
                {
                    message = "Tìm kiếm lớp học thành công.",
                    data = result
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


        // =====================================================
        // 19. TÌM KIẾM + PHÂN TRANG
        // GET:
        // api/LopHoc/search/paged
        // =====================================================

        [HttpGet("search/paged")]
        public async Task<IActionResult> SearchPaged(
            [FromQuery] string? maLopCode,
            [FromQuery] string? tenLop,
            [FromQuery] int pageNumber = 1,
            [FromQuery] int pageSize = 10)
        {
            try
            {
                var result =
                    await _service.SearchPagedAsync(
                        maLopCode,
                        tenLop,
                        pageNumber,
                        pageSize);

                return Ok(new
                {
                    message = "Tìm kiếm và phân trang thành công.",
                    data = result
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


        // =====================================================
        // 20. LẤY TẤT CẢ + PHÂN TRANG
        // GET:
        // api/LopHoc/paged
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
    }
}