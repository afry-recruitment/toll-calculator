using TollFeeCalculator.Services.HolidayService;

namespace TollFeeCalculator.Test.Mock
{
    public class HolidayServiceMock : IHolidayService
    {
        public Task<bool> IsHoliday(DateTime date)
        {
            return new Task<bool>(() => HolidayDates(date).Any(date.Date.Equals));
        }

        public List<DateTime> HolidayDates(DateTime date) => [
                new(date.Year, 01, 01),
                new(date.Year, 01, 06),
                new(date.Year, 04, 18),
                new(date.Year, 04, 20),
                new(date.Year, 04, 21),
                new(date.Year, 05, 01),
                new(date.Year, 05, 29),
                new(date.Year, 06, 06),
                new(date.Year, 06, 08),
                new(date.Year, 06, 21),
                new(date.Year, 11, 01),
                new(date.Year, 12, 24),
                new(date.Year, 12, 25),
                new(date.Year, 12, 26),
                new(date.Year, 12, 31),
        ];
    }
}
