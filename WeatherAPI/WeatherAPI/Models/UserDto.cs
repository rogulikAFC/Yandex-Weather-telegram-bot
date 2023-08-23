namespace WeatherAPI.Models
{
    public class UserDto
    {
        public long Id { get; set; }
        public virtual ICollection<PlaceWithoutUserDto> Places { get; set; }
            = new List<PlaceWithoutUserDto>();
    }
}
