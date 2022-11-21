using Moq;
using TollFeeCalculator.TollFees;
using TollFeeCalculator.Vehicles;

namespace TollFeeCalculatorTests.TollFeesTests.TollFeeTests
{
    public class GetTollFeeForTimeOfDayTests
    {
        private readonly Mock<ITollFeeRepository> _tollFeeRepositoryMock = new();
        private readonly Mock<IVehicle> _vehicleMock = new();

        public GetTollFeeForTimeOfDayTests()
        {
            _tollFeeRepositoryMock.DefaultValue = DefaultValue.Mock;
            _vehicleMock.SetupGet(x => x.VehicleClassification).Returns(VehicleClassification.Standard);
            _vehicleMock.SetupGet(x => x.VehicleType).Returns(VehicleType.Car);
        }

        [Fact]
        public void ShouldReturnTollFee_IfTollFeeIsSetForTimeSpecified()
        {
            var timeToTest = new DateTime(2022, 1, 1, 11, 30, 0);
            var expectedTollFee = SetTollFeeTimeIntervalsInRepositoryMock();

            var result = TollFee.GetTollFeeForTimeOfDay(timeToTest, _vehicleMock.Object, _tollFeeRepositoryMock.Object);

            Assert.Equal(expectedTollFee, result);
        }

        [Fact]
        public void ShouldReturnZero_IfTollFeeIsNotSetForTimeSpecified()
        {
            var timeToTest = new DateTime(2022, 1, 1, 9, 30, 0);
            const int expectedTollFee = 0;
            _ = SetTollFeeTimeIntervalsInRepositoryMock();

            var result = TollFee.GetTollFeeForTimeOfDay(timeToTest, _vehicleMock.Object, _tollFeeRepositoryMock.Object);

            Assert.Equal(expectedTollFee, result);
        }
        
        [Fact]
        public void ShouldThrowArgumentNullException_IfNoVehicleIsSpecified()
        {
            var exception =  Assert.Throws<ArgumentNullException>(() =>
                TollFee.GetTollFeeForTimeOfDay(new DateTime(), null, _tollFeeRepositoryMock.Object));
            Assert.StartsWith("Vehicle cannot be null", exception.Message);
        }

        [Fact]
        public void ShouldThrowArgumentNullException_IfNoTollFeeRepositoryIsSpecified()
        {
         var exception =  Assert.Throws<ArgumentNullException>(() =>
                TollFee.GetTollFeeForTimeOfDay(new DateTime(), _vehicleMock.Object, null));

            Assert.StartsWith("TollRepository cannot be null!", exception.Message);
        }

        private int SetTollFeeTimeIntervalsInRepositoryMock()
        {
            const int tollFee = 23;
            var fromTime = new TimeOnly(10, 0);
            var untilTime = new TimeOnly(12, 0);
            var tollFeeTimeIntervals = new List<TollFeeTimeInterval>()
                { new(fromTime, untilTime, tollFee) };

            _tollFeeRepositoryMock.Setup(x => x.TollFeesForTimeIntervals)
                .Returns(tollFeeTimeIntervals);

            return tollFee;
        }
    }
}
