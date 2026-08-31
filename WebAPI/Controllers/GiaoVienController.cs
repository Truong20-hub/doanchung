using BLL.Interfaces;
using DTO.GiaoVien;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using DTO.NguoiDung;

namespace WebAPI.Controllers
{
    [Authorize(Roles = "Admin")]
    [Route("api/[controller]")]
    [ApiController]
    public class GiaoVienController : ControllerBase
    {
        private readonly IGiaoVienService _giaoVienService;

        public GiaoVienController(IGiaoVienService giaoVienService)
        {
            _giaoVienService = giaoVienService;
        }

        // GET: api/GiaoVien
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var result = await _giaoVienService.GetAllAsync();

            return Ok(result);
        }

        // GET: api/GiaoVien/1
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById([FromQuery] int id)
        {
            var result = await _giaoVienService.GetByIdAsync(id);

            if (result == null)
                return NotFound("Không tìm thấy giáo viên.");

            return Ok(result);
        }

        // POST: api/GiaoVien
        [HttpPost]
        public async Task<IActionResult> Create(
       [FromBody] CreateTeacherRequest request
       )
        {
            try
            {
                await _giaoVienService.CreateAsync(request);

                return Ok(new
                {
                    message = "Thêm giáo viên thành công."
                });
            }
            catch (Exception ex)
            {
                throw new Exception(ex.InnerException?.Message ?? ex.Message);
            }
        }
            // PUT: api/GiaoVien/1
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(
            [FromQuery] int id,
            UpdateTeacherRequest request)
        {
            await _giaoVienService.UpdateAsync(id, request);

            return Ok("Cập nhật giáo viên thành công.");
        }

        // DELETE: api/GiaoVien/1
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete([FromQuery] int id)
        {
            try
            {
                await _giaoVienService.DeleteAsync(id);
                return Ok("Xóa giáo viên thành công.");
            }
            catch (Exception ex)
            {
                throw new Exception(ex.InnerException?.Message ?? ex.Message);
            }
        
        }
        // Tìm giáo viên theo tên
        [HttpGet("ten")]
        public async Task<IActionResult> GetByName([FromQuery] string name)
        {
            if (string.IsNullOrWhiteSpace(name))
                return BadRequest("Tên giáo viên không được để trống.");

            var result = await _giaoVienService.GetByNameAsync(name);

            if (!result.Any())
                return NotFound("Không tìm thấy giáo viên.");

            return Ok(result);
        }

        // Tìm giáo viên theo chuyên môn
        [HttpGet("chuyen-mon")]
        public async Task<IActionResult> GetByChuyenMon([FromQuery] string chuyenMon)
        {
            if (string.IsNullOrWhiteSpace(chuyenMon))
                return BadRequest("Chuyên môn không được để trống.");

            var result = await _giaoVienService.GetByChuyenMonAsync(chuyenMon);

            if (!result.Any())
                return NotFound("Không tìm thấy giáo viên.");

            return Ok(result);
        }

        // Tìm kiếm giáo viên theo từ khóa
        [HttpGet("search")]
        public async Task<IActionResult> SearchTeacher([FromQuery] string keyword)
        {
            if (string.IsNullOrWhiteSpace(keyword))
                return BadRequest("Từ khóa không được để trống.");

            var result = await _giaoVienService.SearchTeacher(keyword);

            if (!result.Any())
                return NotFound("Không tìm thấy giáo viên.");

            return Ok(result);
        }
        // Phân trang danh sách giáo viên
        [HttpGet("paged")]
        public async Task<IActionResult> GetPaged(
    [FromQuery] int pageNumber = 1,
    [FromQuery] int pageSize = 10)
        {
            var result = await _giaoVienService.GetPagedAsync(pageNumber, pageSize);

            return Ok(result);
        }
        // Tìm kiếm giáo viên theo tên và phân trang
        [HttpGet("ten-paged")]
        public async Task<IEnumerable<GiaoVienResponse>> GetByNamePagingAsync(
    [FromQuery]string name,
    [FromQuery]int pageNumber,
    [FromQuery]int pageSize)
        {
            var ds = await _giaoVienService.GetAllAsync();

            var result = ds
                .Where(x => x.HoTen != null &&
                            x.HoTen.Contains(name, StringComparison.OrdinalIgnoreCase))
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize);

            return result.Select(x => new GiaoVienResponse
            {
                MaGiaoVien = x.MaGiaoVien,
                MaNguoiDung = x.MaNguoiDung,
                HoTen = x.HoTen,
                GioiTinh = x.GioiTinh,
                NgaySinh = x.NgaySinh,
                SoDienThoai = x.SoDienThoai,
                Email = x.Email,
                ChuyenMon = x.ChuyenMon,
                NgayVaoLam = x.NgayVaoLam,
                LuongTheoGio = x.LuongTheoGio,
                DangHoatDong = x.DangHoatDong,
                TenDangNhap = x.TenDangNhap
            });
        }
        // Tìm kiếm giáo viên theo chuyên môn và phân trang
        [HttpGet("chuyen-mon-paged")]
        public async Task<IEnumerable<GiaoVienResponse>> GetByChuyenMonPagingAsync(
    [FromQuery] string chuyenMon,
    [FromQuery] int pageNumber,
    [FromQuery] int pageSize)
        {
            var ds = await _giaoVienService.GetAllAsync();

            var result = ds
                .Where(x => x.ChuyenMon != null &&
                            x.ChuyenMon.Contains(chuyenMon, StringComparison.OrdinalIgnoreCase))
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize);

            return result.Select(x => new GiaoVienResponse
            {
                MaGiaoVien = x.MaGiaoVien,
                MaNguoiDung = x.MaNguoiDung,
                HoTen = x.HoTen,
                GioiTinh = x.GioiTinh,
                NgaySinh = x.NgaySinh,
                SoDienThoai = x.SoDienThoai,
                Email = x.Email,
                ChuyenMon = x.ChuyenMon,
                NgayVaoLam = x.NgayVaoLam,
                LuongTheoGio = x.LuongTheoGio,
                DangHoatDong = x.DangHoatDong,
                TenDangNhap = x.TenDangNhap
            });
        }

    }
}
////
///  ok
///  ok
/// ///