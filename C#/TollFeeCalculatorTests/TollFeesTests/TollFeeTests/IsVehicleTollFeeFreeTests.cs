using Moq;
using TollFeeCalculator.TollFees;
using TollFeeCalculator.Vehicles;

namespace TollFeeCalculatorTests.TollFeesTests.TollFeeTests
{
    public class IsVehicleTollFeeFreeTests
    {
        private readonly Mock<ITollFeeRepository> _tollFeeRepositoryMock = new();
        private readonly Mock<IVehicle> _vehicleMock = new();

        public IsVehicleTollFeeFreeTests()
        {
            _tollFeeRepositoryMock.DefaultValue = DefaultValue.Mock;
            SetTollFeeFreeVehiclesInRepositoryMock();

            _vehicleMock.DefaultValue = DefaultValue.Mock;
        }

        [Fact]
        public void ShouldReturnTrue_IfVehicleClassificationIsTollFeeFree()
        {
            _vehicleMock.Setup(v => v.VehicleClassification)
                .Returns(VehicleClassification.Emergency);
            _vehicleMock.Setup(v => v.VehicleType)
                .Returns(VehicleType.Car);

            var result = TollFee.IsVehicleTollFeeFree(_vehicleMock.Object, _tollFeeRepositoryMock.Object);

            Assert.True(result);
        }
        
        [Fact]
        public void ShouldReturnTrue_IfVehicleTypeIsTollFeeFree()
        {
            _vehicleMock.Setup(v => v.VehicleType)
                .Returns(VehicleType.Motorbike);
            _vehicleMock.Setup(v => v.VehicleClassification)
                .Returns(VehicleClassification.Standard);

            var result = TollFee.IsVehicleTollFeeFree(_vehicleMock.Object, _tollFeeRepositoryMock.Object);

            Assert.True(result);
        }

        [Fact]
        public void ShouldReturnFalse_IfVehicleIsNotTollFeeFree()
        {
            _vehicleMock.Setup(v => v.VehicleClassification)
                .Returns(VehicleClassification.Standard);
            _vehicleMock.Setup(v => v.VehicleType)
                .Returns(VehicleType.Car);

            var result = TollFee.IsVehicleTollFeeFree(_vehicleMock.Object, _tollFeeRepositoryMock.Object);

            Assert.False(result);
        }

        [Fact]
        public void ShouldThrowArgumentNullException_IfNoVehicleIsSpecified()
        {
            var exception = Assert.Throws<ArgumentNullException>(() =>
                TollFee.IsVehicleTollFeeFree(null, _tollFeeRepositoryMock.Object));

            Assert.StartsWith("Vehicle cannot be null", exception.Message);
        }

        [Fact]
        public void ShouldThrowArgumentNullException_IfNoTollFeeRepositoryIsSpecified()
        {
            var exception = Assert.Throws<ArgumentNullException>(() =>
                TollFee.IsVehicleTollFeeFree(_vehicleMock.Object, null));

            Assert.StartsWith("TollRepository cannot be null!", exception.Message);
        }


        private void SetTollFeeFreeVehiclesInRepositoryMock()
        {
            var tollFreeVehicleClassifications = new List<VehicleClassification>()
                {
                    VehicleClassification.Diplomat,
                    VehicleClassification.Emergency,
                    VehicleClassification.Foreign,
                    VehicleClassification.Military,
                };

            var tollFreeVehicleTypes = new List<VehicleType>()
                {
                    VehicleType.Motorbike,
                    VehicleType.Tractor,
                };

            _tollFeeRepositoryMock.Setup(x => x.TollFeeFreeVehicleClassifications)
                .Returns(tollFreeVehicleClassifications);
            _tollFeeRepositoryMock.Setup(x => x.TollFeeFreeVehicleTypes)
                .Returns(tollFreeVehicleTypes);
        }

    }
}
