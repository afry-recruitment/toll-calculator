using TrafficToll.Internals.DataAccess;
using TrafficToll.Internals.Enums;
using TrafficToll.Internals.Models;
using TrafficToll.Internals.ValueObjects;

namespace TrafficToll.Internals;
internal static class TollCalculatorCore
{

    /**
     * Calculate the total toll fee for one day
     *
     * @param vehicle - the vehicle
     * @param dates   - date and time of all passes on one day
     * @return - the total toll fee for that day
     */

    private static TrafficTollSpecification TrafficTollSpecification { get; }

    static TollCalculatorCore()
    {
        TrafficTollSpecification = TrafficTollSpecificationRepository.GetTrafficTollSpecification();
    }

    public static int GetTollFee(TollCalculationInput tollCalculationInput, TollCalculationArguments tollCalculationArguments)
    {
        if (IsTollFreeVehicle(tollCalculationInput.VehicleType) || IsTollFreeDate(tollCalculationInput.Date))
            return 0;

        var groupedTimeSpans = GroupByMaxValidTollTime(tollCalculationInput.PassingTimes, TrafficTollSpecification.ValidTollTime);

        throw new NotImplementedException();
    }

    private static bool IsTollFreeVehicle(VehicleType vehicle)
    {
        return TrafficTollSpecification.TollFreeVehicleTypes.Select(x => (VehicleType)x).Contains(vehicle);
    }

    private static bool IsTollFreeDate(DateTime date)
    {
        return TrafficTollSpecification.TollFreeDates.Contains(date);
    }

    private static int CalculateHighestFee(IEnumerable<TimeSpan> groupedTimeSpans)
    {
        var tollTimePrizes = TrafficTollSpecification.DailyTollTimePrizes;
        var feeTypes = groupedTimeSpans.Select(x => tollTimePrizes.First(tollTime => tollTime.Start <= x && x < tollTime.End).TollTrafficType).OfType<TollTrafficType>();
        throw new NotImplementedException();
    }

    private static IEnumerable<IEnumerable<TimeSpan>> GroupByMaxValidTollTime(IEnumerable<TimeSpan> timeSpans, TimeSpan validTollTime)
    {
        var groupList = new List<IEnumerable<TimeSpan>>();

        while (true)
        {
            var startTime = timeSpans.First();
            var group = timeSpans.Where(x => x >= startTime && x < startTime + validTollTime);
            timeSpans = timeSpans.Where(x => !group.Contains(x));
            groupList.Add(timeSpans);

            if (!timeSpans.Any())
                break;
        }

        return groupList;
    }
}