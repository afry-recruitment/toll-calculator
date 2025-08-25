using System;
using System.Globalization;
using System.Runtime.InteropServices.JavaScript;
using TollFeeCalculator;

public class TollCalculator
{

     /*
     * Calculate the total toll fee for one day
     *
     * @param vehicle - the vehicle
     * @param dates   - date and time of all passes on one day
     * @return - the total toll fee for that day
     */

     private readonly int dailyCap = 60; //Daily toll cap
     private readonly int[] tolls = new []{8,13,18}; // Toll amounts low, medium high
     
     
     public int GetTollFee(IVehicle vehicle, DateTime[]? dates)
     {
         //Handle edge cases
        if (dates is null || dates.Length == 0) return 0;
        if (vehicle is null)
            throw new ArgumentException(nameof(vehicle), "Vehicle is null");
        //Sort to chronological order for calculations
        Array.Sort(dates);
        
        int totalFee = 0;
        int currentDayFee = 0;
        DateTime day = dates[0].Date;
        
        //Track 60 min interval
        DateTime intervalStart = dates[0];
        int intervalMax = GetTollFee(dates[0], vehicle);

        for (int i = 1; i < dates.Length; i++)
        {
            DateTime currentPass = dates[i];
            
            // New day? finish previous day.
            if (currentPass.Date != day.Date)
            {
                currentDayFee += intervalMax;
                totalFee += Math.Min(currentDayFee, dailyCap);
                
                //Reset for a new day
                day = currentPass.Date;
                currentDayFee = 0;
                intervalStart = currentPass;
                intervalMax = GetTollFee(currentPass, vehicle);
                continue;
            }
            
            int fee = GetTollFee(currentPass, vehicle);
            
            //Only charge highest fee in 60 min interval
            if ((currentPass - intervalStart).TotalMinutes < 60)
            {
                if(fee > intervalMax) intervalMax = fee;
            }
            else
            {
                // New interval
                currentDayFee += intervalMax;
                intervalStart = currentPass;
                intervalMax = fee;
            }
        }
        
        //Add final interval and apply daily cap
        currentDayFee += intervalMax;
        totalFee += Math.Min(currentDayFee, dailyCap);
        
        return totalFee;
     }
    
    

    public int GetTollFee(DateTime date, IVehicle vehicle)
    {
        //No fee on weekends or holidays
        if (IsTollFreeDate(date) || vehicle.IsTollFree()) return 0;

        int hour = date.Hour;
        int minute = date.Minute;
        
        //Fixed logic
        if (hour == 6 && minute <= 29) return tolls[0];
        else if (hour == 6 && minute >= 30) return tolls[1];
        else if (hour == 7) return tolls[2];
        else if (hour == 8 && minute <= 29) return tolls[1];
        else if (hour == 8 && minute >= 30) return tolls[0];
        else if (hour >= 9 && hour <= 14) return tolls[0]; 
        else if (hour == 15 && minute <= 29) return tolls[1];
        else if (hour == 15 && minute >= 30) return tolls[2];
        else if (hour == 16) return tolls[2];
        else if (hour == 17) return tolls[1];
        else if (hour == 18 && minute <= 29) return tolls[0];
        else return 0;
    }
    //Removed unnecessary date variables and cleaned up.
    private Boolean IsTollFreeDate(DateTime date)
    {
        return date.DayOfWeek == DayOfWeek.Saturday || date.DayOfWeek == DayOfWeek.Sunday || IsHoliday();
    }
    
    Boolean IsHoliday()
    {
        //Implement for relevant holidays
        return false;
    }
    
}