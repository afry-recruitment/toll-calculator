using System;
using System.Collections.Generic;
using System.Text;

namespace TollCalculator
{
    class Program
    {
        static void Main(string[] args)
        {
            var tollCalculator = new TollFeeCalculator();
            var dates = new DateTime[] { new DateTime(2022, 06, 07, 06, 29, 29), new DateTime(2022, 06, 07, 08, 29, 29), };
            
            //CAR
            var carToll = tollCalculator.GetTollFee(new Car(), dates);
            Console.WriteLine($"Car - {carToll}");

            //Motorbike
            var motorbikeToll = tollCalculator.GetTollFee(new Motorbike(), dates);
            Console.WriteLine($"Motorbike - {motorbikeToll}");

            //Emergency
            var emergencyToll = tollCalculator.GetTollFee(new Emergency(), dates);
            Console.WriteLine($"Emergency - {emergencyToll}");

            //Foreign
            var foreignToll = tollCalculator.GetTollFee(new Foreign(), dates);
            Console.WriteLine($"Foreign - {foreignToll}");

            //Military
            var militaryToll = tollCalculator.GetTollFee(new Military(), dates);
            Console.WriteLine($"Military - {militaryToll}");

            //Tractor
            var tractorToll = tollCalculator.GetTollFee(new Tractor(), dates);
            Console.WriteLine($"Tractor - {tractorToll}");

            //Diplomat
            var diplomatToll = tollCalculator.GetTollFee(new Diplomat(), dates);
            Console.WriteLine($"Diplomat - {diplomatToll}");

        }
    }
}