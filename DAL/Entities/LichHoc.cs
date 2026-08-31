using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace DAL.Entities;

[Table("lich_hoc")]
public partial class LichHoc
{
    [Key]
    [Column("ma_lich")]
    public int MaLich { get; set; }

    [Column("ma_lop")]
    public int MaLop { get; set; }

    [Column("thu_trong_tuan")]
    [StringLength(5)]
    public string ThuTrongTuan { get; set; } = null!;

    [Column("gio_bat_dau")]
    public TimeOnly GioBatDau { get; set; }

    [Column("gio_ket_thuc")]
    public TimeOnly GioKetThuc { get; set; }

    [ForeignKey("MaLop")]
    [InverseProperty("LichHocs")]
    public virtual LopHoc MaLopNavigation { get; set; } = null!;
}
