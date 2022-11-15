using System;
using DateTimeExtensions;
using DateTimeExtensions.WorkingDays;

namespace Helpers
{
    public static class HolidayValidator
    {
        public static bool GetPublicHoliday(DateTime date, string countryCode)
        {
            if (countryCode is null)
                throw new ArgumentNullException(nameof(countryCode));

            int month = date.Month;
            int day = date.Day;

            if (
                month == 3 && (day == 28 || day == 29) 
                ||
                month == 11 && day == 1)
            {
                return true;
            }

            if (date.IsHoliday(new WorkingDayCultureInfo(countryCode)))
            {
                return true;
            }

            return false;
        }
    }
}
