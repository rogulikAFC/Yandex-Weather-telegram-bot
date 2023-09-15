import requests
import schedule
from app.util.objects import user_data
from app import bot, host_with_protocol


@bot.message_handler(commands=["cancelreceive"])
def cancel_receive_forecasts(message):
    """
    Cancels forecasts distribution
    """

    url = f"{host_with_protocol}/api/users/{message.chat.id}?user_id={message.chat.id}"

    response = requests.get(url, verify=False)

    if response.status_code == 401:
        bot.send_message(
            message.chat.id, "*You must registrate to use the bot.* /registrate to start", parse_mode="markdown")
        
        return

    user_data["timeToSendWeather"] = None

    try:
        schedule.clear()
        bot.send_message(message.chat.id, "You have unsubscribed")

    except:
        bot.send_message("Something went wrong")
