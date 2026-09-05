using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace DAL.Entities;

[Table("tin_nhan")]
[Index("MaNguoiGui", Name = "idx_tinnhan_nguoi_gui")]
[Index("MaNguoiNhan", Name = "idx_tinnhan_nguoi_nhan")]
[Index("MaHocVien", Name = "idx_tinnhan_hoc_vien")]
public partial class TinNhan
{
    [Key]
    [Column("ma_tin_nhan")]
    public int MaTinNhan { get; set; }

    [Column("ma_nguoi_gui")]
    public int MaNguoiGui { get; set; }

    [Column("ma_nguoi_nhan")]
    public int MaNguoiNhan { get; set; }

    [Column("ma_hoc_vien")]
    public int MaHocVien { get; set; }

    [Column("noi_dung")]
    [StringLength(2000)]
    public string NoiDung { get; set; } = null!;

    [Column("thoi_gian_gui")]
    public DateTime ThoiGianGui { get; set; }

    [Column("da_doc")]
    public bool DaDoc { get; set; }

    [ForeignKey("MaNguoiGui")]
    [InverseProperty("TinNhansDaGui")]
    public virtual NguoiDung MaNguoiGuiNavigation { get; set; } = null!;

    [ForeignKey("MaNguoiNhan")]
    [InverseProperty("TinNhansDaNhan")]
    public virtual NguoiDung MaNguoiNhanNavigation { get; set; } = null!;

    [ForeignKey("MaHocVien")]
    [InverseProperty("TinNhans")]
    public virtual HocVien MaHocVienNavigation { get; set; } = null!;
}
