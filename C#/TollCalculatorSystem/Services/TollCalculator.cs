using TollCalculatorSystem.Models;

namespace TollCalculatorSystem.Services
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

        // Note: Keeping int since swedish congestion charges are whole SEK. Would use decimal for proper monetary calculations in prod
        public int GetTollFee(Vehicle vehicle, DateTime[] dates)
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

        private bool IsTollFreeVehicle(Vehicle vehicle)
        {
            if (vehicle == null) return false;
            String vehicleType = vehicle.GetVehicleType();
            return vehicleType.Equals(TollFreeVehicles.Motorbike.ToString()) ||
                   vehicleType.Equals(TollFreeVehicles.Tractor.ToString()) ||
                   vehicleType.Equals(TollFreeVehicles.Emergency.ToString()) ||
                   vehicleType.Equals(TollFreeVehicles.Diplomat.ToString()) ||
                   vehicleType.Equals(TollFreeVehicles.Foreign.ToString()) ||
                   vehicleType.Equals(TollFreeVehicles.Military.ToString());
        }

        public int GetTollFee(DateTime date, Vehicle vehicle)
        {
            if (IsTollFreeDate(date) || IsTollFreeVehicle(vehicle)) return 0;

            int hour = date.Hour;
            int minute = date.Minute;

            if (hour == 6 && minute <= 29) return 8;            // 6 - 6.29
            if (hour == 6) return 13;                           // 6 - 6.59
            if (hour == 7) return 18;                           // 7 - 7.59
            if (hour == 8 && minute <= 29) return 13;           // 8 - 8.29
            if (hour == 8 && minute >= 30) return 8;            // 8.30 - 8-59
            if (hour >= 9 && hour <= 14) return 8;              // 8.30 - 14.59
            if (hour == 15 && minute <= 29) return 13;          // 15 - 15.29
            if ((hour == 15 && minute >= 30) || hour == 16)
                return 18;                                      // 15.30 - 16.59
            if (hour == 17) return 13;                          // 17 - 17.59
            if (hour == 18 && minute <= 29) return 8;           // 18 - 18.29

            return 0;                                           // 18.30 - 05.59 no fee
        }

        private Boolean IsTollFreeDate(DateTime date)
        {
            int year = date.Year;
            int month = date.Month;
            int day = date.Day;

            if (date.DayOfWeek == DayOfWeek.Saturday || date.DayOfWeek == DayOfWeek.Sunday) return true;

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

        private enum TollFreeVehicles
        {
            Motorbike = 0,
            Tractor = 1,
            Emergency = 2,
            Diplomat = 3,
            Foreign = 4,
            Military = 5
        }
        public void InspectAllTimeSlots()
        {
            var car = new Car();
            var date = new DateTime(2025, 8, 15); // weekday

            for (int hour = 0; hour < 24; hour++)
            {
                for (int minute = 0; minute < 60; minute++)
                {
                    var time = date.AddHours(hour).AddMinutes(minute);
                    int fee = GetTollFee(time, car);

                    Console.WriteLine($"{hour:D2}:{minute:D2} | {fee, 4}");

                    if (minute == 29 || minute == 59) Console.WriteLine("---------------------");
                }
            }
        }
    }
}