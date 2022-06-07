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
            var toll = tollCalculator.GetTollFee(new Car(), dates);
            Console.WriteLine($"CAR - {toll}");

            //Motorbike

            //Emergency

            //Foreign

            //Military

            //Tractor

            //Diplomat
        }
    }
}