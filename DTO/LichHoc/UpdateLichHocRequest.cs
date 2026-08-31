using System.ComponentModel.DataAnnotations;

namespace DTO.LichHoc
{
    public class UpdateLichHocRequest
    {
        [Required(ErrorMessage = "Mã lớp không được để trống")]
        public int MaLop { get; set; }

        [Required(ErrorMessage = "Thứ trong tuần không được để trống")]
        [StringLength(5, ErrorMessage = "Thứ trong tuần không quá 5 ký tự")]
        public string ThuTrongTuan { get; set; } = null!;

        [Required(ErrorMessage = "Giờ bắt đầu không được để trống")]
        public TimeOnly GioBatDau { get; set; }

        [Required(ErrorMessage = "Giờ kết thúc không được để trống")]
        public TimeOnly GioKetThuc { get; set; }
    }
}