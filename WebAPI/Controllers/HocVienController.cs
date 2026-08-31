using BLL.Interfaces;
using DTO.HocVien;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class HocVienController : ControllerBase
    {
        private readonly IHocVienService _hocVienService;

        public HocVienController(
            IHocVienService hocVienService)
        {
            _hocVienService = hocVienService;
        }

        // =========================================================
        // GET: api/HocVien
        // Lấy tất cả học viên
        // =========================================================

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var result = await _hocVienService.GetAllAsync();

            return Ok(result);
        }

        // =========================================================
        // GET: api/HocVien/5
        // Lấy học viên theo mã học viên
        // =========================================================

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var result =
                await _hocVienService.GetByIdAsync(id);

            if (result == null)
            {
                return NotFound(
                    $"Không tìm thấy học viên có mã {id}.");
            }

            return Ok(result);
        }

        // =========================================================
        // POST: api/HocVien
        // Tạo học viên + tài khoản người dùng
        // =========================================================

        [HttpPost]
        public async Task<IActionResult> Create(
            [FromBody] CreateHocVienRequest request)
        {
            try
            {
                var result =
                    await _hocVienService.CreateAsync(request);

                return CreatedAtAction(
                    nameof(GetById),
                    new { id = result.MaHocVien },
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

        // =========================================================
        // PUT: api/HocVien/5
        // Cập nhật học viên
        // =========================================================

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(
            int id,
            [FromBody] UpdateHocVienRequest request)
        {
            try
            {
                var result =
                    await _hocVienService
                        .UpdateAsync(id, request);

                if (result == null)
                {
                    return NotFound(
                        $"Không tìm thấy học viên có mã {id}.");
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

        // =========================================================
        // DELETE: api/HocVien/5
        // Xóa học viên
        // =========================================================

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                var result =
                    await _hocVienService.DeleteAsync(id);

                if (!result)
                {
                    return NotFound(
                        $"Không tìm thấy học viên có mã {id}.");
                }

                return Ok(new
                {
                    message = "Xóa học viên thành công."
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

        // =========================================================
        // GET: api/HocVien/nguoi-dung/5
        // Theo mã người dùng
        // =========================================================

        [HttpGet("nguoi-dung/{maNguoiDung}")]
        public async Task<IActionResult> GetByNguoiDungId(
            int maNguoiDung)
        {
            var result =
                await _hocVienService
                    .GetByNguoiDungIdAsync(maNguoiDung);

            return Ok(result);
        }

        // =========================================================
        // GET: api/HocVien/ten?name=Nguyen
        // Theo tên học viên
        // =========================================================

        [HttpGet("ten")]
        public async Task<IActionResult> GetByName(
            [FromQuery] string name)
        {
            var result =
                await _hocVienService
                    .GetByNameAsync(name);

            return Ok(result);
        }

        // =========================================================
        // GET: api/HocVien/phu-huynh?parentName=Nguyen
        // Theo tên phụ huynh
        // =========================================================

        [HttpGet("phu-huynh")]
        public async Task<IActionResult> GetByParentName(
            [FromQuery] string parentName)
        {
            var result =
                await _hocVienService
                    .GetByParentNameAsync(parentName);

            return Ok(result);
        }

        // =========================================================
        // GET: api/HocVien/dia-chi?address=Hung Yen
        // Theo địa chỉ
        // =========================================================

        [HttpGet("dia-chi")]
        public async Task<IActionResult> GetByAddress(
            [FromQuery] string address)
        {
            var result =
                await _hocVienService
                    .GetByAddressAsync(address);

            return Ok(result);
        }

        // =========================================================
        // GET: api/HocVien/so-dien-thoai?phone=098
        // Theo số điện thoại
        // =========================================================

        [HttpGet("so-dien-thoai")]
        public async Task<IActionResult> GetByPhone(
            [FromQuery] string phone)
        {
            var result =
                await _hocVienService
                    .GetByPhoneAsync(phone);

            return Ok(result);
        }

        // =========================================================
        // GET: api/HocVien/email?email=gmail
        // Theo email
        // =========================================================

        [HttpGet("email")]
        public async Task<IActionResult> GetByEmail(
            [FromQuery] string email)
        {
            var result =
                await _hocVienService
                    .GetByEmailAsync(email);

            return Ok(result);
        }

        // =========================================================
        // GET: api/HocVien/search?keyword=Nguyen
        // Tìm kiếm tổng hợp
        // =========================================================

        [HttpGet("search")]
        public async Task<IActionResult> Search(
            [FromQuery] string keyword)
        {
            var result =
                await _hocVienService
                    .SearchAsync(keyword);

            return Ok(result);
        }

        // =========================================================
        // GET: api/HocVien/paging?pageNumber=1&pageSize=10
        // Phân trang
        // =========================================================

        [HttpGet("paging")]
        public async Task<IActionResult> GetPaged(
            [FromQuery] int pageNumber = 1,
            [FromQuery] int pageSize = 10)
        {
            var result =
                await _hocVienService
                    .GetPagedAsync(
                        pageNumber,
                        pageSize);

            return Ok(result);
        }

        // =========================================================
        // GET: api/HocVien/paging/nguoi-dung/5?pageNumber=1&pageSize=10
        // Mã người dùng + phân trang
        // =========================================================

        [HttpGet("paging/nguoi-dung/{maNguoiDung}")]
        public async Task<IActionResult>
            GetByNguoiDungIdPaged(
                int maNguoiDung,
                [FromQuery] int pageNumber = 1,
                [FromQuery] int pageSize = 10)
        {
            var result =
                await _hocVienService
                    .GetByNguoiDungIdPagedAsync(
                        maNguoiDung,
                        pageNumber,
                        pageSize);

            return Ok(result);
        }

        // =========================================================
        // GET: api/HocVien/paging/ten?name=Nguyen&pageNumber=1&pageSize=10
        // Tên + phân trang
        // =========================================================

        [HttpGet("paging/ten")]
        public async Task<IActionResult> GetByNamePaged(
            [FromQuery] string name,
            [FromQuery] int pageNumber = 1,
            [FromQuery] int pageSize = 10)
        {
            var result =
                await _hocVienService
                    .GetByNamePagedAsync(
                        name,
                        pageNumber,
                        pageSize);

            return Ok(result);
        }

        // =========================================================
        // GET: api/HocVien/paging/phu-huynh
        // =========================================================

        [HttpGet("paging/phu-huynh")]
        public async Task<IActionResult>
            GetByParentNamePaged(
                [FromQuery] string parentName,
                [FromQuery] int pageNumber = 1,
                [FromQuery] int pageSize = 10)
        {
            var result =
                await _hocVienService
                    .GetByParentNamePagedAsync(
                        parentName,
                        pageNumber,
                        pageSize);

            return Ok(result);
        }

        // =========================================================
        // GET: api/HocVien/paging/dia-chi
        // =========================================================

        [HttpGet("paging/dia-chi")]
        public async Task<IActionResult> GetByAddressPaged(
            [FromQuery] string address,
            [FromQuery] int pageNumber = 1,
            [FromQuery] int pageSize = 10)
        {
            var result =
                await _hocVienService
                    .GetByAddressPagedAsync(
                        address,
                        pageNumber,
                        pageSize);

            return Ok(result);
        }

        // =========================================================
        // GET: api/HocVien/paging/so-dien-thoai
        // =========================================================

        [HttpGet("paging/so-dien-thoai")]
        public async Task<IActionResult> GetByPhonePaged(
            [FromQuery] string phone,
            [FromQuery] int pageNumber = 1,
            [FromQuery] int pageSize = 10)
        {
            var result =
                await _hocVienService
                    .GetByPhonePagedAsync(
                        phone,
                        pageNumber,
                        pageSize);

            return Ok(result);
        }

        // =========================================================
        // GET: api/HocVien/paging/email
        // =========================================================

        [HttpGet("paging/email")]
        public async Task<IActionResult> GetByEmailPaged(
            [FromQuery] string email,
            [FromQuery] int pageNumber = 1,
            [FromQuery] int pageSize = 10)
        {
            var result =
                await _hocVienService
                    .GetByEmailPagedAsync(
                        email,
                        pageNumber,
                        pageSize);

            return Ok(result);
        }

        // =========================================================
        // GET: api/HocVien/search-paging
        // Tìm kiếm + phân trang
        // =========================================================

        [HttpGet("search-paging")]
        public async Task<IActionResult> SearchPaged(
            [FromQuery] string keyword,
            [FromQuery] int pageNumber = 1,
            [FromQuery] int pageSize = 10)
        {
            var result =
                await _hocVienService
                    .SearchPagedAsync(
                        keyword,
                        pageNumber,
                        pageSize);

            return Ok(result);
        }
    }
}