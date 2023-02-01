using Xunit;
using TollCalculator.Model;
using TollCalculator.Parsing;
using TollCalculator;

namespace TollCalculatorData.DataTests
{
    public class TollCalculatorTests
    {
        string pathToTestData = Directory.GetParent(Directory.GetCurrentDirectory()).Parent.Parent.FullName;

        [Fact]
        public void Valid_congestion_tax_for_time_range_between_0800_to_1700()
        {
            //Arrange
            string startupPath = pathToTestData + @"\Data\CarTestData1.csv";
            string[] csvLines = File.ReadAllLines(startupPath);
            Car car = new Car();
            VheicleDataItem carDataItems = CsvLineParser.Parse(car, csvLines);
            TollCalculatorFee tollCalculator = new TollCalculatorFee();

            //Act
            int test = tollCalculator.GetTollFee(carDataItems.vehicleType, carDataItems.registerDate);

            //Assert
            Assert.Equal(54, test);

        }
        [Fact]
        public void Valid_Congestion_tax_for_time_range_between_1501_to_1700()
        {
            //Arrange
            string startupPath = pathToTestData + @"\Data\CarTestData2.csv";
            string[] csvLines = File.ReadAllLines(startupPath);
            Car car = new Car();
            VheicleDataItem carDataItems = CsvLineParser.Parse(car, csvLines);
            TollCalculatorFee tollCalculator = new TollCalculatorFee();

            //Act
            int test = tollCalculator.GetTollFee(carDataItems.vehicleType, carDataItems.registerDate);

            //Assert
            Assert.Equal(38, test);

        }
        [Fact]
        public void Valid_Congestion_tax_for_time_range_between_0700_and_1545()
        {
            //Arrange
            string startupPath = pathToTestData + @"\Data\CarTestData6.csv";
            string[] csvLines = File.ReadAllLines(startupPath);
            Car car = new Car();
            VheicleDataItem carDataItems = CsvLineParser.Parse(car, csvLines);
            TollCalculatorFee tollCalculator = new TollCalculatorFee();

            //Act
            int test = tollCalculator.GetTollFee(carDataItems.vehicleType, carDataItems.registerDate);

            //Assert
            Assert.Equal(44, test);

        }
        [Fact]
        public void Valid_Congestion_tax_total_value_over_60()
        {
            //Arrange
            string startupPath = pathToTestData + @"\Data\CarTestData5.csv";
            string[] csvLines = File.ReadAllLines(startupPath);
            Car car = new Car();
            VheicleDataItem carDataItems = CsvLineParser.Parse(car, csvLines);
            TollCalculatorFee tollCalculator = new TollCalculatorFee();

            //Act
            int test = tollCalculator.GetTollFee(carDataItems.vehicleType, carDataItems.registerDate);

            //Assert
            Assert.Equal(60, test);

        }
    }
}