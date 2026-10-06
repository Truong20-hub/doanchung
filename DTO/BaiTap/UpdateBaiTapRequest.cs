using System.ComponentModel.DataAnnotations;

namespace DTO.BaiTap
{
    public class UpdateBaiTapRequest
    {
        [Required(ErrorMessage = "Mã lớp không được để trống")]
        public int MaLop { get; set; }

        public int? MaBuoi { get; set; }

        [Required(ErrorMessage = "Mã giáo viên không được để trống")]
        public int MaGiaoVien { get; set; }

        [Required(ErrorMessage = "Tiêu đề không được để trống")]
        [StringLength(200, ErrorMessage = "Tiêu đề không quá 200 ký tự")]
        public string TieuDe { get; set; } = null!;

        public string? MoTa { get; set; }

        public DateOnly? NgayGiao { get; set; }

        public DateOnly? HanNop { get; set; }

        // Chỉ lưu ĐƯỜNG DẪN / URL tới tệp. Gửi null hoặc rỗng để gỡ tệp đính kèm
        [StringLength(500, ErrorMessage = "Đường dẫn tệp không quá 500 ký tự")]
        public string? FileDinhKem { get; set; }
    }
}
