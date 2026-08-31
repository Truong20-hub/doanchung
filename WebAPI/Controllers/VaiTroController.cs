using BLL.Interfaces;
using DTO.VaiTro;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace WebAPI.Controllers
{
    [Authorize(Roles="Admin")]
    [Route("api/[controller]")]
    [ApiController]
    public class VaiTroController : ControllerBase
    {
        private readonly IVaiTroService _vaiTroService;

        public VaiTroController(IVaiTroService vaiTroService)
        {
            _vaiTroService = vaiTroService;
        }

        // GET: api/VaiTro
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var result = await _vaiTroService.GetAllVaiTroAsync();
            return Ok(result);
        }

        // GET: api/VaiTro/1
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var result = await _vaiTroService.GetVaiTroByIdAsync(id);

            if (result == null)
                return NotFound();

            return Ok(result);
        }

        // POST: api/VaiTro
        [HttpPost]
        public async Task<IActionResult> Create(CreateVaiTroRequest request)
        {
            try
            {
                await _vaiTroService.AddVaiTroAsync(request);
                return Ok("Thêm thành công.");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        // PUT: api/VaiTro/1
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, UpdateVaiTroRequest request)
        {
            await _vaiTroService.UpdateVaiTroAsync(id, request);

            return Ok("Cập nhật thành công.");
        }

        // DELETE: api/VaiTro/1
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            await _vaiTroService.DeleteVaiTroAsync(id);

            return Ok("Xóa thành công.");
        }
    }
}