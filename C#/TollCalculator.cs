using System;
using System.Linq;
using TollFeeCalculator;

public class TollCalculator
{
    private const int MaximumDailyFee = 60;
    private const int LowFee = 8;
    private const int MediumFee = 13;
    private const int HighFee = 18;


    /// <summary>
    /// Calculates the total toll fee for one day. Passages within one hour of
    /// an interval's first passage are charged at the highest applicable fee;
    /// the day's total is capped at 60 SEK.
    /// </summary>
    /// <param name="vehicle">The vehicle making the passages.</param>
    /// <param name="dates">The passage times of one day, in any order.</param>
    /// <returns>The total toll fee for that day, or zero when no passage times are supplied.</returns>
    /// <exception cref="ArgumentNullException"><paramref name="dates"/> is null.</exception>
    /// <exception cref="ArgumentException"><paramref name="dates"/> contains passages from more than one day.</exception>
    public int GetTollFee(Vehicle vehicle, DateTime[] dates)
    {
        if (dates == null)
        {
            throw new ArgumentNullException(nameof(dates));
        }

        if (dates.Length == 0)
        {
            return 0;
        }

        if (dates.Select(date => date.Date).Distinct().Count() > 1)
        {
            throw new ArgumentException("All passage times must be on the same day.", nameof(dates));
        }

        DateTime[] orderedDates = dates.OrderBy(date => date).ToArray();
        DateTime intervalStart = orderedDates[0];
        int intervalFee = GetTollFee(intervalStart, vehicle);
        int dailyFee = 0;

        foreach (DateTime date in orderedDates.Skip(1))
        {
            int fee = GetTollFee(date, vehicle);
            if (date <= intervalStart.AddHours(1))
            {
                intervalFee = Math.Max(intervalFee, fee);
            }
            else
            {
                dailyFee += intervalFee;
                intervalStart = date;
                intervalFee = fee;
            }
        }

        return Math.Min(dailyFee + intervalFee, MaximumDailyFee);
    }

    /// <summary>
    /// Returns the toll fee for a single passage. Fee-free dates (weekends,
    /// public holidays) and fee-free vehicle types always return 0.
    /// </summary>
    /// <param name="date">The date and time of the passage.</param>
    /// <param name="vehicle">The vehicle making the passage.</param>
    /// <returns>The fee for this passage, in SEK, before the hourly and daily rules are applied.</returns>
    public int GetTollFee(DateTime date, Vehicle vehicle)
    {
        if (IsTollFreeDate(date) || IsTollFreeVehicle(vehicle))
        {
            return 0;
        }

        int minuteOfDay = date.Hour * 60 + date.Minute;

        if (minuteOfDay >= 360 && minuteOfDay < 390) return LowFee;    // 06:00–06:29
        if (minuteOfDay < 420 && minuteOfDay >= 390) return MediumFee; // 06:30–06:59
        if (minuteOfDay < 480 && minuteOfDay >= 420) return HighFee;   // 07:00–07:59
        if (minuteOfDay < 510 && minuteOfDay >= 480) return MediumFee; // 08:00–08:29
        if (minuteOfDay < 900 && minuteOfDay >= 510) return LowFee;    // 08:30–14:59
        if (minuteOfDay < 930 && minuteOfDay >= 900) return MediumFee; // 15:00–15:29
        if (minuteOfDay < 1020 && minuteOfDay >= 930) return HighFee;  // 15:30–16:59
        if (minuteOfDay < 1080 && minuteOfDay >= 1020) return MediumFee; // 17:00–17:59
        if (minuteOfDay < 1110 && minuteOfDay >= 1080) return LowFee;  // 18:00–18:29

        return 0;
    }

    private static bool IsTollFreeVehicle(Vehicle vehicle)
    {
        if (vehicle == null)
        {
            return false;
        }

        string vehicleType = vehicle.GetVehicleType();
        return Enum.GetNames(typeof(TollFreeVehicles))
            .Any(name => String.Equals(name, vehicleType, StringComparison.OrdinalIgnoreCase));
    }

    private static bool IsTollFreeDate(DateTime date)
    {
        DateTime day = date.Date;

        if (day.DayOfWeek == DayOfWeek.Saturday ||
            day.DayOfWeek == DayOfWeek.Sunday)
        {
            return true;
        }

        return IsSwedishPublicHoliday(day);
    }

    private static bool IsSwedishPublicHoliday(DateTime date)
    {
        // Recognizes New Year's Day, Epiphany, Good Friday, Easter Sunday and
        // Easter Monday, May Day, Ascension Day, Sweden's National Day,
        // Midsummer Day, All Saints' Day, Christmas Day, and Boxing Day.
        // Good Friday is Easter Sunday minus 2 days, Easter Monday is plus 1,
        // and Ascension Day is plus 39 days. Whit Sunday always falls on a
        // Sunday, so it is covered by the weekend exemption.
        int year = date.Year;
        DateTime easterSunday = GetEasterSunday(year);

        return (date.Month == 1 && (date.Day == 1 || date.Day == 6)) ||
               date == easterSunday.AddDays(-2) ||
               date == easterSunday ||
               date == easterSunday.AddDays(1) ||
               (date.Month == 5 && date.Day == 1) ||
               date == easterSunday.AddDays(39) ||
               (date.Month == 6 && date.Day == 6) ||
               (date.Month == 12 && (date.Day == 25 || date.Day == 26));
    }

    /// <summary>
    /// Calculates the date of Easter Sunday for the given year using the
    /// anonymous Gregorian algorithm (Meeus/Jones/Butcher). Easter is needed
    /// because several Swedish public holidays are defined relative to it.
    /// </summary>
    private static DateTime GetEasterSunday(int year)
    {
        int a = year % 19;
        int b = year / 100;
        int c = year % 100;
        int d = b / 4;
        int e = b % 4;
        int f = (b + 8) / 25;
        int g = (b - f + 1) / 3;
        int h = (19 * a + b - d - g + 15) % 30;
        int i = c / 4;
        int k = c % 4;
        int l = (32 + 2 * e + 2 * i - h - k) % 7;
        int m = (a + 11 * h + 22 * l) / 451;
        int month = (h + l - 7 * m + 114) / 31;
        int day = (h + l - 7 * m + 114) % 31 + 1;

        return new DateTime(year, month, day);
    }

    private enum TollFreeVehicles
    {
        Motorbike,
        Tractor,
        Emergency,
        Diplomat,
        Foreign,
        Military
    }
}
