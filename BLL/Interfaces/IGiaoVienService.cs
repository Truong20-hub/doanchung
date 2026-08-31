using DTO.Comon;
using DTO.GiaoVien;
using DTO.NguoiDung;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.Interfaces
{
    public interface IGiaoVienService
    {
        Task<IEnumerable<GiaoVienResponse>> GetAllAsync();

        Task<GiaoVienResponse?> GetByIdAsync(int id);

        Task UpdateAsync(int id, UpdateTeacherRequest request);

        Task DeleteAsync(int id);
        Task CreateAsync(CreateTeacherRequest request);
        // lấy giáo viên theo tên
        Task<IEnumerable<GiaoVienResponse>> GetByNameAsync(string name);
        // lấy giáo viên theo chuyên môn 
        Task<IEnumerable<GiaoVienResponse>> GetByChuyenMonAsync(string chuyenMon);
        // tìm kiếm giáo viên theo tên, số điện thoại, email, chuyên môn
        Task<IEnumerable<GiaoVienResponse>> SearchTeacher(string keyword);
        // lấy giáo viên phân trang
        Task<PagedResult<GiaoVienResponse>> GetPagedAsync(int pageNumber, int pageSize);
        // lấy giáo viên theo tên phân trang
        Task<IEnumerable<GiaoVienResponse>> GetByNamePagingAsync(
    string name,
    int pageNumber,
    int pageSize);
        // lấy giáo viên theo chuyên môn phân trang
        Task<IEnumerable<GiaoVienResponse>> GetByChuyenMonPagingAsync(
   string chuyenMon,
   int pageNumber,
   int pageSize);
        // tìm kiếm giáo viên theo tên, số điện thoại, email, chuyên môn phân trang
        Task<IEnumerable<GiaoVienResponse>> SearchTeacherPaging(
    string keyword,
    int pageNumber,
    int pageSize);
    }
}
