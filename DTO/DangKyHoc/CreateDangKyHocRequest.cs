using System.ComponentModel.DataAnnotations;

namespace DTO.DangKyHoc
{
    public class CreateDangKyHocRequest
    {
        [Required(ErrorMessage = "Mã học viên không được để trống.")]
        public int MaHocVien { get; set; }

        [Required(ErrorMessage = "Mã lớp không được để trống.")]
        public int MaLop { get; set; }

        public DateOnly? NgayDangKy { get; set; }

        [StringLength(20, ErrorMessage = "Trạng thái không được vượt quá 20 ký tự.")]
        public string? TrangThai { get; set; }
    }
}