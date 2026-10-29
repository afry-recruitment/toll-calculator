namespace TollFeeCalculator;

/// <summary>Fee per time of day. Each interval applies from its start until the next interval starts.</summary>
public static class TollFeeSchedule
{
    private static readonly (TimeOnly From, int Fee)[] Intervals =
    [
        (new(0, 0), 0),
        (new(6, 0), 8),
        (new(6, 30), 13),
        (new(7, 0), 18),
        (new(8, 0), 13),
        (new(8, 30), 8),
        (new(15, 0), 13),
        (new(15, 30), 18),
        (new(17, 0), 13),
        (new(18, 0), 8),
        (new(18, 30), 0),
    ];

    public static int GetFee(TimeOnly time) => Intervals.Last(i => i.From <= time).Fee;
}
