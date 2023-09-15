import requests
from telebot.types import InlineKeyboardButton, InlineKeyboardMarkup
from app import bot, frontend_host, host_with_protocol


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
