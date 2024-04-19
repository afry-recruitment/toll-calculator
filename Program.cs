using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;

namespace TollFeeCalculator
{
    class Program
    {
        static async Task Main()
        {
            try
            {
                TollFeeCalculator tollFeeCalculator = new TollFeeCalculator();
                Vehicle vehicle = new Vehicle() { RegistrationNumber = "OFU835", Class = Vehicle.VehicleClass.Civil, Type = Vehicle.VehicleType.Car};
                tollFeeCalculator.NewTollFee(ref vehicle, DateTime.Parse("2024-04-18 18:22:00")); // 8
                tollFeeCalculator.NewTollFee(ref vehicle, DateTime.Parse("2024-04-19 11:15:00")); // 8
                tollFeeCalculator.NewTollFee(ref vehicle, DateTime.Parse("2024-04-19 17:11:00")); // 13
                tollFeeCalculator.NewTollFee(ref vehicle, DateTime.Parse("2024-04-20 18:55:00")); // 0

                Console.WriteLine($"Toll fee for 2024-04-19: {tollFeeCalculator.GetTotalTollFee(vehicle, DateTime.Parse("2024-04-19 01:01:00"))} SEK (21 SEK)");
                Console.WriteLine($"Total toll fed: {tollFeeCalculator.GetTotalTollFee(vehicle)} SEK (29 SEK)");

            }
            catch (Exception e)
            {
                Console.WriteLine("Exception: " + e.Message);
            }

            finally
            {
                Console.WriteLine("");
                Console.WriteLine("Press ENTER to exit.");
                Console.ReadLine();
            }
        }
    }
}