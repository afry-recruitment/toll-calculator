namespace TollFeeCalculator;
public static class TollFeeCalculatorHelper
{
    /// <summary>
    /// Toll Charges based on timings
    /// </summary>
    public enum TollCharges
    {
        NoCharges,
        MinimumCharge = 8,
        AverageChange = 13,
        MaxCharge = 18
    }

    /// <summary>
    /// Calculate Toll Fee Based On Hour
    /// </summary>
    /// <returns>The toll fee at given hour.</returns>
    /// <param name="date">The date, must be of DateTimeKind Local.</param>
    public static int TollChargesPerPass(int hour, int minute)
    {
        if (IsRushHours(hour, minute))
            return (int)TollCharges.MaxCharge;
        if (IsAverageFeeHours(hour, minute))
            return (int)TollCharges.AverageChange;
        if (IsMinimumFeeHours(hour, minute))
            return (int)TollCharges.MinimumCharge;
        return (int)TollCharges.NoCharges;
    }

    /// <summary>
    /// Rush Hour Timings
    /// </summary>
    /// <returns>If Given Time Is Rush Hour</returns>
    /// <param name="hour">Given Hour</param>
    /// <param name="minute">Given Minute</param>
    public static bool IsRushHours(int hour, int minute)
    {
        return (hour == 7 || (hour == 15 && minute >= 30) || hour == 16);
    }

    /// <summary>
    /// Minimum Fee Timings
    /// </summary>
    /// <returns>If Given Time Is Minimun Fee Hour</returns>
    /// <param name="hour">Given Hour</param>
    /// <param name="minute">Given Minute</param>
    public static bool IsMinimumFeeHours(int hour, int minute)
    {
        return ((hour == 6 && minute <= 29)
        || (hour == 8 && minute >= 30)
        || (hour >= 9 && hour <= 14)
        || (hour == 18 && minute <= 29));
    }

    /// <summary>
    /// Average Fee Timings
    /// </summary>
    /// <returns>If Given Time Is Average Fee Hour</returns>
    /// <param name="hour">Given Hour</param>
    /// <param name="minute">Given Minute</param>
    public static bool IsAverageFeeHours(int hour, int minute)
    {
        return ((hour == 6 && minute >= 30 && minute <= 59)
        || (hour == 8 && minute <= 29)
        || (hour == 15 && minute <= 29)
        || hour == 17);
    }
}