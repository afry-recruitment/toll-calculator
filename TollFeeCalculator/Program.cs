using System;

namespace TollFeeCalculator
{
    internal class Program
    {
        static void Main(string[] args)
        {
            IVehicle car = new Car();
            IVehicle motorbike = new Motorbike();

            DateTime[] dates = {
            new DateTime(2024, 8, 5, 6, 0, 0),
            new DateTime(2024, 8, 5, 6, 30, 0), 
            new DateTime(2024, 8, 5, 7, 30, 0)  
        };                                      

            TollCalculator calculator = new TollCalculator();

            int tollFeeCar = calculator.CalculateTollFee(car, dates);
            Console.WriteLine($"The total toll fee for the car is: {tollFeeCar}");

            int tollFeeMotorbike = calculator.CalculateTollFee(motorbike, dates);
            Console.WriteLine($"The total toll fee for the motorbike is: {tollFeeMotorbike}");
        }
    }
}
