using System;

namespace TollFeeCalculator
{
    internal class Program
    {
        static void Main(string[] args)
        {
            var vehicle = new Motorbike();
            var dates = new DateTime[]
            {
                new DateTime(2022, 11, 25, 08, 29, 11, 69),
                new DateTime(2022, 11, 25, 15, 48, 32, 54),
            };

            var fee = new TollCalculator().GetTollFee(vehicle, dates);
            Console.WriteLine(fee);
        }
    }
}
