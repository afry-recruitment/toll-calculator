using toll_calculator_jakob.Data;
using toll_calculator_jakob.Models;

namespace toll_calculator_jakob.Services
{
    public class TollCalculatorService : ITollCalculatorService
    {
        public int GetTollFeeTotal(IVehicle vehicle, List<DateTime> dates)
        {
            if (vehicle.IsTollFree || dates is null || dates.Count == 0 || IsTollFreeDate(dates[0])) return 0;

            var start = dates[0];
            var tempFee = GetTollFee(start, vehicle);
            var totalFee = 0;

            foreach (var date in dates.OrderBy(date => date))
            {
                var fee = GetTollFee(date, vehicle);

                if (IsWithinSameHour(start, date))
                {
                    tempFee = Math.Max(fee, tempFee);
                }
                else
                {
                    totalFee += tempFee;
                    if (totalFee >= 60) return 60;

                    tempFee = fee;
                    start = date;
                }
            }

            totalFee += tempFee;
            return totalFee;
        }

        public int GetTollFee(DateTime date, IVehicle vehicle)
        {
            var time = date.TimeOfDay;

            foreach (var interval in TollFeeIntervals.Intervals)
            {
                if (time >= interval.Start && time < interval.End)
                {
                    return interval.Fee;
                }
            }

            return 0;
        }

        public  bool IsTollFreeDate(DateTime date)
        {
            return date.DayOfWeek == DayOfWeek.Saturday || date.DayOfWeek == DayOfWeek.Sunday || IsHoliday(date);
        }

        public  bool IsWithinSameHour(DateTime start, DateTime date)
        {
            var diff = date.TimeOfDay.TotalMinutes - start.TimeOfDay.TotalMinutes;
            return diff < 60;
        }

        public bool IsHoliday(DateTime date)
        {
            var dateOnly = DateOnly.FromDateTime(date);

            if (dateOnly.Month == 7)
                return true;

            if (Holidays.Data.TryGetValue(date.Year, out var holidays) && holidays.Contains(dateOnly))
                return true;

            return false;
        }
    }
}