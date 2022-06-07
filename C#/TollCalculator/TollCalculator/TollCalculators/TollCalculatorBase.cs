using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TollCalculator
{
    public class TollCalculatorBase: IVehicleTollCalculator
    {
        private readonly Vehicle vehicle;
        private const int MaxTollFeeForTheDay = 60;

        public TollCalculatorBase(Vehicle vehicle)
        {
            this.vehicle = vehicle;
        }

        public int GetTollFee(DateTime[] dates, List<TimeToTollFee> timeToTollFees)
        {
            if (dates != null && dates.Length > 0)
            {
                if (this.vehicle.IsTollFreeVehicle())
                {
                    return 0;
                }
                else
                {
                    return this.CalculateTollFeeOnDates(dates, timeToTollFees);
                }
            }
            else
            {
                throw new ArgumentException("No valid dates passed");
            }
        }

        private int CalculateTollFeeOnDates(DateTime[] dates, List<TimeToTollFee> timeToTollFees)
        {
            List<DateTime> datesList = dates.ToList();

            List<string> uniqueDatesString = datesList.Select(x => x.ToShortDateString()).Distinct().ToList();
            int totalFee = 0;

            foreach (var dateString in uniqueDatesString)
            {
                var totalFeeForTheDay = 0;
                var dateWithTime = datesList.Where(x => x.ToShortDateString() == dateString).ToList();
                dateWithTime.Sort();

                DateTime startDate = dateWithTime.First();
                int startingTollFee = this.GetTollFeeBasedOnDate(startDate, timeToTollFees);

                foreach (DateTime currentDate in dates)
                {
                    var currentTollFee = this.GetTollFeeBasedOnDate(currentDate, timeToTollFees);
                    if (this.IsDateIntervalMoreThanOneHour(startDate, currentDate))
                    {
                        if (currentTollFee > startingTollFee)
                        {
                            totalFeeForTheDay -= startingTollFee;
                        }
                    }

                    totalFeeForTheDay += currentTollFee;
                    startingTollFee = currentTollFee;
                    startDate = currentDate;
                }

                if (this.IsMoreThanMaxTollFeeForTheDay(totalFeeForTheDay))
                {
                    totalFee += MaxTollFeeForTheDay;
                }
                else
                {
                    totalFee += totalFeeForTheDay;
                }
            }
            return totalFee;
        }

        private bool IsMoreThanMaxTollFeeForTheDay(int totalFee)
        {
            if (totalFee > MaxTollFeeForTheDay)
            {
                return true;
            }
            return false;
        }

        private bool IsDateIntervalMoreThanOneHour(DateTime dateStart, DateTime dateEnd)
        {
            long diffInMillieSeconds = dateEnd.Millisecond - dateStart.Millisecond;
            long minutes = diffInMillieSeconds / 1000 / 60;

            if (minutes <= 60)
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        private int GetTollFeeBasedOnDate(DateTime dateTime, List<TimeToTollFee> timeToTollFees)
        {
            if (this.IsTollFreeDate(dateTime))
            {
                return 0;
            }

            var timeSpan = new TimeSpan(dateTime.Hour, dateTime.Minute, dateTime.Second);
            var tollFee = timeToTollFees.Where(x => timeSpan >= x.StartTime && timeSpan <= x.EndTime).Select(x => x.TollFee).FirstOrDefault();
            return tollFee;
        }

        private bool IsTollFreeDate(DateTime date)
        {
            if (this.IsWeekend(date))
            {
                return true;
            }

            if (this.IsHoliday(date))
            {
                return true;
            }

            return false;
        }

        private bool IsWeekend(DateTime date)
        {
            if (date.DayOfWeek == DayOfWeek.Saturday || date.DayOfWeek == DayOfWeek.Sunday)
                return true;
            return false;
        }

        private bool IsHoliday(DateTime date)
        {
            int year = date.Year;
            int month = date.Month;
            int day = date.Day;

            if (year == 2013)
            {
                if (month == 1 && day == 1 ||
                    month == 3 && (day == 28 || day == 29) ||
                    month == 4 && (day == 1 || day == 30) ||
                    month == 5 && (day == 1 || day == 8 || day == 9) ||
                    month == 6 && (day == 5 || day == 6 || day == 21) ||
                    month == 7 ||
                    month == 11 && day == 1 ||
                    month == 12 && (day == 24 || day == 25 || day == 26 || day == 31))
                {
                    return true;
                }
            }

            return false;
        }

    }
}
