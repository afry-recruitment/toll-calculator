using TollTracker.Models;

namespace TollTracker.Interfaces.Models;

/// <summary>
/// Provides properties for a toll tracking model.
/// </summary>
public interface ITollTrackingModel
{
    /// <summary>
    /// Gets the vehicle associated with the toll tracking.
    /// </summary>
    VehicleModel Vehicle { get; }

    /// <summary>
    /// Gets the collection of timestamps indicating when the vehicle passed the toll. 
    /// </summary>
    IReadOnlyCollection<DateTime> Timestamps { get; }

    /// <summary>
    /// A helper method that returns a string representation of the toll tracking model.
    /// </summary>
    public abstract string ToString();
}