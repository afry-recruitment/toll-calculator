using System;
using NUnit.Framework;

namespace TollFeeCalculator.UnitTests
{
    [TestFixture]
    public class TollCalculatorTests
    {
        private TollCalculator _tollCalculator;
        private IVehicle _vehicle;

        public static DateTime[][] listSameDates = new DateTime[][]
        {
            new DateTime[]
            {
                new DateTime(2022, 11, 17, 14, 30, 25),
                new DateTime(2022, 11, 17, 15, 10, 25)
            },
            new DateTime[]
            {
                new DateTime(2022, 11, 17, 08, 15, 25),
                new DateTime(2022, 11, 17, 08, 30, 25)
            },
        new DateTime[]
            {
                new DateTime(2022, 11, 17, 15, 15, 25)
            }
        };

        public static DateTime[][] listDiffDates = new DateTime[][]
        {
            new DateTime[]
            {
                new DateTime(2022, 11, 17, 06, 15, 25),
                new DateTime(2022, 11, 17, 07, 30, 25),
                new DateTime(2022, 11, 17, 15, 55, 25),
                new DateTime(2022, 11, 17, 19, 01, 25)
            },
            new DateTime[]
            {
                new DateTime(2022, 11, 17, 13, 45, 25),
                new DateTime(2022, 11, 17, 16, 15, 25),
                new DateTime(2022, 11, 17, 16, 59, 25)
            },
        };

        public static DateTime[][] listDates = new DateTime[][]
        {
            new DateTime[]
            {
                new DateTime(2022, 11, 17, 07, 50, 25),
                new DateTime(2022, 11, 17, 15, 35, 25),
                new DateTime(2022, 11, 17, 16, 30, 25),
                new DateTime(2022, 11, 17, 16, 59, 25)
            }
        };

        [SetUp]
        public void SetUp()
        {
            _tollCalculator = new TollCalculator();
            _vehicle = new Car();
        }

        [TestCase("11/25/2022 06:29:11")]
        [TestCase("11/25/2022 08:30:11")]
        [TestCase("11/25/2022 09:30:11")]
        [TestCase("11/25/2022 10:30:11")]
        [TestCase("11/25/2022 11:30:11")]
        [TestCase("11/25/2022 12:30:11")]
        [TestCase("11/25/2022 13:30:11")]
        [TestCase("11/25/2022 14:30:11")]
        [TestCase("11/25/2022 18:15:11")]
        public void GetTollFee_ReturnsEight(DateTime date)
        {
            var result = _tollCalculator.GetTollFee(date, _vehicle);
            Assert.AreEqual(8, result);
        }

        [TestCase("11/25/2022 06:59:11")]
        [TestCase("11/25/2022 08:15:25")]
        [TestCase("11/25/2022 15:25:25")]
        [TestCase("11/25/2022 17:45:25")]
        public void GetTollFee_ReturnsThirteen(DateTime date)
        {
            var result = _tollCalculator.GetTollFee(date, _vehicle);
            Assert.AreEqual(13, result);
        }

        [TestCase("11/25/2022 07:30:25")]
        [TestCase("11/25/2022 15:30:25")]
        [TestCase("11/25/2022 16:30:25")]
        public void GetTollFee_ReturnsEighteen(DateTime date)
        {
            var result = _tollCalculator.GetTollFee(date, _vehicle);
            Assert.AreEqual(18, result);
        }

        [TestCase("11/26/2022 16:30:25")]
        [TestCase("11/27/2022 09:35:25")]
        [TestCase("01/01/2013 09:35:25")]
        [TestCase("03/29/2013 09:35:25")]
        [TestCase("05/01/2013 09:35:25")]
        [TestCase("06/06/2013 09:35:25")]
        [TestCase("07/06/2013 09:35:25")]
        [TestCase("07/25/2013 09:35:25")]
        [TestCase("12/25/2013 09:35:25")]
        public void GetTollFee_FreeDate(DateTime date)
        {
            var result = _tollCalculator.GetTollFee(date, _vehicle);
            Assert.AreEqual(0, result);
        }

        [TestCase("11/26/2022 16:30:25")]
        public void GetTollFee_FreeMotorbike(DateTime date)
        {
            var result = _tollCalculator.GetTollFee(date, new Motorbike());
            Assert.AreEqual(0, result);
        }

        [Test, TestCaseSource("listSameDates")]
        public void GetTollFee_MoreThanOnceEveryHour(DateTime[] listSameHourDates)
        {
            var result = _tollCalculator.GetTollFee(_vehicle, listSameHourDates);
            Assert.AreEqual(13, result);
        }

        [Test, TestCaseSource("listDiffDates")]
        public void GetTollFee_OnceEveryHour(DateTime[] listDiffDates)
        {
            var result = _tollCalculator.GetTollFee(_vehicle, listDiffDates);
            Assert.AreEqual(44, result);
        }

        [Test, TestCaseSource("listDates")]
        public void GetTollFee_MaxSixty(DateTime[] listDates)
        {
            var result = _tollCalculator.GetTollFee(_vehicle, listDates);
            Assert.AreEqual(60, result);
        }
    }
}
