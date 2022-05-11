using System;
using TollFeeCalculator;

public class TollCalculator
{
    /// <summary>
    /// Calculate the total toll fee for one day
    /// </summary>
    /// <param name="vehicle">the vehicle</param>
    /// <param name="dates">date and time of all passes on one day</param>
    /// <returns>the total toll fee for that day</returns>
    public int GetTollFee(Vehicle vehicle, DateTime[] dates)
    {
        if (IsTollFreeVehicle(vehicle)) return 0;

        var intervalStart = dates[0];
        var totalFee = 0;
        var highestFeeHour = 0;
        foreach (DateTime date in dates)
        {
            var ts = date - intervalStart;
            var fee = GetTollFee(date, vehicle);
            if (dates[0] == date)
            {
                highestFeeHour = fee;
            }
            else if (ts.Hours > 0 || ts.Minutes > 60)
            {
                totalFee += highestFeeHour;
                intervalStart = date;
                highestFeeHour = fee;
            }
            else if (highestFeeHour < fee)
            {
                highestFeeHour = fee;
            }
        }
        totalFee += highestFeeHour;
        if (totalFee > 60) totalFee = 60;
        return totalFee;
    }

    /// <summary>
    /// the toll cost for a vehicle at a given time
    /// </summary>
    /// <param name="date">time of passing</param>
    /// <param name="vehicle">what vehicle</param>
    /// <returns>fee</returns>
    public int GetTollFee(DateTime date, Vehicle vehicle)
    {
        var loger = new Loger();
        loger.LogPassing(vehicle, date);

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
    /// Checks of day is TollFree
    /// </summary>
    /// <param name="date">Date of passage</param>
    /// <returns>bool if is toll free or not</returns>
    private Boolean IsTollFreeDate(DateTime date) => date.DayOfWeek == DayOfWeek.Saturday || date.DayOfWeek == DayOfWeek.Sunday || ApiHelper.GetPublicHoliday(date);

    /// <summary>
    /// checks if Vehicle is tollFree
    /// </summary>
    /// <param name="vehicle">the Vehicle</param>
    /// <returns>bool if free</returns>
    private bool IsTollFreeVehicle(Vehicle vehicle)
    {
        if (vehicle == null) return false;
        var vehicleType = vehicle.GetVehicleType();
        return vehicleType.Equals(nameof(TollFreeVehicles.Motorbike)) ||
               vehicleType.Equals(nameof(TollFreeVehicles.Tractor)) ||
               vehicleType.Equals(nameof(TollFreeVehicles.Emergency)) ||
               vehicleType.Equals(nameof(TollFreeVehicles.Diplomat)) ||
               vehicleType.Equals(nameof(TollFreeVehicles.Foreign)) ||
               vehicleType.Equals(nameof(TollFreeVehicles.Military));
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