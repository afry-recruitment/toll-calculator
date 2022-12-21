namespace TrafficToll.Internals.Models;

internal sealed class TollTimePeriod
{
    public TollTimePeriod(TimeSpan start, TimeSpan end, int tollTrafficType)
    {
        Start = start;
        End = end;
        TollTrafficType = tollTrafficType;
    }

    public TimeSpan Start { get; }
    public TimeSpan End { get; }
    public int TollTrafficType { get; }
}
