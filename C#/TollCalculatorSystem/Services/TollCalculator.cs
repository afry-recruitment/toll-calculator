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
        public int CalculateTollFee(IVehicle vehicle, DateTime[] passageTimes)
        {
            if (vehicle == null)
                throw new ArgumentNullException(nameof(vehicle));

            if (passageTimes == null || passageTimes.Length == 0)
                return 0;

            // Early return if toll free vehicle
            if (IsTollFreeVehicle(vehicle))
                return 0;

            Array.Sort(passageTimes);

            DateTime windowStart = passageTimes[0];    // First passage
            // Eearly return if toll free date
            if (IsTollFreeDate(windowStart))
                return 0;

            int totalFee = 0;
            var windowMaxFee = GetTollFee(windowStart, vehicle);

            for (int i = 1; i < passageTimes.Length; i++)
            {
                var currentPassageTime = passageTimes[i];

                var timeSinceWindowStart = currentPassageTime - windowStart;

                if (timeSinceWindowStart >= TimeSpan.FromMinutes(60))
                {
                    totalFee += windowMaxFee;
                    windowStart = currentPassageTime; // Start new time window
                    windowMaxFee = GetTollFee(currentPassageTime, vehicle); // New max fee
                }
                else
                {
                    var fee = GetTollFee(currentPassageTime, vehicle);
                    if (fee > windowMaxFee)
                        windowMaxFee = fee;
                }
            }

            totalFee += windowMaxFee;

            if (totalFee > 60) return 60;
            else return totalFee;
        }

        private bool IsTollFreeVehicle(IVehicle vehicle)
        {
            if (vehicle == null) return false;

            return vehicle.GetVehicleType() switch
            {
                VehicleType.Motorbike or
                VehicleType.Tractor or
                VehicleType.Emergency or
                VehicleType.Diplaomat or
                VehicleType.Foreign or
                VehicleType.Military => true,
                _ => false
            };
        }

        public int GetTollFee(DateTime date, IVehicle vehicle)
        {
            if (vehicle == null)
                throw new ArgumentNullException(nameof(vehicle));
            
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

                    Console.WriteLine($"{hour:D2}:{minute:D2} | {fee,4}");

                    if (minute == 29 || minute == 59) Console.WriteLine("---------------------");
                }
            }
        }
    }
}