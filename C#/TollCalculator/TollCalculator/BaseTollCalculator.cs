using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TollCalculator.vehicles;

namespace TollCalculator.TollCalculator
{

    /// <summary>
    /// Base implementation for TollCalculator
    /// </summary>
    public class BaseTollCalculator : ITollCalculator
    {
        public virtual int GetTollFee(IVehicle vehicle, DateTime[] dates)
        {
            if (vehicle == null && dates == null && dates.Length == 0)
                return 0;

            if (IsTollFreeDate(dates[0]) || vehicle.IsTollFreeVehicle)
                return 0;

            dates = dates.OrderBy(d => d).ToArray();

            var currentHourStart = dates[0];
            int currentHourFee = 0;
            int totalFee = 0;

            DefaultTollValue defaultTollValue = new DefaultTollValue();


            for (int i = 0; i < dates.Length; i++)
            {
                var fee = defaultTollValue.GetTollFee(dates[i]);
                bool withinSameHour = IsWithinSameHour(currentHourStart, dates[i]);
                Console.WriteLine("date {0} - fee {1}", dates[i], fee);

                if (withinSameHour)
                {
                    if (fee > currentHourFee)
                        currentHourFee = fee;
                }
                else
                {
                    totalFee += currentHourFee;
                    currentHourFee = fee;
                    currentHourStart = dates[i];

                }

                if (i == dates.Length - 1)
                    totalFee += currentHourFee;

                if (totalFee >= 60)
                {
                    totalFee = 60;
                    break;
                }

            }

            return totalFee;
        }

        public bool IsWithinSameHour(DateTime startTime, DateTime currentTime)
        {
            return (currentTime - startTime).TotalMinutes <= 60;
        }
        public virtual bool IsTollFreeDate(DateTime date)
        {

            if (date.DayOfWeek == DayOfWeek.Saturday || date.DayOfWeek == DayOfWeek.Sunday) return true;
            var holidayList = new DefaultHolidayCalendar().HolidaysList;
            if (holidayList.Contains(date)) return true;

            return false;
        }

    }
}
