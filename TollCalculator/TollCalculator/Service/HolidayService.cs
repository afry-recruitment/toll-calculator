using System;
using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;
using PublicHoliday;

namespace TollFeeCalculator
{
    public class HolidayService
    {
        /// <summary>
        /// the SwedenPublicHoliday service to check if the specified date falls on a 
        /// public holiday or a day that is not considered a working day/weekend.
        /// </summary>
        /// <param name="date">The date to check for holiday or non-working day status.</param>
        /// <returns>true if the date is a public holiday or a non-working day; otherwise, false.</returns>
        public bool IsHoliday(DateTime date)
        {
            var publicHoliday = new SwedenPublicHoliday();
            // get a list of all holidays 
            return publicHoliday.IsPublicHoliday(date) || !publicHoliday.IsWorkingDay(date);
        }
    }
}