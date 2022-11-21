using Moq;
using TollFeeCalculator.TollFees;

namespace TollFeeCalculatorTests.TollFeesTests.TollFeeTests
{
    public class IsDateTollFeeFreeTests
    {
        private readonly Mock<ITollFeeRepository> _tollFeeRepositoryMock = new Mock<ITollFeeRepository>();

        public IsDateTollFeeFreeTests()
        {
            _tollFeeRepositoryMock.DefaultValue = DefaultValue.Mock;
            SetTollFeeFreeDatesInRepositoryMock();
        }

        [Fact]
        public void ShouldReturnTrue_IfDateIsTollFeeFree()
        {
            var dateToTest = new DateTime(2022, 12, 24);

            var result = TollFee.IsDateTollFeeFree(dateToTest, _tollFeeRepositoryMock.Object);

            Assert.True(result);
        }

        [Fact]
        public void ShouldReturnFalse_IfDateIsNotTollFeeFree()
        {
            var dateToTest = new DateTime(2022, 12, 01);

            var result = TollFee.IsDateTollFeeFree(dateToTest, _tollFeeRepositoryMock.Object);

            Assert.False(result);
        }

        [Fact]
        public void ShouldThrowArgumentNullException_IfNoTollFeeRepositoryIsSpecified()
        {
            var exception =  Assert.Throws<ArgumentNullException>(() =>
                TollFee.IsDateTollFeeFree(new DateTime(), null));

            Assert.StartsWith("TollRepository cannot be null!", exception.Message);
        }

        private void SetTollFeeFreeDatesInRepositoryMock()
        {
            var tollFeeFreeDates = new List<DateOnly>()
            {
                new (2022, 12, 24),
                new (2022, 12, 25)
            };

            _tollFeeRepositoryMock.Setup(x => x.TollFeeFreeDates)
                .Returns(tollFeeFreeDates);
        }
    }
}
