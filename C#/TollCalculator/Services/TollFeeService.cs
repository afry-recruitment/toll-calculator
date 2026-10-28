using TollCalculator.Interfaces;

namespace TollCalculator.Services;

public class TollFeeService
{
    /**
     * Calculate the total toll fee for one day
     *
     * @param vehicle - the vehicle
     * @param dates   - date and time of all passes on one day
     * @return - the total toll fee for that day
     */

    // Fee charged for each time period.
    // The end time is exclusive.
    private static readonly (TimeOnly Start, TimeOnly End, int Fee)[] FeeSchedule =
[
    (new(6, 0),  new(6, 30),  8),
    (new(6, 30), new(7, 0),  13),
    (new(7, 0),  new(8, 0),  18),
    (new(8, 0),  new(8, 30), 13),
    (new(8, 30), new(15, 0),  8),
    (new(15, 0), new(15, 30), 13),
    (new(15, 30),new(17, 0), 18),
    (new(17, 0), new(18, 0), 13),
    (new(18, 0), new(18, 30), 8)
];


    public int GetTollFee(Vehicle vehicle, DateTime[] dates)
    {
        ArgumentNullException.ThrowIfNull(dates);

        if (dates.Length == 0)
            return 0;

        DateTime firstDate = dates[0].Date;

        if (dates.Any(date => date.Date != firstDate))
            throw new ArgumentException(
                "All passages must belong to the same day.",
                nameof(dates));

        // Sort a copy so the original array remains unchanged.
        DateTime[] sortedDates = dates.OrderBy(date => date).ToArray();

        DateTime intervalStart = sortedDates[0];
        int highestFee = GetTollFee(intervalStart, vehicle);
        int totalFee = 0;

        foreach (DateTime date in sortedDates.Skip(1))
        {
            // Passages within 60 minutes are charged at the highest fee.
            if (date - intervalStart <= TimeSpan.FromHours(1))
            {
                highestFee = Math.Max(highestFee, GetTollFee(date, vehicle));
                continue;
            }

            totalFee += highestFee;

            if (totalFee >= 60)
                return 60;

            intervalStart = date;
            highestFee = GetTollFee(date, vehicle);
        }

        return Math.Min(totalFee + highestFee, 60);
    }

    private bool IsTollFreeVehicle(Vehicle vehicle)
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

    // Calculates the toll fee for a single passage.
    public int GetTollFee(DateTime date, Vehicle vehicle)
    {
        if (IsTollFreeDate(date) || IsTollFreeVehicle(vehicle))
            return 0;

        TimeOnly time = TimeOnly.FromDateTime(date);

        foreach (var period in FeeSchedule)
        {
            if (time >= period.Start && time < period.End)
                return period.Fee;
        }

        // No fee is charged outside the defined time periods.
        return 0;
    }

    private bool IsTollFreeDate(DateTime date)
    {
        int year = date.Year;
        int month = date.Month;
        int day = date.Day;

        if (date.DayOfWeek == DayOfWeek.Saturday || date.DayOfWeek == DayOfWeek.Sunday) return true;

        // The holiday dates below are hardcoded for 2013 only, and we would have to update it to support other years
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