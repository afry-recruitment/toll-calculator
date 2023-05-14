using System;
using System.Globalization;
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
        DateTime intervalStart = dates[0];      // Init: [hour window]
        int totalFee = 0;

        if (IsTollFreeDate(dates[0]) || IsTollFreeVehicle(vehicle))  // remark: if tollfree vehical or tollfree date we could return 0 kr imediately
            return 0;   // fee = 0 kr

        int maxFee = 0; // new
        foreach (DateTime date in dates)
        {
            int nextFee = GetTollFee(date, vehicle);        
//            int tempFee = GetTollFee(intervalStart, vehicle);  

//            long diffInMillies = date.Millisecond - intervalStart.Millisecond;  // Remark: Millisecond is a value 0-999 
//            long minutes = diffInMillies/1000/60; // Remark: ? diffInMillies max 999, will not give you minutes  
//                                                     minutes always < 60 ==> always within same hour! 
            long minutes = (long)date.Subtract(intervalStart).TotalMinutes;  // new: Get diff in minutes

            

            if (minutes <= 60)  // ok within same hour  
            {
                /* Remark: it always compare with the first item in period (potential problem if > 2 in the same period */
                /* highest fee for period not correct */
                /* example: 06:10 (8kr), 06:40 (13 kr), 07:05 (18 kr) should give 18 but gave 23 (totalfee=13 - 8 + 18)
                /* orignal
                if (totalFee > 0)
                    totalFee -= tempFee;
                if (nextFee >= tempFee)
                    tempFee = nextFee;
                totalFee += tempFee;
                */

                // New
                if (totalFee > 0)
                    totalFee -= maxFee;
                if (nextFee > maxFee)
                    maxFee = nextFee;
                totalFee += maxFee;
            }
            else
            {
                maxFee = nextFee;
                totalFee += nextFee;
                intervalStart = date;   // New: Set new [hour-window] time assuming time ordered oldest time first (ascending)
            }
        }
        if (totalFee > 60) totalFee = 60;
        return totalFee;
    }


    public bool IsTollFreeVehicle(Vehicle vehicle)
    {
        if (vehicle == null)
            return false;
        String vehicleType = vehicle.GetVehicleType();
        return vehicleType.Equals(TollFreeVehicles.Motorbike.ToString()) ||
               vehicleType.Equals(TollFreeVehicles.Tractor.ToString()) ||
               vehicleType.Equals(TollFreeVehicles.Emergency.ToString()) ||
               vehicleType.Equals(TollFreeVehicles.Diplomat.ToString()) ||
               vehicleType.Equals(TollFreeVehicles.Foreign.ToString()) ||
               vehicleType.Equals(TollFreeVehicles.Military.ToString());
    }

    private int GetTollFee(DateTime date, Vehicle vehicle)
    {
//        if (IsTollFreeDate(date) || IsTollFreeVehicle(vehicle)) return 0;     // Remark: Not needed here, checked erlier

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

    private Boolean IsTollFreeDate(DateTime date)
    {
        int year = date.Year;
        int month = date.Month;
        int day = date.Day;

        if (date.DayOfWeek == DayOfWeek.Saturday || date.DayOfWeek == DayOfWeek.Sunday) return true;

//        if (year == 2013)   // Remark: Wrong year, update holidays for 2023
        if (year == 2023)   
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

    public enum TollFreeVehicles
    {
        Motorbike = 0,
        Tractor = 1,
        Emergency = 2,
        Diplomat = 3,
        Foreign = 4,
        Military = 5
    }

}