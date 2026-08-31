using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace DAL.Entities;

[Table("vai_tro")]
[Index("TenVaiTro", Name = "UQ__vai_tro__E59C937C309CC001", IsUnique = true)]
public partial class VaiTro
{
    [Key]
    [Column("ma_vai_tro")]
    public int MaVaiTro { get; set; }

    [Column("ten_vai_tro")]
    [StringLength(50)]
    public string TenVaiTro { get; set; } = null!;

    [InverseProperty("MaVaiTroNavigation")]
    public virtual ICollection<NguoiDung> NguoiDungs { get; set; } = new List<NguoiDung>();
}
