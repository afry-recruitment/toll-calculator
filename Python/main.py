import tollCalculator
import VehicleTypes
from datetime import datetime
import json 

""" 
    This main file takes a json file with input data and calculates the total toll fee per vehicle using class TollCalculator. 
    The output data is saved as a json file, including a dictionary. The keys are the registration number and the 
    values are the date and toll fee for the day.
"""

def read_data(file_name) -> dict: 

    try: 
        with open(f"{file_name}", "r") as f:
            input_data_dict = json.load(f)
        return input_data_dict
    except FileNotFoundError: 
        print(f"The file name is inaccurate.")
    
        return {}


def main(todays_date):

    folder = "/Users/elino/Documents/AFRY/KODTEST/toll-calculator/Python/Data/"
    input_data_dict = read_data(f"{folder}input_data_{todays_date}.json")
    if input_data_dict: 
        output_data_dict = {}

        for reg_nr in input_data_dict.keys():
            vehicle_type = VehicleTypes.VehicleType(input_data_dict[reg_nr][0])
            dates = [datetime.fromisoformat(d) for d in input_data_dict[reg_nr][1]]

            toll_class = tollCalculator.TollCalculator(vehicle_type, dates)
            results = toll_class.calc_toll_fee()
            date, total_toll_fee = results[0], results[1]

            output_data_dict[reg_nr] = [date, total_toll_fee]

        with open(f"{folder}output_data_{todays_date}.json", "w") as f:
            json.dump(output_data_dict, f)

        print(f"Toll fees were calculated successfully.  \nOutdata is saved to output_data_{todays_date}.json.")
    
    
todays_date = "2025_06_04"
main(todays_date)






