using Afry.TollCalculator.Models;

namespace Afry.TollCalculator.Tests;

public sealed class TollFeeTimeRangeTests
{
    [Fact]
    public void Contains_IsInclusive()
    {
        // Arrange
        var range = new TollFeeTimeRange(new TimeOnly(6, 0), new TimeOnly(6, 29), new Money(800, "SEK"));

        // Act
        var atStart = range.Contains(new TimeOnly(6, 0));
        var inMiddle = range.Contains(new TimeOnly(6, 15));
        var atEnd = range.Contains(new TimeOnly(6, 29));
        var after = range.Contains(new TimeOnly(6, 30));

        // Assert
        Assert.True(atStart);
        Assert.True(inMiddle);
        Assert.True(atEnd);
        Assert.False(after);
    }
}
