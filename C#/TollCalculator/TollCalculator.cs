using System;
using System.Globalization;
using TollFeeCalculator;

namespace TollFeeCalculator
{
    public class TollCalculator : ITollCalculator
    {
        /// <summary>
        /// Calculate the total toll fee for one day
        /// </summary>
        /// <returns>the total toll fee for that day</returns>
        /// <param name="vehicle">the vehicle</param>
        /// <param name="times">date and time of all passes on one day</param>

        public int GetTotalTollFee(IVehicle vehicle, DateOnly date, TimeOnly[] times)
        {
            var intervalStart = date.ToDateTime(times[0]);
            int totalFee = 0;
            foreach (var time in times)
            {
                var dateTime = date.ToDateTime(time);
                int nextFee = GetTollFee(dateTime, vehicle);
                int tempFee = GetTollFee(intervalStart, vehicle);

                long diffInMillies = dateTime.Millisecond - intervalStart.Millisecond;
                long minutes = diffInMillies / 1000 / 60;

                if (minutes <= 60)
                {
                    if (totalFee > 0)
                        totalFee -= tempFee;
                    if (nextFee >= tempFee)
                        tempFee = nextFee;
                    totalFee += tempFee;
                }
                else
                {
                    totalFee += nextFee;
                }
            }
            if (totalFee > 60)
                totalFee = 60;
            return totalFee;
        }

        public int GetTollFee(DateTime date, IVehicle vehicle)
        {
            if (IsTollFreeDate(date) || IsTollFreeVehicle(vehicle))
                return 0;

            int hour = date.Hour;
            int minute = date.Minute;

            if (hour == 6 && minute >= 0 && minute <= 29)
                return 8;
            else if (hour == 6 && minute >= 30 && minute <= 59)
                return 13;
            else if (hour == 7)
                return 18;
            else if (hour == 8 && minute >= 0 && minute <= 29)
                return 13;
            else if (hour == 8 && minute <= 30)
                return 8;
            else if (hour >= 9 && hour <= 14)
                return 8;
            else if (hour == 15 && minute >= 0 && minute <= 29)
                return 13;
            else if (hour == 15 && minute >= 30)
                return 18;
            else if (hour == 16)
                return 18;
            else if (hour == 17)
                return 13;
            else if (hour == 18 && minute >= 0 && minute <= 29)
                return 8;
            else
                return 0;
        }

        private bool IsTollFreeVehicle(IVehicle vehicle)
        {
            return vehicle?.IsTollFree ?? false;
        }
        private Boolean IsTollFreeDate(DateTime date)
        {
            int year = date.Year;
            int month = date.Month;
            int day = date.Day;

            if (date.DayOfWeek == DayOfWeek.Saturday || date.DayOfWeek == DayOfWeek.Sunday)
                return true;

            if (year == 2013)
            {
                if (
                    month == 1 && day == 1
                    || month == 3 && (day == 28 || day == 29)
                    || month == 4 && (day == 1 || day == 30)
                    || month == 5 && (day == 1 || day == 8 || day == 9)
                    || month == 6 && (day == 5 || day == 6 || day == 21)
                    || month == 7
                    || month == 11 && day == 1
                    || month == 12 && (day == 24 || day == 25 || day == 26 || day == 31)
                )
                {
                    return true;
                }
            }
            return false;
        }
    }
}
