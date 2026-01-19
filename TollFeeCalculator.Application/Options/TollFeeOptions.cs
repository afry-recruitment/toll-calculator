namespace TollFeeCalculator.Application.Options
{
    public sealed class TollFeeOptions
    {
        public int ChargeWindowMinutes { get; init; } = 60; // Time window in minutes to consider multiple passages as a single charge
        public int MaxDailyFee { get; init; } = 60; // Maximum toll fee per day
        public bool TollFreeWeekends { get; init; } = true; // If true, Saturdays and Sundays are considered toll-free
        public int HolidaysCacheDurationHours { get; init; } = 24; // Duration to cache holiday data in hours
        public bool HolidaysFailResultsTollFree { get; init; } = false; // If true, in case of holiday provider failure, days will be considered toll-free
    }
}
