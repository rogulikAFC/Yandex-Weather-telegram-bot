import telebot

bot = telebot.TeleBot("5904545227:AAG_ByuFVp5872Z5xe4gcQeHLnxkZa_k7Dc")

@bot.message_handler(commands=['start'])
def send_welcome(message):
    print(message)

    bot.reply_to(message, "Hi!")

bot.infinity_polling()
