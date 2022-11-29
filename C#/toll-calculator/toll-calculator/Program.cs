using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TollFeeCalculator
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Vehicle car = new Car();
            Vehicle bike = new Motorbike();

            TollCalculator tollCalculator = new TollCalculator();

            DateTime dateTime1 = new DateTime(2022, 1, 17, 10, 20, 55);
            DateTime dateTime2 = new DateTime(2022, 1, 17, 14, 40, 05);
            DateTime dateTime3 = new DateTime(2022, 1, 17, 15, 20, 20);
            DateTime dateTime4 = new DateTime(2022, 1, 17, 17, 20, 30);
            DateTime dateTime5 = new DateTime(2022, 1, 19, 10, 20, 30);
            DateTime dateTime6 = new DateTime(2022, 1, 20, 05, 20, 30);
            DateTime dateTime7 = new DateTime(2022, 1, 17, 20, 20, 55);

            DateTime[] dates = { dateTime1, dateTime2, dateTime3, dateTime4, dateTime5, dateTime6, dateTime7 };

            // Car
            Console.WriteLine(tollCalculator.GetTollFee(car, dates));
            // Bike
            Console.WriteLine(tollCalculator.GetTollFee(bike, dates));
        }
    }
}
