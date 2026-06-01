using TollCalculator.Holidays;

namespace TollCalculator;

public class TollFeeSchedule(IHolidayProvider holidayProvider)
{
    private static readonly (TimeOnly Start, TimeOnly End, int Fee)[] FeeSchedule =
    [
        (new TimeOnly(6, 0),   new TimeOnly(6, 30),   8),
        (new TimeOnly(6, 30),  new TimeOnly(7, 0),   13),
        (new TimeOnly(7, 0),   new TimeOnly(8, 0),   18),
        (new TimeOnly(8, 0),   new TimeOnly(8, 30),  13),
        (new TimeOnly(8, 30),  new TimeOnly(15, 0),   8),
        (new TimeOnly(15, 0),  new TimeOnly(15, 30), 13),
        (new TimeOnly(15, 30), new TimeOnly(17, 0),  18),
        (new TimeOnly(17, 0),  new TimeOnly(18, 0),  13),
        (new TimeOnly(18, 0),  new TimeOnly(18, 30),  8)
    ];
    
    public int GetTollFee(DateTime date)
    {
        if (IsTollFreeDate(date))
            return 0;
        
        var time = TimeOnly.FromDateTime(date);
        
        foreach (var (start, end, fee) in FeeSchedule)
        {
            if (time >= start && time < end) 
                return fee;
        }
        
        return 0;
    }
    
    private bool IsTollFreeDate(DateTime date)
    {
        if (IsWeekend(date.DayOfWeek)) 
            return true;

        if (holidayProvider.IsHoliday(date))
            return true;
        
        return false;
    }
    
    public static bool IsWeekend(DayOfWeek dayOfWeek) => dayOfWeek is DayOfWeek.Saturday or DayOfWeek.Sunday;
}
