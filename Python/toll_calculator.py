from datetime import time, timedelta
from enum import Enum
import holidays

# Fee schedule per the city's requirements. Start inclusive, end exclusive.
# The schedule differs from the C# original, see README.

FEES = [
    (time(6, 0), time(6, 30), 8),
    (time(6, 30), time(7, 0), 13),
    (time(7, 0), time(8, 0), 18),
    (time(8, 0), time(8, 30), 13),
    (time(8, 30), time(15, 0), 8),
    (time(15, 0), time(15, 30), 13),
    (time(15, 30), time(17, 0), 18),
    (time(17, 0), time(18, 0), 13),
    (time(18, 0), time(18, 30), 8),
]


class VehicleType(Enum):
    CAR = "Car"
    MOTORBIKE = "Motorbike"
    TRACTOR = "Tractor"
    EMERGENCY = "Emergency"
    DIPLOMAT = "Diplomat"
    FOREIGN = "Foreign"
    MILITARY = "Military"


TOLL_FREE_VEHICLES = frozenset({
    VehicleType.MOTORBIKE,
    VehicleType.TRACTOR,
    VehicleType.EMERGENCY,
    VehicleType.DIPLOMAT,
    VehicleType.FOREIGN,
    VehicleType.MILITARY,
})


SWEDISH_HOLIDAYS = holidays.Sweden()
MAX_DAILY_FEE = 60


def fee_for_time(passage):
    t = passage.time()
    for start, end, fee in FEES:
        if start <= t < end:
            return fee
    return 0


def is_toll_free_date(passage):
    return passage.weekday() >= 5 or passage.date() in SWEDISH_HOLIDAYS


def calculate_daily_fee(vehicle_type, passages):
    if vehicle_type in TOLL_FREE_VEHICLES:
        return 0
    if not passages:
        return 0
    ordered = sorted(passages)
    if is_toll_free_date(ordered[0]):
        return 0

    total = 0
    window_start = ordered[0]
    window_max = fee_for_time(ordered[0])

    for passage in ordered[1:]:
        if passage - window_start < timedelta(hours=1):
            window_max = max(window_max, fee_for_time(passage))
        else:
            total += window_max
            window_start = passage
            window_max = fee_for_time(passage)

    total += window_max
    return min(total, MAX_DAILY_FEE)
