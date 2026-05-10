public static class TollIntervalHelper 
{
    // Inte det bästa namnet, kanske TollIntervalRule eller liknande
    public static readonly List<TollInterval> TollIntervals =
    [
        new(new TimeSpan(6, 0, 0),  new TimeSpan(6, 30, 0), 8),
        new(new TimeSpan(6, 30, 0), new TimeSpan(7, 0, 0), 13),
        new(new TimeSpan(7, 0, 0),  new TimeSpan(8, 0, 0), 18),
        new(new TimeSpan(8, 0, 0),  new TimeSpan(8, 30, 0), 13),
        new(new TimeSpan(8, 30, 0), new TimeSpan(15, 0, 0), 8),
        new(new TimeSpan(15, 0, 0), new TimeSpan(15, 30, 0), 13),
        new(new TimeSpan(15, 30, 0), new TimeSpan(17, 0, 0), 18),
        new(new TimeSpan(17, 0, 0), new TimeSpan(18, 0, 0), 13),
        new(new TimeSpan(18, 0, 0), new TimeSpan(18, 30, 0), 8),
    ];
}
