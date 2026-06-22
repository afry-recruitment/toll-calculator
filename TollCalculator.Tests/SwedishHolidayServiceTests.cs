using TollCalculator.Services;

namespace TollCalculator.Tests
{
    public class SwedishHolidayServiceTests
    {
        private readonly SwedishHolidayService _holidayService;

        public SwedishHolidayServiceTests()
        {
            _holidayService = new SwedishHolidayService();
        }

        [Fact]
        public void IsWeekend_Saturday_ReturnsTrue()
        {
            Assert.True(_holidayService.IsWeekend(new DateTime(2026, 6, 13)));
        }

        [Fact]
        public void IsWeekend_Sunday_ReturnsTrue()
        {
            Assert.True(_holidayService.IsWeekend(new DateTime(2026, 6, 14)));
        }

        [Fact]
        public void IsWeekend_Monday_ReturnsFalse()
        {
            Assert.False(_holidayService.IsWeekend(new DateTime(2026, 6, 15)));
        }

        [Theory]
        [InlineData(1, 1)]   // Nyårsdagen
        [InlineData(1, 6)]   // Trettondedag jul
        [InlineData(5, 1)]   // Första maj
        [InlineData(6, 6)]   // Nationaldagen
        [InlineData(12, 25)] // Juldagen
        [InlineData(12, 26)] // Annandag jul
        [InlineData(12, 31)] // Nyårsafton
        public void IsPublicHoliday_FixedHoliday_ReturnsTrue(int month, int day)
        {
            // Arrange
            var date = new DateTime(2026, month, day);

            // Act
            var result = _holidayService.IsPublicHoliday(date);

            // Assert
            Assert.True(result);
        }

        [Theory]
        [InlineData(2026, 6, 19)] // Already a Friday
        [InlineData(2027, 6, 25)] // First Friday after June 19
        public void GetMidsummerEve_ReturnsCorrectDate(int year, int month, int day)
        {
            // Arrange
            var expected = new DateTime(year, month, day);

            // Act
            var result = _holidayService.GetMidsummerEve(year);

            // Assert
            Assert.Equal(expected, result);
        }

        [Theory]
        [InlineData(2026, 10, 31)] // Already a Saturday
        [InlineData(2027, 11, 6)]  // First Saturday after October 31
        public void GetAllSaints_ReturnsCorrectDate(int year, int month, int day)
        {
            // Arrange
            var expected = new DateTime(year, month, day);

            // Act
            var result = _holidayService.GetAllSaints(year);

            // Assert
            Assert.Equal(expected, result);
        }

        [Theory]
        [InlineData(2024, 3, 31)] // Easter Sunday 2024
        [InlineData(2025, 4, 20)] // Easter Sunday 2025
        [InlineData(2026, 4, 5)]  // Easter Sunday 2026
        [InlineData(2027, 3, 28)] // Easter Sunday 2027
        public void GetEaster_ReturnsCorrectDate(int year, int month, int day)
        {
            // Arrange
            var expected = new DateTime(year, month, day);

            // Act
            var result = _holidayService.GetEaster(year);

            // Assert
            Assert.Equal(expected, result);
        }
    }
}
