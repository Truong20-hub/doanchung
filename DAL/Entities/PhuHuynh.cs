using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace DAL.Entities;

[Table("phu_huynh")]
[Index(nameof(MaNguoiDung), Name = "UQ_phuhuynh_nguoidung", IsUnique = true)]
public partial class PhuHuynh
{
    [Key]
    [Column("ma_phu_huynh")]
    public int MaPhuHuynh { get; set; }

    [Column("ma_nguoi_dung")]
    public int MaNguoiDung { get; set; }

    [ForeignKey(nameof(MaNguoiDung))]
    [InverseProperty(nameof(NguoiDung.PhuHuynh))]
    public virtual NguoiDung MaNguoiDungNavigation { get; set; } = null!;

    [InverseProperty(nameof(PhuHuynhHocVien.MaPhuHuynhNavigation))]
    public virtual ICollection<PhuHuynhHocVien> PhuHuynhHocViens { get; set; } =
        new List<PhuHuynhHocVien>();
}
