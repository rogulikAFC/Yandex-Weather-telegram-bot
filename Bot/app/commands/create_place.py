import requests
from telebot.types import InlineKeyboardButton, InlineKeyboardMarkup
from app.util.objects import place_for_create
from app import bot, host_with_protocol, frontend_host


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
    if not message.location:
        sended_message = bot.send_message(
            message.chat.id, "There is no location, try again")

        bot.register_next_step_handler(
            sended_message, create_place_step_location)

        return

    place_for_create["lat"] = message.location.latitude
    place_for_create["lon"] = message.location.longitude

    sended_message = bot.send_message(
        message.chat.id, "Send your place's name")

    bot.register_next_step_handler(sended_message, create_place_step_main)


def create_place_step_commit():
    user_id = place_for_create['userId']

    url = f"{host_with_protocol}/api/Places?user_id={user_id}"

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
    create_place_step_commit()


@bot.callback_query_handler(func=lambda c: c.data == 'set_place_main_false')
def set_place_main_false(call):
    place_for_create["isMain"] = False
    create_place_step_commit()


def create_place_step_main(message):
    if not message.text:
        sended_message = bot.send_message(
            message.chat.id, "There is not text, try again")

        bot.register_next_step_handler(
            sended_message, create_place_step_main)

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
