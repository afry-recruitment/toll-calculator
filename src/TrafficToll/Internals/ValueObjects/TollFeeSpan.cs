namespace TrafficToll.Internals.ValueObjects
{
    internal sealed record TollFeeSpan
    {
        public TollFeeSpan(TimeSpan start, TimeSpan end, int tollTrafficPrize)
        {
            Start = start;
            End = end;
            TollTrafficPrize = tollTrafficPrize;
        }

        public TimeSpan Start { get; }
        public TimeSpan End { get; }
        public int TollTrafficPrize { get; }
    }
}
