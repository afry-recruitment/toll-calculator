using TollCalculator.Builder;
using TollCalculator.Config;
using TollCalculator.Vehicles;

namespace TollCalculator
{
    public class TollCalculator
    {
        #region Constructors

        public TollCalculator() { }

        public TollCalculator(HashSet<VehicleType> tollFreeVehicles)
        {
            this._tollFreeVehicles = tollFreeVehicles;
        }

        public TollCalculator(TollCalculatorConfig config)
        {
            ArgumentNullException.ThrowIfNull(config, nameof(config));

            if (config.TollFreeVehicles != null && TollFreeVehicles.Any())
            {
                _tollFreeVehicles = config.TollFreeVehicles;
            }

            if (config.Holidays != null && config.Holidays.Any())
            {
                _holidays = config.Holidays;
            }

            if (config.TimeZoneFees != null && config.TimeZoneFees.Any())
            {
                _timeZoneFees = config.TimeZoneFees;
            }

            if (config.DailyFeeLimit is not null)
            {
                _dailyFeeLimit = config.DailyFeeLimit;
            }
        }

        #endregion Constructors

        public decimal GetTollFee(VehicleType vehicle, IEnumerable<DateTime> dates)
        {
            if (TollFreeVehicles.Contains(vehicle))
                return 0;

            return dates
                .Where(FilterFreeDays)
                .Select(d => (time: d, fee: GetFeeOfTime(d.TimeOfDay)))
                .GroupBy(hour => $"{hour.time.ToString("yyyy-MM-dd-HH")}")
                .Select(g => { return (g.First().time, fee: g.Max(i => i.fee)); })
                .GroupBy(day => day.time.ToString("yyyy-MM-dd"))
                .Select(g => { 
                    var sumOfDay = g.Sum(i => i.fee);
                    sumOfDay = sumOfDay > DailyFeeLimit ? DailyFeeLimit : sumOfDay;
                    return (g.First().time, sumOfDay);
                })
                .Sum(f => f.sumOfDay);
        }

        private bool FilterFreeDays(DateTime dateTime) =>
                dateTime.DayOfWeek != DayOfWeek.Saturday
                && dateTime.DayOfWeek != DayOfWeek.Sunday
                && !Holydays.TryGetValue($"{dateTime:MM-dd}", out var _);        

        private decimal GetFeeOfTime(TimeSpan dateTime) => 
            TimeZoneFees
                .Where(tz => tz.Key.Start <= dateTime && tz.Key.End >= dateTime)
                .SingleOrDefault().Value;
               

        public HashSet<VehicleType> TollFreeVehicles => _tollFreeVehicles ?? TollCalculatorConfigDefaults.TollFreeVehicles;
        public Dictionary<FeeTimeZone, decimal> TimeZoneFees => _timeZoneFees ?? TollCalculatorConfigDefaults.TimeZoneFees;
        public HashSet<string> Holydays => _holidays ?? TollCalculatorConfigDefaults.Holidays;
        public decimal DailyFeeLimit => _dailyFeeLimit ?? TollCalculatorConfigDefaults.DailyFeeLimit;

        private HashSet<VehicleType>? _tollFreeVehicles;
        private HashSet<string>? _holidays;
        private Dictionary<FeeTimeZone, decimal>? _timeZoneFees;
        private decimal? _dailyFeeLimit;
    }
}
