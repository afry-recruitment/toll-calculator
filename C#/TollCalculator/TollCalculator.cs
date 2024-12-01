using Toll_Calculator.Vehicles;


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
        dates = dates.Distinct().ToArray();
        Array.Sort(dates);
        var groupedByDay = dates
                        .GroupBy(date => date.Date)
                        .OrderBy(group => group.Key);

        int highestFeeThisHour = 0;
        int totalFee = 0;
        int totalFeeThisDay = 0;

        foreach (var dayGrouped in groupedByDay)
        {
            var timeToCompareWithHour = dayGrouped.First();
            totalFeeThisDay = 0;
            highestFeeThisHour = 0;
            foreach (var date in dayGrouped)
            {
                var feeForCurrentDate = GetTollFee(date, vehicle);
                if (feeForCurrentDate > 0)
                {
                    TimeSpan differenceHour = date - timeToCompareWithHour;
                    if (differenceHour.TotalMinutes < 60)
                    {
                        if (feeForCurrentDate > highestFeeThisHour)
                        {
                            totalFeeThisDay -= highestFeeThisHour;
                            totalFeeThisDay += feeForCurrentDate;
                            highestFeeThisHour = feeForCurrentDate;
                        }
                    }
                    else
                    {
                        totalFeeThisDay += feeForCurrentDate;
                        highestFeeThisHour = feeForCurrentDate;
                        timeToCompareWithHour = date;
                    }
                }
                else
                {
                    timeToCompareWithHour = date;
                }
            }

            if (totalFeeThisDay > 60) totalFeeThisDay = 60;
            totalFee += totalFeeThisDay;
        }
        return totalFee;
    }

    private bool IsTollFreeVehicle(Vehicle vehicle)
    {
        if (vehicle == null) return false;
        String vehicleType = vehicle.GetVehicleType();
        return Enum.GetNames(typeof(TollFreeVehicles)).Contains(vehicleType);
    }

    public int GetTollFee(DateTime date, Vehicle vehicle)
    {
        if (IsTollFreeVehicle(vehicle) || IsTollFreeDate(date)) return 0;

        int hour = date.Hour;
        int minute = date.Minute;

        if (hour == 6 && minute >= 0 && minute <= 29) return 8;
        else if (hour == 6 && minute >= 30 && minute <= 59) return 13;
        else if (hour == 7 && minute >= 0 && minute <= 59) return 18;
        else if (hour == 8 && minute >= 0 && minute <= 29) return 13;
        else if ((hour == 8 && minute >= 30 && minute <= 59) || (hour >= 8 && hour <= 14 && minute >= 0 && minute <= 59)) return 8;
        else if (hour == 15 && minute >= 0 && minute <= 29) return 13;
        else if ((hour == 15 && minute >= 30 && minute <= 59) || (hour == 16 && minute >= 0 && minute <= 59)) return 18;
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

        switch (date.Month)
        {
            case 1 when date.Day == 1:                                                          //new year
            case 1 when date.Day == 6:                                                          //13-day after christmas
            case 3 when date.Day == 28 || date.Day == 29:                                       //easter(ish)
            case 4 when date.Day == 1:                                                          //easter(ish)
            case 5 when date.Day == 9:                                                          //kristi himmelfard
            case 5 when date.Day == 1:                                                          // first of may
            case 6 when date.Day == 6:                                                          // national day
            case 6 when date.Day >= 19 && date.Day <= 25 && date.DayOfWeek == DayOfWeek.Friday: //midsummer
            case 7:                                                                             //summer-month
            case 12 when date.Day == 24 || date.Day == 25 || date.Day == 26 || date.Day == 31:  //christmas
                return true;
            default:
                return false;
        }

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