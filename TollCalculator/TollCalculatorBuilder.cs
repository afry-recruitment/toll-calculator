using TollCalculator.Config;
using TollCalculator.Vehicles;

namespace TollCalculator.Builder
{
    public class TollCalculatorBuilder
    {
        private HashSet<VehicleType>? _tollFreeVehicles;
        private HashSet<string>? _holidays;
        private Dictionary<FeeTimeZone, decimal>? _timeZoneFees;
        private decimal? _dailyFeeLimit;

        public static TollCalculatorBuilder Create() => new();

        public TollCalculatorBuilder AddHolidays(IEnumerable<DateOnly> dates)
        {
            _holidays ??= new HashSet<string>();

            foreach (var dateOnly in dates)
            {
                _holidays.Add(dateOnly.ToString("MM-dd"));
            }
            return this;
        }

        public TollCalculatorBuilder AddTimeZones(Dictionary<FeeTimeZone, decimal> zones)
        {
            _timeZoneFees ??= new();

            foreach (var zone in zones)
            {
                _timeZoneFees.TryAdd(zone.Key, zone.Value);
            }

            return this;
        }

        public TollCalculatorBuilder AddTollFreeVehicles(IEnumerable<VehicleType> vehicles)
        {
            _tollFreeVehicles ??= new();

            foreach (var vehicle in vehicles)
            {
                _tollFreeVehicles.Add(vehicle);
            }

            return this;
        }

        public TollCalculatorBuilder SetDailyLimit(decimal limit)
        {
            _dailyFeeLimit = limit;
            return this;
        }

        public TollCalculator Build()
        {
            ArgumentNullException.ThrowIfNull(_holidays);
            ArgumentNullException.ThrowIfNull(_timeZoneFees);
            ArgumentNullException.ThrowIfNull(_dailyFeeLimit);

            return new(new TollCalculatorConfig(_tollFreeVehicles, _holidays, _timeZoneFees, _dailyFeeLimit));
        }
    }
}
