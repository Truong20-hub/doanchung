using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace DAL.Entities;

[Table("phu_huynh_hoc_vien")]
[Index(nameof(MaHocVien), Name = "idx_phuhuynh_hocvien_hocvien")]
public partial class PhuHuynhHocVien
{
    [Column("ma_phu_huynh")]
    public int MaPhuHuynh { get; set; }

    [Column("ma_hoc_vien")]
    public int MaHocVien { get; set; }

    [ForeignKey(nameof(MaPhuHuynh))]
    [InverseProperty(nameof(PhuHuynh.PhuHuynhHocViens))]
    public virtual PhuHuynh MaPhuHuynhNavigation { get; set; } = null!;

    [ForeignKey(nameof(MaHocVien))]
    [InverseProperty(nameof(HocVien.PhuHuynhHocViens))]
    public virtual HocVien MaHocVienNavigation { get; set; } = null!;
}
