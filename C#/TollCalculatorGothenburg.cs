using PublicHoliday;
using System;
using System.Linq;
using Test_C__app.Afry_toll_calculator_fork.C_;

public class TollCalculatorGothenburg: TollCalculator
{
    /* The list of fee timespans
     * StartTime is the variable for when the fee starts counting
     * Fee is the Fee in SEK
    */
    private FeeSpan[] FeeSpans =
    {
        new FeeSpan(new TimeSpan(00, 00, 00), 00),
        new FeeSpan(new TimeSpan(06, 00, 00), 08),
        new FeeSpan(new TimeSpan(06, 30, 00), 13),
        new FeeSpan(new TimeSpan(07, 00, 00), 18),
        new FeeSpan(new TimeSpan(08, 00, 00), 13),
        new FeeSpan(new TimeSpan(08, 30, 00), 08),
        new FeeSpan(new TimeSpan(15, 00, 00), 13),
        new FeeSpan(new TimeSpan(15, 30, 00), 18),
        new FeeSpan(new TimeSpan(17, 00, 00), 13),
        new FeeSpan(new TimeSpan(18, 00, 00), 08),
        new FeeSpan(new TimeSpan(18, 30, 00), 00)
    };

    /**
     * Calculate the total toll fee for the sent data
     *
     * @param vehicle - the vehicle
     * @param dates   - date and time of all passes for the veichle
     * @return - the total toll fee for that day
     */

    public int GetTollFee(VehicleType vehicle, DateTime[] dates)
    {
        var dayTimeStamps = dates;
        int totalDayFee = 0;
        var datesSeperatedByDay =
            from date in dates
            group date by date.Date into d
            select  d.ToArray() ;
        foreach (var currentDates in datesSeperatedByDay)
        {
            totalDayFee += GetTollFeeForDay(vehicle, currentDates);
        }
        return totalDayFee;
    }

    /**
     * Calculate the total toll fee for one day
     *
     * @param vehicle - the vehicle
     * @param dates   - date and time of all passes on one day
     * @return - the total toll fee for that day
     */
    public int GetTollFeeForDay(VehicleType vehicle, DateTime[] dayTimeStamps)
    {
        if (!IsTimeStampsSameDay(dayTimeStamps))
            throw new Exception();
        DateTime intervalStart = dayTimeStamps[0];
        int totalFee = 0;
        int lastFee = 0;
        foreach (DateTime date in dayTimeStamps)
        {
            int nextFee = GetTollFee(vehicle, date);
            long diffInTicks = date.Ticks - intervalStart.Ticks;
            double minutes = TimeSpan.FromTicks(diffInTicks).TotalMinutes;
            if (minutes <= 60d)
            {
                if (nextFee >= lastFee) lastFee = nextFee;
            } else
            {
                totalFee += lastFee;
                lastFee = nextFee;
                intervalStart = date;
            }
        }
        totalFee += lastFee;
        if (totalFee > 60) totalFee = 60;
        return totalFee;
    }


    /*
     * Calculate the toll fee for the vehicle at a specific timestamp
     * @param vehicle - the vehicle
     * @param dates - date and time of the timestamp
     * @return - the fee for that time stamp
     */
    public int GetTollFee(VehicleType vehicle, DateTime date){
        if (IsTollFreeDate(date) || IsTollFreeVehicle(vehicle)) return 0;
        int timeStamp = (date.Hour * 100) + date.Minute;
        var fee =
            from span in FeeSpans
            where span.StartTime <= date.TimeOfDay
            select span.Fee;
        return fee.Last();

    }

    private Boolean IsTollFreeDate(DateTime date) {
        //Weekends are free
        if (date.DayOfWeek == DayOfWeek.Saturday || date.DayOfWeek == DayOfWeek.Sunday) return true;
        //July is free
        if (date.Month == 7) return true;
        var holidays = new SwedenPublicHoliday().PublicHolidays(date.Year);
        if (holidays.Contains(date.Date)) return true;

        return false;
    }
    private Boolean IsTimeStampsSameDay(DateTime[] stamps ) 
    {
        return stamps.Select(stamp => stamp.Date)
                     .Distinct()
                     .Count() == 1;
    }

    private bool IsTollFreeVehicle(VehicleType vehicle) {
        return TollFreeVehicleTypes.Contains(vehicle);
    }
    public VehicleType[] TollFreeVehicleTypes
    {
        get => new VehicleType[]{
            VehicleType.Motorbike,
            VehicleType.Tractor,
            VehicleType.Emergency,
            VehicleType.Diplomat,
            VehicleType.Foreign,
            VehicleType.Military };
    }
}