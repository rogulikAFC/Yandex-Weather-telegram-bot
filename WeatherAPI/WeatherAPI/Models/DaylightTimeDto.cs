namespace WeatherAPI.Models
{
    public class DaylightTimeDto
    {
        public DaylightTimeDto(
            TimeOnly sunriseTime, TimeOnly sunsetTime)
        {
            SunriseTime = sunriseTime;
            SunsetTime = sunsetTime;
        }

        public TimeOnly SunriseTime { get; set; }
        public TimeOnly SunsetTime { get; set; }
        public TimeSpan DaylightTime => SunsetTime - SunriseTime;
    }
}
