using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Moq;
using Newtonsoft.Json.Linq;
using TollFeeCalculator.Enums;
using TollFeeCalculator.Models;
using TollFeeCalculator.Models.Configuration;
using TollFeeCalculator.Services;
using TollFeeCalculator.Test.Mock;

namespace TollFeeCalculator.Test
{
    public class TollServiceTests
    {
        private readonly ITollService _tollService;

        public TollServiceTests()
        {
            var serviceProvider = new ServiceCollection()
            .AddLogging(builder => builder.AddConsole())
            .BuildServiceProvider();

            var loggerMock = serviceProvider.GetService<ILogger<TollService>>();
            var options = Options.Create<TollSettings>(new TollSettingsMock());
            _tollService = new TollService(loggerMock, new HolidayServiceMock(), options);
        }

        [Theory]
        [InlineData(VehicleTypes.Car, false)]
        [InlineData(VehicleTypes.Motorbike, true)]
        [InlineData(VehicleTypes.Tractor, true)]
        [InlineData(VehicleTypes.Emergency, true)]
        [InlineData(VehicleTypes.Diplomat, true)]
        [InlineData(VehicleTypes.Foreign, true)]
        [InlineData(VehicleTypes.Military, true)]
        public void IsTollFreeVehicle_ShouldReturnTrue(VehicleTypes vehicleType, bool expected)
        {
            var vehicle = new Vehicle(vehicleType);
            var isToolFree = _tollService.IsTollFreeVehicle(vehicle);
            Assert.Equal(expected, isToolFree);
        }


        [Theory]
        [InlineData("2025-01-01", 0)]
        [InlineData("2025-06-06", 0)]
        [InlineData("2025-12-24", 0)]
        [InlineData("2025-12-25", 0)]
        [InlineData("2025-12-31", 0)]
        public async Task GetTollFee_ForHolidays_ShouldReturnZero(string dateValue, int expectedToll)
        {
            var date = DateTime.Parse(dateValue);
            var vehicle = new Vehicle(VehicleTypes.Car);
            var tollFee = await _tollService.GetTollFee(vehicle, date);
            Assert.Equal(expectedToll, tollFee);
        }

        [Theory]
        [InlineData("2025-02-22", 0)]
        [InlineData("2025-02-23", 0)]
        public async Task GetTollFee_ForWeekends_ShouldReturnZero(string dateValue, int expectedToll)
        {
            var date = DateTime.Parse(dateValue);
            var vehicle = new Vehicle(VehicleTypes.Car);
            var tollFee = await _tollService.GetTollFee(vehicle, date);
            Assert.Equal(expectedToll, tollFee);
        }

        [Theory]
        [InlineData("2025-07-1", 0)]
        [InlineData("2025-07-22", 0)]
        public async Task GetTollFee_ForTollFreeMonth_ShouldReturnZero(string dateValue, int expectedToll)
        {
            var date = DateTime.Parse(dateValue);
            var vehicle = new Vehicle(VehicleTypes.Car);
            var tollFee = await _tollService.GetTollFee(vehicle, date);
            Assert.Equal(expectedToll, tollFee);
        }

        [Theory]
        [InlineData("2025-02-28 06:16", 8)]
        [InlineData("2025-02-28 06:30", 13)]
        [InlineData("2025-02-28 15:45", 18)]
        [InlineData("2025-02-28 21:05", 0)]
        public async Task GetTollFee_ShouldReturnFeeAccordingToTime(string dateValue, int expectedToll)
        {
            var date = DateTime.Parse(dateValue);
            var vehicle = new Vehicle(VehicleTypes.Car);
            var tollFee = await _tollService.GetTollFee(vehicle, date);
            Assert.Equal(expectedToll, tollFee);
        }

        [Theory]
        [InlineData(new[] { "2025-02-28 14:59", "2025-02-28 15:45" }, 18)]
        [InlineData(new[] { "2025-02-28 06:15", "2025-02-28 06:45" }, 13)]
        public void GetTollFee_MultipleEntriesWithinHour_ShouldOnlyReturnHighestFee(string[] dateValues, int expectedToll)
        {
            var dates = dateValues.Select(v => DateTime.Parse(v));
            var vehicle = new Vehicle(VehicleTypes.Car);
            var tollFee = _tollService.GetTollFee(vehicle, dates);
            Assert.Equal(expectedToll, tollFee);
        }

        [Theory]
        [InlineData(new[] { "2025-02-28 06:27", "2025-02-28 08:27", "2025-02-28 14:59", "2025-02-28 15:45" , "2025-02-28 17:03", "2025-02-28 18:13" }, 60)]
        public void GetTollFee_ShouldMaxCostSixtyPerDay(string[] dateValues, int expectedToll)
        {
            var dates = dateValues.Select(v => DateTime.Parse(v));
            var vehicle = new Vehicle(VehicleTypes.Car);
            var tollFee = _tollService.GetTollFee(vehicle, dates);
            Assert.Equal(expectedToll, tollFee);
        }
    }
}