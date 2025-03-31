using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using RoadToll.Application.DailyFee;
using RoadToll.Application.Models.RoadToll;
using RoadToll.Test.Core;

namespace RoadToll.Test
{
    public class DailyFeeScheduleCalculatorTests(ConfigurationFixture fixture) : IClassFixture<ConfigurationFixture>
    {
        private readonly IOptions<RoadTollConfiguration> options = fixture.ServiceProvider.GetRequiredService<IOptions<RoadTollConfiguration>>();

        [Theory]
        [InlineData("2013-01-01 05:59:59", 0)]
        [InlineData("2013-01-01 06:00:00", 8)]
        [InlineData("2013-01-01 06:30:00", 13)]
        [InlineData("2013-01-01 07:59:00", 18)]
        [InlineData("2013-01-01 08:00:00", 13)]
        [InlineData("2013-01-01 08:30:00", 8)]
        [InlineData("2013-05-01 14:59:00", 8)]
        [InlineData("2013-05-01 15:00:00", 13)]
        [InlineData("2013-05-01 16:00:00", 18)]
        [InlineData("2013-05-01 17:00:00", 13)]
        [InlineData("2013-05-01 18:29:59", 8)]
        [InlineData("2013-05-01 18:30:00", 0)]
        public void GetTollFee_ShouldReturnCorrectFee_ForMatchingInterval(string date, int expected)
        {
            var calculator = new DailyFeeScheduleCalculator(options);

            var result = calculator.GetTollFee(DateTime.Parse(date));

            Assert.Equal(expected, result);
        }
    }
}
