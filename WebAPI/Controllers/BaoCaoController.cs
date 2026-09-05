using BLL.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace WebAPI.Controllers;

[ApiController]
[Route("api/[controller]")]
public class BaoCaoController : ControllerBase
{
    private readonly IBaoCaoService _service;

    public BaoCaoController(IBaoCaoService service)
    {
        _service = service;
    }

    [HttpGet("diem")]
    public async Task<IActionResult> GetBaoCaoDiem(
        [FromQuery] int? maLop,
        [FromQuery] int? maHocVien)
    {
        try
        {
            return Ok(await _service.GetBaoCaoDiemAsync(maLop, maHocVien));
        }
        catch (KeyNotFoundException ex)
        {
            return NotFound(new { message = ex.Message });
        }
        catch (Exception ex)
        {
            return StatusCode(500, new
            {
                message = "Lỗi khi lấy báo cáo điểm.",
                detail = ex.Message
            });
        }
    }

    [HttpGet("chuyen-can")]
    public async Task<IActionResult> GetBaoCaoChuyenCan(
        [FromQuery] int? maLop)
    {
        try
        {
            return Ok(await _service.GetBaoCaoChuyenCanAsync(maLop));
        }
        catch (KeyNotFoundException ex)
        {
            return NotFound(new { message = ex.Message });
        }
        catch (Exception ex)
        {
            return StatusCode(500, new
            {
                message = "Lỗi khi lấy báo cáo chuyên cần.",
                detail = ex.Message
            });
        }
    }

    [HttpGet("hoc-phi")]
    public async Task<IActionResult> GetBaoCaoHocPhi(
        [FromQuery] int? nam,
        [FromQuery] int? thang)
    {
        try
        {
            var reportYear = nam ?? DateTime.Now.Year;
            return Ok(await _service.GetBaoCaoHocPhiAsync(reportYear, thang));
        }
        catch (ArgumentException ex)
        {
            return BadRequest(new { message = ex.Message });
        }
        catch (Exception ex)
        {
            return StatusCode(500, new
            {
                message = "Lỗi khi lấy báo cáo học phí.",
                detail = ex.Message
            });
        }
    }
}
