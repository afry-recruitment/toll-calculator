using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using RoadToll.Application.DateChecker;
using RoadToll.Application.Models.RoadToll;
using RoadToll.Test.Core;

namespace RoadToll.Test
{
    public class TollFreeDateCheckerTests(ConfigurationFixture fixture) : IClassFixture<ConfigurationFixture>
    {
        private readonly IOptions<FeeFreeDates> options = fixture.ServiceProvider.GetRequiredService<IOptions<FeeFreeDates>>();

        [Fact]
        public void IsTollFreeDate_ShouldReturnTrueOrFalse_ForDaySpecific()
        {
            var checker = new TollFreeDateChecker(options);

            var resultSaturday = checker.IsTollFreeDate(new DateTime(2013, 5, 25)); // Saturday
            var resultSunday = checker.IsTollFreeDate(new DateTime(2013, 5, 26)); // Sunday
            var resultMonday = checker.IsTollFreeDate(new DateTime(2013, 5, 27)); // Monday

            Assert.True(resultSaturday);
            Assert.True(resultSunday);
            Assert.False(resultMonday);
        }

        [Fact]
        public void IsTollFreeDate_ShouldReturnTrueOrFalse_ForIndividualDays()
        {
            var checker = new TollFreeDateChecker(options);

            var resultHoliday = checker.IsTollFreeDate(new DateTime(2013, 4, 1));
            var resultNonHoliday = checker.IsTollFreeDate(new DateTime(2013, 4, 2));

            Assert.True(resultHoliday);
            Assert.False(resultNonHoliday);
        }

        [Fact]
        public void IsTollFreeDate_ShouldReturnTrueOrFalse_ForDateRange()
        {
            var checker = new TollFreeDateChecker(options);

            var resultWithinRange = checker.IsTollFreeDate(new DateTime(2013, 7, 15));
            var resultStartDate = checker.IsTollFreeDate(new DateTime(2013, 3, 28));
            var resultEndDate = checker.IsTollFreeDate(new DateTime(2013, 5, 9));
            var resultOutsideRange = checker.IsTollFreeDate(new DateTime(2013, 08, 1));

            Assert.True(resultWithinRange);
            Assert.True(resultStartDate);
            Assert.True(resultEndDate);
            Assert.False(resultOutsideRange);
        }
    }
}
