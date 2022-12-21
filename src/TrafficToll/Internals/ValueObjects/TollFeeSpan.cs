namespace TrafficToll.Internals.ValueObjects
{
    internal sealed record TollFeeSpan
    {
        public TollFeeSpan(TimeSpan start, TimeSpan end, int tollPrice)
        {
            Start = start;
            End = end;
            TollPrice = tollPrice;
        }

        public TimeSpan Start { get; }
        public TimeSpan End { get; }
        public int TollPrice { get; }
    }
}
