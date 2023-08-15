using WeatherAPI.Entities;
using WeatherAPI.Models;

namespace WeatherAPI.Services
{
    public interface IWeatherScraper
    {
        public Task<AllDayForecastDto?> GetForecastToDateByPlace(
            Guid placeId, DateOnly date);
    }
}
