using System.Collections.ObjectModel;
using TollFeeCalculator.Core;

namespace TollFeeCalculator.Pricing
{
    public class TimeBasedTollCalculator : ITollCalculator
    {
        private static readonly ReadOnlyCollection<TmeBasedPricingInterval> _tollFees = new(new List<TmeBasedPricingInterval>
        {
            new TmeBasedPricingInterval { StartTime = new TimeSpan(6, 0, 0), EndTime = new TimeSpan(6, 29, 0), Price = 8m },
            new TmeBasedPricingInterval { StartTime = new TimeSpan(6, 30, 0), EndTime = new TimeSpan(6, 59, 0), Price = 13m },
            new TmeBasedPricingInterval { StartTime = new TimeSpan(7, 0, 0), EndTime = new TimeSpan(7, 59, 0), Price = 18m },
            new TmeBasedPricingInterval { StartTime = new TimeSpan(8, 0, 0), EndTime = new TimeSpan(8, 29, 0), Price = 13m },
            new TmeBasedPricingInterval { StartTime = new TimeSpan(8, 30, 0), EndTime = new TimeSpan(14, 59, 0), Price = 8m },
            new TmeBasedPricingInterval { StartTime = new TimeSpan(15, 0, 0), EndTime = new TimeSpan(15, 29, 0), Price = 13m },
            new TmeBasedPricingInterval { StartTime = new TimeSpan(15, 30, 0), EndTime = new TimeSpan(16, 59, 0), Price = 18m },
            new TmeBasedPricingInterval { StartTime = new TimeSpan(17, 0, 0), EndTime = new TimeSpan(17, 59, 0), Price = 13m },
            new TmeBasedPricingInterval { StartTime = new TimeSpan(18, 0, 0), EndTime = new TimeSpan(18, 29, 0), Price = 8m }
        });

        public decimal CalculateToll(IVehicle vehicle, DateTime date)
        {
            foreach (var interval in _tollFees)
            {
                if (date.TimeOfDay >= interval.StartTime && date.TimeOfDay <= interval.EndTime)
                {
                    return interval.Price;
                }
            }

            return 0m;
        }
    }
}
