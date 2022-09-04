using System;
using System.Collections.Generic;
using TollFeeCalculator;
using Xunit;

namespace Test_TollFeeCalculator
{
    public class UnitTest
    {
        [Fact]
        public void min_fee_car_vehicle()
        {
            // Arrange
            Car car = new Car();
            TollCalculator tollCalculator = new TollCalculator();
            DateTime[] carDateTimes = new DateTime[]
            {
                  new DateTime(2023,09,29,10,16,0),//8
            };

            // Act
            int actual = tollCalculator.GetTollFee(car, carDateTimes);

            // Assert
            Assert.Equal(8, actual);
        }
        [Fact]
        public void Some_vehicle_types_are_fee_free()
        {
            // Arrange
            Diplomat diplomat = new Diplomat();
            TollCalculator tollCalculator = new TollCalculator();
            DateTime[] diplomatDateTimes = new DateTime[]
            {
                  new DateTime(2023,09,29,10,16,0),//8
            };

            // Act
            int actual = tollCalculator.GetTollFee(diplomat, diplomatDateTimes);

            // Assert
            Assert.Equal(0, actual);
        }
        [Fact]
        public void weekends_and_holidays_are_fee_free()
        {
            // Arrange
            Car car = new Car();
            TollCalculator tollCalculator = new TollCalculator();
            DateTime[] carDateTimes = new DateTime[]
            {
                  new DateTime(2023,09,30,10,16,0),//0
            };

            // Act
            int actual = tollCalculator.GetTollFee(car, carDateTimes);

            // Assert
            Assert.Equal(0, actual);
        }
        [Fact]
        public void Car_should_only_be_charged_once_an_hour_highest_one()
        {
            // Arrange
            Car car = new Car();
            TollCalculator tollCalculator = new TollCalculator();
            DateTime[] carDateTimes = new DateTime[]
            {
                  new DateTime(2022,06,21,06,20,0),//8
                  new DateTime(2022,06,21,06,29,0),//8
                  new DateTime(2022,06,21,06,31,0)//13
            };

            // Act
            int actual = tollCalculator.GetTollFee(car, carDateTimes);

            // Assert
            Assert.Equal(13, actual);
        }
        [Fact]
        public void Fees_will_differ_between_8_SEK_and_18_SEK()
        {
            // Arrange
            Car car = new Car();
            TollCalculator tollCalculator = new TollCalculator();
            Random random = new Random();
            TimeSpan start_timepan = TimeSpan.FromHours(0);
            DateTime start_datetime = new DateTime(2022, 06, 21);//random date
            List<int> notvalid_tollfee = new List<int>();

            //generate date
            for (int i = 0; i < 1000; ++i)
            {
                int minutes = random.Next(1440);
                TimeSpan timespan = start_timepan.Add(TimeSpan.FromMinutes(minutes));
                start_datetime = start_datetime + timespan;
                DateTime[] carDateTimes = new DateTime[] { start_datetime };
                // Act
                int tollfee = tollCalculator.GetTollFee(car, carDateTimes);
                if ((tollfee < 8 && tollfee != 0) || tollfee > 18)
                { //not valid 
                    notvalid_tollfee.Add(tollfee);
                }
            }

            // Assert
            Assert.Empty(notvalid_tollfee);
        }
    }
}