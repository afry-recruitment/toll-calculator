using Microsoft.VisualStudio.TestTools.UnitTesting;
using toll_calculator;
using toll_calculator.Vehicles;

namespace toll_calculatorUnitTest
{
    [TestClass]
    public class TollCalculatorTest
    {
        [DataTestMethod]
        [DataRow(2022, 10, 17)]
        [DataRow(2022, 10, 18)]
        [DataRow(2022, 10, 19)]
        [DataRow(2022, 10, 20)]
        [DataRow(2022, 10, 21)]
        public void Given_LocationSweden_And_WorkingDay_When_InvokeIsTollFreeDate_Then_ReturnFalse(int year, int month, int day)
        {
            //Arrange
            var sut = new TollCalculator();
            var date = new DateTime(year, month, day);

            //Act
            var result = sut.IsTollFreeDate(date);

            //Assert
            Assert.IsFalse(result);
        }

        [DataTestMethod]
        [DataRow(2022, 10, 15)]
        [DataRow(2022, 10, 16)]
        public void Given_LocationSweden_And_Weekend_When_InvokeIsTollFreeDate_Then_ReturnTrue(int year, int month, int day)
        {
            //Arrange
            var sut = new TollCalculator();
            var date = new DateTime(year, month, day);

            //Act
            var result = sut.IsTollFreeDate(date);

            //Assert
            Assert.IsTrue(result);
        }

        [DataTestMethod]
        [DataRow(2022, 01, 06)]
        [DataRow(2022, 04, 15)]
        [DataRow(2022, 04, 18)]
        [DataRow(2022, 05, 26)]
        [DataRow(2022, 06, 06)]
        public void Given_LocationSweden_And_Holiday_When_InvokeIsTollFreeDate_Then_ReturnTrue(int year, int month, int day)
        {
            //Arrange
            var sut = new TollCalculator();
            var date = new DateTime(year, month, day);

            //Act
            var result = sut.IsTollFreeDate(date);

            //Assert
            Assert.IsTrue(result);
        }

        [DataTestMethod]
        [DataRow(VehicleType.Tractor)]
        [DataRow(VehicleType.MilitaryVehicle)]
        [DataRow(VehicleType.EmergencyVehicle)]
        [DataRow(VehicleType.ForeignVehicle)]
        [DataRow(VehicleType.DiplomatVehicle)]
        [DataRow(VehicleType.Motorbike)]
        public void Given_TollFreeVehicle_When_InvokeIsTollFreeVehicle_Then_ReturnTrue(VehicleType vehicleType)
        {
            //Arrange
            var sut = new TollCalculator();
            var vehicle = GetVehicle(vehicleType);

            //Act
            var result = sut.IsTollFreeVehicle(vehicle);

            //Assert
            Assert.IsTrue(result);
        }

        [DataTestMethod]
        [DataRow(VehicleType.Car)]
        public void Given_NonTollFreeVehicle_When_InvokeIsTollFreeVehicle_Then_ReturnFalse(VehicleType vehicleType)
        {
            //Arrange
            var sut = new TollCalculator();
            var vehicle = GetVehicle(vehicleType);

            //Act
            var result = sut.IsTollFreeVehicle(vehicle);

            //Assert
            Assert.IsFalse(result);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Given_NonVehicle_When_InvokeIsTollFreeVehicle_Then_ThrowException()
        {
            //Arrange
            var sut = new TollCalculator();

            //Act
            sut.IsTollFreeVehicle(null);
        }

        [DataTestMethod]
        [DataRow(VehicleType.Car, 4, 0, 0, 0)]
        [DataRow(VehicleType.Car, 6, 0, 0, 8)]
        [DataRow(VehicleType.Car, 6, 29, 59, 8)]
        [DataRow(VehicleType.Car, 7, 30, 00, 18)]
        [DataRow(VehicleType.Car, 9, 00, 00, 0)]
        [DataRow(VehicleType.Car, 10, 00, 00, 0)]
        [DataRow(VehicleType.Car, 11, 00, 00, 0)]
        [DataRow(VehicleType.Car, 12, 00, 00, 0)]
        [DataRow(VehicleType.Car, 13, 00, 00, 0)]
        [DataRow(VehicleType.Car, 14, 00, 00, 0)]
        [DataRow(VehicleType.Car, 15, 00, 00, 13)]
        [DataRow(VehicleType.Car, 18, 30, 00, 0)]
        [DataRow(VehicleType.Car, 21, 36, 9, 0)]
        [DataRow(VehicleType.Tractor, 7, 30, 0, 0)]
        public void Given_Vehicle_And_DateTime_When_InvokeGetTollFeeForTime_Then_ReturnFee(VehicleType vehicleType, int hour, int minute, int second, int expectedFee)
        {
            //Arrange
            var sut = new TollCalculator();
            var vehicle = GetVehicle(vehicleType);
            var dateTime = new DateTime(2022, 10, 17, hour, minute, second);

            //Act
            var actualfee = sut.GetTollFeeForTime(vehicle, dateTime);

            //Assert
            Assert.AreEqual(expectedFee, actualfee);
        }

        [TestMethod]
        public void Given_Car_And_TimesForADay_When_GetTollFeeForGivenDay_Then_ReturnFee()
        {
            //Arrange
            var sut = new TollCalculator();
            var car = GetVehicle(VehicleType.Car);
            var times = new List<DateTime>()
            {
                new DateTime(2022, 10, 17, 6, 36, 33),
                new DateTime(2022, 10, 17, 15, 0, 0),
                new DateTime(2022, 10, 17, 16, 29, 59),
                new DateTime(2022, 10, 17, 18, 30, 0)
            };

            //Act
            var actualFee = sut.GetTollFeeForGivenDay(car, times);

            //Assert
            Assert.AreEqual(44, actualFee);
        }

        [TestMethod]
        public void Given_Car_And_SeveralTimesInSameHour_When_GetTollFeeForGivenDay_Then_ReturnOnlyHighestFee()
        {
            //Arrange
            var sut = new TollCalculator();
            var car = GetVehicle(VehicleType.Car);
            var times = new List<DateTime>()
            {
                new DateTime(2022, 10, 17, 5, 59, 59),
                new DateTime(2022, 10, 17, 6, 1, 1),
                new DateTime(2022, 10, 17, 6, 31, 1),
                new DateTime(2022, 10, 17, 6, 36, 33),
                new DateTime(2022, 10, 17, 6, 49, 11),
            };

            //Act
            var actualFee = sut.GetTollFeeForGivenDay(car, times);

            //Assert
            Assert.AreEqual(13, actualFee);
        }

        [TestMethod]
        public void Given_Car_And_TimesThatTogetherExceedMaximumFee_When_GetTollFeeForGivenDay_Then_ReturnMaximumFee()
        {
            //Arrange
            var sut = new TollCalculator();
            var car = GetVehicle(VehicleType.Car);
            var times = new List<DateTime>()
            {
                new DateTime(2022, 10, 17, 6, 0, 0),
                new DateTime(2022, 10, 17, 7, 0, 0),
                new DateTime(2022, 10, 17, 8, 0, 0),
                new DateTime(2022, 10, 17, 15, 0, 0),
                new DateTime(2022, 10, 17, 16, 0, 0),
                new DateTime(2022, 10, 17, 17, 0, 0),
                new DateTime(2022, 10, 17, 18, 0, 0)
            };

            //Act
            var actualFee = sut.GetTollFeeForGivenDay(car, times);

            //Assert
            Assert.AreEqual(TollCalculator.MaximumFeeForGivenDay, actualFee);
        }

        [TestMethod]
        public void Given_Car_And_FeeFreeTimes_When_GetTollFeeForGivenDay_Then_ReturnZero()
        {
            //Arrange
            var sut = new TollCalculator();
            var car = GetVehicle(VehicleType.Car);
            var times = new List<DateTime>()
            {
                new DateTime(2022, 10, 17, 5, 59, 59),
                new DateTime(2022, 10, 17, 9, 0, 0),
                new DateTime(2022, 10, 17, 9, 29, 59),
                new DateTime(2022, 10, 17, 10, 0, 0),
                new DateTime(2022, 10, 17, 10, 29, 59),
                new DateTime(2022, 10, 17, 11, 0, 0),
                new DateTime(2022, 10, 17, 11, 29, 59),
                new DateTime(2022, 10, 17, 12, 0, 0),
                new DateTime(2022, 10, 17, 12, 29, 59),
                new DateTime(2022, 10, 17, 13, 0, 0),
                new DateTime(2022, 10, 17, 13, 29, 59),
                new DateTime(2022, 10, 17, 14, 0, 0),
                new DateTime(2022, 10, 17, 14, 29, 59),
                new DateTime(2022, 10, 17, 18, 30, 0)
            };

            //Act
            var actualFee = sut.GetTollFeeForGivenDay(car, times);

            //Assert
            Assert.AreEqual(0, actualFee);
        }

        [TestMethod]
        public void Given_Car_And_TimesForTwoDays_When_GetTollFeeForDates_Then_ReturnFee()
        {
            //Arrange
            var sut = new TollCalculator();
            var car = GetVehicle(VehicleType.Car);
            var dateTimes = new List<DateTime>()
            {
                new DateTime(2022, 10, 17, 6, 0, 0),
                new DateTime(2022, 10, 18, 18, 0, 0),
                new DateTime(2022, 10, 17, 7, 0, 0),
                new DateTime(2022, 10, 18, 6, 0, 0),
                new DateTime(2022, 10, 17, 8, 0, 0),
                new DateTime(2022, 10, 17, 15, 0, 0),
                new DateTime(2022, 10, 18, 17, 0, 0),
                new DateTime(2022, 10, 17, 16, 0, 0),
                new DateTime(2022, 10, 18, 16, 0, 0),
                new DateTime(2022, 10, 17, 17, 0, 0),
                new DateTime(2022, 10, 17, 18, 0, 0),
                new DateTime(2022, 10, 18, 7, 0, 0),
            };

            //Act
            var actualFee = sut.GetTollFeeForDates(car, dateTimes);

            //Assert
            Assert.AreEqual(104, actualFee);
        }

        private IVehicle GetVehicle(VehicleType vehicleType)
        {
            return vehicleType switch
            {
                VehicleType.Tractor => new Tractor(),
                VehicleType.MilitaryVehicle => new MilitaryVehicle(),
                VehicleType.EmergencyVehicle => new EmergencyVehicle(),
                VehicleType.ForeignVehicle => new ForeignVehicle(),
                VehicleType.DiplomatVehicle => new DiplomatVehicle(),
                VehicleType.Motorbike => new Motorbike(),
                _ => new Car(),
            };
        }
    }
}