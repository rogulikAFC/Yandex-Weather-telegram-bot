using WeatherAPI.Entities;

namespace WeatherAPI.Models
{
    public class PlaceDto
    {
        public Guid Id { get; set; }
        public float Lat { get; set; }
        public float Lon { get; set; }
        public string Name { get; set; } = null!;
        public byte[] MapImageBase64 { get; set; } = null!;
        public UserWithoutPlacesDto? User { get; set; }
    }
}
