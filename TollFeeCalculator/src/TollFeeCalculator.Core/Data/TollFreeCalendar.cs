namespace TollFeeCalculator.Core.Data
{
    /// <summary>
    /// Decides when no toll is charged: weekends, all of July, and the fixed 
    /// holiday list of 2013.
    ///
    /// Dates are data, so a holiday list is used instead of a chain of
    /// month/day comparisons buried in the calculator (a change in holiday law
    /// must not touch the calculation logic) 
    /// 
    /// In production the list would come from configuration or a database; 
    /// the 2013 list is kept exactly as the original code had it.
    /// </summary>
    public static class TollFreeCalendar
    {
        /// <summary>Fixed Swedish public holidays of 2013, as in the original code.</summary>
        private static readonly List<DateOnly> Holidays2013 = new()
        {
            new DateOnly(2013, 1, 1),   // New Year's Day
            new DateOnly(2013, 3, 28),  // Skärtorsdagen/Maundy Thursday
            new DateOnly(2013, 3, 29),  // Långfredagen/Good Friday
            new DateOnly(2013, 4, 1),   // Easter Monday
            new DateOnly(2013, 4, 30),  // Valborgsnatten/Walpurgis Night
            new DateOnly(2013, 5, 1),   // May Day
            new DateOnly(2013, 5, 8),   // Day before Ascension
            new DateOnly(2013, 5, 9),   // Ascension Day
            new DateOnly(2013, 6, 5),   // Day before the Swedish National Day
            new DateOnly(2013, 6, 6),   // Swedish National Day
            new DateOnly(2013, 6, 21),  // Midsummer
            new DateOnly(2013, 11, 1),  // All Saints' Day
            new DateOnly(2013, 12, 24), // Christmas Eve
            new DateOnly(2013, 12, 25), // Christmas Day
            new DateOnly(2013, 12, 26), // Boxing Day
            new DateOnly(2013, 12, 31), // New Year's Eve
        };

        /// <summary>
        /// True when no toll is charged on the given date.
        /// </summary>
        public static bool IsTollFreeDate(DateTime date)
        {
            DateOnly day = DateOnly.FromDateTime(date);

            return day.DayOfWeek is DayOfWeek.Saturday or DayOfWeek.Sunday
                || day.Month == 7                 // July is toll-free
                || Holidays2013.Contains(day);
        }
    }
}
