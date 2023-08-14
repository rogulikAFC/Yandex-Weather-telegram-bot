using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.UI;
using WeatherAPI.Models;

namespace WeatherAPI.Services
{
    public class WeatherScraper : IWeatherScraper
    {
        public AllDayForecastDto? GetForecastToDateByPlace(
            PlaceDto /* must be placeID */ place, DateOnly date)
        {
            var dateString = date.ToString("dd MMMM");

            var url = $"https://yandex.com/weather/?lat={place.Lat}&lon={place.Lon}";

            var driver = new ChromeDriver();

            driver.Navigate().GoToUrl(url);

            var wait = new WebDriverWait(driver, TimeSpan.FromSeconds(2));

            try
            {
                wait.Until(d => d.FindElement(By.XPath(
                    $"//article[@class='card' and .//div[@class='a11y-hidden' and contains(text(), '{dateString}')]]")));
            }
            catch
            {
                return null;
            }

            var forecastCard = driver.FindElement(By.XPath(
                $"//article[@class='card' and .//div[@class='a11y-hidden' and contains(text(), '{dateString}')]]"));

            var dayInfo = forecastCard.FindElement(By.XPath(
                ".//div[@class='forecast-details__day-info']"));

            Console.WriteLine(dayInfo.Text);

            var partOfDayRows = dayInfo.FindElements(By.ClassName("weather-table__row"));

            var partOfDayForecastDtos = new List<PartOfDayForecastDto>();
            
            foreach (var row in partOfDayRows)
            {
                var partOfDay = row.FindElement(By.ClassName(
                    "weather-table__daypart")).Text.ToLower();

                var condition = row.FindElement(By.ClassName(
                    "weather-table__body-cell_type_condition")).Text.ToLower();

                var temperatureRangeBlock = row.FindElement(By.ClassName(
                    "weather-table__temp"));

                var temperatureElementsRange = temperatureRangeBlock
                    .FindElements(By.ClassName("temp__value"));

                var temperatureRange = new List<int>();

                foreach (var temperatureElement in temperatureElementsRange)
                {
                    var temperature = int.Parse(temperatureElement.Text);

                    temperatureRange.Add(temperature);
                }

                var feelsLikeBlock = row.FindElement(
                    By.ClassName("weather-table__body-cell_type_feels-like"));

                var feelsLikeString = feelsLikeBlock.FindElement(
                    By.ClassName("temp__value")).Text;

                var feelsLike = int.Parse(feelsLikeString);

                var pressureSting = row.FindElement(By.ClassName(
                    "weather-table__body-cell_type_air-pressure")).Text;

                var pressure = int.Parse(pressureSting);

                var humidityString = row.FindElement(By.ClassName(
                    "weather-table__body-cell_type_humidity")).Text;

                humidityString = humidityString.Remove(humidityString.IndexOf("%"));
                var humidity = int.Parse(humidityString);

                var windSpeedString = row.FindElement(By.ClassName("wind-speed")).Text;
                windSpeedString = windSpeedString.Replace(",", ".");
                var windSpeed = float.Parse(windSpeedString);

                var windDirectionBlock = row.FindElement(
                    By.ClassName("weather-table__wind-direction"));

                var windDirectionElement = windDirectionBlock.FindElement(
                    By.ClassName("icon-abbr"));

                var windDirection = windDirectionElement.GetAttribute("title");
                windDirection = windDirection.Split(" ")[1];

                var partOfDayForecastDto = new PartOfDayForecastDto(
                    partOfDay, condition, temperatureRange, feelsLike,
                    pressure, humidity, windSpeed, windDirection);

                partOfDayForecastDtos.Add(partOfDayForecastDto);
            }

            var forecastFields = dayInfo.FindElement(By.ClassName("forecast-fields"));

            string? UVIndexValueString = null;
            int? UVIndexValue = null;
            string? UVIndexDescription = null;

            try
            {
                var UVIndexBlock = forecastFields.FindElement(By.XPath(
                    ".//dt[text()='UV Index']/following-sibling::*")).Text;

                UVIndexValueString = UVIndexBlock.Split(",")[0];
                UVIndexValue = int.Parse(UVIndexValueString);

                UVIndexDescription = UVIndexBlock.Split(",")[1]
                    .Trim().ToLower();
            }
            catch { }

            int? waterTemperature = null;

            try
            {
                var waterTemperatureString = dayInfo.FindElement(By.XPath(
                    ".//dt[text()='Water temperature']/following-sibling::*")).Text;

                waterTemperatureString = waterTemperatureString.Split(" ")[0];

                waterTemperature = int.Parse(waterTemperatureString);
            }
            catch { }

            string? magneticFieldStatus = null;

            try
            {
                magneticFieldStatus = dayInfo.FindElement(By.XPath(
                    ".//dt[text()='Magnetic field']/following-sibling::*")).Text;
            }
            catch { }


            var daylightColumn = dayInfo.FindElement(By.ClassName(
                "forecast-details__right-column"));

            var sunriseSunsetBlock = daylightColumn.FindElement(By.ClassName("sunrise-sunset"));

            var sunriseString = sunriseSunsetBlock.FindElement(
                By.ClassName("sunrise-sunset__description_value_sunrise")).Text;

            var sunriseValue = TimeOnly.Parse(sunriseString);

            var sunsetString = sunriseSunsetBlock.FindElement(
                By.ClassName("sunrise-sunset__description_value_sunset")).Text;

            var sunsetValue = TimeOnly.Parse(sunsetString);

            var moonStatus = daylightColumn.FindElement(
                By.ClassName("forecast-details__moon")).Text.ToLower();

            var allDayForecastDto = new AllDayForecastDto(
                date, place, partOfDayForecastDtos,
                UVIndexValue, UVIndexDescription, magneticFieldStatus,
                waterTemperature, sunriseValue, sunsetValue, moonStatus);

            driver.Quit();

            return allDayForecastDto;
        }
    }
}
