using System.ComponentModel.DataAnnotations;

namespace DTO.ThongBao;

public class UpdateThongBaoRequest
{
    [Range(1, int.MaxValue, ErrorMessage = "Mã người dùng không hợp lệ")]
    public int MaNguoiDung { get; set; }

    [Required(ErrorMessage = "Loại thông báo không được để trống")]
    [StringLength(20, ErrorMessage = "Loại thông báo không quá 20 ký tự")]
    public string LoaiThongBao { get; set; } = null!;

    [Required(ErrorMessage = "Tiêu đề không được để trống")]
    [StringLength(255, ErrorMessage = "Tiêu đề không quá 255 ký tự")]
    public string TieuDe { get; set; } = null!;

    [Required(ErrorMessage = "Nội dung không được để trống")]
    [StringLength(2000, ErrorMessage = "Nội dung không quá 2000 ký tự")]
    public string NoiDung { get; set; } = null!;

    [Range(1, int.MaxValue, ErrorMessage = "Mã học viên không hợp lệ")]
    public int? MaHocVien { get; set; }

    [Range(1, int.MaxValue, ErrorMessage = "Mã lớp không hợp lệ")]
    public int? MaLop { get; set; }

    [Range(1, int.MaxValue, ErrorMessage = "Mã hóa đơn không hợp lệ")]
    public int? MaHoaDon { get; set; }

    public bool DaDoc { get; set; }
}
