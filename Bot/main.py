import telebot

bot = telebot.TeleBot("5904545227:AAG_ByuFVp5872Z5xe4gcQeHLnxkZa_k7Dc")

@bot.message_handler(commands=['start'])
def send_welcome(message):
    print(message)

    bot.reply_to(message,
                 """
Hello! 👋🏻
I'm bot, that gets info about weather in your place.
I've developed by [rogulik](https://github.com/rogulikAFC/Yandex-Weather-telegram-bot)
""",
                parse_mode="markdown")

bot.infinity_polling()
