import requests
from app import bot, host_with_protocol

@bot.message_handler(commands=['start'])
def send_welcome(message):
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