using TollCalculator.Models;
using TollCalculator.Services;

namespace TollCalculator.Tests
{
    public class TollCalculatorServiceTests
    {
        private readonly TollCalculatorService _calculator;
        private readonly VehicleRegistry _registry;
        private readonly SwedishHolidayService _holidayService;

        public TollCalculatorServiceTests()
        {
            _registry = new VehicleRegistry(new List<Vehicle>
            {
                new Car("CAR123"),
                new Bus("BUS888"),
                new Car("AAA111"),
                new Car("BBB222")
            });

            _holidayService = new SwedishHolidayService();
            _calculator = new TollCalculatorService(_registry, _holidayService);
        }

        [Fact]
        public void Calculate_EntriesForTwoVehicles_GroupedByRegNo()
        {
            // Arrange
            var entries = new List<TollEntry>
            {
                new TollEntry( "AAA111", new DateTime(2026, 6, 7, 8, 10, 11)),
                new TollEntry( "BBB222", new DateTime(2026, 6, 7, 8, 15, 15)),
                new TollEntry( "AAA111", new DateTime(2026, 6, 7, 8, 20, 20))
            };
            
            // Act
            var results = _calculator.Calculate(entries);

            // Assert
            Assert.Equal(2, results.Count);
            Assert.Equal(2, results.First(v => v.RegNo == "AAA111").Details.Count);
        }

        [Fact]
        public void Calculate_TollFreeVehicle_ReturnsZeroFee()
        {
            // Arrange
            var entries = new List<TollEntry>
            {
                new TollEntry("CAR123", new DateTime(2026, 6, 8, 8, 0, 0)),
                new TollEntry("BUS888", new DateTime(2026, 6, 8, 8, 0, 1))
            };

            // Act
            var results = _calculator.Calculate(entries);

            // Assert
            var bussFee = results.First(v => v.RegNo == "BUS888").TotalFee;
            var carFee = results.First(v => v.RegNo == "CAR123").TotalFee;

            Assert.Equal(0, bussFee);
            Assert.True(carFee > 0);
        }

        [Fact]
        public void Calculate_EmptyList_ReturnsEmptyList()
        {
            // Arrange
            var entries = new List<TollEntry>();

            // Act
            var results = _calculator.Calculate(entries);

            // Assert
            Assert.Empty(results);
        }

        [Theory]
        [InlineData(6, 0, 8)]
        [InlineData(6, 30, 13)]
        [InlineData(7, 0, 18)]
        [InlineData(8, 30, 8)]
        [InlineData(15, 30, 18)]
        [InlineData(19, 0, 0)]
        public void Calculate_SingleCarEntry_ReturnsCorrectFee(int hour, int minute, int expectedFee)
        {
            // Arrange
            var entries = new List<TollEntry>
            {
                new TollEntry("CAR123", new DateTime(2026, 6, 8, hour, minute, 0)) // Monday
            };

            // Act
            var results = _calculator.Calculate(entries);

            // Assert
            Assert.Equal(expectedFee, results.First().TotalFee);
        }

        [Fact]
        public void Calculate_FeesExceedDailyCap_Returns60()
        {
            var entries = new List<TollEntry>
            {
                new TollEntry("CAR123", new DateTime(2026, 6, 8, 7, 0, 0)),  // 18 SEK
                new TollEntry("CAR123", new DateTime(2026, 6, 8, 9, 0, 0)),  // 8 SEK
                new TollEntry("CAR123", new DateTime(2026, 6, 8, 15, 30, 0)),// 18 SEK
                new TollEntry("CAR123", new DateTime(2026, 6, 8, 16, 31, 0)), // 18 SEK
                new TollEntry("CAR123", new DateTime(2026, 6, 8, 17, 0, 0)), // 13 SEK
            };
            // (total 75 SEK)

            // Act
            var results = _calculator.Calculate(entries);

            // Assert
            Assert.Equal(60, results.First().TotalFee);
        }

        [Fact]
        public void Calculate_TwoEntriesWithinSameHour_ReturnsMostExpensiveFee()
        {
            // Arrange
            var entries = new List<TollEntry>
            {
                new TollEntry("CAR123", new DateTime(2026, 6, 8, 7, 0, 0)),  // 18 SEK
                new TollEntry("CAR123", new DateTime(2026, 6, 8, 7, 30, 0)), // 13 SEK
            };
            // Should return 18, not 31

            // Act
            var results = _calculator.Calculate(entries);

            // Assert
            Assert.Equal(18, results.First().TotalFee);
        }

        [Fact]
        public void Calculate_SingleCarEntry_DetailsFeeMatchesTotalFee()
        {
            // Arrange
            var entries = new List<TollEntry>
            {
                new TollEntry("CAR123", new DateTime(2026, 6, 8, 7, 0, 0)) // 18 SEK
            };

            // Act
            var results = _calculator.Calculate(entries);

            // Assert
            Assert.Equal(18, results.First().Details.First().Fee);
        }

        [Fact]
        public void Calculate_CarEntryAtNight_ReturnsZeroFee()
        {
            // Arrange
            var entries = new List<TollEntry>
            {
                new TollEntry("CAR123", new DateTime(2026, 6, 8, 3, 0, 0)) // 03:00 night
            };

            // Act
            var results = _calculator.Calculate(entries);

            // Assert
            Assert.Equal(0, results.First().TotalFee);
        }

        [Fact]
        public void GetUnknownVehicles_UnregisteredVehicle_ReturnsRegNo()
        {
            // Arrange
            var entries = new List<TollEntry>
            {
                new TollEntry("UNKNOWN", new DateTime(2026, 6, 8, 7, 0, 0)),
                new TollEntry("CAR123", new DateTime(2026, 6, 8, 8, 0, 0))  // known
            };

            // Act
            var unknowns = _calculator.GetUnknownVehicles(entries);

            // Assert
            Assert.Single(unknowns);
            Assert.Contains("UNKNOWN", unknowns);
        }

        [Fact]
        public void Calculate_UnregisteredVehicle_ChargesFullFeeAndFlaggedAsUnknown()
        {
            // Arrange
            var entries = new List<TollEntry>
            {
                new TollEntry("UNKNOWN", new DateTime(2026, 6, 8, 7, 0, 0)), // rush hour
                new TollEntry("CAR123", new DateTime(2026, 6, 8, 7, 0, 0))   // known
            };

            // Act
            var results = _calculator.Calculate(entries);

            // Assert
            var unknown = results.First(v => v.RegNo == "UNKNOWN");
            Assert.True(unknown.IsUnknown);
            Assert.Equal(18, unknown.TotalFee);  // charged rush hour fee
            Assert.False(results.First(v => v.RegNo == "CAR123").IsUnknown);
        }

        [Fact]
        public void Calculate_NullInput_ThrowsArgumentNullException()
        {
            // Act & Assert
            Assert.Throws<ArgumentNullException>(() => _calculator.Calculate(null));
        }

        [Theory]
        [InlineData(6, 29, 8)]   // last minute of 8 SEK period
        [InlineData(6, 30, 13)]  // first minute of 13 SEK period
        [InlineData(6, 59, 13)]  // last minute of 13 SEK period
        [InlineData(7, 0, 18)]   // first minute of rush hour
        [InlineData(7, 59, 18)]  // last minute of rush hour
        [InlineData(8, 0, 13)]   // first minute after rush hour
        [InlineData(8, 29, 13)]  // last minute of 13 SEK period
        [InlineData(8, 30, 8)]   // first minute of long 8 SEK period
        [InlineData(14, 59, 8)]  // last minute of long 8 SEK period
        [InlineData(15, 0, 13)]  // first minute of afternoon
        [InlineData(15, 29, 13)] // last minute of 13 SEK period
        [InlineData(15, 30, 18)] // first minute of afternoon rush hour
        [InlineData(16, 59, 18)] // last minute of afternoon rush hour
        [InlineData(17, 0, 13)]  // first minute after afternoon rush
        [InlineData(17, 59, 13)] // last minute of 13 SEK period
        [InlineData(18, 0, 8)]   // first minute of last period
        [InlineData(18, 29, 8)]  // last minute of last period
        [InlineData(18, 30, 0)]  // first minute of free period
        public void Calculate_EntryAtFeeBoundary_ReturnsCorrectFee(int hour, int minute, int expectedFee)
        {
            // Arrange
            var entries = new List<TollEntry>
            {
                new TollEntry("CAR123", new DateTime(2026, 6, 8, hour, minute, 0))
            };

            // Act
            var results = _calculator.Calculate(entries);

            // Assert
            Assert.Equal(expectedFee, results.First().TotalFee);
        }

        [Fact]
        public void Calculate_EntriesSpanningMidnight_AppliesDailyCapPerDay()
        {
            // Arrange
            var entries = new List<TollEntry>
            {
                // Monday — many rush hour entries
                new TollEntry("CAR123", new DateTime(2026, 6, 8, 7, 0, 0)),   // 18 SEK
                new TollEntry("CAR123", new DateTime(2026, 6, 8, 15, 30, 0)), // 18 SEK
                new TollEntry("CAR123", new DateTime(2026, 6, 8, 16, 0, 0)),  // 18 SEK
                new TollEntry("CAR123", new DateTime(2026, 6, 8, 17, 0, 0)),  // 13 SEK
                // Tuesday
                new TollEntry("CAR123", new DateTime(2026, 6, 9, 7, 0, 0)),   // 18 SEK
                new TollEntry("CAR123", new DateTime(2026, 6, 9, 15, 30, 0)), // 18 SEK
                new TollEntry("CAR123", new DateTime(2026, 6, 9, 16, 0, 0)),  // 18 SEK
                new TollEntry("CAR123", new DateTime(2026, 6, 9, 17, 0, 0)),  // 13 SEK
            };
            // Monday: 18 + 18 + 13 = 49 SEK
            // Tuesday: 18 + 18 + 13 = 49 SEK
            // Total = 98 SEK (under daily cap both days)

            // Act
            var results = _calculator.Calculate(entries);

            // Assert
            Assert.Equal(98, results.First().TotalFee);
        }

        [Theory]
        [InlineData(5, 59, 59, 0)]
        [InlineData(6, 00, 00, 8)]
        [InlineData(6, 29, 59, 8)]
        [InlineData(6, 30, 00, 13)]
        [InlineData(6, 59, 59, 13)]
        [InlineData(7, 00, 00, 18)]
        [InlineData(7, 59, 59, 18)]
        [InlineData(8, 00, 00, 13)]
        [InlineData(8, 29, 59, 13)]
        [InlineData(8, 30, 00, 8)]
        [InlineData(14, 59, 59, 8)]
        [InlineData(15, 00, 00, 13)]
        [InlineData(15, 29, 59, 13)]
        [InlineData(15, 30, 00, 18)]
        [InlineData(16, 59, 59, 18)]
        [InlineData(17, 00, 00, 13)]
        [InlineData(17, 59, 59, 13)]
        [InlineData(18, 00, 00, 8)]
        [InlineData(18, 29, 59, 8)]
        [InlineData(18, 30, 00, 0)]
        public void Calculate_EntryAtFeeBoundarySeconds_ReturnCorrectFee(int hour, int minute, int second, int expectedFee)
        {
            // Arrange
            List<TollEntry> entries = new List<TollEntry>
                {
                    new TollEntry("CAR123", new DateTime(2026, 6, 8, hour, minute, second))
                };

            // Act
            var results = _calculator.Calculate(entries);

            // Assert
            Assert.Equal(expectedFee, results.First().TotalFee);
        }
    }
}
