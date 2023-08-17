using AutoMapper;
using WeatherAPI.Entities;
using WeatherAPI.Models;

namespace WeatherAPI.MappingProfiles
{
    public class UserProfile : Profile
    {
        public UserProfile()
        {
            CreateMap<User, UserWithoutPlacesDto>();
        }
    }
}
