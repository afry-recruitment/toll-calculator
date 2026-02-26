using TollFeeCalculator.Interfaces;
using TollFeeCalculator.Models;

namespace TollFeeCalculator.Services;

public class TollFeeCalculatorService(
TollFeeCalculatorConfig config,
    IHolidayProvider holidayProvider) 
    : ITollFeeCalculator
{
    public int CalculateFee(IEnumerable<DateTime> timeStamps)
    {
        if (timeStamps == null!) return 0;
    
        var passages = timeStamps.ToList();
        if (passages.Count == 0) return 0;

        return passages
            .GroupBy(dt => dt.Date)
            .Sum(dayGroup => CalculateDailyFee(dayGroup.OrderBy(dt => dt)));
    }
    
    private int CalculateDailyFee(IEnumerable<DateTime> sortedPassages)
    {
        int dailyTotal = 0;
        DateTime? windowStart = null;
        int windowMaxFee = 0;

        foreach (var passage in sortedPassages)
        {
            int fee = GetPassageFee(passage);

            if (windowStart == null ||
                (passage - windowStart.Value).TotalMinutes > config.ChargeIntervalMinutes)
            {
                dailyTotal += windowMaxFee;
                windowStart = passage;
                windowMaxFee = fee;
            }
            else if (fee > windowMaxFee)
            {
                windowMaxFee = fee;
            }
        }

        dailyTotal += windowMaxFee;

        return Math.Min(dailyTotal, config.DailyMaxFee);
    }

    private int GetPassageFee(DateTime passage)
    {
        if (holidayProvider.IsHoliday(passage))
            return 0;

        var time = TimeOnly.FromDateTime(passage);

        var interval = config.FeeSchedule
            .FirstOrDefault(f =>
                time >= f.Start &&
                time < f.End);

        return interval?.Fee ?? 0;
    }
}