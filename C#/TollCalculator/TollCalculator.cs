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

    public int GetTollFee(Vehicle vehicle, List<DateTime> dates)
    {
        if(dates==null) throw new ArgumentNullException("Dates Can't be null");
        if(dates.Count==0) return 0;
        if(dates.Count==1) return GetTollFee(dates[0], vehicle);
        
        
        var totalFee = 0;
       foreach(var datesGroupedByDay in dates.GroupBy(x=>x.Day)){
            foreach (var datesWithInHour in datesGroupedByDay.GroupBy(x => x.Hour))
                    {
                        var tmpFee = 0;
                        foreach (var date in datesWithInHour)
                        {
                            if (GetTollFee(date, vehicle) > tmpFee)
                            {
                                tmpFee = GetTollFee(date, vehicle);
                            }
                        }
                        totalFee += tmpFee;
                    }
       }
        

        if (totalFee > 60) totalFee = 60;
    
        return totalFee;
    }

    public bool IsTollFreeVehicle(Vehicle vehicle)
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
        foreach(var tollFee in TollFeeData.TollFees){
            if(tollFee.StartTime.ToTimeSpan() <= date.TimeOfDay &&
                                    tollFee.EndTime.ToTimeSpan() >= date.TimeOfDay)
                                    return tollFee.Fee;
        }
        return 0;
    }

    public Boolean IsTollFreeDate(DateTime date)
    {
        int year = date.Year;
        int month = date.Month;
        int day = date.Day;

        if (date.DayOfWeek == DayOfWeek.Saturday || date.DayOfWeek == DayOfWeek.Sunday) return true;
        if(month==7) return true;
        foreach(var tollFreeDate in TollFeeData.TollFreeDates){
            if(month==tollFreeDate.Month&&tollFreeDate.Day==day)
            return true;
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