using TollFeeCalculator;

public class TollCalculator
{

    /**
     * Calculate the total toll fee for one day
     *
     * @param vehicle - the vehicle
     * @param dates   - date and time of all passes on one day
     * @return - the total toll fee for that day
     */

    public int GetTollFee(Vehicle vehicle, DateTime[] dates)
    {
        if (IsTollFreeVehicle(vehicle) || dates is null || dates.Length == 0) return 0;
        int totalFee = 0;
        DateTime? previousTollTime = null;
        int previousFee = 0;

        foreach (var time in dates)
        {
            var currentFee = GetTollFee(time, vehicle);
            if (!previousTollTime.HasValue || !IsWithinOneHour(time, previousTollTime.Value))
            {
                totalFee += currentFee;
            }
            else
            {
                if (currentFee > previousFee)
                {
                    totalFee += currentFee - previousFee;
                }
            }
            previousTollTime = time;
            previousFee = currentFee;
        }
        return Math.Min(totalFee, 60);
    }
    private bool IsWithinOneHour(DateTime currentTime, DateTime previousTime)
    {
        var timeSinceLastToll = currentTime - previousTime;
        return timeSinceLastToll <= TimeSpan.FromMinutes(60);
    }
    private bool IsTollFreeVehicle(Vehicle vehicle)
    {
        // readable, scalable, maintainable generic code. instead of better performance, but less readable, less maintainable, less scalable code.
        var tollFreeVehicleTypes = Enum.GetNames(typeof(TollFreeVehicles));
        return tollFreeVehicleTypes.Any(tollFreeVehicleTypes => tollFreeVehicleTypes.Equals(vehicle.GetVehicleType()));
    }

    private static readonly List<(TimeSpan Start, TimeSpan End, int Fee)> TollRates = new()
    {
        (new TimeSpan(6, 0, 0), new TimeSpan(6, 29, 59), 8),
        (new TimeSpan(6, 30, 0), new TimeSpan(6, 59, 59), 13),
        (new TimeSpan(7, 0, 0), new TimeSpan(7, 59, 59), 18),
        (new TimeSpan(8, 0, 0), new TimeSpan(8, 29, 59), 13),
        (new TimeSpan(8, 30, 0), new TimeSpan(14, 59, 59), 8),
        (new TimeSpan(15, 0, 0), new TimeSpan(15, 29, 59), 13),
        (new TimeSpan(15, 30, 0), new TimeSpan(16, 59, 59), 18),
        (new TimeSpan(17, 0, 0), new TimeSpan(17, 59, 59), 13),
        (new TimeSpan(18, 0, 0), new TimeSpan(18, 29, 59), 8),
    };

    public int GetTollFee(DateTime date, Vehicle vehicle)
    {
        if (IsTollFreeDate(date) || IsTollFreeVehicle(vehicle)) return 0;
        var time = date.TimeOfDay;
        var result = 0;
        foreach (var (start, end, fee) in TollRates)
        {
            if (time >= start && time <= end)
            {
                result = fee;
                break;
            }
        }
        return result;
    }

    private static readonly HashSet<(int Month, int Day)> TollFreeDates = new()
    {
        (1, 1),
        (3, 28), (3, 29),
        (4, 1), (4, 30),
        (5, 1), (5, 8), (5, 9),
        (6, 5), (6, 6), (6, 21),
        (11, 1),
        (12, 24), (12, 25), (12, 26), (12, 31)
    };

    private Boolean IsTollFreeDate(DateTime date) =>
           date.DayOfWeek == DayOfWeek.Saturday
        || date.DayOfWeek == DayOfWeek.Sunday
        || date.Month == 7
        || TollFreeDates.Contains((date.Month, date.Day));

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