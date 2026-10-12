namespace TollFeeCalculator.Core.Data
{
    /// <summary>
    /// Prices a single toll pass by the time of day.
    ///
    /// The fee table is data, so it is expressed as a table of minute
    /// ranges instead of a chain of if/else conditions (prone to errors)
    /// 
    /// No IFeeSchedule interface on purpose: an interface starts to pay off when a second, 
    /// genuinely different implementation exists (e.g. another city with different rules). 
    /// 
    /// The static method is already a seam that can become an interface later
    /// without touching the callers' call sites more than one line.
    /// </summary>
    public static class TollFeeTimeIntervals
    {
        /// <summary>
        /// The toll fee in SEK for one pass at the given moment.
        /// Slots based on the old code fixed:
        ///     if (hour == 6 && minute >= 0 && minute <= 29) return 8;
		///     else if (hour == 6 && minute >= 30 && minute <= 59) return 13;
		///     else if (hour == 7 && minute >= 0 && minute <= 59) return 18;
		///     else if (hour == 8 && minute >= 0 && minute <= 29) return 13;
		///     else if (hour >= 8 && hour <= 14) return 8;						// FIX-1
		///     else if (hour == 15 && minute >= 0 && minute <= 29) return 13;
		///     else if (hour == 15 || hour == 16) return 18;					// IMPROVE-1
        ///     else if (hour == 17 && minute >= 0 && minute <= 59) return 13;
		///     else if (hour == 18 && minute >= 0 && minute <= 29) return 8;
		///     else return 0;
        /// </summary>
        public static int GetFee(DateTime date)
        {
            return (date.Hour, date.Minute) switch
            {
                (6, >= 0 and <= 29) => 8,
                (6, >= 30) => 13,
                (7, _) => 18,
                (8, <= 29) => 13,
                ( >= 8 and <= 14, _) => 8,
                (15, <= 29) => 13,
                (15 or 16, _) => 18,
                (17, _) => 13,
                (18, <= 29) => 8,
                _ => 0
            };
        }
    }
}
