using ConsoleClient;
using TollFeeCalculatoric;

namespace RunTests
{
    public class UniTestTollCalculator
    {
        TollCalculator tollCalculator = new TollCalculator();

        [Test]
        public void ChargedOncePerHour()
        {
            DateTime[] dateTimes = {
                           new DateTime(2023, 3, 22, 6, 00, 0),
                           new DateTime(2023, 3, 22, 6, 32, 0),
                           new DateTime(2023, 3, 22, 7, 0, 0),
            };
            int expectedCost = 8;
            var actualCost = tollCalculator.GetTollFee(new Car(), dateTimes);
            Assert.That(actualCost, Is.EqualTo(expectedCost), $"Actual cost: {actualCost} expected: {expectedCost}");
        }

        [Test]
        public void RushHour_ExpectHighestFee()
        {
            DateTime[] dateTimes = {
                           new DateTime(2023, 3, 22, 15, 0, 0),
                           new DateTime(2023, 3, 22, 15, 15, 0),
                           new DateTime(2023, 3, 22, 15, 30, 0),
                           new DateTime(2023, 3, 22, 16, 0, 0),
                           new DateTime(2023, 3, 22, 16, 15, 0),
                           new DateTime(2023, 3, 22, 16, 30, 0),
            };
            int expectedCost = 18;
            var actualCost = tollCalculator.GetTollFee(new Car(), dateTimes);
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
            var actualCost = tollCalculator.GetTollFee(new Car(), dateTimes);
            Assert.That(actualCost, Is.EqualTo(expectedCost), $"Actual cost: {actualCost} expected: {expectedCost}");
        }

        [Test]
        public void MaxCost_ExpectMaxCharge()
        {
            DateTime[] dateTimes = {
                           new DateTime(2023, 3, 22, 6, 0, 0),
                           //new DateTime(2023, 3, 22, 6, 30, 0),
                           new DateTime(2023, 3, 22, 7, 0, 0),
                           new DateTime(2023, 3, 22, 8, 0, 0),
                           new DateTime(2023, 3, 22, 9, 0, 0),
                           new DateTime(2023, 3, 22, 10, 0, 0),
                           new DateTime(2023, 3, 22, 11, 0, 0),
                           new DateTime(2023, 3, 22, 12, 0, 0),
                           new DateTime(2023, 3, 22, 13, 0, 0),
                           new DateTime(2023, 3, 22, 14, 0, 0),
                           new DateTime(2023, 3, 22, 16, 0, 0),
                           new DateTime(2023, 3, 22, 17, 0, 0),
                           new DateTime(2023, 3, 22, 18, 0, 0),
                           new DateTime(2023, 3, 22, 19, 0, 0),
                           new DateTime(2023, 3, 22, 20, 0, 0),
            };
            int expectedCost = 60;
            var actualCost = tollCalculator.GetTollFee(new Car(), dateTimes);
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
            var actualCost = tollCalculator.GetTollFee(new Car(), dateTime);
            Assert.That(actualCost, Is.EqualTo(expectedCost), $"Actual cost: {actualCost} expected: {expectedCost}");
        }

        [Test]
        public void Christmess_ExpectCost()
        {
            DateTime[] dateTime = {
                           new DateTime(2023, 12, 24, 9, 30, 0),
                           new DateTime(2023, 12, 24, 12, 30, 0),
                           new DateTime(2023, 12, 24, 14, 00, 0),
                           new DateTime(2013, 12, 24, 9, 30, 0),
                           new DateTime(2013, 12, 24, 12, 30, 0),
                           new DateTime(2013, 12, 24, 14, 0, 0),
                           new DateTime(1989, 12, 24, 9, 30, 0),
                           new DateTime(1989, 12, 24, 12, 30, 0),
                           new DateTime(1989, 12, 24, 14, 0, 0)
            };
            int expectedCost = 16;
            var actualCost = tollCalculator.GetTollFee(new Car(), dateTime);
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
            var actualCost = tollCalculator.GetTollFee(new Car(), dateTime);
            Assert.That(actualCost, Is.EqualTo(expectedCost), $"Actual cost: {actualCost} expected: {expectedCost}");
        }

        [Test]
        public void LongFriday_ExpectNoCost()
        {
            DateTime[] dateTime = { 
                           new DateTime(2023, 4, 7, 16, 0, 0),
                           new DateTime(2013, 3, 29, 16, 30, 0),
                           new DateTime(1989, 3, 24, 16, 30, 0),
            };
            int expectedCost = 0;
            var actualCost = tollCalculator.GetTollFee(new Car(), dateTime);
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
            var actualCost = tollCalculator.GetTollFee(new Car(), dateTime);
            Assert.That(actualCost, Is.EqualTo(expectedCost), $"Actual cost: {actualCost} expected: {expectedCost}");
        }

        [Test]
        public void NatonalDay_ExpectNoCost()
        {
            //TODO finish up
            DateTime[] dateTime = {
                           new DateTime(2023, 6, 6, 15, 0, 0),
                           new DateTime(2023, 6, 6, 16, 0, 0),
                           new DateTime(2023, 6, 6, 1, 0, 0)
            };
            int expectedCost = 0;
            var actualCost = tollCalculator.GetTollFee(new Car(), dateTime);
            Assert.That(actualCost, Is.EqualTo(expectedCost), $"Actual cost: {actualCost} expected: {expectedCost}");
        }

        [Test]
        public void NewYear_ExpectNoCost()
        {
            //TODO finish up
            DateTime[] dateTime = {
                           new DateTime(2023, 4, 10, 15, 0, 0),
                           new DateTime(2013, 4, 1, 15, 30, 0),
                           new DateTime(1989, 3, 27, 11, 30, 0),
            };
            int expectedCost = 0;
            var actualCost = tollCalculator.GetTollFee(new Car(), dateTime);
            Assert.That(actualCost, Is.EqualTo(expectedCost), $"Actual cost: {actualCost} expected: {expectedCost}");
        }
        //TODO Code Below needs to be lockt over so its tested correctly

        [Test]
        public void AscensionDate_ExpectNoCost()//Kristi himmelsfärds
        {
            //TODO finish up
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
            var actualCost = tollCalculator.GetTollFee(new Car(), dateTime);
            Assert.That(actualCost, Is.EqualTo(expectedCost), $"Actual cost: {actualCost} expected: {expectedCost}");
        }

        [Test]
        public void PentecostDay_ExpectNoCost()//Pingstdagen
        {
            //TODO finish up
            DateTime[] dateTime = {
                           new DateTime(2023, 5, 28, 15, 0, 0),
                           new DateTime(2023, 5, 28, 16, 0, 0),
                           new DateTime(2023, 5, 28, 17, 0, 0),
                           new DateTime(2013, 5, 19, 15, 0, 0),
                           new DateTime(2013, 5, 19, 16, 0, 0),
                           new DateTime(2013, 5, 19, 17, 0, 0),
                           new DateTime(1989, 5, 14, 15, 0, 0),
                           new DateTime(1989, 5, 14, 16, 0, 0),
                           new DateTime(1989, 5, 14, 17, 0, 0)
            };
            int expectedCost = 0;
            var actualCost = tollCalculator.GetTollFee(new Car(), dateTime);
            Assert.That(actualCost, Is.EqualTo(expectedCost), $"Actual cost: {actualCost} expected: {expectedCost}");
        }
    }
}