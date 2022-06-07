using FluentAssertions;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace TollCalculator.UnitTest
{
    public class TollFeeCalculatorShould
    {
        private readonly TollFeeCalculator tollFeeCalculator;

        public TollFeeCalculatorShould()
        {
            this.tollFeeCalculator = new TollFeeCalculator();
        }

        [Fact]
        public void ThrowErrorWhenEmptyDatePassed()
        {
            var dates = new DateTime[] { };
            Assert.Throws<ArgumentException>(() => this.tollFeeCalculator.GetTollFee(new Car(), dates));
        }

        [Fact]
        public void ThrowErrorWhenEmptyArrayOfDatesPassed()
        {
            var dates = new DateTime[] { };
            Assert.Throws<ArgumentException>(() => this.tollFeeCalculator.GetTollFee(new Car(), null));
        }

        [Fact]
        public void ReturnValueWhenCarVehiclePassed()
        {
            var dates = new DateTime[] { new DateTime(2022, 06, 07, 06, 30, 0)};
            var tollFee = this.tollFeeCalculator.GetTollFee(new Car(), dates);
            tollFee.Should().NotBe(null);
        }

        [Fact]
        public void ReturnValueWhenDiplomatVehiclePassed()
        {
            var dates = new DateTime[] { new DateTime(2022, 06, 07, 06, 30, 0) };
            var tollFee = this.tollFeeCalculator.GetTollFee(new Diplomat(), dates);
            tollFee.Should().NotBe(null);
        }

        [Fact]
        public void ReturnValueWhenEmergencyVehiclePassed()
        {
            var dates = new DateTime[] { new DateTime(2022, 06, 07, 06, 30, 0) };
            var tollFee = this.tollFeeCalculator.GetTollFee(new Emergency(), dates);
            tollFee.Should().NotBe(null);
        }

        [Fact]
        public void ReturnValueWhenForeignVehiclePassed()
        {
            var dates = new DateTime[] { new DateTime(2022, 06, 07, 06, 30, 0) };
            var tollFee = this.tollFeeCalculator.GetTollFee(new Foreign(), dates);
            tollFee.Should().NotBe(null);
        }

        [Fact]
        public void ReturnValueWhenMilitaryVehiclePassed()
        {
            var dates = new DateTime[] { new DateTime(2022, 06, 07, 06, 30, 0) };
            var tollFee = this.tollFeeCalculator.GetTollFee(new Military(), dates);
            tollFee.Should().NotBe(null);
        }

        [Fact]
        public void ReturnValueWhenMotorbikeVehiclePassed()
        {
            var dates = new DateTime[] { new DateTime(2022, 06, 07, 06, 30, 0) };
            var tollFee = this.tollFeeCalculator.GetTollFee(new Motorbike(), dates);
            tollFee.Should().NotBe(null);
        }

        [Fact]
        public void ReturnValueWhenTractorVehiclePassed()
        {
            var dates = new DateTime[] { new DateTime(2022, 06, 07, 06, 30, 0) };
            var tollFee = this.tollFeeCalculator.GetTollFee(new Tractor(), dates);
            tollFee.Should().NotBe(null);
        }
    }
}
