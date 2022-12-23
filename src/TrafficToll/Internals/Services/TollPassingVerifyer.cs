using TrafficToll.Internals.Enums;
using TrafficToll.Internals.ValueObjects;

namespace TrafficToll.Internals.Services
{
    internal class TollPassingVerifyer
    {
        private readonly IEnumerable<(int year, int month, int day)> _tollFreeDates;
        private readonly IEnumerable<VehicleType> _tollFreeVehicles;

        public TollPassingVerifyer(TollableParameters tollableParameters)
        {
            _tollFreeDates = tollableParameters.TollFreeDates;
            _tollFreeVehicles = tollableParameters.TollFreeVehicles;
        }

        public IEnumerable<DateTime> GetTollablePassings(IEnumerable<DateTime> passings, VehicleType vehicleType = VehicleType.Other)
        {
            if(_tollFreeVehicles.Contains(vehicleType)) return Array.Empty<DateTime>();
            var tollablePassings = PassingsWhereThereIsNoHoliday(passings, _tollFreeDates);
            return PassingsWhichAreNotDuringWeekend(tollablePassings);
        }

        private static IEnumerable<DateTime> PassingsWhichAreNotDuringWeekend(IEnumerable<DateTime> passings)
        {
            return passings.Where(x => !(x.DayOfWeek == DayOfWeek.Saturday || x.DayOfWeek == DayOfWeek.Sunday));
        }

        private static IEnumerable<DateTime> PassingsWhereThereIsNoHoliday(IEnumerable<DateTime> passings, IEnumerable<(int year, int month, int date)> tollFreeDates)
        {
            return passings.Where(x => !tollFreeDates.Contains((x.Year, x.Month, x.Day)));
        }
    }
}
