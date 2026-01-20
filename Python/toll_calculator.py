from vehicle import Vehicle
from datetime import datetime, time
from dataclasses import dataclass
from helper.calender import SwedishCalendar

"""The fee hours in the TollCalculator class is based of the logic found in Java/TollCalculator.java
 1. The "Rush Hour" (18 SEK)
 Morning (07:00-07:59):
  §57: else if (hour == 7 && minute >= 0 && minute <= 59) return 18;
 Afternoon(15:30-16:59):
  §61: else if (hour == 15 && minute >= 0 || hour == 16 && minute <= 59) return 18;
 2. The "Standard Fee" (13 SEK)
 Morning (06:30-06:59 & 08:00-08:29):
  §56: else if (hour == 6 && minute >= 30 && minute <= 59) return 13;
  §58: else if (hour == 8 && minute >= 0 && minute <= 29) return 13;
 Afternoon (15:00-15:29 & 17:00-17:59):
  §60: else if (hour == 15 && minute >= 0 && minute <= 29) return 13;
  §62: else if (hour == 17 && minute >= 0 && minute <= 59) return 13;
 3. The "Low Fee" (8 SEK)
 Morning/Day (06:00-06:29 & 08:30-14:59):
  §55: if (hour == 6 && minute >= 0 && minute <= 29) return 8;
  §59: else if (hour >= 8 && hour <= 14 && minute >= 30 && minute <= 59) return 8;
   The found logic currently induces a bug where no fee (0 SEK)
   is charged during the first 30 minutes of every hour between 09:00 and 14:00.
 Evening (18:00-18:29):
  §63: else if (hour == 18 && minute >= 0 && minute <= 29) return 8;
 4. No Fee (0 SEK)
  All hours except the ones listed above.
"""

@dataclass(frozen=True)
class FeeRange:
    start: time
    end: time
    fee: int

class TollCalculator:
    def __init__(self):
        self.calender = SwedishCalendar().load_holidays()

    FEE_SCHEDULE = [
        FeeRange(start=time(6, 0),  end=time(6, 29,59), fee=8),
        FeeRange(start=time(6, 30), end=time(6, 59,59), fee=13),
        FeeRange(start=time(7, 0),  end=time(7, 59,59), fee=18), # Morning Rush
        FeeRange(start=time(8, 0),  end=time(8, 29,59), fee=13),
        FeeRange(start=time(8, 30), end=time(14,59,59), fee=8),  # Fixed mid-day gap
        FeeRange(start=time(15, 0), end=time(15,29,59), fee=13),
        FeeRange(start=time(15, 30),end=time(16,59,59), fee=18), # Afternoon Rush
        FeeRange(start=time(17, 0), end=time(17,59,59), fee=13),
        FeeRange(start=time(18, 0), end=time(18,29,59), fee=8),
    ]

    def _get_toll_fee(self, vehicle: Vehicle, date: datetime) -> int:
        if self._is_toll_free_date(date) or vehicle.is_toll_free():
            return 0
        passing_time = date.time()

        for fee_range in self.FEE_SCHEDULE:
            if fee_range.start <= passing_time <= fee_range.end:
                return fee_range.fee
        return 0
    
    def _is_toll_free_date(self, date: datetime) -> bool:
        return self.calender.is_holiday_or_weekend(date)
        
    def calculate_total_fee(self, vehicle: Vehicle, dates: list[datetime]) -> int:
        """Calculate total toll fee for the given vehicle & list of dates.
        Requirements:
        - 60-minute rolling window: only the highest fee within any 60-minute period is charged.
        - Daily max: 60 SEK per vehicle.
        """
        if not dates:
            return 0
        sorted_dates = sorted(dates)
        total_fee = 0
        interval_start = sorted_dates[0]
        current_highest_fee = self._get_toll_fee(vehicle, interval_start)

        # If available, we start at the second pass of the day, otherwise use only the first pass to calculate the fee.
        for i in range(1, len(sorted_dates)):
            next_date = sorted_dates[i]
            next_fee = self._get_toll_fee(vehicle, next_date)
            diff_in_seconds = (next_date - interval_start).total_seconds()
            minutes = diff_in_seconds / 60
            # Check if within the 60-minute interval
            if minutes <= 60:
                if next_fee > current_highest_fee:
                    current_highest_fee = next_fee
            else:
                total_fee += current_highest_fee
                interval_start = next_date
                current_highest_fee = next_fee

        total_fee += current_highest_fee
        return min(total_fee, 60)  # Daily max fee is 60 SEK
        