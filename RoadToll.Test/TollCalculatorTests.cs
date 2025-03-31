using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using RoadToll.Application.Calculator;
using RoadToll.Application.DailyFee;
using RoadToll.Application.DateChecker;
using RoadToll.Application.Models;
using RoadToll.Application.Models.RoadToll;
using RoadToll.Test.Core;

namespace RoadToll.Test
{
    public class TollCalculatorTests : IClassFixture<ConfigurationFixture>
    {
        private readonly IOptions<RoadTollConfiguration> _option;
        private readonly IDailyFeeScheduleCalculator _dailyFeeScheduleCalculator;
        private readonly ITollFreeDateChecker _tollFreeDateChecker;
        private readonly TollCalculator _tollCalculator;

        public TollCalculatorTests(ConfigurationFixture fixture)
        {
            _option = fixture.ServiceProvider.GetRequiredService<IOptions<RoadTollConfiguration>>();
            _dailyFeeScheduleCalculator = fixture.ServiceProvider.GetService<IDailyFeeScheduleCalculator>()!;
            _tollFreeDateChecker = fixture.ServiceProvider.GetService<ITollFreeDateChecker>()!;
            _tollCalculator = new TollCalculator(
                _option,
                _dailyFeeScheduleCalculator,
                _tollFreeDateChecker);
        }

        [Fact]
        public void GetTollFee_AllDatesNotOnSameDay_ThrowsArgumentException()
        {
            var vehicle = Vehicle.Car;
            var dates = new DateTime[] { new(2013, 1, 2, 7, 15, 0), new DateTime(2013, 1, 2, 7, 15, 0).AddDays(1) };

            var exception = Assert.Throws<ArgumentException>(() => _tollCalculator.GetTollFee(vehicle, dates));
            Assert.Equal("All dates must be on the same day.", exception.Message);
        }

        [Fact]
        public void GetTollFee_VehicleIsTollFree_ReturnsZero()
        {
            var vehicle = Vehicle.Motorbike;
            var dates = new DateTime[] { new(2013, 1, 2, 7, 15, 0) };
            var expected = 0;

            var fee = _tollCalculator.GetTollFee(vehicle, dates);

            Assert.Equal(expected, fee);
        }

        [Fact]
        public void GetTollFee_DateIsTollFree_ReturnsZero()
        {
            var vehicle = Vehicle.Car;
            var dates = new DateTime[] { new(2013, 4, 1, 7, 15, 0) };
            var expected = 0;

            var fee = _tollCalculator.GetTollFee(vehicle, dates);

            Assert.Equal(expected, fee);
        }

        [Fact]
        public void GetTollFee_SingleDate_CalculatesFeeCorrectly()
        {
            var vehicle = Vehicle.Car;
            var date = new DateTime(2013, 1, 2, 7, 15, 0);
            var dates = new DateTime[] { date };
            var expected = 18;

            var fee = _tollCalculator.GetTollFee(vehicle, dates);

            Assert.Equal(expected, fee);
        }

        /*
            Date 1 and Date 2 fall within the same interval but 
            have different toll costs. Therefore, 
            the fee for the day should be the higher of the two, 
            which is Date 2 with a cost of 18.
         */
        [Fact]
        public void GetTollFee_OnlyHighestFeeApplied_WhenMultipleDatesInSameInterval()
        {
            var vehicle = Vehicle.Car;
            var date1 = new DateTime(2013, 1, 2, 6, 29, 0);
            var date2 = date1.AddMinutes(45);
            var dates = new DateTime[] { date1, date2 };
            var expected = 18;

            var fee = _tollCalculator.GetTollFee(vehicle, dates);

            Assert.Equal(expected, fee);
        }

        /*
            date 1 and date 2 are within the same interval, 
            so the fee for date 2 should be 0. 
            The fee for date 3 should be 13.
         */
        [Fact]
        public void GetTollFee_MultipleDates_CalculatesFeeCorrectly()
        {
            var vehicle = Vehicle.Car;
            var date1 = new DateTime(2013, 1, 2, 7, 15, 0);
            var date2 = date1.AddMinutes(30);
            var date3 = date1.AddMinutes(61);
            var dates = new DateTime[] { date1, date2, date3 };
            var expected = 31;

            var fee = _tollCalculator.GetTollFee(vehicle, dates);

            Assert.Equal(expected, fee);
        }

        /* 
            All dates fall into separate intervals. 
            The total fee is 62 (13 + 18 + 13 + 18), 
            which exceeds the maximum daily fee. 
            Therefore, the final fee should be capped at 60.
         */
        [Fact]
        public void GetTollFee_TotalFeeExceedsMaxDailyFee_ReturnsMaxDailyFee()
        {
            var vehicle = Vehicle.Car;
            var date1 = new DateTime(2013, 1, 2, 6, 30, 0);
            var date2 = date1.AddMinutes(61);
            var date3 = new DateTime(2013, 1, 2, 15, 00, 0);
            var date4 = date3.AddMinutes(61);
            var dates = new DateTime[] { date1, date2, date3, date4 };
            var expected = _option.Value.MaxDailyFee;

            var fee = _tollCalculator.GetTollFee(vehicle, dates);

            Assert.Equal(expected, fee);
        }
    }
}
