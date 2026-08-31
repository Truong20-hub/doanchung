using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DAL.Entities;

[Table("phong_hoc")]
public partial class PhongHoc
{
    [Key]
    [Column("ma_phong")]
    public int MaPhong { get; set; }

    [Column("ten_phong")]
    [StringLength(50)]
    public string TenPhong { get; set; } = null!;

    [Column("suc_chua")]
    public int? SucChua { get; set; }

    [Column("vi_tri")]
    [StringLength(100)]
    public string? ViTri { get; set; }

    [InverseProperty("MaPhongNavigation")]
    public virtual ICollection<LopHoc> LopHocs { get; set; } = new List<LopHoc>();
}