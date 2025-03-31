using Microsoft.Extensions.Options;
using RoadToll.Application.Models.RoadToll;

namespace RoadToll.Application.DateChecker
{
    public class TollFreeDateChecker(IOptions<FeeFreeDates> configuration) : ITollFreeDateChecker
    {
        public bool IsTollFreeDate(DateTime date)
        {
            var feeFreeDates = configuration.Value;
            var dateOnly = DateOnly.FromDateTime(date.Date);

            var isFreeDayOfWeek = feeFreeDates.DaySpecific.Contains(date.DayOfWeek);
            var isFreeIndividualDays = feeFreeDates.IndividualDays.Any(d => d == dateOnly);
            var isInFreeDateRange = feeFreeDates.DateRange.Any(x => dateOnly >= x.StartDate && dateOnly <= x.EndDate);

            return isFreeDayOfWeek || isFreeIndividualDays || isInFreeDateRange;
        }
    }
}
