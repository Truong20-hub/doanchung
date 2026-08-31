using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DAL.Entities;

namespace BLL.Interfaces;

public interface IJwtService
{
    string GenerateToken(NguoiDung user);
}
