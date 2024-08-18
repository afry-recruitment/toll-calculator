using System;

namespace TollFeeCalculator
{
    public class TimeInterval
    {
        public TimeSpan Start { get; }
        public TimeSpan End { get; }
        public int Fee { get; }

        public TimeInterval(TimeSpan start, TimeSpan end, int fee)
        {
            Start = start;
            End = end;
            Fee = fee;
        }

        public bool IsWithinInterval(TimeSpan time)
        {
            return time >= Start && time <= End;
        }
    }
}