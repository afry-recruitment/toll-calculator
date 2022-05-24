using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace TollFeeCalculator.Tests
{
    [TestClass()]
    public class TollCalculatorTests
    {
        [TestMethod()]
        public void TestSingleFee()
        {
            Vehicle car = new Vehicle(VehicleTypeEnum.Car);

            Assert.AreEqual(13, TollCalculator.GetTollFee(new DateTime(2020, 1, 7, 8, 0, 0), car));
        }

        [TestMethod()]
        public void TestHolidayFeeFree()
        {
            Vehicle car = new Vehicle(VehicleTypeEnum.Car);

            //2022-01-01       Nyårsdagen (Lördag)
            Assert.AreEqual(0, TollCalculator.GetTollFee(new DateTime(2022, 1, 1, 8, 0, 0), car));

            //2022-01-06       Trettondedag jul (Torsdag)
            Assert.AreEqual(0, TollCalculator.GetTollFee(new DateTime(2022, 1, 6, 8, 0, 0), car));

            //2022 - 04 - 15       Långfredagen(Fredag)
            Assert.AreEqual(0, TollCalculator.GetTollFee(new DateTime(2022, 4, 15, 8, 0, 0), car));

            //2022 - 04 - 17       Påskdagen(Söndag)
            Assert.AreEqual(0, TollCalculator.GetTollFee(new DateTime(2022, 4, 17, 8, 0, 0), car));

            //2022 - 04 - 18       Annandag påsk(Måndag)
            Assert.AreEqual(0, TollCalculator.GetTollFee(new DateTime(2022, 4, 18, 8, 0, 0), car));

            //2022 - 05 - 01       Första maj(Söndag)
            Assert.AreEqual(0, TollCalculator.GetTollFee(new DateTime(2022, 5, 1, 8, 0, 0), car));

            //2022 - 05 - 26       Kristi himmelfärdsdag(Torsdag)
            Assert.AreEqual(0, TollCalculator.GetTollFee(new DateTime(2022, 5, 26, 8, 0, 0), car));

            //2022 - 06 - 05       Pingstdagen(Söndag)
            Assert.AreEqual(0, TollCalculator.GetTollFee(new DateTime(2022, 6, 5, 8, 0, 0), car));

            //2022 - 06 - 06       Sveriges nationaldag(Måndag)
            Assert.AreEqual(0, TollCalculator.GetTollFee(new DateTime(2022, 6, 6, 8, 0, 0), car));

            //2022 - 06 - 24       Midsommarafton(Fredag)
            Assert.AreEqual(0, TollCalculator.GetTollFee(new DateTime(2022, 6, 24, 8, 0, 0), car));

            //2022 - 06 - 25       Midsommardagen(Lördag)
            Assert.AreEqual(0, TollCalculator.GetTollFee(new DateTime(2022, 6, 25, 8, 0, 0), car));

            //2022 - 11 - 05       Alla helgons dag(Lördag)
            Assert.AreEqual(0, TollCalculator.GetTollFee(new DateTime(2022, 11, 5, 8, 0, 0), car));

            //2022 - 12 - 24       Julafton(Lördag)
            Assert.AreEqual(0, TollCalculator.GetTollFee(new DateTime(2022, 12, 24, 8, 0, 0), car));

            //2022 - 12 - 25       Juldagen(Söndag)
            Assert.AreEqual(0, TollCalculator.GetTollFee(new DateTime(2022, 12, 25, 8, 0, 0), car));

            //2022 - 12 - 26       Annandag jul(Måndag)
            Assert.AreEqual(0, TollCalculator.GetTollFee(new DateTime(2022, 12, 26, 8, 0, 0), car));

            //2022 - 12 - 31       Nyårsafton(Lördag)
            Assert.AreEqual(0, TollCalculator.GetTollFee(new DateTime(2022, 12, 31, 8, 0, 0), car));
        }

        [TestMethod()]
        public void TestExemptVechilesFee()
        {
            Vehicle bike = new Vehicle(VehicleTypeEnum.Motorbike);
            Vehicle diplomat = new Vehicle(VehicleTypeEnum.Diplomat);
            Vehicle car = new Vehicle(VehicleTypeEnum.Car);

            Assert.AreEqual(0, TollCalculator.GetTollFee(new DateTime(2020, 4, 6, 8, 0, 0), bike));
            Assert.AreEqual(0, TollCalculator.GetTollFee(new DateTime(2020, 4, 6, 8, 0, 0), diplomat));
            Assert.AreNotEqual(0, TollCalculator.GetTollFee(new DateTime(2020, 4, 6, 8, 0, 0), car));
        }

        [TestMethod()]
        public void TestOnlyChargeOncePerHour()
        {
            Vehicle car = new Vehicle(VehicleTypeEnum.Car);

            DateTime[] dates =
            {
                new DateTime(2020, 1, 7, 6, 0, 0),
                new DateTime(2020, 1, 7, 6, 30, 0),
                new DateTime(2020, 1, 7, 8, 0, 0),
                new DateTime(2020, 1, 7, 8, 30, 0),
                new DateTime(2020, 1, 7, 8, 45, 0),
                new DateTime(2020, 1, 7, 17, 45, 0),
            };

            Assert.AreEqual(39, TollCalculator.GetTollFee(car, dates));
        }

        [TestMethod()]
        public void TestMaxFee()
        {
            Vehicle car = new Vehicle(VehicleTypeEnum.Car);

            DateTime[] dates =
            {
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
                new DateTime(2020, 1, 7, 18, 0, 0)
            };

            Assert.AreEqual(60, TollCalculator.GetTollFee(car, dates));
        }


        [TestMethod()]
        public void TestGauss()
        {
            Assert.AreEqual(new DateTime(2022, 4, 17), DateUtil.GaussEasterAlgorithm(2022));
            Assert.AreEqual(new DateTime(2018, 4, 1), DateUtil.GaussEasterAlgorithm(2018));
            Assert.AreEqual(new DateTime(2024, 3, 31), DateUtil.GaussEasterAlgorithm(2024));
        }
    }
}