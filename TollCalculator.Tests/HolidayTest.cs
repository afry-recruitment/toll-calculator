using Xunit;
using TollFeeCalculator;

namespace TollCalculator.Tests
{
    public class HolidayTest
    {
        [Fact]
        public void Saturday()
        {
            DateTime saturday = new DateTime(2025, 11, 15, 11, 11, 11);
            bool isWeekend = DateHandler.IsWeekend(saturday);

            Assert.True(isWeekend);
        }

        [Fact]
        public void Sunday()
        {
            DateTime sunday = new DateTime(2025, 11, 16, 11, 11, 11);
            bool isWeekend = DateHandler.IsWeekend(sunday);

            Assert.True(isWeekend);
        }

        [Fact]
        public void Weekday()
        {
            DateTime monday = new DateTime(2025, 10, 20, 11, 11, 11);
            DateTime tuesday = new DateTime(2025, 10, 21, 11, 11, 11);
            DateTime wednesday = new DateTime(2025, 10, 22, 11, 11, 11);
            DateTime thursday = new DateTime(2025, 10, 23, 11, 11, 11);
            DateTime friday = new DateTime(2025, 10, 24, 11, 11, 11);


            bool isWeekendMonday = DateHandler.IsWeekend(monday);
            bool isWeekendTuesday = DateHandler.IsWeekend(tuesday);
            bool isWeekendWednesday = DateHandler.IsWeekend(wednesday);
            bool isWeekendThursday = DateHandler.IsWeekend(thursday);
            bool isWeekendFriday = DateHandler.IsWeekend(friday);

            Assert.False(isWeekendMonday);
            Assert.False(isWeekendTuesday);
            Assert.False(isWeekendWednesday);
            Assert.False(isWeekendThursday);
            Assert.False(isWeekendFriday);
        }

        [Fact]
        public void Year()
        {
            DateTime monday2025 = new DateTime(2025, 10, 20, 11, 11, 11);
            DateTime monday2024 = new DateTime(2024, 10, 21, 11, 11, 11);
            
            bool isWeekendMonday2025 = DateHandler.IsWeekend(monday2025);
            bool isWeekendMonday2024 = DateHandler.IsWeekend(monday2024);
            
            Assert.False(isWeekendMonday2025);
            Assert.False(isWeekendMonday2024);
        }

        [Fact]
        public void ChristmasdayFixedDate()
        {
            DateTime christmas2025 = new DateTime(2025, 12, 25, 11, 11, 11);
            DateTime christmas2024 = new DateTime(2024, 12, 25, 11, 11, 11);
            
            bool isHoliday2025 = DateHandler.IsPublicHoliday(christmas2025);
            bool isHoliday2024 = DateHandler.IsPublicHoliday(christmas2024);

            Assert.True(isHoliday2025);
            Assert.True(isHoliday2024);
        }

        [Fact]
        public void DayBeforeChristmas()
        {
            DateTime dayBeforeChristmas2025 = new DateTime(2025, 12, 23, 11, 11, 11);
            DateTime dayBeforeChristmas2024 = new DateTime(2024, 12, 23, 11, 11, 11);

            bool isHoliday2025 = DateHandler.IsPublicHoliday(dayBeforeChristmas2025);
            bool isHoliday2024 = DateHandler.IsPublicHoliday(dayBeforeChristmas2024);

            Assert.False(isHoliday2025);
            Assert.False(isHoliday2024);
        }
        
        [Fact]
        public void GoodFridayChangingDate()
        {
            DateTime goodFriday2025 = new DateTime(2025, 04, 18, 11, 11, 11);
            DateTime notGoodFriday2024 = new DateTime(2024, 04, 18, 11, 11, 11);
            DateTime goodFriday2024 = new DateTime(2024, 03, 29, 11, 11, 11);

            bool isHoliday2025 = DateHandler.IsPublicHoliday(goodFriday2025);
            bool isAHoliday2024 = DateHandler.IsPublicHoliday(notGoodFriday2024);
            bool isHoliday2024 = DateHandler.IsPublicHoliday(goodFriday2024);

            Assert.True(isHoliday2025);
            Assert.False(isAHoliday2024);
            Assert.True(isHoliday2024);
        }
        
    }
}

