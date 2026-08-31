using System.ComponentModel.DataAnnotations;

namespace DTO.LopHoc
{
    public class CreateLopHocRequest
    {
        [Required(ErrorMessage = "Mã lớp không được để trống")]
        [StringLength(30, ErrorMessage = "Mã lớp không được vượt quá 30 ký tự")]
        public string MaLopCode { get; set; } = null!;


        [Required(ErrorMessage = "Tên lớp không được để trống")]
        [StringLength(150, ErrorMessage = "Tên lớp không được vượt quá 150 ký tự")]
        public string TenLop { get; set; } = null!;


        [Required(ErrorMessage = "Mã khóa học không được để trống")]
        public int MaKhoaHoc { get; set; }


        // Có thể không có giáo viên
        public int? MaGiaoVien { get; set; }


        // Có thể không có phòng học
        public int? MaPhong { get; set; }


        [StringLength(500, ErrorMessage = "Avatar không được vượt quá 500 ký tự")]
        public string? AvatarUrl { get; set; }


        public DateOnly? NgayBatDau { get; set; }


        public DateOnly? NgayKetThuc { get; set; }


        [Range(1, int.MaxValue, ErrorMessage = "Sĩ số tối đa phải lớn hơn 0")]
        public int? SiSoToiDa { get; set; }


        [RegularExpression(
      "^(Huy|DaKetThuc|DangHoc|SapKhaiGiang)$",
      ErrorMessage = "Trạng thái phải thuộc một trong các giá trị: Huy, DaKetThuc, DangHoc, SapKhaiGiang"
  )]
        [StringLength(20, ErrorMessage = "Trạng thái không được vượt quá 20 ký tự")]
        public string? TrangThai { get; set; }
    }
}