using TollFeeCalculator.Infrastructure.Repositories;

namespace TollFeeCalculator.Tests.Infrastructure
{
    public class TollRepositoryTests
    {
        private readonly TollRepository _tollRepository;

        public TollRepositoryTests()
        {
            _tollRepository = new TollRepository();
        }

        [Theory]
        [InlineData("2025-01-01")] // New Year's Day
        [InlineData("2025-12-25")] // Christmas 
        [InlineData("2025-12-31")] // New Year's Eve
        public void IsTollFreeDate_ShouldReturnTrue_ForHolidays(string dateStr)
        {
            var date = DateTime.Parse(dateStr);
            bool result = _tollRepository.IsTollFreeDate(date);

            Assert.True(result);
        }

        [Theory]
        [InlineData("2025-02-08")] // Saturday
        [InlineData("2025-02-09")] // Sunday
        public void IsTollFreeDate_ShouldReturnTrue_ForWeekends(string dateStr)
        {
            var date = DateTime.Parse(dateStr);
            bool result = _tollRepository.IsTollFreeDate(date);

            Assert.True(result);
        }

        [Theory]
        [InlineData("2025-02-07 06:00", 8)]
        [InlineData("2025-02-07 07:30", 18)]
        [InlineData("2025-02-07 08:15", 13)] // Rush hour
        [InlineData("2025-02-07 15:30", 18)]
        [InlineData("2025-02-07 17:45", 13)] // Rush hour
        [InlineData("2025-02-07 18:20", 8)]
        [InlineData("2025-02-07 22:00", 0)] // No toll after 18:30
        public void GetTollFeeByTime_ShouldReturnCorrectFee(string dateStr, int expectedFee)
        {
            var date = DateTime.Parse(dateStr);
            int result = _tollRepository.GetTollFeeByTime(date);

            Assert.Equal(expectedFee, result);
        }

        [Fact]
        public void GetTollFeeByTime_ShouldReturnZero_ForNonTollHours()
        {
            var date = new DateTime(2025, 02, 06, 23, 0, 0); // Late night
            int result = _tollRepository.GetTollFeeByTime(date);

            Assert.Equal(0, result);
        }
    }
}
