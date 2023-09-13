namespace WeatherAPI.Models
{
    public class DaylightTimeDto
    {
        public DaylightTimeDto(
            string sunriseTime, string sunsetTime)
        {
            SunriseTime = sunriseTime;
            SunsetTime = sunsetTime;
        }

        //public TimeOnly SunriseTime { get; set; }
        //public TimeOnly SunsetTime { get; set; }

        public string SunriseTime { get; set; }
        public string SunsetTime { get; set; }
        //public TimeSpan DaylightTime => SunsetTime - SunriseTime;
    }
}
