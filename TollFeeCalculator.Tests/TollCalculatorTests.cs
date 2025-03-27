using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using TollFeeCalculator.Core;
using TollFeeCalculator.Pricing;
using TollFeeCalculator.Services;
using TollFeeCalculator.Vehicles;

namespace TollFeeCalculator.Tests
{
    [TestFixture]
    public class TollCalculatorTests
    {
        private TollCalculatorService _calculator;
        private IServiceProvider _serviceProvider;

        [SetUp]
        public void Setup()
        {

            var services = new ServiceCollection();

            services.AddLogging(builder =>
            {
                builder.AddDebug();
            });

            // Register your services
            services.AddTransient<ITollCalculator, TimeBasedTollCalculator>();
            services.AddTransient<ITollExemptionRulesService, TollExemptionRulesService>();
            services.AddTransient<TollCalculatorService>();
            services.AddTransient<IVehicle, Car>();

            // Build the service provider
            _serviceProvider = services.BuildServiceProvider();

            // Resolve the TollCalculator
            _calculator = _serviceProvider.GetService<TollCalculatorService>();
        }
        [TearDown]
        public void TearDown()
        {
            if (_serviceProvider is IDisposable disposable)
            {
                disposable.Dispose();
            }
        }

        [Test]
        public void GetTotalTollFee_Car_OnChristmas_ReturnsNoFee()
        {
            // Arrange
            IVehicle car = new Car();
            DateTime[] dates = { new DateTime(2025, 12, 24, 7, 30, 0) };

            // Act
            decimal totalFee = _calculator.GetTollFee(car, dates);

            // Assert
            Assert.That(totalFee, Is.EqualTo(0));
        }

        [Test]
        public void GetTotalTollFee_Car_MorningRushHour_ReturnsCorrectFee()
        {
            // Arrange
            IVehicle car = new Car();
            DateTime[] dates = { new DateTime(2025, 1, 7, 7, 30, 0) };

            // Act
            decimal totalFee = _calculator.GetTollFee(car, dates);

            // Assert
            Assert.That(totalFee, Is.EqualTo(18));
        }

        [Test]
        public void GetTotalTollFee_Motorbike_ReturnsZero()
        {
            // Arrange
            IVehicle motorbike = new Motorbike();
            DateTime[] dates = { new DateTime(2013, 1, 7, 7, 30, 0) };

            // Act
            decimal totalFee = _calculator.GetTollFee(motorbike, dates);

            // Assert
            Assert.That(totalFee, Is.EqualTo(0));

        }

        [Test]
        public void GetTotalTollFee_Car_MultiplePassesWithinOneHour_ReturnsMax18()
        {
            // Arrange
            IVehicle car = new Car();
            DateTime[] dates = {
                new DateTime(2013, 1, 7, 7, 0, 0),
                new DateTime(2013, 1, 7, 7, 15, 0),
                new DateTime(2013, 1, 7, 7, 45, 0)
            };

            // Act
            decimal totalFee = _calculator.GetTollFee(car, dates);

            // Assert
            Assert.That(totalFee, Is.EqualTo(18));
        }

        [Test]
        public void GetTotalTollFee_Car_Weekend_ReturnsZero()
        {
            // Arrange
            IVehicle car = new Car();
            DateTime[] dates = { new DateTime(2024, 01, 14, 10, 0, 0) };

            // Act
            decimal totalFee = _calculator.GetTollFee(car, dates);

            // Assert
            Assert.That(totalFee, Is.EqualTo(0));
        }

        [Test]
        public void GetTotalTollFee_Car_MultipleDays_ReturnsCorrectTotal()
        {
            // Arrange
            IVehicle car = new Car();
            DateTime[] dates = {
                new DateTime(2025, 1, 7, 7, 30, 0),
                new DateTime(2025, 1, 8, 8, 0, 0)
            };

            // Act
            decimal totalFee = _calculator.GetTollFee(car, dates);

            // Assert
            Assert.That(totalFee, Is.EqualTo(31m));
        }
        [Test]
        public void GetTotalTollFee_Car_MultipleTimes_ReturnsCorrectTotal()
        {
            // Arrange
            IVehicle car = new Car();
            DateTime[] dates = {
                new DateTime(2013, 1, 7, 7, 30, 0),
                new DateTime(2013, 1, 7, 8, 0, 0)
            };

            // Act
            decimal totalFee = _calculator.GetTollFee(car, dates);

            // Assert
            Assert.That(totalFee, Is.EqualTo(18m));
        }

        [Test]
        public void GetTotalTollFee_Car_MultipleTimesADay_ReturnsCorrectTotal()
        {
            // Arrange
            IVehicle car = new Car();
            DateTime[] dates = {
                new DateTime(2013, 1, 7, 7, 30, 0),  // Måndag, 18 kr
                new DateTime(2013, 1, 7, 8, 0, 0),   // Måndag, 13 kr
                new DateTime(2013, 1, 7, 11, 30, 0),
                new DateTime(2013, 1, 7, 12, 32, 0),
                new DateTime(2013, 1, 7, 14, 30, 0),
                new DateTime(2013, 1, 7, 16, 09, 0),
                new DateTime(2013, 1, 7, 17, 18, 0),
                new DateTime(2013, 1, 7, 18, 59, 0),



            };

            // Act
            decimal totalFee = _calculator.GetTollFee(car, dates);

            // Assert
            Assert.That(totalFee, Is.EqualTo(60m));
        }

        [Test]
        public void GetTotalTollFee_Car_MultipleMaxValue_ReturnsCorrectTotal()
        {
            // Arrange
            IVehicle car = new Car();
            DateTime[] dates = {
                new DateTime(2013, 1, 7, 7, 30, 0),
                new DateTime(2013, 1, 7, 8, 0, 0),
                new DateTime(2013, 1, 7, 11, 30, 0),
                new DateTime(2013, 1, 7, 12, 32, 0),
                new DateTime(2013, 1, 7, 14, 30, 0),
                new DateTime(2013, 1, 7, 16, 09, 0),
                new DateTime(2013, 1, 7, 17, 18, 0),
                new DateTime(2013, 1, 7, 18, 59, 0),
                new DateTime(2013, 1, 9, 7, 30, 0),
                new DateTime(2013, 1, 9, 8, 0, 0),
                new DateTime(2013, 1, 9, 11, 30, 0),
                new DateTime(2013, 1, 9, 12, 32, 0),
                new DateTime(2013, 1, 9, 14, 30, 0),
                new DateTime(2013, 1, 9, 16, 09, 0),
                new DateTime(2013, 1, 9, 17, 18, 0),
                new DateTime(2013, 1, 9, 18, 59, 0),


            };

            // Act
            decimal totalFee = _calculator.GetTollFee(car, dates);

            // Assert
            Assert.That(totalFee, Is.EqualTo(120m));
        }
    }
}