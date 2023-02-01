using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TollCalculator.Model;
using TollCalculator.Parsing;

namespace TollFeeCalculatorTest
{
    public class ExceptionTests
    {
        [Fact]
        public void Should_Throw_Exception_ForInvalid_Line()
        {
            //Arrange
            string[] csvLines = new[] { "Car;10/27/2022 17:00:00 AM" };

            //Act
            Car car = new Car();

            //Assert
            Assert.Throws<FormatException>(() => CsvLineParser.Parse(car, csvLines));
        }

        [Fact]
        public void Should_Throw_Exception_ForInvalid_Line_2()
        {
            //Arrange
            string[] csvLines = new[] { "Car;10/27/202  2 17:0 0:00 AM" };

            //Act
            Car car = new Car();

            //Assert
            Assert.Throws<FormatException>(() => CsvLineParser.Parse(car, csvLines));
        }
        [Fact]
        public void Should_Throw_Exception_ForInvalid_Line_3()
        {
            //Arrange
            string[] csvLines = new[] { "10/27/202  2 17:0 0:00 AM" };

            //Act
            Car car = new Car();

            //Assert
            Assert.Throws<ArgumentOutOfRangeException>(() => CsvLineParser.Parse(car, csvLines));
        }
    }
}
