from datetime import datetime, timedelta
import sys
import holidays

class TollCalculator:
    """
    TollCalculator class that calculates toll fees and holds information for different dates.
    """
    def __init__(self, dataset):
        self.dates_and_fees = {}
        with open(dataset) as file:
            vehicle_entries = file.readlines()

        def sort_entries(date: str):
            return datetime.strptime(date.strip().split(',')[1], '%Y-%m-%dT%H:%M:%S')

        vehicle_entries_sorted = sorted(vehicle_entries, key=sort_entries)

        hour_period_start = None
        hour_period = None
        last_toll_fee = 0

        for vehicle_entry in vehicle_entries_sorted:
            vehicle_entry = vehicle_entry.strip()
            entry_vehicle_type = vehicle_entry.split(',')[0]
            entry_date = datetime.strptime(vehicle_entry.split(',')[1], '%Y-%m-%dT%H:%M:%S')
            if hour_period_start is None:
                hour_period_start = entry_date
                print(f"[DEBUG] first hour_period_start: {hour_period_start}")
                hour_period = hour_period_start + timedelta(hours=1)
                print(f"[DEBUG] first hour_period: {hour_period}")
            if not entry_date.date() in self.dates_and_fees:
                self.dates_and_fees[entry_date.date()] = 0
            print(f"[DEBUG] vehicle: {entry_vehicle_type}, time: {entry_date}")
            toll_fee = self.get_toll_fee_for_entry(entry_vehicle_type, entry_date)
            print(f"[DEBUG] tollfee for entry: {toll_fee}")
            if entry_date < hour_period:
                print("[DEBUG] entry is within hour period")
                if last_toll_fee < toll_fee:
                    print(f"[DEBUG] toll_fee is higher than last_toll_fee. Remove last_toll_fee ({last_toll_fee}) from total_fee on entry_date")
                    self.dates_and_fees[entry_date.date()] -= last_toll_fee
                    print(f"[DEBUG] add {toll_fee} to total_fee on entry_date")
                    self.dates_and_fees[entry_date.date()] += toll_fee
                else:
                    print("[DEBUG] toll_fee is less or equal than last_toll_fee. Do not add fee")
            else:
                hour_period = entry_date + timedelta(hours=1)
                print(f"[DEBUG] entry is not within hour period. New hour period: {hour_period}")
                print(f"[DEBUG] add {toll_fee} to total_fee on entry_date")
                self.dates_and_fees[entry_date.date()] += toll_fee
            if self.dates_and_fees[entry_date.date()] > 60:
                print(f"[DEBUG] Total toll fee is higher than 60 sek for current date: {self.dates_and_fees[entry_date.date()]}")
                self.dates_and_fees[entry_date.date()] = 60
            last_toll_fee = toll_fee
            print("---------------")

    def get_toll_fee_for_entry(self, vehicle: str, date: datetime) -> int:
        """
        Calculate toll fee for one entry.
        """
        toll_fee_for_entry = 0
        if vehicle in ["motorbike", "tractor", "emergency", "diplomat", "foreign", "military"]:
            print(f"[DEBUG] vehicle is toll free: {vehicle}")
            return 0

        holidays_swe = holidays.country_holidays('SE')

        if date in holidays_swe:
            print(f"[DEBUG] date is a holiday: {date}")
            return 0
        if date.weekday() == 5 or date.weekday() == 6:
            print(f"[DEBUG] date is a weekend: {date}")
            return 0
        toll_fee_for_entry = self.get_period_toll_fee(date)
        return toll_fee_for_entry

    def get_period_toll_fee(self, date: datetime) -> int:
        """
        Get toll fee based on time of day.
        """
        period_toll_fee = 0
        if date.hour == 6 and date.minute <= 29:
            print("[DEBUG] 06:00 => 06:29 tollfee: 8")
            period_toll_fee = 8
        elif date.hour == 6 and date.minute >= 30:
            print("[DEBUG] 06:30 => 06:59 tollfee: 13")
            period_toll_fee = 13
        elif date.hour == 7:
            print("[DEBUG] 07:00 => 07:59 tollfee: 18")
            period_toll_fee = 18
        elif date.hour == 8 and date.minute <= 29:
            print("[DEBUG] 08:00 => 08:29 tollfee: 13")
            period_toll_fee = 13
        elif date.hour == 8 and date.minute >= 30:
            print("[DEBUG] 08:30 => 08:59 tollfee: 8")
            period_toll_fee = 8
        elif date.hour >=9 and date.hour <= 14:
            print("[DEBUG] 09:00 => 14:59 tollfee: 8")
            period_toll_fee = 8
        elif date.hour == 15 and date.minute <= 29:
            print("[DEBUG] 15:00 => 15:29 tollfee: 13")
            period_toll_fee = 13
        elif date.hour == 15 and date.minute >= 30:
            print("[DEBUG] 15:30 => 15:59 tollfee: 18")
            period_toll_fee = 18
        elif date.hour == 16:
            print("[DEBUG] 16:00 => 16:59 tollfee: 18")
            period_toll_fee = 18
        elif date.hour == 17:
            print("[DEBUG] 17:00 => 17:59 tollfee: 13")
            period_toll_fee = 13
        elif date.hour == 18 and date.minute <= 29:
            print("[DEBUG] 18:00 => 18:29 tollfee: 8")
            period_toll_fee = 8
        return period_toll_fee

if __name__ == "__main__":
    try:
        toll_calculator = TollCalculator(sys.argv[1])
    except IndexError as e:
        print("No dataset selected")
        sys.exit()
    print(toll_calculator.dates_and_fees)
