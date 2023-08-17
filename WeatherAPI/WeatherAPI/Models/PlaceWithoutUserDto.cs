namespace WeatherAPI.Models
{
    public class PlaceWithoutUserDto
    {
        public Guid Id { get; set; }
        public float Lat { get; set; }
        public float Lon { get; set; }
        public string Name { get; set; } = null!;
    }
}
