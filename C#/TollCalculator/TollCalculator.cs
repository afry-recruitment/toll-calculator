namespace TollFeeCalculator;

public class TollCalculator
{
    private const int MaxDailyFee = 60;
    private const int IntervalMinutes = 60;

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
