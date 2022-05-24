using System;

namespace TollFeeCalculator
{
    public static class TollCalculator
    {
        /**
         * Calculate the total toll fee for one day
         *
         * @param vehicle - the vehicle
         * @param dates   - date and time of all passes on one day, needs to be sorted in ascending order
         * @return - the total toll fee for that day
         */
        public static int GetTollFee(Vehicle vehicle, DateTime[] dates)
        {
            DateTime intervalStart = dates[0];
            int totalFee = 0;
            int highestFeeInInterval = 0;

            for (int i = 0; i < dates.Length; ++i)
            {
                DateTime date = dates[i];
                int nextFee = GetTollFee(date, vehicle);
                var dif = date.Subtract(intervalStart);

                if (dif.TotalMinutes <= 60)
                {
                    if (nextFee > highestFeeInInterval)
                        highestFeeInInterval = nextFee;
                }
                else
                {
                    intervalStart = date;
                    totalFee += highestFeeInInterval;
                    highestFeeInInterval = nextFee;
                }

                if (dif.TotalMinutes > 60 && i == dates.Length - 1)
                    totalFee += nextFee;
            }

            return Math.Min(totalFee, 60);
        }

        /**
         * Calculate the toll fee for a given time
         *
         * @param dates   - date and time
         * @param vehicle - the vehicle
         * @return - the toll fee
         */
        public static int GetTollFee(DateTime date, Vehicle vehicle)
        {
            if (IsTollFreeDate(date) || vehicle.IsTollFreeVechicle())
                return 0;

            int hour = date.Hour;
            int minute = date.Minute;

            if (hour == 6 && minute <= 29) return 8;
            else if (hour == 6 && minute >= 30) return 13;
            else if (hour == 7) return 18;
            else if (hour == 8 && minute <= 29) return 13;
            else if (hour == 8 && minute >= 30) return 8;
            else if (hour >= 9 && hour <= 14) return 8;
            else if (hour == 15 && minute <= 29) return 13;
            else if (hour == 15 && minute >= 30) return 18;
            else if (hour == 16) return 18;
            else if (hour == 17) return 13;
            else if (hour == 18 && minute <= 29) return 8;

            return 0;
        }

        private static bool IsTollFreeDate(DateTime date)
        {
            if (date.DayOfWeek == DayOfWeek.Saturday || date.DayOfWeek == DayOfWeek.Sunday)
                return true;

            return DateUtil.IsHoliday(date);
        }
    }
}