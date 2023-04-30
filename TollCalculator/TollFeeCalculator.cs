using PublicHoliday;
using TollCalculator.Exceptions;
using TollCalculator.Models;

public static class TollFeeCalculator
{
    public const int MAXIMUM_DAILY_TOLL_FEE_AMOUNT = 60;

    public static int CalculateTotalDailyTollFee(Vehicle vehicle, TollFee[] tollFees)
    {
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
        if (IsTollFreeVehicle(vehicle) || IsTollFreeDate(date))
        {
            return new TollFee(0, date, vehicle.LicensePlate);
        }

        return new TollFee(GetTollFeeAmount(date), date, vehicle.LicensePlate);
    }

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
    public static bool IsTollFreeVehicle(Vehicle vehicle)
    {
        return Enum.IsDefined(typeof(TollFreeVehicles), vehicle.GetType().Name);
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