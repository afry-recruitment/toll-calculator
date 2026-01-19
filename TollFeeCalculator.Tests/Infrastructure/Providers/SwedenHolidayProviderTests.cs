using TollFeeCalculator.Infrastructure.Providers;

namespace TollFeeCalculator.Tests.Infrastructure.Providers;

[TestClass]
public sealed class SwedenHolidayProviderTests
{
    [TestMethod]
    public async Task GetHolidaysAsync_ReturnsByYearSet()
    {
        var provider = new SwedenHolidayProvider();

        var holidays = await provider.GetHolidaysAsync(2026, CancellationToken.None);

        Assert.IsTrue(holidays.Count > 0);
        Assert.IsTrue(holidays.All(d => d.Year == 2026));
    }

    [TestMethod]
    public async Task GetHolidaysAsync_WhenCancelled_Throws()
    {
        var provider = new SwedenHolidayProvider();

        using var cts = new CancellationTokenSource();
        cts.Cancel();

        await Assert.ThrowsExceptionAsync<OperationCanceledException>(() => provider.GetHolidaysAsync(2026, cts.Token));
    }
}