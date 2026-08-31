using AutoMapper;
using DAL.Entities;
using DTO.ChiTietHoaDon;

namespace BLL.Mappings
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<ChiTietHoaDon, ChiTietHoaDonResponse>();

            CreateMap<CreateChiTietHoaDonRequest, ChiTietHoaDon>()
                .ForMember(dest => dest.MaChiTietHoaDon, opt => opt.Ignore())
                .ForMember(dest => dest.MaHoaDonNavigation, opt => opt.Ignore())
                .ForMember(dest => dest.GhiChu, opt => opt.MapFrom(src => src.GhiChu));

            CreateMap<UpdateChiTietHoaDonRequest, ChiTietHoaDon>()
                .ForMember(dest => dest.MaChiTietHoaDon, opt => opt.Ignore())
                .ForMember(dest => dest.MaHoaDonNavigation, opt => opt.Ignore())
                .ForMember(dest => dest.GhiChu, opt => opt.MapFrom(src => src.GhiChu));
        }
    }
}
