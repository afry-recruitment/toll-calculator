namespace TollFeeCalculator;

public class TollCalculator
{
    /// <summary>
    /// The highest total fee that a vehicle can accumulate on a single day. 
    /// </summary>
    const int MaxDailyTotal = 60;

    /// <summary>
    /// Calculate the total toll fee for one day
    /// </summary>
    /// <param name="vehicle">the vehicle</param>
    /// <param name="dates">date and time of all passes on one day</param>
    /// <returns>the total toll fee for that day</returns>
    public static int GetTollFee(Vehicle vehicle, DateTime[] dates)
    {
        if (IsTollFreeVehicle(vehicle)) return 0;
        if (dates.Length == 0) return 0;
        if (dates.Length == 1) return GetTollFee(dates[0], vehicle);

        // There are multiple passes on one day so we have to check if any of them
        // occur within an hour of each other, and then sum everything.

        // Create a list of passes with the fee for each pass, and keep it in chronological order.
        List<(DateTime Time, int Fee)> passes = dates.Select(d => (Time: d, Fee: GetTollFee(d, vehicle)))
                                                     .OrderBy(x => x.Time)
                                                     .ToList();

        RemoveSubHourIntervals();

        int totalFee = passes.Sum(x => x.Fee);
        return Math.Min(totalFee, MaxDailyTotal);


        void RemoveSubHourIntervals()
        {
            // This goes through the list of passes and removes passes that occur less than 1 hour
            // after the previous pass, keeping the highest fee of the two.

            int i = 0;
            while (i < passes.Count - 1)
            {
                var timeA = passes[i];
                var timeB = passes[i + 1];

                TimeSpan timeBetween = timeB.Time - timeA.Time;
                if (timeBetween < TimeSpan.FromHours(1))
                {
                    // The two passes are less than 1 hour apart. Update the first one
                    // with the higher fee and remove the second one.
                    passes[i] = timeA with { Fee = Math.Max(timeA.Fee, timeB.Fee) };
                    passes.RemoveAt(i + 1);
                }
                else
                {
                    i += 1;
                }
            }
        }
    }

    public static bool IsTollFreeVehicle(Vehicle? vehicle)
    {
        return Enum.TryParse<TollFreeVehicles>(vehicle?.GetVehicleType(), out _);
    }

    public static int GetTollFee(DateTime date, Vehicle vehicle)
    {
        if (IsTollFreeDate(date) || IsTollFreeVehicle(vehicle)) return 0;

        return TollFees.FirstOrDefault(f => f.IsMatch(date)) is { } fee
                   ? fee.Fee
                   : 0;
    }

    private static readonly TollFeeSpecification[] TollFees =
    [
        new TollFeeSpecification(0600..0630, 8),
        new TollFeeSpecification(0630..0700, 13),
        new TollFeeSpecification(0700..0800, 18),
        new TollFeeSpecification(0800..0830, 13),
        new TollFeeSpecification(0830..1500, 8),
        new TollFeeSpecification(1500..1530, 13),
        new TollFeeSpecification(1530..1700, 18),
        new TollFeeSpecification(1700..1800, 13),
        new TollFeeSpecification(1800..1830, 8),
    ];

    /// <summary>
    /// Specifies the toll fee for a specific time interval.
    /// </summary>
    /// <param name="interval">A time interval in the form of an integer range.
    /// The hundreds and thousands digits form the hour, and the ones and tens form the minute.</param>
    /// <param name="fee">The toll fee for the time interval.</param>
    private readonly struct TollFeeSpecification(Range interval, int fee)
    {
        public int Fee => fee;

        /// <summary>
        /// Checks if this toll fee matches a specific time.
        /// </summary>
        public bool IsMatch(DateTime time)
        {
            int integerTime = time.Hour * 100 + time.Minute;
            return integerTime >= interval.Start.Value && integerTime < interval.End.Value;
        }
    };

    public static bool IsTollFreeDate(DateTime date)
    {
        int year = date.Year;
        int month = date.Month;
        int day = date.Day;

        if (date.DayOfWeek == DayOfWeek.Saturday || date.DayOfWeek == DayOfWeek.Sunday) return true;

        if (year == 2013)
        {
            if (month == 1 && day == 1 ||
                month == 3 && (day == 28 || day == 29) ||
                month == 4 && (day == 1 || day == 30) ||
                month == 5 && (day == 1 || day == 8 || day == 9) ||
                month == 6 && (day == 5 || day == 6 || day == 21) ||
                month == 7 ||
                month == 11 && day == 1 ||
                month == 12 && (day == 24 || day == 25 || day == 26 || day == 31))
            {
                return true;
            }
        }
        return false;
    }

    private enum TollFreeVehicles
    {
        Motorbike = 0,
        Tractor = 1,
        Emergency = 2,
        Diplomat = 3,
        Foreign = 4,
        Military = 5
    }
}