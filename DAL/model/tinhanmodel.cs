namespace DAL.Model;

public record TinNhanItem(int MaTinNhan, int MaNguoiGui, int MaNguoiNhan, string NoiDung, DateTime ThoiGianGui);

public record NguoiGuiSummary(int MaNguoiGui, string HoTen, int TongTin, int ChuaDoc, DateTime TinGanNhat);