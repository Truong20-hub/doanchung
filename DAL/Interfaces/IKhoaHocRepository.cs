using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DAL.Entities;

namespace DAL.Interfaces
{
    

    
        public interface IKhoaHocRepository
        {
            // =====================================================
            // 1. LẤY DANH SÁCH TẤT CẢ KHÓA HỌC
            // =====================================================

            Task<IEnumerable<KhoaHoc>> GetAllAsync();


            // =====================================================
            // 2. LẤY KHÓA HỌC + LỌC THEO TÊN KHÓA HỌC
            // =====================================================

            Task<IEnumerable<KhoaHoc>> GetByNameAsync(
                string tenKhoaHoc);


            // =====================================================
            // 3. LẤY KHÓA HỌC + LỌC THEO TRÌNH ĐỘ
            // =====================================================

            Task<IEnumerable<KhoaHoc>> GetByTrinhDoAsync(
                string trinhDo);


            // =====================================================
            // 4. LẤY KHÓA HỌC + LỌC THEO HỌC PHÍ
            // =====================================================

            Task<IEnumerable<KhoaHoc>> GetByHocPhiAsync(
                decimal hocPhi);


            // =====================================================
            // 5. LẤY KHÓA HỌC + PHÂN TRANG
            // =====================================================

            Task<object> GetPagedAsync(
                int pageNumber,
                int pageSize);


            // =====================================================
            // 6. PHÂN TRANG + LỌC THEO TÊN KHÓA HỌC
            // =====================================================

            Task<object> GetPagedByNameAsync(
                string tenKhoaHoc,
                int pageNumber,
                int pageSize);


            // =====================================================
            // 7. PHÂN TRANG + LỌC THEO TRÌNH ĐỘ
            // =====================================================

            Task<object> GetPagedByTrinhDoAsync(
                string trinhDo,
                int pageNumber,
                int pageSize);


            // =====================================================
            // 8. PHÂN TRANG + LỌC THEO HỌC PHÍ
            // =====================================================

            Task<object> GetPagedByHocPhiAsync(
                decimal hocPhi,
                int pageNumber,
                int pageSize);


            // =====================================================
            // 9. TẠO KHÓA HỌC MỚI
            // =====================================================

            Task<KhoaHoc> AddAsync(
                KhoaHoc khoaHoc);


            // =====================================================
            // 10. CẬP NHẬT KHÓA HỌC
            // =====================================================

            Task<KhoaHoc?> UpdateAsync(
                KhoaHoc khoaHoc);


            // =====================================================
            // 11. XÓA KHÓA HỌC
            // =====================================================

            Task<bool> DeleteAsync(
                int id);


            // =====================================================
            // 12. TÌM KIẾM
            //    Tên khóa học + trình độ + học phí
            // =====================================================

            Task<IEnumerable<KhoaHoc>> SearchAsync(
                string? tenKhoaHoc,
                string? trinhDo,
                decimal? hocPhi);


            // =====================================================
            // 13. TÌM KIẾM + PHÂN TRANG
            //    Tên khóa học + trình độ + học phí
            // =====================================================

            Task<object> SearchPagedAsync(
                string? tenKhoaHoc,
                string? trinhDo,
                decimal? hocPhi,
                int pageNumber,
                int pageSize);
        // =====================================================
        // 14. lấy học phí chuẩn tất cả khóa học không lặp
        //  
        // =====================================================
        Task<IEnumerable<decimal>> GetAllHocPhiChuanAsync();
        // =====================================================
        // 15. lấy trình độ tất cả khóa học không lặp
        //  
        // =====================================================
        Task<IEnumerable<string>> GetAllTrinhDoAsync();

        // =====================================================
        // 16. lấy tên của tất cả khóa học không lặp
        //  
        // =====================================================
        Task<IEnumerable<string>> GetAllTenKhoaHocAsync();
        // =====================================================
        // 17. kiểm tra mã của khóa học có tồn tại ko
        //  
        // =====================================================
        Task<bool> ExistsByMakhoahocAsync(int makhoahoc);
        //====================================================
        // 18 lấy khóa học theo mã
        //=====================================================
        Task<KhoaHoc> GetByIdAsync(int maCode);
        // ====================================================
        // 19. kiểm tra ten khoa học có tồn tại ko
        // =====================================================
        Task<bool> ExistsByTenKhoaHocAsync(string tenKhoaHoc);
        // =====================================================
        // 20. kiểm tra mã code của khóa học có tồn tại ko
        // =====================================================
        Task<bool> ExistsByMaCodeAsync(string maCode);
    }


}
