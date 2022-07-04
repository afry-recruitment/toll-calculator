using System;
using TollCalculator.vehicles;

namespace TollCalculator
{
    internal class Program
    {
        static void Main(string[] args)
        {

            DateTime[] dates = {
                DateTime.Now.AddDays(2).AddHours(-11),
                DateTime.Now.AddDays(2).AddHours(-1).AddMinutes(10),
                DateTime.Now.AddDays(2).AddHours(-2),
                DateTime.Now.AddDays(2).AddHours(-3),
                DateTime.Now.AddDays(2).AddHours(-4),
                DateTime.Now.AddDays(2).AddHours(-5),
                DateTime.Now.AddDays(2).AddHours(-11)};

            var tractor = new Tractor();
            Console.WriteLine("Tractor Toll Fee : {0}", tractor.TollCalculator.GetTollFee(tractor, dates));

            var car = new Car();
            Console.WriteLine("car Toll Fee : {0}", car.TollCalculator.GetTollFee(car, dates));

            var motorBike = new MotorBike();
            Console.WriteLine("motorBike Toll Fee : {0}", motorBike.TollCalculator.GetTollFee(motorBike, dates));

            var emergency = new Emergency();
            Console.WriteLine("emergency Toll Fee : {0}", emergency.TollCalculator.GetTollFee(emergency, dates));

            var diplomat = new Diplomat();
            Console.WriteLine("diplomat Toll Fee : {0}", diplomat.TollCalculator.GetTollFee(diplomat, dates));

            var military = new Military();
            Console.WriteLine("military Toll Fee : {0}", military.TollCalculator.GetTollFee(military, dates));

            var foriegn = new Foreign();
            Console.WriteLine("foriegn Toll Fee : {0}", foriegn.TollCalculator.GetTollFee(foriegn, dates));

        }
    }
}
