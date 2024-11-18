namespace toll_calculator_jakob.Data
{
    public static class TollFeeIntervals
    {
        public static readonly List<(TimeSpan Start, TimeSpan End, int Fee)> Intervals =
        [
            (TimeSpan.FromHours(6), TimeSpan.FromHours(6) + TimeSpan.FromMinutes(30), 8),  // 06:00 - 06:30
            (TimeSpan.FromHours(6) + TimeSpan.FromMinutes(30), TimeSpan.FromHours(7), 13), // 06:30 - 07:00
            (TimeSpan.FromHours(7), TimeSpan.FromHours(8), 18),                            // 07:00 - 08:00
            (TimeSpan.FromHours(8), TimeSpan.FromHours(8) + TimeSpan.FromMinutes(30), 13), // 08:00 - 08:30
            (TimeSpan.FromHours(8) + TimeSpan.FromMinutes(30), TimeSpan.FromHours(15), 8), // 08:30 - 15:00
            (TimeSpan.FromHours(15), TimeSpan.FromHours(15) + TimeSpan.FromMinutes(30), 13), // 15:00 - 15:30
            (TimeSpan.FromHours(15) + TimeSpan.FromMinutes(30), TimeSpan.FromHours(17), 18), // 15:30 - 17:00
            (TimeSpan.FromHours(17), TimeSpan.FromHours(18), 13),                          // 17:00 - 18:00
            (TimeSpan.FromHours(18), TimeSpan.FromHours(18) + TimeSpan.FromMinutes(30), 8)  // 18:00 - 18:30
        ];
    }
}