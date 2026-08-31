using DTO.DiemDanh;

namespace BLL.Interfaces
{
    public interface IDiemDanhService
    {
        // GET
        Task<IEnumerable<DiemDanhResponse>>
            GetAllAsync();

        Task<DiemDanhResponse?>
            GetByIdAsync(int id);

        Task<IEnumerable<DiemDanhResponse>>
            GetByMaBuoiAsync(int maBuoi);

        Task<IEnumerable<DiemDanhResponse>>
            GetByMaHocVienAsync(int maHocVien);

        // PAGING
        Task<object> GetPagedAsync(
            int pageNumber,
            int pageSize);

        // SEARCH
        Task<object> SearchPagedAsync(
            int? maBuoi,
            int? maHocVien,
            string? trangThai,
            int pageNumber,
            int pageSize);

        // CREATE
        Task<DiemDanhResponse>
            CreateAsync(
                CreateDiemDanhRequest request);

        // UPDATE
        Task<DiemDanhResponse?>
            UpdateAsync(
                int id,
                UpdateDiemDanhRequest request);

        // DELETE
        Task<bool> DeleteAsync(int id);
    }
}