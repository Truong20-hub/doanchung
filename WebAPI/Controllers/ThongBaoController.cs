using BLL.Interfaces;
using DTO.ThongBao;
using Microsoft.AspNetCore.Mvc;

namespace WebAPI.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ThongBaoController : ControllerBase
{
    private readonly IThongBaoService _service;

    public ThongBaoController(IThongBaoService service)
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
            return StatusCode(500, new { message = "Lỗi khi lấy danh sách thông báo.", detail = ex.Message });
        }
    }

    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetById(int id)
    {
        try
        {
            var result = await _service.GetByIdAsync(id);
            return result == null
                ? NotFound(new { message = $"Không tìm thấy thông báo có mã {id}." })
                : Ok(result);
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { message = "Lỗi khi lấy thông báo.", detail = ex.Message });
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
            return StatusCode(500, new { message = "Lỗi khi lấy thông báo theo người dùng.", detail = ex.Message });
        }
    }

    [HttpGet("nguoi-dung/{maNguoiDung:int}/chua-doc")]
    public async Task<IActionResult> GetUnread(int maNguoiDung)
    {
        try
        {
            return Ok(await _service.GetUnreadAsync(maNguoiDung));
        }
        catch (KeyNotFoundException ex)
        {
            return NotFound(new { message = ex.Message });
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { message = "Lỗi khi lấy thông báo chưa đọc.", detail = ex.Message });
        }
    }

    [HttpGet("loai/{loaiThongBao}")]
    public async Task<IActionResult> GetByLoai(string loaiThongBao)
    {
        try
        {
            return Ok(await _service.GetByLoaiAsync(loaiThongBao));
        }
        catch (ArgumentException ex)
        {
            return BadRequest(new { message = ex.Message });
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { message = "Lỗi khi lọc thông báo theo loại.", detail = ex.Message });
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
            return StatusCode(500, new { message = "Lỗi khi tìm kiếm thông báo.", detail = ex.Message });
        }
    }

    [HttpGet("paged")]
    public async Task<IActionResult> GetPaged([FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 10)
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
            return StatusCode(500, new { message = "Lỗi khi phân trang thông báo.", detail = ex.Message });
        }
    }

    [HttpGet("search-paged")]
    public async Task<IActionResult> SearchPaged([FromQuery] string? keyword, [FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 10)
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
            return StatusCode(500, new { message = "Lỗi khi tìm kiếm và phân trang thông báo.", detail = ex.Message });
        }
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateThongBaoRequest request)
    {
        try
        {
            var result = await _service.CreateAsync(request);
            return CreatedAtAction(nameof(GetById), new { id = result.MaThongBao }, result);
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
            return StatusCode(500, new { message = "Lỗi khi tạo thông báo.", detail = ex.Message });
        }
    }

    [HttpPut("{id:int}")]
    public async Task<IActionResult> Update(int id, [FromBody] UpdateThongBaoRequest request)
    {
        try
        {
            var result = await _service.UpdateAsync(id, request);
            return result == null
                ? NotFound(new { message = $"Không tìm thấy thông báo có mã {id}." })
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
            return StatusCode(500, new { message = "Lỗi khi cập nhật thông báo.", detail = ex.Message });
        }
    }

    [HttpPatch("{id:int}/da-doc")]
    public async Task<IActionResult> MarkAsRead(int id)
    {
        try
        {
            await _service.MarkAsReadAsync(id);
            return Ok(new { message = "Đã đánh dấu thông báo là đã đọc." });
        }
        catch (KeyNotFoundException ex)
        {
            return NotFound(new { message = ex.Message });
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { message = "Lỗi khi đánh dấu thông báo đã đọc.", detail = ex.Message });
        }
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(int id)
    {
        try
        {
            await _service.DeleteAsync(id);
            return Ok(new { message = $"Đã xóa thông báo có mã {id} thành công." });
        }
        catch (KeyNotFoundException ex)
        {
            return NotFound(new { message = ex.Message });
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { message = "Lỗi khi xóa thông báo.", detail = ex.Message });
        }
    }
}
