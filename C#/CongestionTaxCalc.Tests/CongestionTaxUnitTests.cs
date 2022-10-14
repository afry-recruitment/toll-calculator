using NUnit.Framework;
using System;

namespace CongestionTaxCalc.Tests
{
    public class Tests
    {
        private CongestionTaxCalculator congestionTaxCalculator;

        [SetUp]
        public void Setup()
        {
            congestionTaxCalculator = new CongestionTaxCalculator();
        }
        [Test]
        [TestCase("Car")]
        public void TestCongentionTaxForCarSingleToll(string vehicle)
        {

            var arrdates = new DateTime[1];
            arrdates[0] = Convert.ToDateTime("2013-03-22 14:07:27");
            var result = congestionTaxCalculator.GetTax(vehicle, arrdates);
            Assert.AreEqual(8, result);
        }
        [Test]
        [TestCase("Car")]
        public void TestCongentionTaxForCarMultipleTolls(string vehicle)
        {

            var arrdates = new DateTime[2];
            arrdates[0] = Convert.ToDateTime("2013-02-07 06:23:27");
            arrdates[1] = Convert.ToDateTime("2013-02-07 15:27:00");
            var result = congestionTaxCalculator.GetTax(vehicle, arrdates);
            Assert.AreEqual(21, result);
        }

        [Test]
        [TestCase("Car")]
        public void TestCongentionTaxForCarMultipleTollsWithinOneHour(string vehicle)
        {
            var arrdates = new DateTime[5];
            arrdates[0] = Convert.ToDateTime("2013-02-08 06:27:00");
            arrdates[1] = Convert.ToDateTime("2013-02-08 06:20:27");
            arrdates[2] = Convert.ToDateTime("2013-02-08 14:35:00");
            arrdates[3] = Convert.ToDateTime("2013-02-08 15:29:00");
            arrdates[4] = Convert.ToDateTime("2013-02-08 18:35:00");
            var result = congestionTaxCalculator.GetTax(vehicle, arrdates);
            Assert.AreEqual(21, result);
        }
        [Test]
        [TestCase("Foreign")]
        public void TestCongentionTaxForTollExemptedVehicle(string vehicle)
        {
            var arrdates = new DateTime[1];
            arrdates[0] = Convert.ToDateTime("2013-02-08 16:01:00");
            var result = congestionTaxCalculator.GetTax(vehicle, arrdates);
            Assert.AreEqual(0, result);
        }
        [Test]
        [TestCase("Car")]
        public void TestCongentionTaxForWeekend(string vehicle)
        {
            var arrdates = new DateTime[1];
            arrdates[0] = Convert.ToDateTime("2013-02-10 11:15:00");        //Sunday
            var result = congestionTaxCalculator.GetTax(vehicle, arrdates);
            Assert.AreEqual(0, result);
        }
    }
}