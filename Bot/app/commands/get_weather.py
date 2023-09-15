import requests
import datetime
from io import BytesIO
from PIL import Image
import base64
from app import host_with_protocol, bot
from app.util.objects import user_data


def get_weather(place_id, user_id):
    """
    Sends message with forecast in place by id for user by id
    """

    date = datetime.date.today()

    url = f'{host_with_protocol}/api/forecasts/{place_id}/date/{date.year}/{date.month}/{date.day}?user_id={user_id}'

    response = requests.get(url, verify=False)

    if not response.ok:
        if response.status_code == 401:
            bot.send_message(user_id, "You are not authorized")

        return

    result = response.json()

    forecast_by_parts_of_day = result["forecastByPartsOfDay"]

    forecast_by_parts_of_day_array = []

    for part_of_day in forecast_by_parts_of_day:
        temperature_string = ""

        if len(part_of_day["temperature"]["rangeInDegreesCelsium"]) == 1:
            temperature_string = f'Temperature is {part_of_day["temperature"]["rangeInDegreesCelsium"][0]}℃'

        else:
            temperature_string = f'Temperature is in range from {part_of_day["temperature"]["rangeInDegreesCelsium"][0]}℃ to {part_of_day["temperature"]["rangeInDegreesCelsium"][1]}℃'

        forecast_string = f"""
*{part_of_day["partOfDay"].title()}.* {part_of_day["condition"].title()}.
*{temperature_string}.* It feels like {part_of_day["temperature"]["feelsLikeInDegreesCelsium"]}℃.
Pressure is {part_of_day["pressureInMmHg"]} mm hg, humidity is {part_of_day["humidityInPercents"]}%.
Wind has {part_of_day["wind"]["direction"]} direction and {part_of_day["wind"]["speedInMetersPerSecond"]} mps speed.
"""

        forecast_by_parts_of_day_array.botend(forecast_string)

    uv_index = ""

    if (result["uvIndex"]):
        uv_index
        uv_index = f'''
UV index is {result["uvIndex"]["description"]} ({result["uvIndex"]["value"]})'''

    image_bytes = base64.b64decode(result["place"]["mapImageBase64"])
    image = Image.open(BytesIO(image_bytes))

    message = f"""
*Forecast in "{result["place"]["name"].strip()}" for {result["date"]}*
{''.join(part_of_day_forecast for part_of_day_forecast in forecast_by_parts_of_day_array)}
{uv_index}
Sunrise will be at {result["daylightTime"]["sunriseTime"]}, sunset at {result["daylightTime"]["sunsetTime"]}
"""

    bot.send_photo(user_id, image, message, parse_mode="markdown")


def get_forecasts():
    """
    Returns forecasts for user's places that marked as main
    """

    user_id = user_data["userId"]

    url = f"{host_with_protocol}/api/Places/by_user/{user_id}?user_id={user_id}"

    response = requests.get(url, verify=False)

    if (not response.ok):
        print("something went wrong")

        return

    places = response.json()
    places = filter(lambda p: p["isMain"], places)

    for place in places:
        get_weather(place["id"], user_id)
