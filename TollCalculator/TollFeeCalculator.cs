using TollCalculator.Models;

public class TollFeeCalculator
{
    public const int MAXIMUM_DAILY_TOLL_FEE_AMOUNT = 60;

    public int CalculateTotalDailyTollFee(Vehicle vehicle, TollFee[] tollFees)
    {
        if (vehicle == null || tollFees == null)
        {
            throw new ArgumentNullException();
        }

        // check if any of the tollFees are from different days
        if (tollFees.Select(x => x.tollDate.Day).Distinct().Count() < 1)
        {
            throw new Exception();
        }

        // check if all tollFees belong to right vehicle
        if (!tollFees.All(x => x.vehicleLicensePlate == vehicle.LicensePlate))
        {
            throw new Exception();
        }

        tollFees.OrderBy(t => t.tollDate);

        var totalFee = tollFees[0].tollFee;

        // stop calculating once we hit maximum daily amount
        while (totalFee >= MAXIMUM_DAILY_TOLL_FEE_AMOUNT)
        {
            var prevTollFee = tollFees[0].tollFee;
            var prevTollDate = tollFees[0].tollDate;

            for (int i = 1; i < tollFees.Length; i++)
            {
                var currTollDate = tollFees[i].tollDate;
                int currTollFee = tollFees[i].tollFee;

                TimeSpan timeDiff = currTollDate.TimeOfDay - prevTollDate.TimeOfDay;

                if (timeDiff.TotalMinutes >= 60)
                {
                    totalFee += currTollFee;
                }

                else if (currTollFee > prevTollFee)
                {
                    totalFee += currTollFee - prevTollFee;
                }

                prevTollFee = currTollFee;
                prevTollDate = currTollDate;
            }
        }

        return Math.Min(totalFee, MAXIMUM_DAILY_TOLL_FEE_AMOUNT);
    }

    // imagined scenario: every time a vehicle passes the toll booth
    // the time, toll cost for that hour and the vehicles license plate are saved to a db
    // at the end of the day, all passings for the vehicle are retrived and the totalcost is calculated above
    public TollFee CalculateTollFee(Vehicle vehicle, DateTime date)
    {
        if (vehicle == null)
        {
            throw new ArgumentNullException();
        }

        if (date.Year != 2023)
        {
            throw new Exception();
        }

        if (vehicle.IsTollFree || IsTollFreeDate(date))
        {
            return new TollFee(0, date, vehicle.LicensePlate);
        }

        var tollFee = GetTollFeeForHour(date);
        return new TollFee(tollFee, date, vehicle.LicensePlate);
    }

    public int GetTollFeeForHour(DateTime date)
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