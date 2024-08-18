using System;

namespace TollFeeCalculator
{
    public class TollFreeDate
    {
        public bool IsTollFree { get; private set; }
        public TollFreeDate(DateTime date)
        {
            IsTollFree = IsTollFreeDate(date);
        }

        private bool IsTollFreeDate(DateTime date) // Denna funktion borde vara kopplad till ett API.
        {
            int year = date.Year;
            int month = date.Month;
            int day = date.Day;

            if (date.DayOfWeek == DayOfWeek.Saturday || date.DayOfWeek == DayOfWeek.Sunday) return true;

            if (year == 2024)
            {
                if (month == 1 && day == 1 ||
                    month == 1 && day == 6 ||
                    month == 3 && (day == 29 || day == 31) ||
                    month == 4 && day == 1 ||
                    month == 5 && (day == 1 || day == 9 || day == 19) ||
                    month == 6 && (day == 6 || day == 21 || day == 22) ||
                    month == 11 && day == 2 ||
                    month == 12 && (day == 24 || day == 25 || day == 26 || day == 31))
                {
                    return true;
                }
            }
            return false;
        }
    }
}