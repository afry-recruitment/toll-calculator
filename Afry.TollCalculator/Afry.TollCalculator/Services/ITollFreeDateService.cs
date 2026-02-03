using Afry.TollCalculator.Infrastructure;

namespace Afry.TollCalculator.Services;

public interface ITollFreeDateService
{
    /// <summary>
    /// Determines whether the given timestamp falls on a toll-free local date.
    /// </summary>
    /// <param name="date">The timestamp to evaluate.</param>
    /// <returns>A <see cref="Result{T}"/> containing <c>true</c> if toll-free; otherwise <c>false</c>.</returns>
    Result<bool> IsTollFreeDate(DateTimeOffset date);
}
