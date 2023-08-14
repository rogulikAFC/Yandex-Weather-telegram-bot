namespace WeatherAPI.Models
{
    public class AllDayForecastDto
    {
        public AllDayForecastDto(
            DateOnly date, PlaceDto place,
            IEnumerable<PartOfDayForecastDto> forecastByPartsOfDay,
            int? UVIndexValue, string? UVIndexDescription,
            string? magneticFieldStatus, int? waterTemperature,
            TimeOnly sunriseTime, TimeOnly sunsetTime, 
            string moonStatus)
        {
            Date = date;

            Place = place;

            ForecastByPartsOfDay = forecastByPartsOfDay;

            if (!(UVIndexValue == null || UVIndexDescription == null))
            {
                UVIndex = new UVIndexDto((int)UVIndexValue, UVIndexDescription);
            }

            MagneticFieldStatus = magneticFieldStatus;

            WaterTemperature = waterTemperature;
            
            if (!(sunriseTime == null || sunsetTime == null))
            {
                DaylightTime = new DaylightTimeDto(
                    sunriseTime, sunsetTime);
            }

            MoonStatus = moonStatus;
        }

        public DateOnly Date { get; set; }

        public PlaceDto Place { get; set; } = null!;

        public IEnumerable<PartOfDayForecastDto> ForecastByPartsOfDay { get; set; }
            = new List<PartOfDayForecastDto>();

        public UVIndexDto? UVIndex { get; set; }

        public string? MagneticFieldStatus { get; set; }

        public int? WaterTemperature { get; set; }

        public DaylightTimeDto DaylightTime { get; set; }

        public string MoonStatus { get; set; }
    }
}
