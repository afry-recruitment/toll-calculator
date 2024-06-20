namespace TollTracker.Interfaces.Models;

/// <summary>
/// Provides properties for a toll price model.
/// </summary>
public interface ITollPriceModel
{
    /// <summary>
    /// Gets the start time of the toll price period.
    /// </summary>
    TimeOnly StartTime { get; }

    /// <summary>
    /// Gets the end time of the toll price period.
    /// </summary>
    TimeOnly EndTime { get; }

    /// <summary>
    /// Gets the price of the toll for the specified period.
    /// </summary>
    int Price { get; }
}
