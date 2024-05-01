namespace Tollculator.Calculator;

public static class TollCalculatorHelper
{
    private static readonly Dictionary<(TimeOnly startTime, TimeOnly endTime), int> TollFees = new()
    {
        { (new TimeOnly(6, 0), new TimeOnly(6, 29)), 8 },
        { (new TimeOnly(6, 30), new TimeOnly(6, 59)), 13 },
        { (new TimeOnly(7, 0), new TimeOnly(7, 59)), 18 },
        { (new TimeOnly(8, 0), new TimeOnly(8, 29)), 13 },
        { (new TimeOnly(8, 30), new TimeOnly(14, 59)), 8 },
        { (new TimeOnly(15, 0), new TimeOnly(15, 29)), 13 },
        { (new TimeOnly(15, 30), new TimeOnly(16, 59)), 18 },
        { (new TimeOnly(17, 0), new TimeOnly(17, 59)), 13 },
        { (new TimeOnly(18, 0), new TimeOnly(18, 29)), 8 }
    };

    public static int GetHighestTollFromTimes(List<TimeOnly> times)
    {
        var tollFee = 0;

        foreach (var time in times)
        {
            var current = GetTollFee(time);
            if (current > tollFee)
            {
                tollFee = current;
            }
        }

        return tollFee;
    }

    private static int GetTollFee(TimeOnly time)
    {
        foreach (var (timeRange, fee) in TollFees)
        {
            if (IsTimeBetween(time, timeRange.startTime, timeRange.endTime))
            {
                return fee;
            }
        }

        return 0;
    }

    private static bool IsTimeBetween(TimeOnly time, TimeOnly startTime, TimeOnly endTime) => time >= startTime && time <= endTime;
}
