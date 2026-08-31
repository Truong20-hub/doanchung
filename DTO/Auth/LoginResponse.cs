using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
namespace DTO.Auth;

public class LoginResponse
{
    public int MaNguoiDung { get; set; }

    public string HoTen { get; set; } = string.Empty;

    public string TenDangNhap { get; set; } = string.Empty;

    public string VaiTro { get; set; } = string.Empty;

    public string Token { get; set; } = string.Empty;
}
