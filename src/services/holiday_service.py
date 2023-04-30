import datetime
import logging

from src.model.holiday import Holiday


def is_today_a_holiday_or_weekend() -> bool:
    today = datetime.date.today()
    holidays = Holiday.objects(date=today).count()

    if holidays > 0:
        logging.info('Holiday, Vehicle can park without charging')
        return True

    if today.weekday() in [5, 6]:
        logging.info('Weekends, Vehicle can park without charging')
        return True

    return False
