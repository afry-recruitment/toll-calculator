using Afry.TollCalculator.Models;

namespace Afry.TollCalculator.Models;

/// <summary>
/// A time-of-day range with an associated fee.
/// </summary>
public sealed class TollFeeTimeRange(TimeOnly from, TimeOnly to, Money fee)
{
    /// <summary>
    /// Inclusive start time.
    /// </summary>
    public TimeOnly From { get; } = from;

    /// <summary>
    /// Inclusive end time.
    /// </summary>
    public TimeOnly To { get; } = to;

    /// <summary>
    /// The fee charged when a pass occurs within this time range.
    /// </summary>
    public Money Fee { get; } = fee;

    /// <summary>
    /// Returns <c>true</c> when <paramref name="time"/> is within the range (inclusive).
    /// </summary>
    public bool Contains(TimeOnly time) => time >= From && time <= To;
}