using Microsoft.Extensions.Options;
using RoadToll.Application.Models.RoadToll;

namespace RoadToll.Application.DailyFee
{
    public class DailyFeeScheduleCalculator(IOptions<RoadTollConfiguration> configuration) : IDailyFeeScheduleCalculator
    {
        public int GetTollFee(DateTime date)
        {
            TimeOnly vehiclePassingTime = TimeOnly.FromDateTime(date);
            var roadTollConfiguration = configuration.Value;

            foreach (var interval in roadTollConfiguration!.RoadTollSchedule)
            {
                if (vehiclePassingTime >= interval.From && vehiclePassingTime <= interval.To)
                {
                    return interval.Cost;
                }
            }

            return 0;
        }
    }
}
