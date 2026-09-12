using System;
using System.Globalization;
using TollFeeCalculator;

public class TollCalculator
{

    /**
     * Calculate the total toll fee for one day
     *
     * Passages within 60 minutes of each other are grouped into the same
     * charging window. Only the highest single fee within each window is
     * counted; separate windows (more than 60 minutes apart) are summed.
     *
     * @param vehicle - the vehicle
     * @param dates   - date and time of all passes on one day
     * @return - the total toll fee for that day
     */

    public int GetTollFee(Vehicle vehicle, DateTime[] dates)
    {
        DateTime intervalStart = dates[0];
        int totalFee = 0;
        int currentWindowFee = 0; // fee currently counted toward totalFee for the active window

        foreach (DateTime date in dates)
        {
            int nextFee = GetTollFee(date, vehicle);

            TimeSpan diff = date - intervalStart;

            double diffInMinutes = diff.TotalMinutes;

            if (diffInMinutes <= 60)
            {
                // A higher fee appeared in this window: replace what was previously
                // counted with the new, higher fee.
                if (nextFee > currentWindowFee)
                {
                    totalFee -= currentWindowFee;
                    totalFee += nextFee;
                    currentWindowFee = nextFee;
                }
            }
            else
            {
                // More than 60 minutes since the window started: begin a new window.
                intervalStart = date;
                currentWindowFee = nextFee;
                totalFee += nextFee;
            }
        }
        if (totalFee > 60) totalFee = 60;
        return totalFee;
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

        int totalMinutes = hour * 60 + minute;

        if (totalMinutes >= 360 && totalMinutes <= 389) return 8;	        // 06:00 to 06:29
        else if (totalMinutes >= 390 && totalMinutes <= 419) return 13;	    // 06:30 to 06:59
        else if (totalMinutes >= 420 && totalMinutes <= 479) return 18;	    // 07:00 to 07:59
        else if (totalMinutes >= 480 && totalMinutes <= 509) return 13;	    // 08:00 to 08:29
        else if (totalMinutes >= 510 && totalMinutes <= 899) return 8;	    // 08:30 to 14:59
        else if (totalMinutes >= 900 && totalMinutes <= 929) return 13;	    // 15:00 to 15:29
        else if (totalMinutes >= 930 && totalMinutes <= 1019) return 18;	// 15:30 to 16:59
        else if (totalMinutes >= 1020 && totalMinutes <= 1079) return 13;	// 17:00 to 17:59
        else if (totalMinutes >= 1080 && totalMinutes <= 1109) return 8;	// 18:00 to 18:29
        else return 0;                                                      // 18:30 to 23:59 and 00:00 to 05:59 (not charged)
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