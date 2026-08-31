using BLL.Interfaces;
using DTO.Auth;
using DTO.NguoiDung;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace WebAPI.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class NguoiDungController : ControllerBase
    {
        private readonly INguoiDungService _nguoiDungService;

        public NguoiDungController(INguoiDungService nguoiDungService)
        {
            _nguoiDungService = nguoiDungService;
        }

        // GET: api/NguoiDung
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var result = await _nguoiDungService.GetAllAsync();
            return Ok(result);
        }

        // GET: api/NguoiDung/1
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var result = await _nguoiDungService.GetByIdAsync(id);

            if (result == null)
                return NotFound("Không tìm thấy người dùng.");

            return Ok(result);
        }

        // POST: api/NguoiDung
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateNguoiDungRequest request)
        {
            await _nguoiDungService.CreateAsync(request);

            return Ok("Thêm người dùng thành công.");
        }

        // PUT: api/NguoiDung/1
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] UpdateNguoiDungRequest request)
        {
            await _nguoiDungService.UpdateAsync(id, request);

            return Ok("Cập nhật người dùng thành công.");
        }

        // DELETE: api/NguoiDung/1
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            await _nguoiDungService.DeleteAsync(id);

            return Ok("Xóa người dùng thành công.");
        }
        [AllowAnonymous]
        [HttpPost("login")]
        public async Task<IActionResult> Login(LoginResquest request)
        {
            var result = await _nguoiDungService.LoginAsync(request);

            return Ok(result);
        }
    }
}