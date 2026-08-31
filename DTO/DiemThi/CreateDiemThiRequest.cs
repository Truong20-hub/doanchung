using System.ComponentModel.DataAnnotations;

namespace DTO.DiemThi;

public class CreateDiemThiRequest
{
    [Required(ErrorMessage = "Mã kỳ thi không được để trống")]
    public int MaKyThi { get; set; }

    [Required(ErrorMessage = "Mã học viên không được để trống")]
    public int MaHocVien { get; set; }

    [Range(0, 10, ErrorMessage = "Điểm nghe phải trong khoảng 0-10")]
    public decimal? DiemNghe { get; set; }

    [Range(0, 10, ErrorMessage = "Điểm nói phải trong khoảng 0-10")]
    public decimal? DiemNoi { get; set; }

    [Range(0, 10, ErrorMessage = "Điểm đọc phải trong khoảng 0-10")]
    public decimal? DiemDoc { get; set; }

    [Range(0, 10, ErrorMessage = "Điểm viết phải trong khoảng 0-10")]
    public decimal? DiemViet { get; set; }

    [StringLength(1000, ErrorMessage = "Nhận xét tối đa 1000 ký tự")]
    public string? NhanXet { get; set; }
}