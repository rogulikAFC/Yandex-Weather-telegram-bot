using WeatherAPI.Entities;

namespace WeatherAPI.Models
{
    public class PlaceDto
    {
        public Guid Id { get; set; }
        public float Lat { get; set; }
        public float Lon { get; set; }
        public string Name { get; set; } = null!;
        public User? User { get; set; }
    }
}
