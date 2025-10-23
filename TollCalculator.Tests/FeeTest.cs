using Xunit;
using TollCalculator;
using TollFeeCalculator;

namespace TollCalculator.Tests
{
    public class FeeTest
    {
        [Fact]
        public void SingleFee()
        {
            DateTime singleFee = new DateTime(2025, 10, 23, 06, 00, 00);
            Vehicle car = new Car();

            int isSingleFee = TollFeeCalculator.TollCalculator.GetTollFee(car, singleFee);

            Assert.Equal(8, isSingleFee);
        }

        [Fact]
        public void GetDifferentFeeOnTimeOfDay()
        {
            //Den här kan göras mer gedigen genom att testa samtliga tidsintervall från gettollfee funktionen. 
            // Man skulle också kunna testa alla tidpunkter för hela dagen så att det blir bortkopplat från tidsintervallen i funktionen.
            DateTime zero = new DateTime(2025, 10, 23, 05, 59, 59);
            DateTime eight = new DateTime(2025, 10, 23, 06, 00, 00);
            DateTime thirteen = new DateTime(2025, 10, 23, 15, 00, 00);
            DateTime eighteen = new DateTime(2025, 10, 23, 15, 30, 00);

            Vehicle car = new Car();

            int isZero = TollFeeCalculator.TollCalculator.GetTollFee(car, zero);
            int isEight = TollFeeCalculator.TollCalculator.GetTollFee(car, eight);
            int isThirteen = TollFeeCalculator.TollCalculator.GetTollFee(car, thirteen);
            int isEighteen = TollFeeCalculator.TollCalculator.GetTollFee(car, eighteen);

            Assert.Equal(0, isZero);
            Assert.Equal(8, isEight);
            Assert.Equal(13, isThirteen);
            Assert.Equal(18, isEighteen);

        }

        [Fact]
        public void GetMissedFeeIntervals()
        {
            DateTime nine = new DateTime(2025, 10, 23, 09, 00, 00);
            DateTime nineThirty = new DateTime(2025, 10, 23, 09, 29, 00);
            DateTime ten = new DateTime(2025, 10, 23, 10, 00, 00);
            DateTime tenThirty = new DateTime(2025, 10, 23, 10, 29, 00);
            DateTime eleven = new DateTime(2025, 10, 23, 11, 00, 00);
            DateTime elevenThirty = new DateTime(2025, 10, 23, 11, 29, 00);
            DateTime twelve = new DateTime(2025, 10, 23, 12, 00, 00);
            DateTime twelveThirty = new DateTime(2025, 10, 23, 12, 29, 00);
            DateTime thirteen = new DateTime(2025, 10, 23, 13, 00, 00);
            DateTime thirteenThirty = new DateTime(2025, 10, 23, 13, 29, 00);
            DateTime forteen = new DateTime(2025, 10, 23, 14, 00, 00);
            DateTime forteenThirty = new DateTime(2025, 10, 23, 14, 29, 00);

            Vehicle car = new Car();

            int isNine = TollFeeCalculator.TollCalculator.GetTollFee(car, nine);
            int isNineThirty = TollFeeCalculator.TollCalculator.GetTollFee(car, nineThirty);
            int isTen = TollFeeCalculator.TollCalculator.GetTollFee(car, ten);
            int isTenThirty = TollFeeCalculator.TollCalculator.GetTollFee(car, tenThirty);
            int isEleven = TollFeeCalculator.TollCalculator.GetTollFee(car, eleven);
            int isElevenThirty = TollFeeCalculator.TollCalculator.GetTollFee(car, elevenThirty);
            int isTwelve = TollFeeCalculator.TollCalculator.GetTollFee(car, twelve);
            int isTwelveThirty = TollFeeCalculator.TollCalculator.GetTollFee(car, twelveThirty);
            int isThirteen = TollFeeCalculator.TollCalculator.GetTollFee(car, thirteen);
            int isThirteenThirty = TollFeeCalculator.TollCalculator.GetTollFee(car, thirteenThirty);
            int isForteen = TollFeeCalculator.TollCalculator.GetTollFee(car, forteen);
            int isForteenThirty = TollFeeCalculator.TollCalculator.GetTollFee(car, forteenThirty);

            Assert.Equal(8, isNine);
            Assert.Equal(8, isNineThirty);
            Assert.Equal(8, isTen);
            Assert.Equal(8, isTenThirty);
            Assert.Equal(8, isEleven);
            Assert.Equal(8, isElevenThirty);
            Assert.Equal(8, isTwelve);
            Assert.Equal(8, isTwelveThirty);
            Assert.Equal(8, isThirteen);
            Assert.Equal(8, isThirteenThirty);
            Assert.Equal(8, isForteen);
            Assert.Equal(8, isForteenThirty);
        }

        [Fact]
        public void GetHighestHourFee()
        {
            DateTime[] passagesOnHour = new DateTime[]
            {
                new DateTime(2025, 10, 23, 07, 59, 59),
                new DateTime(2025, 10, 23, 08,29,59),
                new DateTime(2025, 10, 23, 08,49,59)
            };
            Vehicle car = new Car();

            int highestHourFee = TollFeeCalculator.TollCalculator.GetTollFee(car, passagesOnHour);

            Assert.Equal(18, highestHourFee);
        }
        
        [Fact]
        public void GetNewIntervallFee()
        {
            DateTime[] passagesOnHour = new DateTime[]
            {
                new DateTime(2025, 10, 23, 06, 00, 00),
                new DateTime(2025, 10, 23, 07, 59, 59),
                new DateTime(2025, 10, 23, 08,29,59),
                new DateTime(2025, 10, 23, 08,49,59)
            };
            Vehicle car = new Car();

            int isChargedOncePerHour = TollFeeCalculator.TollCalculator.GetTollFee(car, passagesOnHour);

            Assert.Equal(8+18, isChargedOncePerHour);
        }
        
        [Fact]
        public void MoreThanMaxFee()
        {
            DateTime[] passagesDuringADay = new DateTime[]
            {
                new DateTime(2025, 10, 23, 06, 00, 00), //8
                new DateTime(2025, 10, 23, 07,59,59), //18
                new DateTime(2025, 10, 23, 09,00,00), //8
                new DateTime(2025, 10, 23, 11,00,00), //8
                new DateTime(2025, 10, 23, 15,30,00), //18
                new DateTime(2025, 10, 23, 17,00,00) //13
            };

            Vehicle car = new Car();

            int isMaxFee = TollFeeCalculator.TollCalculator.GetTollFee(car, passagesDuringADay);

            Assert.Equal(60, isMaxFee);
        }

        [Fact]
        public void NoFeeVehicle()
        {
            DateTime[] passagesDuringADay = new DateTime[]
            {
                new DateTime(2025, 10, 23, 06, 00, 00), //8
                new DateTime(2025, 10, 23, 07,59,59), //18
                new DateTime(2025, 10, 23, 09,00,00), //8
                new DateTime(2025, 10, 23, 11,00,00), //8
                new DateTime(2025, 10, 23, 15,30,00), //18
                new DateTime(2025, 10, 23, 17,00,00) //13
            };

            Vehicle emergency = new Emergency();

            int isNoFeeVehicle = TollFeeCalculator.TollCalculator.GetTollFee(emergency, passagesDuringADay);

            Assert.Equal(0, isNoFeeVehicle);
        }

        [Fact]
        public void noFeeOnHoliday()
        {
            DateTime[] passagesDuringAHoliday = new DateTime[]
            {
                new DateTime(2025, 12, 24, 06, 00, 00), //8
                new DateTime(2025, 12, 24, 07,59,59), //18
                new DateTime(2025, 12, 24, 09,00,00), //8
                new DateTime(2025, 12, 24, 11,00,00), //8
                new DateTime(2025, 12, 24, 15,30,00), //18
                new DateTime(2025, 12, 24, 17,00,00) //13
            };

            Vehicle car = new Car();

            int isNoFeeDate = TollFeeCalculator.TollCalculator.GetTollFee(car, passagesDuringAHoliday);
        }

        [Fact]
        public void noFeeOnWeekendDay()
        {
            DateTime[] passagesDuringAWeekendDay = new DateTime[]
            {
                new DateTime(2025, 11, 22, 06, 00, 00), //8
                new DateTime(2025, 11, 22, 07,59,59), //18
                new DateTime(2025, 11, 22, 09,00,00), //8
                new DateTime(2025, 11, 22, 11,00,00), //8
                new DateTime(2025, 11, 22, 15,30,00), //18
                new DateTime(2025, 11, 22, 17,00,00) //13
            };

            Vehicle car = new Car();

            int isNoFeeDate = TollFeeCalculator.TollCalculator.GetTollFee(car, passagesDuringAWeekendDay);
        }
    
    }
}

