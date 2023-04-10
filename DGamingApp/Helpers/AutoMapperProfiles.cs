using AutoMapper;
using DGamingApp.Dto;
using DGamingApp.Entities;
using DGamingApp.Extensions;

namespace DGamingApp.Helpers
{
    public class AutoMapperProfiles : Profile
    {
        public AutoMapperProfiles()
        {
            CreateMap<AppUser, MemberDto>()
                .ForMember(dest => dest.PhotoUrl, opt => opt.MapFrom(src => src.Photos.FirstOrDefault(x => x.IsMain).Url))
                .ForMember(dest => dest.Age, opt => opt.MapFrom(src => src.DateOfBirth.CalculateAge()));
            CreateMap<Photo, PhotoDto>();

            CreateMap<MemberUpdateDto, AppUser>();

            CreateMap<RegisterDto, AppUser>();
        }
    }
}
 