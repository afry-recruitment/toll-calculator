using TollCalculatorSystem.Models;
using PublicHoliday;

namespace TollCalculatorSystem.Services
{

    public class TollCalculator
    {
        private readonly record struct FeeChange(TimeOnly StartTime, int Fee);

        private const int Free = 0;
        private const int LowFee = 8;
        private const int MediumFee = 13;
        private const int HighFee = 18;

        private static readonly FeeChange[] FeeTable =
        {
            // Fallback rule: from midnight (00:00) fee is always 0.
            // Ensures there's always at least one matching rule before the first toll interval start.
            // Without this row, queries before 06:00 would give an empty result and First() would throw exception.
            new(TimeOnly.MinValue, Free),

            new(new TimeOnly(6, 00), LowFee),
            new(new TimeOnly(6, 30), MediumFee),
            new(new TimeOnly(7, 00), HighFee),
            new(new TimeOnly(8, 00), MediumFee),
            new(new TimeOnly(8, 30), LowFee),
            new(new TimeOnly(15, 00), MediumFee),
            new(new TimeOnly(17, 00), MediumFee),
            new(new TimeOnly(18, 00), LowFee),
            new(new TimeOnly(18, 30), Free),
        };

        // Note: Keeping int since swedish congestion charges are whole SEK. Would use decimal for proper monetary calculations in prod
        public int CalculateTollFee(IVehicle vehicle, DateTime[] passageTimes)
        {
            // Check to ensure it is one day

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

        public int GetTollFeeBasedOnTimeOfDay(DateTime passageTime)
        {
            var timeOfDay = TimeOnly.FromDateTime(passageTime);

            var fee = FeeTable
                .Where(rule => rule.StartTime <= timeOfDay)   // Keep rules (passed time/fees) that already have started (StartTime <= timeOfDay)
                .OrderByDescending(rule => rule.StartTime)    // Put most recent started rule first
                .First()                                      // Select most recent rule
                .Fee;                                         // Take fee from recent rule

            return fee;
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