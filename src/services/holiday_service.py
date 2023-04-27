import datetime

from src.model.holiday import Holiday


def is_today_a_holiday_or_weekend() -> bool:
    today = datetime.date.today()
    holidays = Holiday.objects(date=today).count()

    if holidays > 0:
        return True

    if today.weekday() in [5, 6]:
        return True

    return False
