from dotenv import dotenv_values
from telebot import TeleBot


config_values = dotenv_values('.env')

bot = TeleBot(config_values["TG_BOT_KEY"])
host_with_protocol = config_values["TG_BOT_HOST_WITH_PROTOCOL"]
frontend_host = config_values["FRONTEND_HOST"]  # localhost doesn't available

from .util import scheduler
from .util import objects

from .commands import help
from .commands import start
from .commands import registrate
from .commands import get_weather
from .commands import set_time
from .commands import cancel_receive_forecasts
from .commands import create_place

bot.infinity_polling()