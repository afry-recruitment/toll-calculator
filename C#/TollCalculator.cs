using System;
using System.Globalization;
using System.Linq;
using TollFeeCalculator;

public class TollCalculator
{

    /**
     * Calculate the total toll fee for one day
     *
     * @param vehicle - the vehicle
     * @param dates   - date and time of all passes on one day
     * @return - the total toll fee for that day
     */

    public int GetTollFee(Vehicle vehicle, DateTime[] dates)
    {
     
        int totalFee=0,minutes = 0;
          
        for(int i=0;i<dates.Length;i++)
        {
          
         int nextFee = GetTollFee(dates[i], vehicle);
        
           
            if (i >= 1)
            {
                minutes = (int)dates[i - 1].Subtract(dates[i]).TotalMinutes;
                
                if (minutes >= -60)
                { 
                   totalFee -= GetMin(GetTollFee(dates[i - 1], vehicle), GetTollFee(dates[i], vehicle));
                }
                
            }
            totalFee += nextFee;
        }
        if (totalFee > 60) totalFee = 60;
        return totalFee;
    }
   
    public static int GetMin(int first, int second)
    {
        return first < second ? first : second;
    }
   
    private bool IsTollFreeVehicle(Vehicle vehicle)
    {
       foreach (string name in Enum.GetNames(typeof(TollFreeVehicles)))
            {
                if (vehicle.GetVehicleType().Equals(name))
                return true;
            }
                return false;
    }

    public int GetTollFee(DateTime date, Vehicle vehicle)
    {
        DateTime daystart = DateTime.Today.AddHours(6);

        if (IsTollFreeDate(date) || IsTollFreeVehicle(vehicle)) return 0;

        int hour = date.Hour;
        int minute = date.Minute;

        return GetInterval(hour, minute);
    }

    private Boolean IsTollFreeDate(DateTime date)
    {
       SweCalendar c = new SweCalendar();

        if (c.isHoliday(date.Date)||date.DayOfWeek == DayOfWeek.Saturday || date.DayOfWeek == DayOfWeek.Sunday)
            return true;
        else
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

    private int GetInterval(int hour, int minute)
    {
        
        if ((hour == 6 && minute >= 0 && minute <= 29) || (hour == 8 && minute >= 30) || (hour >= 9 && hour <= 14) || (hour == 18 && minute >= 0 && minute <= 29))
        {
            return 8;
        }
        else if ((hour == 6 && minute >= 30) || (hour >= 8 && hour <= 14 && minute <= 59) || (hour == 15 && minute >= 0 && minute <= 29) || (hour == 17) || (hour == 18 && minute >= 30))
        {
            return 13;
        }
        else if ((hour == 7) || (hour == 15 && minute >= 30) || (hour >= 16 && hour <= 17 && minute <= 59))
        {
            return 18;
        }
        else
        {
           return 0;
        }
    }

}