namespace TollCalculator.Services.Policies;

public sealed class DefaultTollFeeSchedule : ITollFeeSchedule
{
    public int GetFee(DateTime dateTime)
    {
        var time = dateTime.TimeOfDay;

        if (time >= new TimeSpan(6, 0, 0) && time <= new TimeSpan(6, 29, 59)) return 8;
        if (time >= new TimeSpan(6, 30, 0) && time <= new TimeSpan(6, 59, 59)) return 13;
        if (time >= new TimeSpan(7, 0, 0) && time <= new TimeSpan(7, 59, 59)) return 18;
        if (time >= new TimeSpan(8, 0, 0) && time <= new TimeSpan(8, 29, 59)) return 13;
        if (time >= new TimeSpan(8, 30, 0) && time <= new TimeSpan(14, 59, 59)) return 8;
        if (time >= new TimeSpan(15, 0, 0) && time <= new TimeSpan(15, 29, 59)) return 13;
        if (time >= new TimeSpan(15, 30, 0) && time <= new TimeSpan(16, 59, 59)) return 18;
        if (time >= new TimeSpan(17, 0, 0) && time <= new TimeSpan(17, 59, 59)) return 13;
        if (time >= new TimeSpan(18, 0, 0) && time <= new TimeSpan(18, 29, 59)) return 8;

        return 0;
    }
}