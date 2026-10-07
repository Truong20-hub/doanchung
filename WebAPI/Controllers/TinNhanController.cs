using BLL.Interfaces;
using DAL.Context;
using DTO.TinNhan;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using DAL.Model;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace WebAPI.Controllers;

[ApiController]
[Route("api/[controller]")]
public class TinNhanController : ControllerBase
{
    private readonly ITinNhanService _service;
    private readonly AppDbContext _context;

    public TinNhanController(ITinNhanService service, AppDbContext context)
    {
        _service = service;
        _context = context;
    }

    [Authorize]
    [HttpGet("lop/{maLop:int}/hoi-thoai")]
    public async Task<IActionResult> GetStudentClassConversation(int maLop)
    {
        var participants = await GetStudentChatParticipantsAsync(maLop);
        if (participants == null)
            return NotFound(new { message = "Không tìm thấy lớp học hoặc bạn không có quyền truy cập." });

        var messages = await _service.GetConversationAsync(
            participants.StudentUserId,
            participants.TeacherUserId,
            participants.StudentId);

        return Ok(new
        {
            maNguoiDungGiaoVien = participants.TeacherUserId,
            tinNhans = messages
        });
    }

    [Authorize]
    [HttpPost("lop/{maLop:int}")]
    public async Task<IActionResult> SendStudentClassMessage(
        int maLop,
        [FromBody] SendStudentTinNhanRequest request)
    {
        if (string.IsNullOrWhiteSpace(request.NoiDung))
            return BadRequest(new { message = "Nội dung không được để trống." });

        var participants = await GetStudentChatParticipantsAsync(maLop);
        if (participants == null)
            return NotFound(new { message = "Không tìm thấy lớp học hoặc bạn không có quyền truy cập." });

        try
        {
            var message = await _service.CreateAsync(new CreateTinNhanRequest
            {
                MaNguoiGui = participants.StudentUserId,
                MaNguoiNhan = participants.TeacherUserId,
                MaHocVien = participants.StudentId,
                NoiDung = request.NoiDung.Trim()
            });

            return Ok(message);
        }
        catch (ArgumentException ex)
        {
            return BadRequest(new { message = ex.Message });
        }
        catch (KeyNotFoundException ex)
        {
            return NotFound(new { message = ex.Message });
        }
    }

    private async Task<StudentChatParticipants?> GetStudentChatParticipantsAsync(int maLop)
    {
        var userIdValue = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (!int.TryParse(userIdValue, out var studentUserId))
            return null;

        return await _context.DangKyHocs
            .Where(enrollment =>
                enrollment.MaHocVienNavigation.MaNguoiDung == studentUserId &&
                enrollment.MaLop == maLop &&
                enrollment.TrangThai == "DangHoc" &&
                enrollment.MaLopNavigation.MaGiaoVien != null)
            .Select(enrollment => new StudentChatParticipants(
                enrollment.MaHocVien,
                studentUserId,
                enrollment.MaLopNavigation.MaGiaoVienNavigation!.MaNguoiDung))
            .SingleOrDefaultAsync();
    }

    private sealed record StudentChatParticipants(
        int StudentId,
        int StudentUserId,
        int TeacherUserId);

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
    /// <summary>
    /// Danh sách tất cả người đã từng gửi tin nhắn.
    /// GET api/TinNhan/nguoi-gui
    /// </summary>
    [HttpGet("nguoi-gui")]
    [ProducesResponseType(typeof(NguoiGuiSummary[]), StatusCodes.Status200OK)]
    public async Task<ActionResult<NguoiGuiSummary[]>> GetDanhSachNguoiGui()
    {
        var result = await _service.GetDanhSachNguoiGuiAsync();
        return Ok(result);
    }

    /// <summary>
    /// Danh sách những người đã nhắn cho một người dùng cụ thể.
    /// GET api/TinNhan/nguoi-gui/5
    /// </summary>
    [HttpGet("nguoi-gui/{maNguoiNhan:int}")]
    [ProducesResponseType(typeof(NguoiGuiSummary[]), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<NguoiGuiSummary[]>> GetDanhSachNguoiGuiChoNguoiNhan(int maNguoiNhan)
    {
        if (maNguoiNhan <= 0)
            return BadRequest("Mã người nhận không hợp lệ.");

        var result = await _service.GetDanhSachNguoiGuiChoNguoiNhanAsync(maNguoiNhan);
        return Ok(result);
    }

    /// <summary>
    /// Cuộc trò chuyện giữa hai người.
    /// GET api/TinNhan/cuoc-tro-chuyen?maNguoiA=1&amp;maNguoiB=2
    /// </summary>
    [HttpGet("cuoc-tro-chuyen")]
    [ProducesResponseType(typeof(TinNhanItem[]), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<TinNhanItem[]>> GetCuocTroChuyen(
        [FromQuery] int maNguoiA,
        [FromQuery] int maNguoiB)
    {
        if (maNguoiA <= 0 || maNguoiB <= 0)
            return BadRequest("Mã người dùng không hợp lệ.");

        var result = await _service.GetCuocTroChuyenAsync(maNguoiA, maNguoiB);
        return Ok(result);
    }

}
