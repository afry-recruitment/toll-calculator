using System;
using System.Collections.Generic;
using TollFeeCalculator.TollFees;
using TollFeeCalculator.Vehicles;

namespace TollFeeCalculator
{
    internal class Program
    {
        public static void Main()
        {
            // Just for show

            var tollFeeRepository = TollFeeRepositoryFactory.GetNewTollFeeRepository();
            var tollCalculator = new TollCalculator(tollFeeRepository);

            var tollFeeRepositoryInitialized = TollFeeRepositoryInit.GetNewTollFeeRepository();
            var tollCalculatorInitialized = new TollCalculator(tollFeeRepositoryInitialized);

            var carOne = new Vehicle("CAR 001", VehicleType.Car);
            var carOnePassages = new List<DateTime>() { new(2022, 11, 29, 23, 59, 59) };
            var carOneTollFees = tollCalculatorInitialized.GetTollFee(carOne, carOnePassages);

        }
    }
}
