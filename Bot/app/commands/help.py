from app import bot, frontend_host


@bot.message_handler(commands=["help"])
def help(message):
    bot.send_message(
        message.chat.id,
        f"*List of commands*:\n\n/registrate \- registration\n\n/createplace \- create a place\n\n/settime \- set time, in which you will receive forecasts in your places, marked main\n\n/cancelreceive \- cancels forecasts sending\n\n\n_You can change your places at [this]({frontend_host}/{message.chat.id}) URL_",
        parse_mode="markdownv2")
