namespace TollFeeCalculator;

public sealed class TollCalculator
{
    public const int MaxDailyFee = 60;
    public static readonly TimeSpan ChargeWindow = TimeSpan.FromHours(1);

    /// <summary>
    /// Calculates the total toll fee for the given passages (local Swedish time).
    /// Passages may be unordered and span several days; the daily cap is applied per calendar day.
    /// Within a day, a charge window starts at the first chargeable passage and lasts one hour;
    /// only the highest fee in each window is charged.
    /// </summary>
    public int GetTollFee(Vehicle vehicle, IEnumerable<DateTime> passages)
    {
        ArgumentNullException.ThrowIfNull(vehicle);
        ArgumentNullException.ThrowIfNull(passages);

        if (vehicle.IsTollFree) return 0;

        return passages
            .GroupBy(DateOnly.FromDateTime)
            .Sum(day => GetDailyFee(day));
    }

    /// <summary>Fee for a single passage, without hourly or daily rules applied.</summary>
    public int GetTollFee(Vehicle vehicle, DateTime passage)
    {
        ArgumentNullException.ThrowIfNull(vehicle);
        return vehicle.IsTollFree ? 0 : GetPassageFee(passage);
    }

    public static bool IsTollFreeDate(DateOnly date) =>
        date.DayOfWeek is DayOfWeek.Saturday or DayOfWeek.Sunday ||
        date.Month == 7 ||
        SwedishHolidays.IsPublicHoliday(date) ||
        SwedishHolidays.IsPublicHoliday(date.AddDays(1));

    private static int GetPassageFee(DateTime passage) =>
        IsTollFreeDate(DateOnly.FromDateTime(passage)) ? 0 : TollFeeSchedule.GetFee(TimeOnly.FromDateTime(passage));

    private static int GetDailyFee(IEnumerable<DateTime> passages)
    {
        var chargeable = passages
            .Select(time => (Time: time, Fee: GetPassageFee(time)))
            .Where(p => p.Fee > 0)
            .OrderBy(p => p.Time);

        int total = 0, windowFee = 0;
        DateTime windowStart = default;
        foreach (var (time, fee) in chargeable)
        {
            if (windowFee > 0 && time - windowStart < ChargeWindow)
            {
                windowFee = Math.Max(windowFee, fee);
                continue;
            }
            total += windowFee;
            windowStart = time;
            windowFee = fee;
        }
        return Math.Min(total + windowFee, MaxDailyFee);
    }
}
