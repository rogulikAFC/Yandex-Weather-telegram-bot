using AutoMapper;
using WeatherAPI.Entities;
using WeatherAPI.Models;

namespace WeatherAPI.MappingProfiles
{
    public class PlaceProfile : Profile
    {
        public PlaceProfile()
        {
            CreateMap<Place, PlaceDto>();
        }
    }
}
