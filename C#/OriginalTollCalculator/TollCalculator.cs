using System;
using System.Globalization;
using TollFeeCalculator;
using TollFeeCalculator.Utilities;

public class TollCalculator
{

    // Minor changes to this class has been made to solve the problem.
    // I might have gotten carried away and overbuilt the solution...

    /**
     * Calculate the total toll fee for one day
     *
     * @param vehicle - the vehicle
     * @param dates   - date and time of all passes on one day
     * @return - the total toll fee for that day
     */

    public int GetTollFee(Vehicle vehicle, DateTime[] dates)
    {
        int totalFee = 0;
        int highestFeeInHour = 0;
        DateTime startOfPeriod, endOfPeriod;

        for (int i = 0; i < dates.Length;)
        {
            startOfPeriod = dates[i];
            endOfPeriod = startOfPeriod.AddHours(1);
            if (!dates.Any(passage => passage > startOfPeriod && passage <= endOfPeriod))
            {
                highestFeeInHour = TollStationPassage.GetTollFee(dates[i]);
                i++;
            }
            else
            {
                highestFeeInHour = TollStationPassage.GetTollFee(dates[i]);
                for (int j = 1; j < dates.Length - i; j++)
                {
                    if (dates[i + j] > endOfPeriod)
                    {
                        i += j;
                        break;
                    }
                    if (TollStationPassage.GetTollFee(dates[i + j]) > highestFeeInHour)
                    {
                        highestFeeInHour = TollStationPassage.GetTollFee(dates[i + j]);
                    }
                }
            }
            totalFee += highestFeeInHour;
        }
        if (totalFee > 60) totalFee = 60;
        return totalFee;
    }

    private bool IsTollFreeVehicle(Vehicle vehicle)
    {
        if (vehicle != null)
        {
            string vehicleType = vehicle.GetVehicleType();
            return Enum.IsDefined(typeof(TollFreeVehicles), vehicleType);
        }
        return false;
    }

    public int GetTollFee(DateTime date, Vehicle vehicle)
    {
        if (IsTollFreeDate(date) || IsTollFreeVehicle(vehicle)) return 0;

        int hour = date.Hour;
        int minute = date.Minute;

        if ((hour < 6 && hour > 18) || (hour == 18 && minute == 30))
        {
            return 0;
        }
        else
        {
            if (hour == 18 ||
               (hour == 6 && minute == 0) ||
               (hour == 8 && minute == 0) ||
               (hour >= 9 && hour <= 14))
            {
                return TollFee.FeeLevel1;
            }
            else if ((hour == 6 && minute == 30) ||
                     (hour == 8 && minute == 0) ||
                     (hour == 15 && minute == 0) ||
                      hour == 17)
            {
                return TollFee.FeeLevel2;
            }
            else
            {
                return TollFee.FeeLevel3;
            }
        }
    }

    private Boolean IsTollFreeDate(DateTime date)
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
}