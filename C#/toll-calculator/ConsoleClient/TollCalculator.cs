using ConsoleClient.Enum;
using ConsoleClient.Interfaces;

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

        private List<TollData> tollData = new List<TollData>();
        public int GetTollFee(IVehicle vehicle, DateTime[] dates)
        {
            var tollDataOnVehicle = tollData.FirstOrDefault(v => v.Vehicle.RegNumber == vehicle.RegNumber);
            int totalFee = 0;
            int currentFee = 0;

            foreach (DateTime date in dates)
            {
                if (tollDataOnVehicle == null)
                {
                    tollDataOnVehicle = new TollData(vehicle, date, 1);
                    currentFee = GetTollFee(date, tollDataOnVehicle);
                }
                else if (tollDataOnVehicle.DroveThruDate.Hour == date.Hour)
                {
                    var droveThruCount = tollDataOnVehicle.DroveThruCount + 1;
                    tollDataOnVehicle = tollDataOnVehicle.UpdateTollData(date, droveThruCount);
                    currentFee = GetTollFee(date, tollDataOnVehicle);
                }
                else if (tollDataOnVehicle.DroveThruDate.Hour < date.Hour)
                {
                    if (totalFee == 0)
                    {
                        totalFee = totalFee + currentFee;
                    }

                    tollDataOnVehicle = tollDataOnVehicle.UpdateTollData(date, 1);
                    totalFee = totalFee + GetTollFee(date, tollDataOnVehicle);
                }
            }
            if (totalFee == 0)
            {
                totalFee = currentFee;
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

        public int GetTollFee(DateTime date, TollData tollData)
        {
            if (IsTollFreeDate(date) || IsTollFreeVehicle(tollData.Vehicle)) return 0;

            int hour = date.Hour;
            int minute = date.Minute;
            int fee = 0;

            if (hour < 6 || hour == 18 && minute >= 30) fee = 0;
            else if (hour == 6 && minute >= 0 && minute <= 29) fee = tollData.DroveThruCount > 1 ? 13 : 8;
            else if (hour == 6 && minute >= 30 && minute <= 59) fee = 13;
            else if (hour == 7 && minute >= 0 && minute <= 59) fee = 18;
            else if (hour == 8 && minute >= 0 && minute <= 29) fee = 13;
            else if (hour == 8 && minute >= 30 && minute <= 59) fee = tollData.DroveThruCount > 1 ? 13 : 8;
            else if (hour >= 9 && hour <= 14 && minute >= 0 && minute <= 59) fee = 8;
            else if (hour == 15 && minute >= 0 && minute <= 29) fee = tollData.DroveThruCount > 1 ? 18 : 13;
            else if (hour == 15 && minute >= 30 && minute <= 59) fee = 18;
            else if (hour == 16 && minute >= 0 && minute <= 59) fee = 18;
            else if (hour == 17 && minute >= 0 && minute <= 59) fee = 13;
            else if (hour == 18 && minute >= 0 && minute <= 29) fee = 8;

            return fee;
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
            if (date.Month == 1 && (date.Day == 1 || date.Day == 6) ||
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
            var longFriday = easterSunday.AddDays(-2);//lång fredag
            var easterMonday = easterSunday.AddDays(1);//Pingst afton
            var ascensionDay = easterSunday.AddDays(39);//Kristi himmelsfärds

            if (currentDate.ToString("yyyy-MM-dd") == longFriday.ToString("yyyy-MM-dd") ||
                currentDate.ToString("yyyy-MM-dd") == easterMonday.ToString("yyyy-MM-dd") ||
                currentDate.ToString("yyyy-MM-dd") == ascensionDay.ToString("yyyy-MM-dd")) return true;

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
            const byte goldenFactor = 11;//Spread the values out over a range of 0 to 18
            const byte daysInWeek = 7;
            const byte cycleOfDate = 19;//19 years during which the date of Easter repeats itself
            const byte daysBetween = 225;//days between March 21 (the approximate date)

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