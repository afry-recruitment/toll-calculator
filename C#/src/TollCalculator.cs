namespace TollFeeCalculator;

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
        if (vehicle == null)
        {
            throw new ArgumentNullException($"{nameof(vehicle)} cannot be null.");
        }

        if (dates == null)
        {
            throw new ArgumentNullException($"{nameof(dates)} cannot be null.");
        }

        if (IsTollFreeVehicle(vehicle))
        {
            return 0;
        }

        if (dates.Length == 0)
        {
            return 0;
        }

        var sortedDates = dates.ToList();
        sortedDates.Sort((a, b) => a.CompareTo(b));

        int total = 0;
        var day = new List<DateTime>();
        var curr = sortedDates[0];
        if (curr.Year != 2013)
        {
            throw new ArgumentException($"{nameof(TollCalculator)}.{nameof(GetTollFee)} can only process year 2013.");
        }
        day.Add(curr);

        for (int i = 1; i < sortedDates.Count; i++)
        {
            var next = sortedDates[i];
            if (next.Year != 2013)
            {
                throw new ArgumentException($"{nameof(TollCalculator)}.{nameof(GetTollFee)} can only process year 2013.");
            }

            if (next.DayOfYear == curr.DayOfYear)
            {
                day.Add(next);
            }
            else
            {
                total += GetFeeForDay(day);
                day = new List<DateTime>();
                curr = sortedDates[i];
                if (curr.Year != 2013)
                {
                    throw new ArgumentException($"{nameof(TollCalculator)}.{nameof(GetTollFee)} can only process year 2013.");
                }
                day.Add(curr);
            }
        }

        // sortedDates.Count == 1 jumps here.
        total += GetFeeForDay(day);
        return total;
    }

    private static bool IsTollFreeVehicle(Vehicle vehicle) => vehicle switch
    {
        Diplomat _ => true,
        Emergency _ => true,
        Foreign _ => true,
        Military _ => true,
        Motorbike _ => true,
        Tractor _ => true,
        _ => false
    };

    private static int GetFeeForDay(List<DateTime> day)
    {
        if (day == null)
        {
            throw new ArgumentNullException($"{nameof(day)} cannot be null.");
        }

        if (day.Count == 0)
        {
            return 0;
        }

        if (IsTollFreeDay(day[0].DayOfYear))
        {
            return 0;
        }

        if (day.Count == 1)
        {
            return GetTollFeeByTime(day[0]);
        }

        int total = 0;
        var hour = new List<DateTime>();

        var curr = day[0];
        hour.Add(curr);
        var limit = curr.AddHours(1);

        for (int i = 1; i < day.Count; i++)
        {
            var next = day[i];
            if (next < limit)
            {
                hour.Add(next);
            }
            else
            {
                total += GetHighestFeeInHour(hour);
                hour = new List<DateTime>();
                curr = day[i];
                hour.Add(curr);
                limit = curr.AddHours(1);
            }
        }
        
        // day.Count == 1 jumps here.
        total += GetHighestFeeInHour(hour);

        const int maximumFee = 60;
        if (total > maximumFee)
        {
            total = maximumFee;
        }

        return total;
    }

    private static readonly int[] _tollFreeDays = [
        1, 5, 6, 12, 13, 19, 20, 26, 27, 33, 34, 40, 41, 47, 48, 54, 55, 61, 62, 68, 69,
        75, 76, 82, 83, 87, 88, 89, 90, 91, 96, 97, 103, 104, 110, 111, 117, 118, 120,
        121, 124, 125, 128, 129, 131, 132, 138, 139, 145, 146, 152, 153, 156, 157, 159,
        160, 166, 167, 172, 173, 174, 180, 181, 182, 183, 184, 185, 186, 187, 188, 189,
        190, 191, 192, 193, 194, 195, 196, 197, 198, 199, 200, 201, 202, 203, 204, 205,
        206, 207, 208, 209, 210, 211, 212, 215, 216, 222, 223, 229, 230, 236, 237, 243,
        244, 250, 251, 257, 258, 264, 265, 271, 272, 278, 279, 285, 286, 292, 293, 299,
        300, 305, 306, 307, 313, 314, 320, 321, 327, 328, 334, 335, 341, 342, 348, 349,
        355, 356, 358, 359, 360, 362, 363, 365
    ];

    private static bool IsTollFreeDay(int dayOfYear)
    {
        return -1 < _tollFreeDays.BinarySearch(dayOfYear);
    }

    private static int GetHighestFeeInHour(List<DateTime> hour)
    {
        if (hour == null)
        {
            throw new ArgumentNullException($"{nameof(hour)} cannot be null.");
        }

        if (hour.Count == 0)
        {
            return 0;
        }

        if (hour.Count == 1)
        {
            return GetTollFeeByTime(hour[0]);
        }

        int highestFee = 0;
        for (int i = 0; i < hour.Count; i++)
        {
            int fee = GetTollFeeByTime(hour[i]);
            if (fee > highestFee)
            {
                highestFee = fee;
            }
        }

        return highestFee;
    }

    private static int GetTollFeeByTime(DateTime date)
    {
        int minute = (date.Hour * 60) + date.Minute;

        // 06:00 - 06:29 = 8 kr 
        const int t_0600 = 6 * 60;
        const int t_0629 = (6 * 60) + 29;
        if (minute >= t_0600 && minute <= t_0629)
        {
            return 8;
        }

        // 06:30 - 06:59 = 13 kr
        const int t_0630 = (6 * 60) + 30;
        const int t_0659 = (6 * 60) + 59;
        if (minute >= t_0630 && minute <= t_0659)
        {
            return 13;
        }

        // 07:00 - 07:59 = 18 kr
        const int t_0700 = 7 * 60;
        const int t_0759 = (7 * 60) + 59;
        if (minute >= t_0700 && minute <= t_0759)
        {
            return 18;
        }

        // 08:00 - 08:29 = 13 kr
        const int t_0800 = 8 * 60;
        const int t_0829 = (8 * 60) + 29;
        if (minute >= t_0800 && minute <= t_0829)
        {
            return 13;
        }

        // 08:30 - 14:59 = 8 kr
        const int t_0830 = (8 * 60) + 30;
        const int t_1459 = (14 * 60) + 59;
        if (minute >= t_0830 && minute <= t_1459)
        {
            return 8;
        }

        // 15:00 - 15:29 = 13 kr
        const int t_1500 = 15 * 60;
        const int t_1529 = (15 * 60) + 29;
        if (minute >= t_1500 && minute <= t_1529)
        {
            return 13;
        }

        // 15:30 - 16:59 = 18 kr
        const int t_1530 = (15 * 60) + 30;
        const int t_1659 = (16 * 60) + 59;
        if (minute >= t_1530 && minute <= t_1659)
        {
            return 18;
        }

        // 17:00 - 17:59 = 13 kr
        const int t_1700 = 17 * 60;
        const int t_1759 = (17 * 60) + 59;
        if (minute >= t_1700 && minute <= t_1759)
        {
            return 13;
        }

        // 18:00 - 18:29 = 8 kr
        const int t_1800 = 18 * 60;
        const int t_1829 = (18 * 60) + 29;
        if (minute >= t_1800 && minute <= t_1829)
        {
            return 8;
        }

        // 00:00 - 05:59 = 0 kr
        // 18:30 - 23:59 = 0 kr
        return 0;
    }
}