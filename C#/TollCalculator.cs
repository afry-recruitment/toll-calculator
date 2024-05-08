using System;
using System.Globalization;
using TollFeeCalculator;

public class TollCalculator
{
    /**
        I made a separate file, Program.cs, to put my own functions in because I didn't want to break 
        anything before I had a good grasp on what was going on everywhere.
        Then I ran "dotnet run" to simulate a car or a motorbike driving through the tolls on a random 
        created day in 2013. 

        Since there is only chargeable vehicle and vehicles free of charge I didn't bother creating more 
        vehicle options. There is only car and motorbike to demonstrate the difference between chargeable 
        and free of charge vehicles.

        The given function GetTollFee(Vehicle vehicle, DateTime[] dates) couldn't handle the array 
        of times my function CreateTripsAndFees() created so I built my own logic for checking each passage
        and storing the fees accurately.

        I left a lot of the Console.WriteLine on purpose to make the process easier to read and understand.

        The program could benifit from better code commenting throughout. But I do hope that in this case 
        it will be enough in combinations with the many Console.WriteLine.

        Unfortunatly I haven't seen the movie Hackers, yet... ;) I did, however, see The Net with Sandra 
        Bullock a long time ago! It was also released in 1995, actually. =)
    **/

    /**
     * Calculate the total toll fee for one day
     *
     * @param vehicle - the vehicle
     * @param dates   - date and time of all passes on one day
     * @return - the total toll fee for that day
     */

    // public static int GetTollFee(Vehicle vehicle, DateTime[] dates)
    // {
    //     DateTime intervalStart = dates[0];
    //     int totalFee = 0;
    //     foreach (DateTime date in dates)
    //     {
    //         int nextFee = GetTollFee(date, vehicle);
    //         int tempFee = GetTollFee(intervalStart, vehicle);

    //         long diffInMillies = date.Millisecond - intervalStart.Millisecond;
    //         long minutes = diffInMillies/1000/60;

    //         if (minutes <= 60)
    //         {
    //             if (totalFee > 0) totalFee -= tempFee;
    //             if (nextFee >= tempFee) tempFee = nextFee;
    //             totalFee += tempFee;
    //         }
    //         else
    //         {
    //             totalFee += nextFee;
    //         }
    //     }
    //     if (totalFee > 60) totalFee = 60;
    //     return totalFee;
    // }

    public static bool IsTollFreeVehicle(Vehicle vehicle)
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

    // public static int GetTollFee(DateTime date, Vehicle vehicle)
    public static int GetTollFee(DateTime date)
    {
        //if (IsTollFreeDate(date) || IsTollFreeVehicle(vehicle)) return 0;

        int hour = date.Hour;
        int minute = date.Minute;

        // 6:00 - 6:29
        if (hour == 6 && minute >= 0 && minute <= 29) return 8;
        // 6:30 - 6:59
        else if (hour == 6 && minute >= 30 && minute <= 59) return 13;
        // 7:00 - 7:59
        else if (hour == 7 && minute >= 0 && minute <= 59) return 18;
        // 8:00 - 8:29
        else if (hour == 8 && minute >= 0 && minute <= 29) return 13;
        // 8:30 - 14:59
        // else if (hour >= 8 && hour <= 14 && minute >= 30 && minute <= 59) return 8; wrong? example 10:15 returned 0
        else if (hour == 8 && minute >= 30) return 8;
        else if (hour >= 9 && hour <= 14) return 8;
        // 15:00 - 15:29
        else if (hour == 15 && minute >= 0 && minute <= 29) return 13;
        // 15:30 - 16:59
        //else if (hour == 15 && minute >= 0 || hour == 16 && minute <= 59) return 18; wrong? 15:00 second time.
        else if (hour == 15 && minute >= 30 || hour == 16 && minute <= 59) return 18;
        // 17:00 - 17:59
        else if (hour == 17 && minute >= 0 && minute <= 59) return 13;
        // 18:00 - 18:29
        else if (hour == 18 && minute >= 0 && minute <= 29) return 8;
        else return 0;
    }

    public static Boolean IsTollFreeDate(DateTime date)
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