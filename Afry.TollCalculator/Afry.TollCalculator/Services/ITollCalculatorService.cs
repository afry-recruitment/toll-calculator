using Afry.TollCalculator.Models;
using Afry.TollCalculator.Infrastructure;

namespace Afry.TollCalculator.Services;

public interface ITollCalculatorService
{
    /// <summary>
    /// Calculates the total toll fee for one local day.
    /// </summary>
    /// <param name="vehicle">The vehicle.</param>
    /// <param name="dates">All pass timestamps for the same local day.</param>
    /// <returns>
    /// A <see cref="Result{T}"/> containing the calculated <see cref="Money"/>.
    /// When the input is invalid, returns a failure with an error message.
    /// </returns>
    Result<Money> GetTollFee(Vehicle vehicle, DateTimeOffset[] dates);

    /// <summary>
    /// Calculates the toll fee for a single pass.
    /// </summary>
    /// <param name="date">The pass timestamp.</param>
    /// <param name="vehicle">The vehicle.</param>
    /// <returns>A <see cref="Result{T}"/> containing the calculated <see cref="Money"/>.</returns>
    Result<Money> GetTollFee(DateTimeOffset date, Vehicle vehicle);
}
