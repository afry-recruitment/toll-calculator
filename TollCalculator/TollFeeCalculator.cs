using PublicHoliday;
using TollCalculator.Exceptions;
using TollCalculator.Models;

public static class TollFeeCalculator
{
    public const int MAXIMUM_DAILY_TOLL_FEE_AMOUNT = 60;

    public static int CalculateTotalDailyTollFee(Vehicle vehicle, TollFee[] tollFees, DateTime? bypassException = null)
    {
        // To bypass the UtcNow verification I added the nullable DateTime param so its testable
        if (bypassException == null)
        {
            // Sweden is UTC+2, tolls close at 18:30, calculation should be done long before midnight
            if (DateTime.UtcNow.AddHours(2).TimeOfDay.TotalHours < 18.30)
            {
                throw new TollsHaveNotClosedException();
            }
        }

        if (tollFees.Any(x => x.TollDate.Day != DateTime.Today.Day))
        {
            throw new TollDateDayException();
        }

        if (!tollFees.All(x => x.VehicleLicensePlate == vehicle.LicensePlate))
        {
            throw new VehicleLicensePlateException();
        }

        tollFees.OrderBy(t => t.TollDate);

        var totalDailyTollFeeAmount = tollFees[0].TollFeeAmount;
        var prevTollFeeAmount = tollFees[0].TollFeeAmount;
        var prevTollDate = tollFees[0].TollDate;

        for (int i = 1; i < tollFees.Length; i++)
        {
            var currTollDate = tollFees[i].TollDate;
            var currTollFeeAmount = tollFees[i].TollFeeAmount;

            TimeSpan timeDiff = currTollDate.TimeOfDay - prevTollDate.TimeOfDay;

            if (timeDiff.TotalMinutes >= 60)
            {
                totalDailyTollFeeAmount += currTollFeeAmount;
            }

            else if (currTollFeeAmount > prevTollFeeAmount)
            {
                totalDailyTollFeeAmount += currTollFeeAmount - prevTollFeeAmount;
            }

            prevTollFeeAmount = currTollFeeAmount;
            prevTollDate = currTollDate;
        }

        return Math.Min(totalDailyTollFeeAmount, MAXIMUM_DAILY_TOLL_FEE_AMOUNT);
    }

    public static TollFee NewTollFee(Vehicle vehicle, DateTime date)
    {
        if (date.Year != DateTime.Now.Year)
        {
            throw new NotCurrentYearException();
        }

        if (vehicle.IsTollFree || IsTollFreeDate(date))
        {
            return new TollFee(0, date, vehicle.LicensePlate);
        }

        return new TollFee(GetTollFeeAmount(date), date, vehicle.LicensePlate);
    }

    // I have based the times and prices on the original function
    // but it didnt consider the first 30 minutes of every hour between 09-14
    // I assume it was a bug and set those times to always cost 8 SEK
    public static int GetTollFeeAmount(DateTime date)
    {
        if (date.Hour == 6)
            return date.Minute <= 29 ? 8 : 13;

        if (date.Hour == 7)
            return 18;

        if (date.Hour == 8)
            return date.Minute <= 29 ? 13 : 8;

        if (date.Hour >= 9 && date.Hour <= 14)
            return 8;

        if (date.Hour == 15)
            return date.Minute <= 29 ? 13 : 18;

        if (date.Hour == 16)
            return 18;

        if (date.Hour == 17)
            return 13;

        if (date.Hour == 18)
            return date.Minute <= 29 ? 8 : 0;

        return 0;
    }

    public static bool IsTollFreeDate(DateTime date)
    {
        return date.DayOfWeek == DayOfWeek.Saturday || date.DayOfWeek == DayOfWeek.Sunday
            || new SwedenPublicHoliday().PublicHolidays(date.Year).Contains(date);
    }
}