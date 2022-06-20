using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TollFeeCalculator
{
    class TollCalculator
    {


        /**
     * Calculate the total toll fee for one day
     *
     * @param vehicle - the vehicle
     * @param dates   - date and time of all passes on one day
     * @return - the total toll fee for that day
     */

        public int GetTotalTollFee(Vehicle vehicle, DateTime[] dates)
        {
            int totalFee = 0;

            try
            {
                if (IsTollFreeDate(dates[0]) || vehicle.IsTollFreeVehicles())
                {
                    return 0;
                }
                var datesInOrder = dates.OrderBy(d => d);
                DateTime intervalStart = datesInOrder.FirstOrDefault();
                int initialFee = GetTollFee(intervalStart, vehicle);
                totalFee = initialFee;

                foreach (DateTime date in datesInOrder)
                {
                    int nextFee = GetTollFee(date, vehicle);
                    TimeSpan ts = (date - intervalStart);
                    int diffMinutes = Convert.ToInt32(ts.TotalMinutes);
                    if (diffMinutes <= 60)
                    {
                        if (nextFee > initialFee)
                        {
                            totalFee = totalFee - initialFee;
                            totalFee = totalFee + nextFee;
                        }
                    }
                    else
                    {
                        totalFee = totalFee + nextFee;
                        intervalStart = date;
                        initialFee = nextFee;
                    }
                }


                if (totalFee > 60) totalFee = 60;
                return totalFee;

            }
            catch (Exception ex)
            {
                Console.WriteLine("Error in total fee calculation - GetTotalTollFee() {0},Please contact admin - " + ex.Message, "Date- "+DateTime.Now);
            }

            return totalFee;

        }

        public int GetTollFee(DateTime date, Vehicle vehicle)
        {

            try
            { 
                int hour = date.Hour;
                int minute = date.Minute;

                if (hour == 6 && minute <= 29) return 8;
                else if (hour == 6 && minute <= 59) return 13;
                else if (hour == 7 && minute <= 59) return 18;
                else if (hour == 8 && minute <= 29) return 13;
                else if (hour >= 8 && hour <= 14 && minute <= 59) return 8;
                else if (hour == 15 && minute <= 29) return 13;
                else if ((hour >= 15 && hour <= 16 && minute <= 59)) return 18;
                else if (hour == 17 && minute <= 59) return 13;
                else if (hour == 18 && minute <= 29) return 8;
                else return 0;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error in fee calculation by rush hours - GetTollFee() {0}, Please contact admin- " + ex.Message, "Date- " + DateTime.Now);
            }
            return 0;
        }

        private Boolean IsTollFreeDate(DateTime date)
        {
            int year = date.Year;
            int month = date.Month;
            int day = date.Day;

            if (date.DayOfWeek == DayOfWeek.Saturday || date.DayOfWeek == DayOfWeek.Sunday) return true;

            try
            {
                if (year == 2022)
                {

                    List<DateTime> Holiday = new List<DateTime>() {
                { new DateTime(year,1,1)},
                { new DateTime(year,3,28)},
                { new DateTime(year,3,30)},
                { new DateTime(year,4,1)},
                { new DateTime(year,5,1)},
                { new DateTime(year,5,6)},
                { new DateTime(year,5,21)},
                { new DateTime(year,11,1)},
                { new DateTime(year,12,24)},
                { new DateTime(year,12,25)},
                { new DateTime(year,12,26)},
                { new DateTime(year,12,31)},
            };
                    if (Holiday.Contains(date.Date))
                    {
                        return true;
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error in holiday check - IsTollFreeDate() {0},Please contact admin -" + ex.Message, "Date- " + DateTime.Now);
            }
            return false;
        }
    }
}
