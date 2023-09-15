import datetime
import requests
import schedule
from app.util.objects import user_data
from app import bot, host_with_protocol
from .get_weather import get_forecasts


@bot.message_handler(commands=['settime'])
def add_forecast_send_time_start(message):
    """
    Sets time, in which user gets forecasts
    """

    url = f"{host_with_protocol}/api/users/{message.chat.id}?user_id={message.chat.id}"

    response = requests.get(url, verify=False)

    if response.status_code == 401:
        bot.send_message(
            message.chat.id, "*You must registrate to use the bot.* /registrate to start", parse_mode="markdown")

        return

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