
using TollFeeCalculator.Interfaces;
using TollFeeCalculator.Services;
using TollFeeCalculator.Factories;

namespace TollFeeCalculatorTests;

public class TollFeeCalculatorTests
{
    private readonly ITollFeeCalculator _calculator =
        new TollFeeCalculatorService(
            TollFeeCalculatorConfigFactory.CreateDefault(), 
            new HolidayProvider2026());


    [Fact]
    public void CalculateFee_EmptyList_ReturnsZero()
    {
        var result = _calculator.CalculateFee(new List<DateTime>());
        Assert.Equal(0, result);
    }

    [Fact]
    public void CalculateFee_Null_ReturnsZero()
    {
        var result = _calculator.CalculateFee(null!);
        Assert.Equal(0, result);
    }

    [Fact]
    public void CalculateFee_SinglePassage_ReturnsCorrectFee()
    {
        var passages = new[]
        {
            new DateTime(2026, 1, 2, 7, 30, 0) // 18 SEK
        };

        var result = _calculator.CalculateFee(passages);
        Assert.Equal(18, result);
    }

    [Fact]
    public void CalculateFee_TwoPassagesWithin60Minutes_ChargesHighest()
    {
        var passages = new[]
        {
            new DateTime(2026, 1, 2, 7, 0, 0),  // 18 SEK
            new DateTime(2026, 1, 2, 7, 30, 0)  // 18 SEK
        };

        var result = _calculator.CalculateFee(passages);
        Assert.Equal(18, result); // Only highest
    }
    
    [Fact]
    public void CalculateFee_TwoPassagesWithinSameMinute_ChargesHighest()
    {
        var passages = new[]
        {
            new DateTime(2026, 1, 2, 7, 13, 5),  // 18 SEK
            new DateTime(2026, 1, 2, 7, 13, 5)  // 18 SEK
        };

        var result = _calculator.CalculateFee(passages);
        Assert.Equal(18, result); // Only highest
    }

    [Fact]
    public void CalculateFee_TwoPassagesOver60Minutes_ChargesBoth()
    {
        var passages = new[]
        {
            new DateTime(2026, 1, 2, 7, 0, 0),  // 18 SEK
            new DateTime(2026, 1, 2, 8, 5, 0)   // 13 SEK
        };

        var result = _calculator.CalculateFee(passages);
        Assert.Equal(31, result); // 18 + 13
    }

    [Fact]
    public void CalculateFee_DailyCapExceeded_Returns60()
    {
        var passages = new[]
        {
            new DateTime(2026, 1, 2, 6, 0, 0),
            new DateTime(2026, 1, 2, 7, 30, 0),
            new DateTime(2026, 1, 2, 9, 0, 0),
            new DateTime(2026, 1, 2, 10, 30, 0),
            new DateTime(2026, 1, 2, 15, 30, 0),
            new DateTime(2026, 1, 2, 17, 0, 0)
        };

        var result = _calculator.CalculateFee(passages);
        Assert.Equal(60, result); // Capped
    }

    [Fact]
    public void CalculateFee_ThreePassagesSameWindow_ChargesHighest()
    {
        var passages = new[]
        {
            new DateTime(2026, 1, 2, 6, 0, 0),  // 8 SEK
            new DateTime(2026, 1, 2, 6, 30, 0), // 13 SEK
            new DateTime(2026, 1, 2, 6, 50, 0)  // 13 SEK
        };

        var result = _calculator.CalculateFee(passages);
        Assert.Equal(13, result); // Highest in window
    }

    [Fact]
    public void CalculateFee_TwoDays_SumsBothDays()
    {
        var passages = new[]
        {
            new DateTime(2026, 1, 2, 7, 0, 0),  // Day 1: 18 SEK
            new DateTime(2026, 1, 5, 7, 0, 0)   // Day 2: 18 SEK
        };

        var result = _calculator.CalculateFee(passages);
        Assert.Equal(36, result); // 18 + 18
    }

    [Fact]
    public void CalculateFee_TwoDaysWithMultiplePassages_CalculatesCorrectly()
    {
        var passages = new[]
        {
            // Day 1: 8 + 18 = 26
            new DateTime(2026, 1, 2, 6, 0, 0),  // 8 SEK
            new DateTime(2026, 1, 2, 7, 30, 0), // 18 SEK (90 min later)
            
            // Day 2: 13 + 13 = 26
            new DateTime(2026, 1, 5, 8, 0, 0),  // 13 SEK
            new DateTime(2026, 1, 5, 15, 0, 0)  // 13 SEK
        };

        var result = _calculator.CalculateFee(passages);
        Assert.Equal(52, result); // 26 + 26
    }

    [Fact]
    public void CalculateFee_TwoDaysBothExceedCap_Returns120()
    {
        var passages = new[]
        {
            // Day 1: Would be ~80, capped at 60
            new DateTime(2026, 1, 2, 6, 0, 0),
            new DateTime(2026, 1, 2, 7, 30, 0),
            new DateTime(2026, 1, 2, 9, 0, 0),
            new DateTime(2026, 1, 2, 10, 30, 0),
            new DateTime(2026, 1, 2, 15, 30, 0),
            new DateTime(2026, 1, 2, 17, 0, 0),
            
            // Day 2: Would be ~80, capped at 60
            new DateTime(2026, 1, 5, 6, 0, 0),
            new DateTime(2026, 1, 5, 7, 30, 0),
            new DateTime(2026, 1, 5, 9, 0, 0),
            new DateTime(2026, 1, 5, 10, 30, 0),
            new DateTime(2026, 1, 5, 15, 30, 0),
            new DateTime(2026, 1, 5, 17, 0, 0)
        };

        var result = _calculator.CalculateFee(passages);
        Assert.Equal(120, result); // 60 + 60
    }

    [Fact]
    public void CalculateFee_UnsortedMultipleDays_HandlesCorrectly()
    {
        var passages = new[]
        {
            new DateTime(2026, 1, 5, 15, 0, 0), // Day 2
            new DateTime(2026, 1, 2, 7, 0, 0),  // Day 1
            new DateTime(2026, 1, 5, 7, 0, 0),  // Day 2
            new DateTime(2026, 1, 2, 15, 0, 0)  // Day 1
        };

        var result = _calculator.CalculateFee(passages);
        Assert.Equal(62, result); // Day 1: 31, Day 2: 31
    }

    [Fact]
    public void CalculateFee_Weekend_ReturnsZero()
    {
        var passages = new[]
        {
            new DateTime(2026, 1, 3, 7, 0, 0) // Saturday
        };

        var result = _calculator.CalculateFee(passages);
        Assert.Equal(0, result);
    }

    [Fact]
    public void CalculateFee_Holiday_ReturnsZero()
    {
        var passages = new[]
        {
            new DateTime(2026, 1, 1, 7, 0, 0) // New Year
        };

        var result = _calculator.CalculateFee(passages);
        Assert.Equal(0, result);
    }

    [Fact]
    public void CalculateFee_MixedWeekdayAndWeekend_OnlyChargesWeekday()
    {
        var passages = new[]
        {
            new DateTime(2026, 1, 4, 7, 0, 0), // Friday: 18 SEK
            new DateTime(2026, 1, 5, 7, 0, 0)  // Saturday: 0 SEK
        };

        var result = _calculator.CalculateFee(passages);
        Assert.Equal(18, result);
    }

    [Theory]
    [InlineData(6, 0, 8)]
    [InlineData(6, 30, 13)]
    [InlineData(7, 30, 18)]
    [InlineData(8, 0, 13)]
    [InlineData(9, 0, 8)]
    [InlineData(15, 0, 13)]
    [InlineData(15, 30, 18)]
    [InlineData(17, 0, 13)]
    [InlineData(18, 0, 8)]
    [InlineData(18, 30, 0)]
    public void CalculateFee_VariousTimes_ReturnsCorrectFee(int hour, int minute, int expectedFee)
    {
        var passages = new[]
        {
            new DateTime(2026, 1, 2, hour, minute, 0)
        };

        var result = _calculator.CalculateFee(passages);
        Assert.Equal(expectedFee, result);
    }
    
    [Fact]
    public void CalculateFee_ExactlyAtWindowBoundary_ChargesOnlyTheHighest()
    {
        var passages = new[]
        {
            new DateTime(2026, 1, 2, 7, 0, 0),  // 18 SEK
            new DateTime(2026, 1, 2, 8, 0, 0)   // 13 SEK (exactly 60 min)
        };

        var result = _calculator.CalculateFee(passages);
        Assert.Equal(18, result); // Both charged
    }

    [Fact]
    public void CalculateFee_59MinutesApart_ChargesOnlyHighest()
    {
        var passages = new[]
        {
            new DateTime(2026, 1, 2, 7, 0, 0),  // 18 SEK
            new DateTime(2026, 1, 2, 7, 59, 0)  // 18 SEK (59 min)
        };

        var result = _calculator.CalculateFee(passages);
        Assert.Equal(18, result); // Same window
    }

    [Fact]
    public void CalculateFee_MorningRushHour_Returns18SEK()
    {
        var passages = new[]
        {
            new DateTime(2026, 1, 2, 7, 0, 0)
        };

        var result = _calculator.CalculateFee(passages);
        Assert.Equal(18, result);
    }

    [Fact]
    public void CalculateFee_AfternoonRushHour_Returns18SEK()
    {
        var passages = new[]
        {
            new DateTime(2026, 1, 2, 16, 0, 0)
        };

        var result = _calculator.CalculateFee(passages);
        Assert.Equal(18, result);
    }

    [Fact]
    public void CalculateFee_MidnightPassage_ReturnsZero()
    {
        var passages = new[]
        {
            new DateTime(2026, 1, 2, 0, 0, 0)
        };

        var result = _calculator.CalculateFee(passages);
        Assert.Equal(0, result);
    }

    [Fact]
    public void CalculateFee_EarlyMorningBeforeTollHours_ReturnsZero()
    {
        var passages = new[]
        {
            new DateTime(2026, 1, 2, 5, 30, 0)
        };

        var result = _calculator.CalculateFee(passages);
        Assert.Equal(0, result);
    }

    [Fact]
    public void CalculateFee_LowestFee_Returns8SEK()
    {
        var passages = new[]
        {
            new DateTime(2026, 1, 2, 6, 15, 0) // 8 SEK
        };

        var result = _calculator.CalculateFee(passages);
        Assert.Equal(8, result);
    }

    [Fact]
    public void CalculateFee_Sunday_ReturnsZero()
    {
        var passages = new[]
        {
            new DateTime(2026, 1, 4, 7, 0, 0) // Sunday
        };

        var result = _calculator.CalculateFee(passages);
        Assert.Equal(0, result);
    }

    [Fact]
    public void CalculateFee_MultipleHolidays_ReturnsZero()
    {
        var passages = new[]
        {
            new DateTime(2026, 1, 6, 7, 0, 0),   // Epiphany
            new DateTime(2026, 12, 25, 7, 0, 0)  // Christmas
        };

        var result = _calculator.CalculateFee(passages);
        Assert.Equal(0, result);
    }

    [Fact]
    public void CalculateFee_DailyCapExactly60_Returns60()
    {
        var passages = new[]
        {
            new DateTime(2026, 1, 2, 7, 0, 0),  // 18 SEK
            new DateTime(2026, 1, 2, 8, 30, 0), // 8 SEK
            new DateTime(2026, 1, 2, 10, 0, 0), // 8 SEK
            new DateTime(2026, 1, 2, 11, 30, 0), // 8 SEK
            new DateTime(2026, 1, 2, 15, 30, 0)  // 18 SEK
        };
        // Total: 18 + 8 + 8 + 8 + 18 = 60

        var result = _calculator.CalculateFee(passages);
        Assert.Equal(60, result);
    }

    [Fact]
    public void CalculateFee_WindowWithLowerThenHigherFee_ChargesHigher()
    {
        var passages = new[]
        {
            new DateTime(2026, 1, 2, 6, 0, 0),  // 8 SEK
            new DateTime(2026, 1, 2, 6, 30, 0)  // 13 SEK (higher)
        };

        var result = _calculator.CalculateFee(passages);
        Assert.Equal(13, result);
    }

    [Fact]
    public void CalculateFee_WindowWithHigherThenLowerFee_ChargesHigher()
    {
        var passages = new[]
        {
            new DateTime(2026, 1, 2, 6, 30, 0), // 13 SEK (higher)
            new DateTime(2026, 1, 2, 6, 50, 0)  // 13 SEK
        };

        var result = _calculator.CalculateFee(passages);
        Assert.Equal(13, result);
    }
}