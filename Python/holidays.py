
""" 

    This class checks if a date is a holiday, with class check_date. The function returns a boolean.
    The holidays Easter and Kristihimmelsfärds are hardcoded for years 2025-2029.

    Always toll free dates: 
        Satudays and Sundays 
        January 1st, January 5th, January 6th (Trettondagen) 
        April 30th (Valborgsmässoafton), 
        May 1st, 
        June 5th, June 6th,  
        ALL July,
        December 24th, December 25th, December 26th, December 31st

    Flexibel toll free dates:
        Skärtorsdagen, Långfredagen, Annandagen
        Kristihimmelfärdsdagen and the day before, 
        Midsommarafton, 
        Alla helgonsafton (Friday between 31th of October and 6th of November)
"""

class Holidays(): 
    def __init__(self, year, date):
        self.year = year
        self.date = date

    def is_always_holiday(self) -> bool:
        date = self.date
        if date.weekday() in [5, 6]:
            return True 
        if date.month == 1 and date.day in [1, 5, 6]:
            return True
        if date.month == 4 and date.day == 30: 
            return True
        if date.month == 5 and date.day == 1:
            return True
        if date.month == 6 and date.day in [5, 6]:
            return True
        if date.month == 7:
            return True
        if date.month == 12 and date.day in [24, 25, 26, 31]:
            return True
        else:
            return False
        
    def is_easter(self) -> bool:
        date = self.date
        if self.year == 2025 and date.month == 4 and date.day in [17, 18,21]:
            return True
        if self.year == 2026 and date.month == 4 and date.day in [2, 3,6]:
            return True
        if self.year == 2027 and date.month == 3 and date.day in [25, 26,29]:
            return True
        if self.year == 2028 and date.month == 4 and date.day in [13, 14,17]:
            return True
        if self.year == 2029 and date.month == 3 and date.day in [30, 31]:
            return True
        if self.year == 2029 and date.month == 4 and date.day == 1:
            return True
        else:
            return False
    
    def is_kristihimmelfard(self) -> bool: 
        date = self.date
        if self.year == 2025 and date.month == 5 and date.day in [28, 29]:
            return True
        if self.year == 2026 and date.month == 5 and date.day == [13, 14]:    
            return True
        if self.year == 2027 and date.month == 5 and date.day == [5,6]:
            return True
        if self.year == 2028 and date.month == 5 and date.day == [24,25]:    
            return True
        if self.year == 2029 and date.month == 5 and date.day == [9,10]:
            return True
        else:
            return False

    def is_midsommer(self) -> bool:
        date = self.date
        if date.month == 6 and date.day in [20,21,22,23,24,25,26] and date.weekday() == 4:
            return True 
        else:
            return False
        
    def is_allhelgona(self) -> bool:
        date = self.date
        if date.weekday == 4 and date.month == 10 and date.day in [30, 31]:
            return True
        if date.weekday == 4 and date.month == 11 and date.day in [1,2,3,4,5]:
            return True
        else:
            return False
        

    def check_date(self) -> bool:
        if self.is_always_holiday() or self.is_easter() or self.is_kristihimmelfard() \
            or self.is_midsommer() or self.is_allhelgona():
            return True
        else:
            return False