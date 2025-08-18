using TollCalculatorSystem.Models;
using PublicHoliday;

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
            var windowMaxFee = GetTollFeeBasedOnTimeOfDay(windowStart);

            for (int i = 1; i < passageTimes.Length; i++)
            {
                var currentPassageTime = passageTimes[i];

                var timeSinceWindowStart = currentPassageTime - windowStart;

                if (timeSinceWindowStart >= TimeSpan.FromMinutes(60))
                {
                    totalFee += windowMaxFee;
                    windowStart = currentPassageTime; // Start new time window
                    windowMaxFee = GetTollFeeBasedOnTimeOfDay(currentPassageTime); // New max fee
                }
                else
                {
                    var fee = GetTollFeeBasedOnTimeOfDay(currentPassageTime);
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

        public int GetTollFeeBasedOnTimeOfDay(DateTime date)
        {
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
            if (date.DayOfWeek == DayOfWeek.Saturday || date.DayOfWeek == DayOfWeek.Sunday)
                return true;

            return new SwedenPublicHoliday().IsPublicHoliday(date);
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
                    int fee = GetTollFeeBasedOnTimeOfDay(time);

                    Console.WriteLine($"{hour:D2}:{minute:D2} | {fee,4}");

                    if (minute == 29 || minute == 59) Console.WriteLine("---------------------");
                }
            }
        }
    }
}