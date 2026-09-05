using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace DAL.Entities;

[Table("thong_bao")]
[Index("MaNguoiDung", Name = "idx_thongbao_nguoi_dung")]
[Index("LoaiThongBao", Name = "idx_thongbao_loai")]
[Index("DaDoc", Name = "idx_thongbao_da_doc")]
public partial class ThongBao
{
    [Key]
    [Column("ma_thong_bao")]
    public int MaThongBao { get; set; }

    [Column("ma_nguoi_dung")]
    public int MaNguoiDung { get; set; }

    [Column("loai_thong_bao")]
    [StringLength(20)]
    public string LoaiThongBao { get; set; } = null!;

    [Column("tieu_de")]
    [StringLength(255)]
    public string TieuDe { get; set; } = null!;

    [Column("noi_dung")]
    [StringLength(2000)]
    public string NoiDung { get; set; } = null!;

    [Column("ma_hoc_vien")]
    public int? MaHocVien { get; set; }

    [Column("ma_lop")]
    public int? MaLop { get; set; }

    [Column("ma_hoa_don")]
    public int? MaHoaDon { get; set; }

    [Column("thoi_gian_tao")]
    public DateTime ThoiGianTao { get; set; }

    [Column("da_doc")]
    public bool DaDoc { get; set; }

    [ForeignKey("MaNguoiDung")]
    [InverseProperty("ThongBaos")]
    public virtual NguoiDung MaNguoiDungNavigation { get; set; } = null!;

    [ForeignKey("MaHocVien")]
    [InverseProperty("ThongBaos")]
    public virtual HocVien? MaHocVienNavigation { get; set; }

    [ForeignKey("MaLop")]
    [InverseProperty("ThongBaos")]
    public virtual LopHoc? MaLopNavigation { get; set; }

    [ForeignKey("MaHoaDon")]
    [InverseProperty("ThongBaos")]
    public virtual HoaDon? MaHoaDonNavigation { get; set; }
}
