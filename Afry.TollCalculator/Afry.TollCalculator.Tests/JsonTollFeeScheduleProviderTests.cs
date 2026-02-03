using Microsoft.Extensions.Options;
using Afry.TollCalculator.Services;
using Afry.TollCalculator.Infrastructure.Settings;

namespace Afry.TollCalculator.Tests;

public sealed class JsonTollFeeScheduleProviderTests
{
    [Fact]
    public void GetSchedule_ValidSettings_ReturnsSuccessAndSortedRanges()
    {
        // Arrange
        var settings = TollSettings.Load();
        var options = Options.Create(settings);
        var provider = new JsonTollFeeScheduleProvider(options);

        // Act
        var result = provider.GetSchedule();

        // Assert
        Assert.True(result.IsSuccess);
        Assert.NotNull(result.Value);
        Assert.True(result.Value!.Count > 0);

        for (var i = 1; i < result.Value.Count; i++)
        {
            Assert.True(result.Value[i].From > result.Value[i - 1].From);
        }
    }

    [Fact]
    public void Ctor_InvalidFeeBounds_Throws()
    {
        // Arrange
        var settings = TollSettings.Load();
        settings.FeeMinMinorUnit = 2000;
        settings.FeeMaxMinorUnit = 1000;

        // Act + Assert
        Assert.Throws<InvalidOperationException>(() => _ = new JsonTollFeeScheduleProvider(Options.Create(settings)));
    }
}
