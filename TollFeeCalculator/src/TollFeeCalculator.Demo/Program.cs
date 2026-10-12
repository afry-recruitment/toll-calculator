using TollFeeCalculator.Core.Vehicles;

namespace TollFeeCalculator.Demo
{
    // Console Application for demonstration: only builds vehicles and dates,
    // calls the calculator and displays results. All I/O lives here, not in the library.
    internal class Program
    {
        static void Main(string[] args)
        {
            var calculator = new TollCalculator();
            IVehicle car = new Car();
            IVehicle motorbike = new Motorbike();

            // These examples verify the design and the bugs found:
            // - "Two passes > 1h apart": 06:30 y 07:45 (75 min) -> 13+18 = 31 (with bug was 18)
            // - "Morning and afternoon": 06:30, 08:30 y 15:30 -> 13+8+18 = 39 (with bug was 18)
            var examples = new (string name, IVehicle vehicle, DateTime[] dates)[]
            {
                ("Motorbike exempt",        motorbike, new[] { CreateInterval(7, 0) }),
                ("Two passes > 1h apart",   car,       new[] { CreateInterval(6, 30), CreateInterval(7, 45) }),
                ("Three passes same hour",  car,       new[] { CreateInterval(7, 0), CreateInterval(7, 20), CreateInterval(7, 40) }),
                ("Morning and afternoon",   car,       new[] { CreateInterval(6, 30), CreateInterval(8, 30), CreateInterval(15, 30) }),
                ("New Year's Day(free day)",car,       new[] { CreateInterval(7, 0).AddDays(-1) }),
                ("Daily cap 60",            car,       new[] { CreateInterval(6, 30), CreateInterval(7, 45), CreateInterval(9, 30), CreateInterval(11, 30), CreateInterval(13, 30), CreateInterval(15, 30), CreateInterval(17, 15) }),
                ("Disordered daily cap 60", car,       new[] { CreateInterval(11, 30), CreateInterval(13, 30), CreateInterval(6, 30), CreateInterval(7, 45), CreateInterval(9, 30), CreateInterval(15, 30), CreateInterval(17, 15) })
            };

            foreach (var (name, vehicle, dates) in examples)
                Console.WriteLine($"{name}: { calculator.GetTollFee(vehicle, dates) } kr");
        }

        static DateTime CreateInterval(int h, int m) => new(2013, 1, 2, h, m, 0);
    }
}
