namespace TollFeeCalculator;

internal static class TollTariff
{
    // Each upper boundary is exclusive. The preceding checks cover earlier times.
    internal static int GetFee(TimeOnly time)
    {
        if (time < new TimeOnly(6, 0)) return 0;
        if (time < new TimeOnly(6, 30)) return 8;
        if (time < new TimeOnly(7, 0)) return 13;
        if (time < new TimeOnly(8, 0)) return 18;
        if (time < new TimeOnly(8, 30)) return 13;
        if (time < new TimeOnly(15, 0)) return 8;
        if (time < new TimeOnly(15, 30)) return 13;
        if (time < new TimeOnly(17, 0)) return 18;
        if (time < new TimeOnly(18, 0)) return 13;
        if (time < new TimeOnly(18, 30)) return 8;
        return 0;
    }
}
