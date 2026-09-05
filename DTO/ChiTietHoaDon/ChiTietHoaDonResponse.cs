namespace DTO.ChiTietHoaDon
{
    public class ChiTietHoaDonResponse
    {
        // Mã định danh duy nhất của dòng chi tiết hóa đơn.
        public int MaChiTietHoaDon { get; set; }

        // Mã hóa đơn chứa dòng chi tiết này.
        public int MaHoaDon { get; set; }

        // Số lượng của mục trong hóa đơn.
        public int SoLuong { get; set; }

        // Đơn giá của mục.
        public decimal DonGia { get; set; }

        // Thành tiền của dòng chi tiết, thường là so_luong * don_gia.
        public decimal ThanhTien { get; set; }

        // Ghi chú bổ sung cho dòng chi tiết.
        public string? GhiChu { get; set; }

        // Mã học viên chủ sở hữu của hóa đơn (nếu cần hiển thị thông tin join với hóa đơn).
        public int? MaHocVien { get; set; }

        // Tên học viên được join từ bảng học viên qua hóa đơn.
        public string? HoTenHocVien { get; set; }

        // Mã lớp của hóa đơn (nếu cần hiển thị thông tin join).
        public int? MaLop { get; set; }

        // Mã lớp dạng code của hóa đơn.
        public string? MaLopCode { get; set; }

        // Tên lớp được join từ bảng lop_hoc qua hóa đơn.
        public string? TenLop { get; set; }

        // Trạng thái hiện tại của hóa đơn liên quan.
        public string? TrangThaiHoaDon { get; set; }
    }
}
