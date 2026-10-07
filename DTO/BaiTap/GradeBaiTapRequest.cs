using System.ComponentModel.DataAnnotations;

namespace DTO.BaiTap
{
    public class GradeBaiTapRequest
    {
        [Required]
        [Range(0, 10)]
        public decimal DiemSo { get; set; }

        [StringLength(500)]
        public string? NhanXet { get; set; }
    }
}
