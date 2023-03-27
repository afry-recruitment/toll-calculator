using DataLib.Enum;
using DataLib.Interfaces;

namespace ConsoleClient
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

        public int GetTollFee(IVehicle vehicle, DateTime[] dates)
        {
            DateTime intervalStart = dates[0];
            int totalFee = 0;
            foreach (DateTime date in dates)
            {
                int nextFee = GetTollFee(date, vehicle);
                int tempFee = GetTollFee(intervalStart, vehicle);

                long diffInMillies = date.Millisecond - intervalStart.Millisecond;
                long minutes = diffInMillies / 1000 / 60;

                if (minutes <= 60)
                {
                    if (totalFee > 0) totalFee -= tempFee;
                    if (nextFee >= tempFee) tempFee = nextFee;
                    totalFee += tempFee;
                }
                else
                {
                    totalFee += nextFee;
                }
            }
            if (totalFee > 60) totalFee = 60;
            return totalFee;
        }

        private bool IsTollFreeVehicle(IVehicle vehicle)
        {
            if (vehicle == null) return false;
            var vehicleType = vehicle.GetVehicleType();
            return vehicleType.Equals(TollFreeVehicles.Motorbike.ToString()) ||
                   vehicleType.Equals(TollFreeVehicles.Tractor.ToString()) ||
                   vehicleType.Equals(TollFreeVehicles.Emergency.ToString()) ||
                   vehicleType.Equals(TollFreeVehicles.Diplomat.ToString()) ||
                   vehicleType.Equals(TollFreeVehicles.Foreign.ToString()) ||
                   vehicleType.Equals(TollFreeVehicles.Military.ToString());
        }

        public int GetTollFee(DateTime date, IVehicle vehicle)
        {
            if (IsTollFreeDate(date) || IsTollFreeVehicle(vehicle)) return 0;

            int hour = date.Hour;
            int minute = date.Minute;

            if (hour == 6 && minute >= 0 && minute <= 29) return 8;
            else if (hour == 6 && minute >= 30 && minute <= 59) return 13;
            else if (hour == 7 && minute >= 0 && minute <= 59) return 18;
            else if (hour == 8 && minute >= 0 && minute <= 29) return 13;
            else if (hour >= 8 && hour <= 14 && minute >= 30 && minute <= 59) return 8;
            else if (hour == 15 && minute >= 0 && minute <= 29) return 13;
            else if (hour == 15 && minute >= 0 || hour == 16 && minute <= 59) return 18;
            else if (hour == 17 && minute >= 0 && minute <= 59) return 13;
            else if (hour == 18 && minute >= 0 && minute <= 29) return 8;
            else return 0;
        }

        private bool IsTollFreeDate(DateTime date)
        {
            if (date.DayOfWeek == DayOfWeek.Saturday || date.DayOfWeek == DayOfWeek.Sunday) return true;
            if (HolidaysBasedOnEaster(date)) return true;
            if (OtherTollFreeDays(date)) return true;

            return false;
        }

        private bool OtherTollFreeDays(DateTime date)
        {
            if (date.Month == 1 && date.Day == 1 ||//new year
           date.Month == 5 && date.Day == 1 ||//first of may
           date.Month == 6 && date.Day == 6 ||//national day
           date.Month == 12 && date.Day == 25 ||//christmass days and new year
           date.Day == 26 || date.Day == 31) return true;

            return false;
        }

        private bool HolidaysBasedOnEaster(DateTime date)
        {
            var currentDate = new DateTime(date.Year, date.Month, date.Day);
            var easterSunday = GetDateOfEaster(currentDate.Year);
            var longFriday = easterSunday.AddDays(-2);
            var easterMonday = easterSunday.AddDays(1);
            var ascensionDate = easterSunday.AddDays(39);
            var pentecostDay = easterSunday.AddDays(49);

            if (currentDate.ToString("yyyy-MM-dd") == longFriday.ToString("yyyy-MM-dd") ||
                currentDate.ToString("yyyy-MM-dd") == easterMonday.ToString("yyyy-MM-dd") ||
                currentDate.ToString("yyyy-MM-dd") == ascensionDate.ToString("yyyy-MM-dd") ||
                currentDate.ToString("yyyy-MM-dd") == pentecostDay.ToString("yyyy-MM-dd")) return true;

            return false;
        }

        /// <summary>
        /// Calculate when easter appears every year. 
        /// Used the math from link: https://www.rmg.co.uk/stories/topics/when-easter
        /// </summary>
        /// <param name="year"></param>
        /// <returns></returns>
        public DateTime GetDateOfEaster(int year)
        {
            const int goldenFactor = 11;//Spread the values out over a range of 0 to 18
            const int daysInWeek = 7;
            const int cycleOfDate = 19;//19 years during which the date of Easter repeats itself
            const int daysBetween = 225;//days between March 21 (the approximate date)

            int goldenNumber = (year % cycleOfDate) * goldenFactor;
            int closestFullMoon = daysBetween - goldenNumber; // calculate the number of days between March 21
                                                              // and the full moon closest to that date.

            if (closestFullMoon > 50)
                while (closestFullMoon > 51) closestFullMoon = closestFullMoon - 30;
            else if (closestFullMoon > 48)
                closestFullMoon = closestFullMoon - 1;

            int weekDayOfEaster = (year + (year / 4) + closestFullMoon + 1) % daysInWeek;
            int easterSunday = closestFullMoon + daysInWeek - weekDayOfEaster;

            return easterSunday > 31 ? new DateTime(year, 04, easterSunday - 31) : new DateTime(year, 03, easterSunday);
        }
    }
}