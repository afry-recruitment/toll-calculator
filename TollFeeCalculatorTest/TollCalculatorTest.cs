using TollFeeCalculator;

namespace TollFeeCalculatorTest
{
    public class TollCalculatorTest
    {
        private readonly TollCalculator _tollCalc;
        private readonly IVehicle _car;
        private readonly IVehicle _bike;
        public TollCalculatorTest()
        {
            _tollCalc = new TollCalculator();
            _car = new Car();
            _bike = new Motorbike();
        }

        [Fact]
        public void GetTollFeeTest_ShouldReturn8_SingelTollPass()
        {
            // Arange
            DateTime[] dates = {
                new DateTime(2024, 8, 7, 18, 00, 0)     // +8
            };

            int expectedTotalFee = 8;

            // Act
            int actualTotalFee = _tollCalc.CalculateTollFee(_car, dates);

            // Assert
            Assert.Equal(expectedTotalFee, actualTotalFee);
        }

        [Fact]
        public void GetTollFeeTest_ShouldReturn24_ThreeTollPasses()
        {
            // Arange
            DateTime[] dates = {
                new DateTime(2024, 8, 7, 6, 0, 0),      // +8 (8)
                new DateTime(2024, 8, 7, 13, 30, 0),    // +8 (16)
                new DateTime(2024, 8, 7, 18, 00, 0)     // +8 (24)
            };

            int expectedTotalFee = 24;

            // Act
            int actualTotalFee = _tollCalc.CalculateTollFee(_car, dates);

            // Assert
            Assert.Equal(expectedTotalFee, actualTotalFee);
        }

        [Fact]
        public void GetTollFeeTest_ShouldReturn13_HighestFeeWithinTheHour()
        {
            // Arange
            DateTime[] dates = {
                new DateTime(2024, 8, 7, 6, 25, 0), // 8 (8)
                new DateTime(2024, 8, 7, 6, 30, 0)  // +13 (inom timmen 13)
            };

            int expectedTotalFee = 13;

            // Act
            int actualTotalFee = _tollCalc.CalculateTollFee(_car, dates);

            // Assert
            Assert.Equal(expectedTotalFee, actualTotalFee);
        }

        [Fact]
        public void GetTollFeeTest_ShouldReturnZero_TestTollFreeDates()
        {
            // Arange
            DateTime[] dates = {
                new DateTime(2024, 1, 1),
                new DateTime(2024, 1, 6),
                new DateTime(2024, 3, 29),
                new DateTime(2024, 3, 31),
                new DateTime(2024,4,1),
                new DateTime(2024,5,1),
                new DateTime(2024,5,9),
                new DateTime(2024,5,19)
            };
            // Act
            int actualTotalFee = _tollCalc.CalculateTollFee(_car, dates);
            // Assert
            Assert.Equal(0, actualTotalFee);
        }

        [Fact]
        public void GetTollFeeTest_ShouldReturn60_MultiplePassesExceedingMaxTollFeeOnSingelDay()
        {
            // Arrange
            DateTime[] dates = {
                new DateTime(2024, 8, 7, 6, 25, 0),     // +8
                new DateTime(2024, 8, 7, 7, 30, 0),     // +18 (26)
                new DateTime(2024, 8, 7, 8, 15, 0),     // 13 inom timmen (26)
                new DateTime(2024, 8, 7, 15, 15, 0),    // +13 (39)
                new DateTime(2024, 8, 7, 16, 20, 0),    // +18 (57)
                new DateTime(2024, 8, 7, 17, 25, 0)     // +13 (70)
            };

            int expectedTotalFee = 60;

            // Act
            int actualTotalFee = _tollCalc.CalculateTollFee(_car, dates);

            // Assert
            Assert.Equal(expectedTotalFee, actualTotalFee);
        }

        [Fact]
        public void GetTollFeeTest_ShouldReturn138_MultiplePassesOverMultipleDays()
        {
            // Arrange
            DateTime[] dates = {
                new DateTime(2024, 8, 7, 6, 25, 0),     // 8
                new DateTime(2024, 8, 7, 7, 30, 0),     // +18 (26)
                new DateTime(2024, 8, 7, 8, 15, 0),     // 13 inom timmen (26)
                new DateTime(2024, 8, 7, 15, 15, 0),    // +13 (39)
                new DateTime(2024, 8, 7, 16, 20, 0),    // +18 (57) ->  57

                new DateTime(2024, 8, 8, 6, 25, 0),     // 8
                new DateTime(2024, 8, 8, 7, 30, 0),     // +18 (26)
                new DateTime(2024, 8, 8, 8, 15, 0),     // 13 inom timmen (26)
                new DateTime(2024, 8, 8, 15, 15, 0),    // +13 (39)
                new DateTime(2024, 8, 8, 16, 20, 0),    // +18 (57)
                new DateTime(2024, 8, 8, 17, 25, 0),    // +13 (70) -> 60

                new DateTime(2024, 8, 9, 6, 25, 0),     // 8
                new DateTime(2024, 8, 9, 8, 15, 0)      // +13 (21) -> 21
            };                                          // -> 138

            int expectedTotalFee = 138;

            // Act
            int actualTotalFee = _tollCalc.CalculateTollFee(_car, dates);

            // Assert
            Assert.Equal(expectedTotalFee, actualTotalFee);
        }
        [Fact]
        public void GetTollFeeTest_ShouldReturnZero_OutsideTollTime()
        {
            // Arange
            DateTime[] dates = {
                new DateTime(2024, 8, 7, 19, 50, 0)
            };

            int expectedTotalFee = 0;

            // Act
            int actualTotalFee = _tollCalc.CalculateTollFee(_car, dates);

            // Assert
            Assert.Equal(expectedTotalFee, actualTotalFee);
        }
        [Fact]
        public void GetTollFeeTest_ShouldReturnZero_TollFreeVehicle()
        {
            // Arange
            DateTime[] dates = {
                new DateTime(2024, 8, 7, 6, 0, 0),      // +8 (8)
                new DateTime(2024, 8, 7, 13, 30, 0),    // +8 (16)
                new DateTime(2024, 8, 7, 18, 00, 0)     // +8 (24)
            };

            int expectedTotalFee = 0;

            // Act
            int actualTotalFee = _tollCalc.CalculateTollFee(_bike, dates);

            // Assert
            Assert.Equal(expectedTotalFee, actualTotalFee);
        }
    }
}