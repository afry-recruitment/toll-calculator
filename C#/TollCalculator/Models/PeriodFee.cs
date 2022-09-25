namespace TollCalculator.Models;
public class PeriodFee
{
    public int Fee { get; set; }
    public TimeOnly StartTime { get; set; }
    public TimeOnly EndTime { get; set; }
}