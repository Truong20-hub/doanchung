using DTO.BaiTap;

namespace BLL.Interfaces
{
    public interface IBaiTapService
    {
        Task<IEnumerable<BaiTapResponse>> GetAllAsync();

        Task<BaiTapResponse?> GetByIdAsync(int id);

        Task<IEnumerable<BaiTapResponse>> GetByMaLopAsync(int maLop);

        Task<IEnumerable<BaiTapResponse>> GetByMaGiaoVienAsync(int maGiaoVien);

        Task<IEnumerable<BaiTapResponse>> GetByMaHocVienAsync(int maHocVien);

        Task<IEnumerable<BaiTapSubmissionResponse>> GetSubmissionsAsync(
            int maBaiTap,
            int maGiaoVien);

        Task<BaiTapResponse> SubmitAsync(
            int maBaiTap,
            int maHocVien,
            string? ghiChu,
            string? tepNopJson);

        Task GradeAsync(
            int maBaiTap,
            int maHocVien,
            int maGiaoVien,
            GradeBaiTapRequest request);

        Task<IEnumerable<BaiTapResponse>> SearchAsync(string? keyword);

        Task<object> GetPagedAsync(int pageNumber, int pageSize);

        Task<object> SearchPagedAsync(
            string? keyword,
            int pageNumber,
            int pageSize);

        Task<BaiTapResponse> CreateAsync(CreateBaiTapRequest request);

        Task<BaiTapResponse?> UpdateAsync(int id, UpdateBaiTapRequest request);

        Task<bool> DeleteAsync(int id);
    }
}
