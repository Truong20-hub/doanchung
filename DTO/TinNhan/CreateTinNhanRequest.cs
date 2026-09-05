using System.ComponentModel.DataAnnotations;

namespace DTO.TinNhan;

public class CreateTinNhanRequest
{
    [Range(1, int.MaxValue, ErrorMessage = "Mã người gửi không hợp lệ")]
    public int MaNguoiGui { get; set; }

    [Range(1, int.MaxValue, ErrorMessage = "Mã người nhận không hợp lệ")]
    public int MaNguoiNhan { get; set; }

    [Range(1, int.MaxValue, ErrorMessage = "Mã học viên không hợp lệ")]
    public int MaHocVien { get; set; }

    [Required(ErrorMessage = "Nội dung không được để trống")]
    [StringLength(2000, ErrorMessage = "Nội dung không được vượt quá 2000 ký tự")]
    public string NoiDung { get; set; } = null!;
}
