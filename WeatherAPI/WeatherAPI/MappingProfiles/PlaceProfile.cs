using AutoMapper;
using WeatherAPI.Entities;
using WeatherAPI.Models;

namespace WeatherAPI.MappingProfiles
{
    public class PlaceProfile : Profile
    {
        public PlaceProfile()
        {
            CreateMap<Place, PlaceDto>()
                .ForMember(
                    dest => dest.MapImageBase64,
                    opt => opt.MapFrom("MapImage"));

            CreateMap<Place, PlaceWithoutUserDto>()
                .ForMember(
                    dest => dest.MapImageBase64,
                    opt => opt.MapFrom("MapImage"));

            CreateMap<PlaceForCreateDto, Place>()
                .AfterMap((src, dest) => dest.SetMapImage());

            CreateMap<PlaceForUpdateDto, Place>()
                .AfterMap((src, dest) => dest.SetMapImage());
        }
    }
}
