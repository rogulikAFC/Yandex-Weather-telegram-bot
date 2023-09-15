from time import sleep
from threading import Thread
import schedule


scheduler_job = None

def scedule_checker():
    while True:
        schedule.run_pending()
        sleep(1)

Thread(target=scedule_checker).start()