using TollFeeCalculator.Enterprise.ValueObjects;

namespace TollFeeCalculator.Enterprise.Interfaces
{
    public interface IFeeRangeProvider
    {
        IReadOnlyList<FeeRange> GetRange();
    }
}
