using Nager.Date;
using TollFeeCalculator.Core;

namespace TollFeeCalculator.Services
{
    public class TollExemptionRulesService : ITollExemptionRulesService
    {
        public bool IsTollFreeVehicle(IVehicle vehicle)
        {
            if (vehicle == null) return false;

            VehicleType vehicleType = vehicle.GetVehicleType();

            return Enum.IsDefined(typeof(TollFreeVehicles), vehicleType.ToString());
        }

        public bool IsTollFreeDate(DateTime date)
        {
            return IsWeekend(date) || DateSystem.IsPublicHoliday(date, CountryCode.SE);
        }

        private static bool IsWeekend(DateTime date)
        {
            return date.DayOfWeek == DayOfWeek.Saturday || date.DayOfWeek == DayOfWeek.Sunday;
        }
    }
}
