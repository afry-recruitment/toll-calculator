using ConsoleClient;
using TollFeeCalculatoric;

namespace RunTests
{
    public class UniTestTollCalculator
    {
        TollCalculator tollCalculator = new TollCalculator();

        [Test]
        public void ChargedOncePerHour_HourSixAndSeven()
        {
            DateTime[] dateTimes = {
                           new DateTime(2023, 3, 22, 6, 0, 0),
                           new DateTime(2023, 3, 22, 6, 29, 0),
                           new DateTime(2023, 3, 22, 6, 32, 0),
                           new DateTime(2023, 3, 22, 6, 45, 0),

                           new DateTime(2023, 3, 22, 7, 15, 0),
                           new DateTime(2023, 3, 22, 6, 29, 0),
                           new DateTime(2023, 3, 22, 7, 30, 0),
                           new DateTime(2023, 3, 22, 7, 59, 0)
            };
            int expectedCost = 31;
            var actualCost = tollCalculator.GetTollFee(new Car("abc123"), dateTimes);
            Assert.That(actualCost, Is.EqualTo(expectedCost), $"Actual cost: {actualCost}");
        }

        [Test]
        public void ChargedOncePerHour_HourEightAndMore()
        {
            DateTime[] dateTimes = {
                           new DateTime(2023, 3, 22, 8, 0, 0),//13
                           new DateTime(2023, 3, 22, 8, 45, 0),
                           new DateTime(2023, 3, 22, 10, 15, 0),//8
                           new DateTime(2023, 3, 22, 13, 54, 0),//8
                           new DateTime(2023, 3, 22, 14, 54, 0)//8
            };
            int expectedCost = 37;
            var actualCost = tollCalculator.GetTollFee(new Car("abc123"), dateTimes);
            Assert.That(actualCost, Is.EqualTo(expectedCost), $"Actual cost: {actualCost}");
        }
        [Test]
        public void ChargedOncePerHour_HourThree()
        {
            DateTime[] dateTimes = {
                           new DateTime(2023, 3, 22, 15, 0, 0),
                           new DateTime(2023, 3, 22, 15, 15, 0),
                           new DateTime(2023, 3, 22, 15, 59, 0),
            };
            int expectedCost = 18;
            var actualCost = tollCalculator.GetTollFee(new Car("abc123"), dateTimes);
            Assert.That(actualCost, Is.EqualTo(expectedCost), $"Actual cost: {actualCost}");
        }

        [Test]
        public void ChargedOncePerHour_HourFourFiveSix()
        {
            DateTime[] dateTimes = {
                           new DateTime(2023, 3, 22, 16, 30, 0),//18
                           new DateTime(2023, 3, 22, 16, 45, 0),
                           new DateTime(2023, 3, 22, 16, 55, 0),
                           new DateTime(2023, 3, 22, 16, 59, 0),

                           new DateTime(2023, 3, 22, 17, 30, 0),//13
                           new DateTime(2023, 3, 22, 17, 45, 0),
                           new DateTime(2023, 3, 22, 17, 55, 0),
                           new DateTime(2023, 3, 22, 17, 59, 0),

                           new DateTime(2023, 3, 22, 18, 15, 0),//8
                           new DateTime(2023, 3, 22, 18, 29, 0),
                           new DateTime(2023, 3, 22, 18, 55, 0),
                           new DateTime(2023, 3, 22, 18, 59, 0),
            };
            int expectedCost = 39;
            var actualCost = tollCalculator.GetTollFee(new Car("abc123"), dateTimes);
            Assert.That(actualCost, Is.EqualTo(expectedCost), $"Actual cost: {actualCost}");
        }

        [Test]
        public void RushHour_ExpectHighestFee()
        {
            DateTime[] dateTimes = {
                
                          new DateTime(2023, 3, 22, 6, 0, 0),//13
                          new DateTime(2023, 3, 22, 6, 24, 0),

                          new DateTime(2023, 3, 22, 8, 11, 0),//13
                          new DateTime(2023, 3, 22, 8, 29, 0),

                          new DateTime(2023, 3, 22, 15, 30, 0),//18
                          new DateTime(2023, 3, 22, 15, 42, 0),
                          
                          new DateTime(2023, 3, 22, 16, 0, 0),//18
                          new DateTime(2023, 3, 22, 16, 15, 0),
                          new DateTime(2023, 3, 22, 16, 30, 0),
            };
            int expectedCost = 60;
            var actualCost = tollCalculator.GetTollFee(new Car("abc123"), dateTimes);
            Assert.That(actualCost, Is.EqualTo(expectedCost), $"Actual cost: {actualCost} expected: {expectedCost}");
        }

        [Test]
        public void WeekendsTollFree_ExpectNoCost()
        {
            DateTime[] dateTimes = {
                           new DateTime(2023, 3, 25, 9, 30, 0),//Saturday
                           new DateTime(2023, 3, 25, 14, 0, 0),
                           new DateTime(2023, 3, 25, 16, 0, 0),

                           new DateTime(2023, 3, 26, 9, 30, 0),//SunDay
                           new DateTime(2023, 3, 26, 12, 30, 0),
                           new DateTime(2023, 3, 26, 14, 10, 0),

                           new DateTime(2023, 3, 26, 16, 45, 0),
                           new DateTime(2023, 3, 26, 17, 30, 0)
            };
            int expectedCost = 0;
            var actualCost = tollCalculator.GetTollFee(new Car("abc123"), dateTimes);
            Assert.That(actualCost, Is.EqualTo(expectedCost), $"Actual cost: {actualCost} expected: {expectedCost}");
        }

        [Test]
        public void MaxCost_ExpectMaxCharge()
        {
            DateTime[] dateTimes = {
                           new DateTime(2023, 3, 22, 8, 0, 0),
                           new DateTime(2023, 3, 22, 8, 15, 0),
                           new DateTime(2023, 3, 22, 8, 29, 0),
                           new DateTime(2023, 3, 22, 8, 32, 0),
                           new DateTime(2023, 3, 22, 8, 45, 0),
                           new DateTime(2023, 3, 22, 9, 15, 0),
                           new DateTime(2023, 3, 22, 9, 43, 0),
                           new DateTime(2023, 3, 22, 10, 15, 0),
                           new DateTime(2023, 3, 22, 11, 15, 0),
                           new DateTime(2023, 3, 22, 11, 30, 0),
                           new DateTime(2023, 3, 22, 12, 45, 0),
                           new DateTime(2023, 3, 22, 12, 55, 0),
                           new DateTime(2023, 3, 22, 13, 24, 0),
                           new DateTime(2023, 3, 22, 13, 54, 0),
                           new DateTime(2023, 3, 22, 14, 34, 0),
                           new DateTime(2023, 3, 22, 14, 59, 0),
            };
            int expectedCost = 60;
            var actualCost = tollCalculator.GetTollFee(new Car("abc123"), dateTimes);
            Assert.That(actualCost, Is.EqualTo(expectedCost), $"Actual cost: {actualCost} expected: {expectedCost}");
        }

        [Test]
        public void ChristmessDays_ExpectNoCost()
        {
            DateTime[] dateTime = {
                           new DateTime(2023, 12, 25, 9, 30, 0),
                           new DateTime(2023, 12, 26, 14, 0, 0),
                           new DateTime(2013, 12, 25, 9, 0, 0),

                           new DateTime(2013, 12, 26, 14, 0, 0),
                           new DateTime(1989, 12, 25, 9, 0, 0),
                           new DateTime(1989, 12, 26, 14, 0, 0),
            };
            int expectedCost = 0;
            var actualCost = tollCalculator.GetTollFee(new Car("abc123"), dateTime);
            Assert.That(actualCost, Is.EqualTo(expectedCost), $"Actual cost: {actualCost} expected: {expectedCost}");
        }

        [Test]
        public void FirstOfMay_ExpectNoCost()
        {
            DateTime[] dateTime = {
                           new DateTime(2023, 5, 1, 9, 30, 0),
                           new DateTime(2023, 5, 1, 11, 30, 0),
                           new DateTime(2023, 5, 1, 14, 30, 0),

                           new DateTime(2013, 5, 1, 9, 30, 0),
                           new DateTime(2013, 5, 1, 11, 30, 0),
                           new DateTime(2013, 5, 1, 14, 30, 0),

                           new DateTime(1989, 5, 1, 9, 30, 0),
                           new DateTime(1989, 5, 1, 11, 30, 0),
                           new DateTime(1989, 5, 1, 14, 30, 0)
            };
            int expectedCost = 0;
            var actualCost = tollCalculator.GetTollFee(new Car("abc123"), dateTime);
            Assert.That(actualCost, Is.EqualTo(expectedCost), $"Actual cost: {actualCost} expected: {expectedCost}");
        }

        [Test]
        public void LongFriday_ExpectNoCost()
        {
            DateTime[] dateTime = {
                           new DateTime(2023, 4, 7, 15, 0, 0),
                           new DateTime(2023, 4, 7, 16, 0, 0),
                           new DateTime(2023, 4, 7, 17, 0, 0),

                           new DateTime(2013, 3, 29, 15, 30, 0),
                           new DateTime(2013, 3, 29, 16, 30, 0),
                           new DateTime(2013, 3, 29, 17, 30, 0),

                           new DateTime(1989, 3, 24, 15, 30, 0),
                           new DateTime(1989, 3, 24, 16, 30, 0),
                           new DateTime(1989, 3, 24, 17, 30, 0),
            };
            int expectedCost = 0;
            var actualCost = tollCalculator.GetTollFee(new Car("abc123"), dateTime);
            Assert.That(actualCost, Is.EqualTo(expectedCost), $"Actual cost: {actualCost} expected: {expectedCost}");
        }

        [Test]
        public void EasterMonday_ExpectNoCost()
        {
            DateTime[] dateTime = {
                           new DateTime(2023, 4, 10, 15, 0, 0),
                           new DateTime(2023, 4, 10, 16, 0, 0),
                           new DateTime(2023, 4, 10, 17, 0, 0),

                           new DateTime(2013, 4, 1, 15, 0, 0),
                           new DateTime(2013, 4, 1, 16, 0, 0),
                           new DateTime(2013, 4, 1, 17, 8, 0),

                           new DateTime(1989, 3, 27, 15, 0, 0),
                           new DateTime(1989, 3, 27, 16, 0, 0),
                           new DateTime(1989, 3, 27, 17, 0, 0),
            };
            int expectedCost = 0;
            var actualCost = tollCalculator.GetTollFee(new Car("abc123"), dateTime);
            Assert.That(actualCost, Is.EqualTo(expectedCost), $"Actual cost: {actualCost} expected: {expectedCost}");
        }

        [Test]
        public void NatonalDay_ExpectNoCost()
        {
            DateTime[] dateTime = {
                           new DateTime(2023, 6, 6, 15, 0, 0),
                           new DateTime(2023, 6, 6, 16, 0, 0),
                           new DateTime(2023, 6, 6, 1, 0, 0)
            };
            int expectedCost = 0;
            var actualCost = tollCalculator.GetTollFee(new Car("abc123"), dateTime);
            Assert.That(actualCost, Is.EqualTo(expectedCost), $"Actual cost: {actualCost} expected: {expectedCost}");
        }

        [Test]
        public void NewYear_ExpectNoCost()
        {
            DateTime[] dateTime = {
                           new DateTime(2023, 12, 31, 15, 0, 0),
                           new DateTime(2023, 12, 31, 16, 0, 0),
                           new DateTime(2023, 12, 31, 17, 0, 0),
                           new DateTime(2023, 1, 1, 15, 0, 0),
                           new DateTime(2023, 1, 1, 16, 0, 0),
                           new DateTime(2023, 1, 1, 17, 0, 0),

                           new DateTime(2013, 12, 31, 15, 30, 0),
                           new DateTime(2013, 12, 31, 15, 30, 0),
                           new DateTime(2013, 12, 31, 15, 30, 0),
                           new DateTime(2013, 1, 1, 15, 0, 0),
                           new DateTime(2013, 1, 1, 16, 0, 0),
                           new DateTime(2013, 1, 1, 17, 0, 0),

                           new DateTime(1989, 12, 31, 15, 30, 0),
                           new DateTime(1989, 12, 31, 16, 30, 0),
                           new DateTime(1989, 12, 31, 17, 30, 0),
                           new DateTime(1989, 1, 1, 15, 30, 0),
                           new DateTime(1989, 1, 1, 16, 30, 0),
                           new DateTime(1989, 1, 1, 17, 30, 0),
            };
            int expectedCost = 0;
            var actualCost = tollCalculator.GetTollFee(new Car("abc123"), dateTime);
            Assert.That(actualCost, Is.EqualTo(expectedCost), $"Actual cost: {actualCost} expected: {expectedCost}");
        }

        [Test]
        public void AscensionDay_ExpectNoCost()//Kristi himmelsfärds
        {
            DateTime[] dateTime = {
                           new DateTime(2023, 5, 18, 15, 0, 0),
                           new DateTime(2023, 5, 18, 16, 0, 0),
                           new DateTime(2023, 5, 18, 17, 0, 0),

                           new DateTime(2013, 5, 9, 15, 0, 0),
                           new DateTime(2013, 5, 9, 16, 0, 0),
                           new DateTime(2013, 5, 9, 17, 0, 0),

                           new DateTime(1989, 5, 4, 15, 0, 0),
                           new DateTime(1989, 5, 4, 16, 0, 0),
                           new DateTime(1989, 5, 4, 17, 0, 0),
            };
            int expectedCost = 0;
            var actualCost = tollCalculator.GetTollFee(new Car("abc123"), dateTime);
            Assert.That(actualCost, Is.EqualTo(expectedCost), $"Actual cost: {actualCost} expected: {expectedCost}");
        }
    }
}