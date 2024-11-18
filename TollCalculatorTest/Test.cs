using toll_calculator_jakob.Services;

namespace TollCalculatorTest
{
    [TestClass]
    public class TollCalculatorTests
    {
        private readonly ITollCalculatorService calculator;
        public TollCalculatorTests()
        {
            calculator = new TollCalculatorService();
        }

        [TestMethod]
        public void GetTollFeeTotal_ShouldReturnCorrectFee_ForMultiplePassings()
        {
            // Arrange
            var testVehicle = TestDataFactory.CreateTestVehicle();
            var testDates = TestDataFactory.GenerateTestDates();

            // Act
            int totalFee = calculator.GetTollFeeTotal(testVehicle, testDates);

            // Assert
            Assert.AreEqual(52, totalFee);
        }

        [TestMethod]
        public void GetTollFeeTotal_ShouldReturnCorrectFee_ForMultiplePassingsOverLimit()
        {
            // Arrange
            var testVehicle = TestDataFactory.CreateTestVehicle();
            var testDates = TestDataFactory.GenerateTestDatesOverLimit();

            // Act
            int totalFee = calculator.GetTollFeeTotal(testVehicle, testDates);

            // Assert
            Assert.AreEqual(60, totalFee);
        }

        public void GetTollFeeTotal_ShouldReturnCorrectFee_ForMultiplePassings_Holiday()
        {
            // Arrange
            var testVehicle = TestDataFactory.CreateTestVehicle();
            var testDates = TestDataFactory.GenerateTestDatesHoliday();

            // Act
            int totalFee = calculator.GetTollFeeTotal(testVehicle, testDates);

            // Assert
            Assert.AreEqual(52, totalFee);
        }

        [TestMethod]
        public void GetTollFeeTotal_ShouldReturnZero_ForTollFreeVehicle()
        {
            // Arrange
            var tollFreeVehicle = TestDataFactory.CreateTestVehicle(true);
            var testDates = TestDataFactory.GenerateTestDates();

            // Act
            int totalFee = calculator.GetTollFeeTotal(tollFreeVehicle, testDates);

            // Assert
            Assert.AreEqual(0, totalFee);
        }

        [TestMethod]
        public void GetTollFeeTotal_ShouldReturnCorrectFee_ForSinglePassing()
        {
            // Arrange
            var testVehicle = TestDataFactory.CreateTestVehicle();
            var testDates = new List<DateTime> { new(2024, 11, 14, 6, 15, 59, 999) };

            // Act
            int totalFee = calculator.GetTollFeeTotal(testVehicle, testDates);

            // Assert
            Assert.AreEqual(8, totalFee);
        }

        [TestMethod]
        public void GetTollFeeTotal_ShouldReturnZero_ForNoDates()
        {
            // Arrange
            var testVehicle = TestDataFactory.CreateTestVehicle();
            var testDates = new List<DateTime>();

            // Act
            int totalFee = calculator.GetTollFeeTotal(testVehicle, testDates);

            // Assert
            Assert.AreEqual(0, totalFee);
        }
    }
}