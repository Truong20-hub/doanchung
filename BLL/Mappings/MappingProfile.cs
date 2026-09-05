using AutoMapper;
using DAL.Entities;
using DTO.ChiTietHoaDon;

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
        }
    }
}
