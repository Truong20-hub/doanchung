using AutoMapper;
using DAL.Entities;
using DTO.ChiTietHoaDon;
using DTO.TinNhan;
using DTO.ThongBao;

namespace BLL.Mappings
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<ChiTietHoaDon, ChiTietHoaDonResponse>()
                .ForMember(dest => dest.MaHocVien,
                    opt => opt.MapFrom(src => src.MaHoaDonNavigation == null
                        ? null
                        : (int?)src.MaHoaDonNavigation.MaHocVien))
                .ForMember(dest => dest.HoTenHocVien,
                    opt => opt.MapFrom(src => src.MaHoaDonNavigation == null
                        ? null
                        : src.MaHoaDonNavigation.MaHocVienNavigation.HoTen))
                .ForMember(dest => dest.MaLop,
                    opt => opt.MapFrom(src => src.MaHoaDonNavigation == null
                        ? null
                        : (int?)src.MaHoaDonNavigation.MaLop))
                .ForMember(dest => dest.MaLopCode,
                    opt => opt.MapFrom(src => src.MaHoaDonNavigation == null
                        ? null
                        : src.MaHoaDonNavigation.MaLopNavigation.MaLopCode))
                .ForMember(dest => dest.TenLop,
                    opt => opt.MapFrom(src => src.MaHoaDonNavigation == null
                        ? null
                        : src.MaHoaDonNavigation.MaLopNavigation.TenLop))
                .ForMember(dest => dest.TrangThaiHoaDon,
                    opt => opt.MapFrom(src => src.MaHoaDonNavigation == null
                        ? null
                        : src.MaHoaDonNavigation.TrangThai));

            CreateMap<CreateChiTietHoaDonRequest, ChiTietHoaDon>()
                .ForMember(dest => dest.MaChiTietHoaDon, opt => opt.Ignore())
                .ForMember(dest => dest.MaHoaDonNavigation, opt => opt.Ignore())
                .ForMember(dest => dest.ThanhTien, opt => opt.Ignore());

            CreateMap<UpdateChiTietHoaDonRequest, ChiTietHoaDon>()
                .ForMember(dest => dest.MaChiTietHoaDon, opt => opt.Ignore())
                .ForMember(dest => dest.MaHoaDonNavigation, opt => opt.Ignore())
                .ForMember(dest => dest.ThanhTien, opt => opt.Ignore());

            CreateMap<TinNhan, TinNhanResponse>()
                .ForMember(dest => dest.TenNguoiGui,
                    opt => opt.MapFrom(src => src.MaNguoiGuiNavigation.HoTen))
                .ForMember(dest => dest.TenNguoiNhan,
                    opt => opt.MapFrom(src => src.MaNguoiNhanNavigation.HoTen))
                .ForMember(dest => dest.HoTenHocVien,
                    opt => opt.MapFrom(src => src.MaHocVienNavigation.HoTen));

            CreateMap<CreateTinNhanRequest, TinNhan>()
                .ForMember(dest => dest.MaTinNhan, opt => opt.Ignore())
                .ForMember(dest => dest.ThoiGianGui, opt => opt.Ignore())
                .ForMember(dest => dest.DaDoc, opt => opt.Ignore())
                .ForMember(dest => dest.MaNguoiGuiNavigation, opt => opt.Ignore())
                .ForMember(dest => dest.MaNguoiNhanNavigation, opt => opt.Ignore())
                .ForMember(dest => dest.MaHocVienNavigation, opt => opt.Ignore());

            CreateMap<UpdateTinNhanRequest, TinNhan>()
                .ForMember(dest => dest.MaTinNhan, opt => opt.Ignore())
                .ForMember(dest => dest.ThoiGianGui, opt => opt.Ignore())
                .ForMember(dest => dest.MaNguoiGuiNavigation, opt => opt.Ignore())
                .ForMember(dest => dest.MaNguoiNhanNavigation, opt => opt.Ignore())
                .ForMember(dest => dest.MaHocVienNavigation, opt => opt.Ignore());

            CreateMap<ThongBao, ThongBaoResponse>()
                .ForMember(dest => dest.TenNguoiDung,
                    opt => opt.MapFrom(src => src.MaNguoiDungNavigation.HoTen))
                .ForMember(dest => dest.HoTenHocVien,
                    opt => opt.MapFrom(src => src.MaHocVienNavigation == null
                        ? null
                        : src.MaHocVienNavigation.HoTen))
                .ForMember(dest => dest.TenLop,
                    opt => opt.MapFrom(src => src.MaLopNavigation == null
                        ? null
                        : src.MaLopNavigation.TenLop));

            CreateMap<CreateThongBaoRequest, ThongBao>()
                .ForMember(dest => dest.MaThongBao, opt => opt.Ignore())
                .ForMember(dest => dest.ThoiGianTao, opt => opt.Ignore())
                .ForMember(dest => dest.DaDoc, opt => opt.Ignore())
                .ForMember(dest => dest.MaNguoiDungNavigation, opt => opt.Ignore())
                .ForMember(dest => dest.MaHocVienNavigation, opt => opt.Ignore())
                .ForMember(dest => dest.MaLopNavigation, opt => opt.Ignore())
                .ForMember(dest => dest.MaHoaDonNavigation, opt => opt.Ignore());

            CreateMap<UpdateThongBaoRequest, ThongBao>()
                .ForMember(dest => dest.MaThongBao, opt => opt.Ignore())
                .ForMember(dest => dest.ThoiGianTao, opt => opt.Ignore())
                .ForMember(dest => dest.MaNguoiDungNavigation, opt => opt.Ignore())
                .ForMember(dest => dest.MaHocVienNavigation, opt => opt.Ignore())
                .ForMember(dest => dest.MaLopNavigation, opt => opt.Ignore())
                .ForMember(dest => dest.MaHoaDonNavigation, opt => opt.Ignore());
        }
    }
}
