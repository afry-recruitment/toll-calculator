using TrafficToll.Internals.Models;
using TrafficToll.Internals.ValueObjects;

namespace TrafficToll.Internals
{
    internal static class TrafficTollFactory
    {
        public static TollCalculationArguments CreateTollCalculationArguments(TrafficTollSpecification trafficTollSpecification)
        {
            return new TollCalculationArguments(
                maximumDailyFee: trafficTollSpecification.MaximumDailyFee,
                validTollTime: trafficTollSpecification.ValidTollTime,
                dailyTollTimePrizes: trafficTollSpecification.DailyTollTimePrizes,
                priceMapping: trafficTollSpecification.PriceMapping);
        }

        public static TollCalculationInput CreateTollCalculationInput(IEnumerable<DateTime> passings, int vehicleType)
        {
            return new TollCalculationInput(passings, vehicleType);
        }
    }
}
