﻿using TollFeeCalculator;

public class TollCalculator
{
    const int DAILY_MAX_FEE = 60;

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

        var sortedDates = GetDateTimeSpans(dates);

        foreach(var sortedDatesList in sortedDates)
        {
            int oldFee = 0;

            foreach(var dt in sortedDatesList)
            {
                int newFee = GetTollFee(dt, vehicle);
                if(newFee > oldFee)
                {
                    oldFee = newFee;
                }
            }
            
            totalFee += oldFee;
        }

        if (totalFee > DAILY_MAX_FEE) totalFee = DAILY_MAX_FEE;
        return totalFee;
    }

    /// <summary>
    /// Split the DateTimes into segments that are each one hour long.
    /// </summary>
    /// <param name="dates"></param>
    /// <returns></returns>
    public List<List<DateTime>> GetDateTimeSpans(DateTime[] dates)
    {
        List<List<DateTime>> dateTimes = new List<List<DateTime>>();
        for(int i = 0; i < dates.Length; i++)
        {
            // Add the first element
            if(i == 0)
            {
                dateTimes.Add(new List<DateTime>());
                dateTimes[dateTimes.Count - 1].Add(dates[i]);
                continue;
            }

            // Compare the current date time to the first element in the list. Has more than one hour passed? If so, start on the next list.
            if(DateTime.Compare(dates[i],
                dateTimes[dateTimes.Count - 1][0].AddHours(1)) > 0)
            {
                dateTimes.Add(new List<DateTime>());
                dateTimes[dateTimes.Count - 1].Add(dates[i]);
            }
            else
            {
                dateTimes[dateTimes.Count - 1].Add(dates[i]);
            }
        }

        return dateTimes;
    }

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

    public int GetTollFee(DateTime date, Vehicle vehicle)
    {
        if (IsTollFreeDate(date) || IsTollFreeVehicle(vehicle)) return 0;

        int hour = date.Hour;
        int minute = date.Minute;

        if (hour == 6 && minute >= 0 && minute <= 29) return 8;
        else if (hour == 6 && minute >= 30 && minute <= 59) return 13;
        else if (hour == 7 && minute >= 0 && minute <= 59) return 18;
        else if (hour == 8 && minute >= 0 && minute <= 29) return 13;
        else if (hour == 8 && minute >= 30 && minute <= 59) return 8;
        else if (hour >= 9 && hour <= 14) return 8;
        else if (hour == 15 && minute >= 0 && minute <= 29) return 13;
        else if (hour == 15 && minute >= 29 || hour == 16 && minute <= 59) return 18;
        else if (hour == 17 && minute >= 0 && minute <= 59) return 13;
        else if (hour == 18 && minute >= 0 && minute <= 29) return 8;
        else return 0;
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