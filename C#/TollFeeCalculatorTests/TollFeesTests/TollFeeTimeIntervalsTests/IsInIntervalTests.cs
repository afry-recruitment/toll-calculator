using TollFeeCalculator.TollFees;

namespace TollFeeCalculatorTests.TollFeesTests.TollFeeTimeIntervalsTests
{
    public class IsInIntervalTests
    {
        private readonly TollFeeTimeInterval _sut;

        public IsInIntervalTests()
        {
            var fromTime = new TimeOnly(10, 12);
            var untilTime = new TimeOnly(13, 52);
            const int tollFee = 23;

            _sut = new TollFeeTimeInterval(
                fromTime,
                untilTime,
                tollFee);
        }

        [Fact]
        public void ShouldReturnTrue_WhenDateTimeTimeIsInInterval()
        {
            var timeToTest = new DateTime(2022, 1, 1, 12, 34, 56);

            var result = _sut.IsInInterval(timeToTest);

            Assert.True(result);

        }

        [Fact]
        public void ShouldReturnTrue_WhenTimeOnlyTimeIsInInterval()
        {
            var timeToTest = new TimeOnly(12, 34);

            var result = _sut.IsInInterval(timeToTest);

            Assert.True(result);

        }

        [Fact]
        public void ShouldReturnFalse_WhenDateTimeTimeIsNotInInterval()
        {
            var timeToTest = new DateTime(2022, 1, 1, 09, 12, 34);

            var result = _sut.IsInInterval(timeToTest);

            Assert.False(result);

        }

        [Fact]
        public void ShouldReturnFalse_WhenTimeOnlyTimeIsNotInInterval()
        {
            var timeToTest = new TimeOnly(09, 12);

            var result = _sut.IsInInterval(timeToTest);

            Assert.False(result);

        }

    }
}
