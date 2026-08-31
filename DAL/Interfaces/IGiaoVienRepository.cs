using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using DAL.Entities;

namespace DAL.Interfaces;

public interface IGiaoVienRepository
{
    // lấy tất cả giáo viên
    Task<IEnumerable<GiaoVien>> GetAllAsync();
    // lấy giáo viên theo id
    Task<GiaoVien?> GetByIdAsync(int id);
    // thêm giáo viên
    Task AddAsync(GiaoVien giaoVien);
    // cập nhật giáo viên
    Task UpdateAsync(GiaoVien giaoVien);
    // xóa giáo viên
    Task DeleteAsync(int id);
    // Lưu thay đổi
    Task SaveChangesAsync();
    // kiểm tra giáo viên có tồn tại hay không theo id người dùng
    Task<bool> ExistsByNguoiDungIdAsync(int maNguoiDung);
    // Lấy giáo viên phân trang 
    Task<(IEnumerable<GiaoVien> Items, int TotalItems)> GetPagedAsync(int pageNumber, int pageSize);
    // Lấy giáo viên theo tên
    Task<IEnumerable<GiaoVien>> GetByNameAsync(string name);
    // Lấy giáo viên theo chuyên môn
    Task<IEnumerable<GiaoVien>> GetByChuyenMonAsync(string chuyenMon);
    // Tìm kiếm giáo viên theo tên, số điện thoại, email, chuyên môn
    Task<IEnumerable<GiaoVien>> SearchTeacherAsync(string keyword);
    // Lấy giáo viên theo tên phân trang
    Task<IEnumerable<GiaoVien>> GetByNamePagingAsync(string name, int pageNumber, int pageSize);
    // Lấy giáo viên theo chuyên môn phân trang
    Task<IEnumerable<GiaoVien>> GetByChuyenMonPagingAsync(string chuyenMon, int pageNumber, int pageSize);
    // Tìm kiếm giáo viên theo tên, số điện thoại, email, chuyên môn phân trang
    Task<IEnumerable<GiaoVien>> SearchTeacherPagingAsync(string keyword, int pageNumber, int pageSize);
}
