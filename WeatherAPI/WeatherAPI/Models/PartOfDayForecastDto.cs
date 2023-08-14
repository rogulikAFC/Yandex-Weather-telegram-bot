namespace WeatherAPI.Models
{
    public class PartOfDayForecastDto
    {
        public PartOfDayForecastDto(
            string partOfDay, string condition, 
            IEnumerable<int> temperatureRangeInDegreesCelsium,
            int feelsLikeInDegreesCelsium, int pressureInMmHg, int humidityInPercents,
            float windSpeedInMethersPerSecond, string windDirection)
        {
            PartOfDay = partOfDay;

            Condition = condition;

            Temperature = new TemperatureDto(
                temperatureRangeInDegreesCelsium, feelsLikeInDegreesCelsium);

            PressureInMmHg = pressureInMmHg;

            HumidityInPercents = humidityInPercents;

            Wind = new WindDto(
                windSpeedInMethersPerSecond, windDirection);
        }

        public string PartOfDay { get; set; } = null!;
        public string Condition { get; set; } = null!;
        public TemperatureDto Temperature { get; set; } = null!;
        public int PressureInMmHg { get; set; }
        public int HumidityInPercents { get; set; }
        public WindDto Wind { get; set; } = null!;
    }
}
