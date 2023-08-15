using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WeatherAPI.DAL;
using WeatherAPI.Models;
using WeatherAPI.Services;

namespace WeatherAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ForecastsController : ControllerBase
    {
        private readonly IWeatherScraper _weatherScraper;
        private readonly IUnitOfWork _unitOfWork;

        public ForecastsController(
            IWeatherScraper weatherScraper, IUnitOfWork unitOfWork)
        {
            _weatherScraper = weatherScraper
                ?? throw new ArgumentNullException(nameof(weatherScraper));

            _unitOfWork = unitOfWork
                ?? throw new ArgumentNullException(nameof(unitOfWork));
        }

        [HttpGet("{placeId}/date/{year}/{month}/{day}")]
        public async Task<ActionResult<AllDayForecastDto?>> GetAllDayForecastByPlaceAndDate(
            Guid placeId, int year, int month, int day)
        {
            var place = await _unitOfWork.PlaceRepository
                .GetByIdAsync(placeId);

            if (place == null)
            {
                return NotFound();
            }

            var date = new DateOnly(year, month, day);

            var allDayForecastDto = await _weatherScraper
                .GetForecastToDateByPlace(placeId, date);

            if (allDayForecastDto == null)
            {
                return BadRequest();
            }

            return Ok(allDayForecastDto);
        }
    }
}
