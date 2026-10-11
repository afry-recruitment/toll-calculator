using TollFeeCalculator;

public class TollCalculator
{
    private const int MAX_TOLL_FEE_PER_DAY = 60; // Max fee per day is 60

    /// <summary>
    /// The toll fee schedule. This is the single source of truth: the calculation
    /// and anything that displays the schedule (e.g. the simulator) both read it,
    /// so they can never drift apart.
    /// </summary>
    public static readonly IReadOnlyList<FeeInterval> Schedule = new FeeInterval[]
    {
        new(new TimeOnly(6, 0),  new TimeOnly(6, 29),  8),
        new(new TimeOnly(6, 30), new TimeOnly(6, 59),  13),
        new(new TimeOnly(7, 0),  new TimeOnly(7, 59),  18),
        new(new TimeOnly(8, 0),  new TimeOnly(8, 29),  13),
        new(new TimeOnly(8, 30), new TimeOnly(14, 59), 8),
        new(new TimeOnly(15, 0), new TimeOnly(15, 29), 13),
        new(new TimeOnly(15, 30), new TimeOnly(16, 59), 18),
        new(new TimeOnly(17, 0), new TimeOnly(17, 59), 13),
        new(new TimeOnly(18, 0), new TimeOnly(18, 29), 8),
    };

    /// <summary>
    /// Individual toll-free dates (public holidays), keyed by full date so that a
    /// new year is added as pure data. Recurring toll-free periods (whole months)
    /// and weekends are handled by rule and are not listed here.
    /// </summary>
    private static readonly HashSet<DateOnly> TollFreeDates = new()
    {
        // 2013
        new DateOnly(2013, 1, 1),
        new DateOnly(2013, 3, 28),
        new DateOnly(2013, 3, 29),
        new DateOnly(2013, 4, 1),
        new DateOnly(2013, 4, 30),
        new DateOnly(2013, 5, 1),
        new DateOnly(2013, 5, 8),
        new DateOnly(2013, 5, 9),
        new DateOnly(2013, 6, 5),
        new DateOnly(2013, 6, 6),
        new DateOnly(2013, 6, 21),
        new DateOnly(2013, 11, 1),
        new DateOnly(2013, 12, 24),
        new DateOnly(2013, 12, 25),
        new DateOnly(2013, 12, 26),
        new DateOnly(2013, 12, 31)
    };

    // Recurring toll-free periods, regardless of year.
    private static readonly HashSet<int> TollFreeMonths = new() { 7 }; // whole of July (Stockholm rule)

    // Vehicle types that are never charged.
    private static readonly HashSet<string> TollFreeVehicleTypes = new(StringComparer.Ordinal)
    {
        "Motorbike",
        "Tractor",
        "Emergency",
        "Diplomat",
        "Foreign",
        "Military",
    };
    
    /**
     * Calculate the total toll fee for one day.
     *
     * The supplied dates array is not modified.
     *
     * @param vehicle - the vehicle
     * @param dates   - date and time of all passes on one day
     * @return - the total toll fee for that day
     */

    public int GetTollFee(Vehicle vehicle, DateTime[] dates)
    {
        // Check for null
        if (vehicle == null || dates == null) throw new ArgumentNullException();

        // Check if the dates array is empty
        if (dates.Length == 0) throw new ArgumentException("Dates array is empty");

        // If we have only one date, just return the fee for that one
        if (dates.Length == 1) return GetTollFee(dates[0], vehicle);

        // Check that all dates are on the same day, by comparing the first date to the rest. (Trust but verify)
        for (int i = 1; i < dates.Length; i++)
        {
            if (!dates[i].Date.Equals(dates[0].Date)) throw new ArgumentException("Dates are not on the same day");
        }

        // Work on a copy so the caller's array is left untouched.
        DateTime[] sorted = (DateTime[])dates.Clone();
        Array.Sort(sorted);

        // Calculate the daily total: within each 1-hour window (anchored at the
        // first pass of the window) only the highest fee is charged.
        DateTime windowstart = sorted[0];
        int windowfee = 0;
        int totalfee = 0;

        foreach (DateTime date in sorted)
        {
            int fee = GetTollFee(date, vehicle);

            if ((date - windowstart).TotalMinutes <= 60)
            {
                windowfee = Math.Max(windowfee, fee);
            }
            else
            {
                totalfee += windowfee;
                windowstart = date;
                windowfee = fee;
            }
        }

        totalfee += windowfee;

        if (totalfee > MAX_TOLL_FEE_PER_DAY) return MAX_TOLL_FEE_PER_DAY;

        return totalfee;
    }

    private bool IsTollFreeVehicle(Vehicle vehicle)
    {
        return vehicle != null && TollFreeVehicleTypes.Contains(vehicle.GetVehicleType());
    }

    private int GetTollFee(DateTime date, Vehicle vehicle)
    {
        if (IsTollFreeDate(date) || IsTollFreeVehicle(vehicle)) return 0;

        TimeOnly time = new TimeOnly(date.Hour, date.Minute);

        foreach (FeeInterval interval in Schedule)
        {
            if (time >= interval.Start && time <= interval.End) return interval.Fee;
        }

        return 0;
    }

    private bool IsTollFreeDate(DateTime date)
    {
        DateOnly day = DateOnly.FromDateTime(date);

        if (day.DayOfWeek is DayOfWeek.Saturday or DayOfWeek.Sunday) return true;
        if (TollFreeMonths.Contains(day.Month)) return true;

        return TollFreeDates.Contains(day);
    }
}