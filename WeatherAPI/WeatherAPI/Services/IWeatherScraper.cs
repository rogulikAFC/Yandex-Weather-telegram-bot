using WeatherAPI.Entities;
using WeatherAPI.Models;

namespace WeatherAPI.Services
{
    public interface IWeatherScraper
    {
        public AllDayForecastDto? GetForecastToDateByPlace(
            PlaceDto place, DateOnly date);
    }
}
