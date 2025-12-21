using System;
using TollFeeCalculator;

namespace TollFeeCalculator
{
    class Program
    {
        static void Main(string[] args)
        {
            // Example usage
            TollCalculator calculator = new TollCalculator();
            
            Vehicle car = new Car();
            Vehicle motorbike = new Motorbike();
            
            // Test with some dates
            DateTime[] dates = new DateTime[]
            {
                new DateTime(2013, 1, 2, 6, 15, 0),  // Regular day, 6:15 AM
                new DateTime(2013, 1, 2, 7, 30, 0),  // Same day, 7:30 AM
                new DateTime(2013, 1, 2, 15, 45, 0)  // Same day, 3:45 PM
            };
            
            int carFee = calculator.GetTollFee(car, dates);
            int motorbikeFee = calculator.GetTollFee(motorbike, dates);
            
            Console.WriteLine($"Car toll fee: {carFee} SEK");
            Console.WriteLine($"Motorbike toll fee: {motorbikeFee} SEK");
            
            Console.WriteLine("\nPress any key to exit...");
            Console.ReadKey();
        }
    }
}