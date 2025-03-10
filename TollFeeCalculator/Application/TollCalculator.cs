using DateTimeExtensions;
using TollFeeCalculator.Domain;
using TollFeeCalculator.Domain.Vehicle;

namespace TollFeeCalculator.Application
{
    public class TollCalculator
    {
        private const int MaxDailyFee = 60;
        private static readonly HashSet<VehicleType> TollFreeVehicleTypes = new HashSet<VehicleType>
        {
            VehicleType.Motorbike,
            VehicleType.Tractor,
            VehicleType.Emergency,
            VehicleType.Diplomat,
            VehicleType.Foreign,
            VehicleType.Military
        };

        public static int GetFinalTollFee(IVehicle vehicle, DateTime[] dates)
        {
            if (dates == null || dates.Length == 0) return 0;

            Array.Sort(dates);
            DateTime intervalStart = dates[0];
            int totalFee = 0;

            foreach (DateTime date in dates)
            {
                int currentFee = GetTollFee(date, vehicle);
                TimeSpan timeSpan = date - intervalStart;

                if (timeSpan.TotalMinutes <= 60)
                {
                    totalFee -= GetTollFee(intervalStart, vehicle);
                    totalFee += Math.Max(currentFee, GetTollFee(intervalStart, vehicle));
                }
                else
                {
                    totalFee += currentFee;
                    intervalStart = date;
                }
            }

            return Math.Min(totalFee, MaxDailyFee);
        }


        private static bool IsTollFreeVehicle(IVehicle vehicle)
        {
            if (vehicle == null) return false;
            return TollFreeVehicleTypes.Contains(vehicle.GetVehicleType());
        }

        public static int GetTollFee(DateTime date, IVehicle vehicle)
        {
            if (IsTollFreeDate(date) || IsTollFreeVehicle(vehicle)) return 0;
            return TollFeePolicy.GetFee(date);
        }

        private static bool IsTollFreeDate(DateTime date)
        {
            if (date.DayOfWeek == DayOfWeek.Saturday || date.DayOfWeek == DayOfWeek.Sunday) return true;
            return date.IsHoliday();
        }
    }
}
