from typing import List
from datetime import datetime, time
from itertools import groupby
from bisect import bisect_left
import holidays

# start, end, fee
FEE_TIME_TABLE = sorted(
    [
        (time(6, 0), time(6, 29), 8),
        (time(6, 30), time(6, 59), 13),
        (time(7, 0), time(7, 59), 18),
        (time(8, 0), time(8, 29), 13),
        (time(8, 30), time(8, 59), 8),
        (time(9, 30), time(9, 59), 8),
        (time(10, 30), time(10, 59), 8),
        (time(11, 30), time(11, 59), 8),
        (time(12, 30), time(12, 59), 8),
        (time(13, 30), time(13, 59), 8),
        (time(14, 30), time(14, 59), 8),
        (time(15, 0), time(15, 29), 13),
        (time(15, 30), time(16, 59), 18),
        (time(17, 0), time(17, 59), 13),
        (time(18, 0), time(18, 29), 8),
    ],
    key=lambda x: x[0],
)
_TIME_INTERVAL_STARTS = [interval[0] for interval in FEE_TIME_TABLE]

MAX_FEE_PER_DAY = 60

_TOLL_FREE_VEHICLES = [
    "Motorbike",
    "Tractor",
    "Emergency",
    "Diplomat",
    "Foreign",
    "Military",
]

POSSIBLE_VEHICLES = _TOLL_FREE_VEHICLES + ["Car"]


def get_toll_fee(vehicle: str, dates: List[datetime]):
    if _is_toll_free_vehicle(vehicle):
        return 0

    sorted_datetimes = sorted(dates, key=lambda dt: dt.date())
    grouped_by_date = groupby(sorted_datetimes, key=lambda dt: dt.date())

    return sum(_get_sum_per_day(list(datetimes)) for _, datetimes in grouped_by_date)


def _get_sum_per_day(times_during_day: List[datetime]):
    if _is_toll_free_day(times_during_day[0]):
        return 0

    grouped_by_hour = groupby(times_during_day, key=lambda dt: dt.hour)
    day_fee = sum(
        _get_highest_fee_per_hour(list(times)) for _, times in grouped_by_hour
    )
    if day_fee > MAX_FEE_PER_DAY:
        return MAX_FEE_PER_DAY
    return day_fee


def _get_highest_fee_per_hour(times_during_hour: List[datetime]):
    return max(
        _get_fee_for_time(time_of_day.time()) for time_of_day in times_during_hour
    )


def _get_fee_for_time(time_of_day: time):
    # using a binary search to find time interval the time of the toll fee falls into

    time_hours_minutes = time(time_of_day.hour, time_of_day.minute, 0, 0)
    idx = bisect_left(_TIME_INTERVAL_STARTS, time_hours_minutes)

    if (
        idx < len(_TIME_INTERVAL_STARTS)
        and FEE_TIME_TABLE[idx][0] == time_hours_minutes
    ):
        return FEE_TIME_TABLE[idx][2]  # exact match
    if idx > 0:
        start, end, value = FEE_TIME_TABLE[idx - 1]  # interval to the left
        if start <= time_hours_minutes <= end:
            return value
    return 0  # time outside of interval


def _is_toll_free_vehicle(vehicle_type: str):
    return vehicle_type in _TOLL_FREE_VEHICLES


def _is_toll_free_day(date: datetime):
    return date in holidays.country_holidays("SE") or date.weekday() >= 5
