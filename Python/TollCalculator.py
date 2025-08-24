from datetime import datetime
from holidays import Holidays
import VehicleTypes
import numpy as np

""" 
    This is a class for calculating the total toll fee for a vechicle one day.
    The class takes a vehicle type and a list of dates as input.
    The script assumes that the dates are within the same day, and are sorted. 
"""

class TollCalculator:
    def __init__(self, vehicle_type : VehicleTypes.VehicleType, dates : list):
        self.vehicle_type = vehicle_type
        self.dates = dates 

    """ 
    Toll free vehicles: 
        Motorbike, Emergency, Diplomat, Foreign, Military and Tractor
    """
    
    def isTollFreeVehicle(self, vehicle_enum) -> bool:
        if (vehicle_enum == VehicleTypes.VehicleType.MOTORBIKE or vehicle_enum == VehicleTypes.VehicleType.EMERGENCY or \
           vehicle_enum == VehicleTypes.VehicleType.DIPLOMAT or vehicle_enum == VehicleTypes.VehicleType.FOREIGN \
            or vehicle_enum == VehicleTypes.VehicleType.MILITARY or vehicle_enum == VehicleTypes.VehicleType.TRACTOR):
            return True
        else:
            return False

    """ 
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
        Kristihimmelfärdsdagen, 
        Midsommarafton, 
        Alla helgons afton (Friday between 31th of October and 6th of November)
    """
    
    def isTollFreeDate(self, date) -> bool:
        if Holidays(date.year, date).check_date():
            return True
        else:
            return False
    
    def get_time(self, date) -> tuple:
        return date.hour, date.minute
    
    
    """ 
    Cost each hour:
        6:00 - 6:29 = 8 
        6:30 - 6:59 = 13 
        7:00 - 7:59 = 18 
        8:00 - 8:29 = 13 
        8:30 - 14:59 = 8
        15:00 - 15:29 = 13
        15:30 - 16:59 = 18
        17:00 - 17:59 = 13
        18:00 - 18:29 = 8
        else 0 
    
    """

    def get_current_toll_fee(self, date) -> float: 
        hours, minutes = self.get_time(date)

        if (hours == 6 and minutes <= 29) or (hours == 18 and minutes <= 29):
            return 8.0
        if ((hours == 6 and minutes >= 30) or (hours == 8 and minutes <= 29) or
            (hours == 15 and minutes <= 29) or (hours == 17)):
            return 13.0
        if (hours == 7) or (hours == 15 and minutes >= 30) or (hours == 16):
            return 18.0
        if (hours == 8 and minutes >= 30) or ( 9 <= hours <= 14 and minutes <= 59) or (hours == 18 and minutes <= 29):
            return 8.0
        else:
            return 0.0

    """ This function returns a list of Booleans: 
        True if the time stamp is within one hour of the previous one
        else False. 
    """
    
    def calc_timestamps_diff(self, dates) -> list: 
        time_diff_lst = []
        for n, date in enumerate(dates):

            if not isinstance(date, datetime):
                raise TypeError("The date should be of type datetime.")
            
            if n == 0:
                time_diff_lst.append(False)
                continue

            diff = date - dates[n-1]
            diff_min = np.abs(diff.total_seconds() / 60)
            if diff_min <= 60:
                time_diff_lst.append(True)
            else:
                time_diff_lst.append(False)

        return time_diff_lst
            

    """ 
        The function calc_toll_fee calculates the total toll fee for a vehicle on a given date.
        The output is a list with two elements:
        - The date in question 
        - The total toll fee 
    """

    def calc_toll_fee(self) -> list:
        final_date = self.dates[0].date().isoformat()

        # Check if vehicle type or date is toll free
        if self.isTollFreeVehicle(self.vehicle_type) or self.isTollFreeDate(self.dates[0]):
            return [final_date, 0.0]
        
        if len(self.dates) == 1:
            return [final_date, self.get_current_toll_fee(self.dates[0])]
        
        total_toll_fee = 0.0
        time_diff_lst = self.calc_timestamps_diff(self.dates)

        for n, date in enumerate(self.dates):
            # If timestamp not within one hour of the previous one, add toll fee
            if not time_diff_lst[n]:
                
                total_toll_fee += self.get_current_toll_fee(date)
                continue

            # Else we must compare to previous one 
            new_toll_fee = self.get_current_toll_fee(date)
            prev_toll_fee = self.get_current_toll_fee(self.dates[n-1])
            if new_toll_fee > prev_toll_fee:
                total_toll_fee -= prev_toll_fee
                total_toll_fee += new_toll_fee

        # Max total fee per day is 60.0
        if total_toll_fee > 60.0:
            total_toll_fee = 60.0

        return [final_date, total_toll_fee]
     
