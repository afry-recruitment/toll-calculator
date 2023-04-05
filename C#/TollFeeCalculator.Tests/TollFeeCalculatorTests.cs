using NUnit.Framework;
using System;

namespace TollFeeCalculator.Tests
{
    public class TollFeeCalculatorTests
    {
        private TollFeeCalculator _calculator;

        [SetUp]
        public void Setup()
        {
            _calculator = new TollFeeCalculator();
        }

        [Test]
        public void GetTollFee_NoFee()
        {
            var dates = new DateTime[] { new DateTime(2023, 4, 2, 10, 0, 0) };
            int fee = _calculator.GetTollFee(dates, "Bicycle");
            Assert.AreEqual(0, fee);
        }

        [Test]
        public void GetTollFee_Weekend()
        {
            var dates = new DateTime[] { new DateTime(2023, 4, 8, 7, 0, 0) };
            int fee = _calculator.GetTollFee(dates, "Car");
            Assert.AreEqual(0, fee);
        }

        [Test]
        public void GetTollFee_HighestFee()
        {
            var dates = new DateTime[] { new DateTime(2023, 4, 5, 7, 0, 0) };
            int fee = _calculator.GetTollFee(dates, "Car");
            Assert.AreEqual(18, fee);
        }

        [Test]
        public void GetTollFee_MultipleFeesSameHour()
        {
            var dates = new DateTime[] { new DateTime(2023, 4, 5, 7, 0, 0), new DateTime(2023, 4, 5, 7, 30, 0), new DateTime(2023, 4, 5, 7, 45, 0) };
            int fee = _calculator.GetTollFee(dates, "Car");
            Assert.AreEqual(18, fee);
        }

        [Test]
        public void GetTollFee_MultipleFeesDifferentHour()
        {
            var dates = new DateTime[] { new DateTime(2023, 4, 5, 7, 0, 0), new DateTime(2023, 4, 5, 8, 0, 0), new DateTime(2023, 4, 5, 9, 0, 0) };
            int fee = _calculator.GetTollFee(dates, "Car");
            Assert.AreEqual(54, fee);
        }

        [Test]
        public void GetTollFee_MaximumFee()
        {
            var dates = new DateTime[] { new DateTime(2023, 4, 5, 7, 0, 0), new DateTime(2023, 4, 5, 8, 0, 0), new DateTime(2023, 4, 5, 9, 0, 0), new DateTime(2023, 4, 5, 10, 0, 0) };
            int fee = _calculator.GetTollFee(dates, "Car");
            Assert.AreEqual(60, fee);
        }
    }
}
