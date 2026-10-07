using System.ComponentModel.DataAnnotations;

namespace DTO.TinNhan;

public class SendStudentTinNhanRequest
{
    [Required(ErrorMessage = "Nội dung không được để trống")]
    [StringLength(2000, ErrorMessage = "Nội dung không được vượt quá 2000 ký tự")]
    public string NoiDung { get; set; } = null!;
}
