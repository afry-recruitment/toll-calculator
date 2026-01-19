using TollFeeCalculator.Enterprise.Interfaces;
using TollFeeCalculator.Enterprise.Policies.Schedules;
using TollFeeCalculator.Enterprise.ValueObjects;

namespace TollFeeCalculator.Application.Providers
{
    public sealed class GothenburgFeeRangeProvider : IFeeRangeProvider
    {
        public IReadOnlyList<FeeRange> GetRange() => GothenburgFeeSchedule.Range;
    }

}
