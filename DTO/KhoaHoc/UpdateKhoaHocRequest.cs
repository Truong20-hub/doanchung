using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTO.KhoaHoc
{
    public class UpdateKhoaHocRequest
    {
        [Required(ErrorMessage = "Tên khóa học không được để trống")]
        [StringLength(150, ErrorMessage = "Tên khóa học tối đa 150 ký tự")]
        public string TenKhoaHoc { get; set; } = null!;

        [StringLength(20, ErrorMessage = "Mã khóa học tối đa 20 ký tự")]
        public string? MaCode { get; set; }

        [StringLength(50, ErrorMessage = "Trình độ tối đa 50 ký tự")]
        public string? TrinhDo { get; set; }

        public string? MoTa { get; set; }

        [Range(1, int.MaxValue, ErrorMessage = "Tổng số buổi phải lớn hơn 0")]
        public int? TongSoBuoi { get; set; }

        [Range(1, int.MaxValue, ErrorMessage = "Số tuần học phải lớn hơn 0")]
        public int? SoTuanHoc { get; set; }

        [Range(0, double.MaxValue, ErrorMessage = "Học phí không được âm")]
        public decimal? HocPhiChuan { get; set; }

        public bool? DangHoatDong { get; set; }
    }
}
