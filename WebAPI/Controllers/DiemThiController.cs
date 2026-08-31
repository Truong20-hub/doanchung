using System.Collections.Generic;
using System.Threading.Tasks;
using BLL.Interfaces;
using DTO.DiemThi;
using Microsoft.AspNetCore.Mvc;

namespace WebAPI.Controllers;

[ApiController]
[Route("api/[controller]")]
public class DiemThiController : ControllerBase
{
    private readonly IDiemThiService _diemThiService;

    public DiemThiController(IDiemThiService diemThiService)
    {
        _diemThiService = diemThiService;
    }

    // GET: api/DiemThi
    [HttpGet]
    public async Task<ActionResult<IEnumerable<DiemThiResponse>>> GetAll()
    {
        var result = await _diemThiService.GetAllAsync();
        return Ok(result);
    }

    // GET: api/DiemThi/5
    [HttpGet("{id:int}")]
    public async Task<ActionResult<DiemThiResponse>> GetById(int id)
    {
        var result = await _diemThiService.GetByIdAsync(id);
        if (result == null)
        {
            return NotFound(new { message = $"Không tìm thấy điểm thi có mã {id}." });
        }

        return Ok(result);
    }

    // GET: api/DiemThi/kythi/3
    [HttpGet("kythi/{maKyThi:int}")]
    public async Task<ActionResult<IEnumerable<DiemThiResponse>>> GetByMaKyThi(int maKyThi)
    {
        var result = await _diemThiService.GetByMaKyThiAsync(maKyThi);
        return Ok(result);
    }

    // GET: api/DiemThi/hocvien/7
    [HttpGet("hocvien/{maHocVien:int}")]
    public async Task<ActionResult<IEnumerable<DiemThiResponse>>> GetByMaHocVien(int maHocVien)
    {
        var result = await _diemThiService.GetByMaHocVienAsync(maHocVien);
        return Ok(result);
    }

    // POST: api/DiemThi
    [HttpPost]
    public async Task<ActionResult<DiemThiResponse>> Create([FromBody] CreateDiemThiRequest request)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        try
        {
            var created = await _diemThiService.CreateAsync(request);
            return CreatedAtAction(nameof(GetById), new { id = created.MaDiemThi }, created);
        }
        catch (InvalidOperationException ex)
        {
            return Conflict(new { message = ex.Message });
        }
    }

    // PUT: api/DiemThi/5
    [HttpPut("{id:int}")]
    public async Task<ActionResult<DiemThiResponse>> Update(int id, [FromBody] UpdateDiemThiRequest request)
    {
        if (id != request.MaDiemThi)
        {
            return BadRequest(new { message = "Mã điểm thi trên URL và trong body không khớp." });
        }

        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        try
        {
            var updated = await _diemThiService.UpdateAsync(request);
            return Ok(updated);
        }
        catch (KeyNotFoundException ex)
        {
            return NotFound(new { message = ex.Message });
        }
        catch (InvalidOperationException ex)
        {
            return Conflict(new { message = ex.Message });
        }
    }

    // DELETE: api/DiemThi/5
    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(int id)
    {
        try
        {
            await _diemThiService.DeleteAsync(id);
            return NoContent();
        }
        catch (KeyNotFoundException ex)
        {
            return NotFound(new { message = ex.Message });
        }
    }
}