using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTO.HocVien
{
    public class UpdateHocVienRequest
    {
        [StringLength(10, ErrorMessage = "Giới tính không được vượt quá 10 ký tự")]
        public string? GioiTinh { get; set; }

        public DateOnly? NgaySinh { get; set; }

        [StringLength(255, ErrorMessage = "Địa chỉ không được vượt quá 255 ký tự")]
        public string? DiaChi { get; set; }

        [StringLength(100, ErrorMessage = "Tên phụ huynh không được vượt quá 100 ký tự")]
        public string? TenPhuHuynh { get; set; }

        [StringLength(20, ErrorMessage = "Số điện thoại phụ huynh không được vượt quá 20 ký tự")]
        public string? SdtPhuHuynh { get; set; }

        public DateOnly? NgayNhapHoc { get; set; }
    }
}
