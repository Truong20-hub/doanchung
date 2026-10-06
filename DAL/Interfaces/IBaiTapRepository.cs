using DAL.Entities;

namespace DAL.Interfaces
{
    public interface IBaiTapRepository
    {
        Task<IEnumerable<BaiTap>> GetAllAsync();

        Task<BaiTap?> GetByIdAsync(int id);

        Task<IEnumerable<BaiTap>> GetByMaLopAsync(int maLop);

        Task<IEnumerable<BaiTap>> GetByMaGiaoVienAsync(int maGiaoVien);

        // Bài tập của các lớp mà học viên đã đăng ký
        // (kèm bài nộp/điểm của chính học viên đó trong Diems)
        Task<IEnumerable<BaiTap>> GetByMaHocVienAsync(int maHocVien);

        Task<IEnumerable<BaiTap>> SearchAsync(string? keyword);

        Task<(IEnumerable<BaiTap> Data, int TotalItems)> SearchPagedAsync(
            string? keyword,
            int pageNumber,
            int pageSize);

        Task<bool> ExistsByIdAsync(int id);

        Task<bool> ExistsByTieuDeAsync(
            int maLop,
            string tieuDe,
            int? excludeId = null);

        Task<BaiTap> AddAsync(BaiTap baiTap);

        Task<BaiTap?> UpdateAsync(BaiTap baiTap);

        Task<bool> DeleteAsync(int id);
    }
}
