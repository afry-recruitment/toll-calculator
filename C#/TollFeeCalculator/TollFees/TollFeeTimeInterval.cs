using System;

namespace TollFeeCalculator.TollFees
{
    public class TollFeeTimeInterval
    {
        public TimeOnly From { get; }

        public TimeOnly Until { get; }

        public int TollFee { get; }

        public TollFeeTimeInterval(TimeOnly from, TimeOnly until, int tollFee)
        {
            From = from;
            Until = until;
            TollFee = tollFee;
        }

        public bool IsInInterval(DateTime time) => IsInInterval(TimeOnly.FromDateTime(time));

        public bool IsInInterval(TimeOnly time)
        {
            return From <= time && time < Until;
        }

    }
}
