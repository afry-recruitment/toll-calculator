from datetime import datetime, timedelta
import json
import os

import holidays

from vehicle import Vehicle


class TollCalculator:
    def __init__(self) -> None:
        self.swedish_holidays: holidays.HolidayBase = self.get_swedish_holidays()
        config: dict = self.load_config()
        try:
            self.toll_free_vehicles: set = set(config["toll_free_vehicles"])
            self.toll_schedule: list = config["toll_schedule"]
        except KeyError as e:
            raise KeyError(f"config.json is missing required key: {e}")


    def load_config(self) -> dict:
        config_path = os.path.join(os.path.dirname(__file__), "config.json")
        try:
            with open(config_path) as f:
                return json.load(f)
        except FileNotFoundError:
            raise FileNotFoundError("config.json is not found. Make sure the file exists in the same directory as toll_calculator.py")


    def get_swedish_holidays(self, year: int | None = None) -> holidays.HolidayBase:
        if year is None:
            year = datetime.now().year
        return holidays.Sweden(years=year)


    def get_toll_fee(self, vehicle: Vehicle, dates: list[datetime]) -> int:
        if self.is_toll_free_vehicle(vehicle):
            return 0
        
        if not dates:
            return 0
        
        sorted_dates = sorted(dates)
        total_fee = 0
        interval_start = sorted_dates[0]
        window_fees = []

        for date in sorted_dates:
            fee = self.get_toll_fee_for_pass(vehicle, date)
            if date - interval_start <= timedelta(minutes=60):
                window_fees.append(fee)
            else:
                total_fee += max(window_fees)
                interval_start = date
                window_fees = [fee]
        
        total_fee += max(window_fees)
        return min(total_fee, 60)


    def is_toll_free_vehicle(self, vehicle: Vehicle) -> bool:
        if vehicle is None:
            return False
        return vehicle.get_vehicle_type() in self.toll_free_vehicles


    def get_toll_fee_for_pass(self, vehicle: Vehicle, date: datetime) -> int:
        if self.is_toll_free_vehicle(vehicle) or self.is_toll_free_date(date):
            return 0
        
        current_time = (date.hour, date.minute)

        for entry in self.toll_schedule:
            from_time = tuple(int(x) for x in entry["from"].split(":"))
            to_time = tuple(int(x) for x in entry["to"].split(":"))
            if from_time <= current_time <= to_time:
                return entry["fee"]

        return 0


    def is_toll_free_date(self, date: datetime) -> bool:
        if date.weekday() >= 5:
            return True
        
        if date.year not in self.swedish_holidays.years:
            self.swedish_holidays.update(self.get_swedish_holidays(date.year))

        return date in self.swedish_holidays
