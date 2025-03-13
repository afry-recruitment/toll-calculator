namespace TollFeeCalculator.Models;

public record TollFeeRate(TimeOnly StartTime, TimeOnly EndTime, decimal Fee)
{
    public bool IsWithinTimeInterval(TimeOnly passageTime)
    {
        if (StartTime <= EndTime)
            return passageTime >= StartTime && passageTime <= EndTime;

        return passageTime >= StartTime || passageTime <= EndTime;
    }
}