using System;
using System.Collections.Generic;
using System.Text;

namespace TollFeeCalculator
{
    public static class TollFeeSchedule
    {
        public static bool IsTollFreeDate(DateTime date)
        {
            // Check if the date is a weekend (Saturday or Sunday)
            if (date.DayOfWeek == DayOfWeek.Saturday || date.DayOfWeek == DayOfWeek.Sunday)
            {
                return true;
            }
            // Check for public holidays (example: January 1st, December 25th)
            if ((date.Month == (int)Months.January && date.Day == 1) || (date.Month == (int)Months.May && date.Day == 1) || 
                (date.Month == (int)Months.June && date.Day == 6) ||
                (date.Month == (int)Months.December && (date.Day == 24 || date.Day == 25 || date.Day == 26 || date.Day == 31)))
            {
                return true;
            }
            return false;
        
        public enum Months
        {
            January = 1,
            February = 2,
            March = 3,
            April = 4,
            May = 5,
            June = 6,
            July = 7,
            August = 8,
            September = 9,
            October = 10,
            November = 11,
            December = 12
        }
    }
}
