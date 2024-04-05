using TollFeeCalculator;

namespace TollFeeCalculatorTests
{

    public class TollCalculatorTests
    {
        private readonly TollCalculator _tollCalculator = new TollCalculator();
        private readonly Vehicle _car = new Vehicle("ABC123", VehicleType.Car);
        private readonly Vehicle _motorbike = new Vehicle("GHJ234", VehicleType.Motorbike);


        [Fact]
        public void GetTollFee_Car_RushHour_ShouldChargeMaximumFee()
        {
            // Rush hour
            var dates = new[] { new DateTime(2024, 4, 2, 15, 45, 0) };
            var fee = _tollCalculator.GetTollFee(_car, dates);
            Assert.Equal(18, fee);
        }

        [Fact]
        public void GetTollFee_Car_MultiplePasses_ShouldNotExceedMaxDailyFee()
        {
            // Multiple passes including rush hours
            var dates = new[] {
                new DateTime(2024, 4, 2, 7, 30, 0),
                new DateTime(2024, 4, 2, 8, 35, 0),
                new DateTime(2024, 4, 2, 15, 40, 0),
                new DateTime(2024, 4, 2, 17, 50, 0),
                new DateTime(2024, 4, 2, 13, 30, 0),
                new DateTime(2024, 4, 2, 14, 35, 0)
            };
            var fee = _tollCalculator.GetTollFee(_car, dates);
            Assert.Equal(60, fee); // Maximum daily fee is 60
        }

        [Fact]
        public void GetTollFee_Car_MultiplePassesWithinHour_ShouldChargeOnceWithMaxFee()
        {
            // Multiple passes within one hour
            var dates = new[] {
                new DateTime(2024, 4, 2, 15, 0, 0),
                new DateTime(2024, 4, 2, 15, 45, 0) // Within the same hour as previous
            };
            var fee = _tollCalculator.GetTollFee(_car, dates);
            Assert.Equal(18, fee); // Only the highest fee in the hour should be charged
        }

        [Fact]
        public void GetTollFee_Motorbike_ShouldBeFree()
        {
            // Motorbike is toll-free
            var dates = new[] { new DateTime(2024, 4, 2, 10, 0, 0) };
            var fee = _tollCalculator.GetTollFee(_motorbike, dates);
            Assert.Equal(0, fee);

            // just to verify that other vechile type will be charged Motorbike within the same date
            var fee2 = _tollCalculator.GetTollFee(_car, dates);
            Assert.NotEqual(0, fee2);
        }

        [Fact]
        public void GetTollFee_Car_Weekend_ShouldBeFree()
        {
            // Weekend (Sunday)
            var dates = new[] { new DateTime(2024, 4, 7, 10, 0, 0) };
            var fee = _tollCalculator.GetTollFee(_car, dates);
            Assert.Equal(0, fee);
        }

        [Fact]
        public void GetTollFee_Car_Holiday_ShouldBeFree()
        {
            // Holiday (example date)
            var dates = new[] {
                new DateTime(2024, 1, 1),
                new DateTime(2024, 1, 5),
                new DateTime(2024, 3, 28),
                new DateTime(2024, 3, 29),
                new DateTime(2024, 4, 1),
                new DateTime(2024, 4, 30),
                new DateTime(2024, 5, 1),
                new DateTime(2024, 5, 8),
                new DateTime(2024, 5, 9),
                new DateTime(2024, 6, 5),
                new DateTime(2024, 6, 6),
                new DateTime(2024, 6, 21),
                new DateTime(2024, 11, 1),
                new DateTime(2024, 12, 24),
                new DateTime(2024, 12, 25),
                new DateTime(2024, 12, 26),
                new DateTime(2024, 12, 31)
            };
            var fee = _tollCalculator.GetTollFee(_car, dates);
            Assert.Equal(0, fee);
        }

        [Fact]
        public void GetTollFee_Car_MixedPasses_ShouldAccumulateCorrectly()
        {
            // Passes during rush and non-rush hours
            var dates = new[] {
                new DateTime(2024, 4, 2, 7, 30, 0), // Rush hour
                new DateTime(2024, 4, 2, 9, 0, 0), // Non-rush hour
                new DateTime(2024, 4, 2, 16, 30, 0) // Rush hour
            };
            var fee = _tollCalculator.GetTollFee(_car, dates);
            Assert.Equal(18 + 8 + 18, fee); // Sum of the individual fees without exceeding max per hour
        }

        [Fact]
        public void GetTollFee_Car_MultipleDays_ShouldHandleWeekendsAndHolidays()
        {
            // Passes on a weekend and a holiday
            var dates = new[] {
                new DateTime(2024, 4, 6, 10, 0, 0), // Saturday
                new DateTime(2024, 4, 7, 10, 0, 0), // Sunday
                new DateTime(2025, 12, 25, 10, 0, 0) // Holiday
            };
            var fee = _tollCalculator.GetTollFee(_car, dates);
            Assert.Equal(0, fee); // No fee should be charged
        }

        [Fact]
        public void GetTollFee_TollFreeVehicle_NonRushHour_ShouldBeFree()
        {
            // Non-rush hour
            var dates = new[] { new DateTime(2024, 4, 2, 10, 0, 0) };
            var fee = _tollCalculator.GetTollFee(_motorbike, dates);
            Assert.Equal(0, fee); // Motorbike is toll-free
        }

        [Fact]
        public void GetTollFee_Car_MultipleDays_ShouldNotExceedMaxDailyFee()
        {
            // Multiple passes across different days, ensuring the total fee does not exceed the maximum daily fee
            var dates = new[] {
                new DateTime(2024, 4, 2, 7, 30, 0), // Rush hour
                new DateTime(2024, 4, 3, 10, 0, 0), // Non-rush hour
                new DateTime(2024, 4, 4, 7, 30, 0) // Rush hour
            };
            var fee = _tollCalculator.GetTollFee(_car, dates);
            Assert.True(fee <= 60); // Total fee should not exceed the maximum daily fee
        }

        [Fact]
        public void DailyFeeShouldNotExceedMaxCap()
        {
            // multiple passages through the toll on the same day
            var passages = new DateTime[] {
                new DateTime(2024, 4, 2, 6, 0, 0),
                new DateTime(2024, 4, 2, 7, 0, 0),
                new DateTime(2024, 4, 2, 8, 0, 0),
                new DateTime(2024, 4, 2, 9, 0, 0),
                new DateTime(2024, 4, 2, 15, 30, 0),
                new DateTime(2024, 4, 2, 16, 30, 0)
            };
            var vehicle = new Vehicle("XYZ123", VehicleType.Car);
            var fee = _tollCalculator.GetTollFee(vehicle, passages);

            // Then the total fee should not exceed the 60 SEK daily cap
            Assert.Equal(60, fee);
        }

        [Fact]
        public void OnlyHighestFeeChargedWithinSameHour()
        {
            // multiple passes within the same hour, including a transition from a lower to a higher fee period
            var passes = new DateTime[] {
               new DateTime(2024, 4, 2, 6, 15, 0), // Should cost 8 SEK
               new DateTime(2024, 4, 2, 6, 45, 0)  // Should cost 13 SEK within the same hour as the previous pass
             };

            var vehicle = new Vehicle("XYZ123", VehicleType.Car);

            var fee = _tollCalculator.GetTollFee(vehicle, passes);

            // Then only the highest fee (13 SEK) within the same hour should be charged
            Assert.Equal(13, fee);
        }

        [Fact]
        public void PassesAcrossDifferentDays_ShouldAccuratelyTrackAndApplyFees()
        {
            // Including a mix of a normal weekday, a weekend, and a holiday
            var weekdayPass = new DateTime(2024, 4, 2, 10, 0, 0);  // Normal weekday
            var weekendPass = new DateTime(2024, 4, 6, 10, 0, 0);  // Saturday
            var holidayPass = new DateTime(2024, 1, 1, 10, 0, 0);  // New Year's Day

            var totalFee = _tollCalculator.GetTollFee(_car, new[] { weekdayPass, weekendPass, holidayPass });
       
            Assert.Equal(8, totalFee);
        }

        [Fact]
        public void PassEmptyDatesArray_ShouldReturnZeroFee()
        {
            var emptyDates = new DateTime[] { };
            var fee = _tollCalculator.GetTollFee(_car, emptyDates);
            Assert.Equal(0, fee); // Expecting no fee for empty date array
        }

        [Fact]
        public void PassNullVehicle_ShouldReturnZeroFeeO()
        {
            var dates = new[] { new DateTime(2024, 4, 2, 10, 0, 0) };

            var fee = _tollCalculator.GetTollFee(null, dates);
            Assert.Equal(0, fee);
        }
    }
}