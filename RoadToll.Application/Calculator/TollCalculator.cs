using Microsoft.Extensions.Options;
using RoadToll.Application.DailyFee;
using RoadToll.Application.DateChecker;
using RoadToll.Application.Models;
using RoadToll.Application.Models.RoadToll;

namespace RoadToll.Application.Calculator
{
    public class TollCalculator(IOptions<RoadTollConfiguration> configuration, IDailyFeeScheduleCalculator dailyFeeScheduleCalculator, ITollFreeDateChecker tollFreeDateChecker) : ITollCalculator
    {
        public int GetTollFee(Vehicle vehicle, DateTime[] dates)
        {
            if (!AllDatesOnSameDay(dates))
            {
                throw new ArgumentException("All dates must be on the same day.");
            }

            if (vehicle.IsTollFreeVehicle)
            {
                return 0;
            }

            var roadTollConfiguration = configuration.Value;

            Array.Sort(dates);

            int totalFee = 0;
            int intervalFee = 0;
            DateTime intervalStart = dates[0];

            foreach (var date in dates)
            {
                if (!tollFreeDateChecker.IsTollFreeDate(date))
                {
                    var currentFee = dailyFeeScheduleCalculator.GetTollFee(date);
                    var minutesBetween = (date - intervalStart).TotalMinutes;

                    if (minutesBetween <= roadTollConfiguration!.TollFreeLimitMinutes)
                    {
                        intervalFee = Math.Max(intervalFee, currentFee);
                    }
                    else
                    {
                        totalFee += intervalFee;
                        intervalStart = date;
                        intervalFee = currentFee;
                    }
                }

            }

            totalFee += intervalFee;

            return Math.Min(totalFee, roadTollConfiguration!.MaxDailyFee);
        }

        private static bool AllDatesOnSameDay(DateTime[] dates) => dates.All(date => date.Date == dates[0].Date);
    }
}
