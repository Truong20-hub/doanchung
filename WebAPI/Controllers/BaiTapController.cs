using BLL.Interfaces;
using DTO.BaiTap;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;

namespace WebAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class BaiTapController : ControllerBase
    {
        private readonly IBaiTapService _service;
        private readonly IWebHostEnvironment _environment;

        public BaiTapController(
            IBaiTapService service,
            IWebHostEnvironment environment)
        {
            _service = service;
            _environment = environment;
        }

        // Trả lỗi thống nhất theo kiểu các controller khác
        private IActionResult HandleError(Exception ex, string message)
        {
            return ex switch
            {
                ArgumentException => BadRequest(new { message = ex.Message }),
                KeyNotFoundException => NotFound(new { message = ex.Message }),
                InvalidOperationException => Conflict(new { message = ex.Message }),
                _ => StatusCode(500, new { message, detail = ex.Message })
            };
        }

        // GET api/BaiTap
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            try
            {
                return Ok(await _service.GetAllAsync());
            }
            catch (Exception ex)
            {
                return HandleError(ex, "Lỗi khi lấy danh sách bài tập.");
            }
        }

        // GET api/BaiTap/5
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
                        message = $"Không tìm thấy bài tập có mã {id}."
                    });
                }

                return Ok(result);
            }
            catch (Exception ex)
            {
                return HandleError(ex, "Lỗi khi lấy bài tập.");
            }
        }

        // GET api/BaiTap/lop/1
        [HttpGet("lop/{maLop:int}")]
        public async Task<IActionResult> GetByMaLop(int maLop)
        {
            try
            {
                return Ok(await _service.GetByMaLopAsync(maLop));
            }
            catch (Exception ex)
            {
                return HandleError(ex, "Lỗi khi lấy bài tập theo lớp.");
            }
        }

        // GET api/BaiTap/giao-vien/1
        [HttpGet("giao-vien/{maGiaoVien:int}")]
        public async Task<IActionResult> GetByMaGiaoVien(int maGiaoVien)
        {
            try
            {
                return Ok(await _service.GetByMaGiaoVienAsync(maGiaoVien));
            }
            catch (Exception ex)
            {
                return HandleError(ex, "Lỗi khi lấy bài tập theo giáo viên.");
            }
        }

        // GET api/BaiTap/hoc-vien/1  (màn hình bài tập của học viên)
        [HttpGet("hoc-vien/{maHocVien:int}")]
        public async Task<IActionResult> GetByMaHocVien(int maHocVien)
        {
            try
            {
                return Ok(await _service.GetByMaHocVienAsync(maHocVien));
            }
            catch (Exception ex)
            {
                return HandleError(ex, "Lỗi khi lấy bài tập của học viên.");
            }
        }

        // GET api/BaiTap/5/bai-nop?maGiaoVien=1
        [HttpGet("{id:int}/bai-nop")]
        public async Task<IActionResult> GetSubmissions(
            int id,
            [FromQuery] int maGiaoVien)
        {
            try
            {
                return Ok(await _service.GetSubmissionsAsync(id, maGiaoVien));
            }
            catch (Exception ex)
            {
                return HandleError(ex, "Lỗi khi lấy danh sách bài nộp.");
            }
        }

        // GET api/BaiTap/5/hoc-vien-da-nop?maGiaoVien=1
        [HttpGet("{id:int}/hoc-vien-da-nop")]
        public async Task<IActionResult> GetSubmittedStudents(
            int id,
            [FromQuery] int maGiaoVien)
        {
            try
            {
                var submissions =
                    await _service.GetSubmissionsAsync(id, maGiaoVien);
                return Ok(submissions.Where(item =>
                    item.TrangThaiNop == "DaNop" ||
                    item.TrangThaiNop == "NopTre"));
            }
            catch (Exception ex)
            {
                return HandleError(
                    ex,
                    "Lỗi khi lấy danh sách học viên đã nộp bài.");
            }
        }

        // POST api/BaiTap/5/nop-bai
        [HttpPost("{id:int}/nop-bai")]
        [RequestSizeLimit(105_000_000)]
        public async Task<IActionResult> Submit(
            int id,
            [FromForm] int maHocVien,
            [FromForm] string? ghiChu,
            [FromForm] List<IFormFile>? files)
        {
            var savedFiles = new List<string>();
            try
            {
                var uploadedFiles = files ?? new List<IFormFile>();
                if (uploadedFiles.Count > 5)
                {
                    return BadRequest(new
                    {
                        message = "Mỗi lần nộp tối đa 5 tệp."
                    });
                }

                if (uploadedFiles.Any(file =>
                        file.Length <= 0 || file.Length > 20_000_000))
                {
                    return BadRequest(new
                    {
                        message =
                            "Mỗi tệp phải có dung lượng từ 1 byte đến 20 MB."
                    });
                }

                var uploadDirectory = Path.Combine(
                    _environment.WebRootPath ?? Path.Combine(
                        _environment.ContentRootPath, "wwwroot"),
                    "UploadedAssignments");
                Directory.CreateDirectory(uploadDirectory);

                var submissionFiles =
                    new List<BaiTapSubmissionFileResponse>();
                foreach (var file in uploadedFiles)
                {
                    var extension = Path.GetExtension(file.FileName);
                    if (extension.Length > 10 ||
                        extension.Skip(1).Any(character =>
                            !char.IsLetterOrDigit(character)))
                    {
                        extension = ".bin";
                    }
                    var storedName = $"{Guid.NewGuid():N}{extension}";
                    var path = Path.Combine(uploadDirectory, storedName);
                    await using (var stream = System.IO.File.Create(path))
                    {
                        await file.CopyToAsync(stream);
                    }

                    savedFiles.Add(path);
                    submissionFiles.Add(new BaiTapSubmissionFileResponse
                    {
                        TenFile = Path.GetFileName(file.FileName),
                        DuongDan = $"/UploadedAssignments/{storedName}"
                    });
                }

                var filesJson = submissionFiles.Count == 0
                    ? null
                    : JsonSerializer.Serialize(submissionFiles);
                var result = await _service.SubmitAsync(
                    id, maHocVien, ghiChu, filesJson);
                return Ok(result);
            }
            catch (Exception ex)
            {
                foreach (var path in savedFiles)
                {
                    if (System.IO.File.Exists(path))
                    {
                        System.IO.File.Delete(path);
                    }
                }

                return HandleError(ex, "Lỗi khi nộp bài tập.");
            }
        }

        // PUT api/BaiTap/5/cham-diem/8?maGiaoVien=1
        [HttpPut("{id:int}/cham-diem/{maHocVien:int}")]
        public async Task<IActionResult> Grade(
            int id,
            int maHocVien,
            [FromQuery] int maGiaoVien,
            [FromBody] GradeBaiTapRequest request)
        {
            try
            {
                await _service.GradeAsync(
                    id, maHocVien, maGiaoVien, request);
                return NoContent();
            }
            catch (Exception ex)
            {
                return HandleError(ex, "Lỗi khi chấm điểm bài tập.");
            }
        }

        // GET api/BaiTap/search?keyword=...
        [HttpGet("search")]
        public async Task<IActionResult> Search([FromQuery] string? keyword)
        {
            try
            {
                return Ok(await _service.SearchAsync(keyword));
            }
            catch (Exception ex)
            {
                return HandleError(ex, "Lỗi khi tìm kiếm bài tập.");
            }
        }

        // GET api/BaiTap/paged?pageNumber=1&pageSize=10
        [HttpGet("paged")]
        public async Task<IActionResult> GetPaged(
            [FromQuery] int pageNumber = 1,
            [FromQuery] int pageSize = 10)
        {
            try
            {
                return Ok(await _service.GetPagedAsync(pageNumber, pageSize));
            }
            catch (Exception ex)
            {
                return HandleError(ex, "Lỗi khi phân trang bài tập.");
            }
        }

        // GET api/BaiTap/search-paged?keyword=...&pageNumber=1&pageSize=10
        [HttpGet("search-paged")]
        public async Task<IActionResult> SearchPaged(
            [FromQuery] string? keyword,
            [FromQuery] int pageNumber = 1,
            [FromQuery] int pageSize = 10)
        {
            try
            {
                return Ok(await _service.SearchPagedAsync(
                    keyword, pageNumber, pageSize));
            }
            catch (Exception ex)
            {
                return HandleError(ex, "Lỗi khi tìm kiếm và phân trang.");
            }
        }

        // POST api/BaiTap
        [HttpPost]
        public async Task<IActionResult> Create(
            [FromBody] CreateBaiTapRequest request)
        {
            try
            {
                var result = await _service.CreateAsync(request);

                return CreatedAtAction(
                    nameof(GetById),
                    new { id = result.MaBaiTap },
                    result);
            }
            catch (Exception ex)
            {
                return HandleError(ex, "Có lỗi xảy ra khi thêm bài tập.");
            }
        }

        // PUT api/BaiTap/5
        [HttpPut("{id:int}")]
        public async Task<IActionResult> Update(
            int id,
            [FromBody] UpdateBaiTapRequest request)
        {
            try
            {
                var result = await _service.UpdateAsync(id, request);

                if (result == null)
                {
                    return NotFound(new
                    {
                        message = $"Không tìm thấy bài tập có mã {id}."
                    });
                }

                return Ok(result);
            }
            catch (Exception ex)
            {
                return HandleError(ex, "Có lỗi xảy ra khi cập nhật bài tập.");
            }
        }

        // DELETE api/BaiTap/5
        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                await _service.DeleteAsync(id);

                return Ok(new
                {
                    message = $"Đã xóa bài tập có mã {id} thành công."
                });
            }
            catch (Exception ex)
            {
                return HandleError(ex, "Có lỗi xảy ra khi xóa bài tập.");
            }
        }
    }
}
