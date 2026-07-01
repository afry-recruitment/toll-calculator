namespace TollCalculator.Models;

/// <summary>
/// Represents a time interval and its associated toll fee.
/// </summary>
public record FeeInterval(TimeOnly From, TimeOnly To, int Fee)
{
    public TimeOnly From { get; } = From > To
        ? throw new ArgumentException("From cannot be later than To.")
        : From;

    public int Fee { get; } = Fee < 0
        ? throw new ArgumentException("Fee cannot be negative.")
        : Fee;

    /// <summary>
    /// Returns true if the given time falls within this interval.
    /// </summary>
    public bool Contains(TimeOnly time) => time >= From && time <= To;
}