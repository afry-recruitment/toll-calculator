using DateTimeExtensions;
using DateTimeExtensions.TimeOfDay;
using toll_calculator.Vehicles;

namespace toll_calculator
{
    public class TollCalculator : ITollCalculator
    {
        private readonly HashSet<VehicleType> _tollFreeVehicles = new HashSet<VehicleType>()
            {
                VehicleType.Motorbike,
                VehicleType.Tractor,
                VehicleType.EmergencyVehicle,
                VehicleType.DiplomatVehicle,
                VehicleType.ForeignVehicle,
                VehicleType.MilitaryVehicle
            };

        public const int MaximumFeeForGivenDay = 60;

        public int GetTollFeeForDates(IVehicle vehicle, IEnumerable<DateTime> dateTimes)
        {
            if (!dateTimes.Any())
            {
                return 0;
            }

            int totalFeeForDates = 0;
            var orderedDateTimes = dateTimes.OrderBy(d => d).ToList();

            while (orderedDateTimes.Any())
            {
                var startDateTime = orderedDateTimes.First();
                var currentDayInterval = orderedDateTimes.Where(d => d.Year == startDateTime.Year && d.Month == startDateTime.Month && d.Day == startDateTime.Day);
                int feeForDay = GetTollFeeForGivenDay(vehicle, currentDayInterval);
                totalFeeForDates += feeForDay;
                orderedDateTimes = orderedDateTimes.Except(currentDayInterval).ToList();
            }

            return totalFeeForDates;
        }


        internal int GetTollFeeForGivenDay(IVehicle vehicle, IEnumerable<DateTime> dateTimes)
        {
            if (!dateTimes.Any())
            {
                return 0;
            }

            int feeForGivenDay = 0;

            while (dateTimes.Any())
            {
                var startTime = dateTimes.First();
                var currentHourInterval = dateTimes
                    .Where(d => d.Hour >= startTime.Hour && d <= startTime.AddHours(1))
                    .OrderByDescending(d => GetTollFeeForTime(vehicle, d));
                int highestFeeForCurrentHour = GetTollFeeForTime(vehicle, currentHourInterval.First());

                feeForGivenDay += highestFeeForCurrentHour;
                if (feeForGivenDay > MaximumFeeForGivenDay)
                {
                    feeForGivenDay = MaximumFeeForGivenDay;
                    break;
                }

                dateTimes = dateTimes.Except(currentHourInterval).ToList();
            }

            return feeForGivenDay;
        }

        internal int GetTollFeeForTime(IVehicle vehicle, DateTime dateTime)
        {
            if (IsTollFreeDate(dateTime) || IsTollFreeVehicle(vehicle))
            {
                return 0;
            }

            return dateTime switch
            {
                var d when d.IsBetween(new Time(6, 0, 0), new Time(6, 29, 59)) => 8,
                var d when d.IsBetween(new Time(6, 30, 0), new Time(6, 59, 59)) => 13,
                var d when d.IsBetween(new Time(7, 0, 0), new Time(7, 59, 59)) => 18,
                var d when d.IsBetween(new Time(8, 0, 0), new Time(8, 29, 59)) => 13,
                var d when d.IsBetween(new Time(8, 30, 0), new Time(8, 59, 59)) => 8,
                var d when d.IsBetween(new Time(9, 30, 0), new Time(9, 59, 59)) => 8,
                var d when d.IsBetween(new Time(10, 30, 0), new Time(10, 59, 59)) => 8,
                var d when d.IsBetween(new Time(11, 30, 0), new Time(11, 59, 59)) => 8,
                var d when d.IsBetween(new Time(12, 30, 0), new Time(12, 59, 59)) => 8,
                var d when d.IsBetween(new Time(13, 30, 0), new Time(13, 59, 59)) => 8,
                var d when d.IsBetween(new Time(14, 30, 0), new Time(14, 59, 59)) => 8,
                var d when d.IsBetween(new Time(15, 0, 0), new Time(15, 29, 59)) => 13,
                var d when d.IsBetween(new Time(15, 30, 0), new Time(15, 59, 59)) => 18,
                var d when d.IsBetween(new Time(16, 0, 0), new Time(16, 59, 59)) => 18,
                var d when d.IsBetween(new Time(17, 0, 0), new Time(17, 59, 59)) => 13,
                var d when d.IsBetween(new Time(18, 0, 0), new Time(18, 29, 59)) => 8,
                _ => 0,
            };
        }

        internal bool IsTollFreeVehicle(IVehicle vehicle)
        {
            if (vehicle == null)
            {
                throw new ArgumentNullException();
            }

            return _tollFreeVehicles.Contains(vehicle.GetVehicleType());
        }

        internal bool IsTollFreeDate(DateTime dateTime)
        {
            if (dateTime.DayOfWeek == DayOfWeek.Saturday || dateTime.DayOfWeek == DayOfWeek.Sunday || dateTime.IsHoliday())
            {
                return true;
            }

            return false;
        }
    }
}
