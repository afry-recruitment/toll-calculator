using TollCalculator.Models;

public class TollFeeCalculator
{
    public const int MAXIMUM_DAILY_TOLL_FEE_AMOUNT = 60;

    public int CalculateTollFee(Vehicle vehicle, DateTime[] dates)
    {
        if (vehicle == null || dates == null)
        {
            throw new ArgumentNullException();
        }

        // since I hardcoded holiday dates and they change every year
        if (dates.Any(x => x.Year != 2023))
        {
            throw new Exception();
        }

        if (vehicle.IsTollFree || IsTollFreeDate(dates[0]))
        {
            return 0;
        }

        Array.Sort(dates);

        // start by evaluating first index of array and set totalFee to that value incase length = 1
        var prevDate = dates[0];
        int prevFee = GetTollFee(prevDate);
        int totalFee = prevFee;

        for (int i = 1; i < dates.Length; i++)
        {
            var currDate = dates[i];
            int currFee = GetTollFee(currDate);

            TimeSpan timeDiff = currDate.TimeOfDay - prevDate.TimeOfDay;

            if (timeDiff.TotalMinutes >= 60)
            {
                totalFee += currFee;
            }

            else if (currFee > prevFee)
            {
                totalFee += currFee - prevFee;
            }

            prevFee = currFee;
            prevDate = currDate;
        }

        return Math.Min(totalFee, MAXIMUM_DAILY_TOLL_FEE_AMOUNT);
    }

    public int GetTollFee(DateTime date)
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
    // Decided on using hard-coded holidays to avoid complexity, see README on point 6.
    public bool IsTollFreeDate(DateTime date)
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