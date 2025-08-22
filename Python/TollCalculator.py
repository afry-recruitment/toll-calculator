from datetime import datetime, date
import VehicleTypes
import numpy as np

""" 
    This is a class for calculating the total toll fee for a vechicle one day.
    The class takes a vehicle type and a list of dates as input.
    The script assumes that the dates are within the same day, and are sorted. 
"""

class CalcTollFee:
    def __init__(self, vehicle_type : VehicleTypes.VehicleType, dates : list):
        self.vehicle_type = vehicle_type
        self.dates = dates 

    """ 
    Toll free vehicles: 
        Motorbike, Emergency, Diplomat, Foreign, Military and Tractor
    """
    
    def is_toll_free_vehicle(self, vehicle_enum) -> bool:
        if (vehicle_enum == VehicleTypes.VehicleType.MOTORBIKE or vehicle_enum == VehicleTypes.VehicleType.EMERGENCY or \
           vehicle_enum == VehicleTypes.VehicleType.DIPLOMAT or vehicle_enum == VehicleTypes.VehicleType.FOREIGN \
            or vehicle_enum == VehicleTypes.VehicleType.MILITARY or vehicle_enum == VehicleTypes.VehicleType.TRACTOR):
            return True
        else:
            return False

    """ 
    Toll free dates: 
        Satudays and Sundays 
        January 1st, March 28th, March 29th, April 1st, April 30th,
        May 1st, May 8th, May 9th, June 5th, June 6th, June 21st, 
        ALL July,
        November 1st, December 24th, December 25th, December 26th, December 31st
    """
    
    def is_toll_free_date(self, date) -> bool:
        if date.weekday() in [5, 6]:
            return True 
        elif date.month == 1 and date.day == 1:
            return True
        elif date.month == 3 and date.day in [28, 29]:
            return True
        elif date.month == 4 and date.day in [1, 30]:
            return True
        elif date.month == 5 and date.day in [1, 8, 9]:
            return True
        elif date.month == 6 and date.day in [5, 6, 21]:
            return True
        elif date.month == 7:
            return True
        elif date.month == 11 and date.day == 1:
            return True
        elif date.month == 12 and date.day in [24, 25, 26, 31]:
            return True
    
    def get_time(self, date):
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

    def get_current_toll_fee(self, date): 
        hours, minutes = self.get_time(date)

        if (hours == 6 and minutes <= 29) or (hours == 18 and minutes <= 29):
            new_toll_fee = 8.0
        elif ((hours == 6 and minutes >= 30) or (hours == 8 and minutes <= 29) or
            (hours == 15 and minutes <= 29) or (hours == 17)):
            new_toll_fee = 13.0
        elif (hours == 7) or (hours == 15 and minutes >= 30) or (hours == 16):
            new_toll_fee = 18.0
        else:
            new_toll_fee = 0.0

        return new_toll_fee

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
            

    def calc_toll_fee(self):

        # Check if vehicle type or date is toll free
        if self.is_toll_free_vehicle(self.vehicle_type) or self.is_toll_free_date(self.dates[0]):
            return 0.0
        
        if len(self.dates) == 1:
            return self.get_current_toll_fee(self.dates[0])
        
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

        return total_toll_fee
     
