namespace TollFeeCalculator;

/// <summary>Calculates toll fees in whole SEK for one vehicle and one local calendar day.</summary>
public sealed class TollCalculator
{
    private const int DailyLimit = 60;
    private static readonly TimeSpan IntervalLength = TimeSpan.FromMinutes(60);
    private readonly TollCalendar _calendar;

    public TollCalculator() : this(TollCalendar.Original2013)
    {
    }

    public TollCalculator(TollCalendar calendar)
    {
        ArgumentNullException.ThrowIfNull(calendar);
        _calendar = calendar;
    }

    /// <summary>
    /// Charges the highest fee in each fixed 60-minute window, including its endpoint,
    /// capped at 60 SEK per day. The caller's array is not modified.
    /// </summary>
    /// <param name="vehicle">The vehicle whose passages are being calculated.</param>
    /// <param name="dates">City-local dates and times, all on one date. No time-zone conversion is performed.</param>
    /// <returns>The daily fee in whole SEK, or zero for an empty array.</returns>
    /// <exception cref="ArgumentNullException">Vehicle or passage array is null.</exception>
    /// <exception cref="ArgumentException">Passages span multiple dates.</exception>
    /// <exception cref="ArgumentOutOfRangeException">The calendar does not support the passage year.</exception>
    public int GetTollFee(Vehicle vehicle, DateTime[] dates)
    {
        ArgumentNullException.ThrowIfNull(vehicle);
        ArgumentNullException.ThrowIfNull(dates);
        if (dates.Length == 0) return 0;

        var sortedDates = (DateTime[])dates.Clone();
        Array.Sort(sortedDates);

        if (sortedDates[0].Date != sortedDates[^1].Date)
        {
            throw new ArgumentException("All passages must be on the same day.", nameof(dates));
        }

        bool tollFreeDate = _calendar.IsTollFree(DateOnly.FromDateTime(sortedDates[0]));
        if (tollFreeDate || IsTollFreeVehicle(vehicle)) return 0;

        DateTime intervalStart = sortedDates[0];
        int intervalMaxFee = 0;
        int totalFee = 0;

        foreach (DateTime date in sortedDates)
        {
            int nextFee = TollTariff.GetFee(TimeOnly.FromDateTime(date));
            if (date - intervalStart > IntervalLength)
            {
                totalFee += intervalMaxFee;
                intervalStart = date;
                intervalMaxFee = nextFee;
            }
            else
            {
                intervalMaxFee = Math.Max(intervalMaxFee, nextFee);
            }
        }

        // The last interval has not yet been added to the total.
        return Math.Min(DailyLimit, totalFee + intervalMaxFee);
    }

    /// <summary>Returns one passage's fee using the same calendar and input rules as the daily calculation.</summary>
    public int GetTollFee(DateTime date, Vehicle vehicle)
    {
        ArgumentNullException.ThrowIfNull(vehicle);
        bool tollFreeDate = _calendar.IsTollFree(DateOnly.FromDateTime(date));
        return tollFreeDate || IsTollFreeVehicle(vehicle)
            ? 0
            : TollTariff.GetFee(TimeOnly.FromDateTime(date));
    }

    private static bool IsTollFreeVehicle(Vehicle vehicle)
    {
        return vehicle.Type is VehicleType.Motorbike or VehicleType.Tractor
            or VehicleType.Emergency or VehicleType.Diplomat
            or VehicleType.Foreign or VehicleType.Military;
    }

}
