using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TollFeeCalculator;

namespace TollCalculatorTests
{
    [TestClass]
    public class TollCalculatorTests
    {
        private readonly TollCalculator _calculator = new TollCalculator();

        [TestMethod]
        public void SingleEntry_ReturnsThatFee()
        {
            IVehicle vehicle = new Car();
            var dates = new[] { new DateTime(2025, 8, 18, 6, 44, 0) };

            int result = _calculator.GetDailyTollFee(vehicle, dates);

            Assert.AreEqual(13, result);
        }

        [TestMethod]
        public void MultipleWithinAnHour_TakesHighestFee()
        {
            IVehicle vehicle = new Car();
            var dates = new[]
            {
                new DateTime(2025, 8, 18, 6, 0, 0),
                new DateTime(2025, 8, 18, 6, 29, 0),
                new DateTime(2025, 8, 18, 6, 44, 0),
            };

            int result = _calculator.GetDailyTollFee(vehicle, dates);

            Assert.AreEqual(13, result);
        }

        [TestMethod]
        public void SeparateWindows_AddsBothMaxFees()
        {
            IVehicle vehicle = new Car();
            var dates = new[]
            {
                new DateTime(2025, 8, 18, 6, 0, 0),
                new DateTime(2025, 8, 18, 7, 30, 0)
            };

            int result = _calculator.GetDailyTollFee(vehicle, dates);

            Assert.AreEqual(8 + 18, result);
        }

        [TestMethod]
        public void DailyCap_IsApplied()
        {
            IVehicle vehicle = new Car();
            var dates = new[]
            {
                new DateTime(2025, 8, 18, 6, 31, 0),
                new DateTime(2025, 8, 18, 7, 46, 0),
                new DateTime(2025, 8, 18, 15, 01, 0),
                new DateTime(2025, 8, 18, 16, 37, 0),
            };

            int result = _calculator.GetDailyTollFee(vehicle, dates);

            Assert.AreEqual(60, result); // capped at 60 but adds up to 62
        }

        [TestMethod]
        public void TollFreeVehicle()
        {
            IVehicle vehicle = new Motorbike();
            var dates = new[]
            {
                new DateTime(2025, 8, 18, 6, 0, 0),
                new DateTime(2025, 8, 18, 7, 30, 0),
                new DateTime(2025, 8, 18, 9, 0, 0),
                new DateTime(2025, 8, 18, 15, 0, 0),
            };

            int result = _calculator.GetDailyTollFee(vehicle, dates);
            Assert.AreEqual(0, result);
        }

        [TestMethod]
        public void SingleEntry_Weekend()
        {
            IVehicle vehicle = new Car();
            var dates = new[] { new DateTime(2025, 8, 17, 6, 44, 0) };

            int result = _calculator.GetDailyTollFee(vehicle, dates);

            Assert.AreEqual(0, result); //Sunday
        }
    }
}
