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

    // return toll cost for 1 pass
    public int GetTollFee(DateTime tollPass, Vehicle vehicle)
    {
        if (IsTollFreeDate(tollPass) || IsTollFreeVehicle(vehicle)) return 0;

        int hour = tollPass.Hour;
        int minute = tollPass.Minute;

        if (hour == 6 && minute >= 0 && minute <= 29) return 8;
        else if (hour == 6 && minute >= 30 && minute <= 59) return 13;
        else if (hour == 7 && minute >= 0 && minute <= 59) return 18;
        else if (hour == 8 && minute >= 0 && minute <= 29) return 13;
        else if (hour >= 8 && minute >= 30 && minute <= 59) return 8;
        else if (hour == 15 && minute >= 0 && minute <= 29) return 13;
        else if (hour == 15 && minute >= 0 || hour == 16 && minute <= 59) return 18;
        else if (hour == 17 && minute >= 0 && minute <= 59) return 13;
        else if (hour == 18 && minute >= 0 && minute <= 29) return 8;
        else return 0;
    }

    // return toll cost for 1 day
    public int GetTollFee(Vehicle vehicle, List<DateTime> tollPasses)
    {
        int totalFee = 0;

        //get list of tollpasses made today
        DateTime today = DateTime.Today;
        List<DateTime> todaysPasses = tollPasses.Where(p => p.Date == today).ToList();

        while (todaysPasses.Count() > 0)
        {
            //get first tollpass
            DateTime firstPass = todaysPasses.FirstOrDefault();
            DateTime maxFreeTime = firstPass.AddHours(1);

            //get list of passes inside of current hour
            List<DateTime> currentHour = todaysPasses.Where(p => 0 >= p.CompareTo(maxFreeTime)).ToList();

            //get highest fee and add to totalFee
            int highestFee = 0;
            foreach (var pass in currentHour)
            {
                int tempFee = GetTollFee(pass, vehicle);
                if (tempFee > highestFee)
                {
                    highestFee = tempFee;
                }
            }

            totalFee += highestFee;
            highestFee = 0;

            //get list of passes outside of current hour
            List<DateTime> passes = todaysPasses.Where(p => 0 < p.CompareTo(maxFreeTime)).ToList();

            //loops through list of passes untill list is empty
            todaysPasses = passes;
        }

        // returns 60 if fee is over max limit
        if (totalFee > 60) totalFee = 60;
        return totalFee;
    }

    // returns true if vehicle-type is free
    private bool IsTollFreeVehicle(Vehicle vehicle)
    {
        if (vehicle == null) return false;
        String vehicleType = vehicle.GetVehicleType();
        return vehicleType.Equals(TollVehicles.Motorbike.ToString()) ||
               vehicleType.Equals(TollVehicles.Tractor.ToString()) ||
               vehicleType.Equals(TollVehicles.Emergency.ToString()) ||
               vehicleType.Equals(TollVehicles.Diplomat.ToString()) ||
               vehicleType.Equals(TollVehicles.Foreign.ToString()) ||
               vehicleType.Equals(TollVehicles.Military.ToString());
    }

    //returns true if toll is free for the current date
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

    //the vehicle-types that are free
    private enum TollVehicles
    {
        //free
        Motorbike = 0,
        Tractor = 1,
        Emergency = 2,
        Diplomat = 3,
        Foreign = 4,
        Military = 5,
        //not free
        Car = 6
    }
}