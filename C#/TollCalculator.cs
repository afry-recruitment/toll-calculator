using System;
using System.Globalization;
using TollFeeCalculator;

public class TollCalculator
{
    private const int MAXIMUM_FEE_FOR_ONE_DAY = 60;

    /**
     * Calculate the total toll fee for one day
     *
     * @param vehicle - the vehicle
     * @param dates   - date and time of all passes on one day
     * @return - the total toll fee for that day
     */
    public int GetTollFee(iVehicle vehicle, DateTime[] dates)
    {
        int totalFee = 0;

        if (IsTollFreeVehicle(vehicle))
            return totalFee;

        if (dates != null && dates.Length > 0)
        {
            dates = dates.OrderBy(x => x).ToArray();

            int hour = dates[0].Hour;
            int hourFee = 0;
            foreach (DateTime date in dates)
            {
                if (totalFee < MAXIMUM_FEE_FOR_ONE_DAY)
                {
                    int Fee = GetTollFee(date);
                    int hr = date.Hour;
                    if (hour == hr)
                    {
                        hourFee = Math.Max(hourFee, Fee);
                    }
                    else
                    {
                        totalFee += hourFee;
                        hour = hr;
                        hourFee = Fee;
                    }
                }
            }
            if (totalFee < MAXIMUM_FEE_FOR_ONE_DAY)
            {
                totalFee += hourFee; // Since the fees for last hour will not be added in the foreach loop when the totalFee < MAXIMUM_FEE_FOR_ONE_DAY and loop has completed
            }
        }
        totalFee = Math.Min(totalFee, MAXIMUM_FEE_FOR_ONE_DAY);

        return totalFee;
    }

    private bool IsTollFreeVehicle(iVehicle vehicle)
    {
        if (vehicle == null) return false;
        return vehicle.FeeFree;
    }

    public class Price
    {
        public TimeOnly StartTime { get; set; }
        public TimeOnly EndTime { get; set; }
        public int Fee { get; set; }
    }

    public int GetTollFee(DateTime datetime)
    {
        DateOnly date = DateOnly.FromDateTime(datetime);
        if (IsTollFreeDate(date))
            return 0;

        TimeOnly time = TimeOnly.FromDateTime(datetime);
        List<Price> lstPrice = GetPrices();
        if (lstPrice != null && lstPrice.Count > 0 && lstPrice.Any(x => x.StartTime <= time && x.EndTime >= time))
            return lstPrice.Where(x => x.StartTime <= time && x.EndTime >= time).Select(x => x.Fee).FirstOrDefault();

        return 0;
    }

    private Boolean IsTollFreeDate(DateOnly date)
    {
        if (date.DayOfWeek == DayOfWeek.Saturday || date.DayOfWeek == DayOfWeek.Sunday)
            return true;

        List<DateOnly> lstHolidays = GetHolidaysForTheYear(date.Year);
        if (lstHolidays != null && lstHolidays.Count > 0 && lstHolidays.Any(x => x == date))
            return true;

        return false;
    }

    private List<DateOnly> GetHolidaysForTheYear(int year)
    {
        // Retrieve the list of hoilidays for the year from the db
        throw new NotImplementedException();
    }

    private List<Price> GetPrices()
    {
        // Retrieve the Prices from the db
        throw new NotImplementedException();
    }
}