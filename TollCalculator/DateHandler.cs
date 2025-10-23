
namespace TollFeeCalculator
{
    public class DateHandler
    {
        public static bool IsWeekend(DateTime date)
        {
            return date.DayOfWeek == DayOfWeek.Saturday || date.DayOfWeek == DayOfWeek.Sunday;
        }
      
        public static bool IsPublicHoliday(DateTime date)
        {
            int year = date.Year;

            // Fixed-date holidays
            if ((date.Month == 1 && date.Day == 1) ||   // New Year's Day
                (date.Month == 1 && date.Day == 6) ||   // Epiphany
                (date.Month == 5 && date.Day == 1) ||   // Labour Day
                (date.Month == 6 && date.Day == 6) ||   // National Day
                (date.Month == 12 && date.Day == 25) || // Christmas Day
                (date.Month == 12 && date.Day == 26))   // Boxing Day
            {
                return true;
            }

            // Easter-based holidays
            DateTime easterSunday = GetEasterSunday(year);
            if (date.Date == easterSunday.AddDays(-2).Date) return true; // Good Friday
            if (date.Date == easterSunday.Date) return true;             // Easter Sunday
            if (date.Date == easterSunday.AddDays(1).Date) return true;  // Easter Monday
            if (date.Date == easterSunday.AddDays(39).Date) return true; // Ascension Day
            if (date.Date == easterSunday.AddDays(49).Date) return true; // Pentecost (Whit Sunday)

            // Midsummer Day: Saturday between June 20–26
            if (date.Month == 6 && date.Day >= 20 && date.Day <= 26 && date.DayOfWeek == DayOfWeek.Saturday)
            {
                return true;
            }

            // All Saints' Day: Saturday between October 31–November 6
            if ((date.Month == 10 && date.Day == 31 && date.DayOfWeek == DayOfWeek.Saturday) ||
                (date.Month == 11 && date.Day <= 6 && date.DayOfWeek == DayOfWeek.Saturday))
            {
                return true;
            }

            return false;
        }

        // Computus algorithm to calculate Easter Sunday
        private static DateTime GetEasterSunday(int year)
        {
            int a = year % 19;
            int b = year / 100;
            int c = year % 100;
            int d = b / 4;
            int e = b % 4;
            int f = (b + 8) / 25;
            int g = (b - f + 1) / 3;
            int h = (19 * a + b - d - g + 15) % 30;
            int i = c / 4;
            int k = c % 4;
            int l = (32 + 2 * e + 2 * i - h - k) % 7;
            int m = (a + 11 * h + 22 * l) / 451;
            int month = (h + l - 7 * m + 114) / 31;
            int day = ((h + l - 7 * m + 114) % 31) + 1;

            return new DateTime(year, month, day);
        }

    }

    
}