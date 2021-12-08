using System;
using System.Globalization;

namespace Toll_calculator
{
    class Program
    {
        static void Main(string[] args)
        {
            Scanner();
        }

        private static void Scanner()
        {
            int totalFee;
            DTO dto = new DTO();
            TollCalculator tc = new TollCalculator();
            TollFeeCalculator.Vehicle vehicle = new TollFeeCalculator.Motorbike(); ;


            Console.WriteLine("If your vehicle type is  motorbike, tractor, emergency, diplomat, foreign, military, please press number 0 and enter.\n" +
                "For other vehicle's types, please press number 1 and enter.");

            dto.VechileTypeInput = Int32.Parse(Console.ReadLine());

            if (dto.VechileTypeInput > 1 || 0 > dto.VechileTypeInput)
            {
                Console.WriteLine("You entered the wrong number. Please run the program again. ");
                return;
            }

            Console.WriteLine("Enther the date as year,month,day,hour,min (fx: 2021,12,2,6,10)");

            try
            {
                dto.DateTimeInput = DateTime.ParseExact(Console.ReadLine(), "yyyy,M,d,H,m", null);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }

            if (dto.VechileTypeInput == 1) vehicle = new TollFeeCalculator.Car();

            totalFee = tc.GetTollFee(vehicle, new[] { dto.DateTimeInput });
            Console.WriteLine(string.Format("Here is you fee to pay: {0} sek.", totalFee));
        }
    }
}
