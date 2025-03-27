using Microsoft.Extensions.Logging;
using TollFeeCalculator.Core;
using TollFeeCalculator.Pricing;

namespace TollFeeCalculator.Services
{
    public class TollCalculatorService
    {
        private readonly ITollCalculator _tollCalculator;
        private readonly ITollExemptionRulesService _tollExemptionRulesService;
        private readonly ILogger<TollCalculatorService> _logger;

        public TollCalculatorService(ITollCalculator tollCalculator, ITollExemptionRulesService tollExemptionService, ILogger<TollCalculatorService> logger)
        {
            _tollCalculator = tollCalculator;
            _tollExemptionRulesService = tollExemptionService;
            _logger = logger;
        }

        public decimal GetTollFee(IVehicle vehicle, DateTime[] dates)
        {
            if (dates == null || dates.Length == 0)
            {
                _logger.LogInformation("No dates provided, returning 0.");
                return 0;
            }
            decimal totalFee = 0;
            Array.Sort(dates);

            DateTime currentDate = DateTime.MinValue;
            decimal dailyTotalFee = 0;
            decimal hourlyMaxFee = 0;
            DateTime lastTollTime = DateTime.MinValue;
            try
            {
                foreach (DateTime date in dates)
                {
                    if (date.Date != currentDate.Date)
                    {
                        dailyTotalFee += hourlyMaxFee;
                        totalFee += Math.Min(dailyTotalFee, 60);
                        dailyTotalFee = 0;
                        hourlyMaxFee = 0;
                        currentDate = date;
                    }

                    if (TollFree(vehicle, date))
                    {
                        _logger.LogDebug($"Date {date} or vehicle {vehicle} is toll-free.");
                        continue;
                    }

                    decimal nextFee = _tollCalculator.CalculateToll(vehicle, date);


                    if (TollWithinLastHour(lastTollTime, date))
                    {
                        hourlyMaxFee = Math.Max(hourlyMaxFee, nextFee);
                    }
                    else
                    {
                        dailyTotalFee += hourlyMaxFee;
                        hourlyMaxFee = nextFee;
                        lastTollTime = date;
                    }
                    lastTollTime = date;
                }

                dailyTotalFee += hourlyMaxFee;

                dailyTotalFee = Math.Min(dailyTotalFee, 60);
                totalFee += dailyTotalFee;

                return totalFee;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while calculating toll fees.");
                throw;
            }
        }

        private static bool TollWithinLastHour(DateTime lastTollTime, DateTime date)
        {
            return lastTollTime != DateTime.MinValue && (date - lastTollTime).TotalMinutes <= 60;
        }

        private bool TollFree(IVehicle vehicle, DateTime date)
        {
            return _tollExemptionRulesService.IsTollFreeDate(date) || _tollExemptionRulesService.IsTollFreeVehicle(vehicle);
        }
    }
}