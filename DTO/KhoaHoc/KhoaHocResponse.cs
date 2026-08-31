using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTO.KhoaHoc
{
    public class KhoaHocResponse
    {
        public int MaKhoaHoc { get; set; }

        public string TenKhoaHoc { get; set; } = null!;

        public string? MaCode { get; set; }

        public string? TrinhDo { get; set; }

        public string? MoTa { get; set; }

        public int? TongSoBuoi { get; set; }

        public int? SoTuanHoc { get; set; }

        public decimal? HocPhiChuan { get; set; }

        public bool? DangHoatDong { get; set; }
    }
}
