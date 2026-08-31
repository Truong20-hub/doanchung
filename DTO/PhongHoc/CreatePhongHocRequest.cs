using System.ComponentModel.DataAnnotations;

namespace DTO.PhongHoc
{
    public class CreatePhongHocRequest
    {
        [Required(ErrorMessage = "Tên phòng không được để trống")]
        [StringLength(50, ErrorMessage = "Tên phòng không được vượt quá 50 ký tự")]
        public string TenPhong { get; set; } = null!;

        [Range(1, int.MaxValue, ErrorMessage = "Sức chứa phải lớn hơn 0")]
        public int? SucChua { get; set; }

        [StringLength(100, ErrorMessage = "Vị trí không được vượt quá 100 ký tự")]
        public string? ViTri { get; set; }
    }
}