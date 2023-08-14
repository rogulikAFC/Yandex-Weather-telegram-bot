namespace WeatherAPI.Models
{
    public class UVIndexDto
    {
        public UVIndexDto(int value, string description)
        {
            Value = value;
            Description = description;
        }

        public int Value { get; set; }
        public string Description { get; set; } = null!;
    }
}
