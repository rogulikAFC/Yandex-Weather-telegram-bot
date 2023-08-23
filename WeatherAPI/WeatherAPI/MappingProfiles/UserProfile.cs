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
            CreateMap<User, UserDto>();
            CreateMap<UserForRegistrationDto, User>();
        }
    }
}
