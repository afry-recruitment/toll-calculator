from datetime import datetime
import json
import os

class SwedishCalendar:
    def __init__(self):
        self.base_path = os.path.dirname(__file__)
        self.json_path = os.path.join(self.base_path, "holidays_se.json")
    
    def load_holidays(self):
        with open(self.json_path, "r", encoding="utf-8") as f:
            data = json.load(f)
            self.free_months = set(data.get("toll_free_months", []))
            self.free_dates = set(data.get("toll_free_dates", []))
        return self
    
    def is_holiday_or_weekend(self, date: datetime) -> bool:
        if date.weekday() >= 5:  # Saturday=5, Sunday=6
            return True
        if date.month in self.free_months:
            return True
        if date.strftime("%Y-%m-%d") in self.free_dates:
            return True
        return False