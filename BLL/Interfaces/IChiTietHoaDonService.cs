using DTO.ChiTietHoaDon;

namespace BLL.Interfaces
{
    public interface IChiTietHoaDonService
    {
        Task<IEnumerable<ChiTietHoaDonResponse>> GetAllAsync();

        // =====================================================
        // 1. LẤY THEO HÓA ĐƠN
        // =====================================================

        Task<IEnumerable<ChiTietHoaDonResponse>> GetByMaHoaDonAsync(int maHoaDon);

        Task<IEnumerable<ChiTietHoaDonResponse>> SearchAsync(string? keyword);

        Task<object> GetPagedAsync(int pageNumber, int pageSize);

        Task<object> SearchPagedAsync(string? keyword, int pageNumber, int pageSize);

        // =====================================================
        // 2. LẤY THEO ID
        // =====================================================

        Task<ChiTietHoaDonResponse?> GetByIdAsync(int id);

        // =====================================================
        // 3. THÊM
        // =====================================================

        Task<ChiTietHoaDonResponse> CreateAsync(CreateChiTietHoaDonRequest request);

        // =====================================================
        // 4. CẬP NHẬT
        // =====================================================

        Task<ChiTietHoaDonResponse?> UpdateAsync(int id, UpdateChiTietHoaDonRequest request);

        // =====================================================
        // 5. XÓA
        // =====================================================

        Task<bool> DeleteAsync(int id);
    }
}
