import random, json
import VehicleTypes
from datetime import datetime


""" This is a file for creating input data (if needed). 
    The input data is a json file with a dicttionary. 
    The keys are the registration number, and the values are a list including the vehicle type and a list of dates."""

""" In this example, 20 vechicles are created of random type and registration number, with random timestamps through out the day. 
    The dates are sorted, and the maximum nr of rides are 8 per vehicle and day.
    The dates are in the June 3rd, 4th and 5th (Holiday). """

year, month, day = 2026, 4, 6
input_data_dict = {}
for _ in range(20): 
    reg_nr = "".join(chr(random.randint(65, 90)) for _ in range(3))  + "".join(str(random.randint(0, 9)) for _ in range(3))
    vehicle_type = random.choice(list(VehicleTypes.VehicleType))
    nr_rides = random.randint(1, 8)
    dates = []
    for n in range(nr_rides):
        date = datetime(year, month, day, random.randint(4, 23), random.randint(0, 59), random.randint(0, 59))
        dates.append(date)
    dates.sort()
    input_data_dict[reg_nr] = [vehicle_type.value, [d.isoformat() for d in dates]]

    with open(f"input_data_{year}_{month}_{day}.json", "w") as f:
        json.dump(input_data_dict, f)
