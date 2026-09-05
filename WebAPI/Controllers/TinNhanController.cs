using BLL.Interfaces;
using DTO.TinNhan;
using Microsoft.AspNetCore.Mvc;

namespace WebAPI.Controllers;

[ApiController]
[Route("api/[controller]")]
public class TinNhanController : ControllerBase
{
    private readonly ITinNhanService _service;

    public TinNhanController(ITinNhanService service)
    {
        _service = service;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        try
        {
            return Ok(await _service.GetAllAsync());
        }
        catch (Exception ex)
        {
            return StatusCode(500, new
            {
                message = "Lỗi khi lấy danh sách tin nhắn.",
                detail = ex.Message
            });
        }
    }

    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetById(int id)
    {
        try
        {
            var result = await _service.GetByIdAsync(id);
            return result == null
                ? NotFound(new { message = $"Không tìm thấy tin nhắn có mã {id}." })
                : Ok(result);
        }
        catch (Exception ex)
        {
            return StatusCode(500, new
            {
                message = "Lỗi khi lấy tin nhắn.",
                detail = ex.Message
            });
        }
    }

    [HttpGet("nguoi-dung/{maNguoiDung:int}")]
    public async Task<IActionResult> GetByMaNguoiDung(int maNguoiDung)
    {
        try
        {
            return Ok(await _service.GetByMaNguoiDungAsync(maNguoiDung));
        }
        catch (KeyNotFoundException ex)
        {
            return NotFound(new { message = ex.Message });
        }
        catch (Exception ex)
        {
            return StatusCode(500, new
            {
                message = "Lỗi khi lấy tin nhắn theo người dùng.",
                detail = ex.Message
            });
        }
    }

    [HttpGet("hoi-thoai")]
    public async Task<IActionResult> GetConversation(
        [FromQuery] int maNguoiDung1,
        [FromQuery] int maNguoiDung2,
        [FromQuery] int maHocVien)
    {
        try
        {
            return Ok(await _service.GetConversationAsync(
                maNguoiDung1,
                maNguoiDung2,
                maHocVien));
        }
        catch (KeyNotFoundException ex)
        {
            return NotFound(new { message = ex.Message });
        }
        catch (Exception ex)
        {
            return StatusCode(500, new
            {
                message = "Lỗi khi lấy hội thoại.",
                detail = ex.Message
            });
        }
    }

    [HttpGet("search")]
    public async Task<IActionResult> Search([FromQuery] string? keyword)
    {
        try
        {
            return Ok(await _service.SearchAsync(keyword));
        }
        catch (Exception ex)
        {
            return StatusCode(500, new
            {
                message = "Lỗi khi tìm kiếm tin nhắn.",
                detail = ex.Message
            });
        }
    }

    [HttpGet("paged")]
    public async Task<IActionResult> GetPaged(
        [FromQuery] int pageNumber = 1,
        [FromQuery] int pageSize = 10)
    {
        try
        {
            return Ok(await _service.GetPagedAsync(pageNumber, pageSize));
        }
        catch (ArgumentException ex)
        {
            return BadRequest(new { message = ex.Message });
        }
        catch (Exception ex)
        {
            return StatusCode(500, new
            {
                message = "Lỗi khi phân trang tin nhắn.",
                detail = ex.Message
            });
        }
    }

    [HttpGet("search-paged")]
    public async Task<IActionResult> SearchPaged(
        [FromQuery] string? keyword,
        [FromQuery] int pageNumber = 1,
        [FromQuery] int pageSize = 10)
    {
        try
        {
            return Ok(await _service.SearchPagedAsync(keyword, pageNumber, pageSize));
        }
        catch (ArgumentException ex)
        {
            return BadRequest(new { message = ex.Message });
        }
        catch (Exception ex)
        {
            return StatusCode(500, new
            {
                message = "Lỗi khi tìm kiếm và phân trang tin nhắn.",
                detail = ex.Message
            });
        }
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateTinNhanRequest request)
    {
        try
        {
            var result = await _service.CreateAsync(request);
            return CreatedAtAction(nameof(GetById), new { id = result.MaTinNhan }, result);
        }
        catch (ArgumentException ex)
        {
            return BadRequest(new { message = ex.Message });
        }
        catch (KeyNotFoundException ex)
        {
            return NotFound(new { message = ex.Message });
        }
        catch (Exception ex)
        {
            return StatusCode(500, new
            {
                message = "Lỗi khi tạo tin nhắn.",
                detail = ex.Message
            });
        }
    }

    [HttpPut("{id:int}")]
    public async Task<IActionResult> Update(int id, [FromBody] UpdateTinNhanRequest request)
    {
        try
        {
            var result = await _service.UpdateAsync(id, request);
            return result == null
                ? NotFound(new { message = $"Không tìm thấy tin nhắn có mã {id}." })
                : Ok(result);
        }
        catch (ArgumentException ex)
        {
            return BadRequest(new { message = ex.Message });
        }
        catch (KeyNotFoundException ex)
        {
            return NotFound(new { message = ex.Message });
        }
        catch (Exception ex)
        {
            return StatusCode(500, new
            {
                message = "Lỗi khi cập nhật tin nhắn.",
                detail = ex.Message
            });
        }
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(int id)
    {
        try
        {
            await _service.DeleteAsync(id);
            return Ok(new { message = $"Đã xóa tin nhắn có mã {id} thành công." });
        }
        catch (KeyNotFoundException ex)
        {
            return NotFound(new { message = ex.Message });
        }
        catch (Exception ex)
        {
            return StatusCode(500, new
            {
                message = "Lỗi khi xóa tin nhắn.",
                detail = ex.Message
            });
        }
    }
}
