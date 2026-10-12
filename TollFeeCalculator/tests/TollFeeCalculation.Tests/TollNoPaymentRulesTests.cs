using TollFeeCalculator.Core.Vehicles;

namespace TollFeeCalculator.Tests
{
    /// <summary>
    /// These tests pass against the original code we have received from our retiring colleague.
    /// 
    /// Checking the exemption rules: 
    /// 
    /// toll-free vehicle categories (motorbikes) and
    /// 
    /// toll-free dates (weekends, all of July, and the 2013 holiday list) 
    /// 
    /// </summary>
    public class TollNoPaymentRulesTests
    {
        private readonly TollCalculator _calculator = new();
        private static readonly DateTime WeekdayPeak = new(2013, 1, 2, 7, 0, 0); // Wednesday 07:00


        [Fact]
        public void Motorbike_IsTollFree()
        {
            Assert.Equal(0, _calculator.GetTollFee(WeekdayPeak, new Motorbike()));
        }

        [Theory]
        [InlineData(2013, 1, 5)]    // Saturday
        [InlineData(2013, 1, 6)]    // Sunday
        [InlineData(2013, 7, 15)]   // July is toll-free in the Swedish system
        [InlineData(2013, 1, 1)]    // New Year's Day
        [InlineData(2013, 12, 25)]  // Christmas Day
        [InlineData(2013, 12, 24)]  // Christmas Eve (half day, toll-free in the rules)
        public void TollFreeDates_ChargeZero(int year, int month, int day)
        {
            Assert.Equal(0, _calculator.GetTollFee(new DateTime(year, month, day, 7, 0, 0), new Car()));
        }

        [Fact]
        public void Car_PaysInANormalDayAtRushHour() // test here only to show that the toll fee is not zero for a car in a normal weekday
        {
            Assert.Equal(18, _calculator.GetTollFee(WeekdayPeak, new Car()));
        }
    }
}
