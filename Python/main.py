from datetime import datetime
from Car import Car
from Motorbike import Motorbike
from TollCalculator import TollCalculator

def main():
    # Car usecae
    car = Car()
    car_dates = [datetime(2023, 12, 18, 8, 0), datetime(2023, 12, 18, 9, 0), datetime(2023, 12, 18, 15, 30)]
    
    # Motorbike usecase
    motorbike = Motorbike()
    motorbike_dates = [datetime(2023, 12, 18, 8, 30), datetime(2023, 12, 18, 10, 0), datetime(2023, 12, 18, 16, 30)]

    calculator = TollCalculator()

    car_fee = calculator.get_toll_fee(car, car_dates)
    motorbike_fee = calculator.get_toll_fee(motorbike, motorbike_dates)

    print(f"Total toll fee for the car: {car_fee} SEK")
    print(f"Total toll fee for the motorbike: {motorbike_fee} SEK")

if __name__ == "__main__":
    main()
