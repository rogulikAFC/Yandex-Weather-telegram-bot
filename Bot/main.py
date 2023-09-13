from os import environ
import schedule
from time import sleep
from threading import Thread
import requests
import datetime
from io import BytesIO
from PIL import Image
import base64
import telebot
from telebot.types import InlineKeyboardMarkup, InlineKeyboardButton

bot = telebot.TeleBot(environ["TG_BOT_KEY"])

user_data = {
    "userId": "",
    "timeToSendWeather": "",
}

place_for_create = {
    "lat": None,
    "lon": None,
    "name": "",
    "isMain": False,
    "userId": None
}

scheduler_job = None

host_with_protocol = environ["TG_BOT_HOST_WITH_PROTOCOL"]
frontend_host = environ["FRONTEND_HOST"]  # localhost doesn't available


@bot.message_handler(commands=['start'])
def send_welcome(message):
    print(message)

    bot.reply_to(message,
                 """
Hello! 👋🏻
I'm bot, that gets info about weather at your place via Yandex weather.
I've developed by [rogulik](https://github.com/rogulikAFC/Yandex-Weather-telegram-bot)

*Use /help to view all bot commands*
""",
                 parse_mode="markdown")

    url = f"{host_with_protocol}/api/users/{message.chat.id}?user_id={message.chat.id}"

    response = requests.get(url, verify=False)

    if response.status_code == 401:
        bot.send_message(
            message.chat.id, "*You must registrate to use the bot.* /registrate to start", parse_mode="markdown")


@bot.message_handler(commands=["help"])
def help(message):
    bot.send_message(
        message.chat.id,
        f"*List of commands*:\n\n/registrate \- registration\n\n/createplace \- create a place\n\n/settime \- set time, in which you will receive forecasts in your places, marked main\n\n/cancelreceive \- cancels forecasts sending\n\n\n_You can change your places at [this]({frontend_host}/{message.chat.id}) URL_",
        parse_mode="markdownv2")


@bot.message_handler(commands=["registrate"])
def registrate_user(message):
    url = f"{host_with_protocol}/api/users/{message.chat.id}?user_id={message.chat.id}"
    response = requests.get(url, verify=False)

    if response.ok:
        bot.send_message(
            message.chat.id, "You are already registrated. Now, you can use the bot :)")

        return

    keyboard = InlineKeyboardMarkup()

    keyboard.add(
        InlineKeyboardButton(
            "Continue registration", url=f"{frontend_host}/registration/{message.chat.id}")
    )

    bot.send_message(
        message.chat.id, f"Please, click to continue", reply_markup=keyboard)


def get_weather(place_id, user_id):
    date = datetime.date.today()

    url = f'{host_with_protocol}/api/forecasts/{place_id}/date/{date.year}/{date.month}/{date.day}?user_id={user_id}'

    response = requests.get(url, verify=False)

    if not response.ok:
        if response.status_code == 401:
            bot.send_message(user_id, "You are not authorized")

        return

    result = response.json()

    forecast_by_parts_of_day = result["forecastByPartsOfDay"]

    print(forecast_by_parts_of_day)

    print(result)

    # "".title()

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

        forecast_by_parts_of_day_array.append(forecast_string)

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

# @bot.message_handler(commands=[""])


def get_forecasts():
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


@bot.message_handler(commands=['settime'])
def add_forecast_send_time_start(message):
    url = f"{host_with_protocol}/api/users/{message.chat.id}?user_id={message.chat.id}"

    response = requests.get(url, verify=False)

    if response.status_code == 401:
        bot.send_message(
            message.chat.id, "*You must registrate to use the bot.* /registrate to start", parse_mode="markdown")

        return

    print(message)
    sended_message = bot.send_message(
        message.chat.id, "Write time in \"HH:MM\" format")
    bot.register_next_step_handler(sended_message, add_forecast_send_time)


def add_forecast_send_time(message):
    time_string = message.text

    try:
        datetime.datetime.strptime(time_string, "%H:%M")

        user_data["userId"] = message.chat.id
        user_data["timeToSendWeather"] = time_string

        print("successfully!")

        schedule.every().day.at(
            user_data["timeToSendWeather"]).do(get_forecasts)

        bot.send_message(
            message.chat.id, f"You will receive forecasts for your main places at {time_string}. /cancelreceive to unsubscribe")

    except:
        print("exception")
        bot.send_message(message.chat.id, "Invalid format, try again")

        bot.register_next_step_handler(message, add_forecast_send_time)


@bot.message_handler(commands=["cancelreceive"])
def cancel_receive_forecasts(message):
    url = f"{host_with_protocol}/api/users/{message.chat.id}?user_id={message.chat.id}"

    response = requests.get(url, verify=False)

    if response.status_code == 401:
        bot.send_message(
            message.chat.id, "*You must registrate to use the bot.* /registrate to start", parse_mode="markdown")
        
        return

    user_data["timeToSendWeather"] = None

    try:
        schedule.cancel_job(scheduler_job)
        bot.send_message(message.chat.id, "You have unsubscribed")

    except:
        bot.send_message("Something went wrong")


@bot.message_handler(commands=["createplace"])
def create_place_step_start(message):
    url = f"{host_with_protocol}/api/users/{message.chat.id}?user_id={message.chat.id}"

    response = requests.get(url, verify=False)

    if response.status_code == 401:
        bot.send_message(
            message.chat.id, "*You must registrate to use the bot.* /registrate to start", parse_mode="markdown")
        
        return
        
    place_for_create.update({
        "lat": None,
        "lon": None,
        "name": "",
        "isMain": False,
        "userId": message.chat.id
    })

    sended_message = bot.send_message(
        message.chat.id, "Send your place's location _\(You can do this only from phone\)_", parse_mode="MarkdownV2")

    bot.register_next_step_handler(sended_message, create_place_step_location)


def create_place_step_location(message):
    print(message)

    if not message.location:
        sended_message = bot.send_message(
            message.chat.id, "There is no location, try again")

        bot.register_next_step_handler(
            sended_message, create_place_step_location)

        return

    print(message.location)

    place_for_create["lat"] = message.location.latitude
    place_for_create["lon"] = message.location.longitude

    print(place_for_create)

    sended_message = bot.send_message(
        message.chat.id, "Send your place's name")

    bot.register_next_step_handler(sended_message, create_place_step_name_main)


def create_place_step_commit():
    user_id = place_for_create['userId']

    url = f"{host_with_protocol}/api/Places?user_id={user_id}"

    print(url)

    response = requests.post(url, json=place_for_create, verify=False)

    if not response.ok:
        place_for_create.update({
            "lat": None,
            "lon": None,
            "name": "",
            "isMain": False,
            "userId": user_id
        })

        bot.send_message(user_id, "Something went wrong. Please, try again")

        if response.status_code == 401:
            bot.send_message(user_id, "You are not authorized")

        return

    bot.send_message(
        user_id, f"Place is created, you can change or delete it at [this url]({frontend_host}/{user_id})", parse_mode="markdown")


@bot.callback_query_handler(func=lambda c: c.data == 'set_place_main_true')
def set_place_main_true(call):
    place_for_create["isMain"] = True
    print(place_for_create)
    create_place_step_commit()


@bot.callback_query_handler(func=lambda c: c.data == 'set_place_main_false')
def set_place_main_false(call):
    place_for_create["isMain"] = False
    print(place_for_create)
    create_place_step_commit()


def create_place_step_name_main(message):
    if not message.text:
        sended_message = bot.send_message(
            message.chat.id, "There is not text, try again")

        bot.register_next_step_handler(
            sended_message, create_place_step_name_main)

        return

    place_for_create["name"] = message.text

    buttons = [
        [
            InlineKeyboardButton(
                "Yes", callback_data="set_place_main_true", value=True),
            InlineKeyboardButton(
                "No", callback_data="set_place_main_false", value=False)
        ]
    ]

    keyboard = InlineKeyboardMarkup(buttons)

    sended_message = bot.send_message(
        message.chat.id, "Do you want to make place main? _\(You will receive forecast for this place every day at time, that you set\)_", reply_markup=keyboard, parse_mode="MarkdownV2")

    print(place_for_create)


def scedule_checker():
    while True:
        schedule.run_pending()
        sleep(1)


Thread(target=scedule_checker).start()

bot.infinity_polling()
