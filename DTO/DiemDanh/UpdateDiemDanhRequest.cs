using System.ComponentModel.DataAnnotations;

namespace DTO.DiemDanh
{
    public class UpdateDiemDanhRequest
    {
        [Required(ErrorMessage = "Mã buổi không được để trống")]
        public int MaBuoi { get; set; }

        [Required(ErrorMessage = "Mã học viên không được để trống")]
        public int MaHocVien { get; set; }

        [Required(ErrorMessage = "Trạng thái không được để trống")]
        [StringLength(
            20,
            ErrorMessage = "Trạng thái không được vượt quá 20 ký tự")]
        public string TrangThai { get; set; } = null!;

        [StringLength(
            255,
            ErrorMessage = "Ghi chú không được vượt quá 255 ký tự")]
        public string? GhiChu { get; set; }
    }
}