using PublicHoliday;
using System;
using TollFeeCalculator;

public class TollCalculator
{
    /// <summary>
    /// Calculate the total toll fee for one day.
    /// </summary>
    /// <param name="vehicle">The <see cref="Vehicle"/>.</param>
    /// <param name="dates">The date and time of all passes on one day.</param>
    /// <returns>A <see langword="int"/> with the total toll fee.</returns>
    public int GetTollFee(Vehicle vehicle, DateTime[] dates)
    {
        DateTime intervalStart = dates[0];
        int highestFee = 0;

        int totalFee = 0;
        for (int i = 0; i < dates.Length; i++)
        {
            int tempFee = 0;

            var timeDifference = dates[i].Subtract(intervalStart);

            if (timeDifference.TotalMinutes > 60)
            {   
                totalFee += highestFee;
                intervalStart = dates[i];
                highestFee = GetTollFee(dates[i], vehicle);
            }
            else
            {
                tempFee = GetTollFee(dates[i], vehicle);
                if (tempFee > highestFee) highestFee = tempFee;
            }
        }

        totalFee += highestFee;

        if (totalFee > 60) totalFee = 60;
        return totalFee;
    }

    /// <summary>
    /// Check if the <see cref="Vehicle"/> is toll free or not.
    /// </summary>
    /// <param name="vehicle">The <see cref="Vehicle"/>.</param>
    /// <returns><see langword="true"/> if the <see cref="Vehicle"/> is toll free and <see langword="false"/> if not.</returns>
    private bool IsTollFreeVehicle(Vehicle vehicle)
    {
        if (vehicle == null) return false;
        String vehicleType = vehicle.GetVehicleType();
        return vehicleType.Equals(TollFreeVehicles.Motorbike.ToString()) ||
               vehicleType.Equals(TollFreeVehicles.Tractor.ToString()) ||
               vehicleType.Equals(TollFreeVehicles.Emergency.ToString()) ||
               vehicleType.Equals(TollFreeVehicles.Diplomat.ToString()) ||
               vehicleType.Equals(TollFreeVehicles.Foreign.ToString()) ||
               vehicleType.Equals(TollFreeVehicles.Military.ToString());
    }

    /// <summary>
    /// Gets the totla toll fee.
    /// </summary>
    /// <param name="date">The date.</param>
    /// <param name="vehicle">The <see cref="Vehicle"/>.</param>
    /// <returns>The toll fee.</returns>
    public int GetTollFee(DateTime date, Vehicle vehicle)
    {
        if (IsTollFreeDate(date) || IsTollFreeVehicle(vehicle)) return 0;

        int hour = date.Hour;
        int minute = date.Minute;

        if (hour == 6 && minute >= 0 && minute <= 29) return 8;
        else if (hour == 6 && minute >= 30 && minute <= 59) return 13;
        else if (hour == 7 && minute >= 0 && minute <= 59) return 18;
        else if (hour == 8 && minute >= 0 && minute <= 29) return 13;
        else if (hour >= 8 && hour <= 14 && minute >= 30 && minute <= 59) return 8;
        else if (hour == 15 && minute >= 0 && minute <= 29) return 13;
        else if (hour == 15 && minute >= 0 || hour == 16 && minute <= 59) return 18;
        else if (hour == 17 && minute >= 0 && minute <= 59) return 13;
        else if (hour == 18 && minute >= 0 && minute <= 29) return 8;
        else return 0;
    }

    /// <summary>
    /// Checks if the date is a toll free date.
    /// </summary>
    /// <param name="date">The date.</param>
    /// <returns><see langword="true"/> if it is a toll free date and <see langword="false"/> if it is not.</returns>
    private Boolean IsTollFreeDate(DateTime date)
    {
        var publicHolidays = new SwedenPublicHoliday().IsPublicHoliday(date);
        if (date.DayOfWeek == DayOfWeek.Saturday || date.DayOfWeek == DayOfWeek.Sunday || publicHolidays) return true;
        return false;
    }

    /// <summary>
    /// All toll free vehicles.
    /// </summary>
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