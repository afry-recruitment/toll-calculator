using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TollFeeCalculator
{
    class Program
    {
        static void Main()
        {
            var calculator = new TollFeeCalculator();

            var dates = new DateTime[] {new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day,DateTime.Now.Hour, DateTime.Now.Minute, DateTime.Now.Second) };
            var vehicleType = "Car";
            Console.WriteLine("Enter the vehicle type:");
            vehicleType = Console.ReadLine();


            int fee = calculator.GetTollFee(dates, vehicleType);
         
            Console.WriteLine($"Total toll fee: {fee} SEK");
        }
    }
}