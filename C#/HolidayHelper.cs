namespace TollFeeCalculator
{
    // This class is based on https://sv.wikipedia.org/wiki/Helgdagar_i_Sverige
    public static class HolidayHelper
    {
        /// <summary>
        /// Checks whether a certain date is a bank holiday in Sweden
        /// </summary>
        /// <param name="date">The date to check</param>
        /// <returns>True if bank holiday, false otherwise</returns>
        public static bool IsSwedishBankHoliday(DateTime date)
        {
            // Fixed dates
            int month = date.Month;
            int day = date.Day;
            if (
                month == 1 && day == 1 ||   // New Year's Day
                month == 1 && day == 6 ||   // Epiphany 
                month == 5 && day == 1 ||   // First of May 
                month == 6 && day == 6 ||   // National Day
                month == 12 && day == 24 || // Christmas Eve
                month == 12 && day == 25 || // Christmas Day
                month == 12 && day == 26 || // Boxing Day
                month == 12 && day == 31    // New Year's Eve
                )
                return true;

            DateTime midsummer = GetMidsummerDay(date.Year);
            if (
                date.DayOfYear == midsummer.DayOfYear ||    // Midsummer
                date.DayOfYear == midsummer.DayOfYear - 1   // Midsummer's Eve
              )
                return true;

            DateTime allHallowsDay = GetAllHallowsDay(date.Year);
            if (date.DayOfYear == allHallowsDay.DayOfYear) // All Hallows' Day
                return true;


            DateTime easter = GetEaster(date.Year);
            return
                date.DayOfYear == easter.DayOfYear - 2 ||   // Good Friday
                date.DayOfYear == easter.DayOfYear - 1 ||   // Holy Saturday
                date.DayOfYear == easter.DayOfYear ||       // Easter
                date.DayOfYear == easter.DayOfYear + 1 ||   // Easter Monday
                date.DayOfYear == easter.DayOfYear + 39 ||  // Ascension
                date.DayOfYear == easter.DayOfYear + 48 ||  // Pentecost's Eve
                date.DayOfYear == easter.DayOfYear + 49;    // Pentecost
        }

        private static DateTime GetMidsummerDay(int year)
        {
            // Pick first Saturday on or after 20th of June
            DateTime day = new(year, 6, 20);
            while (day.DayOfWeek != DayOfWeek.Saturday)
                day = day.AddDays(1);
            return day;
        }

        private static DateTime GetAllHallowsDay(int year)
        {
            // Pick first Saturday on or after 31st of October
            DateTime day = new(year, 10, 31);
            while (day.DayOfWeek != DayOfWeek.Saturday)
                day = day.AddDays(1);
            return day;
        }

        // Based on https://sv.wikipedia.org/wiki/P%C3%A5skdagen
        private static DateTime GetEaster(int year)
        {
            if (year < 1900 || year > 2099)
                throw new ArgumentOutOfRangeException(nameof(year), "The year must be in the range [1900, 2099]");

            int a = year % 19;
            int b = year % 4;
            int c = year % 7;

            int d = (19 * a + 24) % 30;
            int e = (2 * b + 4 * c + 6 * d + 5) % 7;

            int f = d + e;
            if (f == 35 || d == 28 && e == 6)
                f -= 7;

            return new DateTime(year, 3, 22).AddDays(f);
        }
    }
}
