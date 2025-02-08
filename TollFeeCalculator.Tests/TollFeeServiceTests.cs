using Moq;
using TollFeeCalculator.Application.Services;
using TollFeeCalculator.Domain.Entities;
using TollFeeCalculator.Domain.Enums;
using TollFeeCalculator.Infrastructure.Interfaces;

namespace TollFeeCalculator.Tests
{
    public class TollFeeServiceTests
    {
        private readonly Mock<ITollRepository> _tollRepositoryMock;
        private readonly TollFeeService _tollFeeService;

        public TollFeeServiceTests()
        {
            _tollRepositoryMock = new Mock<ITollRepository>();
            _tollFeeService = new TollFeeService(_tollRepositoryMock.Object);
        }

        [Fact]
        public void CalculateTollFee_ShouldReturnZero_WhenVehicleIsTollFree()
        {
            var vehicle = new TestVehicle(VehicleType.Motorbike);
            DateTime[] passageTimes = { new(2025, 02, 07, 7, 30, 0) };

            int result = _tollFeeService.CalculateTollFee(vehicle, passageTimes);

            Assert.Equal(0, result);
        }

        [Fact]
        public void CalculateTollFee_ShouldReturnZero_WhenDateIsHoliday()
        {
            var vehicle = new TestVehicle(VehicleType.Car);
            DateTime[] passageTimes = { new(2025, 12, 25, 8, 15, 0) }; // Christmas (Holiday)

            _tollRepositoryMock.Setup(repo => repo.IsTollFreeDate(It.IsAny<DateTime>())).Returns(true);

            int result = _tollFeeService.CalculateTollFee(vehicle, passageTimes);

            Assert.Equal(0, result);
        }

        [Fact]
        public void CalculateTollFee_ShouldReturnCorrectFee_WhenMultipleTimesInSameHour()
        {
            var vehicle = new TestVehicle(VehicleType.Car);
            DateTime[] passageTimes =
            {
                new(2025, 02, 07, 6, 15, 0), // 8 SEK
                new(2025, 02, 07, 6, 45, 0), // 13 SEK (higher, replaces previous)
                new(2025, 02, 07, 7, 30, 0), // 18 SEK (new hour)
            };

            _tollRepositoryMock.Setup(repo => repo.GetTollFeeByTime(It.Is<DateTime>(d => d.Hour == 6))).Returns(8);
            _tollRepositoryMock.Setup(repo => repo.GetTollFeeByTime(It.Is<DateTime>(d => d.Hour == 6 && d.Minute >= 30))).Returns(13);
            _tollRepositoryMock.Setup(repo => repo.GetTollFeeByTime(It.Is<DateTime>(d => d.Hour == 7))).Returns(18);

            int result = _tollFeeService.CalculateTollFee(vehicle, passageTimes);

            Assert.Equal(31, result); // 13 (6:xx max) + 18 (7:30)
        }

        [Fact]
        public void CalculateTollFee_ShouldCapAtMaximumFeePerDay()
        {
            var vehicle = new TestVehicle(VehicleType.Car);
            DateTime[] passageTimes =
            {
                new(2025, 02, 07, 6, 15, 0), // 8 SEK
                new(2025, 02, 07, 7, 30, 0), // 18 SEK
                new(2025, 02, 07, 8, 30, 0), // 13 SEK
                new(2025, 02, 07, 9, 30, 0), // 8 SEK
                new(2025, 02, 07, 10, 30, 0), // 8 SEK
                new(2025, 02, 07, 11, 30, 0), // 8 SEK
                new(2025, 02, 07, 12, 30, 0), // 8 SEK
                new(2025, 02, 07, 13, 30, 0), // 8 SEK
                new(2025, 02, 07, 14, 30, 0), // 8 SEK
                new(2025, 02, 07, 15, 30, 0), // 8 SEK
                new(2025, 02, 07, 16, 30, 0), // 8 SEK
                new(2025, 02, 07, 17, 30, 0), // 13 SEK
                new(2025, 02, 07, 18, 30, 0), // 8 SEK
            };

            _tollRepositoryMock.Setup(repo => repo.GetTollFeeByTime(It.IsAny<DateTime>())).Returns(8);
            _tollRepositoryMock.Setup(repo => repo.GetTollFeeByTime(It.Is<DateTime>(d => d.Hour == 7))).Returns(18);
            _tollRepositoryMock.Setup(repo => repo.GetTollFeeByTime(It.Is<DateTime>(d => d.Hour == 8))).Returns(13);
            _tollRepositoryMock.Setup(repo => repo.GetTollFeeByTime(It.Is<DateTime>(d => d.Hour == 17))).Returns(13);

            int result = _tollFeeService.CalculateTollFee(vehicle, passageTimes);

            Assert.Equal(60, result); // Max daily fee
        }

        [Fact]
        public void CalculateTollFee_ShouldReturnZero_OnWeekends()
        {
            var vehicle = new TestVehicle(VehicleType.Car);
            DateTime[] passageTimes = { new(2025, 02, 09, 9, 30, 0) }; // Saturday

            _tollRepositoryMock.Setup(repo => repo.IsTollFreeDate(It.IsAny<DateTime>())).Returns(true);

            int result = _tollFeeService.CalculateTollFee(vehicle, passageTimes);

            Assert.Equal(0, result);
        }

        [Fact]
        public void CalculateTollFee_ShouldApplyCorrectFees_ForDifferentTimePeriods()
        {
            var vehicle = new TestVehicle(VehicleType.Car);
            DateTime[] passageTimes =
            {
                new(2025, 2, 06, 6, 00, 0), // 8 SEK
                new(2025, 2, 06, 6, 30, 0), // 8 SEK
                new(2025, 2, 06, 7, 10, 0), // 18 SEK
                new(2025, 2, 06, 8, 10, 0), // 13 SEK
                new(2025, 2, 06, 15, 30, 0), // 8 SEK
                new(2025, 2, 06, 18, 15, 0), // 8 SEK
            };

            _tollRepositoryMock.Setup(repo => repo.GetTollFeeByTime(It.Is<DateTime>(d => d.Hour == 6))).Returns(8);
            _tollRepositoryMock.Setup(repo => repo.GetTollFeeByTime(It.Is<DateTime>(d => d.Hour == 7))).Returns(18);
            _tollRepositoryMock.Setup(repo => repo.GetTollFeeByTime(It.Is<DateTime>(d => d.Hour == 8))).Returns(13);
            _tollRepositoryMock.Setup(repo => repo.GetTollFeeByTime(It.Is<DateTime>(d => d.Hour == 15))).Returns(8);
            _tollRepositoryMock.Setup(repo => repo.GetTollFeeByTime(It.Is<DateTime>(d => d.Hour == 18))).Returns(8);

            int result = _tollFeeService.CalculateTollFee(vehicle, passageTimes);

            Assert.Equal(55, result); // 8 + 8 + 18 + 13 + 8 + 8
        }
    }

    public class TestVehicle : Vehicle
    {
        public TestVehicle(VehicleType vehicleType) : base(vehicleType) { }
    }
}
