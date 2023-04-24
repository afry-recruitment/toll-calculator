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

    private static int GetTollFeeAmount(DateTime date)
    {
        switch (date.Hour)
        {
            case 6:
                return date.Minute >= 0 && date.Minute <= 29
                    ? 8
                    : 13;

            case 7:
                return 18;

            case 8:
                return date.Minute >= 0 && date.Minute <= 29
                    ? 8
                    : 13;

            case 15:
                return date.Minute >= 0 && date.Minute <= 29
                    ? 13
                    : 18;

            case 17:
                return 13;

            case 18:
                return 8;
        }

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