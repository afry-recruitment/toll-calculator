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

    private const int LowFee = 8;
    private const int MediumFee = 13;
    private const int HighFee = 18;

    public int GetTollFee(IVehicle vehicle, DateTime[] dates)
    {
        if (dates == null || dates.Length == 0)
            return 0;

        var orderedDates = dates.OrderBy(d => d).ToArray();

        const int maxDailyFee = 60;
        const int intervalMinutes = 60;

        int totalFee = 0;

        DateTime intervalStart = orderedDates[0];
        int intervalFee = GetTollFee(intervalStart, vehicle);

        foreach (var date in orderedDates.Skip(1))
        {
            int fee = GetTollFee(date, vehicle);

            if ((date - intervalStart).TotalMinutes <= intervalMinutes)
            {
                intervalFee = Math.Max(intervalFee, fee);
            }
            else
            {
                totalFee += intervalFee;

                if (totalFee >= maxDailyFee)
                    return maxDailyFee;

                intervalStart = date;
                intervalFee = fee;
            }
        }

        totalFee += intervalFee;

        return Math.Min(totalFee, maxDailyFee);
    }

    private bool IsTollFreeVehicle(IVehicle vehicle)
    {
        if (vehicle == null) return false;
        string vehicleType = vehicle.GetVehicleType();
        return vehicleType.Equals(TollFreeVehicles.Motorbike.ToString()) ||
               vehicleType.Equals(TollFreeVehicles.Tractor.ToString()) ||
               vehicleType.Equals(TollFreeVehicles.Emergency.ToString()) ||
               vehicleType.Equals(TollFreeVehicles.Diplomat.ToString()) ||
               vehicleType.Equals(TollFreeVehicles.Foreign.ToString()) ||
               vehicleType.Equals(TollFreeVehicles.Military.ToString());
    }

    

    public int GetTollFee(DateTime date, IVehicle vehicle)
    {
        if (IsTollFreeDate(date) || IsTollFreeVehicle(vehicle)) return 0;

        int hour = date.Hour;
        int minute = date.Minute;

        if (hour == 6 && minute <= 29) return LowFee;
        else if (hour == 6) return MediumFee;
        else if (hour == 7) return HighFee;
        else if (hour == 8 && minute <= 29) return MediumFee;
        else if ((hour == 8 && minute >= 30) || (hour >= 9 && hour <= 14)) return LowFee;
        else if (hour == 15 && minute <= 29) return MediumFee;
        else if ((hour == 15 && minute >= 30) || hour == 16) return HighFee;
        else if (hour == 17) return MediumFee;
        else if (hour == 18 && minute <= 29) return LowFee;

        return 0;
    }

    private bool IsTollFreeDate(DateTime date)
    {
        int year = date.Year;
        int month = date.Month;
        int day = date.Day;

        if (date.DayOfWeek == DayOfWeek.Saturday || date.DayOfWeek == DayOfWeek.Sunday) return true;

        if (year == 2013)
        {
            if (month == 1 && day == 1 ||
                month == 3 && (day == 28 || day == 29) ||
                month == 4 && (day == 1 || day == 30) ||
                month == 5 && (day == 1 || day == 8 || day == 9) ||
                month == 6 && (day == 5 || day == 6 || day == 21) ||
                month == 7 ||
                month == 11 && day == 1 ||
                month == 12 && (day == 24 || day == 25 || day == 26 || day == 31))
            {
                return true;
            }
        }
        return false;
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