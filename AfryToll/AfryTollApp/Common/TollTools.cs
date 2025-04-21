using Microsoft.AspNetCore.Http;

namespace AfryTollApp.Common
{
    public static class TollTools
    {
        public static bool IsPublicHoliday(DateTime date)
        {
            // Define an array of public holidays
            DateTime[] publicHolidays = new DateTime[]
            {
            new DateTime(date.Year, 1, 1),    // New Year's Day
            new DateTime(date.Year, 4, 7) ,
            new DateTime(date.Year, 4, 11) ,
            new DateTime(date.Year, 5, 1),    // Labor Day
            new DateTime(date.Year, 7, 4),    // Independence Day
            new DateTime(date.Year, 11, 11),  // Veterans Day
            new DateTime(date.Year, 12, 25)   // Christmas Day
            };

            // Check if the date is one of the public holidays
            foreach (DateTime holiday in publicHolidays)
            {
                if (date.Date == holiday.Date)
                {
                    return true;
                }
            }

            return false;
        }










    }
}
