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
            
            // Test with two passes with 60 min.
            var dates = new DateTime[]
            {
                new DateTime(2013, 1, 2, 6, 0, 0),   // 6:00 AM - 8 SEK
                new DateTime(2013, 1, 2, 7, 0, 0)    // 7:00 AM (exactly 60 min later) - 18 SEK
            };
            
            int carFee = calculator.GetTollFee(car, dates);
            int motorbikeFee = calculator.GetTollFee(motorbike, dates);
            
            Console.WriteLine($"Car toll fee: {carFee} SEK shuld be 18 SEK");
            Console.WriteLine($"Motorbike toll fee: {motorbikeFee} SEK should be 0 SEK");
            
            // Test New Year's Day 2026 - should be free (holiday)
            DateTime[] newYearDates = new DateTime[]
            {
                new DateTime(2026, 1, 1, 15, 30, 0)  // New Year's Day 2026 at 3:30 PM
            };
            
            int newYearFee = calculator.GetTollFee(car, newYearDates);
            Console.WriteLine($"\nNew Year's Day 2026 toll fee: {newYearFee} SEK (should be 0)");
            
            Console.WriteLine("\nPress any key to exit...");
            Console.ReadKey();
        }
    }
}