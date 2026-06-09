namespace TollFeeCalculator;

public class TollCalculator
{
    private const int MaxDailyFee = 60;
    private const int IntervalMinutes = 60;

    /// <summary>
    /// Toll fee per time-of-day, expressed as inclusive minute-of-day ranges.
    /// Any time-of-day not covered here is free.
    /// </summary>
    private static readonly (int StartMinute, int EndMinute, int Fee)[] FeeSchedule =
    {
        ( 6 * 60 +  0,  6 * 60 + 29,  8),
        ( 6 * 60 + 30,  6 * 60 + 59, 13),
        ( 7 * 60 +  0,  7 * 60 + 59, 18),
        ( 8 * 60 +  0,  8 * 60 + 29, 13),
        ( 8 * 60 + 30, 14 * 60 + 59,  8),
        (15 * 60 +  0, 15 * 60 + 29, 13),
        (15 * 60 + 30, 16 * 60 + 59, 18),
        (17 * 60 +  0, 17 * 60 + 59, 13),
        (18 * 60 +  0, 18 * 60 + 29,  8),
    };

    private static readonly HashSet<string> TollFreeVehicleTypes = new(StringComparer.OrdinalIgnoreCase)
    {
        "Motorbike",
        "Tractor",
        "Emergency",
        "Diplomat",
        "Foreign",
        "Military",
    };

    /// <summary>
    /// Calculate the total toll fee for one day. Passes within a rolling
    /// 60-minute window are charged once at the highest applicable fee,
    /// and the daily total is capped at <see cref="MaxDailyFee"/> SEK.
    /// </summary>
    /// <param name="vehicle">The vehicle being charged.</param>
    /// <param name="dates">Timestamps of all passes during the day. Order is not assumed.</param>
    /// <returns>The total toll fee for that day in SEK.</returns>
    public int GetTollFee(Vehicle? vehicle, DateTime[]? dates)
    {
        if (vehicle is null || dates is null || dates.Length == 0)
            return 0;

        var ordered = dates.OrderBy(d => d).ToArray();
        DateTime intervalStart = ordered[0];
        int maxFeeInInterval = GetTollFee(intervalStart, vehicle);
        int totalFee = 0;

        for (int i = 1; i < ordered.Length; i++)
        {
            DateTime date = ordered[i];
            int fee = GetTollFee(date, vehicle);

            if ((date - intervalStart).TotalMinutes <= IntervalMinutes)
            {
                if (fee > maxFeeInInterval) maxFeeInInterval = fee;
            }
            else
            {
                totalFee += maxFeeInInterval;
                intervalStart = date;
                maxFeeInInterval = fee;
            }
        }
        totalFee += maxFeeInInterval;

        return Math.Min(totalFee, MaxDailyFee);
    }

    /// <summary>
    /// Toll fee for a single pass at the given time, ignoring hourly rules.
    /// Returns 0 for fee-free vehicles, weekends and holidays.
    /// </summary>
    public int GetTollFee(DateTime date, Vehicle? vehicle)
    {
        if (vehicle is null || IsTollFreeDate(date) || IsTollFreeVehicle(vehicle))
            return 0;

        int minuteOfDay = date.Hour * 60 + date.Minute;
        foreach (var (startMinute, endMinute, fee) in FeeSchedule)
        {
            if (minuteOfDay >= startMinute && minuteOfDay <= endMinute)
                return fee;
        }
        return 0;
    }

    private static bool IsTollFreeVehicle(Vehicle? vehicle)
    {
        if (vehicle is null) return false;
        return TollFreeVehicleTypes.Contains(vehicle.GetVehicleType());
    }

    private static bool IsTollFreeDate(DateTime date)
    {
        if (date.DayOfWeek == DayOfWeek.Saturday || date.DayOfWeek == DayOfWeek.Sunday)
            return true;

        // Swedish public holidays for 2013. This calendar is the kata fixture
        // and should be replaced by a configurable holiday provider before
        // deploying to additional years.
        if (date.Year == 2013)
        {
            int month = date.Month;
            int day = date.Day;
            if ((month == 1 && day == 1) ||
                (month == 3 && (day == 28 || day == 29)) ||
                (month == 4 && (day == 1 || day == 30)) ||
                (month == 5 && (day == 1 || day == 8 || day == 9)) ||
                (month == 6 && (day == 5 || day == 6 || day == 21)) ||
                (month == 7) ||
                (month == 11 && day == 1) ||
                (month == 12 && (day == 24 || day == 25 || day == 26 || day == 31)))
            {
                return true;
            }
        }
        return false;
    }
}
