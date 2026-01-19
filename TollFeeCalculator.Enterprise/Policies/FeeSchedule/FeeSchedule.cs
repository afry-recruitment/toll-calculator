using TollFeeCalculator.Enterprise.Interfaces;
using TollFeeCalculator.Enterprise.ValueObjects;

namespace TollFeeCalculator.Enterprise.Policies
{
    public sealed class FeeSchedule :IFeeSchedule
    {
        private readonly IReadOnlyList<FeeRange> _range;

        public FeeSchedule(IFeeRangeProvider provider)
        {
            _range = provider.GetRange();
        }

        public int GetFee(TimeOnly time)
        {
            foreach (var r in _range)
            {
                if (time >= r.From && time < r.To) return r.Fee;
            }

            return 0;
        }
    }
}
