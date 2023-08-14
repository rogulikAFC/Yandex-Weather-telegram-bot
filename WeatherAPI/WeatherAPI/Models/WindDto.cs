namespace WeatherAPI.Models
{
    public class WindDto
    {
        public WindDto(float speedInMethersPerSecond, string direction)
        {
            SpeedInMetersPerSecond = speedInMethersPerSecond;
            Direction = direction;
        }

        public float SpeedInMetersPerSecond { get; set; }
        public string Direction { get; set; } = null!;
    }
}
