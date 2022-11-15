using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using TollCalculator.Models;
using TollCalculator.ViewModels;

namespace TollCalculator.Tests
{
    [TestClass]
    public class TollCalculatorTest
    {
        [TestMethod]
        public void GetTollFee_Car_TollFeeNotEqualToZero()
        {
            int expectedCost = 0;

            var vehicle = new Car();

            var calculator = new TollCalculator();

            var customDateMondayEvening = new DateTime(2022, 11, 08, 18, 12, 00);

            Assert.AreNotEqual(expectedCost, calculator.GetTollFee(customDateMondayEvening, vehicle));
        }

        [TestMethod]
        public void GetTollFee_Car_TollFeeEqualToEight()
        {
            int expectedCost = 8;

            var vehicle = new Car();

            var calculator = new TollCalculator();

            var customDateMondayEvening = new DateTime(2022, 11, 08, 18, 12, 00);

            Assert.AreEqual(expectedCost, calculator.GetTollFee(customDateMondayEvening, vehicle));
        }

        [TestMethod]
        public void GetTollFee_Car_TollFeeEqualTo13()
        {
            int expectedCost = 13;

            var vehicle = new Car();

            var calculator = new TollCalculator();

            var customDateMondayEvening = new DateTime(2022, 11, 08, 17, 00, 00);

            Assert.AreEqual(expectedCost, calculator.GetTollFee(customDateMondayEvening, vehicle));
        }

        [TestMethod]
        public void GetTollFee_Car_TollFeeEqualTo18()
        {
            int expectedCost = 18;

            var vehicle = new Car();

            var calculator = new TollCalculator();

            var customDateMondayEvening = new DateTime(2022, 11, 08, 07, 30, 00);

            Assert.AreEqual(expectedCost, calculator.GetTollFee(customDateMondayEvening, vehicle));
        }

        [TestMethod]
        public void GetTollFee_Car_TollFeeEqualTo8()
        {
            int expectedCost = 8;

            var vehicle = new Car();

            var calculator = new TollCalculator();

            //hour >= 8 && hour <= 14 && minute >= 30 && minute <= 59
            var customDateMondayEvening = new DateTime(2022, 11, 08, 9, 30, 00);

            Assert.AreEqual(expectedCost, calculator.GetTollFee(customDateMondayEvening, vehicle));
        }

        [TestMethod]
        public void GetTollFee_Car_TollFeeEqualTo13_2()
        {
            int expectedCost = 13;

            var vehicle = new Car();

            var calculator = new TollCalculator();

            //hour >= 8 && hour <= 14 && minute >= 30 && minute <= 59
            var customDateSunday = new DateTime(2022, 11, 08, 15, 29, 00);

            Assert.AreEqual(expectedCost, calculator.GetTollFee(customDateSunday, vehicle));
        }

        [TestMethod]
        public void GetTollFee_Car_TollFeeEqualTo18_2()
        {
            int expectedCost = 18;

            var vehicle = new Car();

            var calculator = new TollCalculator();

            //else if (hour == 15 && minute >= 0 || hour == 16 && minute <= 59) return 18;
            var customDateSunday = new DateTime(2022, 11, 08, 16, 29, 00);

            Assert.AreEqual(expectedCost, calculator.GetTollFee(customDateSunday, vehicle));
        }

        [TestMethod]
        public void GetTollFee_Car_TollFeeEqualTo13_3()
        {
            int expectedCost = 13;

            var vehicle = new Car();

            var calculator = new TollCalculator();

            //(hour == 17 && minute >= 0 && minute <= 59) return 13
            var customDateSunday = new DateTime(2022, 11, 08, 17, 29, 00);

            Assert.AreEqual(expectedCost, calculator.GetTollFee(customDateSunday, vehicle));
        }

        [TestMethod]
        public void GetTollFee_Car_TollFeeEqualTo8_3()
        {
            int expectedCost = 8;

            var vehicle = new Car();

            var calculator = new TollCalculator();

            //(hour == 18 && minute >= 0 && minute <= 29) return 8;
            var customDateSunday = new DateTime(2022, 11, 08, 18, 29, 00);

            Assert.AreEqual(expectedCost, calculator.GetTollFee(customDateSunday, vehicle));
        }

        [TestMethod]
        public void GetTollFee_Car_TollFeeEqualToHoliday()
        {
            int expectedCost = 0;

            var vehicle = new Car();

            var calculator = new TollCalculator();

            var customDateSunday = new DateTime(2022, 11, 6, 17, 00, 00);

            Assert.AreEqual(expectedCost, calculator.GetTollFee(customDateSunday, vehicle));
        }

        [TestMethod]
        public void GetTollFee_Car_TwoDates_TollFeeEqualTo26()
        {
            int expectedCost = 26;

            var calculator = new TollCalculator();

            var dates = new DateTime[]
            {
                new DateTime(2022, 11, 09, 17, 03, 01),
                new DateTime(2022, 11, 09, 08, 02, 02)
            };

            var tolledVehicle = new TollVehicleViewModel()
            {
                Vehicle = new Car(),
                TollPassesDuringDay = dates
            };

            Assert.AreEqual(expectedCost, calculator.GetTotalTollFeeByDay(tolledVehicle));
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException), "Vehicle type was not provided.")]
        public void GetTollFee_Car_TwoDates_VehicleIsNull_ArgumentNullException()
        {
            var calculator = new TollCalculator();

            var tolledVehicle = new TollVehicleViewModel()
            {
                Vehicle = null,
                TollPassesDuringDay = new DateTime[]
                {
                    new DateTime(2022, 11, 09, 17, 03, 01),
                    new DateTime(2022, 11, 09, 08, 02, 02)
                }
            };

            calculator.GetTotalTollFeeByDay(tolledVehicle);
        }

        [TestMethod]
        public void GetTollFee_Car_TwoDatesMotorbike_FreeToll()
        {
            int expectedCost = 0;

            var calculator = new TollCalculator();

            var tolledVehicle = new TollVehicleViewModel()
            {
                Vehicle = new Motorbike(),
                TollPassesDuringDay = new DateTime[]
            {
                new DateTime(2022, 11, 09, 17, 03, 01),
                new DateTime(2022, 11, 09, 08, 02, 02)
            }
            };

            Assert.AreEqual(expectedCost, calculator.GetTotalTollFeeByDay(tolledVehicle));
        }

        [TestMethod]
        public void GetTollFee_Car_TwoDatesMilitaryVehicle_FreeToll()
        {
            int expectedCost = 0;

            var calculator = new TollCalculator();

            var tolledVehicle = new TollVehicleViewModel()
            {
                Vehicle = new MilitaryVehicle(),
                TollPassesDuringDay = new DateTime[]
                {
                    new DateTime(2022, 11, 09, 17, 03, 01),
                    new DateTime(2022, 11, 09, 08, 02, 02)
                }
            };

            var output = calculator.GetTotalTollFeeByDay(tolledVehicle);

            Assert.AreEqual(expectedCost, output);
        }

        [TestMethod]
        public void GetTollFee_Car_TwoDatesCar_FreeToll()
        {
            int expectedCost = 0;

            var calculator = new TollCalculator();

            var dates = new DateTime[1];
            dates[0] = DateTime.MinValue;

            var tolledVehicle = new TollVehicleViewModel()
            {
                Vehicle = new Car(),
                TollPassesDuringDay = dates
            };

            var output = calculator.GetTotalTollFeeByDay(tolledVehicle);

            Assert.AreEqual(expectedCost, output);
        }

        [TestMethod]
        public void GetTollFee_Car_TwoDatesNewYearsEve_TollFeeEqualToHoliday()
        {
            int expectedCost = 0;

            var calculator = new TollCalculator();

            var tolledVehicle = new TollVehicleViewModel()
            {
                Vehicle = new Car(),
                TollPassesDuringDay = new DateTime[]
                {
                    new DateTime(2022, 01, 01, 17, 03, 01),
                    new DateTime(2022, 01, 01, 19, 03, 01)
                }
            };

            var output = calculator.GetTotalTollFeeByDay(tolledVehicle);

            Assert.AreEqual(expectedCost, output);
        }

        [TestMethod]
        public void GetTollFee_Car_TwoDatesChristmas_TollFeeEqualToHoliday()
        {
            int expectedCost = 0;
            
            var calculator = new TollCalculator();

            var tolledVehicle = new TollVehicleViewModel()
            {
                Vehicle = new Car(),
                TollPassesDuringDay = new DateTime[]
                {
                    new DateTime(2022, 12, 24, 10, 03, 01),
                    new DateTime(2022, 12, 24, 12, 03, 01),
                }
            };

            var output = calculator.GetTotalTollFeeByDay(tolledVehicle);

            Assert.AreEqual(expectedCost, output);
        }

        [TestMethod]
        public void GetTollFee_Car_DateEqualToEasterDay_TollFeeEqualToHoliday()
        {
            int expectedCost = 0;

            var calculator = new TollCalculator();

            var tolledVehicle = new TollVehicleViewModel()
            {
                Vehicle = new Car(),
                TollPassesDuringDay = new DateTime[]
                {
                    new DateTime(2023, 03, 28, 10, 03, 01) 
                }
            };

            var output = calculator.GetTotalTollFeeByDay(tolledVehicle);

            Assert.AreEqual(expectedCost, output);
        }

        [TestMethod]
        public void GetTollFee_Car_DateEqualToEasterDay1_TollFeeEqualToHoliday()
        {
            int expectedCost = 0;
            
            var calculator = new TollCalculator();

            var tolledVehicle = new TollVehicleViewModel()
            {
                Vehicle = new Car(),
                TollPassesDuringDay = new DateTime[]
                {
                    new DateTime(2023, 04, 01, 10, 03, 01),
                }
            };

            var output = calculator.GetTotalTollFeeByDay(tolledVehicle);

            Assert.AreEqual(expectedCost, output);
        }

        [TestMethod]
        public void GetTollFee_Car_DateEqualToEasterDay2_TollFeeEqualToHoliday()
        {
            int expectedCost = 0;

            var calculator = new TollCalculator();

            var tolledVehicle = new TollVehicleViewModel()
            {
                Vehicle = new Car(),
                TollPassesDuringDay = new DateTime[]
                {
                    new DateTime(2023, 04, 02, 10, 03, 01),
                }
            };

            var output = calculator.GetTotalTollFeeByDay(tolledVehicle);

            Assert.AreEqual(expectedCost, output);
        }

        [TestMethod]
        public void GetTollFee_Car_FirstOfMay_TollFeeEqualToHoliday()
        {
            int expectedCost = 0;
            
            var calculator = new TollCalculator();

            var tolledVehicle = new TollVehicleViewModel()
            {
                Vehicle = new Car(),
                TollPassesDuringDay = new DateTime[]
                {
                    new DateTime(2023, 05, 01, 10, 03, 01),
                }
            };

            var output = calculator.GetTotalTollFeeByDay(tolledVehicle);

            Assert.AreEqual(expectedCost, output);
        }

        [TestMethod]
        public void GetTollFee_Car_FirstOfNovember_TollFeeEqualToHoliday()
        {
            int expectedCost = 0;
            
            var calculator = new TollCalculator();

            var tolledVehicle = new TollVehicleViewModel()
            {
                Vehicle = new Car(),
                TollPassesDuringDay = new DateTime[]
                {
                    new DateTime(2022, 11, 01, 10, 03, 01),
                }
            };

            var output = calculator.GetTotalTollFeeByDay(tolledVehicle);

            Assert.AreEqual(expectedCost, output);
        }
    }
}