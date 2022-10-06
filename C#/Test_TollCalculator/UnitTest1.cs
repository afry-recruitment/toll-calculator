using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TollFeeCalculator;


namespace Test_TollCaclulator
{
    [TestClass]
    public class TollFeeCalculatorTest
    {

        TollCalculator tollfeeC;

        //TollCalculator can be accessed from every test

        [TestInitialize]
        public void MyInitialize()
        {
            tollfeeC = new TollCalculator();
        }

        [TestMethod]
        public void Test_minimum_Fee_Car()
        {
            Assert.AreEqual(8, tollfeeC.GetTollFee(new Car(), new DateTime[] {
                new DateTime(2013, 4, 10, 11, 21, 0),
                new DateTime(2013, 4, 10, 06, 15, 0)}));
        }

        [TestMethod]
        public void Test_Fee_Once_An_Hour()
        {
            Assert.AreEqual(18, tollfeeC.GetTollFee(new Car(), new DateTime[] {
                new DateTime(2015, 4, 10, 7, 30, 0),
                new DateTime(2015, 4, 10, 7, 58, 0),
                new DateTime(2015, 4, 10, 7, 15, 0) }));
        }

        [TestMethod]
        public void Test_Rush_Hour_HighestFee()
        {
            Assert.AreEqual(18, tollfeeC.GetTollFee(new Car(), new DateTime[] {
                new DateTime(2015, 4, 10, 7, 15, 0) }));
        }

        [TestMethod]
        public void Test_Maximum_Fee_Car_One_Day()
        {
            Assert.AreEqual(60, tollfeeC.GetTollFee(new Car(), new DateTime[] {
                new DateTime(2020, 1, 7, 6, 0, 0),
                new DateTime(2020, 1, 7, 7, 0, 0),
                new DateTime(2020, 1, 7, 8, 0, 0),
                new DateTime(2020, 1, 7, 9, 0, 0),
                new DateTime(2020, 1, 7, 10, 0, 0),
                new DateTime(2020, 1, 7, 11, 0, 0),
                new DateTime(2020, 1, 7, 12, 0, 0),
                new DateTime(2020, 1, 7, 13, 0, 0),
                new DateTime(2020, 1, 7, 14, 0, 0),
                new DateTime(2020, 1, 7, 15, 0, 0),
                new DateTime(2020, 1, 7, 16, 0, 0),
                new DateTime(2020, 1, 7, 17, 0, 0),
                new DateTime(2020, 1, 7, 18, 0, 0) }));
        }

        [TestMethod]
        public void Test_Some_Vehicles_FeeFree()
        {
            Assert.AreEqual(0, tollfeeC.GetTollFee(new Diplomat(), new DateTime[] {
                new DateTime(2015, 4, 10, 7, 15, 0) }));
            Assert.AreEqual(0, tollfeeC.GetTollFee(new Tractor(), new DateTime[] {
                new DateTime(2015, 4, 10, 7, 15, 0) }));
            Assert.AreEqual(0, tollfeeC.GetTollFee(new Military(), new DateTime[] {
                new DateTime(2015, 4, 10, 7, 15, 0) }));
            Assert.AreEqual(0, tollfeeC.GetTollFee(new Emergency(), new DateTime[] {
                new DateTime(2015, 4, 10, 7, 15, 0) }));
        }

        [TestMethod]
        public void Test_TollFree_Holiday_Weekend()
        {
            Assert.AreEqual(0, tollfeeC.GetTollFee(new Car(), new DateTime[] {
                new DateTime(2022, 5, 26, 7, 15, 0),
                new DateTime(2015, 2, 7, 7, 15, 0)}));
        }

    }
}