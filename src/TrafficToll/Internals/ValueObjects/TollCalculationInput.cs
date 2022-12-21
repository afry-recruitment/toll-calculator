using TrafficToll.Internals.Enums;
using TrafficToll.Internals.Exceptions;

namespace TrafficToll.Internals.ValueObjects
{
    public record TollCalculationInput
    {
        public TimeSpan[] PassingTimes { get; }
        public VehicleType VehicleType { get; }
        public DateTime Date { get; }
        public TollCalculationInput(IEnumerable<DateTime> passings, int vehicleType)
        {
            var passingsIsEmptyMessage = passings.Any() ? null : "DateTime passing collection i empty.";
            var dayMessage = EnsureAllDatesIsSameDay(passings);
            var uniqueDatesMessage = EnsureAllDatesIsUnique(passings);
            var vehicleMessage = EnsureVehicleExists(vehicleType);

            if (passingsIsEmptyMessage != null ||
                dayMessage != null ||
                vehicleMessage != null ||
                uniqueDatesMessage != null)
            {
                throw new InvalidClientInputException(
                    $"While instantiating {nameof(TollCalculationInput)}:" +
                   passingsIsEmptyMessage + dayMessage + vehicleMessage + uniqueDatesMessage);
            }

            PassingTimes = passings.Select(x => new TimeSpan(x.Hour, x.Minute, x.Second, x.Millisecond)).ToArray();
            Date = passings.Select(x => new DateTime(x.Year, x.Month, x.Day)).First();
            VehicleType = (VehicleType)vehicleType;
        }

        private static string? EnsureAllDatesIsSameDay(IEnumerable<DateTime> passings)
        {
            var daysInCalculation = passings.Select(x => x.Day).Distinct();
            if (daysInCalculation.Count() == 1)
                return null;

            return $"The dates provided can only be from the same day, found: {string.Join(", ", daysInCalculation)}\n";
        }

        private static string? EnsureAllDatesIsUnique(IEnumerable<DateTime> passings)
        {
            var passingsAsTimeSpan = passings.Select(x => new TimeSpan(x.Hour, x.Minute, x.Second, x.Millisecond));
            var uniquePassings = passingsAsTimeSpan.Distinct();

            if (uniquePassings.Count() == passings.Count())
                return null;

            var duplications = passingsAsTimeSpan.GroupBy(x => x)
              .Where(g => g.Count() > 1)
              .Select(y => y.Key)
              .ToList();

            return $"The dates provided contains duplicates: {string.Join(", ", duplications)}\n";
        }

        private static string? EnsureVehicleExists(int vehicle)
        {
            var vehicles = Enum.GetValues(typeof(VehicleType)).OfType<int>();
            if (vehicles.Contains(vehicle)) return null;

            return "Vehicle type not found.\n";
        }
    }
}
