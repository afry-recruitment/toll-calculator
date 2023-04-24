using System.Reflection.Metadata.Ecma335;
using TollCalculator.Models;

public static class TollFeeCalculator
{
    public const int MAXIMUM_DAILY_TOLL_FEE_AMOUNT = 60;

    public static int CalculateTotalDailyTollFee(Vehicle vehicle, TollFee[] tollFees)
    {
        if (DateTime.UtcNow.Hour < 19)
        {
            throw new Exception("Cannot start calculating daily total fees before tolls have closed.");
        }

        if (tollFees.Any(x => x.TollDate.Day != DateTime.Today.Day))
        {
            throw new Exception("Can only calculate toll fees from earlier today.");
        }

        if (!tollFees.All(x => x.VehicleLicensePlate == vehicle.LicensePlate))
        {
            throw new Exception("Vehicle license plate mismatch.");
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
        if (date.Year != 2023)
        {
            throw new Exception();
        }

        if (vehicle.IsTollFree || IsTollFreeDate(date))
        {
            return new TollFee(0, date, vehicle.LicensePlate);
        }

        var tollFeeAmount = GetTollFeeAmount(date);
        return new TollFee(tollFeeAmount, date, vehicle.LicensePlate);
    }

    // Toll fees as per original code
    // 6:00 - 6:29 = 8 SEK
    // 6:30 - 6:59 = 13 SEK
    // 7:00 - 7:59 = 18 SEK
    // 08:00 - 08:29 = 13 SEK
    // 08:30 - 14:30 = 8 SEK <---- bug, doesnt count 9:00 - 9:29, 10:00 - 10:29 etc
    // 15:00 - 15:29 = 13 SEK 
    // 15:00 - 16:59 = 18 SEK <---- bug in original code, assuming it should be 15:30 - 16:59
    // 17:00 - 17:59 = 13 SEK
    // 18:00 - 18:29 = 8 SEK
    private static int GetTollFeeAmount(DateTime date)
    {
        if (date.Hour == 6)
            return date.Minute <= 29 ? 8 : 13;

        if (date.Hour == 7)
            return 13;

        if (date.Hour == 8)
            return date.Minute <= 29 ? 13 : 8;

        if (date.Hour >= 9 || date.Hour <= 14)
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

    // Swedish holidays taken from https://www.kalender.se/helgdagar for 2023
    // Decided on using hard-coded holidays to for simplicity, see README on point 6.
    private static bool IsTollFreeDate(DateTime date)
    {
        if (date.DayOfWeek == DayOfWeek.Saturday || date.DayOfWeek == DayOfWeek.Sunday)
        {
            return true;
        }

        switch (date.Month)
        {
            case 1:
                return date.Day == 1 || date.Day == 6;

            case 4:
                return date.Day == 7 || date.Day == 9 || date.Day == 10;

            case 5:
                return date.Day == 1 || date.Day == 18 || date.Day == 28;

            case 6:
                return date.Day == 6 || date.Day == 24;

            case 11:
                return date.Day == 4;

            case 12:
                return date.Day == 25 || date.Day == 26;

            default:
                return false;
        }
    }
}