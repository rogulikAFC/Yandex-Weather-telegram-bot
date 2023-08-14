using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WeatherAPI.Models;
using WeatherAPI.Services;

namespace WeatherAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ForecastsController : ControllerBase
    {
        private readonly IWeatherScraper _weatherScraper;

        public ForecastsController(IWeatherScraper weatherScraper)
        {
            _weatherScraper = weatherScraper;
        }

        [HttpGet]
        public ActionResult<AllDayForecastDto?> Test()
        {
            var placeDto = new PlaceDto()
            {
                Id = new Guid(),
                Lat = 1,
                Lon = 1,
                Name = "Somewhere in the Africa"
            };

            var allDayForecastDto = _weatherScraper
                .GetForecastToDateByPlace(placeDto, new DateOnly(2023, 08, 23));

            if (allDayForecastDto == null )
            {
                return BadRequest();
            }

            return Ok(allDayForecastDto);
        }
    }
}
