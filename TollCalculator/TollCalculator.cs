namespace TollFeeCalculator
{
    public class TollCalculator
    {

        /**
        * Calculate the total toll fee for one day
        *
        * @param vehicle - the vehicle
        * @param dates   - date and time of all passes on one day
        * @return - the total toll fee for that day
        */

        public static int GetTollFee(Vehicle vehicle, DateTime[] dates)
        {
            DateTime intervalStart = dates[0];
            int totalFee = 0;
            int nextFee;
            int tempFee;
            foreach (DateTime date in dates)
            {
                nextFee = GetTollFee(vehicle,date);
                tempFee = GetTollFee(vehicle, intervalStart);

                double minutes = (date - intervalStart).TotalMinutes;

                if (minutes > 60)
                {
                    totalFee += nextFee;
                    intervalStart = date;
                }
                else
                {
                    if (totalFee > 0)
                    {
                        totalFee -= tempFee;
                    }
                    if (nextFee >= tempFee)
                    {
                        tempFee = nextFee;
                    }
                    totalFee += tempFee;
                }

            }
            if (totalFee > 60) totalFee = 60;
            return totalFee;
        }

        private static bool IsTollFreeVehicle(Vehicle vehicle)
        {
            if (vehicle == null) return false;
            return vehicle.IsTollFree;
        }

        public static int GetTollFee(Vehicle vehicle, DateTime date)
        {
            if (IsTollFreeDate(date) || IsTollFreeVehicle(vehicle)) return 0;

            int hour = date.Hour;
            int minute = date.Minute;

            if (hour == 6 && minute >= 0 && minute <= 29) return 8;
            else if (hour == 6 && minute >= 30 && minute <= 59) return 13;
            else if (hour == 7 && minute >= 0 && minute <= 59) return 18;
            else if (hour == 8 && minute >= 0 && minute <= 29) return 13;
            else if (hour == 8 && minute >= 30 && minute <= 59) return 8;
            else if (hour >= 9 && hour <= 14 && minute >= 0 && minute <= 59) return 8;
            else if (hour == 15 && minute >= 0 && minute <= 29) return 13;
            else if (hour == 15 && minute >= 30 && minute <= 59) return 18;
            else if (hour == 16 && minute >= 0 && minute <= 59) return 18;
            else if (hour == 17 && minute >= 0 && minute <= 59) return 13;
            else if (hour == 18 && minute >= 0 && minute <= 29) return 8;
            else return 0;
        }

        private static bool IsTollFreeDate(DateTime date)
        {
            return DateHandler.IsPublicHoliday(date) || DateHandler.IsWeekend(date);
        }
    }
}
