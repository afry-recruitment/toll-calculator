namespace TollFeeCalculator.Models;

public sealed record TollFeeInterval(TimeOnly Start, TimeOnly End, int Fee)
{
    public TimeOnly Start { get; init; } = Start >= End 
        ? throw new ArgumentException("Start must be before End.") 
        : Start;
    
    public int Fee { get; init; } = Fee < 0 
        ? throw new ArgumentOutOfRangeException(nameof(Fee)) 
        : Fee;
}