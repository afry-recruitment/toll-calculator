using toll_calculator.enums;
using toll_calculator.models;
using toll_calculator.repository;
using toll_calculator.value_objects;

namespace toll_calculator;
internal static class TollCalculator
{

    /**
     * Calculate the total toll fee for one day
     *
     * @param vehicle - the vehicle
     * @param dates   - date and time of all passes on one day
     * @return - the total toll fee for that day
     */

    private static TrafficTollSpecification TrafficTollSpecification { get; }

    static TollCalculator()
    {
        TrafficTollSpecification = TrafficTollSpecificationRepository.GetTrafficTollSpecification();
    }

    public static int GetTollFee(TollCalculationInput tollCalculationInput)
    {
        if (IsTollFreeVehicle(tollCalculationInput.VehicleType) || IsTollFreeDate(tollCalculationInput.Date))
            return 0;

        var groupedTollTimes = GroupByMaxValidTollTime(tollCalculationInput.PassingTimes, TrafficTollSpecification.DailyTrafficTollSpecification.ValidTollTime);
        var tollFees = groupedTollTimes.Select(x => CalculateHighestGroupFee(x)).Sum();
        return tollFees < TrafficTollSpecification.DailyTrafficTollSpecification.MaximumDailyFee ? tollFees : TrafficTollSpecification.DailyTrafficTollSpecification.MaximumDailyFee;
    }

    private static bool IsTollFreeVehicle(VehicleType vehicle)
    {
        return TrafficTollSpecification.TollFreeVehicleTypes.Select(x => (VehicleType)x).Contains(vehicle);
    }

    private static bool IsTollFreeDate(DateTime date)
    {
        return TrafficTollSpecification.TollFreeDates.Contains(date);
    }

    private static int CalculateHighestGroupFee(IEnumerable<TimeSpan> groupedTimeSpans)
    {
        var tollTimePrizes = TrafficTollSpecification.DailyTrafficTollSpecification.DailyTollTimePrizes;
        var feeTypes = groupedTimeSpans.Select(x => tollTimePrizes.First(tollTime => tollTime.Start <= x && x < tollTime.End).FeeType).OfType<TollTrafficType>();
        var highestFee = feeTypes.Select(feeType => TrafficTollSpecification.DailyTrafficTollSpecification.PriceMapping[(int)feeType]).Max();
        return highestFee;
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