using TollCalculator.Models;

namespace TollCalculator.Controller;
public class FeeCalculator
{
    private const int LowTrafficFee = 8;
    private const int MeduimTrafficFee = 13;
    private const int HighTrafficFee = 18;

    public int GetTollFee(Vehicle vehicle, List<DateTime> dates)
    {
        var totalFee = 0;
        var datesGroupedByHours = dates.GroupBy(x => x.Hour);
        foreach (var groupedDates in datesGroupedByHours)
        {
            var tmpFee = 0;
            foreach (var date in groupedDates)
            {
                if (GetTollFeeByTime(date, vehicle) > tmpFee)
                {
                    tmpFee = GetTollFeeByTime(date, vehicle);
                }
            }
            totalFee += tmpFee;
        }

        if (totalFee > 60) totalFee = 60;

        return totalFee;

    }

    public int GetTollFeeByTime(DateTime date, Vehicle vehicle)
    {
        var periodsWithFee = GetPeriodsListWithFee();
        return periodsWithFee.Where(x => x.StartTime.ToTimeSpan() <= date.TimeOfDay &&
                                    x.EndTime.ToTimeSpan() >= date.TimeOfDay)
                             .Select(x => x.Fee).FirstOrDefault();
    }

    public List<PeriodFee> GetPeriodsListWithFee()
    {
        return new List<PeriodFee>(){
            new PeriodFee { Fee = 8, StartTime  = new TimeOnly(6, 0), EndTime = new TimeOnly(6, 29)},
            new PeriodFee { Fee = 13, StartTime = new TimeOnly(6, 29), EndTime = new TimeOnly(6, 59)},
            new PeriodFee { Fee = 18, StartTime = new TimeOnly(7, 0), EndTime = new TimeOnly(7, 59)},
            new PeriodFee { Fee = 13, StartTime = new TimeOnly(8, 0), EndTime = new TimeOnly(8, 29)},
            new PeriodFee { Fee = 8, StartTime  = new TimeOnly(8, 30), EndTime = new TimeOnly(14, 59)},
            new PeriodFee { Fee = 13, StartTime = new TimeOnly(15, 0), EndTime = new TimeOnly(15, 29)},
            new PeriodFee { Fee = 18, StartTime = new TimeOnly(15, 30), EndTime = new TimeOnly(16, 59)},
            new PeriodFee { Fee = 13, StartTime = new TimeOnly(17, 0), EndTime = new TimeOnly(17, 59)},
            new PeriodFee { Fee = 8, StartTime  = new TimeOnly(18, 0), EndTime = new TimeOnly(18, 30)},
        };
    }

}