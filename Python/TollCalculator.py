from datetime import datetime
from Vehicle import Vehicle

class TollFreeVehicles:
    Motorbike = "Motorbike"
    Tractor = "Tractor"
    Emergency = "Emergency"
    Diplomat = "Diplomat"
    Foreign = "Foreign"
    Military = "Military"

class TollCalculator:

    def get_toll_fee(self, vehicle, dates):
        interval_start = dates[0]
        total_fee = 0
        for date in sorted(dates):
            next_fee = self._get_toll_fee(date, vehicle)
            temp_fee = self._get_toll_fee(interval_start, vehicle)

            diff_in_millis = (date - interval_start).total_seconds() * 1000
            minutes = diff_in_millis / 1000 / 60

            if minutes <= 60:
                if total_fee > 0:
                    total_fee -= temp_fee
                if next_fee >= temp_fee:
                    temp_fee = next_fee
                total_fee += temp_fee
            else:
                total_fee += next_fee
                interval_start = date

        if total_fee > 60:
            total_fee = 60
        return total_fee

    def _is_toll_free_vehicle(self, vehicle):
        if vehicle is None:
            return False
        vehicle_type = vehicle.get_vehicle_type()
        return vehicle_type in [TollFreeVehicles.Motorbike, TollFreeVehicles.Tractor, 
                                TollFreeVehicles.Emergency, TollFreeVehicles.Diplomat, 
                                TollFreeVehicles.Foreign, TollFreeVehicles.Military]

    def _get_toll_fee(self, date, vehicle):
        if self._is_toll_free_date(date) or self._is_toll_free_vehicle(vehicle):
            return 0

        hour = date.hour
        minute = date.minute

        if hour == 6 and 0 <= minute <= 29: return 8
        elif hour == 6 and 30 <= minute <= 59: return 13
        elif hour == 7 and 0 <= minute <= 59: return 18
        elif hour == 8 and 0 <= minute <= 29: return 13
        elif 9 <= hour <= 14 and 30 <= minute <= 59: return 8
        elif hour == 15 and 0 <= minute <= 29: return 13
        elif (hour == 15 and 30 <= minute) or (hour == 16 and minute <= 59): return 18
        elif hour == 17 and 0 <= minute <= 59: return 13
        elif hour == 18 and 0 <= minute <= 29: return 8
        else: return 0

    def _is_toll_free_date(self, date):
        if date.weekday() >= 5:
            return True

        # Assuming the year 2013 for toll-free dates
        if date.year == 2013:
            if (date.month == 1 and date.day == 1) or \
               (date.month == 3 and (date.day == 28 or date.day == 29)) or \
               (date.month == 4 and (date.day == 1 or date.day == 30)) or \
               (date.month == 5 and (date.day == 1 or date.day == 8 or date.day == 9)) or \
               (date.month == 6 and (date.day == 5 or date.day == 6 or date.day == 21)) or \
               (date.month == 7) or \
               (date.month == 11 and date.day == 1) or \
               (date.month == 12 and (date.day == 24 or date.day == 25 or date.day == 26 or date.day == 31)):
                return True

        return False