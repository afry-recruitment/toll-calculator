using FluentAssertions;
using System;
using Xunit;

namespace TollCalculator.UnitTest
{
    public class CarTollCalculatorShould
    {
        private readonly CarTollCalculator carTollCalculator;

        public CarTollCalculatorShould()
        {
            this.carTollCalculator = new CarTollCalculator(new Car());
        }

        [Fact]
        public void CalculateOnWeekend()
        {
            var dates = new DateTime[] { new DateTime(2022, 06, 04) };
            var tollFees = TimeToTollFeePopulator.InitializeTollFees();
            var totalToll = this.carTollCalculator.GetTollFee(dates, tollFees);

            totalToll.Should().Be(0);
        }

        [Fact]
        public void CalculateOnHoliday()
        {
            var dates = new DateTime[] { new DateTime(2013, 06, 06) };
            var tollFees = TimeToTollFeePopulator.InitializeTollFees();
            var totalToll = this.carTollCalculator.GetTollFee(dates, tollFees);

            totalToll.Should().Be(0);
        }

        [Fact]
        public void CalculateOnNoDatesPassed()
        {
            var dates = new DateTime[] {};
            var tollFees = TimeToTollFeePopulator.InitializeTollFees();
            Assert.Throws<ArgumentException>(() => this.carTollCalculator.GetTollFee(null, tollFees));
        }

        [Fact]
        public void CalculateOnNullDates()
        {
            var tollFees = TimeToTollFeePopulator.InitializeTollFees();
            Assert.Throws<ArgumentException>( () => this.carTollCalculator.GetTollFee(null, tollFees));
        }

        [Fact]
        public void CalculateOnTwoDateTimeIntervalLessThanOneHour()
        {
            var dates = new DateTime[] { new DateTime(2022, 06, 06, 06, 30, 00), new DateTime(2022, 06, 06, 07, 29, 59) };
            var tollFees = TimeToTollFeePopulator.InitializeTollFees();
            var totalToll = this.carTollCalculator.GetTollFee(dates, tollFees);
            totalToll.Should().Be(31);
        }

        [Fact]
        public void CalculateWhereTollFeeMaxIs60()
        {
            var dates = new DateTime[] { 
                new DateTime(2022, 06, 06, 06, 30, 00), 
                new DateTime(2022, 06, 06, 07, 29, 59),
                new DateTime(2022, 06, 06, 08, 29, 59),
                new DateTime(2022, 06, 06, 09, 29, 59),
                new DateTime(2022, 06, 06, 10, 29, 59),
                new DateTime(2022, 06, 06, 11, 29, 59)};
            var tollFees = TimeToTollFeePopulator.InitializeTollFees();
            var totalToll = this.carTollCalculator.GetTollFee(dates, tollFees);
            totalToll.Should().Be(60);
        }

        [Fact]
        public void CalculateOnMultipleDays()
        {
            var dates = new DateTime[] {
                new DateTime(2022, 06, 06, 06, 30, 00),
                new DateTime(2022, 06, 06, 07, 29, 59),
                new DateTime(2022, 06, 06, 08, 29, 59),
                new DateTime(2022, 06, 06, 09, 29, 59),
                new DateTime(2022, 06, 06, 10, 29, 59),
                new DateTime(2022, 06, 06, 11, 29, 59),
                new DateTime(2022, 06, 07, 06, 30, 00),
                new DateTime(2022, 06, 07, 07, 29, 59),
                new DateTime(2022, 06, 07, 08, 29, 59),
                new DateTime(2022, 06, 07, 09, 29, 59),
                new DateTime(2022, 06, 07, 10, 29, 59),
                new DateTime(2022, 06, 07, 11, 29, 59)};
            var tollFees = TimeToTollFeePopulator.InitializeTollFees();
            var totalToll = this.carTollCalculator.GetTollFee(dates, tollFees);
            totalToll.Should().Be(120);
        }
    }
}
