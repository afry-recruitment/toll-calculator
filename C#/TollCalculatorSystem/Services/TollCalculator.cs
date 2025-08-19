using TollCalculatorSystem.Models;
using PublicHoliday;

namespace TollCalculatorSystem.Services
{

    public class TollCalculator
    {
        private static readonly HashSet<VehicleType> TollFreeVehicles =
            new()
            {
                VehicleType.Motorbike,
                VehicleType.Tractor,
                VehicleType.Emergency,
                VehicleType.Diplomat,
                VehicleType.Foreign,
                VehicleType.Military
            };

        private static readonly SwedenPublicHoliday _sweHolidayProvider = new();
        private readonly record struct FeeChange(TimeOnly StartTime, int Fee);

        private const int Free = 0;
        private const int LowFee = 8;
        private const int MediumFee = 13;
        private const int HighFee = 18;
        private const int DailyMaxFee = 60;

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
            new(new TimeOnly(15, 30), HighFee),
            new(new TimeOnly(17, 00), MediumFee),
            new(new TimeOnly(18, 00), LowFee),
            new(new TimeOnly(18, 30), Free),
        };

        // Note: Keeping int since swedish congestion charges are whole SEK. Would use decimal for proper monetary calculations in prod
        public int CalculateTollFee(VehicleType vehicleType, DateTime[] passageTimes)
        {
            if (passageTimes == null || passageTimes.Length == 0)
                throw new ArgumentException("No passes", nameof(passageTimes));

            // Early return if toll free vehicle
            if (IsTollFreeVehicle(vehicleType))
                return 0;

            var sortedPassages = passageTimes
                .Distinct()
                .OrderBy(t => t)
                .ToArray();

            // Verify same day
            var firstDate = sortedPassages[0].Date;
            if (sortedPassages.Any(t => t.Date != firstDate))
                throw new ArgumentException("All passages must be from the same day", nameof(sortedPassages));

            DateTime windowStart = sortedPassages[0];    // First passage

            // Eearly return if toll free date
            if (IsTollFreeDate(windowStart))
                return 0;

            int totalFee = 0;
            var windowMaxFee = GetTollFeeBasedOnTimeOfDay(windowStart);

            // Only runs if more than 1 pass
            for (int i = 1; i < sortedPassages.Length; i++)
            {
                var currentPassageTime = sortedPassages[i];

                var timeSinceWindowStart = currentPassageTime - windowStart;

                if (timeSinceWindowStart >= TimeSpan.FromMinutes(60))
                {
                    totalFee += windowMaxFee;           // Close current time window
                    windowStart = currentPassageTime;   // Start new time window
                    windowMaxFee = GetTollFeeBasedOnTimeOfDay(currentPassageTime); // New max fee
                }
                else
                {
                    var fee = GetTollFeeBasedOnTimeOfDay(currentPassageTime);
                    if (fee > windowMaxFee)
                        windowMaxFee = fee;
                }
            }

            // Add last open window
            totalFee += windowMaxFee;

            return totalFee > DailyMaxFee ? DailyMaxFee : totalFee;
        }

        private bool IsTollFreeVehicle(VehicleType vehicle)
        {
            return TollFreeVehicles.Contains(vehicle);
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

        private bool IsTollFreeDate(DateTime date)
        {
            if (date.DayOfWeek == DayOfWeek.Saturday || date.DayOfWeek == DayOfWeek.Sunday)
                return true;

            return _sweHolidayProvider.IsPublicHoliday(date);
        }
    }
}