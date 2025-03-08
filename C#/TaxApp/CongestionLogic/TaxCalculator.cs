

namespace TaxApp;

public class TaxCalculator : ITaxService
{
    private readonly IDbApi _dbApi;
    public TaxCalculator(IDbApi dbApi)
    {
        _dbApi = dbApi;
    }   
    public int GetTax(IVehicle vehicle, List<DateTime> dates, int cityId = -1)
    {
        var totalDayFee = 0;
        var totalFees = 0; //For complete dataaset.
        var enduranceFee = 0;
        var maxSingleRouleFee = 0;
        

        var configuredCityFees = new List<KeyValuePair<DateTime, int>>();
        

        //IF configured to read DB (cityId)
        if (cityId != -1)
        {
            configuredCityFees = _dbApi.GetDBFees(cityId);
        }

        //If toll-free vehicle then do return 0.
        if (vehicle.GetVehicleType() != TollFreeVehicles.None)
        {
            return 0;
        }

        //Group the passages per day and per hour.
        var grpDates = dates.GroupBy(d => new { d.Date, d.Hour }).Select(z => z).ToList();

        var totDiffMins = (grpDates.Last().Last() - grpDates.First().First()).TotalMinutes;

        foreach (var groupDates in grpDates)
        {

            maxSingleRouleFee = 0;
            enduranceFee = 0;
            totalDayFee = 0;

            foreach (var date in groupDates)
            {
                
                //Do not calculate ordinary weekenddays.
                if (date.DayOfWeek == DayOfWeek.Saturday || date.DayOfWeek == DayOfWeek.Sunday)
                    continue;

                //A free month
                if (date.Month == 7)
                    continue;

                // Check if the date is within the DatesWithoutCharge
                if (Helper.DatesWithoutCharge.Any(d => d.Date == date.Date))                
                    continue;

                //If total timedifference for complete dataset is less than 60 minutes. Just return the highest congestion value
                if (totDiffMins < 60)
                {
                    maxSingleRouleFee = GetFeeWithinOneHour(grpDates.First().First(), grpDates.Last().Last(), configuredCityFees);
                    return maxSingleRouleFee;

                } else if ((groupDates.Last() - groupDates.First()).TotalMinutes < 60)
                {
                    maxSingleRouleFee = GetFeeWithinOneHour(groupDates.First(), groupDates.Last(), configuredCityFees);
                }
                else 
                {
                    //Adding total fee.
                    enduranceFee +=  GetFee(date, configuredCityFees);
                }
                
                //Total Amount per endurance exceeded? Always 60
                if ( totalDayFee >= 60)
                {
                    Console.WriteLine($"The date {date:yyyy-MM-dd} has reached max (60 kr) congestion!");  
                    totalDayFee = 60;
                    break;
                }

                totalDayFee += enduranceFee + maxSingleRouleFee;
                totalFees += totalDayFee;
            }
 
        }

        return totalFees;
    }

    public int GetFee(DateTime date, List<KeyValuePair<DateTime, int>> CityFees)
    {
        return CityFees.FirstOrDefault(h => h.Key.Hour >= date.Hour && h.Key.Minute >= date.Minute && h.Key.Second >= date.Second).Value;
    }
    public int GetFeeWithinOneHour(DateTime start, DateTime end, List<KeyValuePair<DateTime, int>> CityFees)
    {
        return Math.Max(GetFee(start, CityFees), GetFee(end, CityFees));  
    }

}
