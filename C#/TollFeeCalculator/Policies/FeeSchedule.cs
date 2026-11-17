namespace TollFeeCalculator.Policies;

public static class FeeSchedule
{
    // Each entry applies from its start time until the next entry starts.
    private static readonly (TimeOnly Start, int Fee)[] Entries =
    [
        (new TimeOnly(6, 0), 8),
        (new TimeOnly(6, 30), 13),
        (new TimeOnly(7, 0), 18),
        (new TimeOnly(8, 0), 13),
        (new TimeOnly(8, 30), 8),
        (new TimeOnly(15, 0), 13),
        (new TimeOnly(15, 30), 18),
        (new TimeOnly(17, 0), 13),
        (new TimeOnly(18, 0), 8),
        (new TimeOnly(18, 30), 0),
    ];

    public static int FeeAt(TimeOnly time)
    {
        // Before the first entry no entry matches, and the default entry has fee 0.
        return Entries.LastOrDefault(entry => entry.Start <= time).Fee;
    }
}