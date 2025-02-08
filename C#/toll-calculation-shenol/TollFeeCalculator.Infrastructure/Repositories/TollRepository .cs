using TollFeeCalculator.Domain.Interfaces;
using TollFeeCalculator.Infrastructure.Helpers;
using TollFeeCalculator.Infrastructure.Interfaces;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace TollFeeCalculator.Infrastructure.Repositories
{
    public class TollRepository : ITollRepository
    {
        public int GetTollFeeByTime(DateTime date)
        {
            var time = date.TimeOfDay;
            foreach (var (Start, End, Fee) in TollFeeIntervals.FeeIntervals)
            {
                if (time >= Start && time < End)
                    return Fee;
            }
            return 0;
        }
        public bool IsTollFreeDate(DateTime date)
        {
            return date.DayOfWeek == DayOfWeek.Saturday || 
                   date.DayOfWeek == DayOfWeek.Sunday || 
                   Holidays.NationalHolidayDates2025.Contains(date);
        }

        public bool IsTollFreeVehicle(IVehicle vehicle)
        {
            return vehicle.IsTollFree;
        }

    }
}
