using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using TollFeeCalculator;
using vs_cs;

namespace Tests
{
    [TestClass()]
    public class TollCalculatorTests
    {
        /// <summary>
        /// Test getting the total toll fee for a car passing through multiple tolls.
        /// </summary>
        [TestMethod()]
        [DataRow(1, 0)] // jan 1, 2013 was a holiday => toll free
        [DataRow(2, 31)] // jan 2, 2013 was not a holiday nor weekend
        [DataRow(5, 0)] // jan 5, 2013 was a saturday => toll free
        [DataRow(6, 0)] // jan 6, 2013 was a sunday => toll free
        public void GetTotalTollFeeTest(int day, int expectedTotalFee)
        {
            var vehicle = new Car();

            var year = 2013;
            var month = 1;

            var tollPasses = new DateTime[]
            {
            new DateTime(year, month, day, 6, 0, 0), // fee is 8, total is 8
            new DateTime(year, month, day, 6, 30, 0), // fee is 13 but within the hour since last fee, total updated to 13
            new DateTime(year, month, day, 7, 0, 0), // fee is 18 but within the hour since last fee, total is now 18
            new DateTime(year, month, day, 8, 0, 0), // fee is 13, total is now 18+13
            new DateTime(year, month, day, 8, 30, 0), // fee is 8 but within the hour, total is still 18+13
            // total toll fee should be 31
            };

            var actualTotalFee = TollCalculator.GetTotalTollFee(vehicle, tollPasses);

            Assert.AreEqual(expectedTotalFee, actualTotalFee);
        }

        /// <summary>
        /// Test getting the total toll fee for a diplomat vehicle passing through multiple tolls.
        /// </summary>
        [TestMethod()]
        [DataRow(1)] // jan 1, 2013 was a holiday, toll free
        [DataRow(2)] // jan 2, 2013 was not a holiday nor weekend
        [DataRow(5)] // jan 5, 2013 was a saturday, toll free
        [DataRow(6)] // jan 6, 2013 was a sunday, toll free
        public void GetTotalTollFeeTest_WhenVehicleIsDiplomat(int day, int expectedTotalFee = 0)
        {
            var vehicle = new Diplomat();

            var year = 2013;
            var month = 1;

            var tollPasses = new DateTime[]
            {
            new DateTime(year, month, day, 6, 0, 0), // fee is 8, total is 8
            new DateTime(year, month, day, 6, 30, 0), // fee is 13 but within the hour since last fee, total updated to 13
            new DateTime(year, month, day, 7, 0, 0), // fee is 18 but within the hour since last fee, total is now 18
            new DateTime(year, month, day, 8, 0, 0), // fee is 13, total is now 18+13
            new DateTime(year, month, day, 8, 30, 0), // fee is 8 but within the hour, total is still 18+13
            // total toll fee should be 31
            };

            var actualTotalFee = TollCalculator.GetTotalTollFee(vehicle, tollPasses);

            Assert.AreEqual(expectedTotalFee, actualTotalFee);
        }

        /// <summary>
        /// Test getting the total toll fee for a car passing through multiple tolls does 
        /// not total past the max amount of toll fee for a day.
        /// </summary>
        [TestMethod()]
        public void GetTotalTollFeeTest_ShouldNotTotalGreaterThanMaxPerDay()
        {
            var vehicle = new Car();

            var year = 2013;
            var month = 1;
            int day = 2;
            var tollPasses = new DateTime[]
            {
            new DateTime(year, month, day, 6, 0, 0), // fee is 8, total is 8
            new DateTime(year, month, day, 6, 30, 0), // fee is 13 but within the hour since last fee, total updated to 13
            new DateTime(year, month, day, 7, 0, 0), // fee is 18 but within the hour since last fee, total is now 18
            new DateTime(year, month, day, 8, 0, 0), // fee is 13, total is now 18+13
            new DateTime(year, month, day, 8, 30, 0), // fee is 8 but within the hour, total is still 18+13
            new DateTime(year, month, day, 9, 45, 0), // 18+13+8 = 39
            new DateTime(year, month, day, 11, 30, 0), // 18+13+8+8 = 47
            new DateTime(year, month, day, 12, 31, 0), // 18+13+8+8+8 = 55
            new DateTime(year, month, day, 13, 32, 0), // 18+13+8+8+8+8 = 63
            new DateTime(year, month, day, 14, 33, 0), // 18+13+8+8+8+8+8 = 71
            };

            int expectedTotalMaxFeePerDay = 60;
            var actualTotalFee = TollCalculator.GetTotalTollFee(vehicle, tollPasses);

            Assert.AreEqual(expectedTotalMaxFeePerDay, actualTotalFee);
        }

        /// <summary>
        /// Test the getting the toll fee passing through a single toll. 
        /// </summary>
        [TestMethod()]
        [DataRow(6, 0, 8)]
        [DataRow(6, 30, 13)]
        [DataRow(7, 0, 18)]
        [DataRow(8, 0, 13)]
        [DataRow(8, 30, 8)]
        [DataRow(11, 30, 8)]
        [DataRow(12, 31, 8)]
        [DataRow(13, 32, 8)]
        [DataRow(14, 33, 8)]
        public void GetTollFeeTest(int hour, int minute, int expectedTollFee)
        {
            var year = 2013;
            var month = 1;
            var day = 2;
            var second = 0;

            var dateTime = new DateTime(year, month, day, hour, minute, second);
            var vehicle = new Car();

            var actualTollFee = TollCalculator.GetTollFee(dateTime, vehicle);

            Assert.AreEqual(expectedTollFee, actualTollFee);
        }

        /// <summary>
        /// Test the getting the toll fee passing through a single toll on swedish holidays. 
        /// </summary>
        [TestMethod()]
        [DataRow(2013, 5, 1)] // första maj
        [DataRow(2013, 6, 6)] // sveriges nationaldag
        public void GetTollFeeTest_WhenSwedishHolidays_ShouldReturnTollFree(int year, int month, int day)
        {
            var expectedTollFee = 0;
            var hour = 6;
            var minute = 0;
            var second = 0;

            var dateTime = new DateTime(year, month, day, hour, minute, second);
            var vehicle = new Car();

            var actualTollFee = TollCalculator.GetTollFee(dateTime, vehicle);

            Assert.AreEqual(expectedTollFee, actualTollFee);
        }
    }
}