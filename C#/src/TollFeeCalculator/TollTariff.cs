namespace TollFeeCalculator;

/// <summary>Time-of-day prices in whole SEK, before vehicle or date exemptions.</summary>
internal static class TollTariff
{
    private readonly record struct FeeBand(TimeOnly Start, TimeOnly End, int Fee);

    // Start is inclusive and end is exclusive. This also covers fractional seconds.
    private static readonly FeeBand[] Bands =
    {
        new(new(6, 0),   new(6, 30),   8),
        new(new(6, 30),  new(7, 0),   13),
        new(new(7, 0),   new(8, 0),   18),
        new(new(8, 0),   new(8, 30),  13),
        new(new(8, 30),  new(15, 0),   8),
        new(new(15, 0),  new(15, 30), 13),
        new(new(15, 30), new(17, 0),  18),
        new(new(17, 0),  new(18, 0),  13),
        new(new(18, 0),  new(18, 30),  8)
    };

    public static int GetFee(TimeOnly time)
    {
        foreach (FeeBand band in Bands)
        {
            if (time >= band.Start && time < band.End)
                return band.Fee;
        }

        return 0; // Outside the listed charging hours.
    }
}
