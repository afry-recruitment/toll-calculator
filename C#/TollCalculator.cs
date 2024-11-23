using TollFeeCalculator.Vehicles;
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
        DateTime intervalStart = dates[0];
        int totalFee = 0;
        foreach (DateTime date in dates)
        {
            int nextFee = GetTollFee(date, vehicle);
            int tempFee = GetTollFee(intervalStart, vehicle);

            var diffInMillies = (date - intervalStart).TotalMilliseconds;
            var minutes = diffInMillies/1000/60;

            if (minutes <= 60) // Should probably not be inclusive
            {
                if (totalFee > 0) totalFee -= tempFee;
                if (nextFee >= tempFee) tempFee = nextFee;
                totalFee += tempFee;
            }
            else
            {
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

        return hour switch
        {
            6 when minute >= 0 && minute <= 29 => 8,
            6 when minute >= 30 && minute <= 59 => 13,
            7 => 18,
            8 when minute >= 0 && minute <= 29 => 13,
            >= 8 and <= 14 when minute >= 30 && minute <= 59 => 8, // This looks strange, should check with the requirements
            15 when minute >= 0 && minute <= 29 => 13,
            15 => 18,
            16 => 18,
            17 => 13,
            18 when minute >= 0 && minute <= 29 => 8,
            _ => 0,
        };
    }

    private Boolean IsTollFreeDate(DateTime date)
    {
        int year = date.Year;
        int month = date.Month;
        int day = date.Day;

        if (date.DayOfWeek == DayOfWeek.Saturday || date.DayOfWeek == DayOfWeek.Sunday) return true;

        // The year check should be removed since holidays occur every year
        // For some holidays the date may also change depending on the year
        if (year == 2013)
        {
            switch (month)
            {
                case 1 when day == 1:
                case 3 when day == 28 || day == 29:
                case 4 when day == 1 || day == 30:
                case 5 when day == 1 || day == 8 || day == 9:
                case 6 when day == 5 || day == 6 || day == 21:
                case 7:
                case 11 when day == 1:
                case 12 when day == 24 || day == 25 || day == 26 || day == 31:
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