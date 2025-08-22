import TollCalculator
import VehicleTypes
from datetime import datetime

def main():

    # Generate test dates 
    date1 = datetime(2025, 7, 22, 6, 0, 0)
    date2 = datetime(2025, 7, 22, 7, 6, 0)
    date3 = datetime(2025, 7, 22, 8, 14, 0)
    date4 = datetime(2025, 7, 22, 14, 0, 0)
    date5 = datetime(2025, 7, 22, 15, 0, 0)
    date6 = datetime(2025, 7, 22, 15, 3, 0)
    date7 = datetime(2025, 7, 22, 15, 4, 0)
    date8 = datetime(2025, 7, 22, 15, 7, 0)
    date9 = datetime(2025, 7, 22, 17, 30, 0)
    date10 = datetime(2025, 7, 22, 18, 31, 0)
    dates = [date1, date2, date3, date4, date5, date6, date7, date8, date9, date10]

    vehicle_type = VehicleTypes.VehicleType.CAR
    toll_class = TollCalculator.CalcTollFee(vehicle_type, dates)
    
    total_toll_fee = toll_class.calc_toll_fee()
    print(f"Total toll fee cost: {total_toll_fee} SEK")

main()
