using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Toll_Fee_Calculator
{
    class Program
    {
        public static TollCalculator TollCalculator { get; private set; }

        static void Main(string[] args)
        {
            Holiday[] holidays = { new Holiday(1, 1), new Holiday(6, 1), new Holiday(7, 4), new Holiday(9, 4), new Holiday(10, 4), new Holiday(1, 5), new Holiday(18, 5),
            new Holiday(28, 5), new Holiday(6, 6), new Holiday(24, 6), new Holiday(4, 11), new Holiday(24, 12), new Holiday(25, 12), new Holiday(26, 31)};
            Vehicle v1 = new Car(new DateTime[] { new DateTime(2023, 4, 27, 15, 45, 00), new DateTime(2023, 4, 28, 16, 30, 00), new DateTime(2023, 4, 28, 22, 30, 00) });
            Vehicle v2 = new Car(new DateTime[] { new DateTime(2023, 4, 27, 6, 45, 00), new DateTime(2023, 4, 27, 7, 15, 00), new DateTime(2023, 4, 27, 8, 25, 00),
                                                  new DateTime(2023, 4, 27, 15, 5, 00), new DateTime(2023, 4, 27, 15, 45, 00), new DateTime(2023, 4, 27, 16, 55, 00),
                                                  new DateTime(2023, 4, 27, 17, 50, 00)});
            Vehicle v3 = new Motorbike(new DateTime[] { new DateTime(2023, 4, 28, 6, 45, 00), new DateTime(2023, 10, 28, 7, 15, 00)});
            Vehicle v4 = new Military(new DateTime[] { new DateTime(2023, 4, 27, 15, 45, 00)});
            Vehicle v5 = new Emergency(new DateTime[] { new DateTime(2023, 4, 27, 7, 15, 00), new DateTime(2023, 4, 27, 8, 25, 00), new DateTime(2023, 4, 27, 15, 5, 00),
                                                        new DateTime(2023, 4, 28, 15, 45, 00) });

            Vehicle[] vehicles = { v1, v2, v3, v4, v5};
            TollCalculator = new TollCalculator(holidays, vehicles);
            TollCalculator.TollCalculatorSimulator();
            Console.ReadKey();
        }
    }
}
