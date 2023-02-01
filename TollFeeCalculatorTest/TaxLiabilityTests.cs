using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TollCalculator;
using TollCalculator.Model;
using TollCalculator.Parsing;

namespace TollFeeCalculatorTest
{
    public class TaxLiabilityTests
    {
        string startupPath = Directory.GetParent(Directory.GetCurrentDirectory()).Parent.Parent.FullName + @"\Data\CarTestData4.csv";

        [Fact]
        public void Single_charge_rule()
        {
            //Arrange
            string[] csvLines = File.ReadAllLines(startupPath);
            Car car = new Car();
            VheicleDataItem carDataItems = CsvLineParser.Parse(car, csvLines);
            TollCalculatorFee tollCalculator = new TollCalculatorFee();

            //Act
            int test = tollCalculator.GetTollFee(carDataItems.vehicleType, carDataItems.registerDate);

            //Assert
            Assert.Equal(0, test);
        }
        [Fact]
        public void Diplomatnot_liable_to_the_tax()
        {
            //Arrange
            string[] csvLines = File.ReadAllLines(startupPath);
            Diplomat diplomat = new Diplomat();
            VheicleDataItem carDataItems = CsvLineParser.Parse(diplomat, csvLines);
            TollCalculatorFee tollCalculator = new TollCalculatorFee();

            //Act
            int test = tollCalculator.GetTollFee(carDataItems.vehicleType, carDataItems.registerDate);

            //Assert
            Assert.Equal(0, test);
        }
        [Fact]
        public void No_Congestion_when_public_holiday()
        {
            //Arrange
            string[] csvLines = File.ReadAllLines(startupPath);
            Car car = new Car();
            VheicleDataItem carDataItems = CsvLineParser.Parse(car, csvLines);
            TollCalculatorFee tollCalculator = new TollCalculatorFee();

            //Act
            int test = tollCalculator.GetTollFee(carDataItems.vehicleType, carDataItems.registerDate);

            //Assert
            Assert.Equal(0, test);

        }
        [Fact]
        public void Buses_with_a_total_weight_of_at_least_14_tonnes()
        {
            //Arrange
            string[] csvLines = File.ReadAllLines(startupPath);

            Buss buss = new Buss();
            buss.Weight = 14;

            bool test2 = buss.ExceptionsFromCongestionTax();

            VheicleDataItem carDataItems = CsvLineParser.Parse(buss, csvLines);
            TollCalculatorFee tollCalculator = new TollCalculatorFee();

            //Act
            int test = tollCalculator.GetTollFee(carDataItems.vehicleType, carDataItems.registerDate);

            //Assert
            Assert.True(buss.ExceptionsFromCongestionTax());
            Assert.Equal(0, test);

        }
    }
}