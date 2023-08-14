namespace WeatherAPI.Models
{
    public class TemperatureDto
    {
        public TemperatureDto(IEnumerable<int> rangeInDegreesCelsium, int feelsLikeInDegreesCelsium)
        {
            RangeInDegreesCelsium = rangeInDegreesCelsium;
            FeelsLikeInDegreesCelsium = feelsLikeInDegreesCelsium;
        }

        public IEnumerable<int> RangeInDegreesCelsium { get; set; } = new List<int>();
        public int FeelsLikeInDegreesCelsium { get; set; }
    }
}
