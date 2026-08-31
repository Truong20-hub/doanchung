using System.ComponentModel.DataAnnotations;

namespace DTO.BuoiHoc
{
    public class CreateBuoiHocRequest
    {
        [Required(ErrorMessage = "Mã lớp không được để trống")]
        public int MaLop { get; set; }

        [Required(ErrorMessage = "Ngày học không được để trống")]
        public DateOnly NgayHoc { get; set; }

        public TimeOnly? GioBatDau { get; set; }

        public TimeOnly? GioKetThuc { get; set; }

        [StringLength(
            255,
            ErrorMessage = "Nội dung không được vượt quá 255 ký tự")]
        public string? NoiDung { get; set; }

        public bool? BiHuy { get; set; }
    }
}