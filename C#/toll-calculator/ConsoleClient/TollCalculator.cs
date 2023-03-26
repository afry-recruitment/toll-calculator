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
            int year = date.Year;
            int month = date.Month;
            int day = date.Day;

            if (date.DayOfWeek == DayOfWeek.Saturday || date.DayOfWeek == DayOfWeek.Sunday) return true;
            if (HolidaysBasedOnEaster(day, month, year)) return true;
            if (Midsummer(year, day)) return true;

            if (month == 1 && day == 1 ||
                    month == 5 && day == 1 ||
                    month == 6 && day == 6 ||
                    month == 12 && (day == 24 || day == 25 || day == 26 || day == 31))
            {
                return true;
            }
            return false;
        }
        private bool Midsummer(int year, int day)
        {
            DateTime startDate = new DateTime(year, 6, 20);
            int daysToSaturday = (6 - (int)startDate.DayOfWeek) % 7;
            startDate.AddDays(daysToSaturday);

            if (day == startDate.Day) return true;

            return false;
        }


        public bool HolidaysBasedOnEaster(int day, int month, int year)
        {
            var easter = CalculateEasterDay(year);
            var longFriday = easter.Day - 2;
            var maundyThursday = easter.Day - 3;
            var easterMonday = easter.AddDays(1).Day;
            var ascensionDate = easter.AddDays(39).Day;
            var pentecostDate = easter.AddDays(49).Day;
            var whitemonday = easter.AddDays(50).Day;


            if (month == easter.Month &&
                (day == easter.Day || day == easter.Day - 1 || day == longFriday ||
                day == maundyThursday || day == easterMonday || day == ascensionDate ||
                day == pentecostDate || day == whitemonday)) return true;

            return false;
        }

        public DateTime CalculateEasterDay(int year)
        {
            int goldenNumber = year % 19 + 1;
            int century = year / 100 + 1;
            int skippedLeapYears = (3 * century / 4) - 12;
            int correction = ((8 * century + 5) / 25) - 5;
            int epact = (11 * goldenNumber + 20 + correction - skippedLeapYears) % 30;
            if (epact == 25 && goldenNumber > 11 || epact == 24)
            {
                epact++;
            }
            int fullMoon = 44 - epact;
            if (fullMoon < 21)
            {
                fullMoon += 30;
            }
            int sunday = (fullMoon + 7) - ((year + year / 4 + fullMoon + 1) % 7);
            if (sunday > 31)
            {
                sunday -= 31;
                return new DateTime(year, 4, sunday);
            }
            else
            {
                return new DateTime(year, 3, sunday);
            }
        }
    }
}