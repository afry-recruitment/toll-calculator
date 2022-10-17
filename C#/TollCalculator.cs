using System;
using System.Linq;
using System.Globalization;
using System.Runtime.Serialization;
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


    public int GetVehicleTollFee(Vehicle vehicle, DateTime[] dates)
    {
        DateTime intervalStart = dates[0];
        int totalFee = 0;
        for (int i = 0; i < dates.Length; i++)
        {
            DateTime date = dates[i];
            int nextFee = GetTollFee(date, vehicle);
            int tempFee = GetTollFee(intervalStart, vehicle);

            long diffInMillies = date.Millisecond - intervalStart.Millisecond;
            long minutes = diffInMillies/1000/60;
        
                 
            switch (minutes)
            {
                case <= 60:
                    if (totalFee > 0) totalFee -= tempFee;
                    if (nextFee >= tempFee) tempFee = nextFee;
                    totalFee += tempFee;
                    break;
                default:
                    totalFee += nextFee;
                    break;
            }
        }

        if (totalFee > 60) totalFee = 60;
        return totalFee;
    }

     public bool IsTollFreeVehicle(Vehicle vehicle)
    {
       if (vehicle == null) return false;
       if ((vehicle.GetVehicleType() >= 0)&&(vehicle.GetVehicleType() < 6))
            return true;
       else
            return false;
    }

    public int GetTollFee(DateTime date, Vehicle vehicle)
    {
        if (IsTollFreeDate(date) || IsTollFreeVehicle(vehicle)) return 0;

        int hour = (date.Hour)*10;
        int minute = date.Minute;
        int pointer, sum=0;
        List<int> list1 = new List<int>(){60,81,91,101,111,121,131,141,180};
        List<int> list2 = new List<int>(){61,80,150,170,171};
        List<int> list3 = new List<int>(){70,71,151,160,161};

        if ((int)(minute/30)==0)
            pointer=0;
        else
            pointer=1;
           
        sum+=hour+pointer;
        
        if(list1.Contains(sum))
            return 8;
        else if(list2.Contains(sum))
            return 13;
        else if(list3.Contains(sum))
            return 18;
        else 
            return 0;

    }

    private Boolean IsTollFreeDate(DateTime date)
    {
       if (date.DayOfWeek == DayOfWeek.Saturday || date.DayOfWeek == DayOfWeek.Sunday) 
        return true;
      // Please refer https://www.nuget.org/packages/Nager.Date
       if (DateSystem.IsPublicHoliday(date, CountryCode.SE))
        return true;
       return false; 
    }

    }   
   