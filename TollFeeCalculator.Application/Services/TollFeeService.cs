using TollFeeCalculator.Application.Interfaces;
using TollFeeCalculator.Infrastructure.Interfaces;
using TollFeeCalculator.Domain.Interfaces;

namespace TollFeeCalculator.Application.Services
{
    public class TollFeeService(ITollRepository tollRepository) : ITollFeeService
    {
        private const int MaxDailyFee = 60; // Max fee per day is 60 SEK
        public int CalculateTollFee(IVehicle vehicle, DateTime[] dates)
        {
            if (tollRepository.IsTollFreeVehicle(vehicle) || dates == null || dates.Length == 0 || vehicle == null)
                return 0;

            var sortedDates = dates.OrderBy(d => d).ToList();
            int totalFee = 0;
            DateTime intervalStart = sortedDates[0];
            int maxFeeInInterval = 0;

            foreach (var date in sortedDates)
            {
                if (tollRepository.IsTollFreeDate(date)) continue;

                int fee = tollRepository.GetTollFeeByTime(date);
                if ((date - intervalStart).TotalMinutes < 60)
                {
                    maxFeeInInterval = Math.Max(maxFeeInInterval, fee);
                }
                else
                {
                    totalFee += maxFeeInInterval;
                    intervalStart = date;
                    maxFeeInInterval = fee;
                }
            }

            totalFee += maxFeeInInterval;
            return Math.Min(totalFee, MaxDailyFee);
        }
    }
}
