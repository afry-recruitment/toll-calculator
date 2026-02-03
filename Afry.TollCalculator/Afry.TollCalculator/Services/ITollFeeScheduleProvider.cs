using Afry.TollCalculator.Infrastructure;
using Afry.TollCalculator.Models;

namespace Afry.TollCalculator.Services;

public interface ITollFeeScheduleProvider
{
    /// <summary>
    /// Gets the configured time-of-day fee schedule.
    /// </summary>
    /// <returns>
    /// A <see cref="Result{T}"/> containing a list of non-overlapping time ranges with associated fees.
    /// </returns>
    Result<IReadOnlyList<TollFeeTimeRange>> GetSchedule();
}
